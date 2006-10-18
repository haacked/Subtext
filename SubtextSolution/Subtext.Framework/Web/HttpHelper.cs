using System;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Subtext.Framework.Text;

namespace Subtext.Framework.Web
{
	/// <summary>
	/// Static containing helper methods for HTTP operations.
	/// </summary>
	public static class HttpHelper
	{
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
		public static HttpWebRequest CreateRequest(Uri url) 
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
		public static HttpWebResponse GetResponse(Uri url)
		{
			HttpWebRequest request = CreateRequest(url);
			return (HttpWebResponse)request.GetResponse() ;
		}		

		/// <summary>
		/// Returns the text of the page specified by the URL..
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public static string GetPageText(Uri url)
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
		public static IPAddress GetUserIpAddress(HttpContext context)
		{
			if (context == null) return IPAddress.None;

			string result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (String.IsNullOrEmpty(result))
			{
				result = HttpContext.Current.Request.UserHostAddress;
			}
			else
			{
				// Requests behind a proxy might contain multiple IP 
				// addresses in the forwarding header.
				if (result.IndexOf(',') > 0)
				{
					result = StringHelper.LeftBefore(result, ",");
				}
			}

			IPAddress ipAddress;
			if(IPAddress.TryParse(result, out ipAddress))
			{
				return ipAddress;
			}
			return IPAddress.None;
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

		/// <summary>
		/// Determines whether the request is for a static file.
		/// </summary>
		/// <returns>
		/// 	<c>true</c> if [is static file request]; otherwise, <c>false</c>.
		/// </returns>
		public static bool IsStaticFileRequest()
		{
			if(HttpContext.Current == null)
				return true;

			string filePath = HttpContext.Current.Request.Url.AbsolutePath;

			return filePath.EndsWith(".css", StringComparison.InvariantCultureIgnoreCase)
					|| filePath.EndsWith(".jpg", StringComparison.InvariantCultureIgnoreCase)
					|| filePath.EndsWith(".js", StringComparison.InvariantCultureIgnoreCase)
					|| filePath.EndsWith(".gif", StringComparison.InvariantCultureIgnoreCase)
					|| filePath.EndsWith(".png", StringComparison.InvariantCultureIgnoreCase)
					|| filePath.EndsWith(".xml", StringComparison.InvariantCultureIgnoreCase)
					|| filePath.EndsWith(".txt", StringComparison.InvariantCultureIgnoreCase)
					|| filePath.EndsWith(".html", StringComparison.InvariantCultureIgnoreCase)
					|| filePath.EndsWith(".htm", StringComparison.InvariantCultureIgnoreCase);
		}
	}
}
