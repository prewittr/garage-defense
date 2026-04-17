# [FEAT-001] Kinetic Door

**Pillar:** 10-Second Hook + High-Stakes Resource Management
**Owner:** Simone Carver (`atmosphere-architect`) — game-design lead for the feature
**Status:** In-Sprint (Sprint 1)

## Tension → Release
Rhythmic zombie impacts chew through door Integrity → player drags scrap to a damage point and feels a jolt of safety when a hole seals.

## Problem
If the player doesn't feel a **physical jolt of anxiety** when the door takes a hit and a **rush of relief** when they fix it, the entire game fails. The prototype must prove this feel loop before any monetization work begins.

## Outcome
By end of Sprint 4 (Prototype Gate 2026-05-18): ≥35% of failed prototype-test runs contain at least one "ghost refill" tap. That rate proves the Relief loop is landing and the monetization hypothesis (RPM-005) is worth building out.

## Stories that compose this feature
- [RPM-001](RPM-001.md) — Drag-to-Repair core loop with first-pass juice (Sprint 1)
- [RPM-002](RPM-002.md) — Impact Event Cadence (Sprint 2, not yet drafted)
- [RPM-003](RPM-003.md) — Visual Ruin State Machine (Sprint 2, not yet drafted)
- [RPM-004](RPM-004.md) — Full Weld VFX layer (Sprint 3, not yet drafted)
- [RPM-005](RPM-005.md) — Scarcity Squeeze + Ghost Refill telemetry (Sprint 4, not yet drafted)
- [RPM-006](RPM-006.md) — Fail State "RESOURCES EXHAUSTED" (Sprint 4, not yet drafted)

## Non-goals
- No real art this feature-cycle — placeholder sprites until Sprint 3 polish.
- No zombie models / enemy characters — impacts are rhythmic events, not simulated enemies.
- No monetization UX — Refill button is a **ghost tap-zone** only (RPM-005).

## Open Questions
- Shader variant strategy for visual ruin states (likely first ADR).
- Whether to use Rigidbody2D or custom physics impulse for the fail-state door explosion.

## Related Artifacts
- Game Design Doc: `docs/design/game/DESIGN-001-garage-view.md` (pending — Simone, Sprint 1)
- Tech Design Doc: `docs/design/tech/FEAT-001-kinetic-door.md` (pending — Kendra, Sprint 1)
- PRD (Ghost Refill monetization hook): `docs/prd/PRD-001-refill-offer.md` (pending — Kelvin, Sprint 3)
