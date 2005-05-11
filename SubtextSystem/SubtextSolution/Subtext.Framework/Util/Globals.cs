#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Static methods with nowhere to go.  Let's get rid of this.
	/// </summary>
	public sealed class Globals
	{
		private Globals() {}

		//TODO: These methods belong somewhere else. Let's find them new homes.
		#region General




		public static int GetIntQS(string qs)
		{
			return Int32.Parse(HttpContext.Current.Request.QueryString[qs]);
		}

		public static string GetStringQS(string qs)
		{
			return HttpContext.Current.Request.QueryString[qs];
		}

		public static bool CheckNullQS(string qs)
		{
			return HttpContext.Current.Request.QueryString[qs] != null;
		}

		public static string GetUserIpAddress(HttpContext context)
		{
			string result = String.Empty;
			if (context == null) return result;

			result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (null == result || result == String.Empty)
				result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

			return result;
		}

		#endregion

		#region Formatting

		public static string FormatApplicationPath(string app)
		{
			if(app == null)
				return string.Empty;

			if(app == "/")
			{
				return string.Empty;
			}

			if(app.StartsWith("//"))
			{
				app = app.Substring(1);
			}

			if(app != null)
			{
				if(!app.StartsWith("/"))
				{
					app = "/" + app;
				}

				if(!app.EndsWith("/"))
				{
					app += "/";
				}
			}
			return app;
		}

		public static string DateString(DateTime dt, Char c)
		{
			switch(c)
			{
				case 'm':
				case 'M':
					return dt.ToString("yyyy/MM");
				case 'd':
				case 'D':
					return dt.ToString("yyyy/MM/dd");
				case 'y':
				case 'Y':
					return dt.ToString("yyyy");
				default:
					return string.Empty;
			}
		}

			#endregion

		#region Helper

		public static int AllowedItemCount(int ItemCount)
		{
			if(ItemCount > Config.Settings.ItemCount)
			{
				ItemCount = Config.Settings.ItemCount;
			}
			return ItemCount;
		}

		public static string WebPathCombine(string uriOne, string uriTwo)
		{
			string newUri = (uriOne + uriTwo);
			return newUri.Replace("//","/");
		}

		#endregion






		
	}
}

