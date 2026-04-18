#nullable enable

using System;
using UnityEngine;

namespace Rpm.Input.Drag
{
    /// <summary>
    /// Device-agnostic drag-gesture surface. Mouse, touch, and (future)
    /// gamepad-cursor input all publish through the same three events.
    /// </summary>
    /// <remarks>
    /// Coordinates are emitted in screen-space pixels at publish time;
    /// the downstream drag handler converts to door-local space. Keeping
    /// the contract in screen space keeps <c>Rpm.Input</c> independent of
    /// the gameplay coordinate system.
    /// </remarks>
    public interface IDragInput
    {
        /// <summary>Fires when the user first presses down.</summary>
        event Action<Vector2>? OnDragStart;

        /// <summary>Fires on every pointer move while a drag is active.</summary>
        event Action<Vector2>? OnDrag;

        /// <summary>Fires when the user releases.</summary>
        event Action<Vector2>? OnDragEnd;
    }
}
