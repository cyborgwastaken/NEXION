using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Nexion.Core;

namespace Nexion.Player
{
    /// First-person movement + look. Reads devices directly (no .inputactions asset
    /// required). In C-MODE, movement input is delayed to match the design spec's
    /// "physical precision degraded (0.3s input lag)" limitation of that mode.
    [RequireComponent(typeof(CharacterController))]
    public class PlayerController : MonoBehaviour
    {
        [Header("Movement")]
        [SerializeField] private float walkSpeed = 4.5f;
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

            Vector2 lookDelta = Mouse.current?.delta.ReadValue() ?? Vector2.zero;
            transform.Rotate(Vector3.up * lookDelta.x * lookSensitivity);

            pitch = Mathf.Clamp(pitch - lookDelta.y * lookSensitivity, -maxLookAngle, maxLookAngle);
            if (cameraPivot != null)
                cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }

        private void HandleMove()
        {
            Vector2 rawInput = ReadMoveInput();

            bool cpuMode = ModeController.Instance != null && ModeController.Instance.IsCPU;
            Vector2 moveInput = cpuMode ? GetLaggedInput(rawInput) : rawInput;

            Vector3 move = (transform.right * moveInput.x + transform.forward * moveInput.y) * walkSpeed;

            if (controller.isGrounded)
            {
                verticalVelocity = -1f;
                if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
                    verticalVelocity = Mathf.Sqrt(jumpHeight * -2f * gravity);
            }
            verticalVelocity += gravity * Time.deltaTime;

            controller.Move((move + Vector3.up * verticalVelocity) * Time.deltaTime);
        }

        private Vector2 ReadMoveInput()
        {
            Vector2 input = Vector2.zero;
            var kb = Keyboard.current;
            if (kb != null)
            {
                if (kb.wKey.isPressed) input.y += 1;
                if (kb.sKey.isPressed) input.y -= 1;
                if (kb.dKey.isPressed) input.x += 1;
                if (kb.aKey.isPressed) input.x -= 1;
            }

            var pad = Gamepad.current;
            if (pad != null)
                input += pad.leftStick.ReadValue();

            return Vector2.ClampMagnitude(input, 1f);
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
