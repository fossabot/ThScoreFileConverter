﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverterTests.Models.Th095;
using ThScoreFileConverterTests.Models.Th125.Stubs;
using ThScoreFileConverterTests.Models.Th13.Stubs;
using HeaderBase = ThScoreFileConverter.Models.Th095.HeaderBase;

namespace ThScoreFileConverterTests.Models.Th13
{
    [TestClass]
    public class AllScoreDataTests
    {
        internal static void AllScoreDataTestHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TChWithT : struct, Enum       // TCharaWithTotal
            where TLv : struct, Enum            // TLevel
            where TLvPrac : struct, Enum        // TLevelPractice
            where TLvPracWithT : struct, Enum   // TLevelPracticeWithTotal
            where TStPrac : struct, Enum        // TStagePractice
            => TestUtils.Wrap(() =>
            {
                var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();

                Assert.IsNull(allScoreData.Header);
                Assert.AreEqual(0, allScoreData.ClearData.Count);
                Assert.IsNull(allScoreData.Status);
            });

        internal static void SetHeaderTestHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
                var header = TestUtils.Create<HeaderBase>(array);

                var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(header);

                Assert.AreSame(header, allScoreData.Header);
            });

        internal static void SetHeaderTestTwiceHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var array = HeaderBaseTests.MakeByteArray(HeaderBaseTests.ValidProperties);
                var header1 = TestUtils.Create<HeaderBase>(array);
                var header2 = TestUtils.Create<HeaderBase>(array);

                var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(header1);
                allScoreData.Set(header2);

                Assert.AreNotSame(header1, allScoreData.Header);
                Assert.AreSame(header2, allScoreData.Header);
            });

        internal static void SetClearDataTestHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var chara = TestUtils.Cast<TChWithT>(1);
                var clearData = new ClearDataStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> { Chara = chara };

                var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(clearData);

                Assert.AreSame(clearData, allScoreData.ClearData[chara]);
            });

        internal static void SetClearDataTestTwiceHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var chara = TestUtils.Cast<TChWithT>(1);
                var clearData1 = new ClearDataStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> { Chara = chara };
                var clearData2 = new ClearDataStub<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac> { Chara = chara };

                var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(clearData1);
                allScoreData.Set(clearData2);

                Assert.AreSame(clearData1, allScoreData.ClearData[chara]);
                Assert.AreNotSame(clearData2, allScoreData.ClearData[chara]);
            });

        internal static void SetStatusTestHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var status = new StatusStub();

                var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(status);

                Assert.AreSame(status, allScoreData.Status);
            });

        internal static void SetStatusTestTwiceHelper<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>()
            where TChWithT : struct, Enum
            where TLv : struct, Enum
            where TLvPrac : struct, Enum
            where TLvPracWithT : struct, Enum
            where TStPrac : struct, Enum
            => TestUtils.Wrap(() =>
            {
                var status1 = new StatusStub();
                var status2 = new StatusStub();

                var allScoreData = new AllScoreData<TChWithT, TLv, TLvPrac, TLvPracWithT, TStPrac>();
                allScoreData.Set(status1);
                allScoreData.Set(status2);

                Assert.AreNotSame(status1, allScoreData.Status);
                Assert.AreSame(status2, allScoreData.Status);
            });

        [TestMethod]
        public void AllScoreDataTest()
            => AllScoreDataTestHelper<
                CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void SetHeaderTest()
            => SetHeaderTestHelper<
                CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void SetHeaderTestTwice()
            => SetHeaderTestTwiceHelper<
                CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void SetClearDataTest()
            => SetClearDataTestHelper<
                CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void SetClearDataTestTwice()
            => SetClearDataTestTwiceHelper<
                CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void SetStatusTest()
            => SetStatusTestHelper<
                CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice>();

        [TestMethod]
        public void SetStatusTestTwice()
            => SetStatusTestTwiceHelper<
                CharaWithTotal, LevelPractice, LevelPractice, LevelPracticeWithTotal, StagePractice>();
    }
}
