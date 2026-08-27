# NEX//ION — Puzzle Challenges Design Document
> **Purpose:** Complete puzzle design spec for AI agent / developer implementation.
> All puzzles are designed for Unity 6 (C#, UI Toolkit, VFX Graph).
> Difficulty scales across 3 levels: Neon Undercity → Corporate Spire → Digital Void.

---

## QUICK REFERENCE — DIFFICULTY PROGRESSION

| Puzzle ID | Name | Mode | Level | Difficulty | Time Limit |
|---|---|---|---|---|---|
| H-01 | Neon Sequence | H-MODE | 1 | ★☆☆☆☆ | 30s |
| H-02 | Glyph Recall | H-MODE | 1 | ★★☆☆☆ | 5s flash + 20s input |
| H-03 | Silhouette ID | H-MODE | 1 | ★★☆☆☆ | 15s |
| H-04 | Microexpression Read | H-MODE | 2 | ★★★☆☆ | 0.5s per flash |
| H-05 | Memory Fragment Scan | H-MODE | 2 | ★★★☆☆ | 3s view + 30s recall |
| H-06 | Spatial Assembly | H-MODE | 2 | ★★★☆☆ | Untimed |
| H-07 | Drone Shadow | H-MODE | 2 | ★★★★☆ | 8s window |
| H-08 | Stroop Override | H-MODE | 3 | ★★★★☆ | 10s for 8 panels |
| H-09 | Patrol Prediction | H-MODE | 3 | ★★★★★ | 30s observe + 5s act |
| H-10 | Parallel Memory Pull | H-MODE | 3 | ★★★★★ | 4s per stream |
| C-01 | Binary Door | C-MODE | 1 | ★☆☆☆☆ | 45s |
| C-02 | Hex Status Scan | C-MODE | 1 | ★★☆☆☆ | 20s |
| C-03 | ROT13 Terminal | C-MODE | 1 | ★★☆☆☆ | 40s |
| C-04 | Logic Gate Lock | C-MODE | 2 | ★★★☆☆ | 30s |
| C-05 | Node Route | C-MODE | 2 | ★★★☆☆ | 20s |
| C-06 | Base64 Intercept | C-MODE | 2 | ★★★★☆ | 40s |
| C-07 | Packet Forge | C-MODE | 2 | ★★★★☆ | 45s |
| C-08 | Firewall Weave | C-MODE | 3 | ★★★★☆ | Trace meter |
| C-09 | Process Inject | C-MODE | 3 | ★★★★★ | 3 cycle attempts |
| C-10 | Cipher Chain | C-MODE | 3 | ★★★★★ | 90s (4 stages) |
| HB-01 | Signal + Shadow | HYBRID | 2 | ★★★★☆ | 60s total |
| HB-02 | Guard + Ghost | HYBRID | 2 | ★★★★★ | 90s total |
| HB-03 | Kael's Last Memory | HYBRID | 3 | ★★★★★ | Untimed (final) |

---

## H-MODE PUZZLES
> Human-mode puzzles test visual pattern recognition, spatial reasoning, memory, and emotional intelligence.
> These represent what PROCESS (the CPU) fundamentally cannot do — intuition, empathy, emotional context.

---

### H-01 — Neon Sequence
```
Mode:       H-MODE
Level:      1 — Neon Undercity
Type:       Color sequence completion
Difficulty: ★☆☆☆☆
Time Limit: 30 seconds
```

**In-World Context:**
A crumbling neon mural on a district wall. The colors cycle as a gang authentication signal — the right sequence opens a hidden passage to the next district. Kael recognizes the pattern instinctively. PROCESS tries to analyze it and fails ("insufficient contextual data for pattern inference").

**What the Player Sees:**
A horizontal strip of 8 colored panels:
```
[CYAN] [MAGENTA] [PURPLE] [CYAN] [MAGENTA] [PURPLE] [CYAN] [???]
```
Below: three buttons — CYAN / MAGENTA / PURPLE.

**What the Player Does:**
Observe the pattern, tap the correct color for the missing `[???]` panel.

**Solution:** `MAGENTA`  
The repeating group is `CYAN → MAGENTA → PURPLE`. Position 8 = position 2 in the cycle = MAGENTA.

**Fail State:** Pattern resets with a new color set. 3 attempts.

**Design Escalation:** In Level 1, groups repeat every 3. By Level 2-3 variants (if reused), groups repeat every 5 with two color changes.

**Unity Notes:**
- Array of `Image` components with color values
- Shuffle color set from a predefined palette on each play
- Coroutine highlights panels in order on puzzle init
- Button click compares selection against `correctAnswer` ScriptableObject field

---

### H-02 — Glyph Recall
```
Mode:       H-MODE
Level:      1 — Neon Undercity
Type:       Visual working memory
Difficulty: ★★☆☆☆
Time Limit: 5s flash → 20s input
```

**In-World Context:**
A black-market contact uses a sequence of gang glyphs as an authentication handshake. They flash their glyph sequence once. Kael must mirror it back from memory to prove he's legitimate.

**What the Player Sees:**
Phase 1 (Flash): A 3×3 grid of glyphs lights up in a specific ORDER — 4 glyphs highlight one by one in sequence (like Simon Says). Each glyph stays lit for 0.8 seconds.

Phase 2 (Input): The grid goes dark. Player must tap the 4 glyphs in the correct order.

**Glyph set (9 total):** Stylized cyberpunk symbols — eye, circuit, skull, flame, hex, spiral, lock, wave, pulse.

**Solution example:** `EYE → CIRCUIT → SKULL → HEX`

**Fail State:** Contact refuses entry. 2 attempts before they leave.

**Design Escalation:**
- Level 1: 4 glyphs, 9-glyph grid
- Level 2 variant: 5 glyphs, 12-glyph grid, 0.5s each
- Level 3 variant: 6 glyphs, some glyphs look near-identical (distorted glitch VFX on duplicates)

**Unity Notes:**
- `List<int>` tracks sequence order
- Coroutine flashes `Image` highlight components in order
- Player input adds to `playerSequence List<int>`, compared on completion
- Glitch shader variant for Level 3 distortion

---

### H-03 — Silhouette ID
```
Mode:       H-MODE
Level:      1 — Neon Undercity
Type:       Silhouette + shape pattern matching
Difficulty: ★★☆☆☆
Time Limit: 15 seconds
```

**In-World Context:**
Kael needs to confirm the identity of Maren's father — someone he has only a partial corrupted memory of. He spots four figures through a frosted security window. He must identify which one matches the silhouette from his memory.

**What the Player Sees:**
Left side: A clear reference silhouette (man, left arm prosthetic, slight stoop, carrying a data case).  
Right side: Four frosted silhouettes — three are decoys with subtle differences (no prosthetic, different posture, different object carried).

**What the Player Does:**
Compare reference against the four silhouettes. Select the matching one.

**Solution:** One correct match. Decoys differ in: prosthetic arm presence, stoop angle, carried object shape.

**Fail State:** If wrong, frosted window fades — figure moves on. Kael loses track of him. Different path to find him (alternate puzzle).

**Design Escalation:**
- Level 1: 4 silhouettes, 1 difference per decoy
- Level 3 variant (H-03-B, Drone Shadow): 8 silhouettes, moving, 3 differences per decoy, 8s window

**Unity Notes:**
- Sprite-based silhouettes using `Alpha Clip` shader
- Frosted glass = URP `Full Screen Pass Renderer Feature` with blur
- Reference sprite stored in `NPCDataSO` (ScriptableObject per character)

---

### H-04 — Microexpression Read
```
Mode:       H-MODE
Level:      2 — Corporate Spire
Type:       Emotional pattern recognition + temporal sequence
Difficulty: ★★★☆☆
Time Limit: 0.5s per expression flash, 10s choice window
```

**In-World Context:**
A Corporate Spire employee is blocking Kael's path. She's pretending to be calm — but Kael's human intuition reads her face. Three microexpressions flash in sequence. Identifying the pattern determines the correct social manipulation: sympathy, authority, or bribery.

**PROCESS output:** `FACIAL_MUSCLE_DATA: INCONCLUSIVE. RECOMMEND: LOGICAL_APPROACH.`  
*(CPU literally cannot read this — it outputs nothing useful. Human half must take over.)*

**What the Player Sees:**
NPC's face. Three microexpressions flash 0.5s each in a left panel:
1. Brow raise (FEAR)
2. Jaw clench (SUPPRESSED_ANGER)
3. Lip compress (DECEPTION)

Then: `[What is she hiding?]` — three choice buttons:
- A) She's afraid of her superior (→ use sympathy)
- B) She's angry about something unrelated (→ use distraction)
- C) She's lying about her clearance level (→ use bluff)

**Solution:** `C` — the sequence FEAR → SUPPRESSED_ANGER → DECEPTION indicates she's concealing her own unauthorized access. Correct answer opens the correct dialogue branch.

**Why this is H-MODE only:** Expression interpretation requires emotional context. PROCESS can detect muscle movements but cannot assign emotional valence.

**Fail State:** Wrong choice leads to a failed social encounter — must find alternate route (adds 2 minutes to level).

**Design Escalation:**
- Level 2: 3 expression sequence, 3 choices
- Level 3 variant (H-04-B): 5 expression sequence, 5 choices, 0.3s each, expressions partially obscured by glitch effect

**Unity Notes:**
- Sprite animation sheet for facial expressions (or blendshape-driven 3D face)
- `WaitForSeconds(0.5f)` coroutine to flash expressions
- `DialogueManager` reads correct answer from `NPCExpressionSO`
- PROCESS dialogue overlay: greyed-out text `ANALYSIS FAILED` on choice screen

---

### H-05 — Memory Fragment Scan
```
Mode:       H-MODE
Level:      2 — Corporate Spire
Type:       Working memory (visual recall under load)
Difficulty: ★★★☆☆
Time Limit: 3s view + 30s recall
```

**In-World Context:**
Kael recovers a corrupted memory fragment — a room he was in before the procedure. The memory plays for 3 seconds before corrupting. He must identify which objects were in the room to reconstruct the access code hidden inside the memory.

**What the Player Sees:**
Phase 1: A rendered room scene plays for exactly 3 seconds. 7 objects are visible: a blue data drive, a cracked mug, a circuit board diagram on the wall, a red terminal, a window with rain, a stack of folders, a yellow coffee mug.

Phase 2: A 4×3 grid of 12 object thumbnails. Player must select exactly 7 that were in the room.

**Solution:** The 7 correct objects. 5 decoys are plausibly similar objects (green terminal, intact mug, clean diagram, a coat rack, a green data drive).

**Why this is H-MODE:** Memory consolidation and visual scene reconstruction. PROCESS logs data as binary — it cannot reconstruct emotional visual memory.

**Fail State:** Fewer than 5 correct → fragment corrupts. Must find the memory fragment elsewhere in the level.

**Design Escalation:**
- Level 2: 7 objects, 3s view, 30s recall, 12-item grid
- Level 3 variant (H-05-B): 9 objects, 2s view, 25s recall, objects in scene are partially occluded

**Unity Notes:**
- Rendered scene: either a pre-baked RenderTexture or a hidden scene camera
- Object grid: `GridLayoutGroup` with `Toggle` components
- Correct answers stored in `MemoryFragmentSO`
- Timer UI: `Image.fillAmount` countdown arc

---

### H-06 — Spatial Assembly
```
Mode:       H-MODE
Level:      2 — Corporate Spire
Type:       Spatial reasoning + mechanical assembly
Difficulty: ★★★☆☆
Time Limit: Untimed (pressure from NPC watching creates implicit urgency)
```

**In-World Context:**
A physical lock mechanism on a corporate server room is disassembled — sabotaged to prevent unauthorized access. The maintenance manual was stolen. Kael must reassemble it by feel and visual logic. PROCESS tries to cross-reference assembly diagrams and finds no data.

**What the Player Sees:**
5 mechanical components in a workspace panel (rendered as stylized flat icons). A ghost-outline of the correct assembled lock on the right side, with 5 empty slots.

Each component can be:
- Rotated (0°, 90°, 180°, 270°)
- Placed into a specific slot

**Solution:** 5 pieces → 5 slots, each in a specific rotation:
```
Slot 1: HOUSING     → 0°
Slot 2: PIN ARRAY   → 90°
Slot 3: DRIVER BAR  → 180°
Slot 4: SPRING SET  → 0°
Slot 5: CAM DISC    → 270°
```

Only one valid combination completes the ghost outline.

**Why this is H-MODE:** Mental rotation and spatial assembly are human cognitive tasks. PROCESS attempts to brute-force rotation combinations and runs a fake progress bar that never completes.

**Fail State:** Incorrect assembly causes the pieces to rattle (audio cue) and highlight in red. Unlimited attempts but NPC dialogue gets increasingly impatient.

**Unity Notes:**
- Drag-and-drop via `IBeginDragHandler`, `IDropHandler`
- Rotation via right-click / R key cycling through 4 states
- Ghost outline uses `SpriteRenderer` with 30% alpha
- Validation: check each slot's `placedPiece.id` and `rotation` against `LockDataSO`

---

### H-07 — Drone Shadow
```
Mode:       H-MODE
Level:      2 — Corporate Spire
Type:       Shadow/silhouette spatial reasoning + inference
Difficulty: ★★★★☆
Time Limit: 8-second window (drone keeps moving)
```

**In-World Context:**
An AXIOM patrol drone flies overhead. Kael can't see the drone directly (too high, too bright) but sees its shadow moving across the floor. He needs to identify the drone model from its shadow to know its camera blind spot and which direction to hide.

**PROCESS output:** `SHADOW_VECTOR: CALCULATED. MODEL_ID: INSUFFICIENT_DATA.`  
*(CPU can calculate the vector but not infer the 3D shape from a 2D shadow — that requires spatial imagination.)*

**What the Player Sees:**
An animated shadow moves across the scene floor in 8 seconds. Shadow shape: asymmetrical, 4 rotor mounts visible, one side has a larger sensor pod.

Panel on HUD shows 4 drone model silhouettes from above:
- A) Symmetric quad-rotor, no pod
- B) Asymmetric quad-rotor, LEFT sensor pod
- C) Asymmetric quad-rotor, RIGHT sensor pod
- D) Hex-rotor, centered pod

**Solution:** `B` — shadow shows the asymmetric shape with the sensor pod projecting LEFT when shadow is correctly oriented.

**Why this is H-MODE:** Spatial inference from 2D shadow to 3D object shape requires mental rotation and intuitive spatial modeling.

**Fail State:** Wrong selection = Kael hides on the wrong side = drone's camera sweeps him = alert triggered.

**Unity Notes:**
- Blob shadow projected from an off-screen moving transform
- Shadow shape: custom sprite with subtle animation
- 8-second countdown before auto-fail (wrong side hide)
- Correct answer fed into `StealthManager.SetBlindSpot()`

---

### H-08 — Stroop Override
```
Mode:       H-MODE
Level:      3 — Digital Void
Type:       Cognitive interference / Stroop effect
Difficulty: ★★★★☆
Time Limit: 10 seconds for 8 panels
```

**In-World Context:**
Deep in the Digital Void, AXIOM's data architecture uses mislabeled data streams to confuse intruders — a deliberate cognitive trap. Labels say one thing, the data IS another thing. The CPU cannot resolve this conflict (it reads the label, not the value). Kael's human cognition must override the label and read the actual color.

**PROCESS output:** `ERROR: DATA_LABEL CONFLICTS WITH DATA_VALUE. CANNOT RESOLVE.`

**What the Player Sees:**
8 panels in sequence. Each panel shows a WORD written in a COLOR that doesn't match:

```
Panel 1: "CYAN"     — written in MAGENTA     → Answer: MAGENTA
Panel 2: "PURPLE"   — written in CYAN        → Answer: CYAN
Panel 3: "GREEN"    — written in RED         → Answer: RED
Panel 4: "RED"      — written in PURPLE      → Answer: PURPLE
Panel 5: "MAGENTA"  — written in GREEN       → Answer: GREEN
Panel 6: "BLUE"     — written in YELLOW      → Answer: YELLOW
Panel 7: "YELLOW"   — written in BLUE        → Answer: BLUE
Panel 8: "WHITE"    — written in ORANGE      → Answer: ORANGE
```

Player must tap the correct COLOR (not the word) from a color picker for each panel. 10 seconds total. 1.25s per panel.

**Why this is H-MODE:** The Stroop effect specifically exploits human language processing over color perception. Ironically, PROCESS reads the label (digital text) and cannot disambiguate. Only Kael's visual cortex can override the verbal label.

**Fail State:** Each wrong answer locks one data channel. Getting 4+ wrong corrupts the path to the next memory space.

**Unity Notes:**
- `Text` (UI Toolkit Label) with `color` set opposite to word content
- Scramble word/color pairs from a predefined `StroopDataSet[]` array
- Timer: global 10s countdown, progress bar
- Correct sequence stored in `CorrectColors[]` array, checked per panel

---

### H-09 — Patrol Prediction
```
Mode:       H-MODE
Level:      3 — Digital Void
Type:       Temporal pattern recognition + behavioral prediction
Difficulty: ★★★★★
Time Limit: 30s observe + 5s decision window
```

**In-World Context:**
A corrupted security routine in the Digital Void — a phantom guard that loops its patrol. Unlike a real AI, it's a pure behavioral ghost: predictable, but only if Kael watches carefully. PROCESS maps its position every frame but cannot project emotional intent, meaning it can map WHERE it is but not WHERE it will be when Kael needs to cross.

**What the Player Sees:**
A top-down grid map (7×7). A glowing entity (the phantom) moves along a path. Player watches it complete 3 full loops over 30 seconds.

The phantom's path has a subtle variation — on loop 3, it pauses at one node for 1 extra second before continuing. This pause creates a 5-second window.

At 30s: `[CROSS NOW?]` button appears. Player must tap it during the correct 5-second window (when the phantom is at its farthest point from the crossing).

**Solution:** The crossing window is between second 38 and second 43 of the puzzle runtime. Tapping outside this window = caught.

**Why it's hard:** The variation only appears on the third loop. Players who don't watch all 3 loops will miscalculate.

**Why this is H-MODE:** Predicting behavioral variation from a short observation window — inferring intention from pattern anomaly — is fundamentally intuitive.

**Unity Notes:**
- Phantom follows a `List<Vector2Int>` waypoint path with `NavMesh` or manual interpolation
- Loop 3: `WaitForSeconds(1f)` injected at a specific waypoint index
- Crossing window: `bool inSafeWindow` toggled by coroutine
- Button only appears after 30s; `onClick` checks `inSafeWindow`

---

### H-10 — Parallel Memory Pull
```
Mode:       H-MODE
Level:      3 — Digital Void (Final H-MODE Puzzle)
Type:       Divided attention + multi-stream pattern extraction
Difficulty: ★★★★★
Time Limit: 4 seconds per stream (3 streams = 12s total)
```

**In-World Context:**
Kael is finally standing at the door to his deepest memory — the moment before the procedure. But AXIOM corrupted it across three simultaneous memory streams to prevent recovery. The key is hidden across all three. Kael must hold all three in his mind at once and extract the correct sequence.

**PROCESS output:** `PARALLEL_MEMORY_STREAMS: 3. PROCESSING_CAPACITY_EXCEEDED. CANNOT_PRIORITIZE.`  
*(This is the one thing PROCESS genuinely cannot do — hold emotional parallel attention.)*

**What the Player Sees:**
Three memory fragment windows play simultaneously (split screen, 3 columns).

Each fragment is a 4-second clip of a memory scene. Somewhere in each scene, ONE symbol is visible (on a wall, a screen, a piece of paper):
- Stream 1 (Kael's apartment): Symbol appears at second 2.5 on a coffee mug
- Stream 2 (The AXIOM lab): Symbol appears at second 1.0 on a security badge
- Stream 3 (Outside the Undercity): Symbol appears at second 3.2 on a neon sign

Symbols: ◈ / ∇ / ⬡ (from NEX//ION's in-world glyph alphabet)

After 4 seconds, all streams end. Player is shown a 3-slot input:
```
[_] [_] [_]
```
Must input the symbols IN ORDER OF APPEARANCE (chronological, not by stream number).

**Solution:**
- Stream 2 symbol (◈) at 1.0s
- Stream 1 symbol (∇) at 2.5s
- Stream 3 symbol (⬡) at 3.2s
→ Input: `◈ ∇ ⬡`

**Why this is H-MODE:** Divided attention across parallel emotional streams is a uniquely human cognitive capacity. The CPU processes streams sequentially; humans can hold context across simultaneous emotional inputs.

**Win Condition:** Correct sequence unlocks Kael's core memory — the full, uncorrupted version of who he was before the procedure. Major narrative beat.

**Fail State:** One wrong symbol = memory corrupts further. Can retry but each retry shows slightly more glitch distortion on the streams.

**Unity Notes:**
- Three `RenderTexture` video streams running simultaneously (VideoPlayer components or Animator-driven scene cameras)
- Symbol appears via `SetActive(true)` on a child GameObject at specific timestamps
- 3-slot input: UI Toolkit with glyph buttons
- Correct order stored in `FinalMemorySO.symbolOrder[]`
- On success: trigger `MemoryRevealCinematic` (in-engine cutscene)

---

## C-MODE PUZZLES
> CPU-mode puzzles are derived from real CTF (Capture The Flag) challenge types: binary, hex, cipher, network, and logic.
> These represent what Kael's human brain cannot do unaided — raw computation, encoding, and systems manipulation.

---

### C-01 — Binary Door
```
Mode:       C-MODE
Level:      1 — Neon Undercity
Type:       Binary → Decimal conversion
Difficulty: ★☆☆☆☆
Time Limit: 45 seconds
```

**In-World Context:**
A basement door runs on an old digital lock from the 2030s. The keypad only displays raw binary. PROCESS reads it instantly. Kael's human side has to let PROCESS guide his fingers to type the answer.

**What the Player Sees:**
A terminal panel displays:
```
> LOCK_CODE: 01000011
> ENTER_DECIMAL_ACCESS_KEY: ___
```

**Solution:**
`01000011` in binary = `67` in decimal.
(64 + 2 + 1 = 67)

Player types `67` and presses Enter.

**Hint (available after 30s):** "Each 1 doubles from right to left: 1, 2, 4, 8, 16, 32, 64, 128."

**Fail State:** Wrong entry → 10s lockout, then retry. 3 lockouts = alarm triggered.

**Design Escalation:** Level 1 uses 8-bit values (max 255). Level 2 variants use 2-byte values (16-bit).

**Unity Notes:**
- `TMP_InputField` for digit input
- Validate on `KeyCode.Return`
- Binary value generated from `int` to `Convert.ToString(value, 2).PadLeft(8, '0')`
- Store correct decimal in `PuzzleSO.correctAnswer`

---

### C-02 — Hex Status Scan
```
Mode:       C-MODE
Level:      1 — Neon Undercity
Type:       Hex color/value recognition
Difficulty: ★★☆☆☆
Time Limit: 20 seconds
```

**In-World Context:**
A surveillance panel shows 9 CCTV feeds. Each camera's status is tagged with a hex color code. PROCESS knows that `#FF0000` (full red) means an active alert — a camera that has been triggered. Must identify it among the safe `#00FF00` (green) cameras before the alert propagates.

**What the Player Sees:**
A 3×3 grid of camera feed thumbnails. Each has a colored status dot with its hex code displayed underneath:
```
#00FF00  |  #00FF00  |  #00CC00
#33FF00  |  #FF0000  |  #00FF33
#00FF00  |  #00EE00  |  #00FF00
```

Player must click the camera with `#FF0000`.

**Why not trivial:** Several shades of green look similar at a glance. Only one is pure `#FF0000`. The distraction: a `#EE0000` decoy in some variants.

**Solution:** Center camera (position [1,1]).

**Fail State:** Click wrong camera → alert propagates to 3 more cameras → harder follow-up puzzle to shut them all down.

**Unity Notes:**
- Camera thumbnails: `Image` components with `color` set from hex codes
- Hex label: `TextMeshPro` child component
- Player click: `Button.onClick` checks `imageComponent.color == Color.red` (within tolerance)
- Randomize grid layout per play from a `HexStatusDataSO`

---

### C-03 — ROT13 Terminal
```
Mode:       C-MODE
Level:      1 — Neon Undercity
Type:       Classical cipher (ROT13 / Caesar shift)
Difficulty: ★★☆☆☆
Time Limit: 40 seconds
```

**In-World Context:**
An informant left a message on a dead terminal using ROT13 — an old encryption standard, barely encryption at all. PROCESS identifies the encoding type in milliseconds but requires Kael's hands to type the decoded output.

**What the Player Sees:**
```
> ENCRYPTED_MESSAGE_RECEIVED:
> BCRA GUR QBBE
>
> CIPHER_DETECTED: ROT13
> DECODE AND ENTER PLAINTEXT:
> ___
```

**ROT13 Key:**
```
A→N, B→O, C→P, D→Q, E→R, F→S, G→T, H→U, I→V, J→W, K→X, L→Y, M→Z
N→A, O→B, P→C, Q→D, R→E, S→F, T→G, U→H, V→I, W→J, X→K, Y→L, Z→M
```

**Solution:**
`B→O, C→P, R→E, A→N, G→T, U→H, R→E, Q→D, B→O, B→O, E→R`
`BCRA GUR QBBE` → `OPEN THE DOOR`

**Hint (after 25s):** ROT13 reference table displayed on screen.

**Fail State:** After 3 wrong attempts, terminal displays `CONNECTION_LOST` — must find alternate entry.

**Design Escalation:**
- Level 1: ROT13 (shift 13, self-inverse)
- Level 2 variant (C-03-B): Caesar cipher with unknown shift — player must brute-force the shift (1–25) with a limited attempt budget

**Unity Notes:**
- `TMP_InputField` with uppercase enforcement
- Validate against `ROT13.Decode(encodedString)` (static utility method)
- Display cipher reference table as `HintPanel` that fades in after `hintDelay` seconds

---

### C-04 — Logic Gate Lock
```
Mode:       C-MODE
Level:      2 — Corporate Spire
Type:       Boolean logic / combinational circuit
Difficulty: ★★★☆☆
Time Limit: 30 seconds
```

**In-World Context:**
A corporate server room uses a physical-digital hybrid lock: two switches control a logic gate circuit. OUTPUT must equal `1` (HIGH) to unlock. PROCESS analyzes the circuit diagram and projects the correct switch combination.

**What the Player Sees:**
A circuit diagram with:
- 2 input switches: A and B (toggle ON/OFF = 1/0)
- 3 gates: NOT(A) → AND(NOT_A, B) → OR(AND_result, A)
- Output indicator: `LOCK_STATE: [0]`

Truth table for the circuit:

| A | B | NOT_A | AND(NOT_A, B) | OR(result, A) = OUTPUT |
|---|---|---|---|---|
| 0 | 0 | 1 | 0 | 0 |
| 0 | 1 | 1 | 1 | 1 |  ← SOLUTION
| 1 | 0 | 0 | 0 | 1 |  ← also valid
| 1 | 1 | 0 | 0 | 1 |  ← also valid

**Solution:** Any of: `(A=0, B=1)`, `(A=1, B=0)`, `(A=1, B=1)` produces OUTPUT=1.

Player toggles A and B, observes output in real time, selects a valid combination, hits CONFIRM.

**Design Escalation:**
- Level 2: 3 gates, 2 inputs
- Level 3 variant (C-04-B): 5 gates, 3 inputs, NAND and XOR gates added, only ONE valid combination

**Unity Notes:**
- Toggle components update `bool A, bool B` values
- On any toggle change: re-evaluate `LogicCircuit.Evaluate(A, B)` and update output indicator
- Circuit rendered as `LineRenderer` connections between `GateNode` GameObjects
- `GateNode.Evaluate()` is an abstract class implemented by `ANDGate`, `ORGate`, `NOTGate`, `NANDGate`, `XORGate`

---

### C-05 — Node Route
```
Mode:       C-MODE
Level:      2 — Corporate Spire
Type:       Constrained pathfinding / network routing
Difficulty: ★★★☆☆
Time Limit: 20 seconds
```

**In-World Context:**
A corporate internal network. PROCESS must route a data packet from Kael's position (NODE_ALPHA) to the target server (NODE_OMEGA) without touching firewalled nodes or honeypots. Touching a honeypot triggers a trace.

**What the Player Sees:**
A 5×5 grid of nodes. Each node is either:
- `[ ]` — OPEN (safe to route through, shown in cyan)
- `[F]` — FIREWALL (blocked, shown in red, cannot pass)
- `[H]` — HONEYPOT (shown in amber, passing = trace triggered)
- `[S]` — SOURCE (start, shown in white)
- `[T]` — TARGET (end, shown in gold)

Example grid:
```
[S][ ][ ][F][ ]
[ ][H][F][ ][ ]
[ ][ ][ ][ ][F]
[F][ ][H][ ][ ]
[ ][ ][ ][ ][T]
```

Player clicks a path of nodes from S to T. Path lights up as they click.

**Solution:** One or two valid paths exist that avoid all `[F]` and `[H]` nodes.

**Fail State:** Click a Honeypot = trace meter fills 25%. Fill 100% = security lockdown.

**Design Escalation:**
- Level 2: 5×5, 1-2 valid paths
- Level 3 variant (C-05-B): 7×7, firewall nodes pulse (open/closed on timer), only 1 valid path exists at any given moment

**Unity Notes:**
- Grid: 2D array of `NodeTile` components
- Pathfinding: BFS/DFS to validate if selected path is valid
- Path visualization: `LineRenderer` connecting clicked nodes
- Confirm button: validates path, checks no forbidden nodes in selection

---

### C-06 — Base64 Intercept
```
Mode:       C-MODE
Level:      2 — Corporate Spire
Type:       Encoding recognition + Base64 decode
Difficulty: ★★★★☆
Time Limit: 40 seconds
```

**In-World Context:**
PROCESS intercepts a transmission from an AXIOM internal channel. The data is encoded. Before PROCESS can decode it, Kael must confirm the encoding type — a deliberate security redundancy that requires active input to prevent automated bypass.

**What the Player Sees:**
```
> INTERCEPTED_TRANSMISSION:
> QVhJT01fS0VZOiBORVhJT04=
>
> IDENTIFY ENCODING TYPE:
> [ ] HEX    [ ] ROT13    [ ] BASE64    [ ] BINARY
>
> CONFIRM ENCODING → THEN ENTER DECODED VALUE:
> ___
```

**How to identify Base64:**
- Character set is A-Z, a-z, 0-9, `+`, `/`, `=`
- Always ends with `=` or `==` (padding)
- Length is always a multiple of 4

**Solution:**
Encoding = `BASE64`
Decoded: `QVhJT01fS0VZOiBORVhJT04=` → `AXIOM_KEY: NEXION`

**Why this is harder:** Player must know what Base64 looks like to select the right encoding type first, THEN type the decoded value. Two-step.

**Hint (after 25s):** "Check the character set. Pure letters + numbers + + / = is a Base64 signature."

**Unity Notes:**
- Step 1: RadioButton group for encoding type
- Step 2 (unlocks after step 1): `TMP_InputField`
- Validate: `System.Convert.FromBase64String()` → compare to `correctPlaintext`
- Encoding type wrong = hint displayed, try again

---

### C-07 — Packet Forge
```
Mode:       C-MODE
Level:      2 — Corporate Spire
Type:       Hex manipulation + checksum recalculation (XOR)
Difficulty: ★★★★☆
Time Limit: 45 seconds
```

**In-World Context:**
A data packet is in transit to AXIOM's server. PROCESS must intercept it, change the routing flag byte to redirect it to Kael's drop node, and recalculate the packet's checksum so it passes validation. Tampering with the checksum is where most hackers fail.

**What the Player Sees:**
A packet header displayed as hex bytes:
```
HEADER:  [4E] [45] [58] [49] [4F] [4E]
FLAGS:   [02]    ← Must change to [07] to reroute
CHKSUM:  [??]    ← Must recalculate after changing FLAGS
```

**Checksum rule (displayed in C-MODE data overlay):**
`CHKSUM = XOR of all bytes in HEADER + new FLAGS byte`

**Calculation:**
`4E XOR 45 XOR 58 XOR 49 XOR 4F XOR 4E XOR 07`
= Step through XOR:
- `4E XOR 45 = 0B`
- `0B XOR 58 = 53`
- `53 XOR 49 = 1A`
- `1A XOR 4F = 55`
- `55 XOR 4E = 1B`
- `1B XOR 07 = 1C`
→ CHKSUM = `1C`

**Solution:** Change `[02]` → `[07]`, then enter `1C` for CHKSUM.

**Hint (after 30s):** XOR table displayed as a reference.

**Fail State:** Wrong checksum = packet rejected = target server raises alert.

**Unity Notes:**
- Hex byte fields: `TMP_InputField` with hex validation (0-9, A-F, max 2 chars)
- Live checksum preview: recalculates as player types the FLAGS value
- Validate on CONFIRM: check both FLAGS and CHKSUM values
- XOR computed via `int result = 0; foreach(byte b in bytes) result ^= b;`

---

### C-08 — Firewall Weave
```
Mode:       C-MODE
Level:      3 — Digital Void
Type:       Timing + pattern recognition under threat (adapted from "Welcome to the Game II" firewall mechanic)
Difficulty: ★★★★☆
Time Limit: Trace meter (fills if player is caught)
```

**In-World Context:**
The Digital Void's outer layer is guarded by AXIOM's active firewall — a rhythmic pulse of blocked nodes. The firewall doesn't STOP movement; it PUNISHES movement through blocked nodes. PROCESS can calculate the timing pattern if given enough cycles to observe.

**What the Player Sees:**
A 1D corridor of 8 nodes (horizontal). Each node alternates between OPEN (cyan glow) and BLOCKED (red barrier) on a cycle. The timing is not uniform — each node has a different cycle speed:

```
Node: [1]  [2]  [3]  [4]  [5]  [6]  [7]  [8]
Cycle: 1s   2s   1s   3s   1s   2s   1s   2s
```

Player controls a cursor (representing the data packet). Must move right through all 8 nodes. Moving into a BLOCKED node fills the trace meter by 20%.

**Trace Meter:** At 100%, AXIOM lockdown triggers. 5 hits = fail.

**Solution:** Observe 2 cycles first (auto-pause option available), then thread through during the correct timing window. A valid all-open window exists every ~6 seconds.

**Why this escalates:** In the Digital Void version, some nodes have RANDOMIZED cycle additions — they're open/closed based on the pattern, PLUS a random +0.2s variance that PROCESS must account for (and cannot, because it's random noise — another thing humans handle with reaction time better than prediction).

**Unity Notes:**
- `NodeState` coroutine: `while(true) { isOpen = !isOpen; yield return new WaitForSeconds(cycleTime[i]); }`
- Cursor movement: `Input.GetKeyDown(KeyCode.RightArrow)` moves to next node
- Collision check: if `cursor.currentNode.isOpen == false → traceMeter += 20f`
- Trace bar: `Image.fillAmount = traceMeter / 100f`

---

### C-09 — Process Inject
```
Mode:       C-MODE
Level:      3 — Digital Void
Type:       Process manipulation / instruction injection
Difficulty: ★★★★★
Time Limit: 3 attempts (each attempt = one 6-second cycle)
```

**In-World Context:**
An AXIOM security process is running in the Void — a loop of 8 instructions executing repeatedly. PROCESS must inject a new `REDIRECT` instruction between steps 4 and 5 of the cycle to hijack the process's output. Inject too early or too late and the process detects the intrusion.

**What the Player Sees:**
A vertical instruction stack, animating downward like a pipeline:
```
CYCLE RUNNING...
→ [1] FETCH_KEY
→ [2] VALIDATE_AUTH
→ [3] LOAD_PAYLOAD
→ [4] CHECK_INTEGRITY   ← INJECT WINDOW (0.8s)
  [5] ROUTE_OUTPUT
  [6] LOG_ACCESS
  [7] CLEAR_BUFFER
  [8] LOOP
```

A green `[INJECT]` button is available at all times. The injection is only VALID if pressed while instruction `[4] CHECK_INTEGRITY` is the active (highlighted) instruction.

Each instruction is active for 0.75 seconds. The inject window is 0.8s (slight forgiveness).

The player watches the cycle run once (6 seconds) to learn the timing, then must press `[INJECT]` at the right moment on the next cycle.

**Solution:** Press `[INJECT]` between 2.25s and 3.0s into each cycle (when instruction 4 is active).

**Fail State:** Early injection → process detects → cycle resets, attempt lost. Late injection → same.

**Why it's hard:** The cycle speed varies slightly each loop (±0.05s jitter — random noise that PROCESS cannot account for with pure calculation). Human reaction timing with practiced feel is the solution.

**Unity Notes:**
- `activeInstruction` cycles 0–7 via coroutine with `WaitForSeconds(0.75f + Random.Range(-0.05f, 0.05f))`
- Highlight active instruction: `instructions[i].color = Color.green`
- `[INJECT]` click: check `activeInstruction == 3` (0-indexed) → success or failure
- 3 attempts tracked via `attemptsRemaining` int; on 0 → full fail state

---

### C-10 — Cipher Chain
```
Mode:       C-MODE
Level:      3 — Digital Void (Final C-MODE Puzzle)
Type:       Multi-layer decryption chain
Difficulty: ★★★★★
Time Limit: 90 seconds across 4 stages
```

**In-World Context:**
AXIOM's master override key is the final gate before the confrontation with Dr. Vera Kade. AXIOM encrypted it in four layers — a deliberate "onion" to prevent automated decryption. PROCESS knows ALL four algorithms. The challenge is sequencing them correctly and executing each step in order. One wrong step at any layer corrupts the key irreversibly.

**The Encrypted Value:**
```
AXIOM_OVERRIDE_ENCRYPTED: 53 42 67 45 47 52 52 42 30 55 41 41 3d 3d
```

**What the Player Sees:**
Stage display:
```
LAYER 1 OF 4 — IDENTIFY ENCODING
LAYER 2 OF 4 — HEX → ASCII
LAYER 3 OF 4 — BASE64 DECODE
LAYER 4 OF 4 — XOR DECRYPT (KEY: 0x4E)
```

**Stage 1 — Identify the outermost encoding:**
The string `53 42 67 45 47 52 52 42 30 55 41 41 3d 3d` looks like hex.
Player selects: `[ ] BASE64  [ ] HEX  [ ] BINARY  [X] HEX`

**Stage 2 — Hex → ASCII:**
Convert each hex pair to its ASCII character:
```
53=S, 42=B, 67=g, 45=E, 47=G, 52=R, 52=R, 42=B, 30=0, 55=U, 41=A, 41=A, 3d==, 3d==
```
Result: `SBgEGRRB0UAA==`

Player types: `SBgEGRRB0UAA==`

**Stage 3 — Base64 Decode:**
`SBgEGRRB0UAA==` in Base64 → raw bytes: `48 18 04 19 14 41 D1 40 00`
Display as: `48 18 04 19 14 41 D1 40 00`

Player confirms the decoded bytes (displayed, no typing needed — just `[PROCEED]`).

**Stage 4 — XOR Decrypt with key `0x4E`:**
XOR each byte with `0x4E`:
```
48 XOR 4E = 06
18 XOR 4E = 56  (V)
04 XOR 4E = 4A  (J)
19 XOR 4E = 57  (W)
14 XOR 4E = 5A  (Z)
41 XOR 4E = 0F
D1 XOR 4E = 9F
40 XOR 4E = 0E
00 XOR 4E = 4E  (N)
```

Result after filtering printable ASCII: `VJWZx4N` — the override key.

Player types: `VJWZ` (simplified for game — only the 4 printable chars required).

**Final:** AXIOM system unlocked. Dr. Kade's position exposed. End of C-MODE challenges.

**Why it's the hardest:** 4 sequential stages. A wrong answer at any stage corrupts the chain. First two stages require knowledge; stages 3-4 require computation. 90s across all 4 stages.

**Unity Notes:**
- `CipherChainManager` with `currentStage int` (0–3)
- Each stage: separate UI panel with its own input/validation
- On stage complete: `currentStage++`, animate transition (panel slides)
- Stage timeout tracked cumulatively: all 4 stages share the 90s pool
- Fail at any stage → `remainingTime -= 20f` (penalty) + stage resets

---

## HYBRID PUZZLES
> These require active use of BOTH modes — either simultaneously or in tightly sequenced alternation.
> These award HYBRID TOKENS (◈) on completion.

---

### HB-01 — Signal + Shadow
```
Mode:       HYBRID (H-MODE → C-MODE)
Level:      2 — Corporate Spire
Difficulty: ★★★★☆
Time Limit: 60 seconds total
```

**Phase 1 (H-MODE, 30s):**
Kael spots a blinking light pattern on a distant building through a window. Human intuition recognizes it as Morse code. Must decode: `... --- ...` = SOS (3 shorts, 3 longs, 3 shorts).
But the FULL message is longer: `... --- ... / ..-. .. ...- . / .---- .----` = SOS FIVE 11
That's the server number: `SERVER_5_11`.

**Phase 2 (C-MODE, 30s):**
Switch to C-MODE. Navigate the node routing grid to reach `SERVER_5_11` specifically — not just any server. The grid has 12 server endpoints, only one is `5_11`. Routing to the wrong server loses the lead.

**Why it's hybrid:** Morse code is a human pattern task (rhythm + recognition). Node routing is a CPU task (graph traversal). Neither mode alone can solve both phases.

**Unity Notes:**
- Phase 1: Animated light on a building object — `Coroutine` flickers `PointLight` on/off per Morse timing
- Morse decoder: player clicks `[DOT]` and `[DASH]` buttons, auto-decoded by `MorseDecoder.cs`
- Phase 2: Standard node routing grid (C-05 system) with specific target node labeled `5_11`

---

### HB-02 — Guard + Ghost
```
Mode:       HYBRID (Simultaneous)
Level:      2 — Corporate Spire
Difficulty: ★★★★★
Time Limit: 90 seconds
```

**The Challenge:**
Kael must cross a secured corridor while simultaneously running a timed background hack.

**H-MODE thread (physical world):**
Stealth movement through a guarded corridor. 3 guards on patrol. Player controls movement in H-MODE — warm visual filter, physical world visible.

**C-MODE thread (background hack):**
A progress bar in the corner represents a server decrypt running autonomously (simulated by a timer). BUT every 15 seconds, the server demands a manual input: a quick logic gate puzzle (C-04 mini version, 10s window). If the player doesn't switch to C-MODE and solve it in time, the decrypt aborts.

**The Conflict:**
Switching to C-MODE for 10 seconds means going physically blind (H-MODE input stops). Guards can move while you're in C-MODE. You must find a safe alcove to duck into BEFORE switching, or risk being caught mid-mode.

**Solution:** Read the guard pattern (H-MODE), duck into alcove, switch to C-MODE, solve the mini gate puzzle in under 10s, switch back, continue crossing.

**Unity Notes:**
- `ModeManager` blocks player movement input when `currentMode == CPU_MODE`
- Guards continue on `NavMeshAgent` paths regardless of mode
- Hack progress: `Coroutine` that ticks progress and fires `OnInputRequired` event every 15s
- `OnInputRequired`: spawn mini C-04 panel as overlay, 10s countdown

---

### HB-03 — Kael's Last Memory
```
Mode:       HYBRID (Sequential — final puzzle of the game)
Level:      3 — Digital Void
Difficulty: ★★★★★
Time Limit: Untimed (this is the final narrative moment)
```

**Context:**
The game's final puzzle. Dr. Vera Kade has sealed herself behind the ultimate gate — Kael's own identity. The lock is built from his memory (human) and the ION-7 encryption key (digital). Only a being that is both can open it.

**Phase 1 — H-MODE:**
Kael must answer three questions about his own memories correctly. These answers were seeded by memory fragments collected throughout the game:
- "What was the first thing you said to PROCESS?" → Answer in Kael's collected memory fragment from Level 1
- "What is your sister's name?" → Fragment collected in Level 2
- "Why did you break into AXIOM?" → Fragment from Level 1

Correct answers open the emotional lock.

**Phase 2 — C-MODE:**
PROCESS provides the raw ION-7 encryption key it has been generating since Level 1 — built from all the memory data it has processed. Player inputs the key at a terminal. Key is displayed in C-MODE data overlay (rewarding players who paid attention to PROCESS's dialogue).

**Phase 3 — Joint (automatic):**
If both phases succeed: Kael speaks his name aloud (H-MODE) and PROCESS completes the authentication (C-MODE) simultaneously. The door opens.

**Win Condition:** Access granted. Confrontation with Dr. Kade begins. The two halves — for the first time — acted as one.

**Unity Notes:**
- Memory answers: tracked in `GameState.collectedMemories Dictionary<string, string>`
- Answers were saved automatically when fragments were collected
- ION-7 key: string value built incrementally in `PROCESSDataManager` across all 3 levels
- Phase 3 sequence: scripted cutscene trigger → plays `UnifiedSequence` animation

---

## IMPLEMENTATION PRIORITY ORDER

For Unity development, build in this order:

```
SPRINT 1 — Core puzzle framework:
  1. C-01 (Binary Door) — simplest input validation loop
  2. H-01 (Neon Sequence) — simplest pattern display
  3. C-03 (ROT13 Terminal) — text input + cipher utility

SPRINT 2 — Grid systems:
  4. C-05 (Node Route) — grid + pathfinding (reused in HB-01)
  5. C-04 (Logic Gate) — gate eval system (reused in HB-02)
  6. H-03 (Silhouette ID) — sprite comparison system

SPRINT 3 — Memory + timing:
  7. H-02 (Glyph Recall) — sequence memory (reused in H-05, H-10)
  8. H-05 (Memory Fragment Scan) — RenderTexture + recall grid
  9. C-08 (Firewall Weave) — timing system + trace meter

SPRINT 4 — Complex + Hybrid:
  10. C-09 (Process Inject) — reaction timing + cycle system
  11. H-08 (Stroop Override) — fast succession panel system
  12. HB-01, HB-02 — compose from existing systems

SPRINT 5 — Final puzzles:
  13. C-10 (Cipher Chain) — multi-stage manager
  14. H-10 (Parallel Memory Pull) — multi-stream video + input
  15. HB-03 (Kael's Last Memory) — narrative integration
```

---

*Document version: 1.0 | Project NEX//ION | ARX Studios | 2026-08-25*
*Do not ship puzzle solutions publicly — keep this file server-side / internal only.*
