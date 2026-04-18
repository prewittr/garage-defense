#nullable enable

namespace Rpm.Core.Door
{
    /// <summary>
    /// Immutable snapshot of the door's hit-point state. Exposed so VFX,
    /// juice, and HUD layers can read door health without pulling a
    /// dependency on the concrete <c>DoorController</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// All three fields are stored rather than computed so a consumer can
    /// read <see cref="Ratio"/> without a per-call divide. Producers are
    /// expected to compute the ratio once at construction and store it.
    /// </para>
    /// <para>
    /// <see cref="Ratio"/> is always in <c>[0, 1]</c>; the struct does not
    /// enforce this — the <c>DoorController</c> does. Treat the value as
    /// display-ready.
    /// </para>
    /// </remarks>
    public readonly struct DoorHP
    {
        /// <summary>Current hit points.</summary>
        public readonly float Current;

        /// <summary>Maximum hit points for this door.</summary>
        public readonly float Max;

        /// <summary><see cref="Current"/> divided by <see cref="Max"/>, clamped to [0,1].</summary>
        public readonly float Ratio;

        /// <summary>Creates a new snapshot. <paramref name="ratio"/> is not re-derived.</summary>
        /// <param name="current">Current hit points.</param>
        /// <param name="max">Maximum hit points.</param>
        /// <param name="ratio">Pre-computed ratio; typically <paramref name="current"/> / <paramref name="max"/>.</param>
        public DoorHP(float current, float max, float ratio)
        {
            Current = current;
            Max = max;
            Ratio = ratio;
        }
    }
}
