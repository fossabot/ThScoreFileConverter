﻿using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Squirrel;
using ThScoreFileConverterTests.Models;

namespace ThScoreFileConverterTests.Squirrel
{
    [TestClass]
    public class SQIntegerTests
    {
        [TestMethod]
        public void SQIntegerTest()
        {
            var sqinteger = new SQInteger();

            Assert.AreEqual(SQObjectType.Integer, sqinteger.Type);
            Assert.AreEqual(0, sqinteger.Value);
            Assert.AreEqual(0, sqinteger);
        }

        internal static SQInteger CreateTestHelper(byte[] bytes)
        {
            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(bytes);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;

                    return SQInteger.Create(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }
        }

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(0)]
        [DataRow(1)]
        [DataRow(-1)]
        [DataRow(int.MaxValue)]
        [DataRow(int.MinValue)]
        public void CreateTest(int expected)
        {
            var sqinteger = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Integer, expected));

            Assert.AreEqual(SQObjectType.Integer, sqinteger.Type);
            Assert.AreEqual(expected, sqinteger.Value);
            Assert.AreEqual(expected, sqinteger);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CreateTestNull()
        {
            _ = SQInteger.Create(null);

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void CreateTestShortened()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Integer, new byte[3]));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidDataException))]
        public void CreateTestInvalid()
        {
            _ = CreateTestHelper(TestUtils.MakeByteArray((int)SQObjectType.Null, 123));

            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void EqualsTestNull() => Assert.IsFalse(new SQInteger().Equals(null));

        [TestMethod]
        public void EqualsTestNullObject() => Assert.IsFalse(new SQInteger().Equals(null as object));

        [TestMethod]
        public void EqualsTestInvalidType() => Assert.IsFalse(new SQInteger().Equals(SQNull.Instance));

        [TestMethod]
        public void EqualsTestSelf()
        {
            var value = new SQInteger();

            Assert.IsTrue(value.Equals(value));
        }

        [TestMethod]
        public void EqualsTestSelfObject()
        {
            var value = new SQInteger();

            Assert.IsTrue(value.Equals(value as object));
        }

        [TestMethod]
        public void EqualsTestEqual() => Assert.IsTrue(new SQInteger().Equals(new SQInteger(0)));

        [TestMethod]
        public void EqualsTestNotEqual() => Assert.IsFalse(new SQInteger().Equals(new SQInteger(1)));

        [TestMethod]
        public void GetHashCodeTestEqual()
            => Assert.AreEqual(new SQInteger().GetHashCode(), new SQInteger(0).GetHashCode());

        [TestMethod]
        public void GetHashCodeTestNotEqual()
            => Assert.AreNotEqual(new SQInteger().GetHashCode(), new SQInteger(1).GetHashCode());
    }
}
