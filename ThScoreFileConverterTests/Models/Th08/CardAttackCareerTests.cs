﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th08;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th08.Stubs;

namespace ThScoreFileConverterTests.Models.Th08
{
    [TestClass]
    public class CardAttackCareerTests
    {
        internal static CardAttackCareerStub ValidStub { get; } = new CardAttackCareerStub()
        {
            MaxBonuses = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => (chara, index))
                .ToDictionary(pair => pair.chara, pair => (uint)pair.index),
            TrialCounts = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => (chara, index))
                .ToDictionary(pair => pair.chara, pair => 20 + pair.index),
            ClearCounts = Utils.GetEnumerator<CharaWithTotal>()
                .Select((chara, index) => (chara, index))
                .ToDictionary(pair => pair.chara, pair => 20 - pair.index)
        };

        internal static byte[] MakeByteArray(ICardAttackCareer career)
            => TestUtils.MakeByteArray(
                career.MaxBonuses.Values.ToArray(),
                career.TrialCounts.Values.ToArray(),
                career.ClearCounts.Values.ToArray());

        internal static void Validate(ICardAttackCareer expected, ICardAttackCareer actual)
        {
            CollectionAssert.That.AreEqual(expected.MaxBonuses.Values, actual.MaxBonuses.Values);
            CollectionAssert.That.AreEqual(expected.TrialCounts.Values, actual.TrialCounts.Values);
            CollectionAssert.That.AreEqual(expected.ClearCounts.Values, actual.ClearCounts.Values);
        }

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var stub = ValidStub;

            var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(stub));

            Validate(stub, career);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var career = new CardAttackCareer();
            career.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedMaxBonuses() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.MaxBonuses = stub.MaxBonuses.Where(pair => pair.Key != CharaWithTotal.Total)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = TestUtils.Create<CardAttackCareer>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededMaxBonuses() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.MaxBonuses = stub.MaxBonuses.Concat(new Dictionary<CharaWithTotal, uint>
            {
                { TestUtils.Cast<CharaWithTotal>(999), 999u },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(stub));

            CollectionAssert.That.AreNotEqual(stub.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreEqual(stub.MaxBonuses.Values.SkipLast(1), career.MaxBonuses.Values);
            CollectionAssert.That.AreNotEqual(stub.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(stub.ClearCounts.Values, career.ClearCounts.Values);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedTrialCounts() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.TrialCounts = stub.TrialCounts.Where(pair => pair.Key != CharaWithTotal.Total)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = TestUtils.Create<CardAttackCareer>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededTrialCounts() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.TrialCounts = stub.TrialCounts.Concat(new Dictionary<CharaWithTotal, int>
            {
                { TestUtils.Cast<CharaWithTotal>(999), 999 },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(stub));

            CollectionAssert.That.AreEqual(stub.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreNotEqual(stub.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreEqual(stub.TrialCounts.Values.SkipLast(1), career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(stub.ClearCounts.Values, career.ClearCounts.Values);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedClearCounts() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.ClearCounts = stub.ClearCounts.Where(pair => pair.Key != CharaWithTotal.Total)
                .ToDictionary(pair => pair.Key, pair => pair.Value);

            _ = TestUtils.Create<CardAttackCareer>(MakeByteArray(stub));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestExceededClearCounts() => TestUtils.Wrap(() =>
        {
            var stub = new CardAttackCareerStub(ValidStub);
            stub.ClearCounts = stub.ClearCounts.Concat(new Dictionary<CharaWithTotal, int>
            {
                { TestUtils.Cast<CharaWithTotal>(999), 999 },
            }).ToDictionary(pair => pair.Key, pair => pair.Value);

            var career = TestUtils.Create<CardAttackCareer>(MakeByteArray(stub));

            CollectionAssert.That.AreEqual(stub.MaxBonuses.Values, career.MaxBonuses.Values);
            CollectionAssert.That.AreEqual(stub.TrialCounts.Values, career.TrialCounts.Values);
            CollectionAssert.That.AreNotEqual(stub.ClearCounts.Values, career.ClearCounts.Values);
            CollectionAssert.That.AreEqual(stub.ClearCounts.Values.SkipLast(1), career.ClearCounts.Values);
        });
    }
}
