using UnityEngine;

namespace Nexion.Player
{
    /// Makes Asuna (the first-person arms model) visibly hold a weapon. Deliberately
    /// just an equip — no firing/reload/aim-down-sights, since no combat design exists
    /// yet for NEXION. Spawns weaponPrefab under weaponSocket and plays a hold-idle
    /// state on the viewmodel's Animator (FreeTestAnimationController ships a "Pistol"/
    /// "Rifle" hold-pose state named for exactly this).
    public class PlayerViewmodel : MonoBehaviour
    {
        [Tooltip("Animator on the Asuna viewmodel instance (uses FreeTestAnimationController).")]
        [SerializeField] private Animator viewmodelAnimator;

        [Tooltip("Hand-bone transform to parent the weapon under. Find this on the rig once " +
                 "the viewmodel is in the scene — bone names aren't knowable from disk alone.")]
        [SerializeField] private Transform weaponSocket;

        [Tooltip("Weapon prefab to equip, e.g. ScifiPistolMNL21MasterPrefab.")]
        [SerializeField] private GameObject weaponPrefab;

        [Tooltip("Animator state to play once equipped (must exist on viewmodelAnimator's controller).")]
        [SerializeField] private string holdStateName = "Pistol";

        private GameObject equippedWeapon;

        private void Start()
        {
            Equip(weaponPrefab);
        }

        /// Swaps the currently held weapon. Safe to call at runtime once a weapon-select
        /// system exists; for now only called once from Start with the Inspector-assigned prefab.
        public void Equip(GameObject prefab)
        {
            if (equippedWeapon != null)
                Destroy(equippedWeapon);

            weaponPrefab = prefab;
            if (weaponPrefab == null || weaponSocket == null) return;

            equippedWeapon = Instantiate(weaponPrefab, weaponSocket);
            equippedWeapon.transform.localPosition = Vector3.zero;
            equippedWeapon.transform.localRotation = Quaternion.identity;

            if (viewmodelAnimator != null && !string.IsNullOrEmpty(holdStateName))
                viewmodelAnimator.Play(holdStateName);
        }
    }
}
