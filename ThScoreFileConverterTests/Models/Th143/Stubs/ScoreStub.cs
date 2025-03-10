﻿using System.Collections.Generic;
using System.Linq;
using ThScoreFileConverter.Models.Th143;

namespace ThScoreFileConverterTests.Models.Th143.Stubs
{
    internal class ScoreStub : IScore
    {
        public ScoreStub() { }

        public ScoreStub(IScore score)
            : this()
        {
            this.ChallengeCounts = score.ChallengeCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.ClearCounts = score.ClearCounts?.ToDictionary(pair => pair.Key, pair => pair.Value);
            this.HighScore = score.HighScore;
            this.Number = score.Number;
            this.Checksum = score.Checksum;
            this.IsValid = score.IsValid;
            this.Signature = score.Signature;
            this.Size = score.Size;
            this.Version = score.Version;
        }

        public IReadOnlyDictionary<ItemWithTotal, int> ChallengeCounts { get; set; }

        public IReadOnlyDictionary<ItemWithTotal, int> ClearCounts { get; set; }

        public int HighScore { get; set; }

        public int Number { get; set; }

        public uint Checksum { get; set; }

        public bool IsValid { get; set; }

        public string Signature { get; set; }

        public int Size { get; set; }

        public ushort Version { get; set; }
    }
}
