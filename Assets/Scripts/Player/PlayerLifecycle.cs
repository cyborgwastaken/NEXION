using UnityEngine;
using AFPC;

namespace Nexion.Player
{
    /// Owns an AFPC Lifecycle (health/shield/damage/death/respawn) for the player,
    /// without pulling in AFPC's Hero/Movement — those aren't used here (see
    /// PlayerController for the actual CharacterController-based movement).
    /// On death, freezes movement/look by disabling PlayerController; respawn
    /// re-enables it. There is no checkpoint system yet, so Respawn() only
    /// restores health/shield/availability — it does not reposition the player.
    [RequireComponent(typeof(PlayerController))]
    public class PlayerLifecycle : MonoBehaviour
    {
        [Tooltip("Health/shield/damage/death/respawn state. See AFPC.Lifecycle for the full API.")]
        public Lifecycle lifecycle = new();

        [Header("Debug (R/H/T) — compiled out of release builds")]
        [SerializeField] private float debugDamageAmount = 50f;
        [SerializeField] private float debugHealAmount = 50f;

        private PlayerController playerController;

        private void Awake()
        {
            playerController = GetComponent<PlayerController>();
        }

        private void Start()
        {
            lifecycle.Initialize();
            lifecycle.onDeath.AddListener(HandleDeath);
            lifecycle.onRespawn.AddListener(HandleRespawn);

            // Initialize() sets health/shield to max but doesn't fire onHealthChanged/
            // onShieldChanged — broadcast once so listeners (e.g. PlayerHUDBinder,
            // subscribed during its own OnEnable which always runs before this Start)
            // get the real starting values instead of Lifecycle's raw pre-init defaults.
            lifecycle.onHealthChanged?.Invoke(lifecycle.GetHealthValue());
            lifecycle.onShieldChanged?.Invoke(lifecycle.GetShieldValue());
        }

        private void Update()
        {
            lifecycle.Runtime();

#if UNITY_EDITOR || DEVELOPMENT_BUILD
            var input = Nexion.Core.InputManager.Instance;
            if (input == null) return;
            if (input.DamagePressed) lifecycle.Damage(debugDamageAmount);
            if (input.HealPressed) lifecycle.Heal(debugHealAmount);
            if (input.RespawnPressed) lifecycle.Respawn();
#endif
        }

        private void HandleDeath()
        {
            if (playerController != null) playerController.enabled = false;
        }

        private void HandleRespawn()
        {
            if (playerController != null) playerController.enabled = true;
        }
    }
}
