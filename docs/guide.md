# NEX//ION — Editor Wiring Guide

> This is the file to follow while sitting in the Unity Editor. It tells you exactly what GameObjects and components to create, and what script fields to drag where. It gets updated after every batch of new scripts — always re-read the top section before starting a session, in case something changed.
>
> Companion files: [steps.md](steps.md) (what's left, in order) · [progress.md](progress.md) (what exists and why) · [visual_design.md](visual_design.md) (exact colors/material/Volume values, referenced in Step 5).

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
