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
    public class JsHandler : BaseHttpHandler
    {
        private static readonly ScriptElementCollectionRenderer scriptRenderer = new ScriptElementCollectionRenderer(new SkinTemplateCollection());

        protected override void HandleRequest(HttpContext context) {
            context.Response.ContentEncoding = Encoding.UTF8;

            string skinName = context.Request.Params["name"];

            List<string> scripts = (List<string>) scriptRenderer.GetScriptsToBeMerged(skinName);

            //Append all styles into one file

            context.Response.Write("/*" + Environment.NewLine);
            foreach (string script in scripts)
            {
                context.Response.Write(script + Environment.NewLine);
            }
            context.Response.Write("*/" + Environment.NewLine);

            foreach (string script in scripts)
            {
                context.Response.Write(Environment.NewLine + "/* " + script + " */" + Environment.NewLine);
                string path = context.Server.MapPath(script);
                if (File.Exists(path))
                {
                    string jsFile = File.ReadAllText(context.Server.MapPath(script));
                    context.Response.Write(jsFile);
                }
                else
                {
                    context.Response.Write(Environment.NewLine + "/* JS file at " + path + " doesn't exist so cannot be included in the merged JS file. */" + Environment.NewLine);
                }
            }

            SetHeaders(scripts, context);
        }


        private static void SetHeaders(List<string> styles, HttpContext context) {
            foreach (string style in styles) {
                context.Response.AddFileDependency(context.Server.MapPath(style));
            }

            context.Response.Cache.VaryByParams["name"] = true;

            context.Response.Cache.SetValidUntilExpires(true);
            // Client-side caching
            context.Response.Cache.SetLastModifiedFromFileDependencies();
            context.Response.Cache.SetCacheability(HttpCacheability.Public);
        }


        protected override void SetResponseCachePolicy(HttpCachePolicy cache) {
            return;
        }

        public override bool IsReusable
        {
            get {
                return false;
            }
        }

        protected override bool ValidateParameters(HttpContext context)
        {
            string skinName = context.Request.Params["name"];
            if (String.IsNullOrEmpty(skinName))
                return false;
            else
                return true;
        }

        protected override bool RequiresAuthentication
        {
            get { return false; }
        }

        protected override string ContentMimeType
        {
            get { return "text/javascript"; }
        }

    }


}
