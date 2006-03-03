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

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a collection of <see cref="EntryDay">EntryDay</see> Components.
	/// </summary>
	[Serializable]
	public class EntryDayCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="EntryDayCollection">EntryDayCollection</see> class.
		/// </summary>
		public EntryDayCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="EntryDayCollection">EntryDayCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="EntryDayCollection">EntryDayCollection</see> with which to initialize the collection.</param>
		public EntryDayCollection(EntryDayCollection value)	
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="EntryDayCollection">EntryDayCollection</see> class containing the specified array of <see cref="EntryDay">EntryDay</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="EntryDay">EntryDay</see> Components with which to initialize the collection. </param>
		public EntryDayCollection(EntryDay[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="EntryDayCollection">EntryDayCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="EntryDayCollection">EntryDayCollection</see> class.
		/// </para>
		/// </summary>
		public EntryDay this[int index] 
		{
			get	{return ((EntryDay)(this.List[index]));}
		}
		
		public int Add(EntryDay value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="EntryDay">EntryDay</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="EntryDay">EntryDay</see> containing the Components to add to the collection.</param>
		public void AddRange(EntryDay[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="EntryDayCollection">EntryDayCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="EntryDayCollection">EntryDayCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(EntryDayCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((EntryDay)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="EntryDayCollection">EntryDayCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="EntryDayCollection">EntryDayCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(EntryDay value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(EntryDay[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="EntryDayCollection">EntryDayCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="EntryDayCollection">EntryDayCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(EntryDay value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, EntryDay value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(EntryDay value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="EntryDayCollection">EntryDayCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="BlogPostDayCollectionEnumerator">BlogPostDayCollectionEnumerator</see> for the <see cref="EntryDayCollection">EntryDayCollection</see> instance.</returns>
		public new BlogPostDayCollectionEnumerator GetEnumerator()	
		{
			return new BlogPostDayCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="EntryDayCollection">EntryDayCollection</see>.
		/// </summary>
		public class BlogPostDayCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="BlogPostDayCollectionEnumerator">BlogPostDayCollectionEnumerator</see> class referencing the specified <see cref="EntryDayCollection">EntryDayCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="EntryDayCollection">EntryDayCollection</see> to enumerate.</param>
			public BlogPostDayCollectionEnumerator(EntryDayCollection mappings)
			{
				_temp =	mappings;
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public EntryDay Current
			{
				get {return ((EntryDay)(_enumerator.Current));}
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

