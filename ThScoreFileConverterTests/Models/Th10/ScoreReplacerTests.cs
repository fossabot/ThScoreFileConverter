﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th10;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th10.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th10
{
    [TestClass]
    public class ScoreReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                ClearDataTests.MakeValidStub(),
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void ScoreReplacerTest()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ScoreReplacerTestNull()
        {
            _ = new ScoreReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void ScoreReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Player1", replacer.Replace("%T10SCRHRB21"));
        }

        [TestMethod]
        public void ReplaceTestScore()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new ScoreReplacer(ClearDataDictionary);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("123,446,701", replacer.Replace("%T10SCRHRB22"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("123446701", replacer.Replace("%T10SCRHRB22"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestStage()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("Stage 5", replacer.Replace("%T10SCRHRB23"));
        }

        [TestMethod]
        public void ReplaceTestDateTime()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            var expected = new DateTime(1970, 1, 1).AddSeconds(34567890).ToLocalTime().ToString("yyyy/MM/dd HH:mm:ss");
            Assert.AreEqual(expected, replacer.Replace("%T10SCRHRB24"));
        }

        [TestMethod]
        public void ReplaceTestSlowRate()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("1.200%", replacer.Replace("%T10SCRHRB25"));  // really...?
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T10SCRHRB21"));
            Assert.AreEqual("0", replacer.Replace("%T10SCRHRB22"));
            Assert.AreEqual("-------", replacer.Replace("%T10SCRHRB23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T10SCRHRB24"));
            Assert.AreEqual("-----%", replacer.Replace("%T10SCRHRB25"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRankings()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub<CharaWithTotal, StageProgress>()
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = new Dictionary<Level, IReadOnlyList<IScoreData<StageProgress>>>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T10SCRHRB21"));
            Assert.AreEqual("0", replacer.Replace("%T10SCRHRB22"));
            Assert.AreEqual("-------", replacer.Replace("%T10SCRHRB23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T10SCRHRB24"));
            Assert.AreEqual("-----%", replacer.Replace("%T10SCRHRB25"));
        }

        [TestMethod]
        public void ReplaceTestEmptyRanking()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub<CharaWithTotal, StageProgress>()
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = Utils.GetEnumerator<Level>().ToDictionary(
                        level => level,
                        level => new List<IScoreData<StageProgress>>() as IReadOnlyList<IScoreData<StageProgress>>),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("--------", replacer.Replace("%T10SCRHRB21"));
            Assert.AreEqual("0", replacer.Replace("%T10SCRHRB22"));
            Assert.AreEqual("-------", replacer.Replace("%T10SCRHRB23"));
            Assert.AreEqual("----/--/-- --:--:--", replacer.Replace("%T10SCRHRB24"));
            Assert.AreEqual("-----%", replacer.Replace("%T10SCRHRB25"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub<CharaWithTotal, StageProgress>()
                {
                    Chara = CharaWithTotal.ReimuB,
                    Rankings = Utils.GetEnumerator<Level>().ToDictionary(
                        level => level,
                        level => Enumerable.Range(0, 10).Select(
                            index => new ScoreDataStub<StageProgress>()
                            {
                                DateTime = 34567890u,
                                StageProgress = StageProgress.Extra,
                            }).ToList() as IReadOnlyList<IScoreData<StageProgress>>),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new ScoreReplacer(dictionary);
            Assert.AreEqual("Not Clear", replacer.Replace("%T10SCRHRB23"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10XXXHRB21", replacer.Replace("%T10XXXHRB21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10SCRYRB21", replacer.Replace("%T10SCRYRB21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10SCRHXX21", replacer.Replace("%T10SCRHXX21"));
        }

        [TestMethod]
        public void ReplaceTestInvalidRank()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10SCRHRBX1", replacer.Replace("%T10SCRHRBX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new ScoreReplacer(ClearDataDictionary);
            Assert.AreEqual("%T10SCRHRB2X", replacer.Replace("%T10SCRHRB2X"));
        }
    }
}
