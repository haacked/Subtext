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
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;

namespace Subtext.Web.Admin
{
    /// <summary>
    /// Represents a strongly typed collection of <see cref="OpmlItem"/> 
    /// objects that can be individually accessed by index.
    /// </summary>
    /// <remarks>
    /// <b>IOpmlItemList</b> provides an <see cref="IList"/>
    /// that is strongly typed for <see cref="OpmlItem"/> elements.
    /// </remarks>    

    public interface IOpmlItemList
    {
        /// <summary>
        /// Gets the number of elements contained in the 
        /// <see cref="IOpmlItemCollection"/>.
        /// </summary>
        /// <value>The number of elements contained in the 
        /// <see cref="IOpmlItemCollection"/>.</value>
        /// <remarks>Please refer to <see cref="ICollection.Count"/> for details.</remarks>

        int Count { get; }

        /// <summary>
        /// Gets a value indicating whether access to the 
        /// <see cref="IOpmlItemCollection"/> is synchronized (thread-safe).
        /// </summary>
        /// <value><c>true</c> if access to the <see cref="IOpmlItemCollection"/> is 
        /// synchronized (thread-safe); otherwise, <c>false</c>. The default is <c>false</c>.</value>
        /// <remarks>Please refer to <see cref="ICollection.IsSynchronized"/> for details.</remarks>

        bool IsSynchronized { get; }

        /// <summary>
        /// Gets an object that can be used to synchronize access 
        /// to the <see cref="IOpmlItemCollection"/>.
        /// </summary>
        /// <value>An object that can be used to synchronize access 
        /// to the <see cref="IOpmlItemCollection"/>.</value>
        /// <remarks>Please refer to <see cref="ICollection.SyncRoot"/> for details.</remarks>

        object SyncRoot { get; }


        /// <summary>
        /// Copies the entire <see cref="IOpmlItemCollection"/> to a one-dimensional <see cref="Array"/>
        /// of <see cref="OpmlItem"/> elements, starting at the specified index of the target array.
        /// </summary>
        /// <param name="array">The one-dimensional <see cref="Array"/> that is the destination of the
        /// <see cref="OpmlItem"/> elements copied from the <see cref="IOpmlItemCollection"/>. 
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
        /// The number of elements in the source <see cref="IOpmlItemCollection"/> is greater 
        /// than the available space from <paramref name="arrayIndex"/> to the end of the destination 
        /// <paramref name="array"/>.</para></exception>
        /// <remarks>Please refer to <see cref="ICollection.CopyTo"/> for details.</remarks>

        void CopyTo(OpmlItem[] array, int arrayIndex);


        /// <summary>
        /// Returns an <see cref="IOpmlItemEnumerator"/> that can
        /// iterate through the <see cref="IOpmlItemCollection"/>.
        /// </summary>
        /// <returns>An <see cref="IOpmlItemEnumerator"/> 
        /// for the entire <see cref="IOpmlItemCollection"/>.</returns>
        /// <remarks>Please refer to <see cref="IEnumerable.GetEnumerator"/> for details.</remarks>

        IOpmlItemEnumerator GetEnumerator();


        /// <summary>
        /// Gets a value indicating whether the <see cref="IOpmlItemList"/> has a fixed size.
        /// </summary>
        /// <value><c>true</c> if the <see cref="IOpmlItemList"/> has a fixed size;
        /// otherwise, <c>false</c>. The default is <c>false</c>.</value>
        /// <remarks>Please refer to <see cref="IList.IsFixedSize"/> for details.</remarks>

        bool IsFixedSize { get; }


        /// <summary>
        /// Gets a value indicating whether the <see cref="IOpmlItemList"/> is read-only.
        /// </summary>
        /// <value><c>true</c> if the <see cref="IOpmlItemList"/> is read-only;
        /// otherwise, <c>false</c>. The default is <c>false</c>.</value>
        /// <remarks>Please refer to <see cref="IList.IsReadOnly"/> for details.</remarks>

        bool IsReadOnly { get; }

        /// <summary>
        /// Gets or sets the <see cref="OpmlItem"/> element at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index of the 
        /// <see cref="OpmlItem"/> element to get or set.</param>
        /// <value>
        /// The <see cref="OpmlItem"/> element at the specified <paramref name="index"/>.
        /// </value>    
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than zero.</para>
        /// <para>-or-</para>
        /// <para><paramref name="index"/> is equal to or greater than 
        /// <see cref="IOpmlItemCollection.Count"/>.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// The property is set and the <see cref="IOpmlItemList"/> is read-only.</exception>
        /// <remarks>Please refer to <see cref="IList.this"/> for details.</remarks>

        OpmlItem this[int index] { get; set; }


        /// <summary>
        /// Adds a <see cref="OpmlItem"/> to the end 
        /// of the <see cref="IOpmlItemList"/>.
        /// </summary>
        /// <param name="value">The <see cref="OpmlItem"/> object 
        /// to be added to the end of the <see cref="IOpmlItemList"/>.
        /// This argument can be a null reference.
        /// </param>    
        /// <returns>The <see cref="IOpmlItemList"/> index at which
        /// the <paramref name="value"/> has been added.</returns>
        /// <exception cref="NotSupportedException">
        /// <para>The <see cref="IOpmlItemList"/> is read-only.</para>
        /// <para>-or-</para>
        /// <para>The <b>IOpmlItemList</b> has a fixed size.</para></exception>    
        /// <remarks>Please refer to <see cref="IList.Add"/> for details.</remarks>

        int Add(OpmlItem value);


        /// <summary>
        /// Removes all elements from the <see cref="IOpmlItemList"/>.
        /// </summary>
        /// <exception cref="NotSupportedException">
        /// <para>The <see cref="IOpmlItemList"/> is read-only.</para>
        /// <para>-or-</para>
        /// <para>The <b>IOpmlItemList</b> has a fixed size.</para></exception>    
        /// <remarks>Please refer to <see cref="IList.Clear"/> for details.</remarks>

        void Clear();


        /// <summary>
        /// Determines whether the <see cref="IOpmlItemList"/>
        /// contains the specified <see cref="OpmlItem"/> element.
        /// </summary>
        /// <param name="value">The <see cref="OpmlItem"/> object
        /// to locate in the <see cref="IOpmlItemList"/>.
        /// This argument can be a null reference.
        /// </param>    
        /// <returns><c>true</c> if <paramref name="value"/> is found in the 
        /// <see cref="IOpmlItemList"/>; otherwise, <c>false</c>.</returns>
        /// <remarks>Please refer to <see cref="IList.Contains"/> for details.</remarks>

        bool Contains(OpmlItem value);


        /// <summary>
        /// Returns the zero-based index of the first occurrence of the specified 
        /// <see cref="OpmlItem"/> in the <see cref="IOpmlItemList"/>.
        /// </summary>
        /// <param name="value">The <see cref="OpmlItem"/> object 
        /// to locate in the <see cref="IOpmlItemList"/>.
        /// This argument can be a null reference.
        /// </param>    
        /// <returns>
        /// The zero-based index of the first occurrence of <paramref name="value"/> 
        /// in the <see cref="IOpmlItemList"/>, if found; otherwise, -1.
        /// </returns>
        /// <remarks>Please refer to <see cref="IList.IndexOf"/> for details.</remarks>

        int IndexOf(OpmlItem value);


        /// <summary>
        /// Inserts a <see cref="OpmlItem"/> element into the 
        /// <see cref="IOpmlItemList"/> at the specified index.
        /// </summary>
        /// <param name="index">The zero-based index at which 
        /// <paramref name="value"/> should be inserted.</param>
        /// <param name="value">The <see cref="OpmlItem"/> object
        /// to insert into the <see cref="IOpmlItemList"/>.
        /// This argument can be a null reference.
        /// </param>    
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than zero.</para>
        /// <para>-or-</para>
        /// <para><paramref name="index"/> is greater than 
        /// <see cref="IOpmlItemCollection.Count"/>.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <para>The <see cref="IOpmlItemList"/> is read-only.</para>
        /// <para>-or-</para>
        /// <para>The <b>IOpmlItemList</b> has a fixed size.</para></exception>    
        /// <remarks>Please refer to <see cref="IList.Insert"/> for details.</remarks>

        void Insert(int index, OpmlItem value);

        /// <summary>
        /// Removes the first occurrence of the specified <see cref="OpmlItem"/>
        /// from the <see cref="IOpmlItemList"/>.
        /// </summary>
        /// <param name="value">The <see cref="OpmlItem"/> object
        /// to remove from the <see cref="IOpmlItemList"/>.
        /// This argument can be a null reference.
        /// </param>    
        /// <exception cref="NotSupportedException">
        /// <para>The <see cref="IOpmlItemList"/> is read-only.</para>
        /// <para>-or-</para>
        /// <para>The <b>IOpmlItemList</b> has a fixed size.</para></exception>    
        /// <remarks>Please refer to <see cref="IList.Remove"/> for details.</remarks>

        void Remove(OpmlItem value);


        /// <summary>
        /// Removes the element at the specified index of the 
        /// <see cref="IOpmlItemList"/>.
        /// </summary>
        /// <param name="index">The zero-based index of the element to remove.</param>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <para><paramref name="index"/> is less than zero.</para>
        /// <para>-or-</para>
        /// <para><paramref name="index"/> is equal to or greater than 
        /// <see cref="IOpmlItemCollection.Count"/>.</para>
        /// </exception>
        /// <exception cref="NotSupportedException">
        /// <para>The <see cref="IOpmlItemList"/> is read-only.</para>
        /// <para>-or-</para>
        /// <para>The <b>IOpmlItemList</b> has a fixed size.</para></exception>    
        /// <remarks>Please refer to <see cref="IList.RemoveAt"/> for details.</remarks>

        void RemoveAt(int index);
    }
}
