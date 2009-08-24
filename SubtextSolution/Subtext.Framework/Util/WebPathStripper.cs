#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;

//This might need to somehow be provider based. Or even Globalized. Not all dates will be US :)

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Summary description for WebPathStripper.
	/// </summary>
	public static class WebPathStripper
	{
		/// <summary>
		/// Return the value of a url between /category/ and /rss
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string GetCategryFromRss(string url)
		{
			url = url.ToLower(CultureInfo.InvariantCulture);
			int start = url.IndexOf("/category/");
			int stop = url.IndexOf("/rss");
			return url.Substring(start+10,stop-(start+10)).Replace(".aspx",string.Empty);			
		}

		/// <summary>
		/// Removes the trailing RSS slash if there.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <returns></returns>
		public static string RemoveRssSlash(string url)
		{
			if (url.EndsWith("/"))
				url = url.Substring(0,url.Length - 1);
			return Regex.Replace(url, "/rss$", string.Empty);
		}
	}
}
