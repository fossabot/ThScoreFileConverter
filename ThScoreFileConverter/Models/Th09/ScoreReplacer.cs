﻿//-----------------------------------------------------------------------
// <copyright file="ScoreReplacer.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;
using static ThScoreFileConverter.Models.Th09.Parsers;

namespace ThScoreFileConverter.Models.Th09
{
    // %T09SCR[w][xx][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T09SCR({0})({1})([1-5])([1-3])", LevelParser.Pattern, CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreReplacer(IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> rankings)
        {
            if (rankings is null)
                throw new ArgumentNullException(nameof(rankings));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = LevelParser.Parse(match.Groups[1].Value);
                var chara = CharaParser.Parse(match.Groups[2].Value);
                var rank = Utils.ToZeroBased(int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture));
                var type = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                var score = rankings.TryGetValue((chara, level), out var highScores) && (rank < highScores.Count)
                    ? highScores[rank] : null;
                var date = string.Empty;

                switch (type)
                {
                    case 1:     // name
                        return (score != null)
                            ? Encoding.Default.GetString(score.Name.ToArray()).Split('\0')[0] : "--------";
                    case 2:     // score
                        return (score != null)
                            ? Utils.ToNumberString((score.Score * 10) + score.ContinueCount) : "0";
                    case 3:     // date
                        date = (score != null)
                            ? Encoding.Default.GetString(score.Date.ToArray()).Split('\0')[0] : "--/--";
                        return (date != "--/--") ? date : "--/--/--";
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
