﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class SpellCardTests
    {
        internal struct Properties
        {
            public byte[] name;
            public int clearCount;
            public int trialCount;
            public int id;
            public Level level;
        };

        internal static Properties ValidProperties => new Properties()
        {
            name = TestUtils.MakeRandomArray<byte>(0x80),
            clearCount = 123,
            trialCount = 456,
            id = 789,
            level = Level.Normal
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.name,
                properties.clearCount,
                properties.trialCount,
                properties.id - 1,
                (int)properties.level);

        internal static void Validate(in SpellCard spellCard, in Properties properties)
        {
            CollectionAssert.That.AreEqual(properties.name, spellCard.Name);
            Assert.AreEqual(properties.clearCount, spellCard.ClearCount);
            Assert.AreEqual(properties.trialCount, spellCard.TrialCount);
            Assert.AreEqual(properties.id, spellCard.Id);
            Assert.AreEqual(properties.level, spellCard.Level);
        }

        [TestMethod]
        public void SpellCardTest()
            => TestUtils.Wrap(() =>
            {
                var properties = new Properties();
                var spellCard = new SpellCard();

                Validate(spellCard, properties);
                Assert.IsFalse(spellCard.HasTried);
            });

        [TestMethod]
        public void ReadFromTest()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;

                var spellCard = TestUtils.Create<SpellCard>(MakeByteArray(properties));

                Validate(spellCard, properties);
                Assert.IsTrue(spellCard.HasTried);
            });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
            => TestUtils.Wrap(() =>
            {
                var spellCard = new SpellCard();
                spellCard.ReadFrom(null);

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedName()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.name = properties.name.Take(properties.name.Length - 1).ToArray();

                var spellCard = TestUtils.Create<SpellCard>(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        [TestMethod]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestExceededName()
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.name = properties.name.Concat(TestUtils.MakeRandomArray<byte>(1)).ToArray();

                var spellCard = TestUtils.Create<SpellCard>(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });

        public static IEnumerable<object[]> InvalidLevels
            => TestUtils.GetInvalidEnumerators(typeof(Level));

        [SuppressMessage("Microsoft.Performance", "CA1804:RemoveUnusedLocals", MessageId = "spellCard")]
        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidLevels))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ReadFromTestInvalidLevel(int level)
            => TestUtils.Wrap(() =>
            {
                var properties = ValidProperties;
                properties.level = TestUtils.Cast<Level>(level);

                var spellCard = TestUtils.Create<SpellCard>(MakeByteArray(properties));

                Assert.Fail(TestUtils.Unreachable);
            });
    }
}
