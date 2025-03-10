﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Extensions;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using ClearDataStub = ThScoreFileConverterTests.Models.Th06.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th07.Chara, ThScoreFileConverter.Models.Th07.Level>;
using IClearData = ThScoreFileConverter.Models.Th06.IClearData<
    ThScoreFileConverter.Models.Th07.Chara, ThScoreFileConverter.Models.Th07.Level>;
using Level = ThScoreFileConverter.Models.Th07.Level;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class ClearDataTests
    {
        internal static ClearDataStub ValidStub { get; } = new ClearDataStub()
        {
            Signature = "CLRD",
            Size1 = 0x1C,
            Size2 = 0x1C,
            StoryFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (byte)pair.index),
            PracticeFlags = Utils.GetEnumerator<Level>()
                .Select((level, index) => new { level, index })
                .ToDictionary(pair => pair.level, pair => (byte)(10 - pair.index)),
            Chara = Chara.ReimuB
        };

        internal static byte[] MakeByteArray(IClearData clearData)
            => TestUtils.MakeByteArray(
                clearData.Signature.ToCharArray(),
                clearData.Size1,
                clearData.Size2,
                0u,
                clearData.StoryFlags.Values.ToArray(),
                clearData.PracticeFlags.Values.ToArray(),
                (int)clearData.Chara);

        internal static void Validate(IClearData expected, IClearData actual)
        {
            Assert.AreEqual(expected.Signature, actual.Signature);
            Assert.AreEqual(expected.Size1, actual.Size1);
            Assert.AreEqual(expected.Size2, actual.Size2);
            Assert.AreEqual(expected.FirstByteOfData, actual.FirstByteOfData);
            CollectionAssert.That.AreEqual(expected.StoryFlags.Values, actual.StoryFlags.Values);
            CollectionAssert.That.AreEqual(expected.PracticeFlags.Values, actual.PracticeFlags.Values);
            Assert.AreEqual(expected.Chara, actual.Chara);
        }

        [TestMethod]
        public void ClearDataTestChapter() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidStub));
            var clearData = new ClearData(chapter);

            Validate(ValidStub, clearData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ClearDataTestNullChapter() => TestUtils.Wrap(() =>
        {
            _ = new ClearData(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [SuppressMessage("Microsoft.Globalization", "CA1308:NormalizeStringsToUppercase")]
        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSignature() => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub);
            stub.Signature = stub.Signature.ToLowerInvariant();

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ClearDataTestInvalidSize1() => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub);
            --stub.Size1;

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });

        public static IEnumerable<object[]> InvalidCharacters
            => TestUtils.GetInvalidEnumerators(typeof(Chara));

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DynamicData(nameof(InvalidCharacters))]
        [ExpectedException(typeof(InvalidCastException))]
        public void ClearDataTestInvalidChara(int chara) => TestUtils.Wrap(() =>
        {
            var stub = new ClearDataStub(ValidStub)
            {
                Chara = TestUtils.Cast<Chara>(chara),
            };

            var chapter = TestUtils.Create<Chapter>(MakeByteArray(stub));
            _ = new ClearData(chapter);

            Assert.Fail(TestUtils.Unreachable);
        });
    }
}
