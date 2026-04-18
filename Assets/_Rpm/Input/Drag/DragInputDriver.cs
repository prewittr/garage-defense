#nullable enable
using UnityEngine;
using VContainer;

namespace Rpm.Input.Drag
{
    /// <summary>
    /// Scene-resident MonoBehaviour that drives the Singleton
    /// <see cref="DragInput"/>'s polled Pointer snapshot each frame.
    /// Drop one of these onto any persistent GameObject (typically the
    /// <c>Bootstrap</c> GameObject in <c>_Boot.unity</c> or a root object
    /// in <c>TactileTerror_S1.unity</c>).
    /// </summary>
    /// <remarks>
    /// Keeping the driver in its own MonoBehaviour — rather than having
    /// <see cref="DragInput"/> itself inherit MonoBehaviour — preserves
    /// the Singleton-IoC-friendly shape Kendra shipped in RPM-001.
    /// </remarks>
    public sealed class DragInputDriver : MonoBehaviour
    {
        [Inject] private DragInput _dragInput = default!;

        private void Update() => _dragInput.Tick();
    }
}
