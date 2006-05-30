using System;
using System.Collections;

namespace Subtext.UnitTesting.Servers
{
	/// <summary>
	/// Represents a collection of <see cref="ReceivedEmailMessage">EmailMessage</see>.
	/// </summary>
	[Serializable]
	public class ReceivedEmailMessageCollection : CollectionBase
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> class.
		/// </summary>
		public ReceivedEmailMessageCollection()
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> class containing the elements of the specified source collection.
		/// </summary>
		/// <param name="value">A <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> with which to initialize the collection.</param>
		public ReceivedEmailMessageCollection(ReceivedEmailMessageCollection value)
		{
			if(value != null)
			{
				this.AddRange(value);
			}
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> class containing the specified array of <see cref="ReceivedEmailMessage">EmailMessage</see> Components.
		/// </summary>
		/// <param name="value">An array of <see cref="ReceivedEmailMessage">EmailMessage</see> Components with which to initialize the collection. </param>
		public ReceivedEmailMessageCollection(ReceivedEmailMessage[] value)
		{
			if(value != null)
			{
				this.AddRange(value);
			}
		}

		/// <summary>
		/// Gets the <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> at the specified index in the collection.
		/// <para>
		/// In C#, this property is the indexer for the <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> class.
		/// </para>
		/// </summary>
		public ReceivedEmailMessage this[int index]
		{
			get { return ((ReceivedEmailMessage)(this.List[index])); }
		}

		public int Add(ReceivedEmailMessage value)
		{
			if(value != null)
			{
				return this.List.Add(value);
			}
			return -1;
		}

		/// <summary>
		/// Copies the elements of the specified <see cref="ReceivedEmailMessage">EmailMessage</see> array to the end of the collection.
		/// </summary>
		/// <param name="value">An array of type <see cref="ReceivedEmailMessage">EmailMessage</see> containing the Components to add to the collection.</param>
		public void AddRange(ReceivedEmailMessage[] value)
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range from null.");

			if(value != null)
			{
				for(int i = 0; (i < value.Length); i = (i + 1))
				{
					this.Add(value[i]);
				}
			}
		}

		/// <summary>
		/// Adds the contents of another <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> to the end of the collection.
		/// </summary>
		/// <param name="value">A <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> containing the Components to add to the collection. </param>
		public void AddRange(ReceivedEmailMessageCollection value)
		{
			if(value == null)
				throw new ArgumentNullException("value", "Cannot add a range from null.");

			if(value != null)
			{
				for(int i = 0; (i < value.Count); i = (i + 1))
				{
					this.Add((ReceivedEmailMessage)value.List[i]);
				}
			}
		}

		/// <summary>
		/// Gets a value indicating whether the collection contains the specified <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see>.
		/// </summary>
		/// <param name="value">The <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> to search for in the collection.</param>
		/// <returns><b>true</b> if the collection contains the specified object; otherwise, <b>false</b>.</returns>
		public bool Contains(ReceivedEmailMessage value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Copies the collection Components to a one-dimensional <see cref="T:System.Array">Array</see> instance beginning at the specified index.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="T:System.Array">Array</see> that is the destination of the values copied from the collection.</param>
		/// <param name="index">The index of the array at which to begin inserting.</param>
		public void CopyTo(ReceivedEmailMessage[] array, int index)
		{
			this.List.CopyTo(array, index);
		}

		/// <summary>
		/// Gets the index in the collection of the specified <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see>, if it exists in the collection.
		/// </summary>
		/// <param name="value">The <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> to locate in the collection.</param>
		/// <returns>The index in the collection of the specified object, if found; otherwise, -1.</returns>
		public int IndexOf(ReceivedEmailMessage value)
		{
			return this.List.IndexOf(value);
		}

		public void Insert(int index, ReceivedEmailMessage value)
		{
			List.Insert(index, value);
		}

		public void Remove(ReceivedEmailMessage value)
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
				ReceivedEmailMessage first = (ReceivedEmailMessage)x;
				ReceivedEmailMessage second = (ReceivedEmailMessage)y;
				return first.Subject.CompareTo(second.Subject);
			}
		}


		/// <summary>
		/// Returns an enumerator that can iterate through the <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> instance.
		/// </summary>
		/// <returns>An <see cref="EmailMessageCollectionEnumerator">EmailMessageCollectionEnumerator</see> for the <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> instance.</returns>
		public new EmailMessageCollectionEnumerator GetEnumerator()
		{
			return new EmailMessageCollectionEnumerator(this);
		}

		/// <summary>
		/// Supports a simple iteration over a <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see>.
		/// </summary>
		public class EmailMessageCollectionEnumerator : IEnumerator
		{
			private IEnumerator _enumerator;
			private IEnumerable _temp;

			/// <summary>
			/// Initializes a new instance of the <see cref="EmailMessageCollectionEnumerator">EmailMessageCollectionEnumerator</see> class referencing the specified <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> object.
			/// </summary>
			/// <param name="mappings">The <see cref="ReceivedEmailMessageCollection">EmailMessageCollection</see> to enumerate.</param>
			public EmailMessageCollectionEnumerator(ReceivedEmailMessageCollection mappings)
			{
				_temp = mappings;
				_enumerator = _temp.GetEnumerator();
			}

			/// <summary>
			/// Gets the current element in the collection.
			/// </summary>
			public ReceivedEmailMessage Current
			{
				get { return ((ReceivedEmailMessage)(_enumerator.Current)); }
			}

			object IEnumerator.Current
			{
				get { return _enumerator.Current; }
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
