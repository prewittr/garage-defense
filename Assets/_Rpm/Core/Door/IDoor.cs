#nullable enable

using UnityEngine;

namespace Rpm.Core.Door
{
    /// <summary>
    /// Cross-module contract for the Sprint 1 garage door. Kept in
    /// <c>Rpm.Core</c> so <c>Rpm.Gameplay.Scrap</c> can invoke repair
    /// without a circular dependency on <c>Rpm.Gameplay.Door</c>.
    /// </summary>
    /// <remarks>
    /// The architecture table in ARCHITECTURE.md §2 forbids a
    /// Scrap → Gameplay.Door arrow. Putting <see cref="IDoor"/> in
    /// <c>Rpm.Core</c> keeps Scrap's dependency list as
    /// <c>{Core, Input}</c> while still letting the drag handler call
    /// <see cref="ApplyRepair"/>.
    /// </remarks>
    public interface IDoor
    {
        /// <summary>Current hit-point snapshot.</summary>
        DoorHP HP { get; }

        /// <summary>
        /// Apply damage to the door. Amount is a fraction of max HP.
        /// Resulting HP is clamped at <c>[0, Max]</c>. Raises a
        /// <c>DamageEvent</c> on the shared event bus.
        /// </summary>
        /// <param name="fractionOfMax">Damage as fraction of max HP, in [0..1].</param>
        /// <param name="coord">Door-local 2D coordinate where damage landed.</param>
        void ApplyDamage(float fractionOfMax, Vector2 coord);

        /// <summary>
        /// Apply a repair to the door. Amount is a fraction of max HP.
        /// Returns <c>false</c> if the door is already at full HP (no
        /// scrap should be consumed in that case). Raises a
        /// <c>RepairEvent</c> on the shared event bus when the repair is
        /// accepted.
        /// </summary>
        /// <param name="fractionOfMax">Repair as fraction of max HP, in [0..1].</param>
        /// <param name="coord">Door-local 2D coordinate where repair landed.</param>
        /// <returns><c>true</c> when HP changed; <c>false</c> when rejected.</returns>
        bool ApplyRepair(float fractionOfMax, Vector2 coord);
    }
}
