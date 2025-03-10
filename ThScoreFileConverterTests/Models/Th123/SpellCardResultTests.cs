﻿using System;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ThScoreFileConverter.Models.Th123;

namespace ThScoreFileConverterTests.Models.Th123
{
    [TestClass]
    public class SpellCardResultTests
    {
        [TestMethod]
        public void SpellCardResultTest() => Th105.SpellCardResultTests.SpellCardResultTestHelper<Chara>();

        [TestMethod]
        public void ReadFromTest() => Th105.SpellCardResultTests.ReadFromTestHelper<Chara>();

        [TestMethod]
        [ExpectedException(typeof(ArgumentNullException))]
        public void ReadFromTestNull() => Th105.SpellCardResultTests.ReadFromTestNullHelper<Chara>();

        [TestMethod]
        [ExpectedException(typeof(EndOfStreamException))]
        public void ReadFromTestShortened() => Th105.SpellCardResultTests.ReadFromTestShortenedHelper<Chara>();

        [TestMethod]
        public void ReadFromTestExceeded() => Th105.SpellCardResultTests.ReadFromTestExceededHelper<Chara>();
    }
}
