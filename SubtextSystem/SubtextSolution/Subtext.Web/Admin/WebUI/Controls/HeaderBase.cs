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
using System.Web.UI;
using Subtext.Framework.Format;

namespace Subtext.Web.Admin.WebUI
{
	public class HeaderBase : Control
	{		
		private string _relativePath = String.Empty;
		private string _appPath;

		public HeaderBase()
		{
			_appPath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) + 
				HttpContext.Current.Request.ApplicationPath;
		}

		protected override void Render(HtmlTextWriter output)
		{	
			output.Write(string.Format(System.Globalization.CultureInfo.InvariantCulture, "<base href=\"{0}{1}/\" />", AppPath, RelativePath));
		}

		public string RelativePath
		{
			get	
			{ 
				return _relativePath.StartsWith("/") ? _relativePath.Substring(1) : _relativePath;
			}
			set { _relativePath = value;}
		}

		public string AppPath
		{
			get 
			{ 
				return _appPath.EndsWith("/") ? _appPath : _appPath + "/";
			}
			set { _appPath = value; }
		}
	}
}
