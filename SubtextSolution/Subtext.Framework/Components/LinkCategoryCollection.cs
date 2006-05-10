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
	/// Represents a collection of <see cref="LinkCategory">LinkCategory</see> Components.
	/// </summary>
	[Serializable]
	public class LinkCategoryCollection: CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkCategoryCollection">LinkCategoryCollection</see> class.
		/// </summary>
		public LinkCategoryCollection() 
		{
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkCategoryCollection">LinkCategoryCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="LinkCategoryCollection">LinkCategoryCollection</see> with which to initialize the collection.</param>
		public LinkCategoryCollection(LinkCategoryCollection value)	
		{
			if(value != null)
			{
				this.AddRange(value);
			}
		}
		
		/// <summary>
		/// Initializes a new instance of the <see cref="LinkCategoryCollection">LinkCategoryCollection</see> class containing the specified array of <see cref="LinkCategory">LinkCategory</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="LinkCategory">LinkCategory</see> Components with which to initialize the collection. </param>
		public LinkCategoryCollection(LinkCategory[] value)
		{
			if(value != null)
			{
				this.AddRange(value);
			}
		}
		
		/// <summary>
		/// Gets the <see cref="LinkCategoryCollection">LinkCategoryCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="LinkCategoryCollection">LinkCategoryCollection</see> class.
		/// </para>
		/// </summary>
		public LinkCategory this[int index] 
		{
			get	{return ((LinkCategory)(this.List[index]));}
		}
		
		public int Add(LinkCategory value) 
		{
			if(value != null)
			{
				return this.List.Add(value);
			}
			return -1;
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="LinkCategory">LinkCategory</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="LinkCategory">LinkCategory</see> containing the Components to add to the collection.</param>
		public void AddRange(LinkCategory[] value) 
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range from null.");

			if(value != null)
			{
				for (int i = 0;	(i < value.Length); i = (i + 1)) 
				{
					this.Add(value[i]);
				}
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="LinkCategoryCollection">LinkCategoryCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="LinkCategoryCollection">LinkCategoryCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(LinkCategoryCollection value) 
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range from null.");

			if(value != null)
			{
				for (int i = 0;	(i < value.Count); i = (i +	1))	
				{
					this.Add((LinkCategory)value.List[i]);
				}
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="LinkCategoryCollection">LinkCategoryCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="LinkCategoryCollection">LinkCategoryCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(LinkCategory value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(LinkCategory[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="LinkCategoryCollection">LinkCategoryCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="LinkCategoryCollection">LinkCategoryCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(LinkCategory value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, LinkCategory value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(LinkCategory value) 
		{
			List.Remove(value);
		}

		public void Sort()
		{
			this.InnerList.Sort(new SortListCategoryComparer());
		}

		private sealed class SortListCategoryComparer : IComparer 
		{
			public int Compare(object x, object y)
			{
				LinkCategory first = (LinkCategory) x;
				LinkCategory second = (LinkCategory) y;
				return first.Title.CompareTo(second.Title);
			}
		}

		
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="LinkCategoryCollection">LinkCategoryCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="LinkCategoryCollectionEnumerator">LinkCategoryCollectionEnumerator</see> for the <see cref="LinkCategoryCollection">LinkCategoryCollection</see> instance.</returns>
		public new LinkCategoryCollectionEnumerator GetEnumerator()	
		{
			return new LinkCategoryCollectionEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="LinkCategoryCollection">LinkCategoryCollection</see>.
		/// </summary>
		public class LinkCategoryCollectionEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="LinkCategoryCollectionEnumerator">LinkCategoryCollectionEnumerator</see> class referencing the specified <see cref="LinkCategoryCollection">LinkCategoryCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="LinkCategoryCollection">LinkCategoryCollection</see> to enumerate.</param>
			public LinkCategoryCollectionEnumerator(LinkCategoryCollection mappings)
			{
				_temp =	mappings;
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public LinkCategory Current
			{
				get {return ((LinkCategory)(_enumerator.Current));}
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

