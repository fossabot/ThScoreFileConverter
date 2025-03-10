﻿//-----------------------------------------------------------------------
// <copyright file="ExceptionOccurredEventArgs.cs" company="None">
// Copyright (c) IIHOSHI Yoshinori.
// Licensed under the BSD-2-Clause license. See LICENSE.txt file in the project root for full license information.
// </copyright>
//-----------------------------------------------------------------------

using System;
using System.Diagnostics.CodeAnalysis;

namespace ThScoreFileConverter.Models
{
    /// <summary>
    /// Represents the event data that indicates occurring of an exception.
    /// </summary>
#if DEBUG
    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses", Justification = "Reviewed.")]
#endif
    internal class ExceptionOccurredEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExceptionOccurredEventArgs"/> class.
        /// </summary>
        /// <param name="ex">The exception data.</param>
        public ExceptionOccurredEventArgs(Exception ex) => this.Exception = ex;

        /// <summary>
        /// Gets the exception data.
        /// </summary>
        public Exception Exception { get; }
    }
}
