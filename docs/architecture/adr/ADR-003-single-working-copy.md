# ADR-003: Single working copy at `C:\development\garage-defense`; WSL operates via `/mnt/c` mount

**Status:** Proposed
**Date:** 2026-04-18
**Deciders:** Owner (Roderick Prewitt), Marissa Holloway, Terrell Vaughn, Kendra Brooks
**Consulted:** —
**Informed:** whole team

## Context
The project scaffold was initially cloned to the WSL-native path `/home/rprewitt/development/gd`. Unity Editor on Windows cannot reliably open projects over the `\\wsl.localhost\Ubuntu-20.04\...` UNC path — the editor launches, spawns `Unity.exe`, and silently exits with code `0` within ~10 seconds without creating `Library/`. Unity Hub logs confirm the child process exits cleanly without error, but no project window appears. This is a long-standing Unity limitation with UNC-style paths.

Cloning the repo a second time to the Windows-native path `C:\development\garage-defense` resolved the issue instantly. Unity opens, imports packages, and generates the expected `.meta` files for the asmdef split from RPM-007.

We then had two working copies:
- `/home/rprewitt/development/gd` — WSL-native, where agent tooling (gh CLI, git, Claude agents, Marissa comms) had been operating.
- `C:\development\garage-defense` — Windows-native, required for Unity.

Two working copies for one repo is a well-known source of drift — one missed `git pull` produces divergent commits, merge conflicts, and lost work. Neither copy had drifted at the moment of this decision; both matched `origin`.

## Decision Drivers
- Unity Editor on Windows is a hard constraint; no feasible path to running it on WSL file shares.
- Single source of truth eliminates drift risk at zero ongoing cost.
- WSL file I/O on `/mnt/c` is slower than native ext4, but only meaningful for massive tree-scans — our agent workload is small-file edits and git operations, where `/mnt/c` latency is imperceptible.
- Agent tooling (gh CLI, ssh, git) works identically against `/mnt/c` paths from WSL.

## Options Considered

### Option A — Single copy at `/mnt/c/development/garage-defense`; WSL mounts, Windows natively owns
- **Pros:** One source of truth; zero drift risk; Unity works natively; WSL shell continues to operate; all our agent tooling still lives in WSL but points at `/mnt/c` paths.
- **Cons:** `/mnt/c` file ops from WSL are marginally slower than native ext4 (not measurable for our workload); `/home/rprewitt/development/gd` becomes an orphan that must be consciously archived or deleted.
- **Cost:** low.

### Option B — Dual working copies with a strict pull-before-commit / push-before-switch protocol
- **Pros:** Each side is native for its use case.
- **Cons:** Humans (and Claude) miss `git pull` under time pressure. Divergent commits → merge conflicts → 1+ hour of recovery each time. Cognitive load of "which side am I on?" compounds.
- **Cost:** high (recurring).

### Option C — Windows-only, abandon WSL
- **Pros:** Radically simple.
- **Cons:** All our shell tooling (bash scripts, gh CLI, Claude agents defined at `/home/rprewitt/.claude/`, existing muscle memory) migrates to Windows. Not a small lift.
- **Cost:** medium one-time, unclear recurring.

## Decision
**Option A.** Adopt `C:\development\garage-defense` (WSL view: `/mnt/c/development/garage-defense`) as the sole working copy. Unity opens it from the Windows path; WSL shell and all agent tooling operate via the `/mnt/c` mount. `/home/rprewitt/development/gd` is immediately retired — no new commits land there; we leave it in place for one sprint as a historical fallback, then delete on Sprint 2 wrap.

## Consequences

### Positive
- One source of truth; zero drift risk.
- Unity Editor operates on a natively-supported path.
- Agent tooling (Marissa, Kendra, Marvin, Terrell, et al.) continues to function from WSL — only the cwd shifts.
- CI behavior unchanged; origin is the canonical truth and both clones pushed to it.

### Negative
- `/mnt/c` I/O is marginally slower than native ext4 from WSL for operations that touch many thousands of small files. Our workload does not stress this.
- One-time mental cost of retraining path-muscle-memory from `/home/rprewitt/development/gd` to `/mnt/c/development/garage-defense`.

### Neutral
- No architectural change to the codebase; purely a working-directory policy.

## Compliance / Follow-up
- **Marissa Holloway** — from this ADR forward, every agent invocation and every Bash command runs with cwd `/mnt/c/development/garage-defense`. Marissa owns calling out any drift in standup digests.
- **Terrell Vaughn** — no CI changes required; origin is canonical.
- **Owner (Roderick Prewitt)** — on any future developer onboarding, clone to `C:\development\garage-defense` or equivalent Windows-native path; never open a Unity project from a `\\wsl.localhost\...` UNC.
- **Sprint 2 wrap** — delete `/home/rprewitt/development/gd` after confirming zero unique content remains (commit SHA comparison on wrap).

## Notes
- Unity Hub logs at `%APPDATA%\UnityHub\logs\info-log.json` confirmed the UNC-open failure mode: `Spawning editor instance ... -projectpath \\\\wsl.localhost\\Ubuntu-20.04\\...` followed ~10 seconds later by `child process exited with code 0` and no further activity.
- The parallel Windows-native clone created a fresh `Library/`, resolved `Packages/packages-lock.json`, generated `.meta` files for the RPM-007 asmdef split, and produced the URP default asset files — exactly the Editor-deferred work Kendra flagged in RPM-007 Notes. These artifacts land in a follow-up commit once the Scene + Addressables steps complete.

---
**Once this ADR is ACCEPTED, it is immutable.** Future changes require a new ADR that supersedes this one.
