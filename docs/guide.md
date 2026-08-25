# NEX//ION — Editor Wiring Guide

> This is the file to follow while sitting in the Unity Editor. It tells you exactly what GameObjects and components to create, and what script fields to drag where. It gets updated after every batch of new scripts — always re-read the top section before starting a session, in case something changed.
>
> Companion files: [steps.md](steps.md) (what's left, in order) · [progress.md](progress.md) (what exists and why).

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

1. Create Empty → name it `Volume_Human`. Add Component → **Volume** (search "Volume", it's `UnityEngine.Rendering.Volume`).
   - Check **Is Global**.
   - Next to **Profile**, click **New** to create a fresh Volume Profile. Save it as `Assets/Settings/HDRPDefaultResources/VP_HumanMode.asset` (or wherever you keep profiles).
   - **Weight**: set to `0` — the script drives this at runtime, don't leave it at 1 or it'll always be visible in the Editor.
   - Add overrides matching the doc's visual language for H-MODE: **Vignette** (subtle), **Color Adjustments** (warm amber/orange push), maybe a light **Chromatic Aberration** at frame edges. This is an art pass — tune to taste.
2. Repeat for `Volume_CPU`:
   - New profile: `VP_CPUMode.asset`.
   - Weight `0`.
   - Overrides: **Color Adjustments** (cold cyan/green), stronger **Chromatic Aberration** or a data-overlay-style effect if you have one, maybe **Bloom** bumped up slightly for the "everything is emissive data" feel.
3. Select `NEXION_Systems`. Add Component → `Mode Visual Controller` (`Nexion.Systems.ModeVisualController`).
4. Drag `Volume_Human` into **Human Volume**, `Volume_CPU` into **Cpu Volume**.

**Test now:** Play mode, hold `Q` — screen should ease into the warm/amber profile over about a third of a second (0.5s switch delay + blend speed). Hold `E` — should ease into cyan. Release both — back to neutral (both volumes fade to weight 0).

### Step 6 — Cleanup note

`Assets/InputSystem_Actions.inputactions` is Unity's stock template asset and is **not used** by anything above. Don't wire it to anything. Safe to ignore; safe to delete if it bothers you in the Project window. See progress.md for why direct device polling was used instead.

---

## What to do when you hit the next step

Steps 4–9 in `steps.md` (terminal hacking, keypad/cipher, dialogue, memory fragments, economy, audio) aren't built yet. When you're ready for the next one, ask for it specifically (e.g. "build the terminal hacking system") — this file will get a new dated section appended the same way Session 1 did, so you always have one place to look for "what do I click."

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
