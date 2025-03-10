﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143
{
    [TestClass]
    public class ShotExReplacerTests
    {
        internal static IReadOnlyDictionary<(Day, int), (string, IBestShotHeader)> BestShots { get; } =
            new List<(string, IBestShotHeader header)>
            {
                (@"C:\path\to\output\bestshots\sc02_03.png", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));

        [TestMethod]
        public void ShotExReplacerTest()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ShotExReplacerTestNullBestShots()
        {
            _ = new ShotExReplacer(null, @"C:\path\to\output\");
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Day, int), (string, IBestShotHeader)>();
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidBestShotPath()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestNullOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, null);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestEmptyOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, string.Empty);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ShotExReplacerTestInvalidOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, "abcde");
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestPath()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(@"bestshots/sc02_03.png", replacer.Replace("%T143SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestWidth()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("4", replacer.Replace("%T143SHOTEX232"));
        }

        [TestMethod]
        public void ReplaceTestHeight()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("5", replacer.Replace("%T143SHOTEX233"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            var expected = new DateTime(1970, 1, 1).AddSeconds(6).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T143SHOTEX234"));
        }

        [TestMethod]
        public void ReplaceTestEmptyBestShots()
        {
            var bestshots = new Dictionary<(Day, int), (string, IBestShotHeader)>();
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOTEX231"));
            Assert.AreEqual("0", replacer.Replace("%T143SHOTEX232"));
            Assert.AreEqual("0", replacer.Replace("%T143SHOTEX233"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T143SHOTEX234"));
        }

        [TestMethod]
        public void ReplaceTestInvalidBestShotPaths()
        {
            var bestshots = new List<(string, IBestShotHeader header)>
            {
                ("abcde", BestShotHeaderTests.ValidStub),
            }.ToDictionary(element => (element.header.Day, (int)element.header.Scene));
            var replacer = new ShotExReplacer(bestshots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestNullOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, null);
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestEmptyOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, string.Empty);
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidOutputFilePath()
        {
            var replacer = new ShotExReplacer(BestShots, "abcde");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOTEX231"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentDay()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOTEX131"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentScene()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual(string.Empty, replacer.Replace("%T143SHOTEX221"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentSpellCard()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143SHOTEX171", replacer.Replace("%T143SHOTEX171"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143XXXXXX231", replacer.Replace("%T143XXXXXX231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidDay()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143SHOTEXX31", replacer.Replace("%T143SHOTEXX31"));
        }

        [TestMethod]
        public void ReplaceTestInvalidScene()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143SHOTEX2X1", replacer.Replace("%T143SHOTEX2X1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ShotExReplacer(BestShots, @"C:\path\to\output\");
            Assert.AreEqual("%T143SHOTEX23X", replacer.Replace("%T143SHOTEX23X"));
        }
    }
}
