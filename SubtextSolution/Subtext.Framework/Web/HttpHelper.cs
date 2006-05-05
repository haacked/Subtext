using System;
using System.Web;
using log4net;

namespace Subtext.Framework.Web
{
	/// <summary>
	/// Static containing helper methods for HTTP operations.
	/// </summary>
	public sealed class HttpHelper
	{
		static ILog Log = new Subtext.Framework.Logging.Log();

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
	}
}
