using UnityEngine;
using UnityEngine.Events;
using Nexion.Core;
using Nexion.Interaction;
using Nexion.UI;

namespace Nexion.Puzzles
{
    /// Numeric keypad puzzle: player enters a code via KeypadUIController.
    /// Distinct from TerminalPuzzle's free-text commands — this is a fixed-length
    /// digit code, matching the doc's "environmental code puzzle" framing (a physical
    /// lock, not a hacking console). Same C-MODE gate pattern as TerminalPuzzle.
    ///
    /// The doc's hybrid-puzzle example (C-MODE reveals a LOCK_ID, H-MODE recalls what
    /// it means) isn't wired up yet — `code` is a plain Inspector field until the
    /// Memory Fragment system exists to feed it a real recovered code.
    public class KeypadPuzzle : MonoBehaviour, IInteractable
    {
        [Header("Keypad")]
        [SerializeField] private string keypadTitle = "SECURITY LOCK";
        [SerializeField] private string code = "1234";
        [SerializeField] private bool requireCpuMode = true;

        public UnityEvent onSolved;
        public UnityEvent onFailed;

        public bool IsSolved { get; private set; }
        public int CodeLength => code.Length;
        public string KeypadTitle => keypadTitle;

        public string InteractionPrompt => IsSolved ? "Lock (bypassed)" : "Use Keypad";
        public bool CanInteract => true;

        public void Interact(GameObject interactor)
        {
            if (requireCpuMode && (ModeController.Instance == null || !ModeController.Instance.IsCPU))
            {
                Debug.Log($"[KeypadPuzzle] {name}: keypad signal unreadable — enter C-MODE to access it.", this);
                return;
            }

            if (KeypadUIController.Instance != null)
                KeypadUIController.Instance.Open(this);
        }

        public bool TrySubmit(string entered)
        {
            if (IsSolved) return true;

            if (entered == code)
            {
                IsSolved = true;
                onSolved?.Invoke();
                return true;
            }

            onFailed?.Invoke();
            return false;
        }
    }
}
