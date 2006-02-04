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

namespace Subtext.Web.Admin 
{
	#region Interface IPageLocationCollection

	/// <summary>
	/// Defines size, enumerators, and synchronization methods for strongly
	/// typed collections of <see cref="PageLocation"/> elements.
	/// </summary>
	/// <remarks>
	/// <b>IPageLocationCollection</b> provides an <see cref="ICollection"/> 
	/// that is strongly typed for <see cref="PageLocation"/> elements.
	/// </remarks>    
	public interface IPageLocationCollection 
	{
		#region Properties
		#region Count

		/// <summary>
		/// Gets the number of elements contained in the 
		/// <see cref="IPageLocationCollection"/>.
		/// </summary>
		/// <value>The number of elements contained in the 
		/// <see cref="IPageLocationCollection"/>.</value>
		/// <remarks>Please refer to <see cref="ICollection.Count"/> for details.</remarks>

		int Count { get; }
        
		#endregion
		#region IsSynchronized
        
		/// <summary>
		/// Gets a value indicating whether access to the 
		/// <see cref="IPageLocationCollection"/> is synchronized (thread-safe).
		/// </summary>
		/// <value><c>true</c> if access to the <see cref="IPageLocationCollection"/> is 
		/// synchronized (thread-safe); otherwise, <c>false</c>. The default is <c>false</c>.</value>
		/// <remarks>Please refer to <see cref="ICollection.IsSynchronized"/> for details.</remarks>

		bool IsSynchronized { get; }
        
		#endregion
		#region SyncRoot

		/// <summary>
		/// Gets an object that can be used to synchronize access 
		/// to the <see cref="IPageLocationCollection"/>.
		/// </summary>
		/// <value>An object that can be used to synchronize access 
		/// to the <see cref="IPageLocationCollection"/>.</value>
		/// <remarks>Please refer to <see cref="ICollection.SyncRoot"/> for details.</remarks>

		object SyncRoot { get; }

		#endregion
		#endregion
		#region Methods
		#region CopyTo

		/// <summary>
		/// Copies the entire <see cref="IPageLocationCollection"/> to a one-dimensional <see cref="Array"/>
		/// of <see cref="PageLocation"/> elements, starting at the specified index of the target array.
		/// </summary>
		/// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the
		/// <see cref="PageLocation"/> elements copied from the <see cref="IPageLocationCollection"/>. 
		/// The <b>Array</b> must have zero-based indexing.</param>
		/// <param name="arrayIndex">The zero-based index in <paramref name="array"/> 
		/// at which copying begins.</param>
		/// <exception cref="ArgumentNullException">
		/// <paramref name="array"/> is a null reference.</exception>    
		/// <exception cref="ArgumentOutOfRangeException">
		/// <paramref name="arrayIndex"/> is less than zero.</exception>    
		/// <exception cref="ArgumentException"><para>
		/// <paramref name="arrayIndex"/> is equal to or greater than the length of <paramref name="array"/>.
		/// </para><para>-or-</para><para>
		/// The number of elements in the source <see cref="IPageLocationCollection"/> is greater 
		/// than the available space from <paramref name="arrayIndex"/> to the end of the destination 
		/// <paramref name="array"/>.</para></exception>
		/// <remarks>Please refer to <see cref="ICollection.CopyTo"/> for details.</remarks>

		void CopyTo(PageLocation[] array, int arrayIndex);
        
		#endregion
		#region GetEnumerator

		/// <summary>
		/// Returns an <see cref="IPageLocationEnumerator"/> that can
		/// iterate through the <see cref="IPageLocationCollection"/>.
		/// </summary>
		/// <returns>An <see cref="IPageLocationEnumerator"/> 
		/// for the entire <see cref="IPageLocationCollection"/>.</returns>
		/// <remarks>Please refer to <see cref="IEnumerable.GetEnumerator"/> for details.</remarks>

		IPageLocationEnumerator GetEnumerator();

		#endregion
		#endregion
	}
    
	#endregion
	#region Interface IPageLocationList

	/// <summary>
	/// Represents a strongly typed collection of <see cref="PageLocation"/> 
	/// objects that can be individually accessed by index.
	/// </summary>
	/// <remarks>
	/// <b>IPageLocationList</b> provides an <see cref="IList"/>
	/// that is strongly typed for <see cref="PageLocation"/> elements.
	/// </remarks>    

	public interface 
		IPageLocationList: IPageLocationCollection 
	{
		#region Properties
		#region IsFixedSize

		/// <summary>
		/// Gets a value indicating whether the <see cref="IPageLocationList"/> has a fixed size.
		/// </summary>
		/// <value><c>true</c> if the <see cref="IPageLocationList"/> has a fixed size;
		/// otherwise, <c>false</c>. The default is <c>false</c>.</value>
		/// <remarks>Please refer to <see cref="IList.IsFixedSize"/> for details.</remarks>

		bool IsFixedSize { get; }
        
		#endregion
		#region IsReadOnly

		/// <summary>
		/// Gets a value indicating whether the <see cref="IPageLocationList"/> is read-only.
		/// </summary>
		/// <value><c>true</c> if the <see cref="IPageLocationList"/> is read-only;
		/// otherwise, <c>false</c>. The default is <c>false</c>.</value>
		/// <remarks>Please refer to <see cref="IList.IsReadOnly"/> for details.</remarks>

		bool IsReadOnly { get; }
        
		#endregion
		#region Item

		/// <summary>
		/// Gets or sets the <see cref="PageLocation"/> element at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index of the 
		/// <see cref="PageLocation"/> element to get or set.</param>
		/// <value>
		/// The <see cref="PageLocation"/> element at the specified <paramref name="index"/>.
		/// </value>    
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="index"/> is less than zero.</para>
		/// <para>-or-</para>
		/// <para><paramref name="index"/> is equal to or greater than 
		/// <see cref="IPageLocationCollection.Count"/>.</para>
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// The property is set and the <see cref="IPageLocationList"/> is read-only.</exception>
		/// <remarks>Please refer to <see cref="IList.this"/> for details.</remarks>

		PageLocation this[int index] { get; set; }

		#endregion
		#endregion
		#region Methods
		#region Add

		/// <summary>
		/// Adds a <see cref="PageLocation"/> to the end 
		/// of the <see cref="IPageLocationList"/>.
		/// </summary>
		/// <param name="value">The <see cref="PageLocation"/> object 
		/// to be added to the end of the <see cref="IPageLocationList"/>.
		/// This argument can be a null reference.
		/// </param>    
		/// <returns>The <see cref="IPageLocationList"/> index at which
		/// the <paramref name="value"/> has been added.</returns>
		/// <exception cref="NotSupportedException">
		/// <para>The <see cref="IPageLocationList"/> is read-only.</para>
		/// <para>-or-</para>
		/// <para>The <b>IPageLocationList</b> has a fixed size.</para></exception>    
		/// <remarks>Please refer to <see cref="IList.Add"/> for details.</remarks>

		int Add(PageLocation value);
        
		#endregion
		#region Clear
        
		/// <summary>
		/// Removes all elements from the <see cref="IPageLocationList"/>.
		/// </summary>
		/// <exception cref="NotSupportedException">
		/// <para>The <see cref="IPageLocationList"/> is read-only.</para>
		/// <para>-or-</para>
		/// <para>The <b>IPageLocationList</b> has a fixed size.</para></exception>    
		/// <remarks>Please refer to <see cref="IList.Clear"/> for details.</remarks>

		void Clear();
        
		#endregion
		#region Contains
        
		/// <summary>
		/// Determines whether the <see cref="IPageLocationList"/>
		/// contains the specified <see cref="PageLocation"/> element.
		/// </summary>
		/// <param name="value">The <see cref="PageLocation"/> object
		/// to locate in the <see cref="IPageLocationList"/>.
		/// This argument can be a null reference.
		/// </param>    
		/// <returns><c>true</c> if <paramref name="value"/> is found in the 
		/// <see cref="IPageLocationList"/>; otherwise, <c>false</c>.</returns>
		/// <remarks>Please refer to <see cref="IList.Contains"/> for details.</remarks>

		bool Contains(PageLocation value);
        
		#endregion
		#region IndexOf

		/// <summary>
		/// Returns the zero-based index of the first occurrence of the specified 
		/// <see cref="PageLocation"/> in the <see cref="IPageLocationList"/>.
		/// </summary>
		/// <param name="value">The <see cref="PageLocation"/> object 
		/// to locate in the <see cref="IPageLocationList"/>.
		/// This argument can be a null reference.
		/// </param>    
		/// <returns>
		/// The zero-based index of the first occurrence of <paramref name="value"/> 
		/// in the <see cref="IPageLocationList"/>, if found; otherwise, -1.
		/// </returns>
		/// <remarks>Please refer to <see cref="IList.IndexOf"/> for details.</remarks>

		int IndexOf(PageLocation value);
        
		#endregion
		#region Insert

		/// <summary>
		/// Inserts a <see cref="PageLocation"/> element into the 
		/// <see cref="IPageLocationList"/> at the specified index.
		/// </summary>
		/// <param name="index">The zero-based index at which 
		/// <paramref name="value"/> should be inserted.</param>
		/// <param name="value">The <see cref="PageLocation"/> object
		/// to insert into the <see cref="IPageLocationList"/>.
		/// This argument can be a null reference.
		/// </param>    
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="index"/> is less than zero.</para>
		/// <para>-or-</para>
		/// <para><paramref name="index"/> is greater than 
		/// <see cref="IPageLocationCollection.Count"/>.</para>
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// <para>The <see cref="IPageLocationList"/> is read-only.</para>
		/// <para>-or-</para>
		/// <para>The <b>IPageLocationList</b> has a fixed size.</para></exception>    
		/// <remarks>Please refer to <see cref="IList.Insert"/> for details.</remarks>

		void Insert(int index, PageLocation value);
        
		#endregion
		#region Remove

		/// <summary>
		/// Removes the first occurrence of the specified <see cref="PageLocation"/>
		/// from the <see cref="IPageLocationList"/>.
		/// </summary>
		/// <param name="value">The <see cref="PageLocation"/> object
		/// to remove from the <see cref="IPageLocationList"/>.
		/// This argument can be a null reference.
		/// </param>    
		/// <exception cref="NotSupportedException">
		/// <para>The <see cref="IPageLocationList"/> is read-only.</para>
		/// <para>-or-</para>
		/// <para>The <b>IPageLocationList</b> has a fixed size.</para></exception>    
		/// <remarks>Please refer to <see cref="IList.Remove"/> for details.</remarks>

		void Remove(PageLocation value);
        
		#endregion
		#region RemoveAt

		/// <summary>
		/// Removes the element at the specified index of the 
		/// <see cref="IPageLocationList"/>.
		/// </summary>
		/// <param name="index">The zero-based index of the element to remove.</param>
		/// <exception cref="ArgumentOutOfRangeException">
		/// <para><paramref name="index"/> is less than zero.</para>
		/// <para>-or-</para>
		/// <para><paramref name="index"/> is equal to or greater than 
		/// <see cref="IPageLocationCollection.Count"/>.</para>
		/// </exception>
		/// <exception cref="NotSupportedException">
		/// <para>The <see cref="IPageLocationList"/> is read-only.</para>
		/// <para>-or-</para>
		/// <para>The <b>IPageLocationList</b> has a fixed size.</para></exception>    
		/// <remarks>Please refer to <see cref="IList.RemoveAt"/> for details.</remarks>

		void RemoveAt(int index);

		#endregion
		#endregion
	}
    
	#endregion
	#region Interface IPageLocationEnumerator

	/// <summary>
	/// Supports type-safe iteration over a collection that 
	/// contains <see cref="PageLocation"/> elements.
	/// </summary>
	/// <remarks>
	/// <b>IPageLocationEnumerator</b> provides an <see cref="IEnumerator"/> 
	/// that is strongly typed for <see cref="PageLocation"/> elements.
	/// </remarks>    
        
	public interface IPageLocationEnumerator 
	{
		#region Properties
		#region Current

		/// <summary>
		/// Gets the current <see cref="PageLocation"/> element in the collection.
		/// </summary>
		/// <value>The current <see cref="PageLocation"/> element in the collection.</value>
		/// <exception cref="InvalidOperationException">The enumerator is positioned 
		/// before the first element of the collection or after the last element.</exception>    
		/// <remarks>Please refer to <see cref="IEnumerator.Current"/> for details.</remarks>    

		PageLocation Current { get; }
        
		#endregion
		#endregion
		#region Methods
		#region MoveNext

		/// <summary>
		/// Advances the enumerator to the next element of the collection.
		/// </summary>
		/// <returns><c>true</c> if the enumerator was successfully advanced to the next element; 
		/// <c>false</c> if the enumerator has passed the end of the collection.</returns>
		/// <exception cref="InvalidOperationException">
		/// The collection was modified after the enumerator was created.</exception>
		/// <remarks>Please refer to <see cref="IEnumerator.MoveNext"/> for details.</remarks>    

		bool MoveNext();
        
		#endregion
		#region Reset

		/// <summary>
		/// Sets the enumerator to its initial position, 
		/// which is before the first element in the collection.
		/// </summary>
		/// <exception cref="InvalidOperationException">
		/// The collection was modified after the enumerator was created.</exception>
		/// <remarks>Please refer to <see cref="IEnumerator.Reset"/> for details.</remarks>    

		void Reset();
        
		#endregion
		#endregion
	}

	#endregion
}

