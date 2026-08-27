using UnityEngine;
using Nexion.Core;

namespace Nexion.Interaction
{
    /// Raycasts from the camera each frame to find an IInteractable and fires it on
    /// the rebindable Interact action (see InputManager / NexionControls.inputactions
    /// — default binding is [F] / gamepad West). Put this on the Player (or its
    /// camera) and give it the camera's transform as the interaction origin.
    public class PlayerInteractor : MonoBehaviour
    {
        [SerializeField] private Transform interactionOrigin;
        [SerializeField] private float interactionRange = 3f;
        [SerializeField] private LayerMask interactableMask = ~0;

        public IInteractable CurrentTarget { get; private set; }

        private void Update()
        {
            CurrentTarget = FindTarget();

            bool interactPressed = InputManager.Instance != null && InputManager.Instance.InteractPressed;

            if (interactPressed && CurrentTarget != null && CurrentTarget.CanInteract)
                CurrentTarget.Interact(gameObject);
        }

        private IInteractable FindTarget()
        {
            Transform origin = interactionOrigin != null ? interactionOrigin : transform;
            if (Physics.Raycast(origin.position, origin.forward, out RaycastHit hit, interactionRange, interactableMask))
                return hit.collider.GetComponentInParent<IInteractable>();
            return null;
        }
    }
}
