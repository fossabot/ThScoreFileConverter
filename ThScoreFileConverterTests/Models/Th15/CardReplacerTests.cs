﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th15.Stubs;
using ISpellCard = ThScoreFileConverter.Models.Th13.ISpellCard<ThScoreFileConverter.Models.Level>;
using SpellCardStub = ThScoreFileConverterTests.Models.Th13.Stubs.SpellCardStub<ThScoreFileConverter.Models.Level>;

namespace ThScoreFileConverterTests.Models.Th15
{
    [TestClass]
    public class CardReplacerTests
    {
        internal static IReadOnlyDictionary<CharaWithTotal, IClearData> ClearDataDictionary { get; } =
            new List<IClearData>
            {
                new ClearDataStub
                {
                    Chara = CharaWithTotal.Total,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            new ClearDataPerGameModeStub
                            {
                                Cards = new Dictionary<int, ISpellCard>()
                                {
                                    { 3, new SpellCardStub { HasTried = true } },
                                    { 4, new SpellCardStub { HasTried = false } },
                                },
                            }
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

        [TestMethod]
        public void CardReplacerTest()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void CardReplacerTestNull()
        {
            _ = new CardReplacer(null, false);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void CardReplacerTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();
            var replacer = new CardReplacer(dictionary, false);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTestName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("弾符「イーグルシューティング」", replacer.Replace("%T15CARD003N"));
            Assert.AreEqual("弾符「イーグルシューティング」", replacer.Replace("%T15CARD004N"));
        }

        [TestMethod]
        public void ReplaceTestRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("Easy", replacer.Replace("%T15CARD003R"));
            Assert.AreEqual("Normal", replacer.Replace("%T15CARD004R"));
        }

        [TestMethod]
        public void ReplaceTestHiddenName()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("弾符「イーグルシューティング」", replacer.Replace("%T15CARD003N"));
            Assert.AreEqual("??????????", replacer.Replace("%T15CARD004N"));
        }

        [TestMethod]
        public void ReplaceTestHiddenRank()
        {
            var replacer = new CardReplacer(ClearDataDictionary, true);
            Assert.AreEqual("Easy", replacer.Replace("%T15CARD003R"));
            Assert.AreEqual("Normal", replacer.Replace("%T15CARD004R"));
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var dictionary = new Dictionary<CharaWithTotal, IClearData>();

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T15CARD003N"));
        }

        [TestMethod]
        public void ReplaceTestEmptyGameModes()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
                {
                    Chara = CharaWithTotal.Total,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>(),
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T15CARD003N"));
        }

        [TestMethod]
        public void ReplaceTestEmptyCards()
        {
            var dictionary = new List<IClearData>
            {
                new ClearDataStub()
                {
                    Chara = CharaWithTotal.Total,
                    GameModeData = new Dictionary<GameMode, IClearDataPerGameMode>
                    {
                        {
                            GameMode.Pointdevice,
                            new ClearDataPerGameModeStub
                            {
                                Cards = new Dictionary<int, ISpellCard>(),
                            }
                        },
                    },
                },
            }.ToDictionary(element => element.Chara);

            var replacer = new CardReplacer(dictionary, true);
            Assert.AreEqual("??????????", replacer.Replace("%T15CARD003N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T15XXXX003N", replacer.Replace("%T15XXXX003N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidNumber()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T15CARD120N", replacer.Replace("%T15CARD120N"));
        }

        [TestMethod]
        public void ReplaceTestInvalidType()
        {
            var replacer = new CardReplacer(ClearDataDictionary, false);
            Assert.AreEqual("%T15CARD003X", replacer.Replace("%T15CARD003X"));
        }
    }
}
