using System;
using IdleGame.Core.Identifiers;
using NUnit.Framework;

namespace IdleGame.Tests.EditMode
{
    public sealed class StableIdTests
    {
        [TestCase("woodcutting")]
        [TestCase("combat")]
        [TestCase("inventory_2")]
        public void IsValidAcceptsLowerSnakeCase(string value)
        {
            Assert.IsTrue(StableId.IsValid(value));
        }

        [TestCase("")]
        [TestCase("Woodcutting")]
        [TestCase("1_inventory")]
        [TestCase("inventory-screen")]
        [TestCase("inventory screen")]
        public void IsValidRejectsUnstableIds(string value)
        {
            Assert.IsFalse(StableId.IsValid(value));
        }

        [Test]
        public void ThrowIfInvalidReportsParameter()
        {
            var exception = Assert.Throws<ArgumentException>(() => StableId.ThrowIfInvalid("Combat", "screenId"));
            Assert.AreEqual("screenId", exception.ParamName);
        }
    }
}
