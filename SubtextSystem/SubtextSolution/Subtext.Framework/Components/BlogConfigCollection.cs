using System;
using System.Collections;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Represents a collection of <see cref="BlogConfig">BlogConfig</see> Components.
	/// </summary>
	[Serializable]
	public class BlogConfigCollection : CollectionBase, IPagedResults
	{
		private int _maxItems;

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogConfigCollection">BlogConfigCollection</see> 
		/// class.
		/// </summary>
		public BlogConfigCollection() : base()
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
		/// <see cref="BlogConfigCollection">BlogConfigCollection</see> class containing the 
		/// specified array of <see cref="BlogConfig">BlogConfig</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="BlogConfig">BlogConfig</see> Components with which to initialize the collection. </param>
		public BlogConfigCollection(BlogConfig[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="BlogConfigCollection">BlogConfigCollection</see> at 
		/// the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the 
		/// <see cref="BlogConfigCollection">BlogConfigCollection</see> class.
		/// </para>
		/// </summary>
		public BlogConfig this[int index] 
		{
			get	{return ((BlogConfig)(this.List[index]));}
		}
		
		public int Add(BlogConfig value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="BlogConfig">BlogConfig</see> 
		/// array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="BlogConfig">BlogConfig</see> 
		/// containing the Components to add to the collection.</param>
		public void AddRange(BlogConfig[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="BlogConfigCollection">BlogConfigCollection</see> 
		/// to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="BlogConfigCollection">BlogConfigCollection</see> containing 
		/// the Components to add to the collection. </param>
		public void AddRange(BlogConfigCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((BlogConfig)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified 
		/// <see cref="BlogConfigCollection">BlogConfigCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="BlogConfigCollection">BlogConfigCollection</see> 
		/// to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, 
		/// <b>false</b>.</returns>
		public bool Contains(BlogConfig value) 
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
		public void CopyTo(BlogConfig[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified 
		/// <see cref="BlogConfigCollection">BlogConfigCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="BlogConfigCollection">BlogConfigCollection</see> 
		/// to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(BlogConfig value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, BlogConfig value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(BlogConfig value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the 
		/// <see cref="BlogConfigCollection">BlogConfigCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="BlogConfigEnumerator">BlogConfigEnumerator</see> for the 
		/// <see cref="BlogConfigCollection">BlogConfigCollection</see> instance.</returns>
		public new BlogConfigEnumerator GetEnumerator()	
		{
			return new BlogConfigEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="BlogConfigCollection">BlogConfigCollection</see>.
		/// </summary>
		public class BlogConfigEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="BlogConfigEnumerator">BlogPostDayEnumerator</see> class referencing the specified <see cref="BlogConfigCollection">BlogConfigCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="BlogConfigCollection">BlogConfigCollection</see> to enumerate.</param>
			public BlogConfigEnumerator(BlogConfigCollection mappings)
			{
				_temp = mappings;
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public BlogConfig Current
			{
				get {return ((BlogConfig)(_enumerator.Current));}
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

