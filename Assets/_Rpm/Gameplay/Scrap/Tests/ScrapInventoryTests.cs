#nullable enable

using NUnit.Framework;

namespace Rpm.Gameplay.Scrap.Tests
{
    /// <summary>
    /// EditMode coverage for <see cref="ScrapInventory"/>: starts at
    /// <see cref="ScrapInventory.StartingCount"/>, decrements on use,
    /// refuses when empty.
    /// </summary>
    [TestFixture]
    public sealed class ScrapInventoryTests
    {
        [Test]
        public void Starts_At_StartingCount()
        {
            var inv = new ScrapInventory();
            Assert.AreEqual(ScrapInventory.StartingCount, inv.Count);
        }

        [Test]
        public void TryUse_Decrements_Count()
        {
            var inv = new ScrapInventory();
            Assert.IsTrue(inv.TryUse());
            Assert.AreEqual(ScrapInventory.StartingCount - 1, inv.Count);
        }

        [Test]
        public void TryUse_On_Empty_Inventory_Returns_False()
        {
            var inv = new ScrapInventory();
            for (var i = 0; i < ScrapInventory.StartingCount; i++)
            {
                Assert.IsTrue(inv.TryUse(), $"Use #{i + 1} should succeed.");
            }
            Assert.AreEqual(0, inv.Count);
            Assert.IsFalse(inv.TryUse(), "TryUse on empty inventory must return false.");
            Assert.AreEqual(0, inv.Count, "Count must not go negative.");
        }
    }
}
