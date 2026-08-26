# NEX//ION — Implementation Steps

> Full engineering roadmap, derived from the Gantt chart in `nexion_synopsis_context.md` (Phases 1–6). Check items off as they land. Granular near-term, coarser further out — refine each phase's steps as you get close to it. Cross-reference: [progress.md](progress.md) for what's actually done, [guide.md](guide.md) for how to wire each step into the Editor.

---

## Phase 1 — Pre-production (already largely satisfied by the synopsis doc + this session)

- [x] GDD groundwork captured in `nexion_synopsis_context.md`
- [x] Unity project exists (Unity 6000.5.9f1, HDRP 17.5.0)
- [x] Git repo initialized
- [ ] Art style guide / visual reference board (moodboard for neon palette, shader style)
- [ ] Paper prototypes for Level 1 layout

## Phase 2 — Core Systems (foundational scripting)

### Step 1 — Dual-mode core mechanic ✅ (this session)
- [x] `GameMode` enum
- [x] `ModeController` (Q/E hold detection, transition delay, `OnModeChanged` event)
- [x] `ModeVisualController` (Volume weight crossfade)

### Step 2 — Player controller ✅ (this session)
- [x] First-person move/look (`CharacterController`)
- [x] C-MODE 0.3s input lag penalty
- [ ] H-MODE stamina system (crouch, sprint, fatigue) — stubbed nowhere yet, needed before Neural Tree's "Somatic Map" upgrade makes sense
- [ ] Third-person camera option (doc lists first/third person as TBD — first-person implemented first; revisit if TBD resolves to third-person)

### Step 3 — Interaction framework ✅ (this session)
- [x] `IInteractable` interface
- [x] `PlayerInteractor` (raycast + interact key)
- [x] `DebugInteractable` (test-only)
- [ ] On-screen interaction prompt UI (currently `InteractionPrompt` string exists but nothing displays it — needs a UI Toolkit or Canvas prompt bound to `PlayerInteractor.CurrentTarget`)

### Step 4 — Terminal Hacking Puzzle System ✅ (Session 2)
- [x] `TerminalPuzzle : MonoBehaviour, IInteractable` — text command interface
- [x] Command parser (`scan`, `bypass <code>`, `exit`, `help`)
- [x] Firewall/lock state — binary solved/unsolved with a single access code (not a multi-stage state machine yet — revisit if a puzzle needs more than one gate)
- [x] UI: terminal screen via UI Toolkit (`Assets/UI/Terminal/TerminalUI.uxml` + `.uss`), styled from visual_design.md palette — monospace font asset not yet imported, using panel default font (see progress.md gap)
- [x] Only enterable in C-MODE (`Require Cpu Mode` toggle on `TerminalPuzzle`, on by default)

### Step 5 — Keypad / Cipher System (not started)
- [ ] `KeypadPuzzle : MonoBehaviour, IInteractable` — numeric/symbol entry
- [ ] Cipher generation/validation logic (pattern recognition, signal tracing per doc)
- [ ] Hybrid variant: code requires an H-MODE-recovered memory fragment as input (per the "LOCK_ID: 7731 = sister's birthday" example in the doc)

### Step 6 — Branching Dialogue Engine (not started)
- [ ] Decide: custom C# system vs. Yarn Spinner (doc lists both; custom gives more control over H/C-MODE gating of dialogue options, Yarn Spinner is faster to author in bulk)
- [ ] `DialogueNode` data structure (2–3 choices, lore-gated not critical-path-gated)
- [ ] Mode-gating: doc specifies emotional dialogue options lock out entirely in C-MODE, NPC faces/emotional state become unreadable

### Step 7 — Memory Fragment System (not started)
- [ ] `MemoryFragment : MonoBehaviour, IInteractable` — collectible, 5 per level (15 total)
- [ ] Persistence (which fragments collected, across scene loads)
- [ ] `Memory Clarity` Neural Tree upgrade hook (partial content preview before collection)
- [ ] Corrupted-memory reconstruction puzzle (used to extract access codes — ties into Step 5 hybrid puzzles)

### Step 8 — Economy & Upgrade Trees (not started)
- [ ] `CurrencyManager` — ₿O (Organics), ₿D (Data Credits), ◈ (Hybrid Tokens)
- [ ] Currency awarding hooks (H-MODE interactions → ₿O, C-MODE hacks → ₿D, Hybrid puzzles only → ◈)
- [ ] `UpgradeTree` data-driven definition (Neural/CPU/Interface trees, tiers, effects from the doc's tables)
- [ ] Upgrade effects actually wired to systems (e.g. `Dual Process` → `ModeController.SetTransitionDelay(0)`, already has a hook ready)

### Step 9 — Mode-reactive audio (not started)
- [ ] `ModeAudioController` — same pattern as `ModeVisualController`, crossfades heartbeat/ambient (H) vs. hum/clock-tick (C) audio sources

## Phase 3 — Level Design (Sep 2026 – Feb 2027)
- [ ] Level 1 — Neon Undercity blockout (ProBuilder)
- [ ] Level 2 — Corporate Spire blockout
- [ ] Level 3 — Digital Void blockout
- [ ] Populate levels with puzzle instances, memory fragments, NPCs using systems above

## Phase 4 — Visual Pipeline (Sep 2026 – Feb 2027)
- [x] Visual design system specified — palette, material recipes, Volume override values ([visual_design.md](visual_design.md))
- [ ] HDRP Volume Profiles authored in-Editor: `VP_WorldDefault`, `VP_HumanMode`, `VP_CPUMode` (values specified, not yet created as assets — see guide.md Session 1 / Step 5)
- [ ] Glitch VFX Graph effects (chromatic aberration bursts, pixel displacement, scan lines) tied to narrative beats — event-driven layer on top of the baseline Volumes, per visual_design.md §4
- [ ] Shader Graph materials: emissive neon tubes, wet reflective floors (recipes specified in visual_design.md §3, not yet built as Shader Graph assets)
- [ ] UI Toolkit HUD (diegetic, minimal)

## Phase 5 — Testing & QA (Jan – Feb 2027)
- [ ] Alpha build
- [ ] Playtesting sessions (min. 10 participants, GameFlow questionnaire per doc's Objective 4)
- [ ] Performance pass — target stable 60 FPS on GTX 1060 / RX 580 class hardware

## Phase 6 — Finalization (Feb – Mar 2027)
- [ ] Beta build
- [ ] Technical documentation
- [ ] Final report + presentation prep

---

## Immediate next action

Wire up Step 4's terminal scripts in the Editor (see guide.md Session 2) and confirm the C-MODE gate + command loop actually works in Play mode before starting Step 5 (keypad/cipher system).
