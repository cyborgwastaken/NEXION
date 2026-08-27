# NEX//ION — Editor Wiring Guide

> This is the file to follow while sitting in the Unity Editor. It tells you exactly what GameObjects and components to create, and what script fields to drag where. It gets updated after every batch of new scripts — always re-read the top section before starting a session, in case something changed.
>
> Companion files: [steps.md](steps.md) (what's left, in order) · [progress.md](progress.md) (what exists and why) · [visual_design.md](visual_design.md) (exact colors/material/Volume values, referenced in Step 5) · [puzzles.md](puzzles.md) (the real 23-puzzle content spec — Sessions 2–3 below are reusable UI shells, not puzzle implementations).

Engine: Unity 6000.5.9f1, HDRP 17.5.0. Open `Assets/OutdoorsScene.unity` (or create a new empty scene) to do this setup.

---

## Session 1 (2026-08-26) — Dual-mode core, player controller, interaction framework

Scripts landed this session: `GameMode`, `ModeController`, `PlayerController`, `ModeVisualController`, `IInteractable`, `PlayerInteractor`, `DebugInteractable`. All under `Assets/Scripts/`.

### Step 1 — Systems object

1. In the Hierarchy: right-click → **Create Empty**. Name it `NEXION_Systems`.
2. Add Component → `Mode Controller` (this is `Nexion.Core.ModeController`).
3. Leave `Transition Delay` at `0.5` for now — that's the design-spec default. You'll drop it to `0` later via code when the "Dual Process" upgrade is unlocked; no need to touch it here.

This object is the single source of truth for the current mode. Nothing else needs a `ModeController` — everything else reads `ModeController.Instance`.

### Step 2 — Player

1. Create Empty → name it `Player`. Position it wherever your test spawn point is.
2. Add Component → **Character Controller**. Set:
   - Height: `1.8`
   - Radius: `0.4`
   - Center: `(0, 0.9, 0)`
3. Add Component → `Player Controller` (`Nexion.Player.PlayerController`).
4. Under `Player`, create a child empty object named `CameraPivot`. Position it at `(0, 1.6, 0)` — eye height.
5. Under `CameraPivot`, create your camera: right-click `CameraPivot` → **Camera** (this makes an HDRP-compatible camera by default since the project is HDRP). Name it `PlayerCamera`, leave its local position at `(0,0,0)`.
6. Delete or disable any other `Main Camera` already in the scene (e.g. the default one in `OutdoorsScene`) so you don't have two active cameras.
7. Select `Player` again. On the `Player Controller` component, drag `CameraPivot` into the **Camera Pivot** field.

At this point you have a working FPS rig: WASD to move, mouse to look, Space to jump, Escape to unlock the cursor.

### Step 3 — Interaction

1. Select `Player` (or `PlayerCamera` — either works, but `Player` is simpler since it already needs to exist).
2. Add Component → `Player Interactor` (`Nexion.Interaction.PlayerInteractor`).
3. Drag `PlayerCamera` into the **Interaction Origin** field (so the raycast fires from where you're actually looking, not from the player's feet).
4. Leave **Interaction Range** at `3` and **Interactable Mask** at `Everything` for now.

### Step 4 — Test interactable (verify the loop works)

1. Create a **Cube** (GameObject → 3D Object → Cube) a few meters in front of the player spawn. Name it `TestTerminal`.
2. Add Component → `Debug Interactable` (`Nexion.Interaction.DebugInteractable`).
3. Leave the default prompt/message, or customize them.

**Test now:** Enter Play mode, walk up to `TestTerminal`, look at it, press `F`. You should see `[DebugInteractable] TestTerminal: Debug interactable triggered.` in the Console. If that works, Steps 1–4 are verified end-to-end. Also test holding `Q` and `E` — no visible effect yet (that's Step 5), but confirm no errors appear in the Console while holding either.

### Step 5 — Mode-reactive visuals (HDRP Volumes)

The full palette, material recipes, and exact override values referenced below live in [visual_design.md](visual_design.md) — this is now the source of truth for every number in this step. Add each override via the Volume component's **Add Override** button, then punch in the value from the table.

**5a — Baseline volume (do this first)**

1. Create Empty → name it `Volume_WorldDefault`. Add Component → **Volume**.
   - Check **Is Global**.
   - New Profile → save as `Assets/Settings/HDRPDefaultResources/VP_WorldDefault.asset`.
   - **Weight**: `1` (this one is always fully on — it's the neutral baseline, not something the script blends).
   - Add overrides and values from visual_design.md §5, **VP_WorldDefault** table: Color Adjustments, Vignette, Fog, Bloom, and enable Screen Space Reflection.
   - This is also where the wet-reflective-floor and neon-emissive material recipes (§3) become visible — SSR needs to be on for reflective floors to show anything.
   - **If the SSR override shows "The current HDRP Asset does not support Screen Space Reflection":** that's a pipeline-level setting, not a Volume one. Click the **Open** button in the warning (or Edit → Project Settings → Graphics → click the SRP Settings asset), then under the HDRP Asset's **Lighting → Reflections**, check **Screen Space Reflection**. Also check Edit → Project Settings → HDRP Default Settings → Default Frame Settings (Camera) → Lighting → **Screen Space Reflection** — both the asset and the frame settings need it enabled.

**5b — Human volume**

1. Create Empty → name it `Volume_Human`. Add Component → **Volume**.
   - Check **Is Global**.
   - New Profile → save as `Assets/Settings/HDRPDefaultResources/VP_HumanMode.asset`.
   - **Weight**: `0` — the script drives this at runtime; leaving it at 1 makes it always-visible in the Editor.
   - Add overrides and values from visual_design.md §5, **VP_HumanMode** table: Color Adjustments, Vignette, Chromatic Aberration, Film Grain.

**5c — CPU volume**

1. Create Empty → name it `Volume_CPU`. Add Component → **Volume**.
   - Check **Is Global**.
   - New Profile → save as `Assets/Settings/HDRPDefaultResources/VP_CPUMode.asset`.
   - **Weight**: `0`.
   - Add overrides and values from visual_design.md §5, **VP_CPUMode** table: Color Adjustments, Vignette, Chromatic Aberration, Bloom, optionally Lens Distortion.

**5d — Wire the controller**

1. Select `NEXION_Systems`. Add Component → `Mode Visual Controller` (`Nexion.Systems.ModeVisualController`).
2. Drag `Volume_Human` into **Human Volume**, `Volume_CPU` into **Cpu Volume**. (`Volume_WorldDefault` doesn't get wired to anything — it's always on by itself.)

**Test now:** Play mode, hold `Q` — screen should ease into the warm/amber profile over about a third of a second (0.5s switch delay + blend speed). Hold `E` — should ease into cyan. Release both — back to neutral (both volumes fade to weight 0).

### Step 6 — Cleanup note

`Assets/InputSystem_Actions.inputactions` is Unity's stock template asset and is **not used** by anything above. Don't wire it to anything. Safe to ignore; safe to delete if it bothers you in the Project window. See progress.md for why direct device polling was used instead.

---

## Session 2 (2026-08-26) — Terminal Hacking Puzzle System

Scripts landed this session: `TerminalPuzzle` (`Assets/Scripts/Puzzles/TerminalPuzzle.cs`), `TerminalUIController` (`Assets/Scripts/UI/TerminalUIController.cs`), plus a UI Toolkit screen at `Assets/UI/Terminal/TerminalUI.uxml` + `TerminalUI.uss`.

How it works: a `TerminalPuzzle` is an `IInteractable` that only opens in **C-MODE** (per the doc — terminals are noise in H-MODE). Interacting opens a shared terminal UI where you type commands: `scan` (hint), `bypass <code>` (attempt the access code), `exit` (close). Solving it fires a `UnityEvent` you can wire to anything (a door, a light, a log message) in the Inspector — no extra code needed for that part.

### Step 7 — Panel Settings asset (one-time, shared by all UI Toolkit screens)

1. In the Project window, right-click `Assets/UI/Terminal/` → **Create → UI Toolkit → Panel Settings Asset**. Name it `TerminalPanelSettings`.
2. Leave its default settings as-is (Scale Mode: Constant Pixel Size is fine for now).

### Step 8 — Terminal UI object

1. Create Empty in the Hierarchy → name it `TerminalUI`.
2. Add Component → **UI Document**.
   - **Panel Settings** → drag in `TerminalPanelSettings`.
   - **Source Asset** → drag in `Assets/UI/Terminal/TerminalUI.uxml`.
3. Add Component → `Terminal UI Controller` (`Nexion.UI.TerminalUIController`).
   - **Player Controller** → drag in the `Player` object (it has the `Player Controller` component from Session 1).
   - **Player Interactor** → drag in the `Player` object again (same object, `Player Interactor` component).
4. Play mode should show nothing yet — the screen is hidden until a terminal is opened. If you instead see the terminal window covering the screen at all times, double check `TerminalUI.uxml` still has `TerminalRoot`'s display driven by the controller (it should auto-hide via `Close()` in `Awake`) — most likely cause is the `UI Document` component's Source Asset wasn't assigned.

### Step 9 — A terminal to test on

1. Create a **Cube** a few meters from the player spawn (like `TestTerminal` from Session 1, but this one's real). Name it `Terminal_Firewall01`.
2. Add Component → `Terminal Puzzle` (`Nexion.Puzzles.TerminalPuzzle`).
   - Leave the default fields (`Access Code: 7731` is the doc's own example — Kael's sister's birthday, LOCK_ID 7731). Change title/hint/code text if you want something custom.
   - **Require Cpu Mode** stays checked.
3. Optional: wire **On Solved** in the Inspector to something visible for testing — e.g. drag any GameObject in and pick `GameObject → SetActive` to toggle a light or door stand-in. Not required to test the core loop.

**Test now:** Play mode. Walk up to `Terminal_Firewall01` **without** holding `E` and press `F` — Console should log "signal unreadable" and nothing opens (confirms the C-MODE gate works). Now hold `E` and press `F` — the terminal window should appear, cursor should unlock, and player movement/look should freeze. Type `scan` + Enter → see the hint. Type `bypass 7731` + Enter → "ACCESS GRANTED", window auto-closes after ~1.2s, controls and cursor lock restore. Try interacting again — should log "ACCESS ALREADY GRANTED" instead of re-opening a fresh puzzle.

---

## Session 3 (2026-08-26) — Keypad / Cipher System

Scripts landed this session: `KeypadPuzzle` (`Assets/Scripts/Puzzles/KeypadPuzzle.cs`), `KeypadUIController` (`Assets/Scripts/UI/KeypadUIController.cs`), plus a UI Toolkit screen at `Assets/UI/Keypad/KeypadUI.uxml` + `KeypadUI.uss`.

How it works: a `KeypadPuzzle` is an `IInteractable`, same C-MODE gate as `TerminalPuzzle`, but the interaction itself is different — a fixed-length numeric code entered on an on-screen keypad (click the digit buttons, or type digits/numpad/Enter/Backspace on the keyboard). This is deliberately a different feel from the terminal's free-text commands, matching the doc's "environmental code puzzle" description of keypads vs. the terminal's "hacking console" description.

### Step 10 — Keypad UI object

1. Create Empty in the Hierarchy → name it `KeypadUI`.
2. Add Component → **UI Document**.
   - **Panel Settings** → drag in the same `TerminalPanelSettings` asset from Session 2 (Panel Settings just controls rendering/scaling, not content — no need for a second one).
   - **Source Asset** → drag in `Assets/UI/Keypad/KeypadUI.uxml`.
3. Add Component → `Keypad UI Controller` (`Nexion.UI.KeypadUIController`).
   - **Player Controller** → drag in the `Player` object.
   - **Player Interactor** → drag in the `Player` object again.

### Step 11 — A keypad to test on

1. Create a **Cube** a few meters from the player spawn. Name it `Keypad_Door01`.
2. Add Component → `Keypad Puzzle` (`Nexion.Puzzles.KeypadPuzzle`).
   - Default **Code** is `1234` (a placeholder — customize per lock once you're building real levels; it's deliberately different from the Terminal's `7731` so the two test objects don't feel like duplicates).
   - **Require Cpu Mode** stays checked.
3. Optional: wire **On Solved** in the Inspector the same way as the terminal (e.g. to a door/light stand-in).

**Test now:** Play mode. Hold `E` (C-MODE) and press `F` on `Keypad_Door01` — the keypad UI should appear with an on-screen numpad, cursor unlocked, player controls frozen. Click `1`, `2`, `3`, `4` — the display should fill in as you go. Click `OK` (or press Enter) — "ACCESS GRANTED", auto-closes after 1s. Try again with a wrong code (e.g. `9999`) first — should show "ACCESS DENIED" and clear the buffer without closing. Also confirm typing digits on the keyboard works the same as clicking.

---

## Session 4 (2026-08-27) — Rebindable input (Input Actions migration)

**Why:** every script up to this point read `Keyboard.current`/`Gamepad.current` directly — WASD, Q/E, Shift, F, Space were hardcoded. That's fine for prototyping but can't support a rebind-keys settings menu at ship time. This session moves all of that onto a proper Input Actions asset (`Assets/Settings/Input/NexionControls.inputactions`) via a new `InputManager`, which `PlayerController`, `ModeController`, and `PlayerInteractor` now read from instead of polling devices.

**No default keybinds changed** — WASD move, mouse look, Space jump, Shift sprint, Q/E for H-MODE/C-MODE, F to interact are all still the defaults, just now data-driven instead of hardcoded. Gamepad defaults also added: left stick move, right stick look, South jump, left stick click sprint, West interact, left/right trigger for H/C-MODE.

**Deliberately not covered:** Escape (pause/cursor-unlock) and the terminal/keypad UI screens' own key handling (digits, Enter, Backspace) stay hardcoded — those are UI/text-entry conventions, not the kind of action players expect a rebind menu to offer.

**Not built yet:** the actual in-game rebind UI (a settings menu where the player clicks "rebind" and presses a new key). `InputManager.SaveBindings()` / `ResetBindings()` exist and persist via `PlayerPrefs` so that menu has something to call into later, but no menu exists yet — this session only makes rebinding *possible*, not player-facing.

### Step 12 — Wire up InputManager

1. Select `NEXION_Systems` (the same object `ModeController` and `ModeVisualController` live on).
2. Add Component → `Input Manager` (`Nexion.Core.InputManager`).
3. Drag `Assets/Settings/Input/NexionControls.inputactions` into the **Input Actions** field.

**This step is not optional** — if `InputManager` isn't added and wired, there's no error or warning; the player will just silently stop responding to WASD/mouse look/Space/Shift/Q/E/F entirely (every script checks `InputManager.Instance != null` and no-ops otherwise). If Play mode suddenly does nothing after pulling these changes, this is almost certainly why.

**Test now:** Play mode. Confirm WASD move, mouse look, Space jump, Shift sprint, Q/E mode switch (with the Volume crossfade from Session 1), and F interact (on `TestTerminal`/`Terminal_Firewall01`/`Keypad_Door01`) all still behave exactly as before. Nothing should look or feel different — this session is a plumbing swap, not a gameplay change.

### Session 4 addendum (2026-08-27) — Controller support

**You don't need to do anything new in the Editor for this** — the gamepad bindings were already in `NexionControls.inputactions` from Step 12 above (left stick move, right stick look, South button jump, left-stick-click sprint, West button interact, left/right trigger for H-MODE/C-MODE). Plug in an Xbox-layout controller (works out of the box) or a PS controller (usually fine via generic HID, no extra package needed) and the core loop should respond immediately — no PlayerInput component or per-scheme setup required, since `InputManager` reads from all bound devices at once rather than locking to one control scheme.

**One real bug fixed:** the right stick was unusably slow. Mouse `<Pointer>/delta` reports an already-scaled per-frame pixel offset; a gamepad stick reports a normalized `-1..1` rate. Both were being read through the same `lookSensitivity` multiplier, so a fully-deflected stick barely turned the camera. `InputManager.LookDelta` now checks which device actually produced the value (`lookAction.activeControl?.device is Gamepad`) and scales stick input by a separate `gamepadLookSpeed` (Inspector field on `InputManager`, defaults to `180`) times `Time.deltaTime`, while mouse look is untouched. If stick look still feels too slow/fast once you test it, tune `gamepadLookSpeed` on `NEXION_Systems` — no code change needed.

**Honest gap — the Terminal and Keypad puzzle screens don't support gamepad input yet:**
- `TerminalUIController` reads free-text from a `TextField` via keyboard `KeyDownEvent`s. There's no virtual/on-screen keyboard, so a gamepad has no way to type a command at all right now.
- `KeypadUIController`'s digit buttons work by mouse click (or keyboard digit keys via direct polling in `Update()`), but nothing maps a gamepad button/D-pad to a digit yet — no `InputSystemUIInputModule`/`EventSystem` is set up in the scene, so UI Toolkit's built-in gamepad navigation isn't wired either.

Neither is required for the game's stated platform (PC/Windows standalone, mouse+keyboard assumed for typing puzzles), so this isn't necessarily a bug — but if you want a controller-only playthrough to work end to end, the keypad is the easier of the two to fix (finite digit set, could map D-pad/face buttons directly like the movement actions did); the terminal's free-text commands are the harder problem and would need either a from-scratch on-screen keyboard or redesigning it to be selection-based instead of typed.

**Test now:** with a controller connected, hold left trigger (H-MODE) / right trigger (C-MODE) and confirm the Volume crossfade still happens, move with the left stick, look with the right stick and confirm it now turns at a normal speed, and press the button bound to Interact (West/X) on `Terminal_Firewall01` or `Keypad_Door01` — the screen should open, but you won't currently be able to input anything into it without falling back to keyboard/mouse.

---

## What to do when you hit the next step

Steps 6–9 in `steps.md` (dialogue, memory fragments, economy, audio) aren't built yet — nor is Sprint 1 of the actual puzzle content in [puzzles.md](puzzles.md), nor the rebind-keys settings menu mentioned above. When you're ready for the next one, ask for it specifically — this file will get a new dated section appended the same way Sessions 1–4 did, so you always have one place to look for "what do I click."

---

## Quick reference — every script and where it lives

| Script | Goes on | Field it needs |
|---|---|---|
| `ModeController` | `NEXION_Systems` | (none — self-contained) |
| `PlayerController` | `Player` | `Camera Pivot` → `CameraPivot` transform |
| `PlayerInteractor` | `Player` | `Interaction Origin` → `PlayerCamera` transform |
| `ModeVisualController` | `NEXION_Systems` | `Human Volume` → `Volume_Human`, `Cpu Volume` → `Volume_CPU` |
| `DebugInteractable` | any test object (e.g. `TestTerminal`) | (none — self-contained) |
| `IInteractable` | — (interface, not a component) | implemented by any script that needs to be interactable |
| `TerminalUIController` | `TerminalUI` (needs a `UI Document` component too) | `Player Controller` → `Player`, `Player Interactor` → `Player` |
| `TerminalPuzzle` | any terminal object (e.g. `Terminal_Firewall01`) | (none required — optionally wire `On Solved`) |
| `KeypadUIController` | `KeypadUI` (needs a `UI Document` component too) | `Player Controller` → `Player`, `Player Interactor` → `Player` |
| `KeypadPuzzle` | any keypad object (e.g. `Keypad_Door01`) | (none required — optionally wire `On Solved` / `On Failed`) |
| `InputManager` | `NEXION_Systems` | `Input Actions` → `NexionControls` asset |
