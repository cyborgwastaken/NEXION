# NEX//ION — Progress Log

> Snapshot of what actually exists in the project right now. Updated after every implementation session. For the full roadmap see [steps.md](steps.md); for editor wiring instructions see [guide.md](guide.md); for the palette/material/Volume design spec see [visual_design.md](visual_design.md).

Last updated: **2026-08-26**

---

## Correction to the synopsis doc

`docs/nexion_synopsis_context.md` specifies **URP** as the render pipeline. The actual project (`ProjectSettings/ProjectVersion.txt`, `Packages/manifest.json`) is **Unity 6000.5.9f1 with HDRP 17.5.0** — there is no URP package installed. All scripting below targets HDRP. This doesn't change any C# gameplay code (the dual-mode, player, and interaction systems are render-pipeline-agnostic), but it matters for:
- Post-processing: use HDRP **Volume** overrides (`UnityEngine.Rendering.HighDefinition` namespace) instead of URP ones when authoring the Human/CPU mode profiles.
- Any future shader work should target HDRP Shader Graph targets, not URP ones.

Active Input Handling is set to **Input System Package (New)** only (`activeInputHandler: 1` in ProjectSettings). The legacy `UnityEngine.Input` class is not usable. See the Input System note below.

---

## Input System note

The project ships with a default `Assets/InputSystem_Actions.inputactions` asset (Unity's stock template asset — Move/Look/Attack/Interact/Jump/Sprint on a "Player" map). **It is currently unused.** No C# wrapper class was generated for it.

Scripts written so far read devices directly (`Keyboard.current`, `Mouse.current`, `Gamepad.current`) instead of going through that asset. This was a deliberate choice: hand-editing `.inputactions` JSON outside the Editor is fragile, and NEX//ION's H-MODE/E-MODE-hold scheme doesn't map cleanly onto that stock asset anyway. You can safely ignore or delete `InputSystem_Actions.inputactions` — nothing references it. If later you want the visual Input Actions editor (rebindable keys, action maps, etc.), that's a deliberate future step, not a gap — see steps.md.

---

## Scripts implemented (Session 1 — 2026-08-26)

| Script | Path | Status | Purpose |
|---|---|---|---|
| `GameMode` | `Assets/Scripts/Core/GameMode.cs` | done | Enum: `Neutral`, `Human`, `CPU` |
| `ModeController` | `Assets/Scripts/Core/ModeController.cs` | done | Reads Q/E (or LT/RT) hold state, owns `CurrentMode`, fires `OnModeChanged`, handles the 0.5s switch delay |
| `PlayerController` | `Assets/Scripts/Player/PlayerController.cs` | done | First-person move/look via `CharacterController`; applies 0.3s input lag while in C-MODE |
| `ModeVisualController` | `Assets/Scripts/Systems/ModeVisualController.cs` | done | Crossfades two HDRP `Volume` weights (Human/CPU) on mode change |
| `IInteractable` | `Assets/Scripts/Interaction/IInteractable.cs` | done | Interface all puzzle/NPC/pickup objects will implement |
| `PlayerInteractor` | `Assets/Scripts/Interaction/PlayerInteractor.cs` | done | Raycasts from camera, calls `Interact()` on `[F]` / gamepad West |
| `DebugInteractable` | `Assets/Scripts/Interaction/DebugInteractable.cs` | done | Throwaway test target — logs to console, nothing more |

**Not yet implemented** (see steps.md for order): terminal hacking puzzle, keypad/cipher system, branching dialogue engine, memory fragment collectibles, currency/economy managers, upgrade trees, mode-reactive audio, actual level geometry.

---

## Known gaps / things to revisit

- `ModeController`'s Human/CPU detection is a simple "which key is held" check — no buffering, no priority rule beyond "if both held, fall back to Neutral." Revisit once playtesting reveals whether that feels right (design doc doesn't specify what happens if both Q and E are held).
- `ModeVisualController` only drives `Volume.weight`. It does not touch audio (heartbeat vs. electronic hum per the doc) — that's a separate `ModeAudioController` to add later, same pattern.
- No scene has been built yet. All wiring instructions in guide.md assume you're starting from the existing `Assets/OutdoorsScene.unity` or a new empty scene.
- `interactableMask` on `PlayerInteractor` defaults to "everything" — fine for prototyping, but once real levels exist this should be narrowed to an `Interactable` layer for performance and to avoid raycasting through puzzle set dressing.
- Visual design is now **specified** (`docs/visual_design.md` — palette, material recipes, exact HDRP Volume override values) but not yet **authored** as actual Volume Profile / Volume assets in the Editor. Guide.md Step 5 walks through creating them; nothing renders differently until that's done in-Editor.
