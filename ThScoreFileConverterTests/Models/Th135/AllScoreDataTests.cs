﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th135;
using ThScoreFileConverterTests.Extensions;

namespace ThScoreFileConverterTests.Models.Th135
{
    using SQOT = ThScoreFileConverter.Squirrel.SQObjectType;

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
            public bool isPlayableMamizou;
            public bool isPlayableKokoro;
            public Dictionary<int, bool> bgmFlags;
        };

        internal static Properties GetValidProperties() => new Properties()
        {
            storyProgress = 1,
            storyClearFlags = Utils.GetEnumerator<Chara>().ToDictionary(
                chara => chara, chara => TestUtils.Cast<LevelFlags>(30 - (int)chara)),
            endingCount = 2,
            ending2Count = 3,
            isEnabledStageTanuki1 = true,
            isEnabledStageTanuki2 = true,
            isEnabledStageKokoro = false,
            isPlayableMamizou = true,
            isPlayableKokoro = false,
            bgmFlags = Enumerable.Range(1, 10).ToDictionary(id => id, id => id % 2 == 0)
        };

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
                    "enable_mamizou", properties.isPlayableMamizou,
                    "enable_kokoro", properties.isPlayableKokoro,
                    "enable_bgm", properties.bgmFlags))
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
            Assert.AreEqual(expected.isPlayableMamizou, actual.IsPlayableMamizou);
            Assert.AreEqual(expected.isPlayableKokoro, actual.IsPlayableKokoro);
            CollectionAssert.That.AreEqual(expected.bgmFlags.Keys, actual.BgmFlags.Keys);
            CollectionAssert.That.AreEqual(expected.bgmFlags.Values, actual.BgmFlags.Values);
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
            var properties = GetValidProperties();

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
            Assert.AreEqual(default, allScoreData.IsPlayableMamizou);
            Assert.AreEqual(default, allScoreData.IsPlayableKokoro);
            Assert.IsNull(allScoreData.BgmFlags);
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
    }
}
