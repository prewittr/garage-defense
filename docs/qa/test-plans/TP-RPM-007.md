# [TP-RPM-007] Test Plan — Module asmdef Split + Composition Root

**Author:** Jasmine Whitfield (`qa-breakdown-analyst`)
**Co-author:** Marvin Sinclair
**Date:** 2026-04-17
**Linked Story:** [RPM-007](../../../tickets/RPM-007.md)
**Status:** Draft

## Scope
- **In scope:** assembly boundaries, compile-dependency graph, composition root wiring, Addressables async scene load, smoke test green on CI.
- **Out of scope:** gameplay behavior (RPM-001), user-visible functionality, perf beyond "doesn't regress baseline."

## Preconditions
- Fresh clone of `feature/tactile-terror`.
- Unity 6000.0.32f1 installed locally (Windows Unity Hub activation).

## Test Cases

| ID | Case | Priority | Steps | Expected | Notes |
|---|---|---|---|---|---|
| TC-001 | Fresh-clone open | **P0** | 1. Clone repo. 2. Open in Unity Hub. 3. Wait for import. | Project opens with zero compile errors. All asmdefs listed in Project view. | |
| TC-002 | asmdef inventory | **P0** | 1. Inspect `Assets/_Rpm/` tree. | Exactly these asmdefs exist: `Rpm.Core`, `Rpm.Input`, `Rpm.Gameplay.Door`, `Rpm.Gameplay.Scrap`, `Rpm.Juice`, `Rpm.App`. Legacy flat `Rpm.asmdef` is **removed**. | |
| TC-003 | Dependency graph — no cycles | **P0** | 1. Run `dotnet sln / assembly-graph` via CI step OR inspect each `.asmdef` references manually. | Zero circular references. Dependency matrix matches ARCHITECTURE.md §2. | Kendra may script this as a CI check. |
| TC-004 | Composition root runs | **P0** | 1. Open scene `Scenes/_Boot.unity`. 2. Enter Play Mode. | VContainer builds container in <500ms. At least one registered `Singleton` resolves (verify via Debug.Log or test). | |
| TC-005 | Addressables async scene load | **P0** | 1. Boot scene runs. 2. Wait for crossfade completion. | `Scenes/TactileTerror_S1.unity` loaded via Addressables. No `SceneManager.LoadScene` sync call in code path (grep `Rpm.App`). | |
| TC-006 | Smoke test — container resolves | **P0** | 1. Run `Rpm.App.Tests.BootstrapSmoke` in EditMode runner. | Test passes. Container resolves at least one service. | CI runs this. |
| TC-007 | Windows Standalone build | P1 | 1. File → Build Settings → Build Windows Standalone. | Build succeeds. Produces `.exe` ≤ 150 MB. | |
| TC-008 | Clean re-import | P1 | 1. Delete `Library/`. 2. Re-open Unity. | Full re-import completes without errors. | Detects implicit hidden deps. |

## Device Matrix Applied — Sprint 1

| Tier | Device | Apply? |
|---|---|---|
| Editor | Unity 6000.0.32f1 | **Yes** |
| Windows Standalone | CI dev build | **Yes** |
| Other platforms | — | **No** (infrastructure change; no user-visible surface) |

## Performance Validation
- **Cold-start time:** `_Boot` scene → `TactileTerror_S1` ready ≤ 3s on desktop.
- **Memory after bootstrap:** < 250 MB RSS.

## Exit Criteria
- [ ] All P0 cases pass.
- [ ] Zero S1 / S2 bugs.
- [ ] Smoke test green in CI.
- [ ] `Tested-By: Jasmine Whitfield` trailer on merge commit.

## Results
_To be filled during Sprint 1._
