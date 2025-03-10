﻿//-----------------------------------------------------------------------
// <copyright file="CardAttackCareer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace ThScoreFileConverter.Models.Th08
{
    internal class CardAttackCareer : IBinaryReadable, ICardAttackCareer    // per story or practice
    {
        public IReadOnlyDictionary<CharaWithTotal, uint> MaxBonuses { get; private set; }

        public IReadOnlyDictionary<CharaWithTotal, int> TrialCounts { get; private set; }

        public IReadOnlyDictionary<CharaWithTotal, int> ClearCounts { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            if (reader == null)
                throw new ArgumentNullException(nameof(reader));

            var charas = Utils.GetEnumerator<CharaWithTotal>();
            this.MaxBonuses = charas.ToDictionary(chara => chara, _ => reader.ReadUInt32());
            this.TrialCounts = charas.ToDictionary(chara => chara, _ => reader.ReadInt32());
            this.ClearCounts = charas.ToDictionary(chara => chara, _ => reader.ReadInt32());
        }
    }
}
