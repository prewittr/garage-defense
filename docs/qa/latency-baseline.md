# Input Latency Baseline (RPM-008)

**Owner:** Kendra Brooks (`performance-hardener`)
**Linked Story:** [RPM-008](../../tickets/RPM-008.md)
**Test Plan:** [TP-RPM-008](./test-plans/TP-RPM-008.md)
**Status:** Placeholder — numbers land on the first real probe run.

---

## Purpose

This file records the Sprint 1 baseline values for drag-input-to-visual latency on
ubuntu-latest desktop CI. Future regressions compare against these numbers. Any
delta > 10% between a new run and this baseline is investigated before merge.

## Threshold

- **Gate:** p95 ≤ 12 ms (enforced by the CI `latency-gate` job).
- **Self-overhead budget:** probe instrumentation ≤ 0.5 ms per TP-RPM-008.

## Baseline (Sprint 1)

**To be filled in on the first real probe run** once:

1. `Scenes/LatencyProbe.unity` exists with a `LatencyProbe` GameObject (Editor-deferred; see RPM-008).
2. A scripted-drag harness produces ≥ 100 samples inside the 5-second capture window.
3. The scene is added to Build Settings so CI's Unity test runner loads it in Play Mode.
4. The Unity tests job uploads `Builds/latency-report.json` as the `latency-report` artifact.
5. The `latency-gate` job evaluates the p95 against the 12 ms threshold.

| Run | Date | Runner | p50 (ms) | p95 (ms) | n | Result | Notes |
|---|---|---|---|---|---|---|---|
| — | _pending_ | ubuntu-latest | _pending_ | _pending_ | _pending_ | _pending_ | First probe after Editor-deferred scene lands. |

## Cross-platform parity (Sprint 2+)

ubuntu-latest runners are desktop-class. Real mobile/console latency is the
Sprint 2+ device-farm probe (iPhone 12, Pixel 6, Steam Deck). When that lands
this table grows a column per device tier.

## Redline

If ubuntu-latest runner jitter prevents reliable sub-12 ms measurement even on
clean code, Kendra files a Redline Escalation Inbox Card to Marissa and we
re-spec the gate (per RPM-008 Notes).
