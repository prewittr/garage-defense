#nullable enable

using NUnit.Framework;
using UnityEngine;

namespace Rpm.Gameplay.Scrap.Tests
{
    /// <summary>
    /// EditMode coverage for <see cref="DamagePointRegistry"/>: nearest
    /// lookup correctness, empty-registry behaviour, unregister semantics,
    /// and grow-beyond-initial-capacity.
    /// </summary>
    [TestFixture]
    public sealed class DamagePointRegistryTests
    {
        [Test]
        public void Empty_Registry_Returns_False_From_NearestTo()
        {
            var reg = new DamagePointRegistry();
            Assert.IsFalse(reg.NearestTo(Vector2.zero, out var found));
            Assert.AreEqual(Vector2.zero, found);
        }

        [Test]
        public void NearestTo_Returns_Closest_Registered_Point()
        {
            var reg = new DamagePointRegistry();
            reg.RegisterPoint(new Vector2(1f, 0f));
            reg.RegisterPoint(new Vector2(0f, 1f));
            reg.RegisterPoint(new Vector2(-1f, -1f));

            Assert.IsTrue(reg.NearestTo(new Vector2(0.9f, 0f), out var near));
            Assert.AreEqual(new Vector2(1f, 0f), near);

            Assert.IsTrue(reg.NearestTo(new Vector2(-0.9f, -0.9f), out near));
            Assert.AreEqual(new Vector2(-1f, -1f), near);
        }

        [Test]
        public void UnregisterPoint_Removes_And_Shifts_Internals()
        {
            var reg = new DamagePointRegistry();
            reg.RegisterPoint(new Vector2(1f, 0f));
            reg.RegisterPoint(new Vector2(0f, 1f));
            Assert.IsTrue(reg.UnregisterPoint(new Vector2(1f, 0f)));
            Assert.AreEqual(1, reg.Count);
            Assert.IsTrue(reg.NearestTo(Vector2.zero, out var found));
            Assert.AreEqual(new Vector2(0f, 1f), found);
        }

        [Test]
        public void UnregisterPoint_Unknown_Coord_Returns_False()
        {
            var reg = new DamagePointRegistry();
            reg.RegisterPoint(Vector2.one);
            Assert.IsFalse(reg.UnregisterPoint(new Vector2(42f, 42f)));
            Assert.AreEqual(1, reg.Count);
        }

        [Test]
        public void Registry_Grows_Beyond_Initial_Capacity()
        {
            var reg = new DamagePointRegistry();
            for (var i = 0; i < 32; i++)
            {
                reg.RegisterPoint(new Vector2(i, i));
            }
            Assert.AreEqual(32, reg.Count);
            Assert.IsTrue(reg.NearestTo(new Vector2(31.1f, 31.1f), out var near));
            Assert.AreEqual(new Vector2(31f, 31f), near);
        }

        [Test]
        public void Clear_Empties_The_Registry()
        {
            var reg = new DamagePointRegistry();
            reg.RegisterPoint(Vector2.one);
            reg.RegisterPoint(Vector2.up);
            reg.Clear();
            Assert.AreEqual(0, reg.Count);
            Assert.IsFalse(reg.NearestTo(Vector2.zero, out _));
        }
    }
}
