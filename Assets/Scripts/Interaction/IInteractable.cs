using UnityEngine;

namespace Nexion.Interaction
{
    /// Implemented by anything the player can interact with: terminals, keypads,
    /// memory fragments, NPCs, Sable's shop counter, etc.
    public interface IInteractable
    {
        string InteractionPrompt { get; }
        bool CanInteract { get; }
        void Interact(GameObject interactor);
    }
}
