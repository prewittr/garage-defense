#nullable enable

using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

namespace Rpm.Input.Drag
{
    /// <summary>
    /// Unity.InputSystem-backed <see cref="IDragInput"/>. Reads the active
    /// <see cref="Pointer"/> device (mouse or touchscreen — the system
    /// routes to whichever is current) so the same component serves
    /// desktop and mobile without a platform fork.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Lifetime: <c>Singleton</c>. The input surface is process-global
    /// rather than scene-scoped because Sprint 1 does not yet switch
    /// gameplay scopes at runtime.
    /// </para>
    /// <para>
    /// <b>Perf contract:</b> <see cref="Tick"/> avoids allocations — it
    /// reads the pointer's current position once per call and raises the
    /// cached <see cref="OnDrag"/> delegate. No per-frame LINQ, no
    /// <c>string</c> concatenation.
    /// </para>
    /// <para>
    /// This class deliberately does not derive from
    /// <see cref="MonoBehaviour"/>. The composition root registers it as
    /// a container-owned singleton and drives <see cref="Tick"/> from an
    /// Entry Point MonoBehaviour (wired in a follow-up Editor commit
    /// alongside scene authoring). Tests exercise the dispatch loop via
    /// the internal <see cref="RaiseForTest(DragPhase, Vector2)"/> helper.
    /// </para>
    /// </remarks>
    public sealed class DragInput : IDragInput, IDisposable
    {
        private bool _wasPressed;

        /// <inheritdoc/>
        public event Action<Vector2>? OnDragStart;

        /// <inheritdoc/>
        public event Action<Vector2>? OnDrag;

        /// <inheritdoc/>
        public event Action<Vector2>? OnDragEnd;

        /// <summary>
        /// Polls <see cref="Pointer.current"/> once and raises the
        /// appropriate event. Intended to be driven from a single Update
        /// MonoBehaviour in the composition root.
        /// </summary>
        public void Tick()
        {
            var pointer = Pointer.current;
            if (pointer is null) return;

            ButtonControl? pressControl = pointer.press;
            if (pressControl is null) return;

            var isPressed = pressControl.isPressed;
            var position = pointer.position.ReadValue();

            if (isPressed && !_wasPressed)
            {
                OnDragStart?.Invoke(position);
            }
            else if (isPressed)
            {
                OnDrag?.Invoke(position);
            }
            else if (_wasPressed)
            {
                OnDragEnd?.Invoke(position);
            }

            _wasPressed = isPressed;
        }

        /// <summary>Releases event subscriptions. Safe to call more than once.</summary>
        public void Dispose()
        {
            OnDragStart = null;
            OnDrag = null;
            OnDragEnd = null;
        }

        /// <summary>Drag-phase enum for <see cref="RaiseForTest(DragPhase, Vector2)"/>.</summary>
        internal enum DragPhase
        {
            /// <summary>Fire <see cref="OnDragStart"/>.</summary>
            Start,

            /// <summary>Fire <see cref="OnDrag"/>.</summary>
            Move,

            /// <summary>Fire <see cref="OnDragEnd"/>.</summary>
            End,
        }

        /// <summary>
        /// Test-only seam — raise a given phase without needing a Unity
        /// <see cref="Pointer"/> device. Used by EditMode tests and by the
        /// composition root's diagnostic harness.
        /// </summary>
        internal void RaiseForTest(DragPhase phase, Vector2 position)
        {
            switch (phase)
            {
                case DragPhase.Start: OnDragStart?.Invoke(position); break;
                case DragPhase.Move: OnDrag?.Invoke(position); break;
                case DragPhase.End: OnDragEnd?.Invoke(position); break;
            }
        }
    }
}
