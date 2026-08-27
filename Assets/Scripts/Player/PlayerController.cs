using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Nexion.Core;

namespace Nexion.Player
{
    /// First-person movement + look. Reads input via InputManager (rebindable Input
    /// Actions) — see Assets/Settings/Input/NexionControls.inputactions for the actual
    /// key/button bindings, not this script. In C-MODE, movement input is delayed to
    /// match the design spec's "physical precision degraded (0.3s input lag)" limitation.
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4.5f;
        [SerializeField] private float sprintSpeed = 7.5f;
        [SerializeField] private float gravity = -18f;
        [SerializeField] private float jumpHeight = 1.2f;

        [Header("Look")]
        [SerializeField] private Transform cameraPivot;
        [SerializeField] private float lookSensitivity = 0.12f;
        [SerializeField] private float maxLookAngle = 85f;

        [Header("C-MODE Penalty")]
        [SerializeField] private float cpuModeInputLag = 0.3f;

        private CharacterController controller;
        private float pitch;
        private float verticalVelocity;
        private Vector2 bufferedMove;
        private readonly Queue<(float time, Vector2 move)> inputHistory = new();

        private void Awake()
        {
            controller = GetComponent<CharacterController>();
        }

        private void Start()
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }

        private void Update()
        {
            HandleCursorToggle();
            HandleLook();
            HandleMove();
        }

        private void HandleCursorToggle()
        {
            // Pause/cursor-unlock convention — deliberately left off the rebindable
            // action set, same treatment as the terminal/keypad UI's Escape handling.
            if (Keyboard.current != null && Keyboard.current.escapeKey.wasPressedThisFrame)
            {
                bool locked = Cursor.lockState == CursorLockMode.Locked;
                Cursor.lockState = locked ? CursorLockMode.None : CursorLockMode.Locked;
                Cursor.visible = locked;
            }
        }

        private void HandleLook()
        {
            if (Cursor.lockState != CursorLockMode.Locked) return;

            Vector2 lookDelta = InputManager.Instance != null ? InputManager.Instance.LookDelta : Vector2.zero;
            transform.Rotate(Vector3.up * lookDelta.x * lookSensitivity);

            pitch = Mathf.Clamp(pitch - lookDelta.y * lookSensitivity, -maxLookAngle, maxLookAngle);
            if (cameraPivot != null)
                cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void HandleMove()
        {
            Vector2 rawInput = InputManager.Instance != null ? InputManager.Instance.MoveInput : Vector2.zero;

            bool cpuMode = ModeController.Instance != null && ModeController.Instance.IsCPU;
            Vector2 moveInput = cpuMode ? GetLaggedInput(rawInput) : rawInput;

            bool sprinting = InputManager.Instance != null && InputManager.Instance.SprintHeld;
            float speed = sprinting ? sprintSpeed : walkSpeed;

            Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * speed;

            if (controller.isGrounded)
            {
                verticalVelocity = -1f;
                if (InputManager.Instance != null && InputManager.Instance.JumpPressed)
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            verticalVelocity += gravity * Time.deltaTime;

            controller.Move((move + Vector3.up * verticalVelocity) * Time.deltaTime);
        }

        private Vector2 GetLaggedInput(Vector2 currentInput)
        {
            inputHistory.Enqueue((Time.time, currentInput));
            while (inputHistory.Count > 0 && Time.time - inputHistory.Peek().time >= cpuModeInputLag)
                bufferedMove = inputHistory.Dequeue().move;
            return bufferedMove;
        }
    }
}
