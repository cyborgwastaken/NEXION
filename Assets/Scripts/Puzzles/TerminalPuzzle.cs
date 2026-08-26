using UnityEngine;
using UnityEngine.Events;
using Nexion.Core;
using Nexion.Interaction;
using Nexion.UI;

namespace Nexion.Puzzles
{
    /// Terminal hacking puzzle: player types commands into TerminalUIController to
    /// bypass a firewall. Per the design doc, terminals read as noise in H-MODE —
    /// Require Cpu Mode gates the interaction behind C-MODE (on by default).
    public class TerminalPuzzle : MonoBehaviour, IInteractable
    {
        [Header("Terminal")]
        [SerializeField] private string terminalTitle = "AXIOM SECURE TERMINAL";
        [SerializeField] private string introText = "FIREWALL ACTIVE. Type 'scan' to begin.";
        [SerializeField] private string hintText = "Firewall requires an access code. Try: bypass <code>";
        [SerializeField] private string accessCode = "7731";
        [SerializeField] private bool requireCpuMode = true;

        public UnityEvent onSolved;

        public bool IsSolved { get; private set; }
        public string TerminalTitle => terminalTitle;
        public string IntroText => introText;

        public string InteractionPrompt => IsSolved ? "Terminal (bypassed)" : "Access Terminal";
        public bool CanInteract => true;

        public void Interact(GameObject interactor)
        {
            if (requireCpuMode && (ModeController.Instance == null || !ModeController.Instance.IsCPU))
            {
                Debug.Log($"[TerminalPuzzle] {name}: signal unreadable — enter C-MODE to access this terminal.", this);
                return;
            }

            if (TerminalUIController.Instance != null)
                TerminalUIController.Instance.Open(this);
        }

        public string ProcessCommand(string raw)
        {
            if (IsSolved)
                return "ACCESS ALREADY GRANTED.";

            string trimmed = (raw ?? string.Empty).Trim();
            string command = trimmed;
            string arg = string.Empty;
            int spaceIndex = trimmed.IndexOf(' ');
            if (spaceIndex > 0)
            {
                command = trimmed.Substring(0, spaceIndex);
                arg = trimmed.Substring(spaceIndex + 1).Trim();
            }

            switch (command.ToLowerInvariant())
            {
                case "help":
                    return "COMMANDS: scan | bypass <code> | exit";
                case "scan":
                    return hintText;
                case "bypass":
                    if (arg == accessCode)
                    {
                        IsSolved = true;
                        onSolved?.Invoke();
                        return "ACCESS GRANTED. Firewall bypassed.";
                    }
                    return "ACCESS DENIED.";
                case "exit":
                    TerminalUIController.Instance?.Close();
                    return string.Empty;
                default:
                    return string.IsNullOrEmpty(command) ? string.Empty : $"UNKNOWN COMMAND: {command}";
            }
        }
    }
}
