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
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

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
		bool dataBound;
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
			if(this.Entry != null && !dataBound)
			{
				dataBound = true;
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
		
		/// <summary>
		/// Gets the entry fully qualifed URL.
		/// </summary>
		/// <value>The entry fully qualifed URL.</value>
		protected string EntryFullyQualifedUrl
		{
			get
			{
				if(Entry != null)
				{
					return Config.CurrentBlog.UrlFormats.EntryFullyQualifiedUrl(this.Entry);
				}
				return string.Empty;
			}
		}
	}
}
