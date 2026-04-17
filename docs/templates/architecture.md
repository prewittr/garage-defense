# System Architecture v<N>

**Maintainer:** Kendra Brooks (`performance-hardener`)
**Last reviewed:** YYYY-MM-DD
**Status:** Living document — this file IS the current state; edits require ADR.

## Context & Constraints
- **Target platforms:** iOS, Android, Steam (Windows/macOS/Linux/Deck), PS5, Xbox Series X|S.
- **Engine:** Unity 6000.0.32f1 LTS · URP.
- **Perf mandate:** 60 FPS on baseline (iPhone 12 / Pixel 6 / Steam Deck).
- **Team scale:** 9 specialists + Owner.
- **Third-party SDKs:** PlayFab, game-ci runners.

## Module Map
Top-level assemblies (`.asmdef`) and their responsibilities. Circular references are a PR blocker.

| Assembly | Depends on | Responsibility |
|---|---|---|
| `Rpm.Core` | — | Types, interfaces, constants |
| `Rpm.Input` | Core, Unity.InputSystem | Device-agnostic input contracts |
| `Rpm.Gameplay.Door` | Core, Input | Door HP, damage events |
| `Rpm.Gameplay.Scrap` | Core, Input | Scrap inventory, drag-drop |
| `Rpm.Juice` | Core | Screen shake, haptics, VFX hooks |
| `Rpm.Economy` | Core, PlayFab | Currency, catalog, purchases |
| `Rpm.Save` | Core | Persistence abstraction |
| `Rpm.Net` | Core, PlayFab | Network boundary, retries |

## Runtime Composition
- **DI container:** VContainer. Composition root: `Rpm.App.Bootstrap`.
- **Lifecycle:** <scene flow, addressables, preload>
- **Scene loading:** Addressables async + crossfade.

## Client ↔ Server Contract
- **Authoritative boundaries:** all economy mutations, all gacha pulls, all timer completions are server-side (PlayFab CloudScript).
- **Idempotency rules:** client requests carry a request id; server dedupes.
- **Replay protection:** receipts validated once, marked consumed.

## Cross-cutting Concerns
- **Logging:** <what sink, what level, how PII is scrubbed>
- **Telemetry:** PlayFab events + custom dashboards.
- **Error handling:** <exception boundaries, degrade-gracefully policy>
- **Feature flags:** <where stored, how toggled>

## Open Questions / Future ADRs
- <topic> — owner, target ADR number

## Accepted ADRs
| ADR | Title | Date |
|---|---|---|
| ADR-001 | VContainer over Zenject | |
| ADR-002 | PlayFab Economy V2 as sole economy source | |

## Amendments
| Version | Date | Change |
|---|---|---|
| 1.0 | | |
