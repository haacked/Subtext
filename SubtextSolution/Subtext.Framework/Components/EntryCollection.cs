#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;
using System.Collections.Generic;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a collection of <see cref="Entry">Entry</see> Components.
	/// </summary>
	[Serializable]
	public class EntryCollection : List<Entry>
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntryCollection">EntryCollection</see> class.
		/// </summary>
		public EntryCollection() : base()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="EntryCollection">EntryCollection</see> class containing the specified array of <see cref="Entry">Entry</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="Entry">Entry</see> Components with which to initialize the collection. </param>
		public EntryCollection(Entry[] value)
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range from null.");

			this.AddRange(value);
		}
			
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="EntryCollection">EntryCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="BlogPostDayEnumerator">BlogPostDayEnumerator</see> for the <see cref="EntryCollection">EntryCollection</see> instance.</returns>
		public new BlogPostDayEnumerator GetEnumerator()	
		{
			return new BlogPostDayEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="EntryCollection">EntryCollection</see>.
		/// </summary>
		public class BlogPostDayEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="BlogPostDayEnumerator">BlogPostDayEnumerator</see> class referencing the specified <see cref="EntryCollection">EntryCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="EntryCollection">EntryCollection</see> to enumerate.</param>
			public BlogPostDayEnumerator(EntryCollection mappings)
			{
				_temp = mappings;
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public Entry Current
			{
				get {return ((Entry)(_enumerator.Current));}
			}
			
			object IEnumerator.Current
			{
				get {return _enumerator.Current;}
			}
			
			/// <summary>
			/// Advances the enumerator to the next element of the collection.
			/// </summary>
			/// <returns><b>true</b> if the enumerator was successfully advanced to the next element; <b>false</b> if the enumerator has passed the end of the collection.</returns>
			public bool MoveNext()
			{
				return _enumerator.MoveNext();
			}
			
			bool IEnumerator.MoveNext()
			{
				return _enumerator.MoveNext();
			}
			
			/// <summary>
			/// Sets the enumerator to its initial position, which is before the first element in the collection.
			/// </summary>
			public void Reset()
			{
				_enumerator.Reset();
			}
			
			void IEnumerator.Reset() 
			{
				_enumerator.Reset();
			}
		}
	}
}