﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th128.Stubs;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class CollectRateReplacerTests
    {
        internal static IReadOnlyDictionary<int, ISpellCard> SpellCards { get; } =
            Definitions.CardTable.ToDictionary(
                pair => pair.Key,
                pair => new SpellCardStub
                {
                    NoIceCount = pair.Key % 3,
                    NoMissCount = pair.Key % 5,
                    TrialCount = pair.Key % 7,
                    Id = pair.Value.Id,
                    Level = pair.Value.Level,
                } as ISpellCard);

        [TestMethod]
        public void CollectRateReplacerTest()
        {
            var replacer = new CollectRateReplacer(SpellCards);
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
            var cards = new Dictionary<int, ISpellCard>();
            var replacer = new CollectRateReplacer(cards);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("4", replacer.Replace("%T128CRGHA231"));
        }

        [TestMethod]
        public void ReplaceTestNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("5", replacer.Replace("%T128CRGHA232"));
        }

        [TestMethod]
        public void ReplaceTestTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("5", replacer.Replace("%T128CRGHA233"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("7", replacer.Replace("%T128CRGXA231"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("8", replacer.Replace("%T128CRGXA232"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtraTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("9", replacer.Replace("%T128CRGXA233"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128CRGHEXT1", replacer.Replace("%T128CRGHEXT1"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("16", replacer.Replace("%T128CRGTA231"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("19", replacer.Replace("%T128CRGTA232"));
        }

        [TestMethod]
        public void ReplaceTestLevelTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("21", replacer.Replace("%T128CRGTA233"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("40", replacer.Replace("%T128CRGHTTL1"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("48", replacer.Replace("%T128CRGHTTL2"));
        }

        [TestMethod]
        public void ReplaceTestStageTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("51", replacer.Replace("%T128CRGHTTL3"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoIceCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("167", replacer.Replace("%T128CRGTTTL1"));
        }

        [TestMethod]
        public void ReplaceTestTotalNoMissCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("200", replacer.Replace("%T128CRGTTTL2"));
        }

        [TestMethod]
        public void ReplaceTestTotalTrialCount()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("215", replacer.Replace("%T128CRGTTTL3"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var cards = new Dictionary<int, ISpellCard>();
            var replacer = new CollectRateReplacer(cards);
            Assert.AreEqual("0", replacer.Replace("%T128CRGHA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128XXXHA231", replacer.Replace("%T128XXXHA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128CRGYA231", replacer.Replace("%T128CRGYA231"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128CRGHXXX1", replacer.Replace("%T128CRGHXXX1"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CollectRateReplacer(SpellCards);
            Assert.AreEqual("%T128CRGHA23X", replacer.Replace("%T128CRGHA23X"));
        }
    }
}
