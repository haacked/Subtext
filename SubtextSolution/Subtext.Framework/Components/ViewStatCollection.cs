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
	/// Represents a collection of <see cref="ViewStat">ViewStat</see> Components.
	/// </summary>
	[Serializable]
	public class ViewStatCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ViewStatCollection">ViewStatCollection</see> class.
		/// </summary>
		public ViewStatCollection() 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ViewStatCollection">ViewStatCollection</see> class containing the specified array of <see cref="ViewStat">ViewStat</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="ViewStat">ViewStat</see> Components with which to initialize the collection. </param>
		public ViewStatCollection(ViewStat[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="ViewStatCollection">ViewStatCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="ViewStatCollection">ViewStatCollection</see> class.
		/// </para>
		/// </summary>
		public ViewStat this[int index] 
		{
			get	{return ((ViewStat)(this.List[index]));}
		}
		
		public int Add(ViewStat value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="ViewStat">ViewStat</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="ViewStat">ViewStat</see> containing the Components to add to the collection.</param>
		public void AddRange(ViewStat[] value) 
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range from null.");

			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="ViewStatCollection">ViewStatCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ViewStatCollection">ViewStatCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(ViewStatCollection value) 
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range from null.");

			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((ViewStat)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="ViewStatCollection">ViewStatCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="ViewStatCollection">ViewStatCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(ViewStat value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(ViewStat[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="ViewStatCollection">ViewStatCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="ViewStatCollection">ViewStatCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(ViewStat value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, ViewStat value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(ViewStat value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="ViewStatCollection">ViewStatCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="BlogPostDayEnumerator">BlogPostDayEnumerator</see> for the <see cref="ViewStatCollection">ViewStatCollection</see> instance.</returns>
		public new BlogPostDayEnumerator GetEnumerator()	
		{
			return new BlogPostDayEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="ViewStatCollection">ViewStatCollection</see>.
		/// </summary>
		public class BlogPostDayEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the 
			/// <see cref="BlogPostDayEnumerator">BlogPostDayEnumerator</see> class referencing the 
			/// specified <see cref="ViewStatCollection">ViewStatCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="ViewStatCollection">ViewStatCollection</see> 
			/// to enumerate.</param>
			public BlogPostDayEnumerator(ViewStatCollection mappings)
			{
				_temp =	mappings;
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public ViewStat Current
			{
				get {return ((ViewStat)(_enumerator.Current));}
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

