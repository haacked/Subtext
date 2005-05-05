using System;
using System.Web;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI
{
	/// <summary>
	/// Summary description for Globals.
	/// </summary>
	public class Globals
	{
		private Globals()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static string Skin()
		{
			return Skin(HttpContext.Current);
		}

		public static string Skin(HttpContext context)
		{
			if(Config.CurrentBlog(context).Skin.SkinName == null)
			{
				Config.CurrentBlog(context).Skin.SkinName = "marvin2";
			}
			return Config.CurrentBlog(context).Skin.SkinName;
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
				title = Config.CurrentBlog(context).Title;
			}
			return title;
		}

		//Allow the title to be set from anywhere in the request
		public static void SetTitle(string title, HttpContext context)
		{
			context.Items[BlogPageTitle] = title;
		}
	}
}
