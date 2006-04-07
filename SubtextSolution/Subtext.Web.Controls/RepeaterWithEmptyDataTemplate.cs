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
		private PlaceHolder emptyDataTemplate;
		
		/// <summary>
		/// Gets or sets the empty data template.  This contains controls 
		/// displayed when the repeater is bound to a datasource with no elements.
		/// </summary>
		/// <value></value>
		public PlaceHolder EmptyDataTemplate
		{
			get 
			{
				return this.emptyDataTemplate;
			}
			set 
			{
				this.emptyDataTemplate = value;
			}
		}
	
		/// <summary>
		/// When each item is created, this checks if the item is the
		/// FooterTemplate. If it is, and the number if items is zero
		/// (empty) then it adds the EmptyDataTemplate to the containting control.
		/// </summary>
		/// <param name="ea">E.</param>
		protected override void OnItemCreated(RepeaterItemEventArgs ea) 
		{ 
			if (ea.Item.ItemType == ListItemType.Footer && this.Items.Count == 0) 
				Controls.Add(EmptyDataTemplate); 
			base.OnItemCreated(ea); 
		}
	}
}