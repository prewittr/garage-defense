---
id: owner-020
from: Marissa Holloway (command-relay) + Kendra Brooks (performance-hardener)
to: Owner (Roderick Prewitt)
sent: 2026-04-17T15:30Z
subject: Unity bump 6000.0.32f1 → 6000.0.73f1 (security advisory); ADR-002 filed
thread: sprint-1-kickoff
in-reply-to: owner-017
status: open
decision: null — FYI; validation pending CI
---

[STATUS]: Yellow → Green (in flight). Unity Hub advisory on `6000.0.32f1` resolved on `feat/rpm-007`. New CI run validating against `6000.0.73f1` now.

[THE WIN]:
- **Version bumped** same-LTS-track: `6000.0.32f1` → **`6000.0.73f1`** (the latest `6000.0.x` LTS patch Unity Hub offers as of today).
- **Package manifest unchanged** — URP 17.0.3, Addressables 2.2.2, VFX Graph 17.0.3, UniTask 2.5.10, VContainer 1.16.4 all stay as-is. No migration debt.
- **ADR-002 filed** (`Proposed`): [`docs/architecture/adr/ADR-002-unity-version-policy.md`](../../docs/architecture/adr/ADR-002-unity-version-policy.md) — pins the `6000.0.x` LTS track, commits us to bumping within **7 calendar days** of any future Unity security advisory on the pinned version. Moves to `Accepted` at the Sprint 1 Tech Design Review (Mon 2026-04-20 14:00).
- **File sweep:** `ProjectVersion.txt`, `README.md`, `ARCHITECTURE.md` §1, 3 QA test plans, 1 template all refer to the new version. Historical comms cards (`owner-012`, etc.) intentionally left as-written — they describe state at time of writing.
- **Commit:** `b57aa19` on `feat/rpm-007`. CI run `24598704751` queued.

[REDLINE]: Two carried risks plus one new one:
1. **First CI run against `6000.0.73f1`** — `game-ci/unity-test-runner@v4` needs to pull a fresh Unity Docker image for this version. First run may take slightly longer than the 5m27s baseline we set on `6000.0.32f1`. Subsequent runs will cache. If run `24598704751` comes back red, Terrell triages same-turn.
2. **Your local Unity Hub** — you still need to install `6000.0.73f1` (~5–6 GB) before you can do the Editor-deferred work on RPM-007. Kick that off in Hub while CI validates server-side.
3. **Stale `UNITY_LICENSE` secret possibility** — if you re-activated the license via "Manage licenses → Add" earlier (rather than reusing the original `.ulf`), the machine-bound secret on GitHub may be stale. CI will tell us; if it fails license activation, I pull the refreshed `.ulf` from `/mnt/c/ProgramData/Unity/Unity_lic.ulf` and re-upload in 30 seconds.

[DECISION REQUIRED]: None — FYI + validation pending.

Next checkpoint: CI run `24598704751` concludes. Either clean-green (we proceed to Editor work on `6000.0.73f1`) or a specific diagnostic card from me.
