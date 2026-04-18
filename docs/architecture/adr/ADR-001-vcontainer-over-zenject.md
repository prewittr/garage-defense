## ADR-001: Use VContainer as the DI container (over Zenject / Reflex)

**Status:** Proposed
**Date:** 2026-04-17
**Deciders:** Kendra Brooks (Lead Dev), Terrell Vaughn (DevOps)
**Consulted:** Malik Ransom (VFX perf implications), Simone Carver (module ownership)
**Informed:** Marissa Holloway (CoS), Marvin Sinclair (BET Lead), Owner

## Context
Sprint 1 lands the first real composition root for Rpm: Redline / Deadline. The flat placeholder `Rpm` assembly is being split into feature-oriented modules (`Rpm.Core`, `Rpm.Input`, `Rpm.Gameplay.Door`, `Rpm.Gameplay.Scrap`, `Rpm.Juice`, `Rpm.App`) per ARCHITECTURE.md §2. Those modules need to resolve services (Save, Net, Economy facade, JuiceBus, per-scene controllers) without singletons, `FindObjectOfType`, or static service locators.

The perf mandate is **60 FPS locked** on iPhone 12 / Pixel 6 / Steam Deck, with a hard constraint of **zero allocations in the night-defense tick**. Any DI container we pick has to stay well clear of the hot path — ideally no reflection emit at resolution time and no per-frame `Resolve` calls on managed heaps.

Three mature Unity-first DI options are in play: **VContainer** (Hadashi), **Zenject / Extenject** (Modest Tree), and **Reflex** (gustavopsantos). A choice is required before any further gameplay code lands on `develop`, because retrofitting a DI swap across every `LifetimeScope` and factory is an O(n) migration we do not want to do twice.

## Decision Drivers
- **Binary size.** Mobile install size is a measurable conversion factor; the container should add <1 MB to the stripped IL2CPP build.
- **Allocation profile.** Container must support compile-time codegen (source generator) or IL2CPP-friendly resolution so that `Resolve` calls outside boot are allocation-free.
- **First-class UniTask support.** `IAsyncStartable` / `IAsyncInitializable` integration matters — async boot is the backbone of the Addressables preload → scene-load sequence described in ARCHITECTURE.md §3.
- **Test ergonomics.** We need a container we can `new` in an EditMode test (no Unity scene required) — see RPM-007 smoke test.
- **Maintenance velocity.** Active upstream, Unity 6 compatibility, responsive issue tracker.

## Options Considered

### Option A — VContainer
- **Pros:**
  - Smallest managed footprint of the three (~170 KB stripped); designed to stay off the GC in hot paths.
  - First-class UniTask integration (`IAsyncStartable`, `RegisterEntryPoint`); we already depend on UniTask.
  - Source-generator path for AOT-safe resolution on IL2CPP.
  - Pure-C# `ContainerBuilder` means tests can build a container without `LifetimeScope` / GameObject overhead.
  - Active maintenance (hadashiA), Unity 6 compatible as of 1.16.x.
- **Cons:**
  - Smaller community than Zenject — fewer StackOverflow hits, no published Unity Learn course.
  - API surface is smaller; a few Zenject-isms (e.g. `SignalBus`) are not 1:1.
- **Cost:** Low. Already in the manifest at `1.16.4`. Zero migration cost.

### Option B — Zenject / Extenject
- **Pros:**
  - Largest community; widely documented; familiar to most Unity hires.
  - Rich feature set — signals, factories, sub-containers, memory pools.
- **Cons:**
  - Heavy reflection at resolve time; known allocation hotspots without careful pre-binding.
  - UniTask integration is community-patched, not first-class.
  - Original maintainer stepped back in 2022; Extenject fork is alive but slower to ship Unity LTS compatibility fixes.
  - Larger binary footprint; noticeably more code to strip.
- **Cost:** Medium. Would require replacing the current `VContainer` package dependency and re-training the team.

### Option C — Reflex
- **Pros:**
  - Smallest of the three; extremely lean API.
  - Good allocation story; actively maintained.
- **Cons:**
  - Youngest of the three; fewer battle-tested integration patterns for Addressables/UniTask.
  - Smaller talent pool; onboarding cost for future contributors.
  - No source-generator story as mature as VContainer's.
- **Cost:** Medium. New dependency, migration of any prototype code.

## Decision
**We chose Option A — VContainer.** It is already in `Packages/manifest.json`, meets the allocation budget via its source generator and pure-C# builder, integrates cleanly with UniTask (our chosen async primitive), and lets us test the composition root without a Unity scene. Zenject's community edge does not outweigh its allocation profile and uncertain maintenance. Reflex is attractive but immature for a 60 FPS, IL2CPP-shipping title targeting six platforms.

## Consequences

### Positive
- Composition root is testable in EditMode without a scene — see `Rpm.App.Tests.BootstrapSmoke`.
- Allocation-free resolution paths are available once we enable the VContainer source generator in the CI build (follow-up).
- Async boot sequence (`IAsyncStartable`) is idiomatic, reducing boilerplate in `Bootstrap.cs`.

### Negative
- Contributors coming from a Zenject background have a short learning curve (scope vs. container, `RegisterEntryPoint` vs. `ITickable`). Mitigation: a one-page primer will accompany ADR-001's move to **Accepted**.
- Some Zenject-ergonomic features (SignalBus, MemoryPool attribute sugar) require hand-rolled equivalents or a thin `Rpm.Core` abstraction.

### Neutral
- We take a hard dependency on `jp.hadashikick.vcontainer` in OpenUPM. Terrell already added the scoped registry.
- `LifetimeScope` becomes the only place singletons are tolerated — this aligns with the CLAUDE.md rule "No singletons except the composition root."

## Compliance / Follow-up
- Enable the VContainer source generator in CI once the first gameplay module ships — owner: Kendra Brooks, deadline: end of Sprint 2.
- Publish a one-page VContainer primer at `docs/architecture/vcontainer-primer.md` before ADR-001 moves to Accepted — owner: Kendra Brooks, deadline: Sprint 1 Review.
- ARCHITECTURE.md §6 "Accepted ADRs" table updated when this ADR is accepted — owner: Kendra Brooks.

## Notes
Option C (Reflex) was floated by Malik on the premise of minimal allocations; the deciding factor against it was UniTask/Addressables integration maturity, not container performance in isolation. No dissents recorded; accepting this ADR is expected at the Sprint 1 Tech Design Review once the smoke test lands green.

---
**Once this ADR is ACCEPTED, it is immutable.** Future changes require a new ADR that supersedes this one.
