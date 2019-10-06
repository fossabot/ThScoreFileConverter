﻿using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models;
using ThScoreFileConverter.Models.Th13;
using ThScoreFileConverter.Models.Th15;
using ThScoreFileConverterTests.Models.Th13.Stubs;

namespace ThScoreFileConverterTests.Models.Th15.Stubs
{
    internal class ClearDataStub : IClearData
    {
        public ClearDataStub() { }

        public ClearDataStub(IClearData clearData)
            : this()
        {
            this.Chara = clearData.Chara;
            this.GameModeData = clearData.GameModeData.ToDictionary(
                pair => pair.Key,
                pair => new ClearDataPerGameModeStub(pair.Value) as IClearDataPerGameMode);
            this.Practices = clearData.Practices.ToDictionary(
                pair => pair.Key,
                pair => new PracticeStub(pair.Value) as IPractice);
            this.Checksum = clearData.Checksum;
            this.IsValid = clearData.IsValid;
            this.Signature = clearData.Signature;
            this.Size = clearData.Size;
            this.Version = clearData.Version;
        }

        public Th15Converter.CharaWithTotal Chara { get; set; }

        public IReadOnlyDictionary<Th15Converter.GameMode, IClearDataPerGameMode> GameModeData { get; set; }

        public IReadOnlyDictionary<(Level, Th15Converter.StagePractice), IPractice> Practices { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}
