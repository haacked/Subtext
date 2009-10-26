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
using System.Globalization;
using System.Web;
using System.Web.UI;

namespace Subtext.Web.Admin.WebUI
{
    public class HeaderBase : Control
    {
        private string _appPath;
        private string _relativePath = String.Empty;

        public HeaderBase()
        {
            _appPath = HttpContext.Current.Request.Url.GetLeftPart(UriPartial.Authority) +
                       HttpContext.Current.Request.ApplicationPath;
        }

        public string RelativePath
        {
            get { return _relativePath.StartsWith("/") ? _relativePath.Substring(1) : _relativePath; }
            set { _relativePath = value; }
        }

        public string AppPath
        {
            get { return _appPath.EndsWith("/") ? _appPath : _appPath + "/"; }
            set { _appPath = value; }
        }

        protected override void Render(HtmlTextWriter output)
        {
            output.Write(string.Format(CultureInfo.InvariantCulture, "<base href=\"{0}{1}/\" />", AppPath, RelativePath));
        }
    }
}