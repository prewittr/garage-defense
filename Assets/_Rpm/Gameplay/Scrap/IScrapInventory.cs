#nullable enable

namespace Rpm.Gameplay.Scrap
{
    /// <summary>
    /// Client-side scrap inventory contract. Authoritative server state is
    /// not yet a concern in Sprint 1 — this interface lives entirely in
    /// the client and is authoritative for the Tactile Terror slice.
    /// </summary>
    public interface IScrapInventory
    {
        /// <summary>Remaining scrap units available for repair.</summary>
        int Count { get; }

        /// <summary>
        /// Consumes one scrap unit if available. Returns <c>false</c> when
        /// the inventory is empty, in which case no state changes.
        /// </summary>
        /// <returns><c>true</c> if one unit was consumed.</returns>
        bool TryUse();
    }
}
