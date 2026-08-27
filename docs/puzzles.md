# NEX//ION — Puzzle Content Tracker

> Tracks implementation status of the 23 specific puzzles designed in [challenges.md](challenges.md). That file is the source of truth for puzzle logic, solutions, and Unity implementation notes — this file only tracks what's built vs. not, organized by challenges.md's own Sprint plan.
>
> **Important distinction:** `TerminalPuzzle` and `KeypadPuzzle` (built in guide.md Sessions 2–3) are reusable *interaction infrastructure* — an `IInteractable` + UI Toolkit screen + player-lock pattern — not any of the 23 puzzles below. None of the specific puzzle logic in challenges.md has been implemented yet. See progress.md's gaps section for detail on what those two scripts actually are.

Last updated: 2026-08-26

---

## Full puzzle list (from challenges.md Quick Reference)

| Puzzle ID | Name | Mode | Level | Difficulty | Status |
|---|---|---|---|---|---|
| H-01 | Neon Sequence | H-MODE | 1 | ★☆☆☆☆ | Not started |
| H-02 | Glyph Recall | H-MODE | 1 | ★★☆☆☆ | Not started |
| H-03 | Silhouette ID | H-MODE | 1 | ★★☆☆☆ | Not started |
| H-04 | Microexpression Read | H-MODE | 2 | ★★★☆☆ | Not started |
| H-05 | Memory Fragment Scan | H-MODE | 2 | ★★★☆☆ | Not started |
| H-06 | Spatial Assembly | H-MODE | 2 | ★★★☆☆ | Not started |
| H-07 | Drone Shadow | H-MODE | 2 | ★★★★☆ | Not started |
| H-08 | Stroop Override | H-MODE | 3 | ★★★★☆ | Not started |
| H-09 | Patrol Prediction | H-MODE | 3 | ★★★★★ | Not started |
| H-10 | Parallel Memory Pull | H-MODE | 3 | ★★★★★ | Not started |
| C-01 | Binary Door | C-MODE | 1 | ★☆☆☆☆ | Not started |
| C-02 | Hex Status Scan | C-MODE | 1 | ★★☆☆☆ | Not started |
| C-03 | ROT13 Terminal | C-MODE | 1 | ★★☆☆☆ | Not started |
| C-04 | Logic Gate Lock | C-MODE | 2 | ★★★☆☆ | Not started |
| C-05 | Node Route | C-MODE | 2 | ★★★☆☆ | Not started |
| C-06 | Base64 Intercept | C-MODE | 2 | ★★★★☆ | Not started |
| C-07 | Packet Forge | C-MODE | 2 | ★★★★☆ | Not started |
| C-08 | Firewall Weave | C-MODE | 3 | ★★★★☆ | Not started |
| C-09 | Process Inject | C-MODE | 3 | ★★★★★ | Not started |
| C-10 | Cipher Chain | C-MODE | 3 | ★★★★★ | Not started |
| HB-01 | Signal + Shadow | HYBRID | 2 | ★★★★☆ | Not started |
| HB-02 | Guard + Ghost | HYBRID | 2 | ★★★★★ | Not started |
| HB-03 | Kael's Last Memory | HYBRID | 3 | ★★★★★ | Not started |

---

## Build order (challenges.md's own Sprint plan)

This is challenges.md's "IMPLEMENTATION PRIORITY ORDER" section, turned into checkboxes. It covers 16 of the 23 puzzles — the ones that establish a reusable system (grid, timing, cipher-chain, etc.) that later puzzles build on.

- [ ] **Sprint 1 — Core puzzle framework**
  - [ ] C-01 Binary Door — simplest input validation loop
  - [ ] H-01 Neon Sequence — simplest pattern display
  - [ ] C-03 ROT13 Terminal — text input + cipher utility
- [ ] **Sprint 2 — Grid systems**
  - [ ] C-05 Node Route — grid + pathfinding (reused in HB-01)
  - [ ] C-04 Logic Gate Lock — gate eval system (reused in HB-02)
  - [ ] H-03 Silhouette ID — sprite comparison system
- [ ] **Sprint 3 — Memory + timing**
  - [ ] H-02 Glyph Recall — sequence memory (reused in H-05, H-10)
  - [ ] H-05 Memory Fragment Scan — RenderTexture + recall grid
  - [ ] C-08 Firewall Weave — timing system + trace meter
- [ ] **Sprint 4 — Complex + Hybrid**
  - [ ] C-09 Process Inject — reaction timing + cycle system
  - [ ] H-08 Stroop Override — fast succession panel system
  - [ ] HB-01, HB-02 — compose from existing systems
- [ ] **Sprint 5 — Final puzzles**
  - [ ] C-10 Cipher Chain — multi-stage manager
  - [ ] H-10 Parallel Memory Pull — multi-stream video + input
  - [ ] HB-03 Kael's Last Memory — narrative integration, needs the Memory Fragment system's collected-answers data

## Not in the Sprint plan (build alongside their sibling system)

challenges.md's priority list skips these 7 — they reuse a system built for a sibling puzzle rather than needing new tech, so slot them in whenever that sibling ships:

- [ ] H-04 Microexpression Read (Level 2) — reuses H-02/H-05-style sequence+choice UI
- [ ] H-06 Spatial Assembly (Level 2) — standalone drag/rotate system, no clear sibling; budget its own slot
- [ ] H-07 Drone Shadow (Level 2) — reuses H-03's silhouette comparison system
- [ ] H-09 Patrol Prediction (Level 3) — reuses C-05-style grid/path observation, adapted to timing
- [ ] C-02 Hex Status Scan (Level 1) — reuses C-01's terminal-panel shell
- [ ] C-06 Base64 Intercept (Level 2) — reuses C-03's cipher-decode input pattern
- [ ] C-07 Packet Forge (Level 2) — reuses C-03/C-06's hex/text input pattern

---

## Relationship to steps.md and the existing scripts

`steps.md` Steps 4–5 (Terminal Hacking, Keypad/Cipher) were built *before* challenges.md existed as a generic interaction shell, not as any specific puzzle here. Concretely:

- `TerminalUIController` (the open/close/lock-player/UI Toolkit pattern) is a solid base for any puzzle that's fundamentally "read a screen, type a value" — C-01, C-02, C-03, C-06, C-07 all fit that shape and can likely reuse it with different validation logic swapped in.
- `KeypadUIController` (numeric buttons + display) doesn't map to anything in challenges.md as-is — none of the 23 puzzles are "recall a memorized digit code." It stays useful as a UI pattern if a future puzzle needs raw numeric entry, but it's not pre-built content for any specific ID above.
- Neither script should be treated as "puzzle 1 done" — they're both still at zero puzzles built against this spec.

Next action, when ready: start Sprint 1 (C-01, H-01, C-03).
