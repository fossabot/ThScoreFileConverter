﻿using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th06;
using ThScoreFileConverterTests.Models.Th06.Stubs;

namespace ThScoreFileConverterTests.Models.Th06
{
    [TestClass]
    public class PracticeReplacerTests
    {
        internal static IReadOnlyDictionary<(Chara, Level, Stage), IPracticeScore> PracticeScores { get; } =
            new List<IPracticeScore>
            {
                new PracticeScoreStub(PracticeScoreTests.ValidStub),
            }.ToDictionary(element => (element.Chara, element.Level, element.Stage));

        [TestMethod]
        public void PracticeReplacerTest()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void PracticeReplacerTestNull()
        {
            _ = new PracticeReplacer(null);
            Assert.Fail(TestUtils.Unreachable);
        }

        [TestMethod]
        public void PracticeReplacerTestEmpty()
        {
            var practiceScores = new Dictionary<(Chara, Level, Stage), IPracticeScore>();
            var replacer = new PracticeReplacer(practiceScores);
            Assert.IsNotNull(replacer);
        }

        [TestMethod]
        public void ReplaceTest()
        {
            var outputSeparator = Settings.Instance.OutputNumberGroupSeparator;

            var replacer = new PracticeReplacer(PracticeScores);

            Settings.Instance.OutputNumberGroupSeparator = true;
            Assert.AreEqual("123,456", replacer.Replace("%T06PRACHRB6"));

            Settings.Instance.OutputNumberGroupSeparator = false;
            Assert.AreEqual("123456", replacer.Replace("%T06PRACHRB6"));

            Settings.Instance.OutputNumberGroupSeparator = outputSeparator;
        }

        [TestMethod]
        public void ReplaceTestEmpty()
        {
            var practiceScores = new Dictionary<(Chara, Level, Stage), IPracticeScore>();
            var replacer = new PracticeReplacer(practiceScores);
            Assert.AreEqual("0", replacer.Replace("%T06PRACHRB6"));
        }

        [TestMethod]
        public void ReplaceTestLevelExtra()
        {
            var practiceScores = new List<IPracticeScore>
            {
                new PracticeScoreStub(PracticeScoreTests.ValidStub)
                {
                    Level = Level.Extra,
                },
            }.ToDictionary(element => (element.Chara, element.Level, element.Stage));
            var replacer = new PracticeReplacer(practiceScores);
            Assert.AreEqual("%T06PRACXRB6", replacer.Replace("%T06PRACXRB6"));
        }

        [TestMethod]
        public void ReplaceTestStageExtra()
        {
            var practiceScores = new List<IPracticeScore>
            {
                new PracticeScoreStub(PracticeScoreTests.ValidStub)
                {
                    Stage = Stage.Extra,
                },
            }.ToDictionary(element => (element.Chara, element.Level, element.Stage));
            var replacer = new PracticeReplacer(practiceScores);
            Assert.AreEqual("%T06PRACHRBX", replacer.Replace("%T06PRACHRBX"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentLevel()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("0", replacer.Replace("%T06PRACNRB6"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentChara()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("0", replacer.Replace("%T06PRACHRA6"));
        }

        [TestMethod]
        public void ReplaceTestNonexistentStage()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("0", replacer.Replace("%T06PRACHRB5"));
        }

        [TestMethod]
        public void ReplaceTestInvalidFormat()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T06XXXXHRB6", replacer.Replace("%T06XXXXHRB6"));
        }

        [TestMethod]
        public void ReplaceTestInvalidLevel()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T06PRACYRB6", replacer.Replace("%T06PRACYRB6"));
        }

        [TestMethod]
        public void ReplaceTestInvalidChara()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T06PRACHXX6", replacer.Replace("%T06PRACHXX6"));
        }

        [TestMethod]
        public void ReplaceTestInvalidStage()
        {
            var replacer = new PracticeReplacer(PracticeScores);
            Assert.AreEqual("%T06PRACHRBY", replacer.Replace("%T06PRACHRBY"));
        }
    }
}
