///////////////////////////////////////////////////////////////////////////////////////
// Copyright (C) 2006-2017 Esper Team. All rights reserved.                           /
// http://esper.codehaus.org                                                          /
// ---------------------------------------------------------------------------------- /
// The software in this package is published under the terms of the GPL license       /
// a copy of which has been included with this distribution in the license.txt file.  /
///////////////////////////////////////////////////////////////////////////////////////


using System;

using com.espertech.esper.util;

namespace com.espertech.esper.collection
{
	/// <summary> General-purpose pair of values of any type. The pair only equals another pair if
	/// the objects that form the pair equal, ie. first pair first object equals (.equals) the second pair first object,
	/// and the first pair second object equals the second pair second object.
	/// </summary>
    
    [Serializable]
    public sealed class UniformPair<T> : MetaDefItem
	{
	    /// <summary>
	    /// Gets or sets the first value within pair.
	    /// </summary>
	    /// <value>The first.</value>
	    public T First { get; set; }

	    /// <summary>
	    /// Gets or sets the second value within pair.
	    /// </summary>
	    /// <value>The second.</value>
	    public T Second { get; set; }

	    /// <summary>
        /// Construct pair of values.
        /// </summary>
        /// <param name="first">is the first value</param>
        /// <param name="second">is the second value</param>

        public UniformPair(T first, T second)
		{
			this.First = first;
			this.Second = second;
		}

        /// <summary>
        /// Determines whether the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <param name="obj">The <see cref="T:System.Object"></see> to compare with the current <see cref="T:System.Object"></see>.</param>
        /// <returns>
        /// true if the specified <see cref="T:System.Object"></see> is equal to the current <see cref="T:System.Object"></see>; otherwise, false.
        /// </returns>
		public  override bool Equals(Object obj)
		{
			if (this == obj)
			{
				return true;
			}
			
			if (!(obj is UniformPair<T>))
			{
				return false;
			}
			
			UniformPair<T> other = (UniformPair<T>) obj;
			
			return
				(First == null?other.First == null:First.Equals(other.First)) && 
				(Second == null?other.Second == null:Second.Equals(other.Second));
		}

        /// <summary>
        /// Serves as a hash function for a particular type.
        /// </summary>
        /// <returns>
        /// A hash code for the current <see cref="T:System.Object"></see>.
        /// </returns>
		public override int GetHashCode()
		{
			return (First == null?0:First.GetHashCode()) ^ (Second == null?0:Second.GetHashCode());
		}

        /// <summary>
        /// Returns a <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> that represents the current <see cref="T:System.Object"></see>.
        /// </returns>
		public override String ToString()
		{
			return "Pair [" + First + ':' + Second + ']';
		}
	}
}
