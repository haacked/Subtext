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

using System.Text.RegularExpressions;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Summary description for WebPathStripper.
	/// </summary>
	public static class WebPathStripper
	{
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
