#nullable enable

using NUnit.Framework;
using Rpm.Core;
using VContainer;

namespace Rpm.App.Tests
{
    /// <summary>
    /// EditMode smoke test covering the composition-root contract:
    /// a transient container mirroring <see cref="Bootstrap.Configure"/>
    /// must resolve <see cref="IBootstrapService"/> and report ready.
    /// </summary>
    /// <remarks>
    /// This test deliberately builds a fresh <see cref="ContainerBuilder"/>
    /// rather than instantiating <see cref="Bootstrap"/> itself: the
    /// <c>LifetimeScope</c> lifecycle is owned by Unity's scene graph and
    /// is exercised separately by Editor smoke runs on <c>_Boot.unity</c>
    /// (deferred in RPM-007 Notes).
    /// </remarks>
    public sealed class BootstrapSmoke
    {
        [Test]
        public void Container_Resolves_BootstrapService_Singleton()
        {
            var builder = new ContainerBuilder();
            builder
                .Register<BootstrapService>(Lifetime.Singleton)
                .As<IBootstrapService>();

            using var container = builder.Build();
            var service = container.Resolve<IBootstrapService>();

            Assert.IsNotNull(service, "VContainer failed to resolve IBootstrapService.");
            Assert.IsTrue(service.IsReady, "BootstrapService.IsReady should be true immediately after resolution.");
        }
    }
}
