# NEX//ION — Team Progress Update

**Date:** 2026-08-31
**Prepared by:** Ayushman (Cyborg)
**Covers:** Everything built from project start through Session 5

> This is a plain-language summary of where the project stands, pulled from the working docs
> (`docs/progress.md`, `docs/steps.md`, `docs/guide.md`, `docs/puzzles.md`). If you want the
> click-by-click Editor setup, read `docs/guide.md`. If you want the full roadmap, read `docs/steps.md`.

---

## 1. What NEX//ION is (quick recap)

A cyberpunk **story-driven puzzle-adventure** game. You explore neon environments, find terminals /
keypads / data fragments, solve hacking and logic puzzles, and unlock narrative beats.

The signature mechanic is the **dual mode**:

- **H-MODE (Human)** — hold `Q` (or left trigger). Emotional/perceptual puzzles, dialogue, memory.
- **C-MODE (CPU)** — hold `E` (or right trigger). Hacking terminals, ciphers, logic locks. Costs a
  small movement/input penalty while active.
- Release both → **Neutral**.

Platform target: **PC / Windows standalone**, mouse + keyboard primary, controller supported for movement.

---

## 2. Important tech corrections (please read before opening the project)

The original synopsis said some things that are no longer accurate. The actual project is:

| Topic | Synopsis said | Actual state |
|---|---|---|
| Render pipeline | URP | **HDRP 17.5.0** (Unity 6000.5.9f1). All post-processing is done with HDRP **Volume** overrides. |
| Input | Legacy input | **New Input System only.** Legacy `UnityEngine.Input` does not work. Bindings live in `Assets/Settings/Input/NexionControls.inputactions`. |
| Stock input asset | — | `Assets/InputSystem_Actions.inputactions` is Unity's template and is **unused** — ignore it. |

None of this changes gameplay C# — the mode, player, and interaction systems are pipeline-agnostic —
but it matters for anyone doing visual/VFX or shader work (target HDRP Shader Graph, not URP).

---

## 3. What is built and working

All C# lives under `Assets/Scripts/`. Each session below is a committed milestone.

### Session 1 — Dual-mode core + player + interaction framework
- **`GameMode`** — the `Neutral / Human / CPU` enum.
- **`ModeController`** — reads the Q/E (or trigger) hold state, owns `CurrentMode`, fires
  `OnModeChanged`, handles the 0.5s switch delay.
- **`PlayerController`** — first-person move/look on a `CharacterController`; applies the 0.3s
  C-MODE input-lag penalty; basic Shift sprint.
- **`ModeVisualController`** — crossfades two HDRP Volume weights (Human / CPU) on mode change.
- **`IInteractable`** + **`PlayerInteractor`** — camera raycast, calls `Interact()` on `F` / gamepad West.
- **`DebugInteractable`** — throwaway console-logging test target.

### Session 2 — Terminal interaction shell
- **`TerminalPuzzle`** — an `IInteractable` terminal, **C-MODE only**, with a command parser
  (`scan`, `bypass <code>`, `exit`, `help`) and an `onSolved` UnityEvent you wire in the Inspector.
- **`TerminalUIController`** + a UI Toolkit screen (`Assets/UI/Terminal/`) styled from the
  Void Black / Signal Cyan palette. Opens/closes the screen and freezes the player while open.

### Session 3 — Keypad interaction shell
- **`KeypadPuzzle`** — an `IInteractable` numeric keypad, same C-MODE gate, fixed-length code,
  `onSolved` / `onFailed` events.
- **`KeypadUIController`** + a UI Toolkit keypad screen (`Assets/UI/Keypad/`) — clickable digit
  grid plus keyboard digit/backspace/enter fallback.

> **Important:** the Terminal and Keypad systems are **reusable infrastructure** (an interactable
> + a UI screen + a player-lock pattern). They are **not** any of the 23 designed puzzles yet.
> See section 5.

### Session 4 — Rebindable input + gamepad support
- **`NexionControls.inputactions`** — proper action asset: Move, Look, Jump, Sprint, Interact,
  ModeHuman, ModeCPU, each with Keyboard&Mouse + Gamepad bindings.
- **`InputManager`** — central lookup, typed accessors (`MoveInput`, `LookDelta`, `JumpPressed`…),
  persists binding overrides via `PlayerPrefs`.
- `ModeController`, `PlayerController`, `PlayerInteractor` refactored to read from `InputManager`
  instead of polling devices directly. **No default keybinds changed.**
- Gamepad works out of the box for move / look / jump / sprint / mode-switch / interact
  (Xbox layout native, PS via generic HID). Fixed a bug where right-stick look was almost motionless.

### Session 5 — Player lifecycle + first-person viewmodel (latest)
This session cherry-picks pieces of the imported **AFPC** asset and the **Free Test Character Asuna** asset.
- **`PlayerLifecycle`** — health / shield / damage / death / respawn for the player (shield absorbs
  first). On death it hard-freezes movement by disabling `PlayerController`; respawn re-enables it.
  Debug keys `R` / `H` / `T` (damage / heal / respawn) for testing — compiled out of release builds.
- **`PlayerHUDBinder`** + a trimmed **`AFPCUI`** — UI Toolkit HUD showing SHIELD / HP bars and a
  full-screen damage flash. (AFPC's endurance bar, interaction label and extensions panel were
  removed — they depended on AFPC systems this project doesn't use.)
- **`PlayerViewmodel`** — spawns Asuna as first-person arms holding a sci-fi pistol. **Equip and
  display only** — no firing/reload/aim, since there's no combat design yet.

---

## 4. Imported asset store packages (now in the repo)

| Package | What we're using it for |
|---|---|
| **Advanced First-Person Controller (AFPC)** | Only the **Lifecycle** part (health/shield/death/respawn) + a trimmed HUD. Not its movement/camera. |
| **Free Test Character Asuna** | First-person arms viewmodel + the sci-fi pistol/holster equipment. Humanoid rig, retarget-ready. |
| **3D Modern Menu UI (SlimUI)** | Main menu / settings / loading screen template — **not integrated yet.** |
| **MilkyWay** (skybox) | Environment / sky — imported, not yet wired into a scene. |

---

## 5. What is NOT built yet (honest gaps)

**Puzzle content — 0 of 23 built.** `docs/challenges.md` fully specifies 23 puzzles (C-01…C-10,
H-01…H-10, HB-01…HB-03) with exact solutions and UI layouts. None are implemented. The Terminal
and Keypad shells are a starting point for the "read a screen, type a value" ones, not finished puzzles.

**Other systems not started (from `docs/steps.md`):**
- Branching **dialogue engine** (custom C# vs. Yarn Spinner — undecided)
- **Memory fragment** collectibles (5 per level, 15 total) + persistence
- **Economy & upgrade trees** (₿O / ₿D / ◈ currencies, Neural / CPU / Interface trees)
- **Mode-reactive audio** (`ModeAudioController` — heartbeat vs. electronic hum)
- **On-screen interaction prompt** (the target string exists, nothing displays it)
- **Rebind-keys settings menu** (persistence hooks are ready; no UI calls them yet)
- Gamepad input **inside** the Terminal/Keypad screens (typing a command / picking digits on a pad)
- Respawn **checkpoint** teleport (respawn currently just refills health, doesn't move you)

**Content / art not started:**
- No level geometry exists. All Editor wiring assumes `Assets/OutdoorsScene.unity` or a fresh scene.
- HDRP Volume Profiles (`VP_WorldDefault`, `VP_HumanMode`, `VP_CPUMode`) are **specified** in
  `docs/visual_design.md` but not yet authored as assets in-Editor.
- Glitch VFX Graph effects, Shader Graph materials (neon tubes, wet floors), diegetic HUD — spec'd, not built.
- Monospace font asset for the terminal not imported yet (using UI Toolkit default).

---

## 6. What's next

**Immediate priority: Sprint 1 of the puzzle content** (from `docs/puzzles.md`):

1. **C-01 Binary Door** — simplest input-validation loop
2. **H-01 Neon Sequence** — simplest pattern-display puzzle
3. **C-03 ROT13 Terminal** — text input + a reusable cipher utility

After that, `docs/puzzles.md` has Sprints 2–5 laid out (grid systems → memory/timing → complex/hybrid
→ final puzzles). Dialogue, memory fragments, economy and audio (steps.md Steps 6–9) sit behind that.

---

## 7. How to run what exists

1. Open the project in **Unity 6000.5.9f1** (HDRP).
2. Open `Assets/OutdoorsScene.unity`.
3. Follow `docs/guide.md` Sessions 1–5 to place the `NEXION_Systems` / `Player` objects and wire
   the script fields. Each session ends with a "Test now" checklist.
4. Key gotcha: if Play mode does nothing, `InputManager` probably isn't added to `NEXION_Systems`
   with `NexionControls` assigned — every input script silently no-ops without it.

---

## 8. Doc map

| File | What it's for |
|---|---|
| `docs/nexion_synopsis_context.md` | Full GDD / project context (note the URP→HDRP correction above) |
| `docs/steps.md` | Full engineering roadmap, Phases 1–6, checkboxes |
| `docs/progress.md` | What actually exists, session by session, + known gaps |
| `docs/guide.md` | Click-by-click Unity Editor wiring for every script |
| `docs/challenges.md` | The 23-puzzle content design (solutions, layouts, fail states) |
| `docs/puzzles.md` | Puzzle build tracker + Sprint order (0/23 done) |
| `docs/visual_design.md` | Palette, material recipes, exact HDRP Volume values |
