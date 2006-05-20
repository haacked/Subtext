using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;

namespace Subtext.Framework.Web
{
	/// <summary>
	/// Static containing helper methods for HTTP operations.
	/// </summary>
	public sealed class HttpHelper
	{
		private HttpHelper()
		{
		}

		/// <summary>
		/// Sets the file not found response.
		/// </summary>
		/// <param name="fileNotFoundPage">The file not found page.</param>
		public static void SetFileNotFoundResponse(string fileNotFoundPage)
		{
			if(HttpContext.Current != null && HttpContext.Current.Response != null)
			{
				HttpContext.Current.Response.StatusCode = 404;
				HttpContext.Current.Response.Redirect(fileNotFoundPage, true);
			}
		}

		/// <summary>
		/// Gets if modified since date.
		/// </summary>
		/// <returns></returns>
		public static DateTime GetIfModifiedSinceDateUTC()
		{
			if(HttpContext.Current != null && HttpContext.Current.Request != null)
			{
				string ifModified = HttpContext.Current.Request.Headers["If-Modified-Since"];
				if(ifModified != null && ifModified.Length > 0)
				{
					return DateTimeHelper.ParseUnknownFormatUTC(ifModified);
				}
			}
			return NullValue.NullDateTime;
		}
		
		private const int defaultTimeout = 60000;
		private static string referer = @"http://SubtextProject.com/Services/default.htm";
		private static readonly string userAgent = VersionInfo.UserAgent
			+ " (" + Environment.OSVersion.ToString() + "; .NET CLR " + Environment.Version.ToString() + ")";

		
		/// <summary>
		/// Creates an <see cref="HttpWebRequest" /> for the specified URL..
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public static HttpWebRequest CreateRequest(string url) 
		{
			WebRequest req = WebRequest.Create(url);
			
			HttpWebRequest wreq = req as HttpWebRequest;
			if (null != wreq) 
			{
				wreq.UserAgent = userAgent;
				wreq.Referer =  referer;
				wreq.Timeout = defaultTimeout;
			}
			return wreq;
		}	

		/// <summary>
		/// Returns an <see cref="HttpWebResponse" /> for the specified URL.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public static HttpWebResponse GetResponse(string url)
		{
			HttpWebRequest request = CreateRequest(url);
			return (HttpWebResponse)request.GetResponse() ;
		}		

		/// <summary>
		/// Returns the text of the page specified by the URL..
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public static string GetPageText(string url)
		{
			HttpWebResponse response = GetResponse(url);
			using (Stream s = response.GetResponseStream())
			{
				string enc = response.ContentEncoding;
				if (enc == null || enc.Trim().Length == 0)
					enc = "us-ascii" ;
				Encoding encode = System.Text.Encoding.GetEncoding(enc);
				using ( StreamReader sr = new StreamReader(s, encode))
				{
					return sr.ReadToEnd() ;
				}
			}
		}
		
		/// <summary>
		/// Returns the IP Address of the user making the current request.
		/// </summary>
		/// <param name="context">The context.</param>
		/// <returns></returns>
		public static string GetUserIpAddress(HttpContext context)
		{
			string result = String.Empty;
			if (context == null) return result;

			result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (null == result || result.Length == 0)
				result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

			return result;
		}
		
		/// <summary>
		/// Combines Two Web Paths much like the Path.Combine method.
		/// </summary>
		/// <param name="uriOne">The URI one.</param>
		/// <param name="uriTwo">The URI two.</param>
		/// <returns></returns>
		public static string CombineWebPaths(string uriOne, string uriTwo)
		{
			string newUri = (uriOne + uriTwo);
			return newUri.Replace("//", "/");
		}
	}
}
