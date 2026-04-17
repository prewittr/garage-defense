# Development Process v1.0

> Canonical reference for how work flows from an Owner vision directive to a shipped feature.
> Companion to [CLAUDE.md](CLAUDE.md) — where CLAUDE.md defines **who** we are and **how** we communicate, PROCESS.md defines **what** we do day-to-day.

**Authored by:** Marissa Holloway (`command-relay`) + Claude (process architect)
**Version:** 1.0
**Last reviewed:** 2026-04-17
**Status:** Draft awaiting Owner sign-off on Stage 4 ownership.

---

## The 10-Stage Pipeline

Every feature traverses these stages. Each stage has a trigger, a single accountable owner, a concrete artifact, and a Definition-of-Done condition. Nothing is "in progress" without living in one of these stages.

| # | Stage | Owner | Input | Output Artifact | DoD |
|---|---|---|---|---|---|
| 0 | **Idea** | Owner | Vision | (informal) | Owner can articulate it in 1–2 sentences. |
| 1 | **Directive** | Owner + Marissa | Idea | Inbox Card ack to Marissa | Marissa can repeat back unambiguously. |
| 2 | **Decomposition** | Marissa | Directive | Work-stream list (sprint-planning notes) | Each stream has a specialist owner; no overlap. |
| 3 | **Specification** | Kelvin (PRD) / Simone (Design) | Work stream | `/docs/prd/<feat>.md` or `/docs/design/<feat>.md` | Meets fields mandated in agent system prompt. |
| 4 | **Elaboration** | **UNASSIGNED (gap)** | PRD + Design | Ticket files `/tickets/RPM-NNN.md` | Meets INVEST — user story, AC, DoD checklist, deps, estimate, risk flags. |
| 5 | **Sprint Planning** | Marissa (facilitates) | Ready tickets | ROADMAP.md sprint update | Each selected ticket has owner + estimate + confidence ≥ 70%. |
| 6 | **Implementation** | Kendra (lead) + Malik / Terrell as scoped | Ready tickets | Commits on `feature/*` branches; PR opened to `develop` | Compiles, tests pass locally, no perf regression, PR opened. |
| 7 | **Code Review** | Kendra | PR | PR approval or review notes | No allocations in hot path, asmdef clean, tests present, docs on public APIs. |
| 8 | **QA** | Jasmine | Build from PR branch | `Tested-By:` trailer or bug file `/tickets/BUG-NNN.md` | Device-matrix pass, 120-sec purchase path when in scope, S1 bugs zero. |
| 9 | **Merge** | Terrell | Approved + Tested-By PR | Merge commit on `develop` | Conventional Commit title, `Tested-By:` trailer, CI green, no force-push. |
| 10 | **Release** | Terrell + Kendra + Marissa (tri-sign) | `develop` at stable SHA | Signed tag on `main`; store uploads when applicable | Sprint DoD met, release notes generated, rollback plan documented. |

---

## Where Inbox Cards Fire

The Inbox Card is not noise — it is filed at specific **stage boundaries**, not on every commit.

| Card source | Fired at | Notes |
|---|---|---|
| Marissa → Owner | Stage 1 ack, Stage 5 kickoff, Stage 10 release, any redline breach | One card, one decision. |
| Kelvin → Marissa | End of Stage 3 with PRD link | Internal, Channel 2. |
| Simone → Marissa | End of Stage 3 with design-doc link | Internal, Channel 2. |
| BET Lead (if hired) → Marissa | End of Stage 4 with READY ticket list | Internal, Channel 2. |
| Kendra → Marissa | Stage 6 complete (PR open), Stage 7 block | Escalate only redlines to Owner. |
| Jasmine → Kendra | Stage 8 pass/fail | Direct to Hardener (Channel 3). |
| Terrell → Owner | Infra/security P0 only (bypasses Marissa) | Explicit CLAUDE.md exception. |

A cross-stage handoff without a card is a process violation.

---

## Rituals (Proposed)

Rituals are lightweight — all async via Inbox Cards — unless explicitly marked synchronous.

| Ritual | Cadence | Owner | Output |
|---|---|---|---|
| **Sprint Planning** | Monday of Week 1 | Marissa | Sprint backlog locked in ROADMAP.md; Inbox Card to Owner. |
| **Backlog Refinement** | Wednesday of Week 1 | BET Lead (or Kelvin+Simone jointly) | Next-sprint tickets move to READY. |
| **Daily Standup** | Each working day, async | Each specialist | 3-line Inbox Card to Marissa: yesterday / today / blockers. |
| **Sprint Review** | Friday of Week 1 (or end of sprint) | Marissa | Demo Package + Inbox Card to Owner. |
| **Retro** | Same day as Review | Marissa | Process amendment proposals if any. |

Duration target for all synchronous rituals: **≤30 minutes.**

---

## Artifact Layout

```
/
├─ CLAUDE.md             team identity + communication hierarchy
├─ PROCESS.md            this file
├─ ROADMAP.md            sprint-by-sprint plan, updated each sprint
├─ SECRETS.md            secret names (no values)
├─ docs/
│  ├─ prd/               Kelvin's PRDs (Stage 3 output)
│  └─ design/            Simone's design docs (Stage 3 output)
├─ tickets/
│  ├─ _README.md         ticket format + INVEST checklist
│  ├─ RPM-001.md         each ready ticket
│  ├─ BUG-NNN.md         QA-filed bugs from Stage 8
│  └─ ...
├─ comms/
│  ├─ _README.md         comms archive convention
│  └─ <person>/          per-person Inbox Cards
├─ Assets/ Packages/ ProjectSettings/   Unity project
└─ .github/workflows/    CI
```

Folders marked above that don't exist yet will be created when their first artifact lands (lazy creation — no empty scaffolds).

---

## Gap Analysis

Where ownership, artifacts, or DoD are fuzzy today:

### Stage 4 — Elaboration (real gap)
**No dedicated owner.** Kelvin has done some of this implicitly during Sprint 0 setup, but his agent role is **monetization strategy**, not ticket elaboration. Simone is fully loaded defining the Garage View + cross-platform input table. Marissa shouldn't produce work artifacts — she's meant to filter UP to the Owner, not generate ticket detail.

Current symptom: tickets RPM-001 through RPM-008 exist in the Sprint 0 backlog (in this chat history and in ROADMAP.md), but there is **no** canonical `/tickets/RPM-001.md` file with the INVEST checklist, risk flags, or test plan. If Kendra opens RPM-001 on Monday, she'll work from memory and chat scrollback — which is exactly the failure mode this process exists to prevent.

### Stage 5 — Sprint Planning (soft gap)
No standing ritual yet. Sprint 0 was kicked off ad-hoc. Sprint 1 needs an explicit Monday planning moment — even if async — or tickets will still be slushy on Day 1.

### Stage 8 — QA (latent gap)
Jasmine is declared, but no actual builds to QA yet. This will be real-tested Sprint 1. If builds aren't reachable to her (no TestFlight / Play Internal slot, no PlayFab dev title), Stage 8 stalls and Stage 9 backs up.

---

## Recommendations on Stage 4 Ownership

Three viable options:

### Option A — Hire a dedicated BET Lead
- Single accountable owner for Stage 4.
- Clean division: Kelvin = monetization economy, Simone = design fantasy, BET Lead = ready tickets.
- Adds one agent; team goes 5F/4M.
- **Risk:** one more seat at the Channel 2 table, slightly more coordination.
- **Fit:** best if ticket-creation volume scales past Sprint 1.

### Option B — Expand Kelvin's role to Lead PM + Elaboration
- No new agent. Kelvin's system prompt expands to include ticket elaboration.
- **Risk:** his monetization output dilutes; PRDs compete with tickets for his attention.
- **Fit:** workable for very small sprints but breaks when the team ships parallel workstreams (which Sprint 1 already does with RPM-001/007/008 paired).

### Option C — Shared Kelvin + Simone refinement session
- Wednesday 30-min joint refinement; Marissa facilitates.
- **Risk:** steals Marissa's filter-bandwidth; tickets bear two-parent syndrome (ownership ambiguity).
- **Fit:** cheap to try; hard to sustain.

### Process-Architect Recommendation
**Option A.** The gap is concrete, the role is real, and Stage 4 is load-bearing for every other stage — if elaboration is sloppy, Stages 6–9 absorb the rework cost. One dedicated owner pays back inside a single sprint.

---

## Amendments
| Version | Date | Change | Approved by |
|---|---|---|---|
| 1.0 | 2026-04-17 | Initial process documentation. | pending |
