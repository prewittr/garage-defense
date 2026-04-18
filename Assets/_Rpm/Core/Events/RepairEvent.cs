#nullable enable

using UnityEngine;

namespace Rpm.Core.Events
{
    /// <summary>
    /// Published on the <see cref="IEventBus"/> the instant a repair
    /// interaction is applied to the door. Immutable and allocation-free.
    /// </summary>
    /// <remarks>
    /// Symmetrical to <see cref="DamageEvent"/>. Emitted only for
    /// successful weld operations; rejected drags (scrap outside a damage
    /// point, or door already full) do not raise this event.
    /// </remarks>
    public readonly struct RepairEvent
    {
        /// <summary>Repair magnitude expressed as a fraction of max HP (0..1).</summary>
        public readonly float Amount;

        /// <summary>Door-local 2D coordinate where the repair landed.</summary>
        public readonly Vector2 Coord;

        /// <summary>Unity <c>Time.timeAsDouble</c> captured at publish.</summary>
        public readonly double TimestampSec;

        /// <summary>Creates a new immutable repair event record.</summary>
        /// <param name="amount">Fraction of max HP, expected in [0..1].</param>
        /// <param name="coord">Door-local 2D coordinate.</param>
        /// <param name="timestampSec">Capture of <c>Time.timeAsDouble</c>.</param>
        public RepairEvent(float amount, Vector2 coord, double timestampSec)
        {
            Amount = amount;
            Coord = coord;
            TimestampSec = timestampSec;
        }
    }
}
