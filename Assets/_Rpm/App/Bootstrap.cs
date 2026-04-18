#nullable enable

using Rpm.Core;
using Rpm.Core.Door;
using Rpm.Core.Events;
using Rpm.Gameplay.Door;
using Rpm.Gameplay.Scrap;
using Rpm.Input.Drag;
using VContainer;
using VContainer.Unity;

namespace Rpm.App
{
    /// <summary>
    /// Composition root for the Rpm client. Attached to a single
    /// <c>Bootstrap</c> GameObject in <c>Scenes/_Boot.unity</c>. All
    /// feature-module registrations fan out from this single
    /// <see cref="Configure"/> call — there are no other
    /// <see cref="LifetimeScope"/> subclasses at runtime.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime rules follow ARCHITECTURE.md §3:
    /// <list type="bullet">
    ///   <item><description><c>Singleton</c> — cross-scene services (Save, Net, Economy facade, JuiceBus, EventBus, DragInput).</description></item>
    ///   <item><description><c>Scoped</c> — per-scene controllers (Door, Scrap, InputRouter).</description></item>
    ///   <item><description><c>Transient</c> — value types and short-lived handlers.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Sprint 1 adds the RPM-001 drag-to-repair loop: an <see cref="IEventBus"/>
    /// singleton, an <see cref="IDragInput"/> singleton, and four scoped
    /// gameplay services (<see cref="DoorController"/>, <see cref="ImpactScheduler"/>,
    /// <see cref="ScrapInventory"/>, <see cref="DragHandler"/>, <see cref="DamagePointRegistry"/>).
    /// The <see cref="DoorController"/> is exposed as both its concrete
    /// type and as <see cref="IDoor"/> so the scheduler and the drag
    /// handler resolve the same instance via the interface.
    /// </para>
    /// <para>
    /// MonoBehaviour registrations use <c>RegisterComponentInHierarchy</c>
    /// so VContainer binds to the prefab-authored instance in the scene
    /// (prefab wiring is Editor-deferred per RPM-001 Notes). The
    /// registration still lives here so the container graph is
    /// container-complete; Malik's feel-layer append can assume every
    /// collaborator resolves.
    /// </para>
    /// </remarks>
    public sealed class Bootstrap : LifetimeScope
    {
        /// <inheritdoc/>
        protected override void Configure(IContainerBuilder builder)
        {
            builder
                .Register<BootstrapService>(Lifetime.Singleton)
                .As<IBootstrapService>();

            // --- Core singletons (cross-scene services) ---
            builder
                .Register<EventBus>(Lifetime.Singleton)
                .As<IEventBus>();

            builder
                .Register<DragInput>(Lifetime.Singleton)
                .As<IDragInput>()
                .AsSelf();

            // --- Scoped gameplay (per-scene) ---
            // DoorController is a MonoBehaviour; binding via
            // RegisterComponentInHierarchy means VContainer finds the
            // prefab-placed component at scene load and injects it. The
            // Editor-deferred work wires the prefab into the scene.
            builder
                .RegisterComponentInHierarchy<DoorController>()
                .As<IDoor>()
                .AsSelf();

            builder
                .RegisterComponentInHierarchy<ImpactScheduler>();

            builder
                .RegisterComponentInHierarchy<DragHandler>();

            // Plain C# services — no scene dependency.
            builder
                .Register<ScrapInventory>(Lifetime.Scoped)
                .As<IScrapInventory>()
                .AsSelf();

            builder
                .Register<DamagePointRegistry>(Lifetime.Scoped);
        }
    }
}
