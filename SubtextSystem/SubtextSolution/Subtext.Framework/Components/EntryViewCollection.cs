#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a collection of <see cref="EntryView">EntryView</see> Components.
	/// </summary>
	[Serializable]
	public class EntryViewCollection: CollectionBase
	{

		/// <summary>
		/// Initializes a new instance of the <see cref="EntryViewCollection">EntryViewCollection</see> class.
		/// </summary>
		public EntryViewCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="EntryViewCollection">EntryViewCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="EntryViewCollection">EntryViewCollection</see> with which to initialize the collection.</param>
		public EntryViewCollection(EntryViewCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="EntryViewCollection">EntryViewCollection</see> class containing the specified array of <see cref="EntryView">EntryView</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="EntryView">EntryView</see> Components with which to initialize the collection. </param>
		public EntryViewCollection(EntryView[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="EntryViewCollection">EntryViewCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="EntryViewCollection">EntryViewCollection</see> class.
		/// </para>
		/// </summary>
		public EntryView this[int index] 
		{
			get	{return ((EntryView)(this.List[index]));}
		}
		
		public int Add(EntryView value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="EntryView">EntryView</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="EntryView">EntryView</see> containing the Components to add to the collection.</param>
		public void AddRange(EntryView[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="EntryViewCollection">EntryViewCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="EntryViewCollection">EntryViewCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(EntryViewCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((EntryView)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="EntryViewCollection">EntryViewCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="EntryViewCollection">EntryViewCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(EntryView value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(EntryView[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="EntryViewCollection">EntryViewCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="EntryViewCollection">EntryViewCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(EntryView value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, EntryView value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(EntryView value) 
		{
			List.Remove(value);
		}

		public object SyncRoot
		{
			get
			{
				return this;
			}
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="EntryViewCollection">EntryViewCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="EntryViewCollectionEnumerator">EntryViewCollectionEnumerator</see> for the <see cref="EntryViewCollection">EntryViewCollection</see> instance.</returns>
		public new EntryViewCollectionEnumerator GetEnumerator()	
		{
			return new EntryViewCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="EntryViewCollection">EntryViewCollection</see>.
		/// </summary>
		public class EntryViewCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="EntryViewCollectionEnumerator">EntryViewCollectionEnumerator</see> class referencing the specified <see cref="EntryViewCollection">EntryViewCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="EntryViewCollection">EntryViewCollection</see> to enumerate.</param>
			public EntryViewCollectionEnumerator(EntryViewCollection mappings)
			{
				_temp =	mappings;
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public EntryView Current
			{
				get {return ((EntryView)(_enumerator.Current));}
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

