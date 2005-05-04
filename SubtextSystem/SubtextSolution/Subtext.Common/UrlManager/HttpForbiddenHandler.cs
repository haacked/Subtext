using System;
using System.Web;

namespace Subtext.Common.UrlManager
{
	/// <summary>
	/// System.Web.HttpForbiddenHandler is not accessible. This just simulates System.Web.HttpForbiddenHandler.
	/// </summary>
	public class HttpForbiddenHandler : System.Web.IHttpHandler
	{
		public HttpForbiddenHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}
		#region IHttpHandler Members

		public void ProcessRequest(HttpContext context)
		{
			throw new HttpException(403,"Invalid Path: " + context.Request.Path);
		}

		public bool IsReusable
		{
			get
			{
				// TODO:  Add HttpForbiddenHandler.IsReusable getter implementation
				return false;
			}
		}

		#endregion
	}
}
