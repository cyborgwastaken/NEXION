using UnityEngine;

namespace Nexion.Interaction
{
    /// Throwaway test object: put this on any collider to verify PlayerInteractor
    /// and the raycast/prompt flow work before building the real terminal, keypad,
    /// and memory fragment systems on top of IInteractable.
    public class DebugInteractable : MonoBehaviour, IInteractable
    {
        [SerializeField] private string prompt = "Interact";
        [SerializeField] private string message = "Debug interactable triggered.";

        public string InteractionPrompt => prompt;
        public bool CanInteract => true;

        public void Interact(GameObject interactor)
        {
            Debug.Log($"[DebugInteractable] {name}: {message}", this);
        }
    }
}
