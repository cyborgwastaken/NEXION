# NEX//ION — Visual Design System

> Formalizes the "Visual Language" section of `nexion_synopsis_context.md` into exact values: hex palette, material recipes, VFX rules, and concrete HDRP Volume Profile settings. This is the reference every future Shader Graph, VFX Graph, and Volume Profile should pull from — don't eyeball colors ad hoc, pull from this table.
>
> Companion files: [guide.md](guide.md) Session 1 / Step 5 plugs these numbers directly into the Editor. [steps.md](steps.md) Phase 4 tracks the remaining art-pipeline work this unblocks.

Last updated: 2026-08-26

---

## 1. Palette

| Name | Hex | Role |
|---|---|---|
| Void Black | `#0A0A0A` | Base world background, unlit shadow areas, UI background |
| Deep Purple | `#1A0030` | Ambient fog tint, distant fill light, secondary shadow color |
| Signal Cyan | `#00FFFF` | C-MODE primary emissive — data overlays, terminal glow, ION-7/PROCESS visual motif |
| Alarm Magenta | `#FF00FF` | AXIOM corporate signage, danger/alert states, rare high-contrast accents — used sparingly, it should read as "corporate/hostile" against the cyan/amber duality |
| Human Amber | `#FF8A3D` | H-MODE primary tint — practical lighting, Sable's shop, Undercity neon signage warmth |
| Corrupt Green | `#39FF6A` | Glitch accents, memory-fragment corruption visuals, secondary C-MODE data color for variety |

**Usage rule:** Cyan and Amber are the two "home" colors — they belong to CPU and Human respectively and should almost never mix in the same shot except during Hybrid-mode moments (where their overlap *is* the point). Magenta is AXIOM's color — reserve it for their tech, signage, and the antagonist's presence so it reads as "other." Green is accent-only, never a base tint.

---

## 2. Typography

- Monospace only, everywhere diegetic: **Source Code Pro** or **JetBrains Mono** (doc's original spec — keep it).
- Terminal/HUD text: Signal Cyan `#00FFFF` on Void Black `#0A0A0A`, per classic terminal convention.
- H-MODE dialogue/subtitle text (if any renders as UI rather than diegetic): Human Amber `#FF8A3D`.

---

## 3. Material recipes

### Neon emissive (tubes, signage, terminal screens)
- HDRP Lit shader, Emissive enabled.
- Emission color: pull from palette table above (Cyan for C-MODE tech, Amber for Undercity practicals, Magenta for AXIOM signage).
- Emission intensity: HDR, roughly **8–15** depending on how "hot" the light should read against Void Black surroundings — start at 10 and tune per-scene.
- Base albedo: near-black or dark gray so the tube itself doesn't compete with its own glow.

### Wet reflective floor
- HDRP Lit shader.
- Metallic: 0 (it's wet concrete/tile, not metal).
- Smoothness: **0.85–0.95** — high enough for sharp neon reflections.
- Enable **Screen Space Reflections** on the HDRP Volume (see Section 5, `VP_WorldDefault`) so the smoothness actually produces visible reflections.
- Optional: a subtle normal map with puddle/ripple detail if you want reflections to distort slightly rather than mirror-perfect.

### Volumetric fog
- Purple-tinted per the palette (Deep Purple `#1A0030`), low density in open areas, denser in Undercity alleys.
- Configured via the Volume's **Fog** override — see `VP_WorldDefault` below.

---

## 4. VFX language (glitch system — implements later, in steps.md Phase 4)

Glitch effects mark **narrative beats**, not continuous ambience — overusing them numbs the effect. Reserve the strong versions for: memory fragment recovery, PROCESS processing something emotionally significant, proximity to AXIOM tracking, and story-critical corruption moments.

| Effect | When | Intensity |
|---|---|---|
| Chromatic aberration burst | Memory fragment pickup, narrative stingers | Short spike (~0.3s), high intensity, decays to baseline |
| Pixel displacement | PROCESS distress / corrupted memory sequences | Localized to UI or a specific surface, not full-screen |
| Scan lines | Sustained C-MODE data-space areas (Digital Void especially) | Low, constant, textural — implement as a Custom Pass or screen-space overlay shader, not a Volume override |

Baseline (non-narrative) chromatic aberration and vignette differences between H-MODE and C-MODE are handled by the Volume Profiles in Section 5 — those are the *ambient* mode feel; the table above is *event-driven* on top of that.

---

## 5. HDRP Volume Profile settings

Three profiles. `VP_WorldDefault` is always active (weight 1, global) and represents the game's neutral baseline look. `VP_HumanMode` and `VP_CPUMode` are the two profiles `ModeVisualController` crossfades in from weight 0, per guide.md Step 5.

### VP_WorldDefault (baseline — create this first, weight 1, always on)

| Override | Setting | Value |
|---|---|---|
| Color Adjustments | Contrast | `+10` |
| Color Adjustments | Saturation | `-5` |
| Vignette | Intensity | `0.25` |
| Vignette | Smoothness | `0.4` |
| Vignette | Color | `#05000A` |
| Fog | Enable Volumetric Fog | on |
| Fog | Color | `#1A0030` |
| Fog | Attenuation Distance | `~50m` (tune per level) |
| Bloom | Threshold | `1.0` |
| Bloom | Intensity | `0.3` |
| Screen Space Reflection | Enabled | on (needed for wet-floor material above) |

### VP_HumanMode (Q held)

| Override | Setting | Value |
|---|---|---|
| Color Adjustments | Post Exposure | `+0.1` |
| Color Adjustments | Contrast | `-5` (softer than baseline) |
| Color Adjustments | Color Filter | `#FFEBD1` (warm near-white) |
| Color Adjustments | Saturation | `+8` |
| Vignette | Intensity | `0.35` |
| Vignette | Smoothness | `0.6` |
| Vignette | Color | `#2B1200` |
| Chromatic Aberration | Intensity | `0.05` (barely there — organic, not glitchy) |
| Film Grain | Intensity | `0.15` (adds tactile/alive texture per doc) |

### VP_CPUMode (E held)

| Override | Setting | Value |
|---|---|---|
| Color Adjustments | Post Exposure | `-0.05` |
| Color Adjustments | Contrast | `+20` (sharper, digital) |
| Color Adjustments | Color Filter | `#C9FFFF` (cool cyan-tinted white) |
| Color Adjustments | Saturation | `-20` (desaturated — accent color comes from world-space data overlays, not post-process tint) |
| Vignette | Intensity | `0.2` |
| Vignette | Smoothness | `0.3` |
| Vignette | Color | `#001A1A` |
| Chromatic Aberration | Intensity | `0.25` (the "glitchy data" edge feel) |
| Bloom | Intensity | `0.6` |
| Bloom | Threshold | `0.8` |
| Lens Distortion | Intensity | `0.05` (optional — faint screen-curvature, "looking through a HUD" feel) |

All values are starting points, not gospel — tune once you can see them in the actual level lighting. If a value feels wrong in-editor, change it here too so this doc stays the source of truth.
