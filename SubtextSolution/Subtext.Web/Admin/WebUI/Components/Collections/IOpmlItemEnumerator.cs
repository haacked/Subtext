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
    /// Supports type-safe iteration over a collection that 
    /// contains <see cref="OpmlItem"/> elements.
    /// </summary>
    /// <remarks>
    /// <b>IOpmlItemEnumerator</b> provides an <see cref="IEnumerator"/> 
    /// that is strongly typed for <see cref="OpmlItem"/> elements.
    /// </remarks>    

    public interface IOpmlItemEnumerator
    {
        #region Properties
        /// <summary>
        /// Gets the current <see cref="OpmlItem"/> element in the collection.
        /// </summary>
        /// <value>The current <see cref="OpmlItem"/> element in the collection.</value>
        /// <exception cref="InvalidOperationException">The enumerator is positioned 
        /// before the first element of the collection or after the last element.</exception>    
        /// <remarks>Please refer to <see cref="IEnumerator.Current"/> for details.</remarks>    

        OpmlItem Current { get; }

        #endregion
        #region Methods

        /// <summary>
        /// Advances the enumerator to the next element of the collection.
        /// </summary>
        /// <returns><c>true</c> if the enumerator was successfully advanced to the next element; 
        /// <c>false</c> if the enumerator has passed the end of the collection.</returns>
        /// <exception cref="InvalidOperationException">
        /// The collection was modified after the enumerator was created.</exception>
        /// <remarks>Please refer to <see cref="IEnumerator.MoveNext"/> for details.</remarks>    

        bool MoveNext();

        /// <summary>
        /// Sets the enumerator to its initial position, 
        /// which is before the first element in the collection.
        /// </summary>
        /// <exception cref="InvalidOperationException">
        /// The collection was modified after the enumerator was created.</exception>
        /// <remarks>Please refer to <see cref="IEnumerator.Reset"/> for details.</remarks>    

        void Reset();

        #endregion
    }
}
