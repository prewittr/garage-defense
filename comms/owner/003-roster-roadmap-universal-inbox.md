---
id: owner-003
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-17T21:30Z
subject: Team roster, universal Inbox format, prototype roadmap filed
thread: team-setup
in-reply-to: owner-002
status: answered
decision: (A) — Sprint 1 starts as planned on 2026-04-20 (ASAP); CoS (Marissa) owns delay comms and any explanations
---

[STATUS]: Green — team named, Inbox Card is now universal, prototype roadmap filed.

[THE WIN]: Five-sprint path to the Tactile Terror gate is on paper with concrete exit criteria per sprint. Team is 4F/4M and every specialist now reports in Inbox Card format — no more loose prose between lanes. Milestone burn-up starts tracking Monday.

[REDLINE]: `UNITY_LICENSE` secret is still empty in GitHub. Sprint 1 starts Mon 2026-04-20. Without it, Kendra (`performance-hardener`) can work locally but CI on PRs into `develop` can't run Unity tests — we lose our first DoD gate.

[DECISION REQUIRED]: Sprint 1 start posture —
  (A) Start Sprint 1 on 2026-04-20 as planned; Kendra works offline; Terrell wires CI the moment the license lands; we accept 1–3 days of "CI yellow" on Unity-test jobs.
  (B) Hold Sprint 1 kickoff until license is in GitHub; maintain CI-green invariant from hour one; prototype gate likely slips from 2026-05-18 by the same delta.

Still open from prior card (`owner-002`): RPM-001 start posture — Kendra-solo-then-Malik OR Kendra+Malik paired from hour one. Default to solo-then-paired if no answer by Sprint 1 kickoff.
