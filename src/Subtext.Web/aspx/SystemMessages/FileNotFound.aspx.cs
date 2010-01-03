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

using System;
using System.Web;
using System.Web.UI;
using Subtext.Framework.Web;

namespace Subtext.Web.SystemMessages
{
    /// <summary>
    /// Displays a file not found message to the user.
    /// </summary>
    public partial class FileNotFound : Page
    {
        protected override void OnLoad(EventArgs e)
        {
            var httpContext = new HttpContextWrapper(HttpContext.Current);
            httpContext.HandleFileNotFound(HttpRuntime.UsingIntegratedPipeline);

            base.OnLoad(e);
        }
    }
}