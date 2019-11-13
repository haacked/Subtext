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

using Subtext.Framework.Security;
using Subtext.Framework.UI.Skinning;
using Subtext.Web.Admin.WebUI.Controls;

namespace Subtext.Web.Skins._System.Controls
{
    public partial class Error : BaseUserControl, IErrorControl
    {
        public SkinControlLoadException Exception
        {
            get;
            set;
        }

        public bool ShowErrorDetails
        {
            get
            {
                return SubtextContext.HttpContext.Request.IsLocal || SubtextContext.User.IsAdministrator();
            }
        }


        public string ControlPath
        {
            get;
            set;
        }
    }
}