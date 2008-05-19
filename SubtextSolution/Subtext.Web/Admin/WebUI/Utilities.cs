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
using System.Collections;
using System.Collections.Specialized;
using System.IO;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework.Components;
using Subtext.Framework.Web;

namespace Subtext.Web.Admin
{
	public static class Utilities
	{
		public static string ResourcePath
		{
			get
			{
				return HttpHelper.CombineWebPaths(HttpContext.Current.Request.ApplicationPath,  "/admin/");
			}
		}

		internal static string AbsolutePath(string path)
		{
			const string ADMIN_DEFAULT = "/admin";

			if (null != HttpContext.Current)
			{
				if (null != path && path.StartsWith("~"))
				{
					string relativePath = path.Substring(1, path.Length - 1);				
					string appPath = HttpContext.Current.Request.ApplicationPath;

					if(relativePath.StartsWith(ADMIN_DEFAULT, StringComparison.InvariantCultureIgnoreCase))
					{
						relativePath = relativePath.Replace(ADMIN_DEFAULT, 
							"admin");
					}
					
					if (relativePath.StartsWith("/"))
						relativePath = relativePath.Substring(1, relativePath.Length - 1);

					return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}{1}{2}", appPath, appPath.EndsWith("/") ? "" : "/",
						relativePath);
				}
				else
					return path;
			}
			else
				return path;
		}

		internal static bool IsNullorEmpty(String test)
		{
			return (test == null || test.Length <= 0);
		}

		internal static string Truncate(string value, int numChars)
		{
			return Truncate(value, numChars, true);
		}

		internal static string Truncate(string value, int numChars, bool useSuffix)
		{
			if (value.Length > numChars)
			{
			
				if (useSuffix && (value.Length > numChars - Constants.TRUNCATE_SUFFIX.Length))
					return value.Substring(0, numChars - Constants.TRUNCATE_SUFFIX.Length) 
						+ Constants.TRUNCATE_SUFFIX;
				else
					return value.Substring(0, numChars);

			}
			else
				return value;
		}

		internal static LinkButton CreateLinkButton(string text)
		{
			LinkButton result = new LinkButton();
			result.Text = text;
			return result;
		}

		internal static HyperLink CreateHyperLink(string text, string url)
		{
			HyperLink result = new HyperLink();
			result.Text = text;
			result.NavigateUrl = url;
			return result;
		}

		internal static WebControl CopyStyles(WebControl control, NameValueCollection styles)
		{
			ICollection styleKeys = styles.Keys;
			foreach (string key in styleKeys)
				control.Style.Add(key, styles[key]);

			return control;
		}

		internal static Encoding GetEncoding(string filepath)
		{
			Encoding result;
			FileStream file = null;
			try 
			{				
				file = new FileStream(filepath, FileMode.Open, FileAccess.Read, FileShare.Read);
			
				if (file.CanSeek)
				{
					// get the bom, if there is one
					byte[] bom = new byte[4]; 
					file.Read(bom, 0, 4);

					// utf-8
					if (bom[0] == 0xef && bom[1] == 0xbb && bom[2] == 0xbf)
						result = System.Text.Encoding.UTF8;
						// ucs-2le, ucs-4le, ucs-16le, utf-16, ucs-2, ucs-4
					else if ((bom[0] == 0xff && bom[1] == 0xfe) || 
						(bom[0] == 0xfe && bom[1] == 0xff) || 
						(bom[0] == 0 && bom[1] == 0 && bom[2] == 0xfe && bom[3] == 0xff))
						result = System.Text.Encoding.Unicode;
						// else ascii
					else
						result = System.Text.Encoding.ASCII;
				}
				else
				{
					// can't detect, set to default
					result = System.Text.Encoding.ASCII;
				}

				return result;
			}
			finally
			{
				if (null != file) file.Close();
			}
		}

	}

	internal sealed class Keys 
	{
		private Keys() {}

		internal const string SESSION_COOKIETEST = "AdminPageCookieTest";

		internal const string QRYSTR_PAGEINDEX = "pg";
		internal const string QRYSTR_CATEGORYID = "catid";
        internal const string QRYSTR_CATEGORYTYPE = "catType";
		internal const string QRYSTR_IMAGEID = "imgid";
	}

	internal sealed class Constants
	{
		private Constants() {}

		internal const string TRUNCATE_SUFFIX = "...";
		internal const string CSSSTYLE_HIDDEN = "display: none;";

		internal const CategoryType DEFAULT_CATEGORYTYPE = CategoryType.PostCollection;

		internal const int PAGE_SIZE_DEFAULT = 10;
		internal const bool ALWAYS_EXPAND_DEFAULT = false;
		internal const bool CREATE_ISACTIVE_DEFAULT = true;

		internal const string URL_CONFIRM = "Confirm.aspx";
		internal const string URL_EDITCATEGORIES = "EditCategories.aspx";
		internal const string URL_EDITGALLERIES = "EditGalleries.aspx";

		internal const string RES_SUCCESSNEW = "Posted successfully.";
		internal const string RES_SUCCESSEDIT = "Updated successfully.";
		internal const string RES_FAILUREEDIT = "Post operation failed.";
		internal const string RES_EXCEPTION = "{0} The error message related to this problem was as follows: {1}";

		internal const string RES_SUCCESSCATEGORYUPDATE = "Categories updated successfully.";
		internal const string RES_FAILURECATEGORYUPDATE = "Category Update failed.";
	}
}

