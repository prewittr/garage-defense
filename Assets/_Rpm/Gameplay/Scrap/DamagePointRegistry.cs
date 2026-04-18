#nullable enable

using System;
using UnityEngine;

namespace Rpm.Gameplay.Scrap
{
    /// <summary>
    /// Tracks active damage-point coordinates on the door and answers
    /// "nearest to" queries in <see cref="O(N)"/> time with zero
    /// allocations on the query path.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. One registry per gameplay scope. The
    /// registry is a plain C# class (no MonoBehaviour) so tests can spin
    /// it up without a scene.
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> the nearest-to lookup walks an array-backed
    /// <see cref="Span{T}"/> and allocates nothing. The backing buffer
    /// grows by a power-of-two doubling policy — register/unregister are
    /// O(N) worst case (array shift on unregister) but do not run on the
    /// drag hot path; they fire on impact/repair events only.
    /// </para>
    /// </remarks>
    public sealed class DamagePointRegistry
    {
        private Vector2[] _points;
        private int _count;

        /// <summary>Creates a registry with a small initial capacity.</summary>
        public DamagePointRegistry()
        {
            _points = new Vector2[8];
            _count = 0;
        }

        /// <summary>Active damage-point count.</summary>
        public int Count => _count;

        /// <summary>Adds a damage point at <paramref name="point"/>.</summary>
        /// <param name="point">Door-local 2D coordinate.</param>
        public void RegisterPoint(Vector2 point)
        {
            if (_count == _points.Length)
            {
                var grown = new Vector2[_points.Length * 2];
                Array.Copy(_points, grown, _count);
                _points = grown;
            }
            _points[_count++] = point;
        }

        /// <summary>
        /// Removes the first damage point whose coordinate is within
        /// <c>0.0001</c> of <paramref name="point"/>. Returns <c>true</c>
        /// when a point was removed.
        /// </summary>
        /// <param name="point">The coordinate to remove.</param>
        public bool UnregisterPoint(Vector2 point)
        {
            const float eps = 1e-4f;
            for (var i = 0; i < _count; i++)
            {
                if ((_points[i] - point).sqrMagnitude <= eps * eps)
                {
                    // swap-remove (order irrelevant for nearest-to semantics)
                    _count--;
                    if (i < _count) _points[i] = _points[_count];
                    _points[_count] = default;
                    return true;
                }
            }
            return false;
        }

        /// <summary>
        /// Removes every currently-registered point. Used by tests and by
        /// scene reload paths. Zero allocation.
        /// </summary>
        public void Clear()
        {
            for (var i = 0; i < _count; i++) _points[i] = default;
            _count = 0;
        }

        /// <summary>
        /// Finds the registered point nearest to <paramref name="probe"/>.
        /// Returns <c>false</c> (and <c>default</c>) if the registry is
        /// empty.
        /// </summary>
        /// <param name="probe">Coordinate being tested.</param>
        /// <param name="nearest">Nearest registered coordinate, when found.</param>
        /// <returns><c>true</c> when a nearest point exists.</returns>
        public bool NearestTo(Vector2 probe, out Vector2 nearest)
        {
            if (_count == 0)
            {
                nearest = default;
                return false;
            }

            // Span<T> over the live prefix of the backing array — no copy,
            // no alloc, safe because _points is an array not a List<T>.
            ReadOnlySpan<Vector2> span = _points.AsSpan(0, _count);
            var bestIdx = 0;
            var bestSqr = (span[0] - probe).sqrMagnitude;
            for (var i = 1; i < span.Length; i++)
            {
                var sqr = (span[i] - probe).sqrMagnitude;
                if (sqr < bestSqr)
                {
                    bestSqr = sqr;
                    bestIdx = i;
                }
            }
            nearest = span[bestIdx];
            return true;
        }
    }
}
