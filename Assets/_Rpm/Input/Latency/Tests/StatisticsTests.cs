#nullable enable

using System;
using NUnit.Framework;

namespace Rpm.Input.Latency.Tests
{
    /// <summary>
    /// EditMode tests for <see cref="Statistics"/>. Validates percentile math against
    /// known-answer synthetic sample arrays so regressions fail CI rather than leaking
    /// into CI's latency-gate report.
    /// </summary>
    [TestFixture]
    public sealed class StatisticsTests
    {
        private const float Epsilon = 1e-4f;

        [Test]
        public void P50_OfTenAscendingIntegers_IsMidpoint()
        {
            // 1..10: ranks 0..9, p50 → rank 4.5 → between 5 and 6 → 5.5.
            float[] buffer = { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            float p50 = Statistics.PercentileP50(buffer, buffer.Length);
            Assert.That(p50, Is.EqualTo(5.5f).Within(Epsilon));
        }

        [Test]
        public void P95_OfOneHundredAscendingIntegers_IsLinearlyInterpolated()
        {
            // 1..100: p95 rank = 0.95 * 99 = 94.05 → 95 + 0.05*(96-95) = 95.05.
            float[] buffer = new float[100];
            for (int i = 0; i < 100; i++) buffer[i] = i + 1;
            float p95 = Statistics.PercentileP95(buffer, buffer.Length);
            Assert.That(p95, Is.EqualTo(95.05f).Within(Epsilon));
        }

        [Test]
        public void Percentile_OnSinglySampledArray_ReturnsThatSample()
        {
            float[] buffer = { 42f, 0f, 0f, 0f };
            Assert.That(Statistics.PercentileP50(buffer, 1), Is.EqualTo(42f).Within(Epsilon));
            Assert.That(Statistics.PercentileP95(buffer, 1), Is.EqualTo(42f).Within(Epsilon));
        }

        [Test]
        public void Percentile_OnEmptyCount_ReturnsZero()
        {
            float[] buffer = { 5f, 5f, 5f };
            Assert.That(Statistics.PercentileP50(buffer, 0), Is.EqualTo(0f).Within(Epsilon));
            Assert.That(Statistics.PercentileP95(buffer, 0), Is.EqualTo(0f).Within(Epsilon));
        }

        [Test]
        public void Percentile_HandlesUnsortedInput_BySortingInPlace()
        {
            float[] buffer = { 10, 2, 7, 1, 5, 9, 3, 8, 4, 6 };
            float p50 = Statistics.PercentileP50(buffer, buffer.Length);
            // Sorted: 1..10 → p50 = 5.5.
            Assert.That(p50, Is.EqualTo(5.5f).Within(Epsilon));
            // Post-condition: buffer is now sorted ascending.
            for (int i = 1; i < buffer.Length; i++)
            {
                Assert.That(buffer[i], Is.GreaterThanOrEqualTo(buffer[i - 1]));
            }
        }

        [Test]
        public void Percentile_WhenP95BelowGateThreshold_RepresentsPassingRun()
        {
            // Synthetic: 95 samples at 8ms, 5 samples at 11.9ms. p95 must land ≤ 12.
            float[] buffer = new float[100];
            for (int i = 0; i < 95; i++) buffer[i] = 8f;
            for (int i = 95; i < 100; i++) buffer[i] = 11.9f;
            float p95 = Statistics.PercentileP95(buffer, buffer.Length);
            Assert.That(p95, Is.LessThanOrEqualTo(12f));
        }

        [Test]
        public void Percentile_WhenP95AboveGateThreshold_RepresentsFailingRun()
        {
            // Synthetic: 90 samples at 8ms, 10 samples at 25ms. p95 must land > 12.
            float[] buffer = new float[100];
            for (int i = 0; i < 90; i++) buffer[i] = 8f;
            for (int i = 90; i < 100; i++) buffer[i] = 25f;
            float p95 = Statistics.PercentileP95(buffer, buffer.Length);
            Assert.That(p95, Is.GreaterThan(12f));
        }

        [Test]
        public void Percentile_WithOutOfRangeProbability_Throws()
        {
            float[] buffer = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>(() => Statistics.Percentile(buffer, 3, -0.1f));
            Assert.Throws<ArgumentOutOfRangeException>(() => Statistics.Percentile(buffer, 3, 1.1f));
        }

        [Test]
        public void Percentile_WithNullBuffer_Throws()
        {
            Assert.Throws<ArgumentNullException>(() => Statistics.PercentileP50(null!, 0));
        }

        [Test]
        public void Percentile_WithNegativeCount_Throws()
        {
            float[] buffer = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>(() => Statistics.PercentileP50(buffer, -1));
        }

        [Test]
        public void Percentile_WithCountExceedingBuffer_Throws()
        {
            float[] buffer = { 1, 2, 3 };
            Assert.Throws<ArgumentOutOfRangeException>(() => Statistics.PercentileP50(buffer, 4));
        }
    }
}
