# Rpm: Redline / Deadline — Prototype Roadmap v1.0

**Prepared by:** Marissa Holloway (`command-relay`), Chief of Staff
**Date:** 2026-04-17
**Target:** Playable "Tactile Terror" prototype → Owner go/no-go gate

---

## Why a Prototype Roadmap (Not a Feature List)
Sprint 0 proved we can stand up a team and a repo. Now we have to prove the **core feel** — the Door tension-relief loop — before we invest another dollar in monetization, art, or platform work. This roadmap is the shortest credible path to that answer.

## Cadence
- **1-week sprints**, Mon–Fri.
- Every sprint ends with a **CoS Inbox Card** to the Owner + a runnable build on `feature/tactile-terror`.
- Daily standup reports from specialists to CoS in Inbox Card format.

## The Prototype Gate
**End of Sprint 4 (Week of 2026-05-18).** Owner playtests the build on iPhone, Android, and PC. Ghost Refill telemetry (RPM-005) is reviewed. Decision: **commit to the monetization build** or **pivot on feel**.

---

## Sprint Plan

### Sprint 0 — Scaffold ✅ COMPLETE (2026-04-17)
- **Leads:** Terrell Vaughn, Kendra Brooks
- **Tickets:** repo init, Unity 6 skeleton, CI skeleton, team protocol.
- **Exit (met):** commit `deb2fbb` on `main`; `develop` + `feature/tactile-terror` branched.

### Sprint 1 — The Bones (2026-04-20 → 2026-04-24)
- **Co-Leads:** Kendra Brooks (`performance-hardener`) + Malik Ransom (`juice-vfx-engineer`) — **paired on RPM-001 per decision owner-005**
- **Support:** Terrell Vaughn (`digital-pit-boss`), Jasmine Whitfield (`qa-breakdown-analyst`)
- **Tickets:** RPM-001 (drag-to-repair core loop + first-pass juice) · RPM-007 (prototype harness) · RPM-008 (input latency gate)
- **Deliverable:** Drag-to-repair playable in the Editor with co-developed first-pass juice: screen-shake magnitude tied to HP%, basic weld spark VFX, placeholder clink-hiss SFX. Placeholder door/scrap sprites. Full VFX Graph + final shaders arrive Sprint 3.
- **Exit:** p95 input latency <12ms on desktop profiler (juice must not blow the gate). CI green on PR → `develop`.

### Sprint 2 — The Pulse (2026-04-27 → 2026-05-01)
- **Co-Leads:** Kendra Brooks + Simone Carver (`atmosphere-architect`)
- **Support:** Malik Ransom (`juice-vfx-engineer`)
- **Tickets:** RPM-002 (impact cadence) · RPM-003 (visual ruin state machine)
- **Deliverable:** Rhythmic 1.5s zombie-hit loop, 4 visual ruin stages, screen-shake magnitude scaled by (1 − HP%).
- **Exit:** A full wave cycle plays 100% → 0% HP without frame drop or crash.

### Sprint 3 — The Feel (2026-05-04 → 2026-05-08)
- **Lead:** Malik Ransom
- **Support:** Kendra Brooks (perf budgets), Simone Carver (soundscape direction)
- **Tickets:** RPM-004 (weld spark VFX + clink-hiss) + polish passes on 002/003
- **Deliverable:** Weld spark VFX at exact drop coords, clink-hiss + structural-groan SFX, dust-fall from rafters, all pooled.
- **Exit:** 60 FPS locked on iPhone 12 + Pixel 6 baseline during peak impact.

### Sprint 4 — The Proof (2026-05-11 → 2026-05-15)
- **Co-Leads:** Kelvin Abernathy (`monetization-strategist`) + Jasmine Whitfield
- **Support:** Terrell Vaughn (telemetry pipe)
- **Tickets:** RPM-005 (scarcity squeeze + Ghost Refill telemetry) · RPM-006 (fail state)
- **Deliverable:** Scrap-exhaustion curve tuned so players deplete T−10s from wave end. Ghost Refill tap-zone live with PlayFab telemetry. "RESOURCES EXHAUSTED" fail state with 3s dramatic pause.
- **Exit:** 50+ playtester sessions captured; ghost-refill-tap rate measurable on a dashboard.

### PROTOTYPE GATE — 2026-05-18 (Owner Playtest)
- **Attendees:** Owner, Marissa Holloway.
- **Format:** Owner plays the build on phone + PC. Telemetry dashboard reviewed.
- **Decision output:**
  - **>35% ghost-refill-tap rate** → **Go.** Proceed to Sprint 5 Go-Path.
  - **15–35%** → **Tune.** One-week feel re-pass (Sprint 5 Pivot-Path).
  - **<15%** → **Hard pivot.** Pillar 1 ("Tactile Terror") is not landing; reconvene on core fantasy.

### Sprint 5 — Polish or Pivot (2026-05-25 → 2026-05-29)
- **Go Path (tickets RPM-100+):**
  - Onboarding tutorial (Simone).
  - PlayFab Economy V2 wiring + first Refill IAP product (Kelvin + Terrell).
  - Receipt validation for a single platform (Terrell).
- **Pivot Path:**
  - Feel re-pass led by Malik + Simone.
  - Redefine the scarcity curve with Kelvin.
  - Re-gate end of Sprint 5.

---

## Critical Dependencies (Owner Action Items)

| Dep | Owner of ask | Needed by | Risk if missed |
|---|---|---|---|
| `UNITY_LICENSE` secret populated in GitHub | Owner | Start of Sprint 1 | CI blocked on Unity tests; dev builds manual only |
| PlayFab title + secret key | Owner | Start of Sprint 4 | Ghost Refill telemetry cannot stream; gate slips |
| Playtester pool (10–50 humans) | Owner + Jalen Montgomery | End of Sprint 3 | No 50-session signal at the gate |
| Budget for a single iOS + Android TestFlight/Play Console slot | Owner | End of Sprint 2 | Must emulate on WebGL, weakens feel evaluation |

## Redlines (Any Breach → CoS Escalation)
1. **60 FPS mandate.** Any ticket breaching this pauses to negotiate VFX-vs-Performance (Malik ↔ Kendra) before merging to `develop`.
2. **12ms input latency.** Non-negotiable; if breached, Sprint 1 extends and Sprint 2 slips by the same amount.
3. **120-second purchase path.** Not in scope until Sprint 4, but instrument from day one so we never miss the signal.

## Milestone Burn-Up (Target vs. Actual — updated weekly)
| Date | Target milestone | Actual |
|---|---|---|
| 2026-04-17 | Sprint 0 scaffold landed | ✅ `deb2fbb` |
| 2026-04-24 | Drag-to-repair playable | pending |
| 2026-05-01 | Impact loop + ruin states | pending |
| 2026-05-08 | Full feel layer on target hardware | pending |
| 2026-05-15 | Ghost Refill telemetry live | pending |
| 2026-05-18 | Prototype Gate decision | pending |

---

## Deferred Hiring Decisions
| Role | Status | Trigger to revisit |
|---|---|---|
| Narrative Lead / World-Builder | **Deferred post-gate** (Owner decision, 2026-04-17) | After prototype gate 2026-05-18 if go-signal |
| BET Lead | **Hired** — Marvin Sinclair (he/him), 2026-04-17 | — |

---

*Any change to this roadmap is an Owner-level decision and ships via a CoS Inbox Card with A/B options.*
