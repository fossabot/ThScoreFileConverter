﻿//-----------------------------------------------------------------------
// <copyright file="BestShotHeader.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using ThScoreFileConverter.Extensions;

namespace ThScoreFileConverter.Models.Th095
{
    internal class BestShotHeader : IBinaryReadable, IBestShotHeader
    {
        public const string ValidSignature = "BSTS";
        public const int SignatureSize = 4;

        public string Signature { get; private set; }

        public Level Level { get; private set; }

        public short Scene { get; private set; }    // 1-based

        public short Width { get; private set; }

        public short Height { get; private set; }

        public int ResultScore { get; private set; }

        public float SlowRate { get; private set; }

        public IEnumerable<byte> CardName { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            this.Signature = Encoding.Default.GetString(reader.ReadExactBytes(SignatureSize));
            if (!this.Signature.Equals(ValidSignature, StringComparison.Ordinal))
                throw new InvalidDataException();

            reader.ReadUInt16();
            this.Level = Utils.ToEnum<Level>(reader.ReadInt16() - 1);
            this.Scene = reader.ReadInt16();
            reader.ReadUInt16();    // 0x0102 ... Version?
            this.Width = reader.ReadInt16();
            this.Height = reader.ReadInt16();
            this.ResultScore = reader.ReadInt32();
            this.SlowRate = reader.ReadSingle();
            this.CardName = reader.ReadExactBytes(0x50);
        }
    }
}
