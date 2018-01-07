﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Tests
{
    // NOTE: Setting the accessibility as public causes CS0051 and CS0053.
    internal sealed class Th07HighScoreWrapper
    {
        private static Type ParentType = typeof(Th07Converter);
        private static string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static string TypeNameToTest = ParentType.FullName + "+HighScore";

        private PrivateObject pobj = null;

        public Th07HighScoreWrapper(Th06ChapterWrapper<Th07Converter> chapter)
        {
            if (chapter == null)
            {
                var ch = new Th06ChapterWrapper<Th07Converter>();
                this.pobj = new PrivateObject(
                    AssemblyNameToTest,
                    TypeNameToTest,
                    new Type[] { ch.Target.GetType() },
                    new object[] { null });
            }
            else
            {
                this.pobj = new PrivateObject(
                    AssemblyNameToTest,
                    TypeNameToTest,
                    new Type[] { chapter.Target.GetType() },
                    new object[] { chapter.Target });
            }
        }

        public Th07HighScoreWrapper(uint score)
        {
            this.pobj = new PrivateObject(
                AssemblyNameToTest,
                TypeNameToTest,
                new Type[] { score.GetType() },
                new object[] { score });
        }

        // NOTE: Enabling the following causes CA1811.
        // public object Target => this.pobj.Target;

        public string Signature
            => this.pobj.GetProperty(nameof(Signature)) as string;
        public short? Size1
            => this.pobj.GetProperty(nameof(Size1)) as short?;
        public short? Size2
            => this.pobj.GetProperty(nameof(Size2)) as short?;
        public byte? FirstByteOfData
            => this.pobj.GetProperty(nameof(FirstByteOfData)) as byte?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(Data)) as byte[];
        public uint? Score
            => this.pobj.GetProperty(nameof(Score)) as uint?;
        public float? SlowRate
            => this.pobj.GetProperty(nameof(SlowRate)) as float?;
        public Th07Converter.Chara? Chara
            => this.pobj.GetProperty(nameof(Chara)) as Th07Converter.Chara?;
        public Th07Converter.Level? Level
            => this.pobj.GetProperty(nameof(Level)) as Th07Converter.Level?;
        public Th07Converter.StageProgress? StageProgress
            => this.pobj.GetProperty(nameof(StageProgress)) as Th07Converter.StageProgress?;
        public IReadOnlyCollection<byte> Name
            => this.pobj.GetProperty(nameof(Name)) as byte[];
        public IReadOnlyCollection<byte> Date
            => this.pobj.GetProperty(nameof(Date)) as byte[];
        public ushort? ContinueCount
            => this.pobj.GetProperty(nameof(ContinueCount)) as ushort?;
    }
}
