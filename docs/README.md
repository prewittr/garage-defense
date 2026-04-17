# /docs — Documentation Index

Canonical home for architecture, design, PRDs, QA, and reusable templates.

## Contents

| Folder | Purpose | Owner |
|---|---|---|
| [architecture/](architecture/) | Top-level system doc, data flow, ADR log (immutable) | Kendra Brooks |
| [design/game/](design/game/) | Player-facing design docs — fantasy, loops, input tables | Simone Carver |
| [design/tech/](design/tech/) | Per-feature technical designs | Kendra Brooks |
| [prd/](prd/) | Product requirement docs — economy, monetization, compliance | Kelvin Abernathy |
| [qa/](qa/) | Device matrix, regression suite, per-ticket test plans | Jasmine Whitfield |
| [templates/](templates/) | Copy-from templates for every artifact | Marvin Sinclair (BET Lead) curates |

## Rules
1. **Copy, don't mutate templates.** Every artifact file is a *copy* of its template with `<placeholders>` replaced.
2. **One file per thing.** One PRD per product decision. One ADR per architectural decision. One user story per ticket.
3. **Immutable once ACCEPTED** for ADRs and Retros. Everything else is living until DONE.
4. **Numbered prefixes** on filenames: `PRD-NNN`, `RPM-NNN`, `ADR-NNN`, `FEAT-NNN`, `TP-RPM-NNN`, `BUG-NNN`.

## Links
- [PROCESS.md](../PROCESS.md) — full agile playbook
- [CLAUDE.md](../CLAUDE.md) — team identity + comms hierarchy
- [ROADMAP.md](../ROADMAP.md) — sprint-by-sprint plan
