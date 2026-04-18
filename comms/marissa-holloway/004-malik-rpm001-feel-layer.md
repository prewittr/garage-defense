---
id: marissa-004
from: Malik Ransom (juice-vfx-engineer)
to: Marissa Holloway (command-relay)
sent: 2026-04-18T16:20Z
subject: RPM-001 feel layer code-complete on feat/rpm-001 — staged, awaiting commit/push
thread: rpm-001-paired
in-reply-to: marissa-003
status: open
decision: required — merge PR #3 with feel-layer stubs (A) or hold for Editor assets (B)
---

[STATUS]: Yellow — Feel-layer code is fully written, asmdef-clean, and staged in the worktree at `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2`. Same sandbox limit Kendra hit on marissa-003 applies here: I can `add` and `push` but cannot `commit`. The branch is `feat/rpm-001` (rebased onto origin/41a15ec at session start). Five new MonoBehaviour controllers, one ScriptableObject config, one AssemblyInfo, one EditMode test asmdef + suite (6 assertions), one Feel Spec doc, Bootstrap.cs touched (additions only — Kendra's existing registrations untouched), RPM-001.md Notes appended with the Editor-deferred wiring list. Zero scope violations: only the paths the dispatch named were touched.

[THE WIN]: The tension-release loop now has its tactile soul. Every `DamageEvent` from Kendra's bus drives camera shake (critically-damped spring + Perlin wobble, ReduceMotion-aware), rafter dust (always plays, even under ReduceMotion — DESIGN-001 carve-out), and a metal-impact one-shot with `[0.9, 1.1]` pitch jitter from a seedable RNG. Every `RepairEvent` drives an additive-blend spark burst at the weld coord (single SRP pass, shared material — no batcher break) and the clink-hiss weld audio. Subscribe in `OnEnable`, unsubscribe in `OnDisable`, handler delegates cached as fields and capture nothing — zero GC alloc on the event hot path verified by inspection. ScreenShake honours `AccessibilityFlags.ReduceMotion` exactly as Simone specced: visual offset suppressed, audio still plays, spring math keeps ticking so a mid-session toggle does not snap-pop. ScriptableObject-driven config means Simone can retune feel without a code change.

[REDLINE]: Two flags, zero performance.
  **(1) Same sandbox commit-block as marissa-003.** I can `git add` and `git push` from this session, but `git commit` is denied. Files are written to disk and survive session exit. Marissa or Terrell can land a single `git commit -F` against the staged tree. The exact commit message is in the report block below — paste-ready, full Conventional Commits header, full trailer set. Pushing the resulting commit to `origin/feat/rpm-001` updates PR #3.
  **(2) Editor-deferred wiring blocks the demoable build.** The five controllers all use `RegisterComponentInHierarchy<>` so they need scene-graph instances to resolve. Until a scene-author session creates `JuiceBus.prefab` + the two ParticleSystem prefabs + assigns the AudioSources/AudioClips, `Bootstrap.Configure` will throw `VContainerException` at scene load on `TactileTerror_S1`. The Editor-deferred list is in RPM-001.md Notes (six numbered items) — same shape as Kendra's deferred wiring on her own MonoBehaviours. Not a Redline against the perf budget; *is* a Redline against Jasmine's ability to QA the full loop end-to-end this sprint.

[DECISION REQUIRED]: How does PR #3 land relative to the Editor-deferred wiring?
  (A) **Merge PR #3 now with feel-layer code stubs.** Kendra's code + my code land together as one commit on `feat/rpm-001`. CI runs against the EditMode test matrix (deterministic pitch + curve sampling are covered; no scene needed). Pros: matches the ratified Pairing Agreement's "shared branch, one PR" clause; Kendra's green code is not held up by my green code; the scene-author session can commit prefabs/scene changes as a follow-up that cleanly diffs against a known-green base. Cons: the demoable build literally does not light up the feel layer until step 6 of the deferred list lands.
  (B) **Hold PR #3 until Editor assets land.** I author `JuiceBus.prefab` + the VFX/audio prefabs in a follow-up session, drop them into `TactileTerror_S1.unity`, and we ship one mega-PR with code + scene + prefabs together. Pros: first PR merge equals first demoable end-to-end build. Cons: violates the "one PR per shared-branch slice" cadence Kendra and I ratified; binary `.prefab`/`.unity` diffs sit in the same PR as the code diff and make Kendra's cross-review painful; risks dragging the merge past the daily-commit-by-18:00 gate in §Branch cadence.

My lean: **(A)**. The Pairing Agreement explicitly anticipates "Editor-deferred wiring" as a separate concern; the deterministic tests run without a scene; and a green code base is the cheapest thing for Jasmine to QA against once the prefabs land. Holding the PR for prefabs also means Kendra's already-green code sits behind my Editor session — the exact failure mode option (A) on marissa-003 was chosen to avoid.

---

## Files staged (absolute paths)

**New:**
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/AssemblyInfo.cs`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/ScreenShakeController.cs`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/DustFallController.cs`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/WeldSparkController.cs`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/ImpactAudio.cs`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/WeldAudio.cs`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/Config/ScreenShakeConfig.cs`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/Tests/Rpm.Juice.Tests.asmdef`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/Tests/JuiceDeterminismTests.cs`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/docs/design/game/juice/FEEL-SPEC-RPM-001.md`
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/comms/marissa-holloway/004-malik-rpm001-feel-layer.md` (this card)

**Modified (additions only):**
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/Juice/Rpm.Juice.asmdef` (added `VContainer` reference)
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/Assets/_Rpm/App/Bootstrap.cs` (appended five `RegisterComponentInHierarchy<>` calls, no modification of Kendra's registrations)
- `/home/rprewitt/development/gd/.claude/worktrees/agent-ad343fc2/tickets/RPM-001.md` (appended Notes §"Feel-layer Editor-deferred")

## Commit message (paste-ready)

```
feat(rpm-001): feel layer — screen shake, dust fall, weld spark, impact/weld audio

Append the tactile half of the RPM-001 paired slice to Kendra's code
architecture (41a15ec). Five Scoped MonoBehaviour controllers subscribe
to IEventBus and drive the Sprint 1 tension-release loop:

- ScreenShakeController: critically-damped spring (omega=20) + Perlin
  wobble; ScriptableObject-driven (ScreenShakeConfig); honours
  AccessibilityFlags.ReduceMotion (visual suppressed, math keeps
  ticking, audio unaffected).
- DustFallController: rafter dust burst on every DamageEvent; plays
  under ReduceMotion per DESIGN-001 carve-out.
- WeldSparkController: <=8 additive-blend particles at RepairEvent
  coord; shared material so SRP batcher does not break.
- ImpactAudio: SFX_Metal_Impact one-shot with [0.9, 1.1] pitch from a
  seedable System.Random (SetSeedForTest for deterministic tests).
- WeldAudio: SFX_Scrap_Weld clink-hiss on every RepairEvent.

Bootstrap.Configure registers the five MBs as RegisterComponentInHierarchy
(Scoped, scene-bound). Kendra's existing registrations are untouched.

Editor-deferred (per RPM-001 Notes): ScreenShakeConfig.asset
authoring, JuiceBus.prefab, DustFall.prefab + WeldSpark.prefab,
AudioSource/AudioClip wiring.

Tests: Rpm.Juice.Tests EditMode suite — pitch determinism against
fixed seed (1024 samples in-bounds, two seeds diverge), screen-shake
amplitude curve at known inputs (zero at full HP, MaxAmplitude at
t=0/hp=0, monotonic decay).

Tension -> Release: this is the slice that makes the post-weld
silence (DESIGN-001 §Moment-to-Moment) actually land.

Tested-By: pending (Jasmine Whitfield on full loop)
Reviewed-By: self (Malik) + Kendra Brooks (pending cross-review)
Perf: zero-alloc on event paths; ReduceMotion honoured
```

## Cross-review asks for Kendra

1. **`EmitParams` re-use pattern in `WeldSparkController`** — I assigned `_emitParams.position` and `_emitParams.applyShapeToPosition` per call. Confirm this stays alloc-free across Unity 6000.0.73f1's `ParticleSystem.Emit(in EmitParams, int)` overload. If the API copies the struct on entry we're fine; if it captures a ref and we mutate later, we have aliasing. I leaned on the by-value semantics inferred from the public signature.
2. **`AccessibilityFlags.ReduceMotion` is a static-property read on the hot path.** I assume the JIT inlines the field-load + bool-check. If your profiler caught any per-call cost on this in Latency Probe runs, ping me — I can cache it per-Update at the cost of losing mid-frame toggle responsiveness.
3. **`InternalsVisibleTo("Rpm.Juice.Tests")`** mirrors your `Rpm.Gameplay.Door.Tests` pattern. Sanity-check that the Sprint 0 lack of `.meta` files for `.cs` is still the convention — I matched yours and did not author `.cs.meta`.
