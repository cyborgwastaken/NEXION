using UnityEngine;
using AFPC;
using Nexion.Player;

namespace Nexion.UI
{
    /// Glue between PlayerLifecycle and the (Nexion-agnostic) AFPCUI view — AFPC's
    /// assembly can't reference Nexion's, so this side owns the wiring instead.
    /// Put on the AFPCUI GameObject (or anywhere else with a reference to both);
    /// see docs/guide.md Session 5.
    public class PlayerHUDBinder : MonoBehaviour
    {
        [SerializeField] private AFPCUI hud;
        [SerializeField] private PlayerLifecycle playerLifecycle;

        private void OnEnable()
        {
            if (playerLifecycle == null) return;
            var lifecycle = playerLifecycle.lifecycle;
            lifecycle.onHealthChanged.AddListener(OnHealthChanged);
            lifecycle.onShieldChanged.AddListener(OnShieldChanged);
            lifecycle.onDamage.AddListener(OnDamage);

            // No manual initial push here — PlayerLifecycle.Start() broadcasts the real
            // starting values once Lifecycle.Initialize() has run, and OnEnable (this
            // method) is always subscribed before any Start() runs, so that broadcast
            // is guaranteed to reach us. Reading lifecycle.GetHealthValue() here instead
            // would race Initialize() and could show 1/100 instead of 100/100.
        }

        private void OnDisable()
        {
            if (playerLifecycle == null) return;
            var lifecycle = playerLifecycle.lifecycle;
            lifecycle.onHealthChanged.RemoveListener(OnHealthChanged);
            lifecycle.onShieldChanged.RemoveListener(OnShieldChanged);
            lifecycle.onDamage.RemoveListener(OnDamage);
        }

        private void OnHealthChanged(float current)
        {
            if (hud != null) hud.SetHealth(current, playerLifecycle.lifecycle.referenceHealth);
        }

        private void OnShieldChanged(float current)
        {
            if (hud != null) hud.SetShield(current, playerLifecycle.lifecycle.referenceShield);
        }

        private void OnDamage(float _)
        {
            if (hud != null) hud.DamageFX();
        }
    }
}
