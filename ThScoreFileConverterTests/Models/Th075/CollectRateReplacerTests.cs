﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th075;
using ThScoreFileConverterTests.Models.Th075.Stubs;
using Level = ThScoreFileConverter.Models.Th075.Level;

namespace ThScoreFileConverterTests.Models.Th075
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<(CharaWithReserved, Level), IClearData> ClearData { get; } =
            Utils.GetEnumerator<Level>().ToDictionary(
                level => (CharaWithReserved.Reimu, level),
                level => new ClearDataStub(ClearDataTests.ValidStub)
                {
                    CardGotCount = Enumerable.Range(1, 100).Select(count => (short)(count % 5)).ToList(),
                    CardTrialCount = Enumerable.Range(1, 100).Select(count => (short)(count % 7)).ToList(),
                    CardTrulyGot = Enumerable.Range(1, 100).Select(count => (byte)(count % 3)).ToList(),
                } as IClearData);

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CollectRateReplacerTestNull()
        {
            _ = new CollectRateReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CollectRateReplacerTestEmpty()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new CollectRateReplacer(clearData);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestClearCount()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("20", replacer.Replace("%T75CRGHRM1"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("21", replacer.Replace("%T75CRGHRM2"));
        }

        [TestMethod]
        public void ReplaceTestTrulyGot()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("16", replacer.Replace("%T75CRGHRM3"));
        }

        [TestMethod]
        public void ReplaceTestTotalClearCount()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("80", replacer.Replace("%T75CRGTRM1"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("86", replacer.Replace("%T75CRGTRM2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrulyGot()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("67", replacer.Replace("%T75CRGTRM3"));
        }

        [TestMethod]
        public void ReplaceTestEmptyClearCount()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new CollectRateReplacer(clearData);
            Assert.AreEqual("0", replacer.Replace("%T75CRGHRM1"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrialCount()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new CollectRateReplacer(clearData);
            Assert.AreEqual("0", replacer.Replace("%T75CRGHRM2"));
        }

        [TestMethod]
        public void ReplaceTestEmptyTrulyGot()
        {
            var clearData = new Dictionary<(CharaWithReserved, Level), IClearData>();
            var replacer = new CollectRateReplacer(clearData);
            Assert.AreEqual("0", replacer.Replace("%T75CRGHRM3"));
        }

        [TestMethod]
        public void ReplaceTestMeiling()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("%T75CRGHML1", replacer.Replace("%T75CRGHML1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("%T75XXXHRM1", replacer.Replace("%T75XXXHRM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("%T75CRGXRM1", replacer.Replace("%T75CRGXRM1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("%T75CRGHXX1", replacer.Replace("%T75CRGHXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(ClearData);
            Assert.AreEqual("%T75CRGHRMX", replacer.Replace("%T75CRGHRMX"));
        }

        [TestMethod]
        public void ReplaceTestInvalidCardId()
        {
#if false
            var cardAttacks = new List<ICardAttack>
            {
                new CardAttackStub(CardAttackTests.ValidStub)
                {
                    CardId = 142,
                    CardName = TestUtils.MakeRandomArray<byte>(0x30),
                },
            }.ToDictionary(element => (int)element.CardId);
            var replacer = new CollectRateReplacer(cardAttacks);
            Assert.AreEqual("0", replacer.Replace("%T75CRGHRM1"));
            Assert.AreEqual("0", replacer.Replace("%T75CRGXRB1"));
            Assert.AreEqual("0", replacer.Replace("%T75CRGPRB1"));
#endif
        }
    }
}
