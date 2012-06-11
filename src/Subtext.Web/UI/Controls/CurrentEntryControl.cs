#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using Subtext.Web.UI.ViewModels;

namespace Subtext.Web.UI.Controls
{
    /// <summary>
    /// Control that contains information about the current entry 
    /// being displayed.  This will allow other controls to use 
    /// data binding syntax to display information about the current 
    /// entry.
    /// </summary>
    public class CurrentEntryControl : BaseControl, IEntryControl
    {
        bool _dataBound;

        #region IEntryControl Members

        /// <summary>
        /// Gets the current entry.
        /// </summary>
        /// <value>The current entry.</value>
        public EntryViewModel Entry { get; set; }

        #endregion

        /// <summary>
        /// Binds a data source to the invoked server control and all its child
        /// controls.
        /// </summary>
        public override void DataBind()
        {
            if (Entry != null && !_dataBound)
            {
                _dataBound = true;
                base.DataBind();
            }
        }
    }
}