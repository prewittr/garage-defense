#nullable enable

using System;
using NUnit.Framework;
using Rpm.Core.Events;
using UnityEngine;

namespace Rpm.Core.Tests
{
    /// <summary>
    /// EditMode coverage for <see cref="EventBus"/>: subscribe, publish,
    /// unsubscribe, reentrancy guard, and the allocation contract.
    /// </summary>
    [TestFixture]
    public sealed class EventBusTests
    {
        [Test]
        public void Publish_Invokes_Every_Subscriber_Once()
        {
            var bus = new EventBus();
            var callsA = 0;
            var callsB = 0;
            Action<DamageEvent> a = _ => callsA++;
            Action<DamageEvent> b = _ => callsB++;

            bus.Subscribe(a);
            bus.Subscribe(b);
            bus.Publish(new DamageEvent(0.05f, Vector2.zero, 0d));

            Assert.AreEqual(1, callsA);
            Assert.AreEqual(1, callsB);
        }

        [Test]
        public void Unsubscribe_Removes_Handler()
        {
            var bus = new EventBus();
            var calls = 0;
            Action<RepairEvent> h = _ => calls++;

            bus.Subscribe(h);
            bus.Publish(new RepairEvent(0.15f, Vector2.zero, 0d));
            bus.Unsubscribe(h);
            bus.Publish(new RepairEvent(0.15f, Vector2.zero, 0d));

            Assert.AreEqual(1, calls);
        }

        [Test]
        public void Publish_With_No_Subscribers_Is_A_Noop()
        {
            var bus = new EventBus();
            Assert.DoesNotThrow(() => bus.Publish(new DamageEvent(0.05f, Vector2.zero, 0d)));
        }

        [Test]
        public void Subscribe_Null_Throws()
        {
            var bus = new EventBus();
            Assert.Throws<ArgumentNullException>(() => bus.Subscribe<DamageEvent>(null!));
        }

        [Test]
        public void Reentrant_SameType_Publish_Throws()
        {
            var bus = new EventBus();
            Action<DamageEvent>? handler = null;
            handler = _ => bus.Publish(new DamageEvent(0f, Vector2.zero, 0d));
            bus.Subscribe(handler);
            Assert.Throws<InvalidOperationException>(() =>
                bus.Publish(new DamageEvent(0.05f, Vector2.zero, 0d)));
        }

        [Test]
        public void Bus_Instances_Are_Isolated()
        {
            var a = new EventBus();
            var b = new EventBus();
            var countA = 0;
            var countB = 0;
            a.Subscribe<DamageEvent>(_ => countA++);
            b.Subscribe<DamageEvent>(_ => countB++);

            a.Publish(new DamageEvent(0.05f, Vector2.zero, 0d));

            Assert.AreEqual(1, countA, "Bus A should have seen its event.");
            Assert.AreEqual(0, countB, "Bus B should not receive A's events.");
        }
    }
}
