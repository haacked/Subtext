using System;
using System.Collections;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a collection of <see cref="BlogInfo">BlogInfo</see> Components.
	/// </summary>
	[Serializable]
	public class BlogInfoCollection : CollectionBase, IPagedResults
	{
		private int _maxItems;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogInfoCollection">BlogInfoCollection</see> 
		/// class.
		/// </summary>
		public BlogInfoCollection() : base()
		{
		}

		/// <summary>
		/// Gets or sets the max items this can contain.
		/// </summary>
		/// <value></value>
		public int MaxItems
		{
			get {return this._maxItems;}
			set {this._maxItems = value;}
		}

		/// <summary>
		/// Initializes a new instance of the 
		/// <see cref="BlogInfoCollection">BlogInfoCollection</see> class containing the 
		/// specified array of <see cref="BlogInfo">BlogInfo</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="BlogInfo">BlogInfo</see> Components with which to initialize the collection. </param>
		public BlogInfoCollection(BlogInfo[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="BlogInfoCollection">BlogInfoCollection</see> at 
		/// the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the 
		/// <see cref="BlogInfoCollection">BlogInfoCollection</see> class.
		/// </para>
		/// </summary>
		public BlogInfo this[int index] 
		{
			get	{return ((BlogInfo)(this.List[index]));}
		}
		
		public int Add(BlogInfo value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="BlogInfo">BlogInfo</see> 
		/// array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="BlogInfo">BlogInfo</see> 
		/// containing the Components to add to the collection.</param>
		public void AddRange(BlogInfo[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="BlogInfoCollection">BlogInfoCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="BlogInfoCollection">BlogInfoCollection</see> containing 
		/// the Components to add to the collection. </param>
		public void AddRange(BlogInfoCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((BlogInfo)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified 
		/// <see cref="BlogInfoCollection">BlogInfoCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="BlogInfoCollection">BlogInfoCollection</see> 
		/// to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, 
		/// <b>false</b>.</returns>
		public bool Contains(BlogInfo value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional 
		/// <see cref="T:System.Array">Array</see> instance beginning 
		/// at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> 
		/// that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(BlogInfo[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified 
		/// <see cref="BlogInfoCollection">BlogInfoCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="BlogInfoCollection">BlogInfoCollection</see> 
		/// to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(BlogInfo value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, BlogInfo value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(BlogInfo value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the 
		/// <see cref="BlogInfoCollection">BlogInfoCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="BlogInfoEnumerator">BlogInfoEnumerator</see> for the 
		/// <see cref="BlogInfoCollection">BlogInfoCollection</see> instance.</returns>
		public new BlogInfoEnumerator GetEnumerator()	
		{
			return new BlogInfoEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="BlogInfoCollection">BlogInfoCollection</see>.
		/// </summary>
		public class BlogInfoEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="BlogInfoEnumerator">BlogPostDayEnumerator</see> class referencing the specified <see cref="BlogInfoCollection">BlogInfoCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="BlogInfoCollection">BlogInfoCollection</see> to enumerate.</param>
			public BlogInfoEnumerator(BlogInfoCollection mappings)
			{
				_temp = mappings;
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public BlogInfo Current
			{
				get {return ((BlogInfo)(_enumerator.Current));}
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

