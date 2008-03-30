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
using System.IO.Compression;
using System.Web;

namespace Subtext.Framework.Web.HttpModules
{
    public class CompressionModule : IHttpModule
    {

        #region IHttpModule Members

        /// <summary>
        /// Disposes of the resources (other than memory) used by the module 
        /// that implements <see cref="T:System.Web.IHttpModule"></see>.
        /// </summary>
        void IHttpModule.Dispose()
        {
            // Nothing to dispose; 
        }

        /// <summary>
        /// Initializes a module and prepares it to handle requests.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpApplication"></see> 
        /// that provides access to the methods, properties, and events common to 
        /// all application objects within an ASP.NET application.
        /// </param>
        void IHttpModule.Init(HttpApplication context)
        {
            context.PostReleaseRequestState += context_PostReleaseRequestState;
        }

        #endregion

        private const string GZIP = "gzip";
        private const string DEFLATE = "deflate";

        /// <summary>
        /// Handles the BeginRequest event of the context control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        void context_PostReleaseRequestState(object sender, EventArgs e)
        {
            HttpApplication app = (HttpApplication) sender;
            if (app.Request.Path.Contains("css.axd") || app.Request.Path.Contains("js.axd"))
            {
                if (IsEncodingAccepted(GZIP))
                {
                    app.Response.Filter = new GZipStream(app.Response.Filter, CompressionMode.Compress);
                    SetEncoding(GZIP);
                }
                else if (IsEncodingAccepted(DEFLATE))
                {
                    app.Response.Filter = new DeflateStream(app.Response.Filter, CompressionMode.Compress);
                    SetEncoding(DEFLATE);
                }
            }
            else if (app.Request.Path.Contains("WebResource.axd"))
            {
                app.Response.Cache.SetExpires(DateTime.Now.AddDays(30));
            }
        }

        /// <summary>
        /// Checks the request headers to see if the specified
        /// encoding is accepted by the client.
        /// </summary>
        private static bool IsEncodingAccepted(string encoding)
        {
            return HttpContext.Current.Request.Headers["Accept-encoding"] != null && HttpContext.Current.Request.Headers["Accept-encoding"].Contains(encoding);
        }

        /// <summary>
        /// Adds the specified encoding to the response headers.
        /// </summary>
        /// <param name="encoding"></param>
        private static void SetEncoding(string encoding)
        {
            HttpContext.Current.Response.AppendHeader("Content-encoding", encoding);
        }

    }
}
