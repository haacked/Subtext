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
	/// Represents a collection of <see cref="Referrer">Referrer</see> Components.
	/// </summary>
	[Serializable]
	public class ReferrerCollection: CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReferrerCollection">ReferrerCollection</see> class.
		/// </summary>
		public ReferrerCollection() 
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReferrerCollection">ReferrerCollection</see> class containing the specified array of <see cref="Referrer">Referrer</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="Referrer">Referrer</see> Components with which to initialize the collection. </param>
		public ReferrerCollection(Referrer[] value)
		{
			this.AddRange(value);
		}
		
		/// <summary>
		/// Gets the <see cref="ReferrerCollection">ReferrerCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="ReferrerCollection">ReferrerCollection</see> class.
		/// </para>
		/// </summary>
		public Referrer this[int index] 
		{
			get	{return ((Referrer)(this.List[index]));}
		}
		
		public int Add(Referrer value) 
		{
			return this.List.Add(value);
		}
		
		/// <summary>
		/// Copies the elements of the specified <see cref="Referrer">Referrer</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="Referrer">Referrer</see> containing the Components to add to the collection.</param>
		public void AddRange(Referrer[] value) 
		{
			for (int i = 0;	(i < value.Length); i = (i + 1)) 
			{
				this.Add(value[i]);
			}
		}
		
		/// <summary>
		/// Adds the contents of another <see cref="ReferrerCollection">ReferrerCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ReferrerCollection">ReferrerCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(ReferrerCollection value) 
		{
			for (int i = 0;	(i < value.Count); i = (i +	1))	
			{
				this.Add((Referrer)value.List[i]);
			}
		}
		
		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="ReferrerCollection">ReferrerCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="ReferrerCollection">ReferrerCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(Referrer value) 
		{
			return this.List.Contains(value);
		}
		
		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(Referrer[] array, int index) 
		{
			this.List.CopyTo(array, index);
		}
		
		/// <summary>
		/// Gets the index in the collection of the specified <see cref="ReferrerCollection">ReferrerCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="ReferrerCollection">ReferrerCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(Referrer value) 
		{
			return this.List.IndexOf(value);
		}
		
		public void Insert(int index, Referrer value)	
		{
			List.Insert(index, value);
		}
		
		public void Remove(Referrer value) 
		{
			List.Remove(value);
		}
		
		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="ReferrerCollection">ReferrerCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="BlogPostDayEnumerator">BlogPostDayEnumerator</see> for the <see cref="ReferrerCollection">ReferrerCollection</see> instance.</returns>
		public new BlogPostDayEnumerator GetEnumerator()	
		{
			return new BlogPostDayEnumerator(this);
		}
		
		/// <summary>
		/// Supports a simple iteration over a <see cref="ReferrerCollection">ReferrerCollection</see>.
		/// </summary>
		public class BlogPostDayEnumerator : IEnumerator	
		{
			private	IEnumerator _enumerator;
			private	IEnumerable _temp;
			
			/// <summary>
			/// Initializes a new instance of the <see cref="BlogPostDayEnumerator">BlogPostDayEnumerator</see> class referencing the specified <see cref="ReferrerCollection">ReferrerCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="ReferrerCollection">ReferrerCollection</see> to enumerate.</param>
			public BlogPostDayEnumerator(ReferrerCollection mappings)
			{
				_temp =	((IEnumerable)(mappings));
				_enumerator = _temp.GetEnumerator();
			}
			
			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public Referrer Current
			{
				get {return ((Referrer)(_enumerator.Current));}
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

