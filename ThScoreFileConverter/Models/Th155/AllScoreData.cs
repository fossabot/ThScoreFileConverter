﻿//-----------------------------------------------------------------------
// <copyright file="AllScoreData.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System.Collections.Generic;
using System.IO;
using System.Linq;
using ThScoreFileConverter.Squirrel;

namespace ThScoreFileConverter.Models.Th155
{
    internal class AllScoreData : IBinaryReadable
    {
        private SQTable allData;

        public AllScoreData()
        {
            this.allData = null;
            this.StoryDictionary = null;
            this.BgmDictionary = null;
            this.EndingDictionary = null;
            this.StageDictionary = null;
        }

        public IReadOnlyDictionary<StoryChara, Story> StoryDictionary { get; private set; }

        public IReadOnlyDictionary<string, int> CharacterDictionary { get; private set; }

        public IReadOnlyDictionary<int, bool> BgmDictionary { get; private set; }

        public IReadOnlyDictionary<string, int> EndingDictionary { get; private set; }

        public IReadOnlyDictionary<int, int> StageDictionary { get; private set; }

        public int Version { get; private set; }

        public void ReadFrom(BinaryReader reader)
        {
            this.allData = SQTable.Create(reader, true);

            this.ParseStoryDictionary();
            this.ParseCharacterDictionary();
            this.ParseBgmDictionary();
            this.ParseEndingDictionary();
            this.ParseStageDictionary();
            this.ParseVersion();
        }

        private static StoryChara? ParseStoryChara(SQObject obj)
        {
            if (obj is SQString str)
            {
                switch (str)
                {
                    case "reimu":
                        return StoryChara.ReimuKasen;
                    case "marisa":
                        return StoryChara.MarisaKoishi;
                    case "nitori":
                        return StoryChara.NitoriKokoro;
                    case "usami":
                        return StoryChara.SumirekoDoremy;
                    case "tenshi":
                        return StoryChara.TenshiShinmyoumaru;
                    case "miko":
                        return StoryChara.MikoByakuren;
                    case "yukari":
                        return StoryChara.YukariReimu;
                    case "mamizou":
                        return StoryChara.MamizouMokou;
                    case "udonge":
                        return StoryChara.ReisenDoremy;
                    case "futo":
                        return StoryChara.FutoIchirin;
                    case "jyoon":
                        return StoryChara.JoonShion;
                    default:
                        return null;
                }
            }
            else
            {
                return null;
            }
        }

        private static Story ParseStory(SQObject obj)
        {
            var story = default(Story);

            if (obj is SQTable dict)
            {
                foreach (var pair in dict.Value)
                {
                    if (pair.Key is SQString key)
                    {
                        if ((key == "stage") && (pair.Value is SQInteger stage))
                            story.Stage = stage;
                        if ((key == "ed") && (pair.Value is SQInteger ed))
                            story.Ed = (LevelFlags)(int)ed;
                        if ((key == "available") && (pair.Value is SQBool available))
                            story.Available = available;
                        if ((key == "overdrive") && (pair.Value is SQInteger overDrive))
                            story.OverDrive = overDrive;
                        if ((key == "stage_overdrive") && (pair.Value is SQInteger stageOverDrive))
                            story.StageOverDrive = stageOverDrive;
                    }
                }
            }

            return story;
        }

        private void ParseStoryDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("story"), out var story))
            {
                if (story is SQTable table)
                {
                    this.StoryDictionary = table.Value
                        .Where(pair => ParseStoryChara(pair.Key) != null)
                        .ToDictionary(pair => ParseStoryChara(pair.Key).Value, pair => ParseStory(pair.Value));
                }
            }
        }

        private void ParseCharacterDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("character"), out var character))
            {
                if (character is SQTable table)
                {
                    this.CharacterDictionary = table.Value
                        .Where(pair => (pair.Key is SQString) && (pair.Value is SQInteger))
                        .ToDictionary(
                            pair => (string)(pair.Key as SQString),
                            pair => (int)(pair.Value as SQInteger));
                }
            }
        }

        private void ParseBgmDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("bgm"), out var bgm))
            {
                if (bgm is SQTable table)
                {
                    this.BgmDictionary = table.Value
                        .Where(pair => (pair.Key is SQInteger) && (pair.Value is SQBool))
                        .ToDictionary(pair => (int)(pair.Key as SQInteger), pair => (bool)(pair.Value as SQBool));
                }
            }
        }

        private void ParseEndingDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("ed"), out var ed))
            {
                if (ed is SQTable table)
                {
                    this.EndingDictionary = table.Value
                        .Where(pair => (pair.Key is SQString) && (pair.Value is SQInteger))
                        .ToDictionary(
                            pair => (string)(pair.Key as SQString),
                            pair => (int)(pair.Value as SQInteger));
                }
            }
        }

        private void ParseStageDictionary()
        {
            if (this.allData.Value.TryGetValue(new SQString("stage"), out var stage))
            {
                if (stage is SQTable table)
                {
                    this.StageDictionary = table.Value
                        .Where(pair => (pair.Key is SQInteger) && (pair.Value is SQInteger))
                        .ToDictionary(pair => (int)(pair.Key as SQInteger), pair => (int)(pair.Value as SQInteger));
                }
            }
        }

        private void ParseVersion() => this.Version = this.GetValue<int>("version");

        private T GetValue<T>(string key)
            where T : struct
        {
            T result = default;

            if (this.allData.Value.TryGetValue(new SQString(key), out var value))
            {
                switch (value)
                {
                    case SQBool sqbool:
                        if (result is bool)
                            result = (T)(object)(bool)sqbool;
                        break;
                    case SQInteger sqinteger:
                        if (result is int)
                            result = (T)(object)(int)sqinteger;
                        break;
                }
            }

            return result;
        }

        public struct Story
        {
            public int Stage;
            public LevelFlags Ed;
            public bool Available;
            public int OverDrive;
            public int StageOverDrive;
        }
    }
}
