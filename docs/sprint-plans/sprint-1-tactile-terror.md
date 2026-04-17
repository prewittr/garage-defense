# Sprint 1 — The Bones (2026-04-17 14:00 → 2026-04-24 14:00)

**Facilitator:** Marissa Holloway (`command-relay`)
**Date drafted:** 2026-04-17
**Date committed (locked):** 2026-04-17 14:00 — Owner-mandated early kickoff per [owner-014](../../comms/owner/014-starting-sprint-early.md)
**Status:** **Committed / Active**
**One-time cadence exception:** this sprint runs Fri→Fri (7 calendar days). Sprint 2 onward resumes standard Mon–Fri.

## Goal
Land the drag-to-repair core loop with first-pass juice running inside a clean module architecture, gated by a CI-enforced <12ms input-latency budget. By Friday EOD the team has a runnable `TactileTerror_S1` scene on Windows Standalone and (at minimum) the Editor that can be demoed to the Owner.

## Committed Tickets

| Ticket | Owner(s) | Size | Confidence | Risk |
|---|---|---|---|---|
| [RPM-007](../../tickets/RPM-007.md) — asmdef split + VContainer bootstrap | Terrell Vaughn + Kendra Brooks | M | 85% | 🟢 |
| [RPM-001](../../tickets/RPM-001.md) — drag-to-repair core loop + first-pass juice (paired) | Kendra Brooks + Malik Ransom | L | 70% | 🟡 |
| [RPM-008](../../tickets/RPM-008.md) — input latency gate <12ms p95 | Kendra Brooks + Jasmine Whitfield | S | 80% | 🟡 |

**Total size-weighted commitment:** 17 (RPM-007 M=5 + RPM-001 L=10 + RPM-008 S=2)

## Capacity

| Specialist | Available | Notes |
|---|---|---|
| Kendra Brooks (`performance-hardener`) | 5 days | Critical-path on all 3 tickets |
| Malik Ransom (`juice-vfx-engineer`) | 5 days | Paired with Kendra on RPM-001 from Day 1 |
| Terrell Vaughn (`digital-pit-boss`) | 3 days | RPM-007 lead Mon-Tue; CI + release Fri |
| Jasmine Whitfield (`qa-breakdown-analyst`) | 5 days | QA gate Thu–Fri; latency validation mid-week |
| Simone Carver (`atmosphere-architect`) | 2 days | DESIGN-001 Garage View doc (parallel, feeds Sprint 2) |
| Kelvin Abernathy (`monetization-strategist`) | 1 day | Early PRD work on Ghost Refill offer (Sprint 4 prep) |
| Marvin Sinclair (`backlog-elaborator`) | 2 days | Wed refinement owner; preps Sprint 2 tickets |
| Jalen Montgomery (`hook-specialist`) | 1 day | Store-listing early-draft (no dependency yet) |
| Marissa Holloway (`command-relay`) | — | Ceremonies + Owner filter |

## Ceremony Calendar

| Day | Time | Ceremony | Duration | Facilitator |
|---|---|---|---|---|
| **Fri 04-17** | **14:00** | **Sprint Planning** (kickoff) | 30 min | Marissa |
| Fri 04-17 | 14:30 | Pairing Agreement (RPM-001) | 30 min | Kendra + Malik |
| Fri 04-17 | 15:00 → EOD | RPM-007 work begins | — | Terrell + Kendra |
| Sat/Sun 04-18/19 | — | Optional / async only | — | No CoS in chair |
| Mon–Fri | by 10:00 | **Daily Standup** (async Inbox Card) | 3-line card | each → Marissa |
| Mon 04-20 | 14:00 | **Tech Design Review** (RPM-001) | 20 min | Kendra |
| Mon 04-20 | async | **ADR-001 Review** (VContainer) | — | Kendra proposes |
| Wed 04-22 | 14:00 | **Backlog Refinement** (Sprint 2) | 30 min | Marvin |
| Fri 04-24 | 14:00 | **Sprint Review** (end of sprint) | 20 min | Marissa |
| Fri 04-24 | 14:20 | **Retrospective** | 10 min | Marissa |
| Fri 04-24 | 16:00 | **Release Cut** (tri-sign) if stable | 15 min | Terrell |

## Critical Path

```
Mon ─── Tue ─── Wed ─── Thu ─── Fri
│                                 │
RPM-007 ─┐                        │
         └─→ RPM-001 ─────────────┤
         └─→ RPM-008 ──────────────┘
```

**Sequence:** RPM-007 **must** land by EOD Tuesday or Wednesday at latest. Every additional day it slips compresses RPM-001 and RPM-008 into a shrinking window. If RPM-007 is not merged by EOD Wed, Marissa files a Redline Escalation card.

## Known Risks

| Risk | Owner | Mitigation |
|---|---|---|
| Unity CI first-run flake (game-ci / .ulf machine-binding) | Terrell | Retry up to 3×; if still failing, file Redline + flip to local builds for the week |
| Paired mode (Kendra + Malik) unexercised; scene/prefab merge conflicts likely | Marvin + Kendra | Pairing-agreement conversation Mon morning post-planning; daily rebase discipline |
| <12ms latency gate on ubuntu-latest runner jitter | Kendra | If gate flakes, re-spec gate threshold with data + file Redline card |
| Admin-bypass habit on `main` | Terrell | Retire post-first-release cut; flip `enforce_admins: true` |

## Dependencies on Owner

| Item | Deadline | Status |
|---|---|---|
| Review & respond to Mon morning kickoff card | Mon 12:00 | Pending |
| Review & respond to Fri Sprint Review card | Fri EOD | Pending |
| Respond to any Redline Escalation cards | Same-day | Pending |

## Exit Criteria (What Has to Be True Friday EOD)

- [ ] RPM-007, RPM-001, RPM-008 all merged to `develop` with `Tested-By:` trailers.
- [ ] ADR-001 (VContainer) Accepted and filed at `docs/architecture/adr/ADR-001-vcontainer.md`.
- [ ] `docs/architecture/ARCHITECTURE.md` v0.2 — aspirational modules 🚧 flipped to ✅.
- [ ] `TactileTerror_S1` scene runs in Editor and Windows Standalone; drag-to-repair loop is demo-able.
- [ ] Input latency p95 ≤ 12ms measured and recorded in `docs/qa/latency-baseline.md`.
- [ ] Zero S1/S2 bugs open.
- [ ] `v0.1.0` tagged on `main` (if release cut executes) with release notes filed.
- [ ] Retro filed at `docs/retros/sprint-1.md`.

## Daily Burn-Down (updated during sprint)

| Day | Tickets Done | In Progress | Blockers |
|---|---|---|---|
| Fri 04-17 (pm) | | | |
| Sat/Sun | (optional/async) | | |
| Mon 04-20 | | | |
| Tue 04-21 | | | |
| Wed 04-22 | | | |
| Thu 04-23 | | | |
| Fri 04-24 (am) | | | |

## Spillover (appended if needed)

_None yet._
