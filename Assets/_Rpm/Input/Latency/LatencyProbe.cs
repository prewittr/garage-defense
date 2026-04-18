#nullable enable

using System;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

namespace Rpm.Input.Latency
{
    /// <summary>
    /// Editor-deferred MonoBehaviour harness that measures the delta between
    /// an input event's device timestamp and the render-frame wall-clock in
    /// milliseconds. After <see cref="TargetSampleCount"/> samples — or
    /// <see cref="CaptureWindowSeconds"/>, whichever comes first — writes a
    /// <see cref="LatencyReport"/> JSON file to <c>Builds/latency-report.json</c>.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Zero GC alloc in the measurement loop. The rolling sample buffer and the
    /// <see cref="LatencyReport"/> are allocated exactly once in <see cref="Awake"/>;
    /// the per-event path only writes to pre-existing <c>float[]</c> slots.
    /// </para>
    /// <para>
    /// This component requires a <c>Scenes/LatencyProbe.unity</c> scene with a
    /// scripted drag harness to actually produce events — see RPM-008 "Editor-deferred"
    /// section. Until that scene exists and is entered in Play Mode, no report file
    /// is produced; the CI latency-gate handles the missing-file case gracefully.
    /// </para>
    /// <para>
    /// Self-overhead budget per TP-RPM-008: ≤ 0.5ms added to measured latency.
    /// </para>
    /// </remarks>
    public sealed class LatencyProbe : MonoBehaviour
    {
        /// <summary>Number of samples to capture before writing the report.</summary>
        public const int TargetSampleCount = 100;

        /// <summary>Maximum wall-clock capture window in seconds before early-flushing.</summary>
        public const float CaptureWindowSeconds = 5f;

        /// <summary>Report path relative to the project root. Load-bearing for CI.</summary>
        public const string ReportRelativePath = "Builds/latency-report.json";

        // Pre-allocated hot-path state. Allocated once in Awake.
        private float[] _samples = Array.Empty<float>();
        private int _sampleCount;
        private float _captureStartTime;
        private bool _reportWritten;
        private LatencyReport? _report;

        /// <summary>Number of samples captured so far this run. Read-only view for diagnostics.</summary>
        public int SampleCount => _sampleCount;

        /// <summary>True once the report JSON has been flushed to disk.</summary>
        public bool ReportWritten => _reportWritten;

        private void Awake()
        {
            _samples = new float[TargetSampleCount];
            _sampleCount = 0;
            _reportWritten = false;
            _report = new LatencyReport();

            // Subscribe to the Input System's low-level event stream. This gives us
            // the event's device timestamp — which predates the Unity frame — so the
            // delta we compute captures the true input-to-render latency.
            InputSystem.onEvent += OnInputEvent;

            _captureStartTime = Time.realtimeSinceStartup;
        }

        private void OnDestroy()
        {
            InputSystem.onEvent -= OnInputEvent;
        }

        private void Update()
        {
            if (_reportWritten) return;

            bool capacityReached = _sampleCount >= TargetSampleCount;
            bool windowExpired = (Time.realtimeSinceStartup - _captureStartTime) >= CaptureWindowSeconds;

            if (capacityReached || (windowExpired && _sampleCount > 0))
            {
                WriteReport();
            }
        }

        /// <summary>
        /// Zero-alloc per-event handler. Computes wall-clock delta between the
        /// device event timestamp and now, converts to ms, and stores in the
        /// preallocated buffer.
        /// </summary>
        private void OnInputEvent(InputEventPtr eventPtr, InputDevice device)
        {
            if (_sampleCount >= TargetSampleCount) return;

            // InputEvent.time is seconds since app start (same domain as realtimeSinceStartup).
            double nowSeconds = Time.realtimeSinceStartupAsDouble;
            double eventSeconds = eventPtr.time;
            double deltaMs = (nowSeconds - eventSeconds) * 1000.0;

            // Reject implausible negatives caused by clock-domain skew on some platforms.
            if (deltaMs < 0.0) deltaMs = 0.0;

            _samples[_sampleCount] = (float)deltaMs;
            _sampleCount++;
        }

        private void WriteReport()
        {
            if (_report is null) return;

            // Allocation here is intentional and one-shot (out of the hot path):
            // we build a right-sized results array for the report payload.
            var payload = new float[_sampleCount];
            Array.Copy(_samples, payload, _sampleCount);

            _report.samples = payload;

            // Percentile calculation reuses the preallocated _samples array as its
            // scratch buffer (Statistics sorts in-place). The payload array above
            // preserves the original ordering for offline analysis.
            _report.p50 = Statistics.PercentileP50(_samples, _sampleCount);
            _report.p95 = Statistics.PercentileP95(_samples, _sampleCount);

            string json = JsonUtility.ToJson(_report);

            string directory = Path.GetDirectoryName(ReportRelativePath);
            if (!string.IsNullOrEmpty(directory) && !Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            File.WriteAllText(ReportRelativePath, json);
            _reportWritten = true;

            Debug.Log($"[LatencyProbe] Report written: {ReportRelativePath} · p50={_report.p50:F2}ms · p95={_report.p95:F2}ms · n={_sampleCount}");
        }
    }
}
