#nullable enable

using UnityEngine;

namespace Rpm.Juice.Config
{
    /// <summary>
    /// Designer-tunable knobs for <see cref="ScreenShakeController"/>.
    /// Authored as a <see cref="ScriptableObject"/> asset so Simone can
    /// retune feel without touching code or recompiling.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The Sprint 1 spec (DESIGN-001 §Screen Shake Curve) is:
    /// <c>shakeMagnitude = (1 - HP/MaxHP) * baseShake</c> driven through a
    /// <b>critically-damped spring</b> (damping ratio 1.0, natural
    /// frequency 20 rad/s). The fields below are the named knobs Malik and
    /// Simone agreed to expose. The default values reproduce DESIGN-001.
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> all reads from this asset happen during
    /// <c>Update</c> on the camera; no allocation. The
    /// <see cref="AmplitudeDecay"/> curve is sampled with
    /// <see cref="AnimationCurve.Evaluate(float)"/> which is alloc-free.
    /// </para>
    /// </remarks>
    [CreateAssetMenu(
        fileName = "ScreenShakeConfig",
        menuName = "Rpm/Juice/Screen Shake Config",
        order = 0)]
    public sealed class ScreenShakeConfig : ScriptableObject
    {
        [Tooltip("Designer curve sampled by normalized time t in [0,1] over the shake lifetime. Defaults to a critically-damped fall-off.")]
        [SerializeField] private AnimationCurve _amplitudeDecay = AnimationCurve.EaseInOut(0f, 1f, 1f, 0f);

        [Tooltip("Maximum positional offset (in world units) applied to the camera at full intensity. DESIGN-001 baseline at 0% HP impact = 1.0.")]
        [SerializeField, Range(0f, 2f)] private float _maxAmplitude = 1.0f;

        [Tooltip("Critically-damped spring natural frequency (rad/s). DESIGN-001 spec = 20.")]
        [SerializeField, Range(1f, 60f)] private float _springOmega = 20f;

        [Tooltip("Total seconds of shake life per impact before the curve sample falls off completely.")]
        [SerializeField, Range(0.05f, 2f)] private float _duration = 0.4f;

        [Tooltip("Per-axis frequency for the Perlin sampler driving the offset direction.")]
        [SerializeField, Range(1f, 60f)] private float _noiseFrequency = 22f;

        /// <summary>Designer-curve fall-off, sampled in normalized time [0,1].</summary>
        public AnimationCurve AmplitudeDecay => _amplitudeDecay;

        /// <summary>Maximum positional offset at full intensity (world units).</summary>
        public float MaxAmplitude => _maxAmplitude;

        /// <summary>Spring natural frequency (rad/s); damping ratio is fixed at 1.0 (critical).</summary>
        public float SpringOmega => _springOmega;

        /// <summary>Total seconds of life per shake impulse.</summary>
        public float Duration => _duration;

        /// <summary>Perlin noise frequency for offset-direction wobble.</summary>
        public float NoiseFrequency => _noiseFrequency;
    }
}
