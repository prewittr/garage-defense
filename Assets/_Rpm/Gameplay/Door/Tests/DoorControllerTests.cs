#nullable enable

using NUnit.Framework;
using Rpm.Core.Door;
using Rpm.Core.Events;
using UnityEngine;

namespace Rpm.Gameplay.Door.Tests
{
    /// <summary>
    /// EditMode coverage for <see cref="DoorController"/>: HP math
    /// (damage, repair, clamps) and event emission through
    /// <see cref="IEventBus"/>.
    /// </summary>
    [TestFixture]
    public sealed class DoorControllerTests
    {
        private GameObject? _go;
        private DoorController? _controller;
        private EventBus? _bus;

        private int _damageCount;
        private int _repairCount;
        private DamageEvent _lastDamage;
        private RepairEvent _lastRepair;

        [SetUp]
        public void SetUp()
        {
            _go = new GameObject("DoorControllerTestsRig");
            _controller = _go.AddComponent<DoorController>();
            _bus = new EventBus();
            _controller.InitForTest(_bus, maxHp: 100f, startingHp: 100f);
            _damageCount = 0;
            _repairCount = 0;
            _bus.Subscribe<DamageEvent>(e => { _damageCount++; _lastDamage = e; });
            _bus.Subscribe<RepairEvent>(e => { _repairCount++; _lastRepair = e; });
        }

        [TearDown]
        public void TearDown()
        {
            if (_go != null) Object.DestroyImmediate(_go);
            _go = null;
            _controller = null;
            _bus = null;
        }

        [Test]
        public void ApplyDamage_Reduces_Current_HP()
        {
            _controller!.ApplyDamage(0.05f, new Vector2(0.1f, 0.2f));
            Assert.AreEqual(95f, _controller.HP.Current, 0.001f);
            Assert.AreEqual(0.95f, _controller.HP.Ratio, 0.001f);
        }

        [Test]
        public void ApplyRepair_Increases_Current_HP()
        {
            _controller!.ApplyDamage(0.30f, Vector2.zero);
            var ok = _controller.ApplyRepair(0.15f, Vector2.zero);
            Assert.IsTrue(ok);
            Assert.AreEqual(85f, _controller.HP.Current, 0.001f);
        }

        [Test]
        public void ApplyRepair_Is_Capped_At_Max()
        {
            _controller!.ApplyDamage(0.05f, Vector2.zero);
            _controller.ApplyRepair(0.50f, Vector2.zero);
            Assert.AreEqual(100f, _controller.HP.Current, 0.001f);
            Assert.AreEqual(1f, _controller.HP.Ratio, 0.001f);
        }

        [Test]
        public void ApplyRepair_On_Full_HP_Returns_False_And_Emits_Nothing()
        {
            var ok = _controller!.ApplyRepair(0.15f, Vector2.zero);
            Assert.IsFalse(ok);
            Assert.AreEqual(0, _repairCount, "No RepairEvent should fire when the door is already full.");
        }

        [Test]
        public void ApplyDamage_Never_Goes_Negative()
        {
            _controller!.ApplyDamage(2.0f, Vector2.zero);
            Assert.AreEqual(0f, _controller.HP.Current, 0.001f);
            Assert.AreEqual(0f, _controller.HP.Ratio, 0.001f);
        }

        [Test]
        public void ApplyDamage_Emits_DamageEvent_With_Same_Amount_And_Coord()
        {
            var coord = new Vector2(0.3f, -0.2f);
            _controller!.ApplyDamage(0.05f, coord);
            Assert.AreEqual(1, _damageCount);
            Assert.AreEqual(0.05f, _lastDamage.Amount, 0.001f);
            Assert.AreEqual(coord, _lastDamage.Coord);
        }

        [Test]
        public void ApplyRepair_Emits_RepairEvent_On_Success()
        {
            _controller!.ApplyDamage(0.2f, Vector2.zero);
            var coord = new Vector2(-0.1f, 0.1f);
            _controller.ApplyRepair(0.15f, coord);
            Assert.AreEqual(1, _repairCount);
            Assert.AreEqual(0.15f, _lastRepair.Amount, 0.001f);
            Assert.AreEqual(coord, _lastRepair.Coord);
        }
    }
}
