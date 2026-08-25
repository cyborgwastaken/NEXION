using UnityEngine;
using UnityEngine.Rendering;
using Nexion.Core;

namespace Nexion.Systems
{
    /// Crossfades two post-processing Volumes (H-MODE warm/amber, C-MODE cyan/data)
    /// based on ModeController.CurrentMode. The Volume Profiles themselves (Vignette,
    /// Color Adjustments, Chromatic Aberration, etc.) are authored in the Editor per
    /// the visual language in the design doc — this script only drives their weight.
    public class ModeVisualController : MonoBehaviour
    {
        [SerializeField] private Volume humanVolume;
        [SerializeField] private Volume cpuVolume;
        [SerializeField] private float blendSpeed = 3f;

        private float humanTarget;
        private float cpuTarget;

        private void Reset()
        {
            if (humanVolume != null) humanVolume.weight = 0f;
            if (cpuVolume != null) cpuVolume.weight = 0f;
        }

        private void Start()
        {
            if (ModeController.Instance != null)
            {
                ModeController.Instance.OnModeChanged += HandleModeChanged;
                HandleModeChanged(ModeController.Instance.CurrentMode);
            }
        }

        private void OnDestroy()
        {
            if (ModeController.Instance != null)
                ModeController.Instance.OnModeChanged -= HandleModeChanged;
        }

        private void HandleModeChanged(GameMode mode)
        {
            humanTarget = mode == GameMode.Human ? 1f : 0f;
            cpuTarget = mode == GameMode.CPU ? 1f : 0f;
        }

        private void Update()
        {
            if (humanVolume != null)
                humanVolume.weight = Mathf.MoveTowards(humanVolume.weight, humanTarget, blendSpeed * Time.deltaTime);
            if (cpuVolume != null)
                cpuVolume.weight = Mathf.MoveTowards(cpuVolume.weight, cpuTarget, blendSpeed * Time.deltaTime);
        }
    }
}
