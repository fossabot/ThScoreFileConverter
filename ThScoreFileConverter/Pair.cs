﻿//-----------------------------------------------------------------------
// <copyright file="Pair.cs" company="None">
//     (c) 2013-2014 IIHOSHI Yoshinori
// </copyright>
//-----------------------------------------------------------------------

[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.LayoutRules",
    "SA1503:CurlyBracketsMustNotBeOmitted",
    Justification = "Reviewed.")]
[assembly: System.Diagnostics.CodeAnalysis.SuppressMessage(
    "StyleCop.CSharp.OrderingRules",
    "SA1201:ElementsMustAppearInTheCorrectOrder",
    Justification = "Reviewed.")]

namespace ThScoreFileConverter
{
    /// <summary>
    /// Pair of instances.
    /// </summary>
    /// <typeparam name="Type1">The type of the <see cref="First"/> property.</typeparam>
    /// <typeparam name="Type2">The type of the <see cref="Second"/> property.</typeparam>
    public class Pair<Type1, Type2>
    {
        /// <summary>
        /// Gets the value of the first component of the pair.
        /// </summary>
        protected Type1 First { get; private set; }

        /// <summary>
        /// Gets the value of the second component of the pair.
        /// </summary>
        protected Type2 Second { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="Pair{Type1,Type2}"/> class.
        /// </summary>
        /// <param name="first">The value of the first component of the pair.</param>
        /// <param name="second">The value of the second component of the pair.</param>
        protected Pair(Type1 first, Type2 second)
        {
            this.First = first;
            this.Second = second;
        }

        /// <summary>
        /// Determines whether the specified instance is equal to the current instance.
        /// </summary>
        /// <param name="obj">The instance to compare with the current instance.</param>
        /// <returns>
        /// <c>true</c> if <paramref name="obj"/> is equal to <c>this</c>; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            if ((obj == null) || (GetType() != obj.GetType()))
                return false;

            var target = (Pair<Type1, Type2>)obj;
            return this.First.Equals(target.First) && this.Second.Equals(target.Second);
        }

        /// <summary>
        /// Serves as the default hash function.
        /// </summary>
        /// <returns>A hash code for the current instance.</returns>
        public override int GetHashCode()
        {
            return this.First.GetHashCode() ^ this.Second.GetHashCode();
        }
    }
}
