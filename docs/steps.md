# NEX//ION — Implementation Steps

> Full engineering roadmap, derived from the Gantt chart in `nexion_synopsis_context.md` (Phases 1–6). Check items off as they land. Granular near-term, coarser further out — refine each phase's steps as you get close to it. Cross-reference: [progress.md](progress.md) for what's actually done, [guide.md](guide.md) for how to wire each step into the Editor, [puzzles.md](puzzles.md) for the actual 23-puzzle content spec that Steps 4–5 below turned out to be infrastructure for, not implementations of.

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
- [x] Basic sprint (Shift, flat speed multiplier — added on disk between sessions)
- [ ] H-MODE stamina system (crouch, fatigue) — sprint exists but isn't gated by stamina yet, needed before Neural Tree's "Somatic Map" upgrade makes sense
- [ ] Third-person camera option (doc lists first/third person as TBD — first-person implemented first; revisit if TBD resolves to third-person)

### Step 3 — Interaction framework ✅ (this session)
- [x] `IInteractable` interface
- [x] `PlayerInteractor` (raycast + interact key)
- [x] `DebugInteractable` (test-only)
- [ ] On-screen interaction prompt UI (currently `InteractionPrompt` string exists but nothing displays it — needs a UI Toolkit or Canvas prompt bound to `PlayerInteractor.CurrentTarget`)

### Step 4 — Terminal interaction shell ✅ (Session 2) — infrastructure, not puzzle content
- [x] `TerminalPuzzle : MonoBehaviour, IInteractable` — text command interface
- [x] Command parser (`scan`, `bypass <code>`, `exit`, `help`)
- [x] Firewall/lock state — binary solved/unsolved with a single access code (not a multi-stage state machine yet — revisit if a puzzle needs more than one gate)
- [x] UI: terminal screen via UI Toolkit (`Assets/UI/Terminal/TerminalUI.uxml` + `.uss`), styled from visual_design.md palette — monospace font asset not yet imported, using panel default font (see progress.md gap)
- [x] Only enterable in C-MODE (`Require Cpu Mode` toggle on `TerminalPuzzle`, on by default)
- **Correction (2026-08-27):** this is a generic "read a screen, type a command" shell — it does not implement any specific puzzle. The actual C-MODE puzzle content is specified in [challenges.md](challenges.md) (C-01 through C-10) and tracked in [puzzles.md](puzzles.md). `TerminalUIController`'s open/close/lock-player pattern is a good base for the terminal-style ones (C-01, C-02, C-03, C-06, C-07) — none of them are built yet.

### Step 5 — Keypad interaction shell ✅ (Session 3) — infrastructure, not puzzle content
- [x] `KeypadPuzzle : MonoBehaviour, IInteractable` — fixed-length numeric code entry
- [x] Validation logic (exact code match; on-screen numpad UI + keyboard digit fallback)
- [x] Only enterable in C-MODE (`Require Cpu Mode` toggle, on by default)
- **Correction (2026-08-27):** same caveat as Step 4 — this is a generic numeric-entry shell, not one of the 23 puzzles in challenges.md. None of them are "recall a memorized digit code," so `KeypadUIController` doesn't map directly onto any puzzle ID; it stays available as a UI pattern if something later needs raw digit entry. See [puzzles.md](puzzles.md) for the real puzzle roadmap and its own Sprint-based build order (Sprint 1: C-01 Binary Door, H-01 Neon Sequence, C-03 ROT13 Terminal).

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

Start Sprint 1 from [puzzles.md](puzzles.md): C-01 Binary Door, H-01 Neon Sequence, C-03 ROT13 Terminal. These are the actual puzzle content from challenges.md — Steps 4–5 above only built the reusable shells they can be built on top of.
