using System;
using System.Collections.Generic;
using BlogML.Xml;
using Subtext.BlogML.Conversion;
using Subtext.Extensibility.Interfaces;

namespace Subtext.BlogML.Interfaces
{
	public interface IBlogMLProvider
	{
		/// <summary>
		/// Returns the number of blog post records to pull from the data store 
		/// at a time when exporting the blog as BlogMl.
		/// </summary>
		int PageSize { get; set; }
		
		/// <summary>
		/// Returns a page of fully hydrated blog posts. The blog posts allow the 
		/// user of this method to navigate blog post categories, comments, etc...
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		IPagedCollection<BlogMLPost> GetBlogPosts(string blogId, int pageIndex, int pageSize);

		/// <summary>
		/// Returns the information about the specified blog
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		BlogMLBlog GetBlog(string blogId);

		/// <summary>
		/// Returns every blog category in the blog.
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		ICollection<BlogMLCategory> GetAllCategories(string blogId);

		/// <summary>
		/// Returns the blog id from whichever context the provider 
		/// happens to be running in.
		/// </summary>
		/// <returns></returns>
		IBlogMLContext GetBlogMlContext();

		/// <summary>
		/// Returns a strategy object responsible for handling Id conversions 
		/// (for example if they need to be converted to guids).  
		/// If Ids do not need to be converted, this should return 
		/// IdConversionStrategy.NullConversionStrategy
		/// </summary>
		IdConversionStrategy IdConversion { get;}

		/// <summary>
		/// Method called before an import begins. Allows the provider to 
		/// initialize any state in the current blog.
		/// </summary>
		void PreImport();

		/// <summary>
		/// Method called when an import is complete.
		void ImportComplete();

		/// <summary>
		/// Creates categories in the new blog based on the blog ml.
		/// Returns a dictionary to map the old category ids to the 
		/// new ones.
		/// </summary>
		/// <param name="blog"></param>
		IDictionary<string, string> CreateCategories(BlogMLBlog blog);

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

		/// <summary>
		/// Creates a blog post and returns the id. Should handle 
		/// dealing with categories.
		/// </summary>
        /// <param name="blog">The imported blog</param>
		/// <param name="post">The imported post</param>
		/// <param name="content">The rewritten content of the post.</param>
		/// <param name="categoryIdMap">A dictionary used to map the blogml category id to the internal category id.</param>
		/// <returns></returns>
		string CreateBlogPost(BlogMLBlog blog, BlogMLPost post, string content, IDictionary<string, string> categoryIdMap);

		/// <summary>
		/// Creates a comment for the specified post.
		/// </summary>
		/// <param name="bmlComment"></param>
		/// <param name="newPostId"></param>
		void CreatePostComment(BlogMLComment bmlComment, string newPostId);

		/// <summary>
		/// Creates a trackback for the post.
		/// </summary>
		/// <param name="trackback"></param>
		void CreatePostTrackback(BlogMLTrackback trackback, string newPostId);

	    /// <summary>
	    /// Sets the extended properties for the blog.
	    /// </summary>
	    /// <param name="extendedProperties"></param>
        void SetBlogMlExtendedProperties(BlogMLBlog.ExtendedPropertiesCollection extendedProperties);
		
		/// <summary>
		/// Lets the provider decide how to log errors.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		void LogError(string message, Exception e);
	}
}