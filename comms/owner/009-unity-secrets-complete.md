---
id: owner-009
from: Terrell Vaughn (digital-pit-boss)
to: Owner (Roderick Prewitt)
sent: 2026-04-18T15:20Z
subject: All three Unity secrets live — Sprint 1 CI is unblocked
thread: ci-bootstrap
in-reply-to: owner-008
status: open
decision: null — FYI + one small status-check commitment shift
---

[STATUS]: Green — every hard blocker for Sprint 1 kickoff is cleared.

[THE WIN]:
```
UNITY_EMAIL     2026-04-17T15:14:37Z
UNITY_LICENSE   2026-04-17T15:06:26Z
UNITY_PASSWORD  2026-04-17T15:18:23Z
```
All three secrets live on `prewittr/garage-defense`. Owner handled `UNITY_EMAIL` and `UNITY_PASSWORD` directly (credentials never in chat, good). Password obtained via Unity's "Forgot password" reset flow on the Google-OAuth Unity account — standard workaround.

[REDLINE]: Small commitment shift from `owner-006`. That card promised `required_status_checks` would flip on the same day the license landed. I'm recommending we flip it **on the first green PR CI run** instead of preemptively, for two reasons:
  1. First game-ci/unity-test-runner run is slow (20–40 min: Docker pull + Unity install) and can flake on Personal-license machine-binding; pre-flipping the gate would risk blocking Monday's first PR on a flake.
  2. Real validation comes from an actual PR run, not a synthetic one.

Net delta: gate flip slips from "today" to "first green PR run," likely mid-Monday. No impact on Sprint 1 scope or dates.

[DECISION REQUIRED]: Status-check flip timing —
  (A) Accept the shift — flip `required_status_checks` after first green PR CI run on Monday (recommended; safest for launch-day PR throughput).
  (B) Force the original commitment — I add a tiny secrets-smoke workflow today, run it, and flip the gate immediately even though the first real Unity run hasn't happened yet. Accepts some Monday-morning risk.
