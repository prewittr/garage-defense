# [TP-RPM-NNN] Test Plan for <Story Title>

**Author:** Jasmine Whitfield (`qa-breakdown-analyst`)
**Date:** YYYY-MM-DD
**Linked Story:** RPM-NNN
**Status:** Draft | Reviewed | Executing | Complete

## Scope
- **In scope:** <what this plan covers>
- **Out of scope:** <what it explicitly does NOT cover>

## Preconditions
- Account state: <fresh install / D1 / D7 / whale>
- Device state: <battery level / network / storage>
- Feature flags: <which on/off>
- Server state: <staging PlayFab title / prod>

## Test Cases

| ID | Case | Priority | Steps | Expected | Notes |
|---|---|---|---|---|---|
| TC-001 | Happy path | P0 | 1. …<br>2. …<br>3. … | <observable result> | |
| TC-002 | Network drop mid-action | P0 | | Graceful degrade, no double-grant | |
| TC-003 | <negative case> | P1 | | | |
| TC-004 | <edge case> | P2 | | | |

## Device Matrix Applied
| Tier | Device | Apply? | Notes |
|---|---|---|---|
| iOS min | iPhone SE (gen3) | Y/N | |
| iOS mid | iPhone 12 | Y/N | |
| iOS high | iPhone 15 Pro | Y/N | |
| iPad | iPad (10th gen) | Y/N | |
| Android min | Samsung A14 | Y/N | |
| Android mid | Pixel 6 | Y/N | |
| Android high | Samsung S24 | Y/N | |
| PC min | GTX 1060 / 8GB RAM | Y/N | |
| Steam Deck | | Y/N | |
| PS5 | | Y/N | |
| Xbox Series S | | Y/N | |
| Xbox Series X | | Y/N | |

## Performance Validation
- **Profiler capture:** <when, what scene, duration>
- **Frame time p50 target:** <x ms>
- **Frame time p95 target:** <x ms>
- **Memory ceiling:** <x MB>
- **Input latency p95:** <x ms>

## Exit Criteria
- [ ] All P0 cases pass
- [ ] Zero S1 bugs
- [ ] Zero S2 bugs
- [ ] S3 bugs triaged to backlog or accepted

## Results (appended after execution)
<Per-case pass/fail, bug links, perf captures.>

## Amendments
| Version | Date | Change |
|---|---|---|
| 1.0 | | |
