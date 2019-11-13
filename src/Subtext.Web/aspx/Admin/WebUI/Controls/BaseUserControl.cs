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

using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Web.Admin.WebUI.Controls
{
    public class BaseUserControl : UserControl
    {
        AdminUrlHelper _adminUrl;

        public BlogUrlHelper Url
        {
            get { return SubtextPage.Url; }
        }

        public AdminUrlHelper AdminUrl
        {
            get
            {
                _adminUrl = _adminUrl ?? new AdminUrlHelper(Url);
                return _adminUrl;
            }
        }

        public SubtextPage SubtextPage
        {
            get { return Page as SubtextPage; }
        }

        public ISubtextContext SubtextContext
        {
            get
            {
                return SubtextPage.SubtextContext;
            }
        }

        public ObjectRepository Repository
        {
            get
            {
                if (_repository == null)
                {
                    _repository = SubtextPage.SubtextContext.Repository;
                }
                return _repository;
            }
        }

        ObjectRepository _repository;
    }
}