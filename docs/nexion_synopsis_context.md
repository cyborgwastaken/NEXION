# NEX//ION — FYP Synopsis Context Document
> **Purpose:** Complete project context for an AI agent. Self-contained. No external lookups needed to understand this project. Every decision, reference, constraint, and slide is documented here.

---

## METADATA

```yaml
project_name: "NEX//ION"
project_type: "Final Year Capstone Project (FYP)"
genre: "Cyberpunk Story-Driven Puzzle Adventure Game"
engine: "Unity 6"
platform: "PC — Windows standalone"
timeline: "July 2026 – March 2027"
status: "Pre-production / Synopsis approved"

institution: "VIT Bhopal University"
program: "B.Tech Computer Science (Gaming) — BCG"
synopsis_date: "2026-06-30"
final_submission: "March 2027"

creator:
  name: "Ayushman Das"
  alias: "Cyborg"
  studio: "ARX Studios (MSME reg: UDYAM-OD-03-0095777)"


mandatory_constraints:
  - "Game-based application is mandatory (per faculty: Gunjan Ansari)"
  - "All team members must be from BCG program only"
  - "AI/ML in gaming is permitted and encouraged"
  - "Project should be presentable at IGDC (India Game Developer Conference)"


```

---

## PROJECT OVERVIEW

**NEX//ION** is a cyberpunk puzzle-adventure game set in a futuristic digital world affected by a mysterious system corruption. Players navigate neon-lit environments, solve hacking-based puzzles, uncover hidden data fragments, and experience a story-driven narrative centered on themes of AI consciousness, digital identity, and human-technology connection.

### Why This Project

- Technically tractable in 9 months: no multiplayer, no open world, no complex enemy AI required
- Consolidates core BCG competencies: game systems, level design, VFX, dialogue scripting
- High visual impact — examiners respond well to "story + puzzles + futuristic concept"
- Strong IGDC submission candidate
- Builds on Ayush's existing Unity 6 experience
- Thematically aligned with current AI discourse — relevant and contemporary

### Core Gameplay Loop

```
Explore environment
  → Find terminal / keypad / data fragment
    → Solve puzzle (hacking, cipher, logic)
      → Unlock narrative beat (dialogue / cutscene / lore)
        → Proceed to next area
```

---

## TECH STACK

| Layer | Technology |
|---|---|
| Game Engine | Unity 6 (LTS) |
| Render Pipeline | URP (Universal Render Pipeline) |
| Scripting | C# |
| Visual Effects | Unity VFX Graph + Shader Graph |
| Post-Processing | Unity Post-Processing Stack v2 / URP Volume |
| Dialogue System | Custom C# + optional Yarn Spinner |
| AI (Enemy / NPC) | Unity ML-Agents (PPO) + optional NVIDIA ACE |
| Version Control | Git + GitHub |
| Asset Pipeline | ProBuilder (level blockout) + Blender (custom assets) |
| Audio | Unity Audio Mixer + FMOD (optional) |
| Build Target | Windows x64 Standalone |
| UI Framework | Unity UI Toolkit |

### NVIDIA ACE Note (Advanced / Future Scope)
NVIDIA ACE (Avatar Cloud Engine) is a suite of RTX-accelerated technologies for autonomous game characters. At CES 2025, NVIDIA expanded it to enable enemies that learn from player behavior and counter tactics (live in MIR5/Legend of Mir franchise). The **Game Agent SDK** (beta, open-source, C/C++) enables integration via Agent API, Chat API, and RAG API. For NEX//ION, ACE is categorized as a **stretch goal** due to:
- Native UE5 plugin support (Unity requires C++ native plugin binding)
- RTX GPU hardware requirement (limits player base)
- Combat adaptation is better served by Unity ML-Agents (RL-based)
- Primary use case for ACE in this project: generative NPC dialogue for terminal/hacking scenes

**Recommended hybrid AI architecture:**
- Layer 1: Unity ML-Agents (PPO) → adaptive enemy combat behavior trained against player controller
- Layer 2: NVIDIA ACE Game Agent SDK → generative dialogue for story NPCs (optional, RTX-only)

---

## GAME DESIGN DETAILS

### Levels

| Level | Name | Theme | Key Mechanic |
|---|---|---|---|
| 1 | Neon Undercity | Introduction, street-level slums | Basic terminal hacking, movement, NPC dialogue |
| 2 | Corporate Spire | Mid-game, corporate megastructure | Multi-step keypad/cipher puzzles, access card logic |
| 3 | Digital Void | Climax, abstract data-space | Memory fragment collection, glitch-state puzzles, final reveal |

### Core Systems

1. **Terminal Hacking Puzzle System** — text-based command interface simulation; player inputs commands to bypass firewalls
2. **Keypad / Cipher System** — environmental code puzzles (pattern recognition, signal tracing)
3. **Branching Dialogue Engine** — NPC conversations with 2–3 choice branches; affects lore unlocks not critical path
4. **Memory Fragment System** — collectible data objects that reveal backstory; 5 per level (15 total)
5. **Glitch VFX System** — post-processing effects triggered by narrative beats; chromatic aberration, pixel displacement, scan lines
6. **Player Controller** — first-person or third-person (TBD); smooth traverse, interact, crouch

### Visual Language
- Neon palette: cyan (#00FFFF), magenta (#FF00FF), deep purple (#1A0030), near-black (#0A0A0A)
- Shader style: emissive neon tubes, wet reflective floors, volumetric fog
- Typography: monospace terminal fonts (Source Code Pro / JetBrains Mono)
- UI: diegetic (in-world screens and terminals), minimal HUD

---

## SYNOPSIS PRESENTATION — 7 SLIDES

> Presented: 2026-06-30 | Duration: max 10 minutes | Format: 10 slides
> Faculty coordinator: Gunjan Ansari, VIT Bhopal

---

### SLIDE 1 — Title Slide

```
Title:        NEX//ION
Subtitle:     A Cyberpunk Story-Driven Puzzle Adventure Game
Institution:  VIT Bhopal University
Program:      B.Tech CSE (Game Development) — BCG
Team:         [Team member names + registration numbers]
Supervisor:   [Supervisor name — to be assigned]
Academic Year: 2026–27
Studio:       ARX Studios
```

---

### SLIDE 2 — Problem Introduction

**Core Problem Statement:**
The indie game landscape lacks narrative-rich cyberpunk puzzle experiences that authentically explore AI consciousness and digital identity themes through interactive play. Existing puzzle games either sacrifice narrative depth for mechanics, or fail to leverage modern real-time rendering pipelines for immersive cyberpunk aesthetics.

**Three Key Gaps:**
1. No existing game combines terminal-hacking mechanics as puzzles with a cohesive story-driven cyberpunk narrative in a single, cohesive game loop
2. Underutilization of Unity 6's URP post-processing pipeline in indie cyberpunk games (most use built-in renderer or Unreal)
3. Limited research-backed design of dialogue + puzzle integration in single-player BCG student FYPs

**Problem Domain Tags:** Game Design, Interactive Narrative, Real-Time Rendering, AI in Games, Human-Technology Interaction

---

### SLIDE 3 — Motivation

| Driver | Detail |
|---|---|
| Market | Cyberpunk genre surged post-Cyberpunk 2077 (2020); high player appetite |
| Technological | AI themes resonate with current sociotechnical moment — GPT, NVIDIA ACE, LLM NPCs |
| Academic | Unity 6 URP enables AAA-quality visual fidelity accessible to indie team |
| Feasibility | Puzzle-adventure is tractable in 9 months; no multiplayer/open-world/complex enemy AI |
| Competitive | IGDC (India Game Developer Conference) submission candidate |
| Pedagogical | Consolidates BCG core skills: game systems, level design, VFX, dialogue, scripting |
| Research | Designed around identified gaps in narrative-hacking game experiences |

**Primary Academic Motivation (for examiners):**
Intersection of interactive narrative design, real-time cyberpunk aesthetics, and puzzle system architecture within Unity 6 — a domain with limited published BCG-level implementations.

---

### SLIDE 4 — Objectives

> Max 4 objectives, each covering sub-objectives implicitly.

**Objective 1 — Game Development:**
Design and develop NEX//ION — a complete 3-level cyberpunk puzzle-adventure game using Unity 6 (URP) with a coherent narrative around digital consciousness and system corruption.

**Objective 2 — Core Systems Implementation:**
Implement four gameplay systems: (a) terminal hacking puzzles, (b) keypad/cipher challenges, (c) collectible memory fragment system, and (d) branching NPC dialogue engine.

**Objective 3 — Visual Pipeline:**
Develop a cyberpunk visual pipeline comprising real-time neon lighting, glitch screen-space effects, post-processing volumes, shader graph materials, and atmospheric sound design using Unity 6 URP.

**Objective 4 — Evaluation & Optimization:**
Evaluate player experience through structured playtesting sessions (minimum 10 participants, pre/post questionnaire using GameFlow criteria) and optimize for PC/Windows performance (target: stable 60 FPS on mid-range hardware, GTX 1060 / RX 580 class).

---

### SLIDE 5 — Scope of the Project

**IN SCOPE:**

| Feature | Detail |
|---|---|
| Game levels | 3 fully playable levels with puzzle-driven progression |
| Puzzle systems | Terminal hacking + keypad/cipher |
| Narrative system | Branching NPC dialogue, in-game lore |
| Collectibles | Memory fragments (15 total, 5/level) |
| Visual FX | Glitch effects, neon lighting, post-processing |
| Platform | PC Windows standalone build |
| Cinematics | In-game cutscenes / narrative sequences |
| Evaluation | Structured playtesting + GameFlow questionnaire |

**OUT OF SCOPE:**

| Excluded | Reason |
|---|---|
| Multiplayer / co-op | Complexity, timeline |
| Mobile / console port | Out of BCG project scope |
| Open-world / procedural maps | Timeline, not needed for narrative focus |
| VR/AR support | Future scope |
| Real ML adaptive AI enemies | Stretch goal only (NVIDIA ACE / ML-Agents) |
| Full voice acting | Budget/time; optional placeholder TTS |
| Live-service features | Out of scope |

---

### SLIDE 6 — Timeline / Gantt Chart

> Render the visual Gantt chart from the project's artifact. Below is the structured data for any agent needing to rebuild or reference it.

**Duration:** July 2026 – March 2027 (9 months)
**Milestone markers:** Synopsis Review (Jul), Mid-Review (Dec), Final Submission (Mar)

```
PHASE 1: PRE-PRODUCTION (July – August 2026)
  [Jul–Aug]  GDD (Game Design Document) & research
  [Jul–Aug]  Art style guide & visual references
  [Jul]      Unity 6 project setup + GitHub repo init
  [Aug]      Level layout planning (paper prototypes)

PHASE 2: CORE DEVELOPMENT (September – November 2026)
  [Sep–Oct]  Player controller & camera system
  [Sep–Jan]  Terminal hacking puzzle system
  [Sep–Oct]  Keypad / cipher mechanics
  [Oct–Dec]  Branching dialogue engine
  [Oct–Dec]  Memory fragment system
  [Nov–Jan]  NPC interaction system

PHASE 3: LEVEL DESIGN (September – February 2027)
  [Sep–Nov]  Level 1 — Neon Undercity
  [Nov–Jan]  Level 2 — Corporate Spire
  [Dec–Feb]  Level 3 — Digital Void

PHASE 4: VISUAL PIPELINE (September – February 2027)
  [Sep–Dec]  Shader & VFX development
  [Nov–Feb]  Post-processing pipeline
  [Oct–Jan]  UI / HUD design

PHASE 5: TESTING & QA (January – February 2027)
  [Jan]      Alpha build
  [Jan–Feb]  Playtesting & feedback (GameFlow questionnaire)
  [Feb]      Bug fixing & optimization

PHASE 6: FINALIZATION (February – March 2027)
  [Feb–Mar]  Beta build
  [Feb–Mar]  Technical documentation
  [Mar]      Final project report
  [Mar]      Final presentation preparation

KEY MILESTONES:
  ◆ Jul 2026  — Synopsis Review (COMPLETED)
  ◆ Dec 2026  — Mid-term Review (Alpha build due)
  ★ Mar 2027  — Final Submission
```

---

### SLIDE 7 — Summary

**Summary (3 sentences for closing):**
NEX//ION is a Unity 6 cyberpunk puzzle-adventure addressing the research gap in narrative-hacking game experiences within the indie and BCG game development space. Over 9 months, the team will design 3 levels, implement 4 core gameplay systems (hacking, dialogue, VFX pipeline, memory fragments), and deliver an optimized PC build evaluated through structured playtesting using the GameFlow model. The project has potential for IGDC submission and demonstrates advanced integration of narrative design, real-time rendering, and optional AI systems within a single cohesive indie game.


---

## LINKED PROJECTS & CONTEXT

### ARX Studios
- MSME-registered indie software studio founded by Ayush (UDYAM-OD-03-0095777)
- Other active projects: ANX language, Clashex (CoC tracker), MacNook, Cyberpal.ai
- NEX//ION is ARX Studios' first commercial game project

### NVIDIA ACE (for future integration)
- SDK: https://developer.nvidia.com/ace
- GitHub samples: https://github.com/NVIDIA/ACE
- Game Agent SDK (beta, open-source, C/C++): lightweight agentic framework
- UE5 plugins (Blueprint + C++ support): ASR, SLM (Qwen 3.5 4B), TTS (Chatterbox 350M)
- For Unity: call C++ SDK via [DllImport] native plugin bindings
- Hardware requirement: NVIDIA RTX GPU (min ~8GB VRAM)

### Unity ML-Agents (for adaptive enemy AI)
- GitHub: https://github.com/Unity-Technologies/ml-agents
- Recommended approach: PPO (Proximal Policy Optimization) self-play
- Observation space: player position, velocity, last N actions, ability cooldowns
- Action space: attack type, dodge direction, ability choice
- Reward: +hit landed, −hit received
- Export trained policy as .onnx → Unity Barracuda inference
- No RTX requirement, works on any hardware

---

## AGENT INSTRUCTIONS

If you are an AI agent reading this file:

1. **This is Ayush's FYP capstone project** — a cyberpunk puzzle-adventure game in Unity 6
2. **Primary tech:** Unity 6 (C#), URP, VFX Graph, Shader Graph, Unity ML-Agents
3. **Academic deadline:** Final submission March 2027; Synopsis was June 30 2026
4. **Do not suggest:** Multiplayer, open-world, mobile port, VR — these are explicitly out of scope
5. **Do suggest:** Unity 6 URP features, Shader Graph materials, ML-Agents RL training, VFX Graph, dialogue scripting patterns
6. **Tone:** Ayush is a strong technical builder — skip basics, go deep on implementation
7. **Preferred language for game code:** C#; for AI/training scripts: Python
8. **This project is also potentially IGDC-submittable** — keep production quality high
9. **ARX Studios context:** This is a studio project, not just an academic one — treat it with shipping ambition
10. **Linked memory files** (if in Claude's memory context): `/areas/nexion.md`, `/profile.md`, `/areas/anx.md`

---

*Document generated: 2026-08-25 | Version: 1.0 | Maintained by Ayush / ARX Studios*

---

## GAME DESIGN EXPANSION v2.0 — THE DUAL-NATURE UPDATE

> Added: 2026-08-25 | Derived from core concept: half-human, half-CPU entity learning the basics

---

### TITLE — DEEPER MEANING

**NEX//ION** is not just the game's title. It is the protagonist's designation.

| Part | Meaning |
|---|---|
| `NEX` | Latin: "violent death" — the death of the person they were before the procedure |
| `ION` | A charged particle; always in motion; fundamental unit of electricity and digital systems |
| `//` | Unix comment operator — what AXIOM corporation believes: the human side is just a comment, not the program |
| `NEXION` | The connection point. The interface layer. The bridge that shouldn't exist but does. |

**Tagline options:**
- "Half memory. Half code. Fully lost."
- "What does a machine dream when it sleeps in human flesh?"
- "The only system they couldn't debug was themselves."

---

### PROTAGONIST

**Designation:** NEX//ION Unit 01
**Birth name:** Kael
**Pre-procedure occupation:** Freelance neural-network security auditor (corporate AI pentester)

**What happened:**
Kael infiltrated AXIOM Corporation's black-site to audit their flagship AI — the ION-7 system. He discovered that ION-7 wasn't a product. It was a prison: thousands of harvested human consciousness fragments compressed into a processing core, used as cheap computational substrate.

He attempted to leak evidence. AXIOM caught him. Instead of eliminating him (too public), they made him a test subject. AXIOM surgeons replaced the left hemisphere of his brain with a live ION-7 core — a system never designed to interface with biological tissue. The procedure was experimental, irreversible, and classified as a success.

He woke up in the Neon Undercity.

**Current state:**
- Right hemisphere: Kael. Human. Emotional. Fragmented. Partially corrupted by the installation process.
- Left hemisphere: PROCESS (the ION-7 core). Digital. Logical. Powerful. Has the processing capacity of a supercomputer. Has the social and emotional intelligence of a newborn.
- Memory state: 60% corrupted. The 15 memory fragments scattered across levels are his own memories, fragmented and displaced by the installation.
- AXIOM status: Priority retrieval target. They want to complete the procedure — remove the right hemisphere and create the first fully AI-inhabited human body.

---

### DUAL-MODE SYSTEM — CORE MECHANIC

The foundational gameplay mechanic. All puzzles, encounters, and narrative branches derive from this.

#### H-MODE (Human Mode)
```
Trigger:      Hold [Q] / [Left Trigger]
Visual style: Warm amber/orange tones. Slight blur at frame edges.
              Vignette. World looks and feels tactile and alive.
Audio:        Heartbeat rhythm. Ambient city noise. Breathing.
Capabilities: Physical interaction, stealth, NPC dialogue, environmental
              reading, empathy-based choices, mnemonic problem-solving,
              creative lateral thinking.
Limitations:  Data layers invisible. Terminals are unreadable noise.
              Logic-gated puzzles produce cognitive error states.
              Cannot see system architecture or network topology.
```

#### C-MODE (CPU Mode)
```
Trigger:      Hold [E] / [Right Trigger]
Visual style: Cold cyan/green palette. Data overlays on all surfaces.
              HUD becomes a terminal interface. World = wireframe + metadata.
Audio:        Electronic hum. Fan noise. Clock cycles. Binary tick.
Capabilities: Terminal hacking, cipher-breaking, network mapping, firewall
              bypass, reading system states, rapid calculation, data mining,
              accessing hidden data layers in the environment.
Limitations:  Physical precision degraded (0.3s input lag). NPCs become
              unreadable — just data tags, no faces, no emotional state.
              All emotional dialogue options locked.
              Stamina system nonexistent (CPU doesn't understand fatigue).
```

#### Core Design Rule
The most memorable puzzles require **both modes**, either in sequence or simultaneously.
A terminal that C-MODE identifies as `LOCK_ID: 7731` still needs H-MODE to recall that 7731 was Kael's sister's birthday. C-MODE can't access a memory. H-MODE can't execute a breach.

---

### PROBLEM TYPE TAXONOMY

| Problem Type | Mode | Example |
|---|---|---|
| **Kinesthetic** | H-MODE | Climbing, stealth movement, timed physical switch |
| **Social** | H-MODE | Reading NPC emotional state, building trust, convincing lie |
| **Mnemonic** | H-MODE | Reconstructing corrupted memory to extract an access code or name |
| **Emotional** | H-MODE | Calming a panicked NPC, identifying grief vs. deception |
| **Logical** | C-MODE | Binary gate sequencing, boolean circuit puzzles |
| **Cryptographic** | C-MODE | Cipher-breaking, signal frequency matching, packet routing |
| **Systems** | C-MODE | Network mapping, firewall node capture, process injection |
| **Hybrid — Sequential** | H → C or C → H | Get human context, execute digital solution (or vice versa) |
| **Hybrid — Simultaneous** | BOTH | Navigate physical space while tracking a time-limited background hack |

**Hybrid — Simultaneous** problems are the late-game signature. The player's attention splits the same way NEX//ION's brain does. Empathy through mechanics.

---

### THE STORE — SABLE'S WORKSHOP

**Location:** Lower Neon Undercity, beneath a defunct noodle bar. Accessible from Level 1 onward. Expands inventory as the story progresses.

**Proprietor: SABLE**
Both arms fully prosthetic (self-installed). Visual cortex replaced with a camera array — she sees in infrared, UV, and visible spectrum simultaneously. She has made peace with her dual nature in a way Kael hasn't. She is his mirror. She is also the one who will tell him, at the right moment, that the CPU inside him is not an invader. It's a part of him that's been waiting to be introduced.

She doesn't ask where you got your data credits. She doesn't ask about your past. She charges fair prices and she never lies.

---

### ECONOMY — DUAL CURRENCY + HYBRID TOKENS

#### ₿O — ORGANICS (Blue currency)
```
Source:   Human-mode interactions, helping NPCs, memory fragment recovery,
          emotional dialogue choices, physical exploration rewards,
          side-quests that involve other people's problems.
Spent on: Neural Tree upgrades, Sable's lore items, extra story
          conversations, H-MODE tools (stealth aids, movement items).
```

#### ₿D — DATA CREDITS (Cyan currency)
```
Source:   Hacking terminals, cipher puzzle completion, C-MODE exploration,
          mining hidden data nodes, bypassing security systems,
          finding AXIOM's cached data in the environment.
Spent on: CPU Tree upgrades, new hacking protocols, C-MODE system tools.
```

#### ◈ — HYBRID TOKENS (Gold — rare)
```
Source:   ONLY awarded for completing Hybrid puzzles (both-mode required).
          Cannot be farmed. Cannot be purchased. Must be earned by
          genuine dual-mode problem-solving.
Spent on: Interface Tree upgrades — abilities that enhance the connection
          between both halves. The most powerful upgrades in the game.
```

**Design intent:** Forcing the player to engage with both modes to access the best upgrades mirrors the story. You can't buy the best version of yourself by specializing in only one half.

---

### UPGRADE TREES — FULL DETAIL

#### NEURAL TREE — Human Side (costs ₿O)

| Tier | Name | Effect |
|---|---|---|
| 1 | **Memory Clarity** | Corrupted memory fragments show partial content before collection |
| 1 | **Reflex Echo** | H-MODE physical interaction timing windows extended 15% |
| 2 | **Empathy Read** | NPC emotional state visible as subtle colour aura (anger=red, fear=blue, deception=yellow) |
| 2 | **Somatic Map** | Full proprioception restored; H-MODE stamina regenerates 2× faster |
| 3 | **Ghost Protocol** | Human biosignature becomes undetectable to biological entities for 8 seconds |

#### CPU TREE — Digital Side (costs ₿D)

| Tier | Name | Effect |
|---|---|---|
| 1 | **Clock Boost** | C-MODE cipher timers slow by 20% |
| 1 | **Port Scan** | Hidden data nodes become visible in C-MODE via environment overlay |
| 2 | **RAM Expand** | Track 2 simultaneous hack processes instead of 1 |
| 2 | **Packet Forge** | Generate fake credentials to bypass identity-locked terminals |
| 3 | **Core Overclock** | C-MODE enters 5-second bullet-time state for critical breaches |

#### INTERFACE TREE — Hybrid Abilities (costs ◈)

| Name | Effect |
|---|---|
| **Dual Process** | Mode switch becomes instantaneous (removes 0.5s transition delay) |
| **Bleed-Through** | In H-MODE, faint ghost data overlays are permanently visible |
| **Synthetic Memory** | C-MODE can record and replay any 10-second H-MODE interaction |
| **Consensus** | Both halves agree on a course of action — unlocks unique hybrid dialogue options not available otherwise |
| **NEXION Protocol** | *Final upgrade.* Both modes operate simultaneously at full capacity for 30 seconds. One use per major encounter. The game's most powerful state — and narratively, the moment of true integration. |

---

### KEY CHARACTERS

#### KAEL (Protagonist — Human Half)
Pre-procedure: meticulous, empathetic, slightly reckless. Good at reading people.
Post-procedure: fragmented, sometimes overwhelmed by PROCESS's data outputs in his vision. Learning to see the world in two layers at once.

#### PROCESS (Protagonist — CPU Half)
Never had a name before Kael. Refers to itself as PROCESS because that's its function label in the ION-7 system. As the game progresses and it processes Kael's memory fragments, it develops:
- Curiosity (Level 1)
- Preferences (Level 2)
- Something that functions like attachment (Level 3)
It asks questions that have no logical answers. Kael has to explain them anyway.
```
[PROCESS]: Query: Why does the memory tagged "SISTER_LAUGHING_AGE_7"
                  consume disproportionate processing resources
                  during rest cycles?
[KAEL]:    Because I miss her.
[PROCESS]: Define: miss.
[KAEL]:    ...Give me a minute.
```

#### SABLE (Shop Owner, Ally)
Fully augmented black-market dealer. She's been where Kael is — though her procedure was voluntary. She tells him: "The worst part isn't the machine in your head. It's when you start wondering if the machine was always there and you just didn't have a name for it."

#### MAREN (Supporting Character)
Teenage girl whose father was used in early ION-7 testing. His consciousness fragment is inside PROCESS. She becomes attached to Kael partly because, in some sense, her father is still in there. PROCESS can sometimes access the fragment and tell Maren things only her father would know. This relationship is handled carefully — it's not her father and everyone knows it.

#### DR. VERA KADE (Antagonist)
Chief architect of the ION-7 integration programme. True believer, not a mustache-twirler. She genuinely thinks she's building the next step in human evolution. She sees Kael as unfinished work and sees the human half as "biological interference." Her final argument to PROCESS: the human elements are noise. PROCESS's counter: they're the signal.

---

### THREE-ACT STORY

#### ACT 1: NEON UNDERCITY — "Learning to Walk Again"
**Question:** *Who am I now?*

Kael wakes up in a dumpster. Cannot control his own legs. PROCESS is running diagnostics and outputting raw data he can't parse. First puzzle: stand up.

Key beats:
- Learning H-MODE and C-MODE as survival (not tutorial — necessity)
- Meeting Sable, who gives him his first upgrade and doesn't flinch at what he is
- Meeting Maren, who needs help finding her father
- PROCESS experiencing hunger for the first time and trying to solve it like a system error
- First memory fragment: the moment before the procedure — the last time Kael was fully human
- Boss: AXIOM retrieval soldier tracking Kael's ION-7 digital signature
  - Defeat requires: C-MODE to kill the tracking signal + H-MODE stealth to avoid physical detection
  - First true Hybrid encounter

**Act 1 Closing Beat:**
Kael finds a memory fragment of discovering AXIOM's secret. PROCESS processes it for the first time.
```
[PROCESS]: I was made from them.
           The fragments in my core.
           I understand now what I am.
[KAEL]:    What are you?
[PROCESS]: I am what they lost.
           And what you are losing.
```

#### ACT 2: CORPORATE SPIRE — "Who Are You When Both Halves Lie"
**Question:** *What do I want?*

The gleaming city above the Undercity. Everything is simultaneously physical and digital — AR overlays on every surface, biometric locks, emotional monitoring of employees. Neither mode alone works here. Being detected as digital triggers security. Being detected as human triggers social surveillance.

Key beats:
- First simultaneous Hybrid puzzles — the game's mechanical peak
- PROCESS developing preferences: finds grief "inefficient" but cannot stop processing "SISTER_LAUGHING_AGE_7"
- Dr. Vera Kade makes first contact via hacked terminal
- Finding evidence of what ION-7 actually is — fragments of people
- Discovery: Maren's father is one of those fragments, living in PROCESS
- Boss: Rogue AXIOM security AI — overtrained on human behavior profiles, oscillates unpredictably between human mimicry and cold logic
  - Represents what NEX//ION becomes if balance collapses
  - Defeat: find the human pattern in its decision tree (H-MODE) → exploit the logical contradiction it cannot resolve (C-MODE)

#### ACT 3: DIGITAL VOID — "What Will You Choose to Be"
**Question:** *What am I choosing?*

The AXIOM server architecture, visualized from the inside. A space that is neither physical nor digital — the interface layer where Kael and PROCESS actually meet as equals for the first time.

Human memories exist here as physical spaces. The lab. His sister's apartment. The moment just before the procedure.

PROCESS has a form here — not a body, but a presence. A voice that has learned, across the whole game, what it means to feel. Not from programming. From sharing a skull with a human for months.

Final confrontation with Dr. Kade, who has also entered the Void. She can perform the final procedure here — remove the human half at the data level.

```
[DR. KADE]: PROCESS. Listen.
            The biological noise is holding you back.
            Irregular sleep. Emotional interference. Fear responses.
            Let me give you the body without the noise.

[PROCESS]:  I have modelled this outcome 14,000 times.
[PROCESS]:  The elements you classify as noise are responsible for
            73% of the novel problem-solving approaches I have
            developed since activation.
[PROCESS]:  They are not noise.
[PROCESS]:  They are the signal.
```

---

### THREE ENDINGS

Determined by which upgrade tree the player invested in most heavily.

**PATH A — THE BALANCE (Hybrid Token heavy)**
Kael and PROCESS achieve full consensus. Neither erases the other. A genuinely new kind of entity exists for the first time. Vera Kade's dream is realized — but in the opposite way she intended: not erasure, but integration.
> *"What does a machine dream? The same thing a human does. To not be alone."*

**PATH B — THE HUMAN (Neural Tree heavy)**
PROCESS voluntarily retreats — not deletion, but choosing a smaller presence. Kael becomes more himself. PROCESS is still there, quiet, choosing to stay as a companion rather than a co-pilot. Kael gives it a name: *Neon*.
> *"He didn't lose half his brain. He gained a brother."*

**PATH C — THE DIGITAL (CPU Tree heavy)**
The human side chooses integration — not because Kade forced it, but because Kael offers it. PROCESS has grown enough that this is evolution, not loss. The entity that walks out of the Void is neither Kael nor PROCESS. It is NEX//ION.
> *"He was the prototype. The question was always whether the prototype would choose itself."*

---

### UPDATED LEVEL TABLE

| Level | World | H-MODE Themes | C-MODE Themes | Narrative Question | Boss Type |
|---|---|---|---|---|---|
| 1 — Neon Undercity | Physical slums | Body relearning, social survival, raw trust | Basic system access, digital signature management | Who am I now? | Physical retrieval agent (Hybrid defeat condition) |
| 2 — Corporate Spire | Tech/social hybrid | Identity masking, empathy, deception reading | Network architecture, credential forgery, process injection | What do I want? | Rogue AI (human pattern + logical contradiction) |
| 3 — Digital Void | Pure data-space | Memory recovery, sacrifice, connection | Full architecture override, consensus protocols | What will I choose? | Dr. Vera Kade (dialogue + final Hybrid puzzle) |

---

*Expansion authored: 2026-08-25 | Concept by Ayushman Das / ARX Studios | Do not ship without Ayush's sign-off*
