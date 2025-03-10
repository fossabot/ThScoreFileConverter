﻿//-----------------------------------------------------------------------
// <copyright file="ClearCount.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThScoreFileConverter.Models.Th09
{
    internal class ClearCount : IBinaryReadable, IClearCount
    {
        public IReadOnlyDictionary<Level, int> Counts { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            this.Counts = Utils.GetEnumerator<Level>().ToDictionary(level => level, _ => reader.ReadInt32());
            reader.ReadUInt32();
        }
    }
}
