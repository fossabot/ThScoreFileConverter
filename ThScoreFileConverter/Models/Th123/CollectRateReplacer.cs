﻿//-----------------------------------------------------------------------
// <copyright file="CollectRateReplacer.cs" company="None">
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

namespace ThScoreFileConverter.Models.Th123
{
    // %T123CRG[x][yy][z]
    internal class CollectRateReplacer : IStringReplaceable
    {
        private static readonly string Pattern = Utils.Format(
            @"%T123CRG({0})({1})([1-2])", Parsers.LevelWithTotalParser.Pattern, Parsers.CharaParser.Pattern);

        private readonly MatchEvaluator evaluator;

        public CollectRateReplacer(IReadOnlyDictionary<Chara, Th105.IClearData<Chara>> clearDataDictionary)
        {
            if (clearDataDictionary is null)
                throw new ArgumentNullException(nameof(clearDataDictionary));

            this.evaluator = new MatchEvaluator(match =>
            {
                var level = Parsers.LevelWithTotalParser.Parse(match.Groups[1].Value);
                var chara = Parsers.CharaParser.Parse(match.Groups[2].Value);
                var type = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);

                if ((chara != Chara.Sanae) && (chara != Chara.Cirno) && (chara != Chara.Meiling))
                    return match.ToString();

                Func<KeyValuePair<(Chara, int), Th105.ISpellCardResult<Chara>>, bool> findByLevel;
                if (level == Th105.LevelWithTotal.Total)
                    findByLevel = pair => true;
                else
                    findByLevel = pair => pair.Value.Level == (Th105.Level)level;

                Func<KeyValuePair<(Chara, int), Th105.ISpellCardResult<Chara>>, bool> countByType;
                if (type == 1)
                    countByType = pair => pair.Value.GotCount > 0;
                else
                    countByType = pair => pair.Value.TrialCount > 0;

                var spellCardResults = clearDataDictionary.TryGetValue(chara, out var clearData)
                    ? clearData.SpellCardResults : new Dictionary<(Chara, int), Th105.ISpellCardResult<Chara>>();
                return Utils.ToNumberString(spellCardResults.Where(findByLevel).Count(countByType));
            });
        }

        public string Replace(string input) => Regex.Replace(input, Pattern, this.evaluator, RegexOptions.IgnoreCase);
    }
}
