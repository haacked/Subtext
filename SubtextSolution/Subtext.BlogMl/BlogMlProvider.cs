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
using Subtext.BlogMl.Conversion;
using Subtext.BlogMl.Interfaces;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;

namespace Subtext.BlogMl
{
	/// <summary>
	/// Provider for accessing data to implement BlogMl.
	/// </summary>
	public abstract class BlogMlProvider : ProviderBase, IBlogMlProvider
	{
		private static BlogMlProvider provider;
		private static GenericProviderCollection<BlogMlProvider> providers = ProviderConfigurationHelper.LoadProviderCollection<BlogMlProvider>("BlogMlProvider", out provider);

		/// <summary>
		/// Returns the default instance of this provider.
		/// </summary>
		/// <returns></returns>
		public static IBlogMlProvider Instance()
		{
			return provider;
		}

		/// <summary>
		/// Returns all the configured Email Providers.
		/// </summary>
		public static GenericProviderCollection<BlogMlProvider> Providers
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
			if (!String.IsNullOrEmpty(configValue["connectionStringName"]))
			{
				ConnectionStringSettings connection = ConfigurationManager.ConnectionStrings[configValue["connectionStringName"]];
				if (connection == null)
					throw new ProviderException(string.Format("No connection string matches the key '{0}'.", configValue["connectionStringName"]));
				this.connectionString = connection.ConnectionString;
			}

			if (!String.IsNullOrEmpty(configValue["connectionString"]))
				this.connectionString = configValue["connectionString"];
			
			if(!String.IsNullOrEmpty(configValue["pageSize"]))
			{
				int postPageSize;
				if (int.TryParse(configValue["pageSize"], out postPageSize))
					this.pageSize = postPageSize;
			}

			base.Initialize(name, configValue);
		}

		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value>The connection string.</value>
		public string ConnectionString
		{
			get { return this.connectionString; }
			set { this.connectionString = value; }
		}

		string connectionString;
		
		/// <summary>
		/// Returns the number of blog post records to pull from the data store 
		/// at a time when exporting the blog as BlogMl.
		/// </summary>
		public int PageSize
		{
			get { return this.pageSize; }
			set { this.pageSize = value; }
		}

		int pageSize = 100;

		/// <summary>
		/// Returns a page of fully hydrated blog posts. The blog posts allow the 
		/// user of this method to navigate blog post categories, comments, etc...
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public abstract IPagedCollection<IBlogMlPost> GetBlogPosts(string blogId, int pageIndex, int pageSize);

		/// <summary>
		/// Returns the information about the specified blog
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		public abstract IBlogMlBlog GetBlog(string blogId);

		/// <summary>
		/// Returns every blog category in the blog.
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		public abstract ICollection<IBlogMlCategory> GetAllCategories(string blogId);

		/// <summary>
		/// Returns the blog id from whichever context the provider 
		/// happens to be running in.
		/// </summary>
		/// <returns></returns>
		public abstract IBlogMlContext GetBlogMlContext();

		/// <summary>
		/// Returns a strategy object responsible for handling Id conversions 
		/// (for example if they need to be converted to guids).  
		/// If Ids do not need to be converted, this should just return 
		/// IdConversionStrategy.Empty
		/// </summary>
		public abstract IdConversionStrategy IdConversion { get;}
	}
}
