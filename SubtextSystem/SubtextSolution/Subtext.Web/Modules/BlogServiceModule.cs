#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.IO;
using System.Web;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Modules
{
	/// <summary>
	/// Summary description for BlogServiceModule.
	/// </summary>
	public class BlogServiceModule : IHttpModule
	{
		public BlogServiceModule()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		void IHttpModule.Init(HttpApplication context) 
		{
			context.BeginRequest += new EventHandler(this.ReWriteServicePath);
					
		}

		void ReWriteServicePath(object sender, EventArgs e)
		{
			HttpContext context  = ((HttpApplication)sender).Context;
			if(context.Request.Path.ToLower().IndexOf("services") > 0 && context.Request.Path.ToLower().IndexOf(".asmx") > 0)
			{
				if(AlllowService(context))

				{
					string fileName = Path.GetFileName(context.Request.Path);
					context.RewritePath("~/Services/" + fileName);
				}
				else
				{
					context.Response.Clear();
					context.Response.End();
				}
			}
		}

		private bool AlllowService(HttpContext context)
		{
			return	( 
				Config.Settings.AllowServiceAccess &&
						(context.Request.RequestType == "GET" || Config.CurrentBlog.AllowServiceAccess)
					);

		}

		void IHttpModule.Dispose() { }

	}
}

