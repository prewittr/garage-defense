using System.Runtime.CompilerServices;

// Grant the paired juice test assembly access to internal test-only
// hooks (e.g. ImpactAudio.NextPitch, ImpactAudio.SetSeedForTest,
// ScreenShakeController.SampleAmplitudeForTest). Mirrors the pattern
// used by Rpm.Gameplay.Door so feel-layer tests can stay deterministic
// without polluting the public API surface.
[assembly: InternalsVisibleTo("Rpm.Juice.Tests")]
