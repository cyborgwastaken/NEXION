# NEX//ION — Progress Log

> Snapshot of what actually exists in the project right now. Updated after every implementation session. For the full roadmap see [steps.md](steps.md); for editor wiring instructions see [guide.md](guide.md); for the palette/material/Volume design spec see [visual_design.md](visual_design.md); for the actual 23-puzzle content spec see [challenges.md](challenges.md), tracked in [puzzles.md](puzzles.md).

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

## Scripts implemented (Session 2 — 2026-08-26)

| Script | Path | Status | Purpose |
|---|---|---|---|
| `TerminalPuzzle` | `Assets/Scripts/Puzzles/TerminalPuzzle.cs` | done | `IInteractable` terminal puzzle, C-MODE gated, command parser (`scan`/`bypass <code>`/`exit`/`help`), fires `onSolved` UnityEvent |
| `TerminalUIController` | `Assets/Scripts/UI/TerminalUIController.cs` | done | Owns the shared terminal UI Toolkit screen; opens/closes it, disables player controller + interactor while open |
| `TerminalUI.uxml` / `.uss` | `Assets/UI/Terminal/` | done | The actual terminal screen — log scroll view + command input, styled from visual_design.md's Void Black / Signal Cyan palette |

## Scripts implemented (Session 3 — 2026-08-26)

| Script | Path | Status | Purpose |
|---|---|---|---|
| `KeypadPuzzle` | `Assets/Scripts/Puzzles/KeypadPuzzle.cs` | done | `IInteractable` keypad puzzle, C-MODE gated, fixed-length numeric code, `onSolved`/`onFailed` UnityEvents |
| `KeypadUIController` | `Assets/Scripts/UI/KeypadUIController.cs` | done | Owns the shared keypad UI Toolkit screen — clickable digit buttons + keyboard digit/backspace/enter fallback |
| `KeypadUI.uxml` / `.uss` | `Assets/UI/Keypad/` | done | The keypad screen — numeric display + 3x4 button grid, same palette as the terminal |

**Not yet implemented** (see steps.md for order): branching dialogue engine, memory fragment collectibles, currency/economy managers, upgrade trees, mode-reactive audio, actual level geometry.

---

## Known gaps / things to revisit

- `ModeController`'s Human/CPU detection is a simple "which key is held" check — no buffering, no priority rule beyond "if both held, fall back to Neutral." Revisit once playtesting reveals whether that feels right (design doc doesn't specify what happens if both Q and E are held).
- `ModeVisualController` only drives `Volume.weight`. It does not touch audio (heartbeat vs. electronic hum per the doc) — that's a separate `ModeAudioController` to add later, same pattern.
- No scene has been built yet. All wiring instructions in guide.md assume you're starting from the existing `Assets/OutdoorsScene.unity` or a new empty scene.
- `interactableMask` on `PlayerInteractor` defaults to "everything" — fine for prototyping, but once real levels exist this should be narrowed to an `Interactable` layer for performance and to avoid raycasting through puzzle set dressing.
- Visual design is now **specified** (`docs/visual_design.md` — palette, material recipes, exact HDRP Volume override values) but not yet **authored** as actual Volume Profile / Volume assets in the Editor. Guide.md Step 5 walks through creating them; nothing renders differently until that's done in-Editor.
- `TerminalUI` uses the UI Toolkit runtime default font, not the JetBrains Mono / Source Code Pro spec'd in visual_design.md §2 — no font asset has been imported into the project yet. Swap it in later via the Panel Settings / USS `-unity-font-definition` once a font asset exists.
- `TerminalPuzzle`'s "firewall" is a single access-code check, not a multi-stage lock. Fine for a first working puzzle; the doc's hybrid-puzzle example (C-MODE reveals `LOCK_ID: 7731`, H-MODE recalls it's a birthday) isn't wired to the memory fragment system yet since that system doesn't exist — `accessCode` is just a hardcoded Inspector field for now.
- Escape-to-close on the terminal only works while the command `TextField` has UI focus. If the player clicks elsewhere and focus is lost, Escape won't close it (only `exit` command or solving it will). Minor, revisit if it's actually annoying in practice.
- **Fixed (2026-08-26):** `TerminalUIController.cs` had `CS0104` ambiguous-reference compile errors on `Cursor` — `UnityEngine.UIElements` defines its own `Cursor` type (for UI Toolkit mouse-cursor styling) which collides with `UnityEngine.Cursor`. Any future script that has `using UnityEngine.UIElements;` and also needs to touch `Cursor.lockState`/`Cursor.visible` must fully qualify it as `UnityEngine.Cursor`. `KeypadUIController.cs` already does this correctly.
- `KeypadPuzzle`'s code is a static string, not a generated cipher — matches the doc's "Keypad / Cipher System" name in spirit (numeric entry, environmental lock) but not yet the "pattern recognition, signal tracing" cipher-generation part. Revisit once a level actually needs procedurally varied codes rather than one fixed number per lock.
- Two separate UI Toolkit screens now exist (Terminal, Keypad) sharing one `TerminalPanelSettings` asset — that's intentional (Panel Settings only controls rendering/scaling, not content) and keeps from needing a near-identical asset per puzzle type. If that naming bothers you later, renaming it to something generic like `NexionPanelSettings` is a safe find-and-reassign in the Inspector, not a code change.

## Scripts implemented (Session 4 — 2026-08-27)

| Script | Path | Status | Purpose |
|---|---|---|---|
| `NexionControls.inputactions` | `Assets/Settings/Input/` | done | The actual rebindable key/button bindings — Move, Look, Jump, Sprint, Interact, ModeHuman, ModeCPU, each with Keyboard&Mouse + Gamepad bindings |
| `InputManager` | `Assets/Scripts/Core/InputManager.cs` | done | Looks up the Player action map once, exposes typed accessors (`MoveInput`, `LookDelta`, `JumpPressed`, etc.), persists binding overrides via `PlayerPrefs` |

`ModeController`, `PlayerController`, and `PlayerInteractor` were edited in place to read from `InputManager.Instance` instead of `Keyboard.current`/`Gamepad.current` directly. No default keybind changed (WASD/mouse/Space/Shift/Q/E/F still work identically) — this was a pure data-driven-input refactor, prompted by needing to support a future key-rebind settings menu. Note: `PlayerController` had already picked up a basic Shift-sprint feature (added directly, outside this doc chain) before this session started — that's preserved and now reads from `InputManager.SprintHeld` like everything else.

**Known gap:** the actual rebind UI (a settings screen that calls Input System's `PerformInteractiveRebinding`) doesn't exist. `InputManager.SaveBindings()`/`ResetBindings()` are there for that screen to call into later, but nothing calls them yet — rebinding is *possible*, not yet *player-facing*.

**Deliberately out of scope:** Escape (pause/cursor-unlock) and the terminal/keypad UI's digit/Enter/Backspace keys stay hardcoded on `Keyboard.current` — those are UI/text-entry conventions, not the kind of action a rebind menu typically exposes.

**Fixed (2026-08-27):** gamepad support was already wired into `NexionControls.inputactions` (left stick move, right stick look, etc.), but the right stick was nearly unusable — `InputManager.LookDelta` was reading the stick's normalized `-1..1` rate through the same `lookSensitivity` scalar tuned for mouse's per-frame pixel delta, making a fully-deflected stick barely turn the camera. Fixed by checking `lookAction.activeControl?.device is Gamepad` and scaling stick input by a separate `gamepadLookSpeed` field times `Time.deltaTime` instead; mouse look is untouched.

**Known gap (controller):** the Terminal and Keypad puzzle UIs have no gamepad input path — `TerminalUIController` needs actual typed text (no virtual keyboard exists), and `KeypadUIController` has no D-pad/button-to-digit mapping and no `EventSystem`/`InputSystemUIInputModule` for UI Toolkit's gamepad navigation. Movement/look/jump/sprint/mode-switch/interact all work fine on a controller; opening a puzzle screen and actually solving it currently still requires keyboard/mouse. Not required for the stated PC/Windows platform target, but real work if a controller-only playthrough becomes a goal.

## Correction (2026-08-27) — challenges.md changes what "puzzle system" means here

`docs/challenges.md` landed with a fully-specified 23-puzzle content design (exact solutions, UI layouts, fail states) — a different and more concrete layer than anything built so far. Reconciling it with the sessions above:

- `TerminalPuzzle`/`TerminalUIController` and `KeypadPuzzle`/`KeypadUIController` are **reusable interaction infrastructure**, not implementations of any of the 23 puzzle IDs in challenges.md. Nothing in Sessions 2–3 should be read as "a puzzle is done." See `docs/puzzles.md` for the actual puzzle-by-puzzle tracker (currently: 0 of 23 built) and `docs/steps.md` Steps 4–5 for the corrected wording.
- challenges.md has its own Sprint 1–5 build order, which is now the authoritative next-steps sequence for puzzle content (starts with C-01 Binary Door, H-01 Neon Sequence, C-03 ROT13 Terminal). `steps.md`'s "Immediate next action" has been updated to point here instead of continuing with Step 6 (dialogue engine) — puzzle content is the more concrete, better-specified work right now.
