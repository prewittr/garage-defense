# [FEEL-SPEC-RPM-001] Sprint 1 Feel Layer — Tension/Release Loop

**Author:** Malik Ransom (`juice-vfx-engineer`)
**Date:** 2026-04-18
**Status:** Code-complete; Editor-deferred wiring tracked in [tickets/RPM-001.md](../../../tickets/RPM-001.md) Notes
**Linked design:** [DESIGN-001 Garage View](../DESIGN-001-garage-view.md)
**Linked story:** [tickets/RPM-001.md](../../../tickets/RPM-001.md) (paired-mode, code by Kendra Brooks)
**Owner-paired-with:** Kendra Brooks (`performance-hardener`)

---

## Pillar mapping

This slice serves all three North Stars:

1. **The 10-Second Hook** — every clip-ready moment lives or dies on these five controllers firing in concert. Without them the demo reel is silent geometry.
2. **High-Stakes Resource Management** — screen shake amplitude scales with damage severity, so the player *feels* HP draining. Audio pitch jitter prevents repetition fatigue across the 60s loop.
3. **Tension → Release** — the weld-spark + weld-audio pair *is* the relief beat. The 1.0–1.5s post-weld silence (DESIGN-001 §Moment-to-Moment) only lands if the weld's audiovisual punch is loud enough to *miss* when it stops.

---

## Architecture seam

All five controllers subscribe to `Rpm.Core.Events.IEventBus` (Kendra's seam, `Singleton`). They never reference `Rpm.Gameplay.*` directly — module map in [ARCHITECTURE.md §2](../../../architecture/ARCHITECTURE.md) stays clean: `Rpm.Juice → Rpm.Core` only.

```
DamageEvent  ──►  ScreenShakeController  (visual; ReduceMotion-suppressed)
            ──►  DustFallController     (visual; always plays)
            ──►  ImpactAudio            (audio; always plays)

RepairEvent  ──►  WeldSparkController    (visual; SRP-batched)
            ──►  WeldAudio              (audio)
```

Subscribe in `OnEnable`, unsubscribe in `OnDisable`. Handler delegates are cached fields and capture nothing → zero GC alloc on the event hot path.

---

## ScreenShakeController

| Field | Spec |
|---|---|
| **Trigger** | `IEventBus.Subscribe<DamageEvent>` |
| **Duration** | `ScreenShakeConfig.Duration` (default 0.4s) |
| **Curve shape** | Critically-damped spring (omega = 20 rad/s, damping ratio 1.0) layered with designer `AnimationCurve` decay (default `EaseInOut(0,1, 1,0)`) |
| **Direction** | Perlin-noise wobble at `NoiseFrequency` (default 22 Hz) on local X/Y |
| **Amplitude** | `Amount * 4 * curveScale * MaxAmplitude` — clamped at the spring intensity ceiling 1.0 |
| **Haptic pattern** | `HAPTIC_Impact` from DESIGN-001 §Haptic Curves (intensity = `1 - HP/MaxHP`, ReduceMotion-gated) — *handled by Rpm.Input layer; this controller does visuals only* |
| **Audio cue** | None directly (paired with `ImpactAudio`) |
| **Perf budget** | 1 Vector3 write per Update; 2 Perlin samples; 0 alloc |
| **Designer knobs** | `AnimationCurve amplitudeDecay`, `float maxAmplitude`, `float springOmega`, `float duration`, `float noiseFrequency` (all on `ScreenShakeConfig` ScriptableObject) |
| **Accessibility** | `AccessibilityFlags.ReduceMotion == true` → camera held at rest position, spring math still ticks (no snap-pop on toggle mid-session) |

**One-sentence summary:** Camera shakes harder the harder you're hit, decays through a critically-damped spring, and stops moving entirely when Reduce Motion is on without dropping any audio.

---

## DustFallController

| Field | Spec |
|---|---|
| **Trigger** | `IEventBus.Subscribe<DamageEvent>` |
| **Duration** | Particle lifetime ~1.2s (DESIGN-001 §VFX) |
| **Curve shape** | Vertical fall, gravity 0.3 (configured on the ParticleSystem prefab) |
| **Burst size** | 7 particles per impact (DESIGN-001 §VFX spec: 6–8) |
| **Pool** | Shared `ParticleSystem` internal pool — `Stop Action = None`, `Play On Awake = false`, `maxParticles` sized for 2 simultaneous bursts |
| **Audio cue** | Particle hiss layer in audio mix (Sprint 3 polish) |
| **Perf budget** | One `Emit(int)` call per impact; 0 managed alloc |
| **Designer knobs** | `ParticleSystem` reference, `_particlesPerBurst` (clamped 0–32) |
| **Accessibility** | Plays under ReduceMotion — DESIGN-001 §Accessibility Baseline carve-out: dust does not trigger vestibular discomfort |

**One-sentence summary:** Rafter dust falls every impact and keeps falling even when Reduce Motion silences the camera shake.

---

## WeldSparkController

| Field | Spec |
|---|---|
| **Trigger** | `IEventBus.Subscribe<RepairEvent>` |
| **Duration** | Particle lifetime ≤ 400ms (DESIGN-001 §VFX) |
| **Curve shape** | Additive-blend spark scatter at coord; configured on the ParticleSystem prefab |
| **Burst size** | 8 particles max (DESIGN-001 §VFX cap) |
| **Coord mapping** | `RepairEvent.Coord` (door-local 2D) interpreted as local X/Y on `_doorPlane` Transform |
| **SRP batching** | Single shared additive material — **no runtime material instances** (would break the SRP batcher per AC) |
| **Audio cue** | Paired with `WeldAudio` |
| **Perf budget** | One `Emit(EmitParams, int)` call per repair; reusable `EmitParams` field; 0 alloc |
| **Designer knobs** | `ParticleSystem` reference, `_doorPlane` Transform, `_particlesPerBurst` (clamped 0–16) |

**One-sentence summary:** Eight additive sparks fly off the welded coord in a single SRP-batched draw call — the visual half of the relief beat.

---

## ImpactAudio

| Field | Spec |
|---|---|
| **Trigger** | `IEventBus.Subscribe<DamageEvent>` |
| **Clip** | `SFX_Metal_Impact` (assigned in Editor-deferred step) |
| **Pitch** | Randomized in `[0.9, 1.1]` per fire (DESIGN-001 §SFX) using a seedable `System.Random` |
| **Volume** | Sprint 1: source default. Volume-rides-HP layer is Sprint 3 polish (DESIGN-001 §SFX hooks the +3dB-below-30% rule) |
| **Audio cue name** | `SFX_Metal_Impact` |
| **Perf budget** | One `PlayOneShot` per impact; 1 `NextDouble` from instance RNG; 0 alloc |
| **Designer knobs** | `_source` AudioSource, `_clip` AudioClip; `MinPitch`/`MaxPitch` constants tracked to DESIGN-001 |
| **Determinism** | `SetSeedForTest(int)` overrides the RNG seed for reproducible test sequences |
| **Accessibility** | **Always plays** (audio is not vestibular) — only the camera-shake visual is suppressed under ReduceMotion |

**One-sentence summary:** Metal-on-metal impact one-shot with `[0.9, 1.1]` pitch jitter from a seedable RNG — fires under Reduce Motion so the player still hears the door taking it.

---

## WeldAudio

| Field | Spec |
|---|---|
| **Trigger** | `IEventBus.Subscribe<RepairEvent>` |
| **Clip** | `SFX_Scrap_Weld` clink-hiss envelope (40ms tick + 200ms hiss decay per DESIGN-001 §SFX) |
| **Spatialization** | Sprint 1: source position. Stereo pan from coord is Editor-deferred wiring on the AudioSource itself (3D settings + spatial blend curve) |
| **Audio cue name** | `SFX_Scrap_Weld` |
| **Perf budget** | One `PlayOneShot` per repair; 0 alloc |
| **Designer knobs** | `_source` AudioSource, `_clip` AudioClip |

**One-sentence summary:** Clink-hiss one-shot on every successful weld — the audio half of the relief beat that the post-weld silence depends on.

---

## Cross-controller perf contract

- **Zero GC alloc on every event-handler path.** Handler delegates cached at `Awake`, captured nothing, subscribed once in `OnEnable`, unsubscribed in `OnDisable`.
- **Zero `FindObjectOfType`.** All collaborators arrive via VContainer `[Inject]`.
- **No SRP batcher break.** Weld sparks use a shared additive material; no runtime material instances.
- **Mobile budget headroom:** the five controllers together do at most one Vector3 write, two Perlin samples, two `Emit` calls, and two `PlayOneShot` calls per impact-frame. None of these are on the per-frame draw critical path — they're event-driven on Kendra's 1.5s impact cadence.

## Tunable surfaces (designer-facing)

| Asset | Lives at | Owner |
|---|---|---|
| `ScreenShakeConfig.asset` | `Assets/_Rpm/Juice/Config/` (Editor-deferred to instantiate) | Simone tunes; Malik authors the asset menu |
| `_particlesPerBurst` (Dust) | Inline serialized field on the controller prefab | Simone |
| `_particlesPerBurst` (Weld) | Inline serialized field on the controller prefab | Simone |
| `MinPitch` / `MaxPitch` constants | `ImpactAudio.cs` — tied to DESIGN-001 §SFX spec | Simone via design-doc amendment |
| AudioSource 3D / spatial blend | Editor-deferred wiring on the prefab AudioSources | Malik |
| ParticleSystem material assignments | Editor-deferred wiring | Malik |

---

## Test coverage (EditMode)

`Assets/_Rpm/Juice/Tests/JuiceDeterminismTests.cs`:

- Pitch always inside `[0.9, 1.1]` across 1024 samples.
- Two `ImpactAudio` instances seeded identically produce byte-identical pitch sequences.
- Two `ImpactAudio` instances seeded differently diverge within 8 samples.
- `ScreenShakeController.SampleAmplitudeForTest` returns 0 at full HP regardless of curve `t`.
- Same sampler returns `MaxAmplitude` at `t=0, hpRatio=0` under the default curve.
- Default decay curve is non-increasing across `t ∈ [0, 1]` and ends at 0.

PlayMode coverage (subscribe/unsubscribe lifecycle, multi-impact stacking, ReduceMotion toggle mid-shake) is on Jasmine's plate per the Pairing Agreement.

---

## Editor-deferred wiring (handoff to Sprint-1 scene-author session)

Tracked in `tickets/RPM-001.md` Notes §Feel-layer Editor-deferred. Summary:

1. Instantiate a `ScreenShakeConfig.asset` in `Assets/_Rpm/Juice/Config/` and set Simone's tuned curve.
2. Author `Rpm/Juice/Prefabs/JuiceBus.prefab` carrying the five controllers as siblings.
3. Wire the `ScreenShakeController._target` to the camera rig Transform in `Scenes/TactileTerror_S1.unity`.
4. Author `Rpm/Juice/VFX/DustFall.prefab` (ParticleSystem on the rafter beam) and assign to `DustFallController._emitter`.
5. Author `Rpm/Juice/VFX/WeldSpark.prefab` (additive-material ParticleSystem) and assign to `WeldSparkController._emitter`; assign `_doorPlane` to the door surface Transform.
6. Wire `ImpactAudio._source` + `_clip` to the metal-impact AudioSource.
7. Wire `WeldAudio._source` + `_clip` to the weld AudioSource.

---

## Amendments

| Version | Date | Change |
|---|---|---|
| 1.0 | 2026-04-18 | Initial Sprint 1 feel-layer spec authored in parallel with Kendra's RPM-001 code architecture. |
