# /tickets — Ticket Archive

Home for every user story and bug on Rpm: Redline / Deadline. Curated by **Marvin Sinclair** (`backlog-elaborator`).

## Numbering Convention

| Prefix | What | Example |
|---|---|---|
| `FEAT-NNN` | Epic / feature (parent to stories) | `FEAT-001-kinetic-door.md` |
| `RPM-NNN` | User story / work ticket | `RPM-001.md` |
| `BUG-NNN` | QA-filed bug (from Jasmine) | `BUG-001.md` |

Numbers are **monotonic and never reused.** If a ticket is cut, mark it `Status: Cut` in the frontmatter and leave the file in place.

## Lifecycle

```
Backlog → Spec'd → Ready → In Sprint → In Progress → In Review → In QA → Done
```

State transitions are recorded in the ticket's `Status:` frontmatter line and committed along with the code change that triggers them.

## Templates

Copy from [`/docs/templates/`](../docs/templates/):

- Features: `feature.md`
- Stories: `user-story.md`
- Bugs: `bug-report.md`

Never mutate the templates themselves.

## Current Sprint Commitment

See [`ROADMAP.md`](../ROADMAP.md) and the active `sprint-plan.md` for which tickets are In Sprint. This folder shows all tickets regardless of state — filter by the `Status:` field in frontmatter.
