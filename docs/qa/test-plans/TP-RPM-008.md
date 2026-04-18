# [TP-RPM-008] Test Plan — Input Latency Gate (<12ms p95)

**Author:** Jasmine Whitfield (`qa-breakdown-analyst`)
**Co-author:** Marvin Sinclair
**Date:** 2026-04-17
**Linked Story:** [RPM-008](../../../tickets/RPM-008.md)
**Status:** Draft

## Scope
- **In scope:** instrumentation of drag-to-render latency, automated probe scene, CI gate that fails on p95 > 12ms, baseline record.
- **Out of scope:** mobile / console-class latency (Sprint 2+), device-farm integration (Sprint 2+), perceptual testing with real users (Sprint 3+).

## Preconditions
- [RPM-007](../../../tickets/RPM-007.md) merged (needs `Rpm.Input` asmdef).
- Minimal drag handler available — either stub in LatencyProbe scene or full RPM-001 merge.

## Test Cases

| ID | Case | Priority | Steps | Expected | Notes |
|---|---|---|---|---|---|
| TC-001 | Probe scene boots | **P0** | 1. Open `Scenes/LatencyProbe.unity`. 2. Enter Play Mode. | Scene runs. Scripted drag begins automatically. | |
| TC-002 | 100 samples captured | **P0** | 1. Wait 5s in Play Mode. 2. Check `Builds/latency-report.json`. | File exists. Contains `samples: [...]` array of length 100, plus `p50` and `p95` numeric fields. | |
| TC-003 | CI gate passes (green) | **P0** | 1. Open PR. 2. Wait for `latency-gate` job. | Job passes. Step summary echoes p50 and p95 in ms. | |
| TC-004 | CI gate fails (red) | **P0** | 1. Artificially insert a `yield return null` × 5 extra frames in drag handler. 2. Open PR. 3. Wait for gate. | Job fails. Step summary shows p95 > 12ms. | Negative test — do NOT merge the artificial code; revert after verifying. |
| TC-005 | Baseline written | **P0** | 1. Merge RPM-001 successfully. | `docs/qa/latency-baseline.md` committed in same PR or immediate follow-up with Sprint 1 p95 recorded. | |
| TC-006 | No alloc in probe | P1 | 1. Profiler → GC Alloc during probe run. | Zero alloc in the measurement loop itself. | Self-instrumentation must not skew results. |
| TC-007 | Step summary format | P2 | 1. Inspect PR status-check details. | Output reads `p50: <x>ms · p95: <y>ms · gate: PASS/FAIL`. | Readability for reviewers. |

## Device Matrix — Sprint 1

| Tier | Device | Apply? | Notes |
|---|---|---|---|
| CI runner | ubuntu-latest | **Yes** | Primary gate. |
| Editor | Unity 6000.0.73f1 desktop | **Yes** | Manual spot-check, numbers within 10% of CI. |
| Windows Standalone | CI dev build | **Yes** | Confirms numbers aren't Editor-inflated. |
| iPhone 12 / Pixel 6 | — | **No (Sprint 2)** | Device-farm probe lands Sprint 2. |

## Performance Validation
- **Gate threshold:** p95 ≤ 12ms on ubuntu-latest CI.
- **Self-overhead budget:** probe instrumentation must add ≤ 0.5ms to measured latency.

## Exit Criteria
- [ ] All P0 cases pass.
- [ ] Gate integrated into CI and runs on every PR.
- [ ] Baseline file exists in `docs/qa/`.
- [ ] `Tested-By: Jasmine Whitfield` trailer on merge commit.
- [ ] **Risk-note filed if** ubuntu-latest jitter prevents reliable <12ms — per RPM-008 Notes, Kendra files Redline Escalation and we re-spec.

## Results
_To be filled during Sprint 1._
