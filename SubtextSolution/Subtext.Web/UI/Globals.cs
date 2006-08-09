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
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI
{
	/// <summary>
	/// Summary description for Globals.
	/// </summary>
	public static class Globals
	{
		/// <summary>
		/// Returns the current skin for the current context.
		/// </summary>
		/// <returns></returns>
		public static string Skin()
		{
            if (Config.CurrentBlog.Skin.TemplateFolder == null)
            {
                Config.CurrentBlog.Skin = SkinConfig.GetDefaultSkin();
            }
            return Config.CurrentBlog.Skin.TemplateFolder;
		}

		private static readonly string BlogPageTitle = "BlogPageTitle";

		/// <summary>
		/// This method will be called during PreRender. If no title was set via
		/// SetTitle(title, context), then we will default to the blog title
		/// </summary>
		/// <param name="context"></param>
		/// <returns></returns>
		public static string CurrentTitle(HttpContext context)
		{
			string title = (string)context.Items[BlogPageTitle];
			if(title == null)
			{
				title = Config.CurrentBlog.Title;
			}
			return title;
		}


		/// <summary>
		/// Allows the page title to be set anywhere within the request.
		/// </summary>
		/// <param name="title">Title.</param>
		/// <param name="context">Context.</param>
		public static void SetTitle(string title, HttpContext context)
		{
			context.Items[BlogPageTitle] = title;
		}
	}
}
