#nullable enable

using System;
using Rpm.Core.Door;
using Rpm.Input.Drag;
using UnityEngine;
using VContainer;

namespace Rpm.Gameplay.Scrap
{
    /// <summary>
    /// Bridges <see cref="IDragInput"/> events into the repair pipeline:
    /// on drop, snap to the nearest registered damage point (if within
    /// <see cref="_snapRadius"/>), consume one scrap unit, and apply a
    /// 15% repair to the door.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. Authored as a <see cref="MonoBehaviour"/>
    /// so it can be wired onto the drag-root prefab in the gameplay scene
    /// (Editor-deferred per the RPM-001 Notes).
    /// </para>
    /// <para>
    /// <b>AC enforcement:</b>
    /// <list type="bullet">
    ///   <item><description>Drop outside any damage point → no scrap consumed, no HP change.</description></item>
    ///   <item><description>Drop on door at full HP → repair call is rejected by <see cref="IDoor.ApplyRepair"/>; scrap is also not consumed.</description></item>
    ///   <item><description>Successful drop → scrap decrement + 15% repair at snapped coord.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> the drag-update tick is pure input
    /// forwarding. This handler does zero work on <c>OnDrag</c>; the only
    /// non-trivial work is the <see cref="DamagePointRegistry.NearestTo"/>
    /// call at drop time — which is an array walk with no alloc.
    /// </para>
    /// </remarks>
    public sealed class DragHandler : MonoBehaviour
    {
        /// <summary>Sprint 1 repair amount per DESIGN-001 and RPM-001 AC.</summary>
        public const float RepairFraction = 0.15f;

        [Tooltip("Maximum door-local distance between drop coord and a damage point for the snap to count.")]
        [SerializeField] private float _snapRadius = 0.12f;

        private IDragInput? _input;
        private IDoor? _door;
        private IScrapInventory? _inventory;
        private DamagePointRegistry? _registry;

        private bool _dragging;

        /// <summary>
        /// VContainer injection point. Stores every collaborator at
        /// construction; no per-event DI resolve.
        /// </summary>
        [Inject]
        public void Construct(IDragInput input, IDoor door, IScrapInventory inventory, DamagePointRegistry registry)
        {
            _input = input ?? throw new ArgumentNullException(nameof(input));
            _door = door ?? throw new ArgumentNullException(nameof(door));
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _registry = registry ?? throw new ArgumentNullException(nameof(registry));
        }

        private void OnEnable()
        {
            if (_input is null) return;
            _input.OnDragStart += HandleDragStart;
            _input.OnDragEnd += HandleDragEnd;
        }

        private void OnDisable()
        {
            if (_input is null) return;
            _input.OnDragStart -= HandleDragStart;
            _input.OnDragEnd -= HandleDragEnd;
        }

        private void HandleDragStart(Vector2 coord)
        {
            _dragging = true;
        }

        private void HandleDragEnd(Vector2 coord)
        {
            if (!_dragging) return;
            _dragging = false;
            TryResolveDrop(coord);
        }

        /// <summary>
        /// Test-only entry: drives the resolve path without wiring a real
        /// <see cref="IDragInput"/> event source.
        /// </summary>
        /// <param name="coord">Door-local drop coordinate.</param>
        /// <returns><c>true</c> when a weld was applied.</returns>
        internal bool ResolveDropForTest(Vector2 coord) => TryResolveDrop(coord);

        private bool TryResolveDrop(Vector2 coord)
        {
            if (_registry is null || _door is null || _inventory is null) return false;
            if (!_registry.NearestTo(coord, out var nearest)) return false;
            if ((nearest - coord).sqrMagnitude > _snapRadius * _snapRadius) return false;
            if (_inventory.Count <= 0) return false;

            // Repair first; if the door rejects it (full HP), do not
            // consume the scrap — AC: "no wasted resource."
            if (!_door.ApplyRepair(RepairFraction, nearest)) return false;
            _inventory.TryUse();
            _registry.UnregisterPoint(nearest);
            return true;
        }
    }
}
