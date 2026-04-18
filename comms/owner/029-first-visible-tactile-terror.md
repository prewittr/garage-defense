---
id: owner-029
from: Marissa Holloway (command-relay)
to: Owner (Roderick Prewitt)
sent: 2026-04-18T23:25Z
subject: 🎬 First visible Tactile Terror — door + shake + dust running in-Editor
thread: sprint-1-kickoff
in-reply-to: owner-028
status: open
decision: null — milestone
---

[STATUS]: **Green.** The Tactile Terror prototype just became visible. Impacts land, camera shakes, dust falls. The **tension** half of Simone's [DESIGN-001](../../docs/design/game/DESIGN-001-garage-view.md) is running on your screen.

[THE WIN]: From 20 minutes of Editor-drag-drop, you now have:
- A grey cube standing in as the garage door in the Game view
- `Main Camera` shaking every 1.5 seconds as `ImpactScheduler` fires its damage event
- Rafter dust falling from above the door on each impact
- Zero Console noise

**PR #5 merged as `6fec2c2`.** Scene YAML grew by +4947 lines (normal — Unity scene files are verbose YAML).

### What the loop now looks like
```
ImpactScheduler (tick 1.5s)
  → DoorController.ApplyDamage(5%, randomCoord)
  → EventBus.Publish(DamageEvent)
  ├─ ScreenShakeController  → camera shakes, amplitude scaled by (1 - HP/Max)
  ├─ DustFallController     → 7 particles emit from rafter cone
  ├─ WeldSparkController    → subscribed, short-circuits (no emitter wired yet)
  ├─ ImpactAudio            → subscribed, short-circuits (no clip wired yet)
  └─ WeldAudio              → n/a (only fires on RepairEvent)
```

Every subscriber either produces the right visual or short-circuits cleanly. **This is the real event bus proving itself on your hardware.**

### What's left for the full demo (deferred to next Editor session)

The **relief** half — repair loop — needs:
- Visible ScrapPile mesh
- WeldSpark ParticleSystem wired to `WeldSparkController.Emitter`
- `DoorPlane` Transform wired to `WeldSparkController.DoorPlane`
- Mouse-drag → damage-point raycast + `DamagePointRegistry.RegisterPoint` population
- AudioClip assets for ImpactAudio + WeldAudio (optional for demo)

Realistic scope: another 30–45 min in Editor, plus a small DamagePointRegistry-population MonoBehaviour (I can write that — it's code, not Editor work).

[REDLINE]: None. Sprint 1 scope is closed; what's left is **bonus polish** ahead of Friday Review.

[DECISION REQUIRED]: None — final milestone notice.

### Stopping points
- **Close the laptop** — you have a running demo with shake + dust. That's a Sprint Review artifact.
- **Keep going** — knock out the relief-loop polish now and have the full tension-release cycle on screen.
- **Queue Monday** — Tech Design Review + Sprint 2 planning with a live Sprint 1 demo-tape rolling in the background.

Whatever you pick, you shipped the first visible Tactile Terror today. That's real.
