﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Wrappers;
using ThScoreFileConverterTests.Models.Th07;
using ThScoreFileConverterTests.Models.Wrappers;

namespace ThScoreFileConverterTests.Models
{
    [TestClass]
    public class Th07AllScoreDataTests
    {
        [TestMethod]
        public void Th07AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new Th07AllScoreDataWrapper();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.RankingsCount);
            Assert.AreEqual(0, allScoreData.ClearDataCount);
            Assert.AreEqual(0, allScoreData.CardAttacksCount);
            Assert.AreEqual(0, allScoreData.PracticeScoresCount);
            Assert.IsNull(allScoreData.PlayStatus);
            Assert.IsNull(allScoreData.LastName);
            Assert.IsNull(allScoreData.VersionInfo);
        });

        [TestMethod]
        public void Th07AllScoreDataSetHeaderTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("TH7K")));
            var header = new Th06HeaderWrapper<Th07Converter>(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                HeaderTests.MakeByteArray(HeaderTests.GetValidProperties("TH7K")));
            var header1 = new Th06HeaderWrapper<Th07Converter>(chapter);
            var header2 = new Th06HeaderWrapper<Th07Converter>(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.Header.Target);
            Assert.AreSame(header2.Target, allScoreData.Header.Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = Th07HighScoreTests.ValidProperties;
            properties.score = 87654u;
            var chapter = ChapterWrapper.Create(Th07HighScoreTests.MakeByteArray(properties));
            var score = new Th07HighScoreWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(score.Target, allScoreData.RankingItem(properties.chara, properties.level, 2).Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetHighScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th07HighScoreTests.ValidProperties;
            properties.score = 87654u;
            var chapter = ChapterWrapper.Create(Th07HighScoreTests.MakeByteArray(properties));
            var score1 = new Th07HighScoreWrapper(chapter);
            var score2 = new Th07HighScoreWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1.Target, allScoreData.RankingItem(properties.chara, properties.level, 2).Target);
            Assert.AreSame(score2.Target, allScoreData.RankingItem(properties.chara, properties.level, 3).Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetClearDataTest() => TestUtils.Wrap(() =>
        {
            var properties = Th07ClearDataTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th07ClearDataTests.MakeByteArray(properties));
            var clearData = new Th07ClearDataWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData.Target, allScoreData.ClearDataItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th07ClearDataTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th07ClearDataTests.MakeByteArray(properties));
            var clearData1 = new Th07ClearDataWrapper(chapter);
            var clearData2 = new Th07ClearDataWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1.Target, allScoreData.ClearDataItem(properties.chara).Target);
            Assert.AreNotSame(clearData2.Target, allScoreData.ClearDataItem(properties.chara).Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetCardAttackTest() => TestUtils.Wrap(() =>
        {
            var properties = Th07CardAttackTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th07CardAttackTests.MakeByteArray(properties));
            var attack = new Th07CardAttackWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(attack);

            Assert.AreSame(attack.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetCardAttackTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th07CardAttackTests.ValidProperties;
            var chapter = ChapterWrapper.Create(Th07CardAttackTests.MakeByteArray(properties));
            var attack1 = new Th07CardAttackWrapper(chapter);
            var attack2 = new Th07CardAttackWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(attack1);
            allScoreData.Set(attack2);

            Assert.AreSame(attack1.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
            Assert.AreNotSame(attack2.Target, allScoreData.CardAttacksItem(properties.cardId).Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetPracticeScoreTest() => TestUtils.Wrap(() =>
        {
            var properties = Th07PracticeScoreTests.ValidProperties;
            properties.level = Th07Converter.Level.Normal;
            properties.stage = Th07Converter.Stage.St6;
            var chapter = ChapterWrapper.Create(Th07PracticeScoreTests.MakeByteArray(properties));
            var score = new Th07PracticeScoreWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(score);

            Assert.AreSame(
                score.Target,
                allScoreData.PracticeScore(properties.chara, properties.level, properties.stage).Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetPracticeScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var properties = Th07PracticeScoreTests.ValidProperties;
            properties.level = Th07Converter.Level.Normal;
            properties.stage = Th07Converter.Stage.St6;
            var chapter = ChapterWrapper.Create(Th07PracticeScoreTests.MakeByteArray(properties));
            var score1 = new Th07PracticeScoreWrapper(chapter);
            var score2 = new Th07PracticeScoreWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(
                score1.Target,
                allScoreData.PracticeScore(properties.chara, properties.level, properties.stage).Target);
            Assert.AreNotSame(
                score2.Target,
                allScoreData.PracticeScore(properties.chara, properties.level, properties.stage).Target);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(Th07Converter.Level.Extra, Th07Converter.Stage.Extra)]
        [DataRow(Th07Converter.Level.Extra, Th07Converter.Stage.St6)]
        [DataRow(Th07Converter.Level.Normal, Th07Converter.Stage.Extra)]
        [DataRow(Th07Converter.Level.Phantasm, Th07Converter.Stage.Phantasm)]
        [DataRow(Th07Converter.Level.Phantasm, Th07Converter.Stage.St6)]
        [DataRow(Th07Converter.Level.Normal, Th07Converter.Stage.Phantasm)]
        public void Th07AllScoreDataSetPracticeScoreTestInvalidPracticeStage(int level, int stage)
            => TestUtils.Wrap(() =>
            {
                var properties = Th07PracticeScoreTests.ValidProperties;
                properties.level = TestUtils.Cast<Th07Converter.Level>(level);
                properties.stage = TestUtils.Cast<Th07Converter.Stage>(stage);
                var chapter = ChapterWrapper.Create(
                    Th07PracticeScoreTests.MakeByteArray(properties));
                var score = new Th07PracticeScoreWrapper(chapter);

                var allScoreData = new Th07AllScoreDataWrapper();
                allScoreData.Set(score);

                Assert.AreEqual(0, allScoreData.PracticeScoresCount);
            });

        [TestMethod]
        public void Th07AllScoreDataSetPlayStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th07PlayStatusTests.MakeByteArray(Th07PlayStatusTests.ValidProperties));
            var header = new Th07PlayStatusWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(header);

            Assert.AreSame(header.Target, allScoreData.PlayStatus.Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetPlayStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                Th07PlayStatusTests.MakeByteArray(Th07PlayStatusTests.ValidProperties));
            var header1 = new Th07PlayStatusWrapper(chapter);
            var header2 = new Th07PlayStatusWrapper(chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1.Target, allScoreData.PlayStatus.Target);
            Assert.AreSame(header2.Target, allScoreData.PlayStatus.Target);
        });

        [TestMethod]
        public void Th07AllScoreDataSetLastNameTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name = new LastName(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(name);

            Assert.AreSame(name, allScoreData.LastName);
        });

        [TestMethod]
        public void Th07AllScoreDataSetLastNameTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name1 = new LastName(chapter.Target as Chapter);
            var name2 = new LastName(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(name1);
            allScoreData.Set(name2);

            Assert.AreNotSame(name1, allScoreData.LastName);
            Assert.AreSame(name2, allScoreData.LastName);
        });

        [TestMethod]
        public void Th07AllScoreDataSetVersionInfoTest() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info = new VersionInfo(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(info);

            Assert.AreSame(info, allScoreData.VersionInfo);
        });

        [TestMethod]
        public void Th07AllScoreDataSetVersionInfoTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = ChapterWrapper.Create(
                VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info1 = new VersionInfo(chapter.Target as Chapter);
            var info2 = new VersionInfo(chapter.Target as Chapter);

            var allScoreData = new Th07AllScoreDataWrapper();
            allScoreData.Set(info1);
            allScoreData.Set(info2);

            Assert.AreNotSame(info1, allScoreData.VersionInfo);
            Assert.AreSame(info2, allScoreData.VersionInfo);
        });
    }
}
