﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Extensions;
using ThScoreFileConverterTests.Models.Th10.Wrappers;
using IChapter = ThScoreFileConverter.Models.Th095.IChapter;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ChapterTests
    {
        internal struct Properties
        {
            public string signature;
            public ushort version;
            public uint checksum;
            public int size;
            public byte[] data;
        };

        internal static Properties DefaultProperties { get; } = new Properties()
        {
            signature = string.Empty,
            version = default,
            checksum = default,
            size = default,
            data = new byte[] { }
        };

        internal static Properties ValidProperties { get; } = new Properties()
        {
            signature = "AB",
            version = 1234,
            checksum = 0x234u,
            size = 16,
            data = new byte[] { 0x56, 0x78, 0x9A, 0xBC }
        };

        internal static byte[] MakeByteArray(in Properties properties)
            => TestUtils.MakeByteArray(
                properties.signature.ToCharArray(),
                properties.version,
                properties.checksum,
                properties.size,
                properties.data);

        internal static void Validate(in Properties expected, IChapter actual)
        {
            Assert.AreEqual(expected.signature, actual.Signature);
            Assert.AreEqual(expected.version, actual.Version);
            Assert.AreEqual(expected.checksum, actual.Checksum);
            Assert.AreEqual(expected.size, actual.Size);
        }

        internal static void Validate(in Properties expected, in ChapterWrapper actual)
        {
            Validate(expected, actual as IChapter);
            CollectionAssert.That.AreEqual(expected.data, actual.Data);
        }

        [TestMethod]
        public void ChapterTest() => TestUtils.Wrap(() =>
        {
            var chapter = new ChapterWrapper();

            Validate(DefaultProperties, chapter);
            Assert.IsTrue(chapter.IsValid);
        });

        [TestMethod]
        public void ChapterTestCopy() => TestUtils.Wrap(() =>
        {
            var chapter1 = new Chapter();
            var chapter2 = new ChapterWrapper(chapter1);

            Validate(DefaultProperties, chapter2);
            Assert.IsTrue(chapter2.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChapterTestNull() => TestUtils.Wrap(() =>
        {
            _ = new ChapterWrapper(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ChapterTestCopyWithExpected()
        {
            var chapter1 = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            var chapter2 = new ChapterWrapper(chapter1, chapter1.Signature, chapter1.Version, chapter1.Size);

            Validate(ValidProperties, chapter2);
            Assert.IsTrue(chapter2.IsValid);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ChapterTestNullWithExpected()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            _ = new ChapterWrapper(null, chapter.Signature, chapter.Version, chapter.Size);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ChapterTestInvalidSignature()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            _ = new ChapterWrapper(chapter, chapter.Signature.ToLowerInvariant(), chapter.Version, chapter.Size);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ChapterTestInvalidVersion()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            _ = new ChapterWrapper(chapter, chapter.Signature, (ushort)(chapter.Version - 1), chapter.Size);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void ChapterTestInvalidSize()
        {
            var chapter = TestUtils.Create<Chapter>(MakeByteArray(ValidProperties));
            _ = new ChapterWrapper(chapter, chapter.Signature, chapter.Version, chapter.Size - 1);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(ValidProperties));

            Validate(ValidProperties, chapter);
            Assert.IsTrue(chapter.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var chapter = new Chapter();
            chapter.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmptySignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = string.Empty;

            // <sig> <ver> < chksum -> <- size --> <- data -->
            // __ __ d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
            //       <sig> <ver> < chksum -> <- size --> <dat>

            // The actual value of the Size property becomes too large,
            // so EndOfStreamException will be thrown.
            _ = TestUtils.Create<Chapter>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortenedSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature = properties.signature.Substring(0, properties.signature.Length - 1);

            // <sig> <ver> < chksum -> <- size --> <- data -->
            // __ 41 d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
            //    <sig> <ver> < chksum -> <- size --> < data >

            // The actual value of the Size property becomes too large,
            // so EndOfStreamException will be thrown.
            _ = TestUtils.Create<Chapter>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestExceededSignature() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.signature += "C";

            // < sig -> <ver> < chksum -> <- size --> <- data -->
            // 41 42 43 d2 04 34 02 00 00 10 00 00 00 56 78 9a bc
            // <sig> <ver> < chksum -> <- size --> <---- data ---->

            // The actual value of the Size property becomes large,
            // so EndOfStreamException will be thrown.
            _ = TestUtils.Create<Chapter>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFromTestNegativeSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size = -1;

            _ = TestUtils.Create<Chapter>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentOutOfRangeException))]
        public void ReadFromTestZeroSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.size = 0;

            _ = TestUtils.Create<Chapter>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });


        [TestMethod]
        public void ReadFromTestShortenedSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;

            var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

            Validate(properties, chapter as IChapter);
            CollectionAssert.That.AreNotEqual(properties.data, chapter.Data);
            Assert.IsFalse(chapter.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestExceededSize() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            ++properties.size;

            _ = TestUtils.Create<Chapter>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestInvalidChecksum() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.checksum;

            var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

            Validate(properties, chapter);
            Assert.IsFalse(chapter.IsValid);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmptyData() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            properties.data = new byte[] { };

            _ = TestUtils.Create<Chapter>(MakeByteArray(properties));

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestMisalignedData() => TestUtils.Wrap(() =>
        {
            var properties = ValidProperties;
            --properties.size;
            properties.data = properties.data.Take(properties.data.Length - 1).ToArray();

            var chapter = TestUtils.Create<ChapterWrapper>(MakeByteArray(properties));

            Validate(properties, chapter);
            Assert.IsFalse(chapter.IsValid);
        });
    }
}
