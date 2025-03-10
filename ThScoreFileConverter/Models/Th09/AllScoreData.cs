﻿//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.Collections.Generic;
using System.Linq;

namespace ThScoreFileConverter.Models.Th09
{
    internal class AllScoreData
    {
        private readonly Dictionary<(Chara, Level), IReadOnlyList<IHighScore>> rankings;

        public AllScoreData()
        {
            var numPairs = Enum.GetValues(typeof(Chara)).Length * Enum.GetValues(typeof(Level)).Length;
            this.rankings = new Dictionary<(Chara, Level), IReadOnlyList<IHighScore>>(numPairs);
        }

        public Header Header { get; private set; }

        public IReadOnlyDictionary<(Chara, Level), IReadOnlyList<IHighScore>> Rankings => this.rankings;

        public IPlayStatus PlayStatus { get; private set; }

        public Th07.LastName LastName { get; private set; }

        public Th07.VersionInfo VersionInfo { get; private set; }

        public void Set(Header header) => this.Header = header;

        public void Set(IHighScore score)
        {
            var key = (score.Chara, score.Level);
            if (!this.rankings.ContainsKey(key))
                this.rankings.Add(key, new IHighScore[5].ToList());
            if ((score.Rank >= 0) && (score.Rank < 5))
            {
                var ranking = this.rankings[key] as List<IHighScore>;
                ranking[score.Rank] = score;
            }
        }

        public void Set(IPlayStatus status) => this.PlayStatus = status;

        public void Set(Th07.LastName name) => this.LastName = name;

        public void Set(Th07.VersionInfo info) => this.VersionInfo = info;
    }
}
