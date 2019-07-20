﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Globalization;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0053.
    internal sealed class Th15ClearDataWrapper
    {
        private static readonly Type ParentType = typeof(Th15Converter);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";
        private static readonly PrivateType PrivateType = new PrivateType(AssemblyNameToTest, TypeNameToTest);

        private readonly PrivateObject pobj = null;

        public Th15ClearDataWrapper(Th10ChapterWrapper chapter)
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest, new object[] { chapter?.Target });
        public Th15ClearDataWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        public object Target
            => this.pobj.Target;
        public string Signature
            => this.pobj.GetProperty(nameof(Signature)) as string;
        public ushort? Version
            => this.pobj.GetProperty(nameof(Version)) as ushort?;
        public uint? Checksum
            => this.pobj.GetProperty(nameof(Checksum)) as uint?;
        public int? Size
            => this.pobj.GetProperty(nameof(Size)) as int?;
        public bool? IsValid
            => this.pobj.GetProperty(nameof(IsValid)) as bool?;
        public IReadOnlyCollection<byte> Data
            => this.pobj.GetProperty(nameof(Data)) as byte[];
        public Th15Converter.CharaWithTotal? Chara
            => this.pobj.GetProperty(nameof(Chara)) as Th15Converter.CharaWithTotal?;
        // NOTE: Th15Converter.ClearDataPerGameMode is a private class.
        // public IReadOnlyDictionary<GameMode, ClearDataPerGameMode> Data1
        //     => this.pobj.GetProperty(nameof(Data1)) as Dictionary<GameMode, ClearDataPerGameMode>;
        public object Data1
            => this.pobj.GetProperty(nameof(Data1));
        public Th15ClearDataPerGameModeWrapper Data1Item(Th15Converter.GameMode mode)
            => new Th15ClearDataPerGameModeWrapper(
                this.Data1.GetType().GetProperty("Item").GetValue(this.Data1, new object[] { mode }));
        public IReadOnlyDictionary<(ThConverter.Level, Th15Converter.StagePractice), Practice> Practices
            => this.pobj.GetProperty(nameof(this.Practices))
                as Dictionary<(ThConverter.Level, Th15Converter.StagePractice), Practice>;

        public static bool CanInitialize(Th10ChapterWrapper chapter)
            => (bool)PrivateType.InvokeStatic(
                nameof(CanInitialize), new object[] { chapter.Target }, CultureInfo.InvariantCulture);
    }
}
