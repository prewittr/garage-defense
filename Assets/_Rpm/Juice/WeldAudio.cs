#nullable enable

using System;
using Rpm.Core.Events;
using UnityEngine;
using VContainer;

namespace Rpm.Juice
{
    /// <summary>
    /// Plays <c>SFX_Scrap_Weld</c> (clink-hiss envelope) on every
    /// <see cref="RepairEvent"/>. The "relief beat" half of the Sprint 1
    /// tension-release loop.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Scoped</c>. Wired in the gameplay scene to the
    /// weld-audio <see cref="AudioSource"/> (Editor-deferred per RPM-001
    /// Notes). DESIGN-001 §SFX specifies the clip should be spatialized
    /// to the door's drop coord — Sprint 1 plays at the source position
    /// and the Editor-deferred step sets up stereo panning.
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> handler delegate cached at <c>Awake</c>;
    /// no per-event alloc; one <see cref="AudioSource.PlayOneShot(AudioClip)"/>
    /// call per repair.
    /// </para>
    /// </remarks>
    public sealed class WeldAudio : MonoBehaviour
    {
        [Tooltip("AudioSource that plays the SFX_Scrap_Weld clink-hiss.")]
        [SerializeField] private AudioSource? _source;

        [Tooltip("SFX_Scrap_Weld clip. Assigned during Editor-deferred wiring.")]
        [SerializeField] private AudioClip? _clip;

        private IEventBus? _bus;
        private Action<RepairEvent>? _handler;

        /// <summary>VContainer injection point.</summary>
        /// <param name="bus">Shared event bus singleton.</param>
        [Inject]
        public void Construct(IEventBus bus)
        {
            _bus = bus ?? throw new ArgumentNullException(nameof(bus));
        }

        private void Awake()
        {
            _handler = OnRepair;
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

        private void OnRepair(RepairEvent evt)
        {
            if (_source is null || _clip is null) return;
            _source.PlayOneShot(_clip);
        }
    }
}
