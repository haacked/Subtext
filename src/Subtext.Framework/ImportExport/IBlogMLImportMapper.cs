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

using BlogML.Xml;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.ImportExport
{
    public interface IBlogMLImportMapper
    {
        Entry ConvertBlogPost(BlogMLPost post, BlogMLBlog blogMLBlog, Blog blog);
        LinkCategory ConvertCategory(BlogMLCategory category);
        FeedbackItem ConvertComment(BlogMLComment comment, string parentPostId);
        FeedbackItem ConvertTrackback(BlogMLTrackback trackback, string parentPostId);
    }
}
