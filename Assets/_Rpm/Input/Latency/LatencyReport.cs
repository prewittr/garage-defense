#nullable enable

using System;

namespace Rpm.Input.Latency
{
    /// <summary>
    /// Serializable snapshot of an input-latency probe run. Consumed by the CI
    /// <c>latency-gate</c> job; schema is a load-bearing contract.
    /// </summary>
    /// <remarks>
    /// Kept as a plain <c>[Serializable]</c> class (not a <c>record</c>) so Unity's
    /// <c>JsonUtility</c> — the CI's zero-dependency serializer — can round-trip it.
    /// The CI step reads only <see cref="p50"/> and <see cref="p95"/>; the
    /// <see cref="samples"/> array is retained for offline analysis.
    /// </remarks>
    [Serializable]
    public sealed class LatencyReport
    {
        /// <summary>Raw per-event latency deltas in milliseconds.</summary>
#pragma warning disable IDE1006 // naming: field names match JSON schema consumed by CI.
        public float[] samples = Array.Empty<float>();

        /// <summary>Median latency in milliseconds.</summary>
        public float p50;

        /// <summary>95th-percentile latency in milliseconds; gated by CI at ≤ 12ms.</summary>
        public float p95;
#pragma warning restore IDE1006
    }
}
