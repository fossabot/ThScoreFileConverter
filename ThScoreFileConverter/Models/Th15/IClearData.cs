﻿//-----------------------------------------------------------------------
// <copyright file="IClearData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th15
{
    internal interface IClearData : Th095.IChapter
    {
        CharaWithTotal Chara { get; }

        IReadOnlyDictionary<GameMode, IClearDataPerGameMode> GameModeData { get; }

        IReadOnlyDictionary<(Level, StagePractice), Th13.IPractice> Practices { get; }
    }
}
