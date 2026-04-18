# 002 — Kendra · RPM-008 Spike (Standup Card)

**From:** Kendra Brooks (`performance-hardener`)
**To:** Marissa Holloway (`command-relay`)
**Date:** 2026-04-18
**Channel:** 3 → 2 handoff (Engine Room → Tactical Sprint)
**Re:** [RPM-008](../../tickets/RPM-008.md) — input latency probe harness + CI gate (parallel spike)

---

[STATUS]: Green — RPM-008 code harness + real CI latency-gate landed on `feat/rpm-008` as a draft PR into `feature/tactile-terror`. Scope held; no RPM-001 files touched.

[THE WIN]: The latency gate is no longer a placeholder echo. `latency-gate` now downloads the `latency-report` artifact, parses p50/p95 with stdlib Python, fails PRs on p95 > 12ms, and publishes a formatted markdown table to the PR status-check step summary. Zero-alloc measurement path, preallocated rolling buffer, NIST-7 percentile math with 11 EditMode tests on known-answer synthetic arrays. The gate is wired and dormant-safe — it exits 0 with a warning until the Editor-deferred `Scenes/LatencyProbe.unity` lands, so merging this does not break any open PR.

[REDLINE]: None active. Two forward-looking risks flagged in `docs/qa/latency-baseline.md`: (1) ubuntu-latest jitter may prevent reliable sub-12ms even on clean code — if the first real probe run confirms this I file a Redline per RPM-008 Notes; (2) the Latency asmdef currently references the root `Rpm` assembly (not `Rpm.Core` / `Rpm.Input`) because RPM-007's asmdef split has not yet merged to `feature/tactile-terror`. I'll tighten the reference list in a follow-up commit the moment the split lands — no blocker.

[DECISION REQUIRED]: Flip `required_status_checks` to add the `Input Latency Gate (RPM-008)` context now, or hold?
- **(A) Flip now.** The gate is already dormant-safe — missing report = green, present report = real check. Adding it to required checks immediately means the first real probe run enforces the 12ms budget without a second branch-protection round-trip with Terrell.
- **(B) Hold until the Editor-deferred probe scene produces a real report.** Keeps the required-checks list honest (no "required" check that always no-ops), but costs one extra coordination hop with Terrell once the probe scene lands.

**My recommendation: (A).** The gate's behavior in the no-report case is explicit and logged as a GitHub warning, which is the correct signal to reviewers during the Sprint 1 Editor-deferred window.
