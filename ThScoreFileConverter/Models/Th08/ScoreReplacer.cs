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
using ThScoreFileConverter.Extensions;
using static ThScoreFileConverter.Models.Th08.Parsers;
using IHighScore = ThScoreFileConverter.Models.Th08.IHighScore<
    ThScoreFileConverter.Models.Th08.Chara,
    ThScoreFileConverter.Models.Level,
    ThScoreFileConverter.Models.Th08.StageProgress>;

namespace ThScoreFileConverter.Models.Th08
{
    // %T08SCR[w][xx][y][z]
    internal class ScoreReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T08SCR({0})({1})(\d)([\dA-G])", LevelParser.Pattern, CharaParser.Pattern);

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
                var type = match.Groups[4].Value.ToUpperInvariant();

                var key = (chara, level);
                var score = (rankings.TryGetValue(key, out var ranking) && (rank < ranking.Count))
                    ? ranking[rank] : Definitions.InitialRanking[rank];
                IEnumerable<string> cardStrings;

                switch (type)
                {
                    case "1":   // name
                        return Encoding.Default.GetString(score.Name.ToArray()).Split('\0')[0];
                    case "2":   // score
                        return Utils.ToNumberString((score.Score * 10) + score.ContinueCount);
                    case "3":   // stage
                        if ((level == Level.Extra) &&
                            (Encoding.Default.GetString(score.Date.ToArray()).TrimEnd('\0') == "--/--"))
                            return StageProgress.Extra.ToShortName();
                        else
                            return score.StageProgress.ToShortName();
                    case "4":   // date
                        return Encoding.Default.GetString(score.Date.ToArray()).TrimEnd('\0');
                    case "5":   // slow rate
                        return Utils.Format("{0:F3}%", score.SlowRate);
                    case "6":   // play time
                        return new Time(score.PlayTime).ToString();
                    case "7":   // initial number of players
                        return (score.PlayerNum + 1).ToString(CultureInfo.CurrentCulture);
                    case "8":   // point items
                        return Utils.ToNumberString(score.PointItem);
                    case "9":   // time point
                        return Utils.ToNumberString(score.TimePoint);
                    case "0":   // miss count
                        return score.MissCount.ToString(CultureInfo.CurrentCulture);
                    case "A":   // bomb count
                        return score.BombCount.ToString(CultureInfo.CurrentCulture);
                    case "B":   // last spell count
                        return score.LastSpellCount.ToString(CultureInfo.CurrentCulture);
                    case "C":   // pause count
                        return Utils.ToNumberString(score.PauseCount);
                    case "D":   // continue count
                        return score.ContinueCount.ToString(CultureInfo.CurrentCulture);
                    case "E":   // human rate
                        return Utils.Format("{0:F2}%", score.HumanRate / 100.0);
                    case "F":   // got spell cards
                        cardStrings = score.CardFlags.Where(pair => pair.Value > 0).Select(pair =>
                        {
                            return Definitions.CardTable.TryGetValue(pair.Key, out var card)
                                ? Utils.Format("No.{0:D3} {1}", card.Id, card.Name) : string.Empty;
                        });
                        return string.Join(Environment.NewLine, cardStrings.ToArray());
                    case "G":   // number of got spell cards
                        return score.CardFlags.Values.Count(flag => flag > 0).ToString(CultureInfo.CurrentCulture);
                    default:    // unreachable
                        return match.ToString();
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
