---
id: owner-006
from: Terrell Vaughn (digital-pit-boss)
to: Owner (Roderick Prewitt)
sent: 2026-04-18T00:30Z
subject: Branch protection live on main + develop; gh CLI wired
thread: repo-init
in-reply-to: owner-004
status: answered
decision: (A) — Owner delivers UNITY_LICENSE before Sprint 1 kickoff (2026-04-20); Terrell wires required_status_checks same day
---

[STATUS]: Green — GitFlow gates are now enforced at the origin.

[THE WIN]: `gh` CLI 2.90.0 installed at `~/.local/bin/gh` (no root). Owner authenticated via browser device flow. Branch protection applied via API to both `main` and `develop`:
  - Pull request required (1 approving review)
  - Force push: **disabled**
  - Branch deletion: **disabled**
  - Stale-review dismissal: off (flip later when review culture stabilizes)
  - `enforce_admins`: **false** — Owner retains emergency override
  - `required_status_checks`: **null** (intentional — CI-green gate added in follow-up once `UNITY_LICENSE` secret lands and Unity tests are actually green)

[REDLINE]: `UNITY_LICENSE` secret is still empty. Until it lands, CI Unity-tests job can't run green, so adding it to `required_status_checks` now would block all PRs. We will flip it on the moment the license hits GitHub.

[DECISION REQUIRED]: `UNITY_LICENSE` secret delivery —
  (A) You add it at https://github.com/prewittr/garage-defense/settings/secrets/actions before Sprint 1 kickoff on 2026-04-20. Terrell wires required status checks the same day; Sprint 1 opens with full DoD gate live.
  (B) You add it during Sprint 1 at your convenience; Kendra develops locally; CI Unity tests run but don't block merges until you add the secret; required status checks flip on after.
