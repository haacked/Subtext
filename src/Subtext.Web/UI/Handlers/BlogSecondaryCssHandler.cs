#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Text;
using System.Web;
using Subtext.Framework.Configuration;

//todo: make this a controller with unit tests.
namespace Subtext.Web.UI.Handlers
{
    /// <summary>
    /// HTTP Handler for rendering a CSS stylesheet.  
    /// This renders the CSS markup stored in the Secondary CSS field within 
    /// the admin options for a blog configuration.
    /// </summary>
    public class BlogSecondaryCssHandler : IHttpHandler
    {
        /// <summary>
        /// Processes the request.
        /// </summary>
        /// <param name="context">Context.</param>
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentEncoding = Encoding.UTF8;
            context.Response.ContentType = "text/css";

            var httpContext = new HttpContextWrapper(context);
            var skin = SkinConfig.GetCurrentSkin(Config.CurrentBlog, httpContext);
            if (skin != null)
            {
                context.Response.Write(skin.CustomCssText);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this handler is reusable.
        /// </summary>
        /// <remarks>
        /// This handler is not reusable.
        /// </remarks>
        /// <value>
        /// 	<c>true</c> if is reusable; otherwise, <c>false</c>.
        /// </value>
        public bool IsReusable
        {
            get { return false; }
        }
    }
}