﻿//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;

namespace ThScoreFileConverter.Models.Th125
{
    internal class AllScoreData
    {
        private readonly List<IScore> scores;

        public AllScoreData() => this.scores = new List<IScore>(Definitions.SpellCards.Count);

        public Th095.HeaderBase Header { get; private set; }

        public IReadOnlyList<IScore> Scores => this.scores;

        public IStatus Status { get; private set; }

        public void Set(Th095.HeaderBase header) => this.Header = header;

        public void Set(IScore score) => this.scores.Add(score);

        public void Set(IStatus status) => this.Status = status;
    }
}
