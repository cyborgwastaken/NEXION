using UnityEngine;
using UnityEngine.InputSystem;

namespace Nexion.Core
{
    /// Single point of contact for all rebindable player input. Wraps the
    /// NexionControls InputActionAsset (Assets/Settings/Input/NexionControls.inputactions)
    /// instead of scripts polling Keyboard/Gamepad directly, so a future settings menu
    /// can rebind any action via the Input System's own rebinding API
    /// (InputActionRebindingExtensions.PerformInteractiveRebinding) without touching
    /// any gameplay code. SaveBindings/LoadBindings persist overrides across sessions.
    ///
    /// Deliberately NOT covered here: Escape (pause/cursor-unlock) and the terminal/
    /// keypad UI screens' own key handling (digits, Enter, Backspace) — those are
    /// UI/text-entry conventions, not gameplay actions players expect to rebind.
    public class InputManager : MonoBehaviour
    {
        public static InputManager Instance { get; private set; }

        private const string BindingOverridesKey = "Nexion.InputBindingOverrides";

        [SerializeField] private InputActionAsset inputActions;

        [Header("Gamepad Look")]
        [Tooltip("Mouse delta (<Pointer>/delta) is already a per-frame pixel offset, but " +
                 "a gamepad stick reports a normalized -1..1 rate, not a delta. Reading both " +
                 "through the same lookSensitivity scalar makes stick-look nearly motionless, " +
                 "so stick input is scaled by this (deg/sec-ish) and Time.deltaTime instead.")]
        [SerializeField] private float gamepadLookSpeed = 180f;

        private InputAction moveAction;
        private InputAction lookAction;
        private InputAction jumpAction;
        private InputAction sprintAction;
        private InputAction interactAction;
        private InputAction modeHumanAction;
        private InputAction modeCpuAction;
        private InputAction damageAction;
        private InputAction healAction;
        private InputAction respawnAction;

        public Vector2 MoveInput => moveAction?.ReadValue<Vector2>() ?? Vector2.zero;

        public Vector2 LookDelta
        {
            get
            {
                if (lookAction == null) return Vector2.zero;
                Vector2 raw = lookAction.ReadValue<Vector2>();
                if (lookAction.activeControl?.device is Gamepad)
                    return raw * gamepadLookSpeed * Time.deltaTime;
                return raw;
            }
        }

        public bool JumpPressed => jumpAction?.WasPressedThisFrame() ?? false;
        public bool SprintHeld => sprintAction?.IsPressed() ?? false;
        public bool InteractPressed => interactAction?.WasPressedThisFrame() ?? false;
        public bool HumanHeld => modeHumanAction?.IsPressed() ?? false;
        public bool CpuHeld => modeCpuAction?.IsPressed() ?? false;

        /// Debug/test-only actions (R/H/T) — see PlayerLifecycle, which is the only
        /// consumer and compiles its usage out of release builds.
        public bool DamagePressed => damageAction?.WasPressedThisFrame() ?? false;
        public bool HealPressed => healAction?.WasPressedThisFrame() ?? false;
        public bool RespawnPressed => respawnAction?.WasPressedThisFrame() ?? false;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            if (inputActions == null)
            {
                Debug.LogError("[InputManager] No Input Action Asset assigned.", this);
                return;
            }

            InputActionMap player = inputActions.FindActionMap("Player", throwIfNotFound: true);
            moveAction = player.FindAction("Move");
            lookAction = player.FindAction("Look");
            jumpAction = player.FindAction("Jump");
            sprintAction = player.FindAction("Sprint");
            interactAction = player.FindAction("Interact");
            modeHumanAction = player.FindAction("ModeHuman");
            modeCpuAction = player.FindAction("ModeCPU");
            damageAction = player.FindAction("Damage");
            healAction = player.FindAction("Heal");
            respawnAction = player.FindAction("Respawn");

            LoadBindings();
        }

        private void OnEnable() => inputActions?.Enable();
        private void OnDisable() => inputActions?.Disable();

        /// Call after a rebind UI changes a binding, to persist it across sessions.
        public void SaveBindings()
        {
            if (inputActions == null) return;
            PlayerPrefs.SetString(BindingOverridesKey, inputActions.SaveBindingOverridesAsJson());
            PlayerPrefs.Save();
        }

        public void ResetBindings()
        {
            inputActions?.RemoveAllBindingOverrides();
            PlayerPrefs.DeleteKey(BindingOverridesKey);
        }

        private void LoadBindings()
        {
            if (inputActions == null) return;
            string json = PlayerPrefs.GetString(BindingOverridesKey, string.Empty);
            if (!string.IsNullOrEmpty(json))
                inputActions.LoadBindingOverridesFromJson(json);
        }
    }
}
