﻿//-----------------------------------------------------------------------
// <copyright file="CardReplacer.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th143
{
    // %T143CARD[x][y][z]
    internal class CardReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T143CARD({0})([0-9])([12])", Parsers.DayParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CardReplacer(IReadOnlyList<IScore> scores, bool hideUntriedCards)
        {
            if (scores is null)
                throw new ArgumentNullException(nameof(scores));

            this.evaluator = new MatchEvaluator(match =>
            {
                var day = Parsers.DayParser.Parse(match.Groups[1].Value);
                var scene = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                scene = (scene == 0) ? 10 : scene;
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                var key = (day, scene);
                if (!Definitions.SpellCards.TryGetValue(key, out var enemyCardPair))
                    return match.ToString();

                if (hideUntriedCards)
                {
                    var score = scores.FirstOrDefault(elem =>
                        (elem != null) &&
                        (elem.Number > 0) &&
                        (elem.Number <= Definitions.SpellCards.Count) &&
                        Definitions.SpellCards.ElementAt(elem.Number - 1).Key.Equals(key));
                    if ((score == null) ||
                        !score.ChallengeCounts.TryGetValue(ItemWithTotal.Total, out var count) ||
                        (count <= 0))
                        return "??????????";
                }

                if (type == 1)
                {
                    return string.Join(" &amp; ", enemyCardPair.Enemies.Select(enemy => enemy.ToLongName()).ToArray());
                }
                else
                {
                    return enemyCardPair.Card;
                }
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
