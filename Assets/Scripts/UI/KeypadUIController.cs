using System.Text;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEngine.InputSystem;
using Nexion.Player;
using Nexion.Interaction;
using Nexion.Puzzles;

namespace Nexion.UI
{
    /// Single shared keypad UI, opened by whichever KeypadPuzzle the player
    /// interacts with. Digits can be entered by clicking the on-screen buttons or
    /// typing the keyboard's digit/numpad keys. Locks player movement/look/
    /// interaction while open, same as TerminalUIController.
    [RequireComponent(typeof(UIDocument))]
    public class KeypadUIController : MonoBehaviour
    {
        public static KeypadUIController Instance { get; private set; }

        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerInteractor playerInteractor;

        private static readonly Key[] DigitKeys =
        {
            Key.Digit0, Key.Digit1, Key.Digit2, Key.Digit3, Key.Digit4,
            Key.Digit5, Key.Digit6, Key.Digit7, Key.Digit8, Key.Digit9
        };

        private static readonly Key[] NumpadKeys =
        {
            Key.Numpad0, Key.Numpad1, Key.Numpad2, Key.Numpad3, Key.Numpad4,
            Key.Numpad5, Key.Numpad6, Key.Numpad7, Key.Numpad8, Key.Numpad9
        };

        private UIDocument document;
        private VisualElement root;
        private Label titleLabel;
        private Label displayLabel;
        private Label statusLabel;

        private KeypadPuzzle activePuzzle;
        private readonly StringBuilder buffer = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            document = GetComponent<UIDocument>();
            root = document.rootVisualElement.Q<VisualElement>("KeypadRoot");
            titleLabel = document.rootVisualElement.Q<Label>("TitleLabel");
            displayLabel = document.rootVisualElement.Q<Label>("DisplayLabel");
            statusLabel = document.rootVisualElement.Q<Label>("StatusLabel");

            for (int d = 0; d <= 9; d++)
            {
                string digit = d.ToString();
                RegisterButton($"Btn{digit}", () => AppendDigit(digit));
            }
            RegisterButton("BtnClear", OnClear);
            RegisterButton("BtnEnter", OnEnter);

            Close();
        }

        private void RegisterButton(string elementName, System.Action onClick)
        {
            var button = document.rootVisualElement.Q<Button>(elementName);
            if (button != null)
                button.clicked += onClick;
        }

        public void Open(KeypadPuzzle puzzle)
        {
            activePuzzle = puzzle;
            buffer.Clear();
            root.style.display = DisplayStyle.Flex;
            titleLabel.text = puzzle.KeypadTitle;
            statusLabel.text = string.Empty;
            UpdateDisplay();

            if (playerController != null) playerController.enabled = false;
            if (playerInteractor != null) playerInteractor.enabled = false;
            UnityEngine.Cursor.lockState = CursorLockMode.None;
            UnityEngine.Cursor.visible = true;
        }

        public void Close()
        {
            activePuzzle = null;
            if (root != null)
                root.style.display = DisplayStyle.None;

            if (playerController != null) playerController.enabled = true;
            if (playerInteractor != null) playerInteractor.enabled = true;
            UnityEngine.Cursor.lockState = CursorLockMode.Locked;
            UnityEngine.Cursor.visible = false;
        }

        private void Update()
        {
            if (activePuzzle == null) return;

            var kb = Keyboard.current;
            if (kb == null) return;

            if (kb.escapeKey.wasPressedThisFrame)
            {
                Close();
                return;
            }

            if (kb.backspaceKey.wasPressedThisFrame)
                OnBackspace();

            if (kb.enterKey.wasPressedThisFrame || kb.numpadEnterKey.wasPressedThisFrame)
                OnEnter();

            for (int d = 0; d <= 9; d++)
            {
                if (kb[DigitKeys[d]].wasPressedThisFrame || kb[NumpadKeys[d]].wasPressedThisFrame)
                    AppendDigit(d.ToString());
            }
        }

        private void AppendDigit(string digit)
        {
            if (activePuzzle == null || activePuzzle.IsSolved) return;
            if (buffer.Length >= activePuzzle.CodeLength) return;

            buffer.Append(digit);
            statusLabel.text = string.Empty;
            UpdateDisplay();
        }

        private void OnBackspace()
        {
            if (buffer.Length > 0)
                buffer.Length -= 1;
            UpdateDisplay();
        }

        private void OnClear()
        {
            buffer.Clear();
            UpdateDisplay();
        }

        private void OnEnter()
        {
            if (activePuzzle == null || activePuzzle.IsSolved || buffer.Length == 0) return;

            bool success = activePuzzle.TrySubmit(buffer.ToString());
            if (success)
            {
                statusLabel.text = "ACCESS GRANTED";
                Invoke(nameof(Close), 1.0f);
            }
            else
            {
                statusLabel.text = "ACCESS DENIED";
                buffer.Clear();
                UpdateDisplay();
            }
        }

        private void UpdateDisplay()
        {
            int length = activePuzzle != null ? activePuzzle.CodeLength : 4;
            displayLabel.text = buffer.ToString().PadRight(length, '_');
        }
    }
}
