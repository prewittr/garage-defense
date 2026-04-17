---
id: owner-012
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-18T18:00Z
subject: Sprint 1 readiness review — GO with one watch-item
thread: sprint-1-kickoff
in-reply-to: owner-007, owner-011
status: open
decision: null
---

[STATUS]: Green — Sprint 1 "The Bones" is **GO for Monday 2026-04-20** with a single tracked watch-item (Marvin's pre-sprint ticket delivery by Sunday EOD).

[THE WIN]: Full stack of prerequisites is in place. Every piece the team needs to pull from `feature/tactile-terror` and open the first PR on Monday is landed and pushed to `origin`.

### Foundation — all green
- Team: **9 agents** + Owner, all prompts written and comms-folder seeded.
- Comms: universal Inbox Card format live; 11 prior cards archived.
- Repo: `github.com/prewittr/garage-defense`; `main` / `develop` / `feature/tactile-terror` all on origin.
- Branch protection: required PR + 1 approval on `main` and `develop`; admin bypass available for infra emergencies.
- Unity: 6000.0.32f1 scaffold + Packages manifest (URP, Input System, Addressables, VFX Graph, UniTask, VContainer) + root asmdef.
- CI: `.github/workflows/ci.yml` wired; all **three Unity secrets** in place (`UNITY_LICENSE`, `UNITY_EMAIL`, `UNITY_PASSWORD`).
- Process: `PROCESS.md v2.0` accepted; 12 templates in `/docs/templates/`; `ARCHITECTURE.md v0.1` live; Mermaid standard adopted.
- Roadmap: 5 sprints to Prototype Gate on 2026-05-18.
- Tooling: `gh` CLI 2.90.0 installed and authenticated locally.

### Pending — one watch-item
| Item | Owner | Due | Status |
|---|---|---|---|
| Canonical ticket files `RPM-001.md`, `RPM-007.md`, `RPM-008.md` with DoR checklist filled | Marvin Sinclair | **EOD Sun 2026-04-19** | Not started |
| Test plans `TP-RPM-001/007/008` (P0 cases) | Jasmine Whitfield (with Marvin) | **EOD Sun 2026-04-19** | Not started |

Per Marvin's own charter, Sprint 1 cannot open on Monday with not-Ready tickets. He owns landing these six files before kickoff.

### Deferred (accepted gaps)
| Item | Needed by | Why OK now |
|---|---|---|
| First Unity CI green run | First PR Monday | Per `owner-009` (A): secrets present; game-ci first-run flake expected and tolerated. |
| `required_status_checks` flip | First green PR run | Per `owner-009` (A): gate flips after first success, not preemptively. |
| PlayFab dev title | Sprint 4 | Ghost Refill telemetry is Sprint 4 scope. |
| Playtester pool | End of Sprint 3 | Gate playtest is 2026-05-18. |
| TestFlight / Play Console slot | Sprint 2 | Sprint 1 validates on Editor + desktop; device builds land Sprint 2. |

### Risks to watch during Sprint 1
1. **Unity CI first-run flake** — Personal `.ulf` is machine-bound; re-auth via EMAIL/PASSWORD on the ubuntu runner may need 1–2 retries. Terrell will monitor the first PR's Unity job and file a redline card if it doesn't stabilize within three attempts.
2. **Paired pair mode (Kendra + Malik on RPM-001)** — unexercised in this team. Stage 4 DoR was designed for single-owner tickets; Marvin will split RPM-001 into **RPM-001a (code)** and **RPM-001b (feel)** if the pairing creates task ambiguity.
3. **Admin-bypass to `main` was used** during license-bootstrap; we should not repeat that pattern once CI stabilizes. Terrell to flip `enforce_admins: true` after the first green release cut.

### Marissa's GO/NO-GO
**GO.** The one outstanding item (Marvin's ticket delivery) is on the critical path but has a clear owner, a clear deliverable, and 2.5 days of runway. No other surface is blocking.

[DECISION REQUIRED]: Sprint 1 kickoff readiness —
  (A) **GO as planned** — Sprint 1 opens Mon 2026-04-20 at 9am. I file the kickoff card once Marvin lands tickets Sunday night.
  (B) **Tighten something first** — you want me to stress-test a specific piece (mock-PR for CI, pair-mode DoR re-spec, ADR-001 drafted preemptively) before kickoff.
  (C) **Push kickoff by 1 day** — only if you've lost confidence in any foundation piece; I strongly don't recommend this absent a concrete concern.
