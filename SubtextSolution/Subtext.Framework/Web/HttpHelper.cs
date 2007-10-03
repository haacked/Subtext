using System;
using System.Configuration;
using System.IO;
using System.Net;
using System.Text;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Properties;

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
		public static void SetFileNotFoundResponse()
		{
			if(HttpContext.Current != null && HttpContext.Current.Response != null)
			{
				SetFileNotFoundResponse(Config.GetFileNotFoundPage());
			}
		}

		/// <param name="fileNotFoundPage">The file not found page.</param>
		private static void SetFileNotFoundResponse(string fileNotFoundPage)
		{
			HttpContext.Current.Response.StatusCode = 404;
			if (fileNotFoundPage != null)
				HttpContext.Current.Response.Redirect(fileNotFoundPage, true);
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
			+ " (" + Environment.OSVersion + "; .NET CLR " + Environment.Version + ")";

		
		/// <summary>
		/// Creates an <see cref="HttpWebRequest" /> for the specified URL..
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <returns></returns>
		public static HttpWebRequest CreateRequest(Uri url) 
		{
			WebRequest req = WebRequest.Create(url);
			SetProxy(req);
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
				Encoding encode = Encoding.GetEncoding(enc);
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
		
		/// <summary>
		/// Sets the proxy on the request if a proxy is configured in Web.config.
		/// </summary>
		/// <param name="request"></param>
		public static void SetProxy(WebRequest request)
		{
            if (request == null)
            {
                throw new ArgumentNullException(Resources.ArgumentNull_Generic);
            }

			IWebProxy proxy = GetProxy();
			if(proxy != null)
				request.Proxy = proxy;
		}

		internal static IWebProxy GetProxy()
		{
			if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyHost"]))
				return null;
			
			IWebProxy proxy;
			string proxyHost = ConfigurationManager.AppSettings["ProxyHost"];

			int proxyPort;
			if (int.TryParse(ConfigurationManager.AppSettings["ProxyPort"], out proxyPort))
			{
				proxy = new WebProxy(proxyHost, proxyPort);
			}
			else
			{
				proxy = new WebProxy(proxyHost);
			}
			if (!String.IsNullOrEmpty(ConfigurationManager.AppSettings["ProxyUsername"]))
			{
				proxy.Credentials = new NetworkCredential(ConfigurationManager.AppSettings["ProxyUsername"], ConfigurationManager.AppSettings["ProxyPassword"]);
			}
			return proxy;
		}

	    /// <summary>
	    /// If the URL is is the format ~/SomePath, this 
	    /// method expands the tilde using the app path.
	    /// </summary>
	    /// <param name="path"></param>
	    public static string ExpandTildePath(string path)
	    {
	        string reference = path;
	        if(reference.Substring(0, 2) == "~/")
	        {
	            string appPath = HttpContext.Current.Request.ApplicationPath;
	            if(appPath == null)
	                appPath = string.Empty;
	            if(appPath.EndsWith("/"))
	            {
	                appPath = StringHelper.Left(appPath, appPath.Length - 1);
	            }
	            return appPath + reference.Substring(1);
	        }
	        return path;
	    }

      internal static bool IsWebResource()
      {
         if (HttpContext.Current == null)
            return true;

         string filePath = HttpContext.Current.Request.Url.AbsolutePath;

         return filePath.EndsWith("WebResource.axd", StringComparison.InvariantCultureIgnoreCase);
      }
   }
}
