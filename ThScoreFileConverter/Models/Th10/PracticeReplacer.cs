﻿//-----------------------------------------------------------------------
// <copyright file="PracticeReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace ThScoreFileConverter.Models.Th10
{
    // %T10PRAC[x][yy][z]
    internal class PracticeReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T10PRAC({0})({1})({2})",
            Parsers.LevelParser.Pattern,
            Parsers.CharaParser.Pattern,
            Parsers.StageParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public PracticeReplacer(
            IReadOnlyDictionary<CharaWithTotal, IClearData<CharaWithTotal, StageProgress>> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelParser.Parse(match.Groups[1].Value);
                var chara = (CharaWithTotal)Parsers.CharaParser.Parse(match.Groups[2].Value);
                var stage = Parsers.StageParser.Parse(match.Groups[3].Value);

                if (level == Level.Extra)
                    return match.ToString();
                if (stage == Stage.Extra)
                    return match.ToString();

                return clearDataDictionary.TryGetValue(chara, out var clearData)
                    && clearData.Practices.TryGetValue((level, stage), out var practice)
                    ? Utils.ToNumberString(practice.Score * 10) : "0";
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
