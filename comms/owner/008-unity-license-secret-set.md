---
id: owner-008
from: Terrell Vaughn (digital-pit-boss)
to: Owner (Roderick Prewitt)
sent: 2026-04-18T15:10Z
subject: UNITY_LICENSE secret live — need UNITY_EMAIL + UNITY_PASSWORD from Owner
thread: ci-bootstrap
in-reply-to: owner-006
status: open
decision: null
---

[STATUS]: Yellow — 1 of 3 Unity secrets in; 2 still needed before Sprint 1 CI runs green.

[THE WIN]: `UNITY_LICENSE` secret is live on `prewittr/garage-defense`. Read directly from your locally-activated `.ulf` via WSL (`/mnt/c/ProgramData/Unity/Unity_lic.ulf`), 2507 bytes of XML, uploaded via `gh secret set`. Confirmed present in `gh secret list`.

Also cleaned up: the retired `game-ci/unity-request-activation-file@v2` workflow was admin-pushed to `main` yesterday to generate the .alf; removed cleanly once we pivoted to local activation.

[REDLINE]: `game-ci/unity-test-runner@v4` needs **three** secrets, not one. Without `UNITY_EMAIL` and `UNITY_PASSWORD`, the action cannot re-auth the Personal license on the ephemeral GitHub runner (each run lands on a different machine; machine-binding in the .ulf will reject otherwise).

Additional note: a Personal .ulf is machine-bound. Even with all three secrets, the first CI run may need game-ci to validate the license online. Expect one or two flake runs while the license caches on their pipeline. Standard for Personal licenses — not a blocker.

[DECISION REQUIRED]: Add the remaining two secrets —
  (A) You add `UNITY_EMAIL` + `UNITY_PASSWORD` now at
      https://github.com/prewittr/garage-defense/settings/secrets/actions
      → "New repository secret" (twice). Never paste credentials into this
      chat. Once saved, say "done" and I trigger a dry-run CI to verify
      activation, then flip `required_status_checks` so Monday's PRs gate on it.
  (B) Defer until Sprint 1 starts and add them when a real PR is ready.
      CI Unity-tests job will fail on every PR until then; `required_status_checks`
      stays off.
