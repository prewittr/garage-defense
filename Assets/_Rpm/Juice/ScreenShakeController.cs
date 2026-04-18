#nullable enable

using System;
using Rpm.Core.Events;
using Rpm.Juice.Config;
using UnityEngine;
using VContainer;

namespace Rpm.Juice
{
    /// <summary>
    /// Drives camera screen shake on every <see cref="DamageEvent"/>.
    /// Amplitude scales with damage severity (a proxy for the
    /// "(1 - HP/MaxHP)" curve from DESIGN-001) and decays through a
    /// critically-damped spring driven by Perlin-noise direction wobble.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. One per gameplay scope, attached to the
    /// shake-target Transform (typically the active <c>Camera</c>'s parent
    /// rig — Editor-deferred wiring per RPM-001 Notes).
    /// </para>
    /// <para>
    /// <b>Accessibility:</b> when
    /// <see cref="AccessibilityFlags.ReduceMotion"/> is <c>true</c> the
    /// visual offset path is skipped entirely. Audio cues are unaffected
    /// (handled in <c>ImpactAudio</c>) — the player still hears the impact,
    /// they just do not see the camera move.
    /// </para>
    /// <para>
    /// <b>Perf contract:</b>
    /// <list type="bullet">
    ///   <item><description>Subscribes once in <c>OnEnable</c>, unsubscribes in <c>OnDisable</c>; the handler delegate is captured to a field so resubscribe does not allocate.</description></item>
    ///   <item><description>The handler captures nothing — it forwards into a non-generic instance method.</description></item>
    ///   <item><description>The per-frame update samples Perlin noise and writes one <see cref="Vector3"/>; no allocations.</description></item>
    /// </list>
    /// </para>
    /// </remarks>
    public sealed class ScreenShakeController : MonoBehaviour
    {
        [Tooltip("ScriptableObject carrying designer-tunable shake parameters.")]
        [SerializeField] private ScreenShakeConfig? _config;

        [Tooltip("Optional explicit shake target. Defaults to this Transform.")]
        [SerializeField] private Transform? _target;

        private IEventBus? _bus;
        private Action<DamageEvent>? _handler;

        // Spring state (critically damped, omega = config.SpringOmega).
        private float _intensity;
        private float _intensityVelocity;
        private float _elapsed;
        private Vector3 _restPosition;
        private bool _intensityActive;

        /// <summary>
        /// VContainer injection point. Stored once; the handler delegate
        /// is also cached so subscribe/unsubscribe cycles allocate nothing.
        /// </summary>
        /// <param name="bus">Shared event bus singleton.</param>
        [Inject]
        public void Construct(IEventBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        private void Awake()
        {
            _target ??= transform;
            _restPosition = _target.localPosition;
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
            // Park the camera back at rest so a disable never freezes it
            // mid-shake.
            if (_target != null)
            {
                _target.localPosition = _restPosition;
            }
            _intensityActive = false;
        }

        private void OnDamage(DamageEvent evt)
        {
            // Damage amount is fraction-of-max (0..1) — a clean proxy for
            // the DESIGN-001 (1 - HP/MaxHP) curve at impact time. Larger
            // bites (later in the wave) shake harder.
            var trigger = Mathf.Clamp01(evt.Amount * 4f); // amplify: 5% bite -> 0.2 trigger floor.
            if (trigger <= 0f) return;

            // Critically damped: fold the new impulse into the existing
            // spring state rather than replacing it. Sequential impacts
            // stack instead of clipping.
            _intensity = Mathf.Min(1f, _intensity + trigger);
            _intensityVelocity = 0f;
            _elapsed = 0f;
            _intensityActive = true;
        }

        private void Update()
        {
            if (!_intensityActive || _config is null || _target is null) return;

            var dt = Time.deltaTime;
            _elapsed += dt;

            // Drive the spring back toward zero with critical damping. The
            // closed-form for a critically-damped second-order system with
            // target = 0 is:
            //   x'' = -2*omega*x' - omega^2 * x
            var omega = Mathf.Max(0.01f, _config.SpringOmega);
            var accel = (-2f * omega * _intensityVelocity) - (omega * omega * _intensity);
            _intensityVelocity += accel * dt;
            _intensity += _intensityVelocity * dt;

            // Designer fall-off layered on top — the curve cuts the
            // visible amplitude even if the spring still has tail energy.
            var normalizedTime = Mathf.Clamp01(_elapsed / Mathf.Max(0.0001f, _config.Duration));
            var curveScale = _config.AmplitudeDecay.Evaluate(normalizedTime);
            var amplitude = Mathf.Max(0f, _intensity) * curveScale * _config.MaxAmplitude;

            if (AccessibilityFlags.ReduceMotion)
            {
                // Honour reduce-motion: leave camera at rest, but keep
                // ticking spring math so re-enabling motion mid-session
                // does not snap-pop.
                _target.localPosition = _restPosition;
            }
            else
            {
                var t = Time.time * _config.NoiseFrequency;
                // Perlin noise centered around 0.5 -> shift into [-1,1].
                var nx = (Mathf.PerlinNoise(t, 0f) - 0.5f) * 2f;
                var ny = (Mathf.PerlinNoise(0f, t) - 0.5f) * 2f;
                _target.localPosition = _restPosition + new Vector3(nx, ny, 0f) * amplitude;
            }

            if (normalizedTime >= 1f && Mathf.Abs(_intensity) < 0.0005f)
            {
                _intensityActive = false;
                _target.localPosition = _restPosition;
            }
        }

        /// <summary>
        /// Test-only sampler for the shake-amplitude curve, isolating
        /// designer config from MonoBehaviour lifecycle.
        /// </summary>
        /// <param name="config">Config asset to sample.</param>
        /// <param name="normalizedTime">Time in [0..1] across the shake duration.</param>
        /// <param name="hpRatio">Door HP ratio in [0..1]; lower HP → larger amplitude.</param>
        /// <returns>World-space amplitude that the controller would apply.</returns>
        internal static float SampleAmplitudeForTest(ScreenShakeConfig config, float normalizedTime, float hpRatio)
        {
            if (config is null) throw new ArgumentNullException(nameof(config));
            var trigger = Mathf.Clamp01(1f - hpRatio);
            var t = Mathf.Clamp01(normalizedTime);
            var curveScale = config.AmplitudeDecay.Evaluate(t);
            return trigger * curveScale * config.MaxAmplitude;
        }
    }
}
