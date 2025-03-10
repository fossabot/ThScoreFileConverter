﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th128;
using ThScoreFileConverterTests.Models.Th10.Stubs;
using IScoreData = ThScoreFileConverter.Models.Th10.IScoreData<ThScoreFileConverter.Models.Th128.StageProgress>;

namespace ThScoreFileConverterTests.Models.Th128
{
    [TestClass]
    public class ScoreDataTests
    {
        internal static ScoreDataStub<StageProgress> ValidStub { get; }
            = Th10.ScoreDataTests.MakeValidStub<StageProgress>();

        internal static byte[] MakeByteArray(IScoreData scoreData)
            => Th10.ScoreDataTests.MakeByteArray(scoreData, 8);

        [TestMethod]
        public void ReadFromTest()
        {
            var array = MakeByteArray(ValidStub);
            var scoreData = TestUtils.Create<ScoreData>(array);

            Th10.ScoreDataTests.Validate(ValidStub, scoreData);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull()
        {
            var scoreData = new ScoreData();
            scoreData.ReadFrom(null);

            Assert.Fail(TestUtils.Unreachable);
        }
    }
}
