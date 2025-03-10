﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Diagnostics.CodeAnalysis;
using ThScoreFileConverter.Models.Th07;
using ThScoreFileConverterTests.Models.Th07.Stubs;
using Chapter = ThScoreFileConverter.Models.Th06.Chapter;
using ClearDataStub = ThScoreFileConverterTests.Models.Th06.Stubs.ClearDataStub<
    ThScoreFileConverter.Models.Th07.Chara, ThScoreFileConverter.Models.Th07.Level>;

namespace ThScoreFileConverterTests.Models.Th07
{
    [TestClass]
    public class AllScoreDataTests
    {
        [TestMethod]
        public void AllScoreDataTest() => TestUtils.Wrap(() =>
        {
            var allScoreData = new AllScoreData();

            Assert.IsNull(allScoreData.Header);
            Assert.AreEqual(0, allScoreData.Rankings.Count);
            Assert.AreEqual(0, allScoreData.ClearData.Count);
            Assert.AreEqual(0, allScoreData.CardAttacks.Count);
            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
            Assert.IsNull(allScoreData.PlayStatus);
            Assert.IsNull(allScoreData.LastName);
            Assert.IsNull(allScoreData.VersionInfo);
        });

        [TestMethod]
        public void SetHeaderTest() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header = new Header(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header);

            Assert.AreSame(header, allScoreData.Header);
        });

        [TestMethod]
        public void SetHeaderTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(HeaderTests.MakeByteArray(HeaderTests.ValidProperties));
            var header1 = new Header(chapter);
            var header2 = new Header(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(header1);
            allScoreData.Set(header2);

            Assert.AreNotSame(header1, allScoreData.Header);
            Assert.AreSame(header2, allScoreData.Header);
        });

        [TestMethod]
        public void SetHighScoreTest() => TestUtils.Wrap(() =>
        {
            var score = new HighScoreStub(HighScoreTests.ValidStub)
            {
                Score = 87654u,
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.Rankings[(score.Chara, score.Level)][2]);
        });

        [TestMethod]
        public void SetHighScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var score1 = new HighScoreStub(HighScoreTests.ValidStub)
            {
                Score = 87654u,
            };
            var score2 = new HighScoreStub(score1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.Rankings[(score1.Chara, score1.Level)][2]);
            Assert.AreSame(score2, allScoreData.Rankings[(score2.Chara, score2.Level)][3]);
        });

        [TestMethod]
        public void SetClearDataTest() => TestUtils.Wrap(() =>
        {
            var clearData = new ClearDataStub(ClearDataTests.ValidStub);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData);

            Assert.AreSame(clearData, allScoreData.ClearData[clearData.Chara]);
        });

        [TestMethod]
        public void SetClearDataTestTwice() => TestUtils.Wrap(() =>
        {
            var clearData1 = new ClearDataStub(ClearDataTests.ValidStub);
            var clearData2 = new ClearDataStub(clearData1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(clearData1);
            allScoreData.Set(clearData2);

            Assert.AreSame(clearData1, allScoreData.ClearData[clearData1.Chara]);
            Assert.AreNotSame(clearData2, allScoreData.ClearData[clearData2.Chara]);
        });

        [TestMethod]
        public void SetCardAttackTest() => TestUtils.Wrap(() =>
        {
            var attack = new CardAttackStub(CardAttackTests.ValidStub);

            var allScoreData = new AllScoreData();
            allScoreData.Set(attack);

            Assert.AreSame(attack, allScoreData.CardAttacks[attack.CardId]);
        });

        [TestMethod]
        public void SetCardAttackTestTwice() => TestUtils.Wrap(() =>
        {
            var attack1 = new CardAttackStub(CardAttackTests.ValidStub);
            var attack2 = new CardAttackStub(attack1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(attack1);
            allScoreData.Set(attack2);

            Assert.AreSame(attack1, allScoreData.CardAttacks[attack1.CardId]);
            Assert.AreNotSame(attack2, allScoreData.CardAttacks[attack2.CardId]);
        });

        [TestMethod]
        public void SetPracticeScoreTest() => TestUtils.Wrap(() =>
        {
            var score = new PracticeScoreStub(PracticeScoreTests.ValidStub)
            {
                Level = Level.Normal,
                Stage = Stage.Six,
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreSame(score, allScoreData.PracticeScores[(score.Chara, score.Level, score.Stage)]);
        });

        [TestMethod]
        public void SetPracticeScoreTestTwice() => TestUtils.Wrap(() =>
        {
            var score1 = new PracticeScoreStub(PracticeScoreTests.ValidStub)
            {
                Level = Level.Normal,
                Stage = Stage.Six,
            };
            var score2 = new PracticeScoreStub(score1);

            var allScoreData = new AllScoreData();
            allScoreData.Set(score1);
            allScoreData.Set(score2);

            Assert.AreSame(score1, allScoreData.PracticeScores[(score1.Chara, score1.Level, score1.Stage)]);
            Assert.AreNotSame(score2, allScoreData.PracticeScores[(score2.Chara, score2.Level, score2.Stage)]);
        });

        [SuppressMessage("Microsoft.Performance", "CA1822:MarkMembersAsStatic")]
        [DataTestMethod]
        [DataRow(Level.Extra, Stage.Extra)]
        [DataRow(Level.Extra, Stage.Six)]
        [DataRow(Level.Normal, Stage.Extra)]
        [DataRow(Level.Phantasm, Stage.Phantasm)]
        [DataRow(Level.Phantasm, Stage.Six)]
        [DataRow(Level.Normal, Stage.Phantasm)]
        public void SetPracticeScoreTestInvalidPracticeStage(int level, int stage) => TestUtils.Wrap(() =>
        {
            var score = new PracticeScoreStub(PracticeScoreTests.ValidStub)
            {
                Level = TestUtils.Cast<Level>(level),
                Stage = TestUtils.Cast<Stage>(stage),
            };

            var allScoreData = new AllScoreData();
            allScoreData.Set(score);

            Assert.AreEqual(0, allScoreData.PracticeScores.Count);
        });

        [TestMethod]
        public void SetPlayStatusTest() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties));
            var status = new PlayStatus(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(status);

            Assert.AreSame(status, allScoreData.PlayStatus);
        });

        [TestMethod]
        public void SetPlayStatusTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(PlayStatusTests.MakeByteArray(PlayStatusTests.ValidProperties));
            var status1 = new PlayStatus(chapter);
            var status2 = new PlayStatus(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(status1);
            allScoreData.Set(status2);

            Assert.AreNotSame(status1, allScoreData.PlayStatus);
            Assert.AreSame(status2, allScoreData.PlayStatus);
        });

        [TestMethod]
        public void SetLastNameTest() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name = new LastName(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(name);

            Assert.AreSame(name, allScoreData.LastName);
        });

        [TestMethod]
        public void SetLastNameTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(LastNameTests.MakeByteArray(LastNameTests.ValidProperties));
            var name1 = new LastName(chapter);
            var name2 = new LastName(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(name1);
            allScoreData.Set(name2);

            Assert.AreNotSame(name1, allScoreData.LastName);
            Assert.AreSame(name2, allScoreData.LastName);
        });

        [TestMethod]
        public void SetVersionInfoTest() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info = new VersionInfo(chapter);

            var allScoreData = new AllScoreData();
            allScoreData.Set(info);

            Assert.AreSame(info, allScoreData.VersionInfo);
        });

        [TestMethod]
        public void SetVersionInfoTestTwice() => TestUtils.Wrap(() =>
        {
            var chapter = TestUtils.Create<Chapter>(VersionInfoTests.MakeByteArray(VersionInfoTests.ValidProperties));
            var info1 = new VersionInfo(chapter);
            var info2 = new VersionInfo(chapter);
            var allScoreData = new AllScoreData();
            allScoreData.Set(info1);
            allScoreData.Set(info2);

            Assert.AreNotSame(info1, allScoreData.VersionInfo);
            Assert.AreSame(info2, allScoreData.VersionInfo);
        });
    }
}
