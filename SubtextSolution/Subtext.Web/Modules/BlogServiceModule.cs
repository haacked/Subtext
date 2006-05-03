#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.IO;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;

namespace Subtext.Web.Modules
{
	/// <summary>
	/// Summary description for BlogServiceModule.
	/// </summary>
	public class BlogServiceModule : IHttpModule
	{
		void IHttpModule.Init(HttpApplication context) 
		{
			context.BeginRequest += new EventHandler(this.ReWriteServicePath);
					
		}

		void ReWriteServicePath(object sender, EventArgs e)
		{
			HttpContext context  = ((HttpApplication)sender).Context;
			
			if(StringHelper.IndexOf(context.Request.Path, "services", ComparisonType.CaseInsensitive) > 0 
				&& StringHelper.IndexOf(context.Request.Path, ".asmx", ComparisonType.CaseInsensitive) > 0)
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

