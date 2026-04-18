#nullable enable

using System;
using Rpm.Core.Door;
using UnityEngine;
using VContainer;

namespace Rpm.Gameplay.Door
{
    /// <summary>
    /// Drives the Sprint 1 impact cadence: every
    /// <see cref="_intervalSeconds"/> seconds, apply a fixed 5% HP damage
    /// bite at a randomized door-local coordinate.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. One instance per gameplay scope. This is
    /// the only systemic damage source in Sprint 1 — player-facing
    /// escalation and wave curves are deferred to RPM-002.
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> the per-tick hot path allocates nothing. The
    /// random coord uses <see cref="UnityEngine.Random.value"/>, and the
    /// <see cref="Vector2"/> passed to
    /// <see cref="IDoor.ApplyDamage"/> is a stack-local struct.
    /// </para>
    /// </remarks>
    public sealed class ImpactScheduler : MonoBehaviour
    {
        [Tooltip("Seconds between zombie-hit impacts. Sprint 1 cadence is flat 1.5s.")]
        [SerializeField] private float _intervalSeconds = 1.5f;

        [Tooltip("Fraction of max HP lost per impact. Sprint 1 spec is 5% (0.05).")]
        [SerializeField, Range(0f, 1f)] private float _damageFraction = 0.05f;

        [Tooltip("Door-local bounds for randomized damage placement (half-extent).")]
        [SerializeField] private Vector2 _coordRange = new(0.4f, 0.4f);

        private IDoor? _door;
        private float _accumulator;
        private bool _active;

        /// <summary>
        /// VContainer injection point. <see cref="IDoor"/> is the only
        /// contract the scheduler needs — keeps it decoupled from the
        /// concrete <see cref="DoorController"/>.
        /// </summary>
        /// <param name="door">Door the scheduler will damage.</param>
        [Inject]
        public void Construct(IDoor door)
        {
            _door = door ?? throw new ArgumentNullException(nameof(door));
        }

        private void OnEnable()
        {
            _accumulator = 0f;
            _active = true;
        }

        private void OnDisable()
        {
            _active = false;
        }

        private void Update()
        {
            if (!_active || _door is null) return;
            _accumulator += Time.deltaTime;
            if (_accumulator < _intervalSeconds) return;
            _accumulator -= _intervalSeconds;
            Fire();
        }

        /// <summary>
        /// Test-only entry: fires one impact immediately without waiting
        /// for the accumulator. Used by EditMode tests that want
        /// deterministic tick behaviour.
        /// </summary>
        internal void FireForTest()
        {
            Fire();
        }

        private void Fire()
        {
            var coord = new Vector2(
                UnityEngine.Random.Range(-_coordRange.x, _coordRange.x),
                UnityEngine.Random.Range(-_coordRange.y, _coordRange.y));
            _door!.ApplyDamage(_damageFraction, coord);
        }
    }
}
