#nullable enable

using System;
using System.Collections.Generic;

namespace Rpm.Core.Events
{
    /// <summary>
    /// Default <see cref="IEventBus"/> implementation. A per-type handler
    /// list cached in a generic static field gives O(1) dispatch with zero
    /// allocations on <see cref="Publish{T}"/> once subscribers are
    /// registered.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Registered as <c>Singleton</c> by the composition root so every
    /// gameplay scope resolves the same instance. The bus itself is not
    /// thread-safe; it is designed for Unity's main thread.
    /// </para>
    /// <para>
    /// <b>Reentrancy:</b> a handler that publishes the same event type it
    /// is handling is rejected with an <see cref="InvalidOperationException"/>
    /// rather than silently re-entering the dispatch loop. Cross-type
    /// publishes from within a handler are permitted.
    /// </para>
    /// </remarks>
    public sealed class EventBus : IEventBus
    {
        /// <summary>
        /// Per-type subscriber container. A class-typed static generic
        /// field gives us one <see cref="List{T}"/> per <typeparamref name="T"/>
        /// shared across every <see cref="EventBus"/> instance. Each bus
        /// uses its own <see cref="_id"/> to namespace its entries so
        /// multiple buses (e.g. in tests) do not cross-talk.
        /// </summary>
        private static class Handlers<T> where T : struct
        {
            // Keyed by bus id; value is the mutable handler list. We
            // deliberately accept one small Dictionary allocation per
            // (bus, event-type) pair — the publish path itself walks only
            // the value list and allocates nothing.
            public static readonly Dictionary<int, List<Action<T>>> ByBus = new(1);

            // Reentrancy guard, keyed the same way.
            public static readonly Dictionary<int, bool> Dispatching = new(1);
        }

        private static int _nextId;
        private readonly int _id;

        /// <summary>Creates a new isolated event bus.</summary>
        public EventBus()
        {
            _id = System.Threading.Interlocked.Increment(ref _nextId);
        }

        /// <inheritdoc/>
        public void Subscribe<T>(Action<T> handler) where T : struct
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            if (!Handlers<T>.ByBus.TryGetValue(_id, out var list))
            {
                list = new List<Action<T>>(4);
                Handlers<T>.ByBus[_id] = list;
            }
            list.Add(handler);
        }

        /// <inheritdoc/>
        public void Unsubscribe<T>(Action<T> handler) where T : struct
        {
            if (handler is null) throw new ArgumentNullException(nameof(handler));
            if (Handlers<T>.ByBus.TryGetValue(_id, out var list))
            {
                list.Remove(handler);
            }
        }

        /// <inheritdoc/>
        public void Publish<T>(in T evt) where T : struct
        {
            if (!Handlers<T>.ByBus.TryGetValue(_id, out var list) || list.Count == 0)
            {
                return;
            }
            if (Handlers<T>.Dispatching.TryGetValue(_id, out var busy) && busy)
            {
                throw new InvalidOperationException(
                    $"EventBus reentrancy detected for {typeof(T).Name}; a handler published the same event type it was handling.");
            }
            Handlers<T>.Dispatching[_id] = true;
            try
            {
                // Index iteration to avoid enumerator allocation.
                var count = list.Count;
                for (var i = 0; i < count; i++)
                {
                    list[i].Invoke(evt);
                }
            }
            finally
            {
                Handlers<T>.Dispatching[_id] = false;
            }
        }
    }
}
