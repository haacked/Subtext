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
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for BaseControl.
	/// </summary>

	public class BaseControl : System.Web.UI.UserControl
	{
		public BaseControl()
		{
			
		}

		private BlogInfo _config = null;
		protected BlogInfo CurrentBlog
		{
			get
			{
				return _config;
			}
		}

		protected virtual string ControlCacheKey
		{
			get
			{
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}:{1}",this.GetType(),CurrentBlog.Id);
			}
		}

		protected override void OnInit(EventArgs e)
		{
			_config = Config.CurrentBlog;
			base.OnInit (e);
		}


//		protected override void OnLoad(EventArgs e)
//		{
//			_config = Config.CurrentBlog;
//			base.OnLoad(e);
//		}

		private string skinFilePath;
		public string SkinFilePath
		{
			get
			{return skinFilePath;}
			set{skinFilePath = value;}
		}
		
		protected void BindCurrentEntryControls(Entry entry, Control root)
		{
			foreach(Control control in root.Controls)
			{
				CurrentEntryControl currentEntryControl = control as CurrentEntryControl;
				if(currentEntryControl != null)
				{
					currentEntryControl.Entry = entry;
					currentEntryControl.DataBind();
				}
			}
		}
	}
}

