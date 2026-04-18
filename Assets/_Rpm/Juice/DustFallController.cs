#nullable enable

using System;
using Rpm.Core.Events;
using UnityEngine;
using VContainer;

namespace Rpm.Juice
{
    /// <summary>
    /// On every <see cref="DamageEvent"/>, triggers a pre-warmed
    /// <see cref="ParticleSystem"/> from the rafters above the door —
    /// "the whole building reacts, not just the door."
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. The serialized
    /// <see cref="ParticleSystem"/> reference points at the rafter dust
    /// emitter prefab in the scene (Editor-deferred per RPM-001 Notes).
    /// </para>
    /// <para>
    /// <b>Pool strategy:</b> we lean on the <see cref="ParticleSystem"/>'s
    /// own internal particle pool. The system is configured
    /// <c>Stop Action = None</c> and <c>Play On Awake = false</c>, with
    /// <c>maxParticles</c> sized for two simultaneous bursts (DESIGN-001
    /// caps a burst at 6–8). The Emit() call writes into the pre-allocated
    /// particle array with zero managed allocation.
    /// </para>
    /// <para>
    /// <b>Accessibility:</b> dust still falls under <c>ReduceMotion</c>.
    /// DESIGN-001 §Accessibility Baseline: only camera shake and haptic
    /// rumble are suppressed; dust does not trigger vestibular discomfort.
    /// </para>
    /// </remarks>
    public sealed class DustFallController : MonoBehaviour
    {
        [Tooltip("Pre-warmed particle system in the scene (rafter dust emitter prefab).")]
        [SerializeField] private ParticleSystem? _emitter;

        [Tooltip("Particles per burst per impact. DESIGN-001 spec: 6-8.")]
        [SerializeField, Range(0, 32)] private int _particlesPerBurst = 7;

        private IEventBus? _bus;
        private Action<DamageEvent>? _handler;

        /// <summary>
        /// VContainer injection point. The handler delegate is cached so
        /// subscribe/unsubscribe cycles allocate nothing.
        /// </summary>
        /// <param name="bus">Shared event bus singleton.</param>
        [Inject]
        public void Construct(IEventBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        private void Awake()
        {
            _handler = OnDamage;
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

        private void OnDamage(DamageEvent evt)
        {
            if (_emitter is null || _particlesPerBurst <= 0) return;
            // Use Emit(int) with a count — the simplest zero-alloc path.
            // The emitter's local position determines the rafter origin;
            // the impact coord is intentionally not threaded through here
            // because dust falls from the building, not from the wound.
            _emitter.Emit(_particlesPerBurst);
        }
    }
}
