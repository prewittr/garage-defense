#nullable enable

using Rpm.Core;
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
    /// Lifetime rules follow ARCHITECTURE.md §3:
    /// <list type="bullet">
    ///   <item><description><c>Singleton</c> — cross-scene services (Save, Net, Economy facade, JuiceBus).</description></item>
    ///   <item><description><c>Scoped</c> — per-scene controllers (Door, Scrap, InputRouter).</description></item>
    ///   <item><description><c>Transient</c> — value types and short-lived handlers.</description></item>
    /// </list>
    /// During Sprint 1 this class only registers <see cref="BootstrapService"/>
    /// as a singleton so the EditMode smoke test can prove container wiring.
    /// </remarks>
    public sealed class Bootstrap : LifetimeScope
    {
        /// <inheritdoc/>
        protected override void Configure(IContainerBuilder builder)
        {
            builder
                .Register<BootstrapService>(Lifetime.Singleton)
                .As<IBootstrapService>();
        }
    }
}
