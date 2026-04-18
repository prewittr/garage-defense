#nullable enable

using System;
using Rpm.Core.Door;
using Rpm.Core.Events;
using UnityEngine;
using VContainer;

namespace Rpm.Gameplay.Door
{
    /// <summary>
    /// Concrete garage-door HP owner for Sprint 1. Holds the authoritative
    /// mutable HP, clamps at the [0, <see cref="_maxHp"/>] boundaries, and
    /// emits <see cref="DamageEvent"/> / <see cref="RepairEvent"/> on the
    /// shared <see cref="IEventBus"/>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. One controller per gameplay scope
    /// (TactileTerror_S1 scene). Authored as a <see cref="MonoBehaviour"/>
    /// so the prefab that carries it (Kendra-owned per the pairing
    /// agreement in RPM-001) can live in the scene graph and collide with
    /// other Unity lifecycle systems cleanly.
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> every call path on this class is allocation
    /// free. The VContainer <see cref="InjectAttribute"/> happens at
    /// construction, not per-call; the event bus publish is a struct copy.
    /// </para>
    /// </remarks>
    public sealed class DoorController : MonoBehaviour, IDoor
    {
        [SerializeField] private float _maxHp = 100f;
        [SerializeField] private float _startingHp = 100f;

        private IEventBus? _bus;
        private float _current;

        /// <inheritdoc/>
        public DoorHP HP
        {
            get
            {
                var max = Mathf.Max(1f, _maxHp);
                var ratio = Mathf.Clamp01(_current / max);
                return new DoorHP(_current, max, ratio);
            }
        }

        /// <summary>
        /// VContainer injection point. Stored for use in
        /// <see cref="ApplyDamage"/> / <see cref="ApplyRepair"/> publishes.
        /// </summary>
        /// <param name="bus">Shared singleton event bus.</param>
        [Inject]
        public void Construct(IEventBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        private void Awake()
        {
            _current = Mathf.Clamp(_startingHp, 0f, _maxHp);
        }

        /// <summary>
        /// Test-only constructor seam. Bypasses the serialized fields so
        /// EditMode tests can spin up a controller without a scene.
        /// </summary>
        /// <param name="bus">Event bus instance.</param>
        /// <param name="maxHp">Maximum HP for this instance.</param>
        /// <param name="startingHp">Initial HP; clamped to <paramref name="maxHp"/>.</param>
        internal void InitForTest(IEventBus bus, float maxHp, float startingHp)
        {
            _bus = bus;
            _maxHp = Mathf.Max(1f, maxHp);
            _current = Mathf.Clamp(startingHp, 0f, _maxHp);
        }

        /// <inheritdoc/>
        public void ApplyDamage(float fractionOfMax, Vector2 coord)
        {
            if (fractionOfMax <= 0f) return;
            var amount = fractionOfMax * _maxHp;
            _current = Mathf.Max(0f, _current - amount);
            _bus?.Publish(new DamageEvent(fractionOfMax, coord, Time.timeAsDouble));
        }

        /// <inheritdoc/>
        public bool ApplyRepair(float fractionOfMax, Vector2 coord)
        {
            if (fractionOfMax <= 0f) return false;
            if (_current >= _maxHp) return false;
            var amount = fractionOfMax * _maxHp;
            _current = Mathf.Min(_maxHp, _current + amount);
            _bus?.Publish(new RepairEvent(fractionOfMax, coord, Time.timeAsDouble));
            return true;
        }
    }
}
