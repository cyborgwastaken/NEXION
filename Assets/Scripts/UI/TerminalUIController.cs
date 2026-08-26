using UnityEngine;
using UnityEngine.UIElements;
using Nexion.Player;
using Nexion.Interaction;
using Nexion.Puzzles;

namespace Nexion.UI
{
    /// Single shared terminal UI, opened by whichever TerminalPuzzle the player
    /// interacts with. Locks player movement/look/interaction while open.
    [RequireComponent(typeof(UIDocument))]
    public class TerminalUIController : MonoBehaviour
    {
        public static TerminalUIController Instance { get; private set; }

        [SerializeField] private PlayerController playerController;
        [SerializeField] private PlayerInteractor playerInteractor;

        private VisualElement root;
        private Label titleLabel;
        private Label logLabel;
        private ScrollView logScroll;
        private TextField inputField;

        private TerminalPuzzle activePuzzle;

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }
            Instance = this;

            var document = GetComponent<UIDocument>();
            root = document.rootVisualElement.Q<VisualElement>("TerminalRoot");
            titleLabel = document.rootVisualElement.Q<Label>("TitleLabel");
            logLabel = document.rootVisualElement.Q<Label>("LogLabel");
            logScroll = document.rootVisualElement.Q<ScrollView>("LogScroll");
            inputField = document.rootVisualElement.Q<TextField>("CommandInput");

            inputField.RegisterCallback<KeyDownEvent>(OnInputKeyDown, TrickleDown.TrickleDown);

            Close();
        }

        public void Open(TerminalPuzzle puzzle)
        {
            activePuzzle = puzzle;
            root.style.display = DisplayStyle.Flex;
            titleLabel.text = puzzle.TerminalTitle;
            logLabel.text = puzzle.IntroText;
            inputField.value = string.Empty;
            inputField.Focus();

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

        private void OnInputKeyDown(KeyDownEvent evt)
        {
            if (evt.keyCode == KeyCode.Escape)
            {
                Close();
                evt.StopPropagation();
                return;
            }

            if (evt.keyCode != KeyCode.Return && evt.keyCode != KeyCode.KeypadEnter)
                return;

            evt.StopPropagation();
            SubmitCommand();
        }

        private void SubmitCommand()
        {
            if (activePuzzle == null) return;

            string raw = inputField.value;
            if (string.IsNullOrWhiteSpace(raw))
            {
                inputField.value = string.Empty;
                return;
            }

            AppendLog($"> {raw}");
            TerminalPuzzle puzzle = activePuzzle;
            string response = puzzle.ProcessCommand(raw);

            if (activePuzzle == null) return; // command closed the terminal (e.g. "exit")

            if (!string.IsNullOrEmpty(response))
                AppendLog(response);

            inputField.value = string.Empty;
            inputField.Focus();

            if (puzzle.IsSolved)
                Invoke(nameof(Close), 1.2f);
        }

        private void AppendLog(string line)
        {
            logLabel.text += "\n" + line;
            logScroll.schedule.Execute(() => logScroll.scrollOffset = new Vector2(0, float.MaxValue));
        }
    }
}
