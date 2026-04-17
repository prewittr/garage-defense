# Communications Archive

Durable record of every **Inbox Card** exchanged on Rpm: Redline / Deadline.

## Convention
- One folder per person (Owner + 8 agents).
- Each message is one markdown file named `NNN-short-subject.md` (zero-padded).
- The file lives in the **recipient's** folder only. Sender is declared in frontmatter.
- To view "messages sent by X," grep `from: X` across `comms/*/`.

## Folders
| Folder | Person | Role |
|---|---|---|
| `owner/` | **Roderick Prewitt** | Studio Owner |
| `marissa-holloway/` | Marissa Holloway (she/her) | Chief of Staff (`command-relay`) |
| `kelvin-abernathy/` | Kelvin Abernathy (he/him) | Product & Revenue (`monetization-strategist`) |
| `simone-carver/` | Simone Carver (she/her) | Lead Game Designer (`atmosphere-architect`) |
| `malik-ransom/` | Malik Ransom (he/him) | Lead Technical Artist (`juice-vfx-engineer`) |
| `kendra-brooks/` | Kendra Brooks (she/her) | Lead Dev & Git Master (`performance-hardener`) |
| `terrell-vaughn/` | Terrell Vaughn (he/him) | SecOps & DevOps (`digital-pit-boss`) |
| `jasmine-whitfield/` | Jasmine Whitfield (she/her) | QA & Stability (`qa-breakdown-analyst`) |
| `jalen-montgomery/` | Jalen Montgomery (he/him) | Growth & UA (`hook-specialist`) |

## Message format
Each file has YAML frontmatter and an Inbox Card body:

```markdown
---
id: owner-001
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-17T18:30Z
subject: One-line summary
thread: sprint-0
status: answered | open | ack
decision: the Owner's A/B response (if applicable)
---

[STATUS]: ...
[THE WIN]: ...
[REDLINE]: ...
[DECISION REQUIRED]: ...
```

## Rules
1. **Only `command-relay` (Marissa)** writes to `owner/` for specialist reports. Exceptions: `digital-pit-boss` may write directly to `owner/` for P0 security/infra escalations (see CLAUDE.md).
2. Every card ends with a `[DECISION REQUIRED]` or it shouldn't have been sent.
3. When the Owner answers, the recipient updates the card's `status:` and `decision:` fields in-place, then files a follow-up if further action was triggered.
4. Never delete a card. Append a reply card instead.
