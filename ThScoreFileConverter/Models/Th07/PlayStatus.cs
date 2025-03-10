﻿//-----------------------------------------------------------------------
// <copyright file="PlayStatus.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThScoreFileConverter.Models.Th07
{
    internal class PlayStatus : Th06.Chapter
    {
        public const string ValidSignature = "PLST";
        public const short ValidSize = 0x0160;

        public PlayStatus(Th06.Chapter chapter)
            : base(chapter, ValidSignature, ValidSize)
        {
            using (var reader = new BinaryReader(new MemoryStream(this.Data, false)))
            {
                reader.ReadUInt32();    // always 0x00000001?
                var hours = reader.ReadInt32();
                var minutes = reader.ReadInt32();
                var seconds = reader.ReadInt32();
                var milliseconds = reader.ReadInt32();
                this.TotalRunningTime = new Time(hours, minutes, seconds, milliseconds, false);
                hours = reader.ReadInt32();
                minutes = reader.ReadInt32();
                seconds = reader.ReadInt32();
                milliseconds = reader.ReadInt32();
                this.TotalPlayTime = new Time(hours, minutes, seconds, milliseconds, false);
                this.PlayCounts = Utils.GetEnumerator<LevelWithTotal>().ToDictionary(level => level, level =>
                {
                    var playCount = new PlayCount();
                    playCount.ReadFrom(reader);
                    return playCount;
                });
            }
        }

        public Time TotalRunningTime { get; }

        public Time TotalPlayTime { get; }

        public IReadOnlyDictionary<LevelWithTotal, PlayCount> PlayCounts { get; }
    }
}
