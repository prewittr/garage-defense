#nullable enable

using System;
using Rpm.Core.Events;
using UnityEngine;
using VContainer;

namespace Rpm.Juice
{
    /// <summary>
    /// Plays <c>SFX_Metal_Impact</c> on every <see cref="DamageEvent"/>
    /// with pitch randomized in <c>[0.9, 1.1]</c>. Audio plays even when
    /// <see cref="AccessibilityFlags.ReduceMotion"/> is on — only the
    /// camera shake is suppressed.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. Wired in the gameplay scene to the
    /// metal-impact <see cref="AudioSource"/> (Editor-deferred per
    /// RPM-001 Notes). Volume-rides on HP per DESIGN-001 §SFX (ceiling
    /// +3dB below 30%); Sprint 1 implements the trigger and pitch
    /// randomization, the volume-ride is layered on top in a follow-up.
    /// </para>
    /// <para>
    /// <b>Determinism:</b> the pitch RNG is a seedable
    /// <see cref="System.Random"/> stored on the instance. Default seed is
    /// derived from the system clock; <see cref="SetSeedForTest"/>
    /// overrides it so unit tests can assert exact pitch sequences.
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> handler delegate cached; no per-event alloc.
    /// <see cref="System.Random.NextDouble"/> reuses the instance state.
    /// </para>
    /// </remarks>
    public sealed class ImpactAudio : MonoBehaviour
    {
        /// <summary>Lower pitch bound per DESIGN-001 §SFX.</summary>
        public const float MinPitch = 0.9f;

        /// <summary>Upper pitch bound per DESIGN-001 §SFX.</summary>
        public const float MaxPitch = 1.1f;

        [Tooltip("AudioSource configured with the SFX_Metal_Impact clip; one-shot, non-looping.")]
        [SerializeField] private AudioSource? _source;

        [Tooltip("SFX_Metal_Impact clip. Assigned during Editor-deferred wiring.")]
        [SerializeField] private AudioClip? _clip;

        private IEventBus? _bus;
        private Action<DamageEvent>? _handler;
        private System.Random _rng = new();

        /// <summary>VContainer injection point.</summary>
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
            if (_source is null || _clip is null) return;
            _source.pitch = NextPitch();
            _source.PlayOneShot(_clip);
        }

        /// <summary>
        /// Pure pitch generator: returns a value in <c>[MinPitch, MaxPitch]</c>
        /// drawn from the controller's seedable RNG. Exposed
        /// <c>internal</c> so EditMode tests can assert deterministic
        /// sequences against a fixed seed.
        /// </summary>
        /// <returns>Pitch multiplier for the next impact one-shot.</returns>
        internal float NextPitch()
        {
            return MinPitch + (float)(_rng.NextDouble() * (MaxPitch - MinPitch));
        }

        /// <summary>
        /// Test-only RNG seed override. Replaces the default
        /// system-clock-seeded <see cref="System.Random"/> with a
        /// fixed-seed instance so the pitch sequence is reproducible.
        /// </summary>
        /// <param name="seed">Deterministic seed.</param>
        internal void SetSeedForTest(int seed)
        {
            _rng = new System.Random(seed);
        }
    }
}
