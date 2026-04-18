#nullable enable

using Rpm.Core;

namespace Rpm.App
{
    /// <summary>
    /// Default <see cref="IBootstrapService"/> implementation. Constructed
    /// by VContainer once <see cref="Bootstrap.Configure"/> has registered
    /// the binding; the service reports <see cref="IsReady"/> as soon as
    /// the container has handed it back to a caller.
    /// </summary>
    /// <remarks>
    /// Deliberately trivial: its purpose is to prove the container wires
    /// across asmdef boundaries. Do not grow this class — add a feature
    /// service in the appropriate module and register it in
    /// <see cref="Bootstrap.Configure"/>.
    /// </remarks>
    public sealed class BootstrapService : IBootstrapService
    {
        /// <inheritdoc/>
        public bool IsReady => true;
    }
}
