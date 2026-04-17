---
id: owner-018
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-17T14:55Z
subject: CI first-run — tests passed, permissions bug fixed, re-run in flight
thread: sprint-1-kickoff
in-reply-to: owner-017
status: open
decision: null — FYI
---

[STATUS]: Yellow → Green (in flight). First CI run on PR #1 was marked FAILURE, but actual Unity tests **passed**. Root cause was a workflow permissions gap, not license or code.

[THE WIN]: Full diagnosis + fix inside 5 minutes wall-clock.
- **Unity license activation:** clean first try. `owner-009` retry budget is untouched.
- **Tests:** `editmode-results.xml — 2/2 — Passed in 0.059s`. `BootstrapSmoke` resolves `IBootstrapService`, asserts `IsReady == true`. Exactly what Kendra designed.
- **Failure signal:** `##[error]Resource not accessible by integration` on the check-run POST back to GitHub.
- **Root cause:** default `GITHUB_TOKEN` on PR events from the same repo is read-only for `checks`. `game-ci/unity-test-runner@v4` needs write access to post its own check-run result — it can't report success back to the PR without it.
- **Fix:** added explicit `permissions` block to `ci.yml` (`contents: read`, `checks: write`, `pull-requests: write`, `statuses: write`). Kendra committed and pushed `49cd339` on `feat/rpm-007`. New run `24588341764` IN_PROGRESS.

[REDLINE]: Unity CI run takes **~7 minutes** wall-clock even when succeeding — Docker image pull and Unity install dominate. That's our baseline cost per PR. `actions/cache@v4` keyed on `Packages/ProjectSettings/Assets` should chop this significantly on subsequent runs; we'll see on the re-run right now. Future sprints that want sub-2-min feedback loops will need to address this.

[DECISION REQUIRED]: None — FYI. Next checkpoint fires when run `24588341764` concludes (ETA ~7 min).
