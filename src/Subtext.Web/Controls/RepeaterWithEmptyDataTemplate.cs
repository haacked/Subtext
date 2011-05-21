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

using System.Web.UI.WebControls;

namespace Subtext.Web.Controls
{
    /// <summary>
    /// An asp:Repeater control that lets you specify a template 
    /// to be displayed if there is no data in the repeater
    /// </summary>
    /// <remarks>
    /// Code taken from <see href="http://weblogs.asp.net/acampbell/archive/2004/06/19/159780.aspx">Alex 
    /// Campbell's blog.</see>
    /// </remarks>
    public class RepeaterWithEmptyDataTemplate : Repeater
    {
        /// <summary>
        /// Gets or sets the empty data template.  This contains controls 
        /// displayed when the repeater is bound to a datasource with no elements.
        /// </summary>
        /// <value></value>
        public PlaceHolder EmptyDataTemplate { get; set; }

        /// <summary>
        /// When each item is created, this checks if the item is the
        /// FooterTemplate. If it is, and the number if items is zero
        /// (empty) then it adds the EmptyDataTemplate to the containting control.
        /// </summary>
        protected override void OnItemCreated(RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Footer && Items.Count == 0)
            {
                Controls.Add(EmptyDataTemplate);
            }
            base.OnItemCreated(e);
        }
    }
}