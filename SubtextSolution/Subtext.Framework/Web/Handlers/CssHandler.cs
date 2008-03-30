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
using System.IO;
using System.Text;
using System.Web;
using Subtext.Extensibility.Web;
using Subtext.Framework.UI.Skinning;

namespace Subtext.Framework.Web.Handlers
{
	public class CssHandler : BaseHttpHandler
	{
        private static readonly StyleSheetElementCollectionRenderer styleRenderer = new StyleSheetElementCollectionRenderer(new SkinTemplateCollection());

		public override void HandleRequest(HttpContext context)
		{
			context.Response.ContentEncoding = Encoding.UTF8;

		    string skinName = context.Request.Params["name"];
            string skinMedia = context.Request.Params["media"];
            string skinTitle = context.Request.Params["title"];
            string skinConditional = context.Request.Params["conditional"];

            List<StyleDefinition> styles = (List<StyleDefinition>)styleRenderer.GetStylesToBeMerged(skinName, skinMedia, skinTitle, skinConditional);
          
            //Append all styles into one file

            context.Response.Write("/*" + Environment.NewLine);
            foreach (StyleDefinition style in styles)
            {
                context.Response.Write(style + Environment.NewLine);
            }
            context.Response.Write("*/" + Environment.NewLine);

            foreach (StyleDefinition style in styles)
		    {
                context.Response.Write(Environment.NewLine + "/* " + style + " */" + Environment.NewLine);
		        string path = context.Server.MapPath(style.Href);
                if(File.Exists(path))
                {
                    
                    string cssFile = File.ReadAllText(path);

                    if (!String.IsNullOrEmpty(style.Media) && styles.Count>1)
                    {
                        context.Response.Write("@media " + style.Media + "{\r\n");
                        context.Response.Write(cssFile);
                        context.Response.Write("\r\n}");
                    }
                    else
                        context.Response.Write(cssFile);
                        
                }
                else
                {
                    context.Response.Write(Environment.NewLine + "/* CSS file at " + path + " doesn't exist so cannot be included in the merged CSS file. */" + Environment.NewLine);
                }
		    }

            SetHeaders(styles, context);
		}


        private static void SetHeaders(List<StyleDefinition> styles, HttpContext context)
        {
            foreach (StyleDefinition style in styles)
            {
                context.Response.AddFileDependency(context.Server.MapPath(style.Href));
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
