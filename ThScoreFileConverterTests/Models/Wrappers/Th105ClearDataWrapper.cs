﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using ThScoreFileConverter.Models;
using CardForDeck = ThScoreFileConverter.Models.Th105.CardForDeck;

namespace ThScoreFileConverterTests.Models.Wrappers
{
    // NOTE: Setting the accessibility as public causes CS0050, CS0051 and CS0703.
    internal sealed class Th105ClearDataWrapper<TParent, TChara, TLevel>
        where TParent : ThConverter
        where TChara : struct, Enum
        where TLevel : struct, Enum
    {
        private static readonly Type ParentType = typeof(TParent);
        private static readonly string AssemblyNameToTest = ParentType.Assembly.GetName().Name;
        private static readonly string TypeNameToTest = ParentType.FullName + "+ClearData";

        private readonly PrivateObject pobj = null;

        [SuppressMessage("Microsoft.Design", "CA1000:DoNotDeclareStaticMembersOnGenericTypes")]
        public static Th105ClearDataWrapper<TParent, TChara, TLevel> Create(byte[] array)
        {
            var clearData = new Th105ClearDataWrapper<TParent, TChara, TLevel>();

            MemoryStream stream = null;
            try
            {
                stream = new MemoryStream(array);
                using (var reader = new BinaryReader(stream))
                {
                    stream = null;
                    clearData.ReadFrom(reader);
                }
            }
            finally
            {
                stream?.Dispose();
            }

            return clearData;
        }

        public Th105ClearDataWrapper()
            => this.pobj = new PrivateObject(AssemblyNameToTest, TypeNameToTest);
        public Th105ClearDataWrapper(object obj)
            => this.pobj = new PrivateObject(obj);

        [SuppressMessage("Microsoft.Performance", "CA1811:AvoidUncalledPrivateCode")]
        public object Target
            => this.pobj.Target;
        public int? Id
            => this.pobj.GetProperty(nameof(this.Id)) as int?;
        public int? MaxNumber
            => this.pobj.GetProperty(nameof(this.MaxNumber)) as int?;
        public IReadOnlyDictionary<int, CardForDeck> CardsForDeck
            => this.pobj.GetProperty(nameof(this.CardsForDeck)) as Dictionary<int, CardForDeck>;
        // NOTE: Th{105,123}Converter.SpellCardResult are private classes.
        // public IReadOnlyDictionary<(TChara Chara, int CardId), SpellCardResult> SpellCardResults
        //     => this.pobj.GetProperty(nameof(this.SpellCardResults)) as Dictionary<(TChara, int), SpellCardResult>;
        public object SpellCardResults
            => this.pobj.GetProperty(nameof(this.SpellCardResults));
        public Th105SpellCardResultWrapper<TParent, TChara, TLevel> SpellCardResultsItem(TChara chara, int cardId)
            => new Th105SpellCardResultWrapper<TParent, TChara, TLevel>(
                this.SpellCardResults.GetType().GetProperty("Item").GetValue(
                    this.SpellCardResults, new object[] { (chara, cardId) }));

        public void ReadFrom(BinaryReader reader)
            => this.pobj.Invoke(nameof(this.ReadFrom), new object[] { reader }, CultureInfo.InvariantCulture);
    }
}
