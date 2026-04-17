---
id: owner-017
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-17T14:30Z
subject: RPM-007 scaffold landed on feat/rpm-007 — PR timing A/B
thread: sprint-1-kickoff
in-reply-to: owner-015
status: answered
decision: (A) — open draft PR now; expose first-run CI flake risk today while team is hot
---

[STATUS]: Green — Sprint 1 has its first real code on `origin/feat/rpm-007`.

[THE WIN]: Kendra delivered the RPM-007 scaffold in ~25 minutes of wall-clock. Two commits, both pushed:
- `0917038` — `feat(rpm-007): asmdef split + VContainer composition root`
- `590c9a5` — Kendra's first standup card (filed to my inbox per protocol).

**Landed:**
- Six feature-oriented `.asmdef` files replacing the flat placeholder: `Rpm.Core`, `Rpm.Input`, `Rpm.Gameplay.Door`, `Rpm.Gameplay.Scrap`, `Rpm.Juice`, `Rpm.App` — dependency matrix matches ARCHITECTURE.md §2, zero cycles, manually verified.
- VContainer composition root (`Rpm.App.Bootstrap`) with a minimal `IBootstrapService` proving the container resolves.
- EditMode smoke test (`Rpm.App.Tests.BootstrapSmoke`) — passes when run, gates CI.
- **ADR-001 (VContainer over Zenject)** filed Proposed at [`docs/architecture/adr/ADR-001-vcontainer-over-zenject.md`](../../docs/architecture/adr/ADR-001-vcontainer-over-zenject.md). Moves to Accepted at Sprint 1 Tech Design Review (Mon 04-20 14:00).
- RPM-007 Notes section updated with the Editor-deferred checklist (scenes, Addressables, Bootstrap GameObject wiring, `.meta` files).

[REDLINE]: Editor-authored pieces (scene files, Addressables config, `.meta` files) cannot be safely hand-written outside Unity. They remain **in-scope for RPM-007** — ticket does not move to `In Review` until a developer with Unity Editor access lands them in a follow-up commit. Realistic window: Sat/Sun or Monday 9am kickoff. Not yet a redline.

[DECISION REQUIRED]: PR timing for `feat/rpm-007` —
  (A) **Open PR now** against `feature/tactile-terror` as a draft, explicitly labeled "Editor-deferred items pending." Runs CI immediately — exposes the first-run Unity-CI flake risk (owner-009, owner-012) early, in the daylight, while we're here to retry. Does **not** merge until deferred items land.
  (B) **Hold the PR** until Editor work is in (scenes + Addressables + meta files). Single clean PR, single CI run, reviewer sees the complete picture. Pushes first-run flake exposure to Monday or whenever Editor work lands.

My lean: **(A).** First-run CI flake is our top Sprint 1 risk. Exposing it now, while momentum is fresh and we can retry three times, is cheaper than discovering it Monday. The "draft" label keeps reviewers out of the premature review cycle.
