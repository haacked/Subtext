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
using System.Collections.Generic;
using System.Text;
using System.Web;
using Subtext.Extensibility.Web;
using Subtext.Framework.UI.Skinning;

namespace Subtext.Framework.Web.Handlers
{
	public class CssHandler : BaseHttpHandler
	{
        private static readonly StyleSheetElementCollectionRenderer styleRenderer = new StyleSheetElementCollectionRenderer(SkinTemplates.Instance());

		public override void HandleRequest(HttpContext context)
		{
			context.Response.ContentEncoding = Encoding.UTF8;

		    string skinName = context.Request.Params["name"];
            string skinMedia = context.Request.Params["media"];
            string skinTitle = context.Request.Params["title"];
            string skinConditional = context.Request.Params["conditional"];

            List<string> styles = (List<string>)styleRenderer.GetStylesToBeMerged(skinName, skinMedia, skinTitle, skinConditional);
          
            //Append all styles into one file

            foreach (string style in styles)
            {
                context.Response.Write("/* -- " + style + " -- */\r\n");
            }
            
            context.Response.Write("\r\n");

		    foreach (string style in styles)
		    {
                context.Response.Write("/* -- " + style + " -- */\r\n");
                context.Response.WriteFile(context.Server.MapPath(style));
		    }

            SetHeaders(styles, context);
		}


        private static void SetHeaders(List<string> styles, HttpContext context)
        {
            foreach (string style in styles)
            {
                context.Response.AddFileDependency(context.Server.MapPath(style));
            }
            
            context.Response.Cache.VaryByParams["name"] = true;
            context.Response.Cache.VaryByParams["media"] = true;
            context.Response.Cache.VaryByParams["title"] = true;
            context.Response.Cache.VaryByParams["conditional"] = true;

            context.Response.Cache.SetValidUntilExpires(true);
            // Client-side caching
            context.Response.Cache.SetLastModifiedFromFileDependencies();
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
        }

        public override void SetResponseCachePolicy(HttpCachePolicy cache)
        {
            return;
        }

        public new bool IsReusable
        {
            get
            {
                return false;
            }
        }

		public override bool ValidateParameters(HttpContext context)
		{
            string skinName = context.Request.Params["name"];
            if (String.IsNullOrEmpty(skinName))
                return false;
            else
                return true;
		}

		public override bool RequiresAuthentication
		{
			get { return false; }
		}

		public override string ContentMimeType
		{
			get { return "text/css"; }
		}
	}
}
