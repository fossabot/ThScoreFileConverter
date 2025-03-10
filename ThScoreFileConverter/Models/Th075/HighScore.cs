﻿//-----------------------------------------------------------------------
// <copyright file="HighScore.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Extensions;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Models.Th075
{
    internal class HighScore : IBinaryReadable, IHighScore
    {
        public HighScore()
        {
        }

        public string Name { get; private set; }

        public byte Month { get; private set; }     // 1-based

        public byte Day { get; private set; }       // 1-based

        public int Score { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.Name = new string(reader.ReadExactBytes(8).Select(ch => Definitions.CharTable[ch]).ToArray());

            this.Month = reader.ReadByte();
            this.Day = reader.ReadByte();
            if ((this.Month == 0) && (this.Day == 0))
            {
                // It's allowed.
            }
            else
            {
                if ((this.Month <= 0) || (this.Month > 12))
                {
                    throw new InvalidDataException(
                        Utils.Format(Resources.InvalidDataExceptionPropertyIsOutOfRange, nameof(this.Month)));
                }

                if ((this.Day <= 0) || (this.Day > DateTime.DaysInMonth(2000, this.Month)))
                {
                    throw new InvalidDataException(
                        Utils.Format(Resources.InvalidDataExceptionPropertyIsOutOfRange, nameof(this.Day)));
                }
            }

            reader.ReadUInt16();    // always 0x0000?
            this.Score = reader.ReadInt32();
        }
    }
}
