﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    public sealed class Th075HighScoreWrapper
    {
        private static readonly Type ParentType = typeof(Th075Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+HighScore";

        private readonly PrivateObject pobj = null;

        public static Th075HighScoreWrapper Create(byte[] array)
        {
            var highScore = new Th075HighScoreWrapper();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    highScore.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return highScore;
        }

        public Th075HighScoreWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th075HighScoreWrapper(object original)
            => this.pobj = new PrivateObject(original);

        public object Target
            => this.pobj.Target;
        public string Name
            => this.pobj.GetProperty(nameof(this.Name)) as string;
        public byte? Month
            => this.pobj.GetProperty(nameof(this.Month)) as byte?;
        public byte? Day
            => this.pobj.GetProperty(nameof(this.Day)) as byte?;
        public int? Score
            => this.pobj.GetProperty(nameof(this.Score)) as int?;

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
