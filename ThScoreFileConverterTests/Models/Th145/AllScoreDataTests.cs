﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th145;
using ThScoreFileConverterTests.Extensions;
using Level = ThScoreFileConverter.Models.Th145.Level;
using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

namespace ThScoreFileConverterTests.Models.Th145
{
    [TestClass]
    public class AllScoreDataTests
    {
        internal struct Properties
        {
            public int storyProgress;
            public Dictionary<Chara, LevelFlags> storyClearFlags;
            public int endingCount;
            public int ending2Count;
            public bool isEnabledStageTanuki1;
            public bool isEnabledStageTanuki2;
            public bool isEnabledStageKokoro;
            public bool isEnabledSt27;
            public bool isEnabledSt28;
            public bool isPlayableMamizou;
            public bool isPlayableKokoro;
            public Dictionary<int, bool> bgmFlags;
            public Dictionary<Level, Dictionary<Chara, int>> clearRanks;
            public Dictionary<Level, Dictionary<Chara, int>> clearTimes;
        };

        internal static Properties MakeValidProperties()
        {
            var charas = Utils.GetEnumerator<Chara>();
            var levels = Utils.GetEnumerator<Level>();

            return new Properties()
            {
                storyProgress = 1,
                storyClearFlags = charas.ToDictionary(
                    chara => chara, chara => TestUtils.Cast<LevelFlags>(30 - (int)chara)),
                endingCount = 2,
                ending2Count = 3,
                isEnabledStageTanuki1 = true,
                isEnabledStageTanuki2 = true,
                isEnabledStageKokoro = false,
                isEnabledSt27 = false,
                isEnabledSt28 = false,
                isPlayableMamizou = true,
                isPlayableKokoro = false,
                bgmFlags = Enumerable.Range(1, 10).ToDictionary(id => id, id => id % 2 == 0),
                clearRanks = levels.ToDictionary(
                    level => level, level => charas.ToDictionary(
                        chara => chara, chara => (int)level * 100 + (int)chara)),
                clearTimes = levels.ToDictionary(
                    level => level, level => charas.ToDictionary(
                        chara => chara, chara => (int)chara * 100 + (int)level))
            };
        }

        internal static byte[] MakeByteArray(in Properties properties)
            => new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table))
                .Concat(TestUtils.MakeSQByteArray(
                    "story_progress", properties.storyProgress,
                    "story_clear", properties.storyClearFlags.Select(pair => (int)pair.Value).ToArray(),
                    "ed_count", properties.endingCount,
                    "ed2_count", properties.ending2Count,
                    "enable_stage_tanuki1", properties.isEnabledStageTanuki1,
                    "enable_stage_tanuki2", properties.isEnabledStageTanuki2,
                    "enable_stage_kokoro", properties.isEnabledStageKokoro,
                    "enable_st27", properties.isEnabledSt27,
                    "enable_st28", properties.isEnabledSt28,
                    "enable_mamizou", properties.isPlayableMamizou,
                    "enable_kokoro", properties.isPlayableKokoro,
                    "enable_bgm", properties.bgmFlags,
                    "clear_rank", properties.clearRanks.Select(
                        perLevelPair => perLevelPair.Value.Select(
                            perCharaPair => perCharaPair.Value).ToArray()).ToArray(),
                    "clear_time", properties.clearTimes.Select(
                        perLevelPair => perLevelPair.Value.Select(
                            perCharaPair => perCharaPair.Value).ToArray()).ToArray()))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray();

        internal static void Validate(in Properties expected, in AllScoreData actual)
        {
            Assert.AreEqual(expected.storyProgress, actual.StoryProgress);
            CollectionAssert.That.AreEqual(expected.storyClearFlags.Keys, actual.StoryClearFlags.Keys);
            CollectionAssert.That.AreEqual(expected.storyClearFlags.Values, actual.StoryClearFlags.Values);
            Assert.AreEqual(expected.endingCount, actual.EndingCount);
            Assert.AreEqual(expected.ending2Count, actual.Ending2Count);
            Assert.AreEqual(expected.isEnabledStageTanuki1, actual.IsEnabledStageTanuki1);
            Assert.AreEqual(expected.isEnabledStageTanuki2, actual.IsEnabledStageTanuki2);
            Assert.AreEqual(expected.isEnabledStageKokoro, actual.IsEnabledStageKokoro);
            Assert.AreEqual(expected.isEnabledSt27, actual.IsEnabledSt27);
            Assert.AreEqual(expected.isEnabledSt28, actual.IsEnabledSt28);
            Assert.AreEqual(expected.isPlayableMamizou, actual.IsPlayableMamizou);
            Assert.AreEqual(expected.isPlayableKokoro, actual.IsPlayableKokoro);
            CollectionAssert.That.AreEqual(expected.bgmFlags.Keys, actual.BgmFlags.Keys);
            CollectionAssert.That.AreEqual(expected.bgmFlags.Values, actual.BgmFlags.Values);
            CollectionAssert.That.AreEqual(expected.clearRanks.Keys, actual.ClearRanks.Keys);

            foreach (var pair in expected.clearRanks)
            {
                CollectionAssert.That.AreEqual(pair.Value.Keys, actual.ClearRanks[pair.Key].Keys);
                CollectionAssert.That.AreEqual(pair.Value.Values, actual.ClearRanks[pair.Key].Values);
            }

            CollectionAssert.That.AreEqual(expected.clearTimes.Keys, actual.ClearTimes.Keys);

            foreach (var pair in expected.clearTimes)
            {
                CollectionAssert.That.AreEqual(pair.Value.Keys, actual.ClearTimes[pair.Key].Keys);
                CollectionAssert.That.AreEqual(pair.Value.Values, actual.ClearTimes[pair.Key].Values);
            }
        }

        [TestMethod]
        public void AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();

            Assert.AreEqual(default, allScoreData.StoryProgress);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(default, allScoreData.EndingCount);
            Assert.AreEqual(default, allScoreData.Ending2Count);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki1);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki2);
            Assert.AreEqual(default, allScoreData.IsEnabledStageKokoro);
            Assert.AreEqual(default, allScoreData.IsPlayableMamizou);
            Assert.AreEqual(default, allScoreData.IsPlayableKokoro);
            Assert.IsNull(allScoreData.BgmFlags);
        });

        [TestMethod]
        public void ReadFromTest() => TestUtils.Wrap(() =>
        {
            var properties = MakeValidProperties();

            var allScoreData = TestUtils.Create<AllScoreData>(MakeByteArray(properties));

            Validate(properties, allScoreData);
        });

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();
            allScoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestEmpty() => TestUtils.Wrap(() =>
        {
            TestUtils.Create<AllScoreData>(new byte[0]);

            Assert.Fail(TestUtils.Unreachable);
        });

        [TestMethod]
        public void ReadFromTestNoKey() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(TestUtils.MakeByteArray((int)SQOT.Null));

            Assert.AreEqual(default, allScoreData.StoryProgress);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(default, allScoreData.EndingCount);
            Assert.AreEqual(default, allScoreData.Ending2Count);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki1);
            Assert.AreEqual(default, allScoreData.IsEnabledStageTanuki2);
            Assert.AreEqual(default, allScoreData.IsEnabledStageKokoro);
            Assert.AreEqual(default, allScoreData.IsEnabledSt27);
            Assert.AreEqual(default, allScoreData.IsEnabledSt28);
            Assert.AreEqual(default, allScoreData.IsPlayableMamizou);
            Assert.AreEqual(default, allScoreData.IsPlayableKokoro);
            Assert.IsNull(allScoreData.BgmFlags);
            Assert.IsNull(allScoreData.ClearRanks);
            Assert.IsNull(allScoreData.ClearTimes);
        });

        [TestMethod]
        public void ReadFromTestNoTables() => TestUtils.Wrap(() =>
        {
            var storyProgressValue = 1;

            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_progress", storyProgressValue))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.AreEqual(storyProgressValue, allScoreData.StoryProgress);
            Assert.IsNull(allScoreData.StoryClearFlags);
            Assert.IsNull(allScoreData.BgmFlags);
            Assert.IsNull(allScoreData.ClearRanks);
            Assert.IsNull(allScoreData.ClearTimes);
        });

        [TestMethod]
        public void ReadFromTestInvalidStoryClear() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_clear", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.StoryClearFlags);
        });

        [TestMethod]
        public void ReadFromTestInvalidStoryClearValue() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("story_clear", new float[] { 123f }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.StoryClearFlags);
            Assert.AreEqual(0, allScoreData.StoryClearFlags.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidEnableBgm() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("enable_bgm", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.BgmFlags);
        });

        [TestMethod]
        public void ReadFromTestInvalidClearRank() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_rank", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.ClearRanks);
        });

        [TestMethod]
        public void ReadFromTestInvalidClearRankValuePerLevel() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_rank", new float[] { 123f }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.ClearRanks);
            Assert.AreEqual(0, allScoreData.ClearRanks.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidClearRankValuePerChara() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_rank", new float[][] { new float[] { 123f } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.ClearRanks);
            Assert.AreEqual(1, allScoreData.ClearRanks.Count);
            Assert.IsNotNull(allScoreData.ClearRanks.First().Value);
            Assert.AreEqual(0, allScoreData.ClearRanks.First().Value.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidClearTime() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_time", 1))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNull(allScoreData.ClearTimes);
        });

        [TestMethod]
        public void ReadFromTestInvalidClearTimeValuePerLevel() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_time", new float[] { 123f }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.ClearTimes);
            Assert.AreEqual(0, allScoreData.ClearTimes.Count);
        });

        [TestMethod]
        public void ReadFromTestInvalidClearTimeValuePerChara() => TestUtils.Wrap(() =>
        {
            var allScoreData = TestUtils.Create<AllScoreData>(new byte[0]
                // .Concat(TestUtils.MakeByteArray((int)SQOT.Table)
                .Concat(TestUtils.MakeSQByteArray("clear_time", new float[][] { new float[] { 123f } }))
                .Concat(TestUtils.MakeByteArray((int)SQOT.Null))
                .ToArray());

            Assert.IsNotNull(allScoreData.ClearTimes);
            Assert.AreEqual(1, allScoreData.ClearTimes.Count);
            Assert.IsNotNull(allScoreData.ClearTimes.First().Value);
            Assert.AreEqual(0, allScoreData.ClearTimes.First().Value.Count);
        });
    }
}
