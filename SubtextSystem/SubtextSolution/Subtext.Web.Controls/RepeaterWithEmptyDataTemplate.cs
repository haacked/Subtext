using System;
using System.Web.UI;
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
		private ITemplate _emptyDataTemplate;
		
		/// <summary>
		/// Gets or sets the empty data template.  This contains controls 
		/// displayed when the repeater is bound to a datasource with no elements.
		/// </summary>
		/// <value></value>
		public ITemplate EmptyDataTemplate
		{
			get 
			{
				return _emptyDataTemplate;
			}
			set 
			{
				_emptyDataTemplate = value;
			}
		}
	
		/// <summary>
		/// Performs the data bind and if the repeater has no 
		/// items, it instantiates the EmptyItemTemplate.
		/// </summary>
		/// <param name="e">E.</param>
		protected override void OnDataBinding(EventArgs e) 
		{
			base.OnDataBinding (e);
			if(this.Items.Count == 0) 
			{
				EmptyDataTemplate.InstantiateIn(this);
			}
		}
	}
}