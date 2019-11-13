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

using CookComputing.XmlRpc;

namespace Subtext.Framework.XmlRpc
{
    public struct WordpressCategory
    {
        public string name;
    }

    public interface IWordPressApi
    {
        [XmlRpcMethod("wp.newCategory",
            Description = "Adds a new category to the blog engine.")]
        int newCategory(
            string blogid,
            string username,
            string password,
            WordpressCategory category);

        [XmlRpcMethod("wp.newPage", Description = "Adds a new page/article to the blog engine.")]
        int newPage(
            string blog_id,
            string username,
            string password,
            Post content,
            bool publish);

        [XmlRpcMethod("wp.editPage", Description = "Adds a new page/article to the blog engine.")]
        int editPage(
            string blog_id,
            string page_id,
            string username,
            string password,
            Post content,
            bool publish);

        [XmlRpcMethod("wp.getPages", Description = "Get an array of all the pages on a blog.")]
        Post[] getPages(
            string blog_id,
            string username,
            string password,
            int numberOfPosts
            );

        [XmlRpcMethod("wp.getPage", Description = "Get the page identified by the page id.")]
        Post getPage(
            string blog_id,
            string page_id,
            string username,
            string password
            );

        [XmlRpcMethod("wp.deletePage", Description = "Removes a page from the blog.")]
        bool deletePage(
            string blog_id,
            string username,
            string password,
            string page_id);
    }
}