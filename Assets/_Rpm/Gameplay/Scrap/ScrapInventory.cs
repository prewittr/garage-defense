#nullable enable

namespace Rpm.Gameplay.Scrap
{
    /// <summary>
    /// Default <see cref="IScrapInventory"/>. Sprint 1 starts every run
    /// with ten units (per DESIGN-001 moment-to-moment layout).
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. Each gameplay scope gets a fresh inventory
    /// — a reloaded scene starts at <see cref="StartingCount"/> again.
    /// </para>
    /// <para>
    /// No allocations on <see cref="TryUse"/>; it is a single field
    /// decrement. Ownership of the field is single-writer by design — only
    /// this class mutates <see cref="Count"/>. External systems observe.
    /// </para>
    /// </remarks>
    public sealed class ScrapInventory : IScrapInventory
    {
        /// <summary>Sprint 1 starting scrap count per DESIGN-001.</summary>
        public const int StartingCount = 10;

        private int _count;

        /// <summary>Creates a fresh inventory at <see cref="StartingCount"/>.</summary>
        public ScrapInventory()
        {
            _count = StartingCount;
        }

        /// <inheritdoc/>
        public int Count => _count;

        /// <inheritdoc/>
        public bool TryUse()
        {
            if (_count <= 0) return false;
            _count--;
            return true;
        }
    }
}
