#nullable enable

using System;
using Rpm.Core.Events;
using UnityEngine;
using VContainer;

namespace Rpm.Juice
{
    /// <summary>
    /// On every <see cref="RepairEvent"/>, fires a tight spark burst at
    /// the repair coord — 8 particles max, single SRP pass, additive
    /// blend. The "weld snap" tactile reward described in DESIGN-001
    /// §Moment-to-Moment.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. Wired in the gameplay scene to a
    /// pre-warmed <see cref="ParticleSystem"/> using a shared additive
    /// material so every spark burst batches into one SRP draw call. No
    /// material instances are created at runtime — that would break the
    /// SRP batcher and is forbidden by the AC.
    /// </para>
    /// <para>
    /// <b>Coordinate mapping:</b> <see cref="RepairEvent.Coord"/> is in
    /// door-local 2D space (matches DamagePointRegistry). The serialized
    /// <see cref="_doorPlane"/> Transform is the door's surface plane;
    /// we treat the X/Y of the coord as offsets along the plane's
    /// local X/Y axes. If <see cref="_doorPlane"/> is null the controller
    /// falls back to firing at its own position (test-friendly).
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> handler is cached, captures nothing.
    /// <see cref="ParticleSystem.Emit(ParticleSystem.EmitParams, int)"/>
    /// re-uses the same <see cref="ParticleSystem.EmitParams"/> field —
    /// zero alloc.
    /// </para>
    /// </remarks>
    public sealed class WeldSparkController : MonoBehaviour
    {
        [Tooltip("Pre-warmed spark particle system. Material must be the shared additive spark material to keep SRP batching intact.")]
        [SerializeField] private ParticleSystem? _emitter;

        [Tooltip("Door surface Transform; the impact coord is interpreted as a local-space (X, Y) offset on this plane.")]
        [SerializeField] private Transform? _doorPlane;

        [Tooltip("Particles per spark burst. DESIGN-001 cap: <= 8.")]
        [SerializeField, Range(0, 16)] private int _particlesPerBurst = 8;

        private IEventBus? _bus;
        private Action<RepairEvent>? _handler;
        private ParticleSystem.EmitParams _emitParams;

        /// <summary>VContainer injection point. Caches handler delegate.</summary>
        /// <param name="bus">Shared event bus singleton.</param>
        [Inject]
        public void Construct(IEventBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        private void Awake()
        {
            _handler = OnRepair;
        }

        private void OnEnable()
        {
            if (_bus is null || _handler is null) return;
            _bus.Subscribe(_handler);
        }

        private void OnDisable()
        {
            if (_bus is null || _handler is null) return;
            _bus.Unsubscribe(_handler);
        }

        private void OnRepair(RepairEvent evt)
        {
            // Unity-aware null check: `is null` bypasses UnityEngine.Object's
            // overloaded `==` and fails to catch unassigned [SerializeField]
            // references. Using `== null` respects Unity's fake-null sentinel.
            if (_emitter == null || _particlesPerBurst <= 0) return;

            Vector3 worldPos;
            if (_doorPlane != null)
            {
                worldPos = _doorPlane.TransformPoint(new Vector3(evt.Coord.x, evt.Coord.y, 0f));
            }
            else
            {
                worldPos = transform.position;
            }

            _emitParams.position = worldPos;
            _emitParams.applyShapeToPosition = false;
            _emitter.Emit(_emitParams, _particlesPerBurst);
        }
    }
}
