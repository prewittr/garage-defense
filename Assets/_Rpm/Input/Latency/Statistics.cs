#nullable enable

using System;

namespace Rpm.Input.Latency
{
    /// <summary>
    /// Zero-allocation percentile helpers over <see cref="float"/> samples.
    /// </summary>
    /// <remarks>
    /// All methods operate on a caller-owned working buffer to avoid GC allocation in the
    /// measurement hot path. Callers are expected to pre-allocate and reuse a single
    /// <see cref="float"/> scratch array. Callers must never pass the live sample buffer
    /// directly — the helpers mutate the working buffer in-place while sorting.
    /// Linear interpolation between adjacent ranks is used, per NIST definition 7.
    /// </remarks>
    public static class Statistics
    {
        /// <summary>
        /// Compute the median (p50) of the first <paramref name="count"/> elements of
        /// <paramref name="workingBuffer"/>. Mutates <paramref name="workingBuffer"/> (sorts it).
        /// Zero allocation.
        /// </summary>
        /// <param name="workingBuffer">
        /// Scratch array owned by the caller; contents are destroyed. Must be populated
        /// with the samples to analyse in <c>[0, count)</c> before the call.
        /// </param>
        /// <param name="count">Number of valid samples at the head of the buffer.</param>
        /// <returns>p50 in the same units as the input samples. Returns 0 when <paramref name="count"/> is 0.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="workingBuffer"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="count"/> is negative or exceeds buffer length.</exception>
        public static float PercentileP50(float[] workingBuffer, int count)
            => Percentile(workingBuffer, count, 0.50f);

        /// <summary>
        /// Compute the 95th-percentile (p95) of the first <paramref name="count"/> elements of
        /// <paramref name="workingBuffer"/>. Mutates <paramref name="workingBuffer"/> (sorts it).
        /// Zero allocation.
        /// </summary>
        /// <param name="workingBuffer">
        /// Scratch array owned by the caller; contents are destroyed. Must be populated
        /// with the samples to analyse in <c>[0, count)</c> before the call.
        /// </param>
        /// <param name="count">Number of valid samples at the head of the buffer.</param>
        /// <returns>p95 in the same units as the input samples. Returns 0 when <paramref name="count"/> is 0.</returns>
        public static float PercentileP95(float[] workingBuffer, int count)
            => Percentile(workingBuffer, count, 0.95f);

        /// <summary>
        /// Generalised percentile (linear interpolation between closest ranks).
        /// </summary>
        /// <param name="workingBuffer">Caller-owned scratch buffer; sorted in-place.</param>
        /// <param name="count">Number of valid samples at the head of the buffer.</param>
        /// <param name="p">Percentile in the closed interval [0, 1].</param>
        /// <returns>Interpolated percentile value. Returns 0 when <paramref name="count"/> is 0.</returns>
        public static float Percentile(float[] workingBuffer, int count, float p)
        {
            if (workingBuffer is null) throw new ArgumentNullException(nameof(workingBuffer));
            if (count < 0 || count > workingBuffer.Length)
                throw new ArgumentOutOfRangeException(nameof(count));
            if (p < 0f || p > 1f)
                throw new ArgumentOutOfRangeException(nameof(p), "Percentile must be in [0, 1].");
            if (count == 0) return 0f;
            if (count == 1) return workingBuffer[0];

            // Array.Sort on a typed float[] is zero-alloc in modern .NET / Mono.
            Array.Sort(workingBuffer, 0, count);

            // NIST method 7: rank = p * (N - 1); linear interp between floor and ceil.
            float rank = p * (count - 1);
            int lo = (int)Math.Floor(rank);
            int hi = (int)Math.Ceiling(rank);
            if (lo == hi) return workingBuffer[lo];
            float frac = rank - lo;
            return workingBuffer[lo] + frac * (workingBuffer[hi] - workingBuffer[lo]);
        }
    }
}
