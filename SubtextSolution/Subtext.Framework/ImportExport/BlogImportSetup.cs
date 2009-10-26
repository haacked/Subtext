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
using Subtext.Framework;
using Subtext.Framework.Providers;

namespace Subtext.ImportExport
{
    /// <summary>
    /// Sets up a blog for import. The Dispose method reverts blog back to its original state.
    /// </summary>
    public class BlogImportSetup : IDisposable
    {
        Action _revertAction;
        public BlogImportSetup(Blog blog, ObjectProvider repository)
        {
            Blog = blog;
            Repository = repository;
            SetupBlogForImport();
        }

        public Blog Blog
        {
            get; private set;
        }

        public ObjectProvider Repository
        {
            get; private set;
        }

        void SetupBlogForImport()
        {
            if(!Blog.DuplicateCommentsEnabled)
            {
                Blog.DuplicateCommentsEnabled = true;
                Repository.UpdateBlog(Blog);
                _revertAction = () =>
                    {
                        Blog.DuplicateCommentsEnabled = false;
                        Repository.UpdateBlog(Blog);
                    };
            }
        }

        public void Dispose()
        {
            Action revertAction = _revertAction;
            if(revertAction != null)
            {
                revertAction();
            }
        }
    }
}
