﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ClearDataStub = ThScoreFileConverterTests.Models.Th13.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;
using IClearData = ThScoreFileConverter.Models.Th13.IClearData<
    ThScoreFileConverter.Models.Th13.CharaWithTotal,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPractice,
    ThScoreFileConverter.Models.Th13.LevelPracticeWithTotal,
    ThScoreFileConverter.Models.Th13.StagePractice>;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class CharaExReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    TotalPlayCount = 23,
                    PlayTime = 4567890,
                    ClearCounts = Utils.GetEnumerator<LevelPracticeWithTotal>()
                        .ToDictionary(level => level, level => 100 - (int)level),
                },
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Sanae,
                    TotalPlayCount = 12,
                    PlayTime = 3456789,
                    ClearCounts = Utils.GetEnumerator<LevelPracticeWithTotal>()
                        .ToDictionary(level => level, level => 50 - (int)level),
                },
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void CharaExReplacerTest()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CharaExReplacerTestNull()
        {
            _ = new CharaExReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CharaExReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaExReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T13CHARAEXHMR1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T13CHARAEXHMR2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("98", replacer.Replace("%T13CHARAEXHMR3"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T13CHARAEXTMR1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T13CHARAEXTMR2"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("679", replacer.Replace("%T13CHARAEXTMR3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T13CHARAEXHTL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T13CHARAEXHTL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("146", replacer.Replace("%T13CHARAEXHTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalTotalPlayCount()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T13CHARAEXTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalPlayTime()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T13CHARAEXTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new CharaExReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("1,008", replacer.Replace("%T13CHARAEXTTL3"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("1008", replacer.Replace("%T13CHARAEXTTL3"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T13CHARAEXHMR1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T13CHARAEXHMR2"));
            Assert.AreEqual("0", replacer.Replace("%T13CHARAEXHMR3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Marisa,
                    ClearCounts = new Dictionary<LevelPracticeWithTotal, int>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CharaExReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T13CHARAEXHMR3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13XXXXXXXHMR1", replacer.Replace("%T13XXXXXXXHMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CHARAEXYMR1", replacer.Replace("%T13CHARAEXYMR1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CHARAEXHXX1", replacer.Replace("%T13CHARAEXHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaExReplacer(ClearDataDictionary);
            Assert.AreEqual("%T13CHARAEXHMRX", replacer.Replace("%T13CHARAEXHMRX"));
        }
    }
}
