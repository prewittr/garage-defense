#nullable enable

using System;

namespace Rpm.Core.Events
{
    /// <summary>
    /// Minimal pub/sub abstraction for cross-module event delivery. Owns no
    /// state relevant to gameplay; its only job is to route immutable event
    /// structs from publishers to subscribers without per-publish
    /// allocations.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Kept deliberately tiny so the Juice layer (Malik's follow-up) can
    /// subscribe to <see cref="DamageEvent"/> and <see cref="RepairEvent"/>
    /// without pulling a dependency on <c>Rpm.Gameplay.Door</c>. Every
    /// downstream module observes the door through this bus — never
    /// directly through the concrete controller.
    /// </para>
    /// <para>
    /// Implementations must guarantee zero allocations on
    /// <see cref="Publish{T}"/> after steady-state subscription. See
    /// <c>EventBus</c> for the reference implementation.
    /// </para>
    /// </remarks>
    public interface IEventBus
    {
        /// <summary>
        /// Register a handler for events of type <typeparamref name="T"/>.
        /// Safe to call during <c>Awake</c>/<c>Start</c>; not safe from
        /// inside a handler of the same type (reentrancy is blocked by
        /// the implementation).
        /// </summary>
        /// <typeparam name="T">Event struct type; must be a value type.</typeparam>
        /// <param name="handler">Delegate invoked for each publish.</param>
        void Subscribe<T>(Action<T> handler) where T : struct;

        /// <summary>Remove a previously registered handler.</summary>
        /// <typeparam name="T">Event struct type; must be a value type.</typeparam>
        /// <param name="handler">The exact delegate instance passed to <see cref="Subscribe{T}"/>.</param>
        void Unsubscribe<T>(Action<T> handler) where T : struct;

        /// <summary>
        /// Synchronously dispatch <paramref name="evt"/> to every current
        /// subscriber. Zero allocations in steady state.
        /// </summary>
        /// <typeparam name="T">Event struct type; must be a value type.</typeparam>
        /// <param name="evt">The immutable event payload.</param>
        void Publish<T>(in T evt) where T : struct;
    }
}
