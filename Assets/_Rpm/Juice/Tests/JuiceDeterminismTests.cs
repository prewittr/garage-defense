#nullable enable

using NUnit.Framework;
using Rpm.Juice.Config;
using UnityEngine;

namespace Rpm.Juice.Tests
{
    /// <summary>
    /// EditMode coverage for the deterministic, math-only paths in the
    /// feel layer:
    /// <list type="bullet">
    ///   <item><description><see cref="ImpactAudio.NextPitch"/> against a fixed seed.</description></item>
    ///   <item><description><see cref="ScreenShakeController.SampleAmplitudeForTest"/> at known curve inputs.</description></item>
    /// </list>
    /// MonoBehaviour event-handler wiring is exercised in PlayMode by
    /// Jasmine; this fixture stays Edit-only to keep CI fast and to
    /// avoid scene-graph dependencies.
    /// </summary>
    [TestFixture]
    public sealed class JuiceDeterminismTests
    {
        private GameObject? _go;

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.DestroyImmediate(_go);
            _go = null;
        }

        [Test]
        public void ImpactAudio_NextPitch_Stays_Within_DesignSpec_Bounds()
        {
            _go = new GameObject("ImpactAudioRig");
            var audio = _go.AddComponent<ImpactAudio>();
            audio.SetSeedForTest(42);

            for (var i = 0; i < 1024; i++)
            {
                var pitch = audio.NextPitch();
                Assert.GreaterOrEqual(pitch, ImpactAudio.MinPitch, $"pitch {pitch} below MinPitch");
                Assert.LessOrEqual(pitch, ImpactAudio.MaxPitch, $"pitch {pitch} above MaxPitch");
            }
        }

        [Test]
        public void ImpactAudio_NextPitch_Is_Deterministic_For_Fixed_Seed()
        {
            _go = new GameObject("ImpactAudioDeterminismRig");
            var a = _go.AddComponent<ImpactAudio>();
            var b = _go.AddComponent<ImpactAudio>();
            a.SetSeedForTest(1234);
            b.SetSeedForTest(1234);

            for (var i = 0; i < 16; i++)
            {
                Assert.AreEqual(a.NextPitch(), b.NextPitch(), 0.0000001f,
                    $"Pitch sequence diverged at index {i}");
            }
        }

        [Test]
        public void ImpactAudio_NextPitch_Two_Different_Seeds_Diverge()
        {
            _go = new GameObject("ImpactAudioDivergeRig");
            var a = _go.AddComponent<ImpactAudio>();
            var b = _go.AddComponent<ImpactAudio>();
            a.SetSeedForTest(1);
            b.SetSeedForTest(2);

            // Pull a few samples — at least one should differ.
            var anyDiff = false;
            for (var i = 0; i < 8; i++)
            {
                if (Mathf.Abs(a.NextPitch() - b.NextPitch()) > 0.0001f)
                {
                    anyDiff = true;
                    break;
                }
            }
            Assert.IsTrue(anyDiff, "Two different seeds should produce different pitch sequences.");
        }

        [Test]
        public void ScreenShake_Amplitude_At_Full_HP_Is_Zero()
        {
            var config = ScriptableObject.CreateInstance<ScreenShakeConfig>();
            try
            {
                // hpRatio = 1.0 -> trigger = 0 -> amplitude = 0 regardless of curve.
                var amplitude = ScreenShakeController.SampleAmplitudeForTest(config, 0f, 1f);
                Assert.AreEqual(0f, amplitude, 0.0001f);
            }
            finally
            {
                Object.DestroyImmediate(config);
            }
        }

        [Test]
        public void ScreenShake_Amplitude_At_Zero_HP_At_Curve_Start_Equals_MaxAmplitude()
        {
            var config = ScriptableObject.CreateInstance<ScreenShakeConfig>();
            try
            {
                // The default decay curve (EaseInOut(0,1, 1,0)) evaluates
                // to 1.0 at t=0 by construction. With hpRatio=0 the
                // trigger is also 1.0, so amplitude should equal
                // MaxAmplitude.
                var amplitude = ScreenShakeController.SampleAmplitudeForTest(config, 0f, 0f);
                Assert.AreEqual(config.MaxAmplitude, amplitude, 0.0001f);
            }
            finally
            {
                Object.DestroyImmediate(config);
            }
        }

        [Test]
        public void ScreenShake_Amplitude_Decays_Monotonically_Under_Default_Curve()
        {
            var config = ScriptableObject.CreateInstance<ScreenShakeConfig>();
            try
            {
                // Default curve is EaseInOut(0,1, 1,0): non-increasing.
                var prev = ScreenShakeController.SampleAmplitudeForTest(config, 0f, 0f);
                for (var i = 1; i <= 10; i++)
                {
                    var t = i / 10f;
                    var current = ScreenShakeController.SampleAmplitudeForTest(config, t, 0f);
                    Assert.LessOrEqual(current, prev + 0.0001f,
                        $"Amplitude should not rise under the default decay curve (t={t}, prev={prev}, cur={current}).");
                    prev = current;
                }
                Assert.AreEqual(0f, prev, 0.0001f, "Default curve should reach zero at t=1.");
            }
            finally
            {
                Object.DestroyImmediate(config);
            }
        }
    }
}
