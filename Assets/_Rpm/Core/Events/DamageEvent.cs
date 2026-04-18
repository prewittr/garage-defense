#nullable enable

using UnityEngine;

namespace Rpm.Core.Events
{
    /// <summary>
    /// Published on the <see cref="IEventBus"/> the instant a damage
    /// interaction is applied to the door. Immutable and allocation-free:
    /// callers pass the struct by value so no heap pressure is introduced
    /// into the impact tick.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The coordinate is specified in the door's local 2D surface space —
    /// the same space used by <c>DamagePointRegistry</c>. Unit conversion
    /// (screen → door-local) happens upstream in input code; downstream
    /// juice and VFX handlers can treat the value as authoritative.
    /// </para>
    /// <para>
    /// <see cref="TimestampSec"/> is <c>Time.timeAsDouble</c> captured at
    /// publish time. Subscribers correlate events by timestamp rather than
    /// frame index to stay resilient across variable frame times.
    /// </para>
    /// </remarks>
    public readonly struct DamageEvent
    {
        /// <summary>Damage magnitude expressed as a fraction of max HP (0..1).</summary>
        public readonly float Amount;

        /// <summary>Door-local 2D coordinate where the damage landed.</summary>
        public readonly Vector2 Coord;

        /// <summary>Unity <c>Time.timeAsDouble</c> captured at publish.</summary>
        public readonly double TimestampSec;

        /// <summary>Creates a new immutable damage event record.</summary>
        /// <param name="amount">Fraction of max HP, expected in [0..1].</param>
        /// <param name="coord">Door-local 2D coordinate.</param>
        /// <param name="timestampSec">Capture of <c>Time.timeAsDouble</c>.</param>
        public DamageEvent(float amount, Vector2 coord, double timestampSec)
        {
            Amount = amount;
            Coord = coord;
            TimestampSec = timestampSec;
        }
    }
}
