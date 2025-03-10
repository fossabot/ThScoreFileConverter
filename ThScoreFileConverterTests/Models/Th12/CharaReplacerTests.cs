﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th12;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using IClearData = ThScoreFileConverter.Models.Th10.IClearData<
    ThScoreFileConverter.Models.Th12.CharaWithTotal, ThScoreFileConverter.Models.Th10.StageProgress>;
using StageProgress = ThScoreFileConverter.Models.Th10.StageProgress;

namespace ThScoreFileConverterTests.Models.Th12
{
    [TestClass]
    public class CharaReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub<CharaWithTotal, StageProgress>
                {
                    Chara = CharaWithTotal.ReimuB,
                    TotalPlayCount = 23,
                    PlayTime = 4567890,
                    ClearCounts = Utils.GetEnumerator<Level>().ToDictionary(level => level, level => 100 - (int)level),
                },
                new ClearDataStub<CharaWithTotal, StageProgress>
                {
                    Chara = CharaWithTotal.MarisaA,
                    TotalPlayCount = 12,
                    PlayTime = 3456789,
                    ClearCounts = Utils.GetEnumerator<Level>().ToDictionary(level => level, level => 50 - (int)level),
                },
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void CharaReplacerTest()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CharaReplacerTestNull()
        {
            _ = new CharaReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CharaReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaReplacer(dictionary);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestTotalPlayCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("23", replacer.Replace("%T12CHARARB1"));
        }

        [TestMethod]
        public void ReplaceTestPlayTime()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("21:08:51", replacer.Replace("%T12CHARARB2"));
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("490", replacer.Replace("%T12CHARARB3"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalTotalPlayCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("35", replacer.Replace("%T12CHARATL1"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalPlayTime()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("37:09:04", replacer.Replace("%T12CHARATL2"));
        }

        [TestMethod]
        public void ReplaceTestCharaTotalClearCount()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("730", replacer.Replace("%T12CHARATL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CharaReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T12CHARARB1"));
            Assert.AreEqual("0:00:00", replacer.Replace("%T12CHARARB2"));
            Assert.AreEqual("0", replacer.Replace("%T12CHARARB3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCounts()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub<CharaWithTotal, StageProgress>
                {
                    Chara = CharaWithTotal.ReimuB,
                    ClearCounts = new Dictionary<Level, int>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CharaReplacer(dictionary);
            Assert.AreEqual("0", replacer.Replace("%T12CHARARB3"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T12XXXXXRB1", replacer.Replace("%T12XXXXXRB1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T12CHARAXX1", replacer.Replace("%T12CHARAXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CharaReplacer(ClearDataDictionary);
            Assert.AreEqual("%T12CHARARBX", replacer.Replace("%T12CHARARBX"));
        }
    }
}
