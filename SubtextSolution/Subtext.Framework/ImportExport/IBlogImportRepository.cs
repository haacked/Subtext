using System;
using BlogML.Xml;

namespace Subtext.ImportExport
{
    public interface IBlogImportRepository
    {
        /// <summary>
        /// Creates categories from the blog ml.
        /// </summary>
        /// <param name="blog"></param>
        void CreateCategories(BlogMLBlog blog);

        /// <summary>
        /// Creates a blog post and returns the id.
        /// </summary>
        string CreateBlogPost(BlogMLBlog blog, BlogMLPost post);

        /// <summary>
        /// Creates a comment in the system.
        /// </summary>
        void CreateComment(BlogMLComment comment, string newPostId);

        /// <summary>
        /// Creates a trackback for the post.
        /// </summary>
        void CreateTrackback(BlogMLTrackback trackback, string newPostId);

        /// <summary>
        /// Sets the extended properties stored in the BlogML that Subtext supports
        /// </summary>
        /// <param name="extendedProperties"></param>
        void SetExtendedProperties(BlogMLBlog.ExtendedPropertiesCollection extendedProperties);

        /// <summary>
        /// The physical path to the attachment directory.
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        string GetAttachmentDirectoryPath();

        /// <summary>
        /// The url to the attachment directory
        /// </summary>
        /// <remarks>
        /// The attachment is passed in to give the blog engine 
        /// the opportunity to use attachment specific directories 
        /// (ex. based on mime type) should it choose.
        /// </remarks>
        string GetAttachmentDirectoryUrl();

        /// <summary>
        /// Sets up the blog for import by allowing duplicate comments, turning off moderation, etc...
        /// </summary>
        /// <returns></returns>
        IDisposable SetupBlogForImport();
    }
}
