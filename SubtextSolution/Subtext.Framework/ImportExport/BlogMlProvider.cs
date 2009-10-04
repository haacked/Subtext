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
using System.Configuration.Provider;
using BlogML.Xml;

namespace Subtext.BlogML
{
    /// <summary>
    /// Provider for accessing data to implement BlogMl.
    /// </summary>
    public abstract class BlogMLProvider : ProviderBase
    {
        /// <summary>
        /// Returns the number of blog post records to pull from the data store 
        /// at a time when exporting the blog as BlogMl.
        /// </summary>
        public int PageSize { get; set; }

        /// <summary>
        /// Returns the blog id from whichever context the provider 
        /// happens to be running in.
        /// </summary>
        /// <returns></returns>
        public abstract BlogMLContext GetBlogMLContext();

        /// <summary>
        /// Method called before an import begins. Allows the provider to 
        /// initialize any state in the current blog.
        /// </summary>
        public abstract void PreImport();

        /// <summary>
        /// Method called when an import is complete.
        public abstract void ImportComplete();

        /// <summary>
        /// Creates categories from the blog ml.
        /// </summary>
        /// <param name="blog"></param>
        public abstract IDictionary<string, string> CreateCategories(BlogMLBlog blog);

        /// <summary>
        /// The physical path to the attachment directory.
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        public abstract string GetAttachmentDirectoryPath(BlogMLAttachment attachment);

        /// <summary>
        /// The url to the attachment directory
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        public abstract string GetAttachmentDirectoryUrl(BlogMLAttachment attachment);

        /// <summary>
        /// Creates a blog post and returns the id.
        /// </summary>
        /// <param name="blog"></param>
        /// <param name="post"></param>
        /// <param name="categoryIdMap">A dictionary used to map the blogml category id to the internal category id.</param>
        /// <returns></returns>
        public abstract string CreateBlogPost(BlogMLBlog blog, BlogMLPost post,
                                              IDictionary<string, string> categoryIdMap);

        /// <summary>
        /// Creates a comment in the system.
        /// </summary>
        public abstract void CreatePostComment(BlogMLComment comment, string newPostId);

        /// <summary>
        /// Creates a trackback for the post.
        /// </summary>
        public abstract void CreatePostTrackback(BlogMLTrackback trackback, string newPostId);

        /// <summary>
        /// Sets the extended properties for the blog.
        /// </summary>
        public abstract void SetBlogMLExtendedProperties(BlogMLBlog.ExtendedPropertiesCollection extendedProperties);

        /// <summary>
        /// Lets the provider decide how to log errors.
        /// </summary>
        public abstract void LogError(string message, Exception exception);
    }
}