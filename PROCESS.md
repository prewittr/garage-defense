# Rpm: Redline / Deadline — Agile Playbook v2.0

> Canonical reference for how work flows from an Owner vision directive to a shipped build.
> Companion to [CLAUDE.md](CLAUDE.md) (who we are + how we communicate) and [ROADMAP.md](ROADMAP.md) (what we're building, when).

**Authored by:** Marissa Holloway (`command-relay`) + Claude (process architect)
**Version:** 2.0
**Last reviewed:** 2026-04-17
**Status:** Accepted by Owner 2026-04-17.

---

## 1. Philosophy
- **Scrum-ish.** 1-week sprints. Weekly release train on stable `develop`.
- **Async-first.** Inbox Cards > meetings. Every ceremony caps at 30 min.
- **Single-accountable.** Every artifact has one named owner. No committees.
- **Shrink-before-sprint.** Tickets leave the backlog only as **Ready**.
- **Show, don't tell.** Every Sprint Review is a runnable artifact, not slides.

## 2. Cadence
| | |
|---|---|
| Sprint length | 1 week (Mon → Fri) |
| Release train | Weekly `develop` → `main` tag if stable (Friday 4pm cut) |
| Hotfix path | Cut `hotfix/*` from `main`, merge to both `main` and `develop` |
| Sprint 0 | Scaffold only; no feature tickets |

## 3. Agile Roles

| Role (agent) | Name | Agile function | Ceremonies owned |
|---|---|---|---|
| `command-relay` | Marissa Holloway | Scrum Master + CoS | Sprint Planning, Review, Retro |
| `backlog-elaborator` | Marvin Sinclair | Product Backlog Owner | Backlog Refinement |
| `monetization-strategist` | Kelvin Abernathy | Lead PM (monetization) | Stage 3 PRDs |
| `atmosphere-architect` | Simone Carver | Lead Game Designer | Stage 3 design docs |
| `performance-hardener` | Kendra Brooks | Tech Lead | Tech Design Review, Code Review |
| `juice-vfx-engineer` | Malik Ransom | Technical Art Lead | Juice contribution to tech design |
| `digital-pit-boss` | Terrell Vaughn | DevOps / SecOps | CI/CD, Release Cut, ADR co-review |
| `qa-breakdown-analyst` | Jasmine Whitfield | QA Lead | Test planning, QA gate |
| `hook-specialist` | Jalen Montgomery | Growth / UA | Store-listing readiness |

## 4. The 10-Stage Pipeline

Every feature traverses these stages. Each has a trigger, a single accountable owner, a concrete artifact, and a DoD.

| # | Stage | Owner | Input | Artifact Produced | DoD |
|---|---|---|---|---|---|
| 0 | Idea | Owner | Vision | (informal) | Articulable in 1–2 sentences. |
| 1 | Directive | Owner + Marissa | Idea | Marissa ack Inbox Card | Marissa can repeat back unambiguously. |
| 2 | Decomposition | Marissa | Directive | Work-stream list | Each stream has a specialist owner. |
| 3 | Specification | Kelvin (PRD) · Simone (Game Design) · Kendra (Tech Design) | Work stream | PRD · Game Design Doc · Tech Design Doc · ADR proposal if architectural | Docs meet template fields. |
| 4 | Elaboration | **Marvin (BET Lead)** | Specs | User Story files in `/tickets/` · Test Plans (with Jasmine) | INVEST + DoR met. |
| 5 | Sprint Planning | Marissa | Ready tickets | `sprint-plan.md` · Owner Inbox Card | Committed tickets have owner + estimate + ≥70% confidence. |
| 6 | Implementation | Kendra (lead) + Malik / Terrell as scoped | Tickets + specs | Commits on `feature/*` + PR | Compiles, tests pass, PR open. |
| 7 | Code Review | Kendra | PR | Approval or review notes | No hot-path alloc, asmdef clean, tests present. |
| 8 | QA | Jasmine | Build from PR | `Tested-By:` trailer OR Bug Report | Device matrix pass, 0 S1/S2. |
| 9 | Merge | Terrell | Approved + Tested-By PR | Merge commit on `develop` | Conventional Commit title, CI green, no force-push. |
| 10 | Release | Terrell + Kendra + Marissa (tri-sign) | `develop` at stable SHA | Signed tag on `main` + release notes | Sprint DoD met, rollback plan documented. |

## 5. Workflow States

```
Backlog → Spec'd → Ready → In Sprint → In Progress → In Review → In QA → Done
```

| State | Precondition | Held by |
|---|---|---|
| Backlog | Feature captured (epic) | Marvin |
| Spec'd | PRD + Design Doc(s) exist | Marvin |
| **Ready** | DoR met — see §7 | Marvin |
| In Sprint | Committed at Sprint Planning | Marissa |
| In Progress | Assignee picked up | Specialist |
| In Review | PR opened to `develop` | Kendra |
| In QA | Code review passed | Jasmine |
| **Done** | DoD met — see §8 | Marissa (final gate) |

Moving between states requires the owner of the receiving state to accept (or reject with a specific reason).

## 6. Estimation
T-shirt sizes only. No story points.

| Size | Meaning |
|---|---|
| XS | < 1 day |
| S | 1–2 days |
| M | 3–5 days (most of a sprint) |
| L | Full sprint |
| XL | Break it down. Too big to commit atomic. |

## 7. Definition of Ready (DoR)
A ticket is Ready only when **all** checked:
- [ ] INVEST: Independent · Negotiable · Valuable · Estimable · Small · Testable
- [ ] User story present (As a / I want / So that)
- [ ] Acceptance criteria in Given/When/Then form
- [ ] Dependencies identified; blocked-by tags on unmet deps
- [ ] Estimate agreed (XS/S/M/L)
- [ ] Test plan sketched (P0 cases minimum)
- [ ] Owner assigned
- [ ] Risk flags raised (🔴/🟡/🟢)

Non-Ready tickets **cannot** be committed in Sprint Planning.

## 8. Definition of Done (DoD)
A ticket is Done only when **all** checked:
- [ ] Code review passed (Kendra)
- [ ] CI green (Terrell) — compile, test runner, latency gate
- [ ] QA passed on device matrix (Jasmine) — `Tested-By:` trailer on merge commit
- [ ] 60 FPS maintained on target devices
- [ ] Demo-able artifact available to Marissa
- [ ] No new S1/S2 bugs introduced

## 9. Ceremonies

### Sprint Planning
- **When:** Monday 9am
- **Duration:** 30 min
- **Facilitator:** Marissa
- **Participants:** All specialists (async OK)
- **Input:** Marvin's READY list (delivered EOD Wednesday prior)
- **Output:** `sprint-plan.md` + Owner Inbox Card
- **Exit:** Every committed ticket has owner + estimate + ≥70% confidence

### Daily Standup
- **When:** Each working day by 10am local
- **Duration:** Async — 3-line Inbox Card
- **Format:** Yesterday / Today / Blockers → to Marissa
- **Marissa** consolidates into daily digest card only if redlines emerge

### Backlog Refinement
- **When:** Wednesday 2pm
- **Duration:** 30 min
- **Facilitator:** Marvin
- **Participants:** Marvin, Kelvin, Simone, Kendra (feasibility), Jasmine (test plans)
- **Output:** Tickets move Spec'd → Ready

### Sprint Review
- **When:** Friday 2pm
- **Duration:** 20 min
- **Facilitator:** Marissa
- **Participants:** Whole team
- **Output:** Demo package + Inbox Card to Owner

### Retrospective
- **When:** Friday 2:20pm (bundled with Review)
- **Duration:** 10 min
- **Facilitator:** Marissa
- **Output:** `retro.md` + any PROCESS amendments filed as Inbox Cards

### Release Cut
- **When:** Friday 4pm (if `develop` is stable)
- **Duration:** 15 min
- **Facilitator:** Terrell
- **Tri-sign:** Terrell + Kendra + Marissa
- **Output:** Signed tag on `main` + release notes + rollback plan

### Tech Design Review
- **When:** As-needed, Stage 3→4 handoff
- **Duration:** 20 min
- **Facilitator:** Kendra
- **Participants:** Kendra, Terrell, Malik (if VFX-adjacent)
- **Output:** Tech Design Doc approved OR redlines to address

### ADR Review
- **When:** Async, any time
- **Facilitator:** Proposer (usually Kendra)
- **Output:** ADR moves Proposed → Accepted / Rejected

### Redline Escalation (ad-hoc)
- **When:** Any time 60 FPS / launch date / compliance is at risk
- **Facilitator:** Whoever detects it
- **Output:** Inbox Card to Marissa immediately; she escalates to Owner if pillar-level

## 10. Artifact Layout

```
/
├─ CLAUDE.md             team identity + communication hierarchy
├─ PROCESS.md            this file
├─ ROADMAP.md            sprint-by-sprint plan
├─ SECRETS.md            secret names (no values)
├─ docs/
│  ├─ README.md          documentation index
│  ├─ architecture/
│  │  ├─ ARCHITECTURE.md          living top-level system doc (Kendra)
│  │  ├─ DATA-FLOW.md             client↔server diagrams (Kendra + Terrell)
│  │  └─ adr/                     immutable decision log
│  │     └─ ADR-NNN-*.md
│  ├─ design/
│  │  ├─ game/                    Simone's player-facing design docs
│  │  │  └─ DESIGN-NNN-*.md
│  │  └─ tech/                    Kendra's per-feature technical designs
│  │     └─ FEAT-NNN-*.md
│  ├─ prd/                        Kelvin's PRDs
│  │  └─ PRD-NNN-*.md
│  ├─ qa/
│  │  ├─ device-matrix.md
│  │  ├─ regression-suite.md
│  │  └─ test-plans/
│  │     └─ TP-RPM-NNN.md
│  └─ templates/                  copy-from templates for every artifact
│     ├─ feature.md
│     ├─ user-story.md
│     ├─ prd.md
│     ├─ game-design.md
│     ├─ tech-design.md
│     ├─ architecture.md
│     ├─ adr.md
│     ├─ test-plan.md
│     ├─ bug-report.md
│     ├─ sprint-plan.md
│     ├─ retro.md
│     └─ release-notes.md
├─ tickets/                       Ready/In-Progress/Done tickets (Marvin owns)
│  ├─ _README.md                  ticket numbering + lifecycle
│  ├─ RPM-NNN.md                  user stories
│  └─ BUG-NNN.md                  bug reports
├─ comms/                         per-person Inbox Card archive
├─ Assets/ Packages/ ProjectSettings/    Unity project
└─ .github/workflows/             CI
```

Folders not yet populated will be created lazily when their first artifact lands.

## 11. Templates Index
Canonical templates in [`docs/templates/`](docs/templates/). Copy, don't mutate.

| Template | Used by | Stage |
|---|---|---|
| [feature.md](docs/templates/feature.md) | Marvin, Marissa | 2 |
| [user-story.md](docs/templates/user-story.md) | Marvin | 4 |
| [prd.md](docs/templates/prd.md) | Kelvin | 3 |
| [game-design.md](docs/templates/game-design.md) | Simone | 3 |
| [tech-design.md](docs/templates/tech-design.md) | Kendra | 3 |
| [architecture.md](docs/templates/architecture.md) | Kendra | Continuous |
| [adr.md](docs/templates/adr.md) | Kendra + reviewers | Any |
| [test-plan.md](docs/templates/test-plan.md) | Jasmine | 4 |
| [bug-report.md](docs/templates/bug-report.md) | Jasmine | 8 |
| [sprint-plan.md](docs/templates/sprint-plan.md) | Marissa | 5 |
| [retro.md](docs/templates/retro.md) | Marissa | End of sprint |
| [release-notes.md](docs/templates/release-notes.md) | Terrell | 10 |

## 12. Metrics
Tracked by Marissa, reported in each Retro:
- **Velocity** — tickets/sprint, size-weighted (XS=1, S=2, M=5, L=10).
- **Cycle time** — Ready → Done, median.
- **Escaped defects** — bugs past QA but failing post-release.
- **Owner approval rate** — % of Sprint Reviews accepted without redlines.
- **DoR violation rate** — tickets that failed refinement, % of attempted.

## 13. Amendments
| Version | Date | Change | Approved by |
|---|---|---|---|
| 1.0 | 2026-04-17 | Initial process documentation (10-stage pipeline, gap analysis) | pending |
| 2.0 | 2026-04-17 | Full agile playbook: philosophy, cadence, ceremonies, DoR/DoD, estimation, templates, metrics, architecture/design/test documentation | Owner (Roderick Prewitt) |
