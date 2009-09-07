#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using Ninject;
using Subtext.BlogML;
using Subtext.BlogML.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;
using Subtext.Framework.Services;

namespace Subtext.ImportExport
{
    //TODO: Fix this. We need to pull BlogMLHttpHandler into Subtext.Framework
    public class SubtextBlogMLHttpHandler : BlogMLHttpHandler, ISubtextHandler
    {
        public override IBlogMLProvider GetBlogMLProvider()
        {
            var handler = new SubtextBlogMLProvider(Config.ConnectionString, SubtextContext, new CommentService(SubtextContext, null), SubtextContext.GetService<IEntryPublisher>());
            handler.PageSize = 100;
            return handler;
        }

        public Blog Blog
        {
            get
            {
                return SubtextContext.Blog;
            }
        }

        public UrlHelper Url
        {
            get
            {
                return SubtextContext.UrlHelper;
            }
        }

        public ObjectProvider Repository
        {
            get
            {
                return SubtextContext.Repository;
            }
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
        AdminUrlHelper _adminUrlHelper;

        [Inject]
        public ISubtextContext SubtextContext
        {
            get;
            set;
        }
    }
}
