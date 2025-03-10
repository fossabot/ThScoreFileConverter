﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class EnumShortNameParserTests
    {
        [TestMethod]
        public void PatternTest()
        {
            var parser = new EnumShortNameParser<Level>();
            Assert.AreEqual("E|N|H|L|X", parser.Pattern);
        }

        [TestMethod]
        public void PatternTestNoShortName()
        {
            var parser = new EnumShortNameParser<DayOfWeek>();
            Assert.AreEqual(string.Empty, parser.Pattern);
        }

        [TestMethod]
        public void ParseTest()
        {
            var parser = new EnumShortNameParser<Level>();
            Assert.AreEqual(Level.Easy, parser.Parse("E"));
            Assert.AreEqual(Level.Normal, parser.Parse("N"));
            Assert.AreEqual(Level.Hard, parser.Parse("H"));
            Assert.AreEqual(Level.Lunatic, parser.Parse("L"));
            Assert.AreEqual(Level.Extra, parser.Parse("X"));
        }

        [TestMethod]
        public void ParseTestMismatchedCase()
        {
            var parser = new EnumShortNameParser<Level>();
            Assert.AreEqual(Level.Easy, parser.Parse("e"));
            Assert.AreEqual(Level.Normal, parser.Parse("n"));
            Assert.AreEqual(Level.Hard, parser.Parse("h"));
            Assert.AreEqual(Level.Lunatic, parser.Parse("l"));
            Assert.AreEqual(Level.Extra, parser.Parse("x"));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseTestEmpty()
        {
            var parser = new EnumShortNameParser<Level>();
            parser.Parse(string.Empty);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void ParseTestUnknown()
        {
            var parser = new EnumShortNameParser<Level>();
            parser.Parse("A");
            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
