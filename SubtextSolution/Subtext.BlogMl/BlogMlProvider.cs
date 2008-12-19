#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Configuration.Provider;
using BlogML.Xml;
using Subtext.BlogML.Conversion;
using Subtext.BlogML.Interfaces;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;

namespace Subtext.BlogML
{
	/// <summary>
	/// Provider for accessing data to implement BlogMl.
	/// </summary>
	public abstract class BlogMLProvider : ProviderBase, IBlogMLProvider
	{
		private static BlogMLProvider provider;
		private static GenericProviderCollection<BlogMLProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<BlogMLProvider>("BlogMLProvider", out provider);

		/// <summary>
		/// Returns the default instance of this provider.
		/// </summary>
		/// <returns></returns>
		public static IBlogMLProvider Instance()
		{
			return provider;
		}

        public BlogMLProvider() {
            PageSize = 1000;
        }

		/// <summary>
		/// Returns all the configured Email Providers.
		/// </summary>
		public static GenericProviderCollection<BlogMLProvider> Providers
		{
			get
			{
				return providers;
			}
		}

		/// <summary>
		/// Initializes the provider with values from its configuration section in web.config.
		/// </summary>
		/// <remarks>
		/// <para>
		/// The connection string can be initialized via the "ConnectionStringName" property 
		/// or via the "ConnectionString" attribute.
		/// </para>
		/// <para>
		///	If the "ConnectionStringName" is specified, the connection string is looked up 
		/// in the ConnectionStrings section of web.config.  If "ConnectionString" is specified, 
		/// it overrides "ConnectionStringName" and is used instead.
		/// </para>
		/// </remarks>
		/// <param name="name">The name.</param>
		/// <param name="configValue">The config value.</param>
		public override void Initialize(string name, System.Collections.Specialized.NameValueCollection configValue)
		{
            //initial value.
            PageSize = 100;

			if (!String.IsNullOrEmpty(configValue["connectionStringName"]))
			{
				ConnectionStringSettings connection = ConfigurationManager.ConnectionStrings[configValue["connectionStringName"]];
				if (connection == null)
					throw new ProviderException(string.Format("No connection string matches the key '{0}'.", configValue["connectionStringName"]));
				ConnectionString = connection.ConnectionString;
			}

            if (!String.IsNullOrEmpty(configValue["connectionString"])) {
                ConnectionString = configValue["connectionString"];
            }
			
			if(!String.IsNullOrEmpty(configValue["pageSize"]))
			{
				int postPageSize;
				if (int.TryParse(configValue["pageSize"], out postPageSize))
					PageSize = postPageSize;
			}

			base.Initialize(name, configValue);
		}

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
		public virtual string ConnectionString
		{
			get;
            set;
		}

		/// <summary>
		/// Returns the number of blog post records to pull from the data store 
		/// at a time when exporting the blog as BlogMl.
		/// </summary>
		public int PageSize
		{
			get;
			set;
		}

		/// <summary>
		/// Returns a page of fully hydrated blog posts. The blog posts allow the 
		/// user of this method to navigate blog post categories, comments, etc...
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public abstract IPagedCollection<BlogMLPost> GetBlogPosts(string blogId, int pageIndex, int pageSize);

		/// <summary>
		/// Returns the information about the specified blog
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		public abstract BlogMLBlog GetBlog(string blogId);

		/// <summary>
		/// Returns every blog category in the blog.
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		public abstract ICollection<BlogMLCategory> GetAllCategories(string blogId);

		/// <summary>
		/// Returns the blog id from whichever context the provider 
		/// happens to be running in.
		/// </summary>
		/// <returns></returns>
		public abstract IBlogMLContext GetBlogMlContext();

		/// <summary>
		/// Returns a strategy object responsible for handling Id conversions 
		/// (for example if they need to be converted to guids).  
		/// If Ids do not need to be converted, this should just return 
		/// IdConversionStrategy.Empty
		/// </summary>
		public abstract IdConversionStrategy IdConversion { get;}

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
		/// <param name="content">The rewritten content of the post.</param>
		/// <param name="categoryIdMap">A dictionary used to map the blogml category id to the internal category id.</param>
		/// <returns></returns>
		public abstract string CreateBlogPost(BlogMLBlog blog, BlogMLPost post, string content, IDictionary<string, string> categoryIdMap);

		/// <summary>
		/// Creates a comment in the system.
		/// </summary>
		/// <param name="bmlComment"></param>
		public abstract void CreatePostComment(BlogMLComment bmlComment, string newPostId);

		/// <summary>
		/// Creates a trackback for the post.
		/// </summary>
		/// <param name="trackback"></param>
		public abstract void CreatePostTrackback(BlogMLTrackback trackback, string newPostId);

        /// <summary>
        /// Sets the extended properties for the blog.
        /// </summary>
        /// <param name="extendedProperties"></param>
        public abstract void SetBlogMlExtendedProperties(BlogMLBlog.ExtendedPropertiesCollection extendedProperties);

	    /// <summary>
		/// Lets the provider decide how to log errors.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public abstract void LogError(string message, Exception e);
	}
}