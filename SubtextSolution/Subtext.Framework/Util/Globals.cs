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
			if (null == result || result.Length == 0)
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

