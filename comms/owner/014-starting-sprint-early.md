---
id: owner-014
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-17T19:30Z
subject: Starting Sprint 1 early (tonight / weekend)? — honest CoS read
thread: sprint-1-kickoff
in-reply-to: owner-013
status: open
decision: null
---

[STATUS]: Yellow — Owner is considering an early start; CoS position is *protect the cadence,* but the call is yours.

[THE WIN]: Team has genuine momentum. Marvin finished 2 days ahead of his own deadline. Every prerequisite is landed. Prototype feel is a fuse that wants to be lit.

[REDLINE]: Three real risks to starting now:

1. **1-week cadence breaks on first sprint.** We codified Mon–Fri sprints in PROCESS.md §2 four hours ago. If Sprint 1 starts Friday evening, every future sprint either runs 8-day (breaking velocity math) or we have a permanent "phantom sprint" asterisk. Breaking our own process before we've run it once is the exact failure mode we wrote the process to prevent.

2. **Friday evening is the worst build window.** Unity-CI first-run flake is the single biggest risk we've been tracking (owner-009, owner-012). A tired team hitting a flaky first PR at 10pm is how weekends get eaten. And if anything goes wrong, there is no CoS in chair and no Owner checkpoint — no escape valve.

3. **Paired mode (Kendra + Malik on RPM-001) is unexercised.** That decision from owner-005 needs a 30-min working-agreement conversation — scene hygiene, branch-sharing, merge cadence — before they pair on a real commit. That conversation should happen Monday morning at planning, not on a tired Friday night.

[DECISION REQUIRED]: When does Sprint 1 clock-start?

  (A) **Tonight — start now.** Not recommended. Breaks cadence, accumulates tech debt from tired work. If you want the momentum, it's cheaper to channel it into weekend prep than weekend commits.

  (B) **Soft start Sunday evening.** Kendra async-reads her tickets, optionally sketches RPM-007 asmdef layout in a scratch branch. No commits to `feature/tactile-terror`, no scene changes. Monday 9am remains the official clock-start with full Sprint Planning ceremony.

  (C) **Hold Monday 9am as planned.** Protects cadence, protects the kickoff ritual, protects the team's weekend. **My lean.**

Caveat on any option: if Kendra, Malik, or Terrell voluntarily want to read tickets or architecture over the weekend on their own time, that's always allowed. Async prep is not "starting the sprint." The sprint clock and the DoD trailer chain start Monday.
