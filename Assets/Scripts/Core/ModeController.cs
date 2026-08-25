using System;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Nexion.Core
{
    /// Central authority for NEX//ION's dual-mode mechanic (H-MODE / C-MODE).
    /// One instance lives in the scene. Everything else (visuals, audio, player
    /// controller, puzzle systems) reads CurrentMode or subscribes to OnModeChanged.
    public class ModeController : MonoBehaviour
    {
        public static ModeController Instance { get; private set; }

        [Header("Mode Switch Timing")]
        [Tooltip("Delay before a mode switch actually applies. Design spec: 0.5s. " +
                 "The 'Dual Process' Interface Tree upgrade should set this to 0 at runtime.")]
        [SerializeField] private float transitionDelay = 0.5f;

        public GameMode CurrentMode { get; private set; } = GameMode.Neutral;
        public bool IsTransitioning { get; private set; }
        public bool IsHuman => CurrentMode == GameMode.Human;
        public bool IsCPU => CurrentMode == GameMode.CPU;
        public bool IsNeutral => CurrentMode == GameMode.Neutral;

        /// Fires once the switch completes and CurrentMode has actually changed.
        public event Action<GameMode> OnModeChanged;

        private GameMode pendingMode;
        private float transitionTimer;

        public void SetTransitionDelay(float seconds) => transitionDelay = Mathf.Max(0f, seconds);

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;
            pendingMode = CurrentMode;
        }

        private void Update()
        {
            GameMode requested = ReadRequestedMode();

            if (requested != CurrentMode && requested != pendingMode)
            {
                pendingMode = requested;
                transitionTimer = transitionDelay;
                IsTransitioning = transitionDelay > 0f;

                if (!IsTransitioning)
                    ApplyPendingMode();
            }
            else if (IsTransitioning)
            {
                transitionTimer -= Time.deltaTime;
                if (transitionTimer <= 0f)
                    ApplyPendingMode();
            }
        }

        private void ApplyPendingMode()
        {
            IsTransitioning = false;
            CurrentMode = pendingMode;
            OnModeChanged?.Invoke(CurrentMode);
        }

        private GameMode ReadRequestedMode()
        {
            bool humanHeld = (Keyboard.current?.qKey.isPressed ?? false)
                || (Gamepad.current?.leftTrigger.isPressed ?? false);
            bool cpuHeld = (Keyboard.current?.eKey.isPressed ?? false)
                || (Gamepad.current?.rightTrigger.isPressed ?? false);

            if (humanHeld && !cpuHeld) return GameMode.Human;
            if (cpuHeld && !humanHeld) return GameMode.CPU;
            return GameMode.Neutral;
        }
    }
}
