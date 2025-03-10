﻿//-----------------------------------------------------------------------
// <copyright file="ScoreTotalReplacer.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th125
{
    // %T125SCRTL[x][y][z]
    internal class ScoreTotalReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T125SCRTL({0})([12])([1-5])", Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public ScoreTotalReplacer(IReadOnlyList<IScore> scores)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var chara = Parsers.CharaParser.Parse(match.Groups[1].Value);
                var method = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                bool TriedAndSucceeded(IScore score)
                    => IsTarget(score, chara, method) && (score.TrialCount > 0) && (score.FirstSuccess > 0);

                switch (type)
                {
                    case 1:     // total score
                        return Utils.ToNumberString(
                            scores.Sum(score => TriedAndSucceeded(score) ? score.HighScore : 0L));
                    case 2:     // total of bestshot scores
                        return Utils.ToNumberString(
                            scores.Sum(score => IsTarget(score, chara, method) ? score.BestshotScore : 0L));
                    case 3:     // total of num of shots
                        return Utils.ToNumberString(
                            scores.Sum(score => IsTarget(score, chara, method) ? score.TrialCount : 0));
                    case 4:     // total of num of shots for the first success
                        return Utils.ToNumberString(
                            scores.Sum(score => TriedAndSucceeded(score) ? score.FirstSuccess : 0L));
                    case 5:     // num of succeeded scenes
                        return scores.Count(TriedAndSucceeded).ToString(CultureInfo.CurrentCulture);
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);

        private static bool IsTarget(IScore score, Chara chara, int method)
        {
            if (score == null)
                return false;

            if (method == 2)
                return score.Chara == chara;

            if (score.LevelScene.Level != Level.Spoiler)
                return score.Chara == chara;

            if (chara == Chara.Hatate)
                return false;

            return score.Chara == (score.LevelScene.Scene <= 4 ? Chara.Aya : Chara.Hatate);
        }
    }
}
