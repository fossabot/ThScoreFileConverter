﻿//-----------------------------------------------------------------------
// <copyright file="SQBool.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

#pragma warning disable SA1600 // Elements should be documented

using System;
using System.IO;
using ThScoreFileConverter.Properties;

namespace ThScoreFileConverter.Squirrel
{
    internal sealed class SQBool : SQObject, IEquatable<SQBool>
    {
        private SQBool(bool value = default)
            : base(SQObjectType.Bool)
            => this.Value = value;

        public static SQBool True { get; } = new SQBool(true);

        public static SQBool False { get; } = new SQBool(false);

        public new bool Value
        {
            get => (bool)base.Value;
            private set => base.Value = value;
        }

        public static implicit operator bool(SQBool sq) => sq.Value;

        public static SQBool Create(BinaryReader reader, bool skipType = false)
        {
            if (reader is null)
                throw new ArgumentNullException(nameof(reader));

            if (!skipType)
            {
                var type = reader.ReadInt32();
                if (type != (int)SQObjectType.Bool)
                    throw new InvalidDataException(Resources.InvalidDataExceptionWrongType);
            }

            return (reader.ReadByte() != 0x00) ? True : False;
        }

        public override bool Equals(object obj) => this.Equals(obj as SQBool);

        public override int GetHashCode() => this.Type.GetHashCode() ^ this.Value.GetHashCode();

        public bool Equals(SQBool other)
        {
            if (other is null)
                return false;

            return (this.Type == other.Type) && (this.Value == other.Value);
        }
    }
}
