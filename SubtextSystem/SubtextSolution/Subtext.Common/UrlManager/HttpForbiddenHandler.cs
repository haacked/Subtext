using System;
using System.Web;

namespace Subtext.Common.UrlManager
{
	/// <summary>
	/// System.Web.HttpForbiddenHandler is not accessible.  
	/// This just simulates System.Web.HttpForbiddenHandler.
	/// </summary>
	public class HttpForbiddenHandler : System.Web.IHttpHandler
	{
		public void ProcessRequest(HttpContext context)
		{
			throw new HttpException(403, "Invalid Path: " + context.Request.Path);
		}

		public bool IsReusable
		{
			get
			{
				return false;
			}
		}
	}
}
