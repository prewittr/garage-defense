#nullable enable

namespace Rpm.Core
{
    /// <summary>
    /// Minimal marker contract proving the composition root (VContainer)
    /// successfully resolves a service across assembly boundaries.
    /// </summary>
    /// <remarks>
    /// This interface lives in <c>Rpm.Core</c> so every downstream module
    /// (App, Gameplay.*, Juice, Input) can depend on it without creating a
    /// cycle back to <c>Rpm.App</c>. The concrete implementation is owned
    /// by the composition root and registered in <c>Rpm.App.Bootstrap</c>.
    /// </remarks>
    public interface IBootstrapService
    {
        /// <summary>
        /// Returns <c>true</c> once the composition root has completed its
        /// wire-up pass and the service graph is safe to resolve from.
        /// </summary>
        bool IsReady { get; }
    }
}
