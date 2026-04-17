# [TP-RPM-001] Test Plan — Kinetic Door Drag-to-Repair Core Loop

**Author:** Jasmine Whitfield (`qa-breakdown-analyst`)
**Co-author:** Marvin Sinclair (DoR facilitation)
**Date:** 2026-04-17
**Linked Story:** [RPM-001](../../../tickets/RPM-001.md)
**Status:** **Draft** (awaiting first build; upgrades to Executing once RPM-007 lands)

## Scope
- **In scope:** drag-to-repair mechanic, impact cadence, first-pass juice (screen-shake, weld spark, impact SFX), accessibility `ReduceMotion`, GC alloc in hot path.
- **Out of scope:** final art, full VFX Graph polish (Sprint 3), wave escalation (Sprint 2), gacha/monetization (later sprints), mobile / console device matrix (Sprint 2 onward).

## Preconditions
- Account state: N/A (prototype has no account yet).
- Device state: Unity Editor OR Windows Standalone build.
- Feature flags: `ReduceMotion` toggled per cases below.
- Server state: N/A (no backend calls in RPM-001).
- Scene: `Scenes/TactileTerror_S1.unity`.

## Test Cases

| ID | Case | Priority | Steps | Expected | Notes |
|---|---|---|---|---|---|
| TC-001 | Happy path repair | **P0** | 1. Open scene. 2. Wait for first impact (HP drops 100→95%). 3. Drag scrap from inventory to the visible damage point. 4. Release. | HP restores 15% (→110% capped at 100%? expect: capped at 100%). Scrap count decreases by 1. Spark VFX + clink-hiss SFX play at drop coord. | Verify cap behavior in AC. |
| TC-002 | Drop outside damage point | **P0** | 1. Start drag on scrap. 2. Release on empty canvas. | Scrap snaps back to inventory. HP unchanged. No VFX / SFX. | |
| TC-003 | Drag onto full-HP door | **P0** | 1. Ensure HP == 100%. 2. Drag scrap onto door. | Action rejected. Scrap returns. No HP change. No VFX / SFX. | |
| TC-004 | Impact cadence | **P0** | 1. Start scene with `ReduceMotion=false`. 2. Observe for 15 seconds. | 10 impacts fire (1.5s each). HP drops 5% per impact. Screen shake scales with HP. Dust-fall triggers each hit. | |
| TC-005 | ReduceMotion skips shake | **P0** | 1. Set `ReduceMotion=true`. 2. Wait for impact. | **No** screen shake. Dust-fall still plays. HP drops normally. | Accessibility smoke. |
| TC-006 | SFX pitch variance | P1 | 1. Record 20 consecutive impacts. 2. Analyze pitch. | Pitches distributed 0.9–1.1 (not all identical). | Ear-fatigue check. |
| TC-007 | GC alloc hot path | **P0** | 1. Open Profiler → Memory → GC Alloc. 2. Drag scrap. | **0 bytes** allocated during drag-update tick. | Fail → file bug + block merge. |
| TC-008 | SRP batcher — weld sparks | P1 | 1. Open Frame Debugger during weld. 2. Inspect draw calls. | Spark VFX batched in single SRP pass, no batcher break. | |
| TC-009 | Input latency p95 | **P0** | 1. Switch to `Scenes/LatencyProbe.unity`. 2. Run RPM-008 harness. | p95 ≤ 12ms. | See [TP-RPM-008](TP-RPM-008.md). |
| TC-010 | Scrap exhaustion | P1 | 1. Use all 10 scrap units. 2. Try to drag nothing. | Inventory empty state UI shown. No crash. | |
| TC-011 | Drag during impact | P2 | 1. Start drag. 2. Let impact fire mid-drag. | Drag continues uninterrupted. Impact still registers HP drop. | Input robustness. |

## Device Matrix Applied — Sprint 1

| Tier | Device | Apply? | Notes |
|---|---|---|---|
| Editor | Unity 6000.0.32f1 on WSL / Windows | **Yes** | Primary validation surface Sprint 1. |
| Windows Standalone | Dev build from CI | **Yes** | P0 cases only. |
| iPhone 12 | — | **No (Sprint 2)** | Physical device + TestFlight slot required. |
| Pixel 6 | — | **No (Sprint 2)** | Physical device + Play Internal slot required. |
| Steam Deck | — | **No (Sprint 2)** | |
| PS5 / Xbox | — | **No (Sprint 3+)** | |

## Performance Validation
- **Profiler capture:** record 10s of active play on the TactileTerror_S1 scene with Profiler running.
- **Frame time p50 target:** < 8ms (comfortable 60 FPS headroom on desktop).
- **Frame time p95 target:** < 16.6ms (hard 60 FPS floor).
- **Memory ceiling (Windows Standalone):** < 500 MB RSS.
- **Input latency p95:** ≤ 12ms (gated by RPM-008).

## Exit Criteria
- [ ] All P0 cases pass.
- [ ] **Zero** S1 bugs.
- [ ] **Zero** S2 bugs.
- [ ] S3 bugs triaged (backlog or accepted for polish in Sprint 2/3).
- [ ] Performance targets met.
- [ ] `Tested-By: Jasmine Whitfield (build 0.1.1, matrix: Editor + Win-Standalone — Sprint 1 scope)` trailer added to merge commit.

## Results (appended after execution)
_To be filled during Sprint 1._
