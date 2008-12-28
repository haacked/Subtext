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
using System.IO;
using Subtext.Framework;
using Subtext.Framework.Format;
using Subtext.Framework.Text;

namespace Subtext.Web.SystemMessages
{
	/// <summary>
	/// Displays a file not found message to the user.
	/// </summary>
	public partial class FileNotFound : System.Web.UI.Page
	{

		protected override void OnLoad(EventArgs e)
		{
			//TODO: Refactor this into a method and unit test it.
			//Multiple blog handling.

			//Since we were redirected here, 
			// we want to make sure we send back a 404 and not a 200   -DF
			Response.StatusCode = 404;
			Response.Status = "404 Not Found";

			string queryString;
			if (Request.QueryString.Count == 0)
			{
				return;
			}

			queryString = Request.QueryString[0];

			if (queryString != null && queryString.Length > 0)
			{
				string urlText = StringHelper.RightAfter(queryString, ";");
				if (urlText != null && urlText.Length > 0)
				{
					Uri uri = HtmlHelper.ParseUri(urlText);
					if (uri == null)
						return;

					string extension = Path.GetExtension(uri.AbsolutePath);
					if (extension == null || extension.Length == 0)
					{
						string uriAbsolutePath = uri.AbsolutePath;
						if (!uriAbsolutePath.EndsWith("/"))
						{
							uriAbsolutePath += "/";
						}
						string subfolder = UrlFormats.GetBlogSubfolderFromRequest(uriAbsolutePath, Request.ApplicationPath);
						Blog info = Subtext.Framework.Configuration.Config.GetBlogInfo(uri.Host, subfolder);
						if (info != null)
						{
							Response.Redirect(uriAbsolutePath + "Default.aspx");
							return;
						}
					}
				}
			}

			base.OnLoad(e);
		}


	}
}
