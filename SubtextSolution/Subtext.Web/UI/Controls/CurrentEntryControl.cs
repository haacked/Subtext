using System;
using System.Web;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Control that contains information about the current entry 
	/// being displayed.  This will allow other controls to use 
	/// data binding syntax to display information about the current 
	/// entry.
	/// </summary>
	public class CurrentEntryControl : BaseControl
	{
		Entry currentEntry;
		/// <summary>
		/// Initializes a new instance of the <see cref="CurrentEntryControl"/> class.
		/// </summary>
		public CurrentEntryControl()
		{
		}
		
		/// <summary>
		/// Gets the current entry.
		/// </summary>
		/// <value>The current entry.</value>
		public Entry Entry
		{
			get
			{
				return this.currentEntry;
			}
			set
			{
				this.currentEntry = value;
			}
		}
		
		/// <summary>
		/// Binds a data source to the invoked server control and all its child
		/// controls.
		/// </summary>
		public override void DataBind()
		{
			if(this.Entry != null)
			{
				base.DataBind();
			}
		}
		
		/// <summary>
		/// Url encodes the string.
		/// </summary>
		/// <param name="s">The s.</param>
		/// <returns></returns>
		protected string UrlEncode(string s)
		{
			return HttpUtility.UrlEncode(s);
		}
	}
}
