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
	public class WebPathStripper
	{
		private WebPathStripper()
		{

		}

		public static int YearFromRequest(string uri)
		{
			return GetEntryIDFromUrl(uri);
		}

		public static DateTime GetDateFromRequest(string uri, string archiveText)
		{
			uri = uri.ToLower();
			uri = CleanStartDateString(uri,archiveText);
			uri = CleanEndDateString(uri);
			return DateTime.ParseExact(uri,dateFormats,new CultureInfo("en-US"),DateTimeStyles.None);
		}

		private static string CleanStartDateString(string uri, string archiveText)
		{
			return uri.Remove(0,uri.LastIndexOf(archiveText) + archiveText.Length+1);
		}

		private static string CleanEndDateString(string uri)
		{
			return Regex.Replace(uri,@"(/|\.aspx)$",string.Empty,RegexOptions.IgnoreCase);
		}

		private static readonly string[] dateFormats = {"yyyy/MM/d","yyyy/MM/dd","yyyy/M/dd","yyyy/M/d","yyyy/MM","yyyy/M"};
		


		public static bool IsNumeric(string text)
		{
			return Regex.IsMatch(text,"^\\d+$");
		}

		/// <summary>
		/// Return the value of a url between /category/ and /rss
		/// </summary>
		/// <param name="url"></param>
		/// <returns></returns>
		public static string GetCategryFromRss(string url)
		{
			url = url.ToLower();
			int start = url.IndexOf("/category/");
			int stop = url.IndexOf("/rss");
			return url.Substring(start+10,stop-(start+10)).Replace(".aspx",string.Empty);			
		}

		public static string RemoveRssSlash(string url)
		{
			return Regex.Replace(url,"/rss$",string.Empty);
		}

		public static string GetReqeustedFileName(string uri)
		{
			return Path.GetFileNameWithoutExtension(uri);
		}

		public static int GetEntryIDFromUrl(string uri)
		{
			try
			{
				return Int32.Parse(GetReqeustedFileName(uri));
			}
			catch
			{
				throw new ArgumentException("Invalid Uri. Integer can not be found");
			}			
		}

		public static string GetBlogAppFromRequest(string path, string app)
		{
			if(!app.StartsWith("/"))
			{
				app = "/" + app;
			}
			if(!app.EndsWith("/"))
			{
				app += "/";
			}
			if(path.StartsWith(app))
			{
				path = path.Remove(0,app.Length);
			}
			int lastSlash = path.IndexOf("/");
			if(lastSlash > -1)
			{
				path = path.Remove(lastSlash,path.Length -lastSlash);
			}
			return path;
		}
	}
}
