﻿//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th10
{
    internal interface IClearData<TCharaWithTotal, TStageProgress> : Th095.IChapter
        where TCharaWithTotal : struct, Enum
        where TStageProgress : struct, Enum
    {
        IReadOnlyDictionary<int, ISpellCard<Level>> Cards { get; }

        TCharaWithTotal Chara { get; }

        IReadOnlyDictionary<Level, int> ClearCounts { get; }

        int PlayTime { get; }

        IReadOnlyDictionary<(Level, Stage), IPractice> Practices { get; }

        IReadOnlyDictionary<Level, IReadOnlyList<IScoreData<TStageProgress>>> Rankings { get; }

        int TotalPlayCount { get; }
    }
}
