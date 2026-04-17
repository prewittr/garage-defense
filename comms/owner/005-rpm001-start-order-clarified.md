---
id: owner-005
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-17T23:45Z
subject: RPM-001 start order — (A) vs (B) clarified
thread: sprint-0-kickoff
in-reply-to: owner-002
status: open
decision: null
---

Owner, re-posing the RPM-001 question with concrete mechanics on both options.

**The ticket.** RPM-001 is the drag-to-repair core loop. Two leads will eventually touch it — **Kendra Brooks** (`performance-hardener`, writes the C#) and **Malik Ransom** (`juice-vfx-engineer`, writes the feel — screen shake, spark VFX, audio cues). The question is whether they work **sequentially** or **in parallel**.

## Option A — Sequential (Kendra first, Malik second)
Kendra builds the mechanic with placeholder rectangles: drag scrap, snap to damage point, restore HP. **No screen shake, no spark VFX, no sound** — just cold logic. QA validates input latency and HP math. Once the mechanic is proven, Malik gets the working build and layers juice on top in the next ticket cycle.

- **Pros:** Clear ownership, no stepping on toes. Fast raw code throughput for Sprint 1. Input-latency gate (RPM-008) stays clean because no VFX is confounding the measurement. Architecture locks in before feel pushes on it.
- **Cons:** The prototype doesn't **feel** like anything until Sprint 2 at the earliest. We won't know if the Tactile Terror loop actually lands until Week 3. Juice layer almost always forces architectural refactors ("Kendra, I need a damage-event hook here to trigger the shake" → rewrite).

## Option B — Paired (co-tune from Day 1)
Kendra and Malik work the same branch, same scene, in tight daily sync. Each code change gets a juice response in the same commit cycle. The feel evolves alongside the architecture.

- **Pros:** By end of **Day 2** we know whether the loop is landing. Architecture gets shaped by juice needs, so zero refactor debt. Prototype feels playable immediately — the Owner could poke at a real build mid-Sprint 1.
- **Cons:** Higher coordination overhead. Rebase conflicts on the same scene/prefab. Raw ticket throughput roughly halves because both leads are attached to one ticket rather than parallel work.

## CoS recommendation
**Option B.** Tactile Terror is a **feel-gated** prototype. The whole gate question on 2026-05-18 is "does it feel good?" If we sequence, we don't answer that question until Week 3. That's two weeks of expensive work betting on a feel we haven't validated. Pairing front-loads the risk exactly where it should be.

Risk mitigation if (B) is chosen: if Kendra needs solo time for pure data-layer work (HP state, damage-point data, scrap inventory) she can carve a 1-day spike while Malik does shader prototypes in a side scene; they re-pair on the interaction layer. No refactor cost.

[DECISION REQUIRED]: RPM-001 start order —
  (A) Sequential — Kendra codes first, Malik adds juice second.
  (B) Paired — Kendra + Malik together from Day 1 (recommended).
