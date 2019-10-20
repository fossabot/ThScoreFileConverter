﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th06.Wrappers;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class ChapterTests
    {
        internal struct Properties
        {
            public string signature;
            public short size1;
            public short size2;
            public byte[] data;
        };

        internal static Properties DefaultProperties { get; } = new Properties()
        {
            signature = string.Empty,
            size1 = default,
            size2 = default,
            data = new byte[] { }
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "ABCD",
            size1 = 12,
            size2 = 34,
            data = new byte[] { 0x56, 0x78, 0x9A, 0xBC }
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.size1,
                properties.size2,
                properties.data);

        internal static void Validate(in Properties expected, in ChapterWrapper actual)
        {
            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.size1, actual.Size1);
            Assert.AreEqual(expected.size2, actual.Size2);
            CollectionAssert.That.AreEqual(expected.data, actual.Data);
            Assert.AreEqual((expected.data?.Length > 0 ? expected.data[0] : default), actual.FirstByteOfData);
        }

        [TestMethod]
        public void ChapterTest() => TestUtils.Wrap(() =>
        {
            var chapter = new ChapterWrapper();

            Validate(DefaultProperties, chapter);
        });

        [TestMethod]
        public void ChapterTestCopy() => TestUtils.Wrap(() =>
        {
            var chapter1 = new ChapterWrapper();
            var chapter2 = new ChapterWrapper(chapter1);

            Validate(DefaultProperties, chapter2);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChapterTestNull() => TestUtils.Wrap(() =>
        {
            _ = new ChapterWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(MakeByteArray(ValidProperties));

            Validate(ValidProperties, chapter);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var chapter = new ChapterWrapper();
            chapter.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            // <-- sig --> size1 size2 <- data -->
            // __ __ __ __ 0c 00 22 00 56 78 9a bc
            //             <-- sig --> size1 size2 <dat>

            // The actual value of the Size1 property becomes too large and
            // the Data property becomes empty,
            // so EndOfStreamException will be thrown.
            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            // <-- sig --> size1 size2 <- data -->
            // __ 41 42 43 0c 00 22 00 56 78 9a bc
            //    <-- sig --> size1 size2 < dat ->

            // The actual value of the Size1 property becomes too large,
            // so EndOfStreamException will be thrown.
            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "E";

            // <--- sig ----> size1 size2 <- data -->
            // 41 42 43 44 45 0c 00 22 00 56 78 9a bc
            // <-- sig --> size1 size2 <---- data ---->

            // The actual value of the Size1 property becomes too large,
            // so EndOfStreamException will be thrown.
            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFromTestNegativeSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size1 = -1;

            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFromTestZeroSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size1 = 0;

            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestShortenedSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Assert.AreEqual(properties.signature, chapter.Signature);
            Assert.AreEqual(properties.size1, chapter.Size1);
            Assert.AreEqual(properties.size2, chapter.Size2);
            CollectionAssert.That.AreNotEqual(properties.data, chapter.Data);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestExceededSize1() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size1;

            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestNegativeSize2() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size2 = -1;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Validate(properties, chapter);
        });

        [TestMethod]
        public void ReadFromTestZeroSize2() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size2 = 0;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Validate(properties, chapter);
        });

        [TestMethod]
        public void ReadFromTestShortenedSize2() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size2;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Validate(properties, chapter);
        });

        [TestMethod]
        public void ReadFromTestExceededSize2() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size2;

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Validate(properties, chapter);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmptyData() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.data = new byte[] { };

            ChapterWrapper.Create(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestMisalignedData() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size1;
            properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

            var chapter = ChapterWrapper.Create(MakeByteArray(properties));

            Validate(properties, chapter);
        });
    }
}
