using System;
using System.Collections;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Util.TimeZoneUtil
{
	/// <summary>
	/// A collection of elements of type WindowsTimeZone
	/// </summary>
	[Serializable]
	public class WindowsTimeZoneCollection : CollectionBase
	{
		/// <summary>
		/// Adds an instance of type WindowsTimeZone to the end of this WindowsTimeZoneCollection.
		/// </summary>
		/// <param name="value">
		/// The WindowsTimeZone to be added to the end of this WindowsTimeZoneCollection.
		/// </param>
		public virtual void Add(WindowsTimeZone value)
		{
			this.List.Add(value);
		}

		/// <summary>
		/// Determines whether a specfic WindowsTimeZone value is in this WindowsTimeZoneCollection.
		/// </summary>
		/// <param name="value">
		/// The WindowsTimeZone value to locate in this WindowsTimeZoneCollection.
		/// </param>
		/// <returns>
		/// true if value is found in this WindowsTimeZoneCollection;
		/// false otherwise.
		/// </returns>
		public virtual bool Contains(WindowsTimeZone value)
		{
			return this.List.Contains(value);
		}

		/// <summary>
		/// Return the zero-based index of the first occurrence of a specific value
		/// in this WindowsTimeZoneCollection
		/// </summary>
		/// <param name="value">
		/// The WindowsTimeZone value to locate in the WindowsTimeZoneCollection.
		/// </param>
		/// <returns>
		/// The zero-based index of the first occurrence of the _ELEMENT value if found;
		/// -1 otherwise.
		/// </returns>
		public virtual int IndexOf(WindowsTimeZone value)
		{
			return this.List.IndexOf(value);
		}

		/// <summary>
		/// Gets or sets the WindowsTimeZone at the given index in this WindowsTimeZoneCollection.
		/// </summary>
		public virtual WindowsTimeZone this[int index]
		{
			get
			{
				return (WindowsTimeZone)this.List[index];
			}
		}

		/// <summary>
		/// Gets the WindowsTimeZone by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public virtual WindowsTimeZone GetById(int id)
		{
			foreach (WindowsTimeZone wtz in this)
			{
				if (wtz.Id == id)
				{
					return wtz;
				}
			}
			return null;
		}

		/// <summary>
		/// Type-specific enumeration class, used by WindowsTimeZoneCollection.GetEnumerator.
		/// </summary>
		public class Enumerator : IEnumerator
		{
			private IEnumerator wrapped;

			public Enumerator(WindowsTimeZoneCollection collection)
			{
				if (collection == null)
				{
					throw new ArgumentNullException("collection", Resources.ArgumentNull_Collection);
				}

				this.wrapped = ((CollectionBase)collection).GetEnumerator();
			}

			public WindowsTimeZone Current
			{
				get
				{
					return (WindowsTimeZone)(this.wrapped.Current);
				}
			}

			object IEnumerator.Current
			{
				get
				{
					return (this.wrapped.Current);
				}
			}

			public bool MoveNext()
			{
				return this.wrapped.MoveNext();
			}

			public void Reset()
			{
				this.wrapped.Reset();
			}

			#region IEnumerator Members


			bool IEnumerator.MoveNext()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			void IEnumerator.Reset()
			{
				throw new Exception("The method or operation is not implemented.");
			}

			#endregion
		}

		/// <summary>
		/// Returns an enumerator that can iterate through the elements of this WindowsTimeZoneCollection.
		/// </summary>
		/// <returns>
		/// An object that implements System.Collections.IEnumerator.
		/// </returns>        
		public new virtual Enumerator GetEnumerator()
		{
			return new Enumerator(this);
		}

		public void SortByTimeZoneBias()
		{
			this.InnerList.Sort(new WindowsTimeZoneBiasSorter());
		}
	}
}
