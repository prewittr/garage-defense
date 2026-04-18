#nullable enable

using UnityEngine;

namespace Rpm.Core.Events
{
    /// <summary>
    /// Static holder for cross-module accessibility toggles. Lives in
    /// <c>Rpm.Core</c> so juice, gameplay, and input layers can all honour
    /// the flag without depending on one another.
    /// </summary>
    /// <remarks>
    /// <para>
    /// The canonical Sprint 1 flag is <see cref="ReduceMotion"/>. When set,
    /// downstream systems (screen shake, haptic impact, camera wobble)
    /// suppress their kinetic output — per DESIGN-001 accessibility
    /// baseline. Dust particles and audio cues still play because they do
    /// not trigger vestibular discomfort.
    /// </para>
    /// <para>
    /// Persistence is via <see cref="PlayerPrefs"/> under a single key.
    /// This is a deliberate Sprint 1 shim; a proper <c>ISavable</c> binding
    /// lands alongside the save service in a later sprint. The shim is
    /// isolated here so future migration is a single-file swap.
    /// </para>
    /// <para>
    /// The holder is static because the flag is read on every impact tick
    /// and on every haptic pulse; any per-read allocation or DI resolve
    /// would show up in the Profiler. Mutation goes through
    /// <see cref="SetReduceMotion"/> so we get a single write site that
    /// persists the change.
    /// </para>
    /// </remarks>
    public static class AccessibilityFlags
    {
        private const string ReduceMotionKey = "rpm.accessibility.reduceMotion";

        private static bool _reduceMotion;
        private static bool _loaded;

        /// <summary>
        /// <c>true</c> when the player has opted into reduced-motion
        /// output. Read on the impact hot path — keep this a single-field
        /// lookup.
        /// </summary>
        public static bool ReduceMotion
        {
            get
            {
                if (!_loaded) Load();
                return _reduceMotion;
            }
        }

        /// <summary>
        /// Updates the flag and persists it to <see cref="PlayerPrefs"/>.
        /// Intended for the settings UI; do not call from gameplay code.
        /// </summary>
        /// <param name="value">New value for <see cref="ReduceMotion"/>.</param>
        public static void SetReduceMotion(bool value)
        {
            _reduceMotion = value;
            _loaded = true;
            PlayerPrefs.SetInt(ReduceMotionKey, value ? 1 : 0);
            PlayerPrefs.Save();
        }

        /// <summary>
        /// Test-only hook to force a value without touching
        /// <see cref="PlayerPrefs"/>. Kept <c>internal</c> so only
        /// <c>Rpm.Core</c> assemblies (and IVT friends) can reach it.
        /// </summary>
        internal static void SetReduceMotionForTest(bool value)
        {
            _reduceMotion = value;
            _loaded = true;
        }

        private static void Load()
        {
            _reduceMotion = PlayerPrefs.GetInt(ReduceMotionKey, 0) == 1;
            _loaded = true;
        }
    }
}
