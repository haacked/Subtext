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

using System;
using System.Collections.Generic;
using BlogML.Xml;

namespace Subtext.ImportExport
{
    public interface IBlogMlImportService
    {
        /// <summary>
        /// Creates categories from the blog ml.
        /// </summary>
        /// <param name="blog"></param>
        IDictionary<string, string> CreateCategories(BlogMLBlog blog);

        /// <summary>
        /// Creates a blog post and returns the id.
        /// </summary>
        /// <param name="blog"></param>
        /// <param name="post"></param>
        /// <param name="categoryIdMap">A dictionary used to map the blogml category id to the internal category id.</param>
        /// <returns></returns>
        string CreateBlogPost(BlogMLBlog blog, BlogMLPost post,
                                              IDictionary<string, string> categoryIdMap);

        /// <summary>
        /// Creates a comment in the system.
        /// </summary>
        void CreatePostComment(BlogMLComment comment, string newPostId);

        /// <summary>
        /// Creates a trackback for the post.
        /// </summary>
        void CreatePostTrackback(BlogMLTrackback trackback, string newPostId);

        /// <summary>
        /// Sets the extended properties stored in the BlogML that Subtext supports
        /// </summary>
        /// <param name="extendedProperties"></param>
        void SetBlogMLExtendedProperties(BlogMLBlog.ExtendedPropertiesCollection extendedProperties);

        /// <summary>
        /// The physical path to the attachment directory.
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        string GetAttachmentDirectoryPath(BlogMLAttachment attachment);

        /// <summary>
        /// The url to the attachment directory
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        string GetAttachmentDirectoryUrl(BlogMLAttachment attachment);

        void LogError(string errorMessage, Exception e);
    }
}
