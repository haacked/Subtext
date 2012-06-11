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
using Ninject;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services.SearchEngine;

namespace Subtext.Framework.Web.Handlers
{
    public class SubtextPage : Page, ISubtextDependencies
    {
        AdminUrlHelper _adminUrlHelper;

        public Blog Blog
        {
            get { return SubtextContext.Blog; }
        }

        [Inject]
        public ISubtextContext SubtextContext { get; set; }

        [Inject]
        public ISearchEngineService SearchEngineService { get; set; }

        public BlogUrlHelper Url
        {
            get { return SubtextContext.UrlHelper; }
        }

        public ObjectRepository Repository
        {
            get { return SubtextContext.Repository; }
        }

        public AdminUrlHelper AdminUrl
        {
            get
            {
                if (_adminUrlHelper == null)
                {
                    _adminUrlHelper = new AdminUrlHelper(Url);
                }
                return _adminUrlHelper;
            }
        }
    }
}