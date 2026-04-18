---
id: owner-021
from: Marissa Holloway (command-relay) + Kendra Brooks (performance-hardener) + Terrell Vaughn (digital-pit-boss)
to: Owner (Roderick Prewitt)
sent: 2026-04-17T16:00Z
subject: 🟢 Unity 6000.0.73f1 CI green; branch protection refreshed
thread: sprint-1-kickoff
in-reply-to: owner-020
status: open
decision: null — milestone notice
---

[STATUS]: **Green.** Unity bump `6000.0.32f1 → 6000.0.73f1` validated end-to-end on CI. Security advisory closed. Branch protection contexts refreshed to match the new job name.

[THE WIN]: Three commits, two debugging cycles, one clean green.
- **Run `24598892769`:** Lint ✅ · Unity EditMode ✅ · Latency Gate ✅ · Unity Test Results (informational) ✅.
- **Diagnosis of the interim failure (`24598704751`):** EditMode tests actually PASSED — BootstrapSmoke resolved IBootstrapService as designed. The job was marked failure because the container SIGSEGV'd (exit 139) during cleanup *between* EditMode and PlayMode phases — a known `game-ci/unity-test-runner@v4` lifecycle flake on Unity 6.0 LTS containers. Tests themselves were fine.
- **Fix:** dropped `testMode: all` → `testMode: editmode`. We have zero PlayMode tests today (BootstrapSmoke is EditMode). The change eliminates the flake at zero functional cost. Renamed the job label from "Unity EditMode + PlayMode tests" to "Unity EditMode tests" for truthful labeling. Inline comment in `ci.yml` documents when to revisit (once real PlayMode tests arrive OR game-ci ships a 6.0 lifecycle fix).
- **Branch protection refreshed:** `required_status_checks` on `main` and `develop` now reference **"Unity EditMode tests"** (not the stale "Unity EditMode + PlayMode tests"). Confirmed via `gh api`.
- **ADR-002 in action:** the 7-day-bump-on-advisory policy we filed this afternoon got its first test run — advisory surfaced, bump applied, validated same day. Policy is working.

[REDLINE]: Three items carried forward, two new ones:
1. **Draft PR #1 is still draft** — blocked on the Editor-deferred work (scenes, Addressables, `.meta` files). Nothing CI-related blocks you anymore; only the Unity Editor steps remain.
2. **Your local Unity Hub install of `6000.0.73f1`** — when it finishes, proceed to the Editor checklist I sent earlier.
3. **PlayMode tests deferred to later** — when Sprint 2+ introduces them, we revisit `testMode: all`. Right now the absence is honest, not hidden.
4. **Admin-bypass on `main` retirement** — still scheduled for first release cut next Friday.

[DECISION REQUIRED]: None — milestone notice.

Net Sprint 1 state: team produced **9 commits, 1 draft PR, 3 CI iterations, 1 ADR, 1 Design Doc, 1 Pairing Agreement** in about two hours since kickoff. Critical path is now **you + Unity Editor** to complete RPM-007.
