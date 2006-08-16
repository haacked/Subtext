using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.Web;
using BlogML;
using Microsoft.ApplicationBlocks.Data;
using Subtext.BlogMl.Conversion;
using Subtext.BlogMl.Interfaces;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace Subtext.BlogMl.Implementations
{
	public class SubtextBlogMlProvider : BlogMlProvider
	{
		SubtextConversionStrategy conversion = new SubtextConversionStrategy();
		/// <summary>
		/// Returns a page of fully hydrated blog posts. The blog posts allow the 
		/// user of this method to navigate blog post categories, comments, etc...
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public override IPagedCollection<IBlogMlPost> GetBlogPosts(string blogId, int pageIndex, int pageSize)
		{
			IPagedCollection<IBlogMlPost> posts = new PagedCollection<IBlogMlPost>();
			using (IDataReader reader = GetPostsAndArticlesReader(blogId, pageIndex, pageSize))
			{
				while (reader.Read())
				{
					posts.Add(ObjectHydrator.LoadPostFromDataReader(reader));
				}

				if (reader.NextResult() && reader.Read())
					posts.MaxItems = DataHelper.ReadInt32(reader, "TotalRecords");
					
				if (posts.Count > 0 && reader.NextResult())
					PopulateCategories(posts, reader);
				
				if (posts.Count > 0 && reader.NextResult())
					PopulateComments(posts, reader);
				
				if (posts.Count > 0 && reader.NextResult())
					PopulateTrackbacks(posts, reader);
				
			}
			return posts;
		}

		private static void PopulateCategories(IPagedCollection<IBlogMlPost> posts, IDataReader reader)
		{
			PostChildrenPopulator populator = delegate(IBlogMlPost post)
			{
				post.CategoryIds.Add(DataHelper.ReadInt32(reader, "CategoryId").ToString(CultureInfo.InvariantCulture));
			};

			ReadAndPopulatePostChildren(posts, reader, "Id", populator);
		}

		private static void PopulateComments(IPagedCollection<IBlogMlPost> posts, IDataReader reader)
		{
			PostChildrenPopulator populator = delegate(IBlogMlPost post)
			{
				post.Comments.Add(ObjectHydrator.LoadCommentFromDataReader(reader));
			};
			
			ReadAndPopulatePostChildren(posts, reader, "ParentID", populator);
		}

		private static void PopulateTrackbacks(IPagedCollection<IBlogMlPost> posts, IDataReader reader)
		{
			PostChildrenPopulator populator = delegate(IBlogMlPost post)
			{
				post.Trackbacks.Add(ObjectHydrator.LoadTrackbackFromDataReader(reader));
			};

			ReadAndPopulatePostChildren(posts, reader, "ParentID", populator);
		}

		private static void ReadAndPopulatePostChildren(IPagedCollection<IBlogMlPost> posts, IDataReader reader, string foreignKey, PostChildrenPopulator populatePostChildren)
		{
			for (int i = 0; i < posts.Count; i++)
			{
				IBlogMlPost post = posts[i];
				int postId = int.Parse(post.Id);
				// We are going to make use of the fact that everything is ordered by Post Id ASC
				// to optimize this...
				while (reader.Read())
				{
					int postIdForeignKey = DataHelper.ReadInt32(reader, foreignKey);

					if (postId < postIdForeignKey)
					{
						while (postId < postIdForeignKey && i < posts.Count)
						{
							i++;
							post = posts[i];
							postId = int.Parse(post.Id);
						}
					}

					if (postId > postIdForeignKey)
						continue;

					if (postId == postIdForeignKey)
						populatePostChildren(post);
				}
			}
		}
		
		private delegate void PostChildrenPopulator(IBlogMlPost post);

		private IDataReader GetReader(string sql, SqlParameter[] p)
		{
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, sql, p);
		}
		
		private IDataReader GetPostsAndArticlesReader(string blogId, int pageIndex, int pageSize)
		{
			int blogIdValue;
			if (!int.TryParse(blogId, out blogIdValue))
				throw new ArgumentException(string.Format("Invalid blog id '{0}' specified", blogId), "blogId");
			
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, blogIdValue),
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
			};
			return GetReader("subtext_GetEntriesForBlogMl", p);
		}

		/// <summary>
		/// Returns the information about the specified blog
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		public override IBlogMlBlog GetBlog(string blogId)
		{
			int blogIdentifier;
			if (int.TryParse(blogId, out blogIdentifier))
			{
				BlogInfo blog = BlogInfo.GetBlogById(blogIdentifier);
				IBlogMlBlog blogMlBlog = new BlogMlBlog(blog.Title, ContentTypes.Xhtml, blog.SubTitle, ContentTypes.Xhtml, blog.RootUrl.ToString(), blog.Author, blog.Email, DateTime.Now);
				return blogMlBlog;
			}
			return null;
		}

		/// <summary>
		/// Returns every blog category in the blog.
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		public override ICollection<IBlogMlCategory> GetAllCategories(string blogId)
		{
			ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.None, ActiveFilter.None);
			Collection<IBlogMlCategory> blogCategories = new Collection<IBlogMlCategory>();
			
			foreach(LinkCategory category in categories)
			{
				IBlogMlCategory blogCategory = new BlogMlCategory(category.Id.ToString(CultureInfo.InvariantCulture)
					, category.Title
					, ContentTypes.Xhtml
					, category.Description
					, category.IsActive
					, category.CategoryType.ToString()
					, DateTime.Now
					, DateTime.Now);
				blogCategories.Add(blogCategory);
			}
			return blogCategories;
		}

		/// <summary>
		/// Returns the context under which blogml import or export is running under.
		/// </summary>
		/// <returns></returns>
		public override IBlogMlContext GetBlogMlContext()
		{
			bool embedValue = false;
			if(HttpContext.Current != null && HttpContext.Current.Request != null)
				embedValue = String.Equals(HttpContext.Current.Request.QueryString["embed"], "true", StringComparison.InvariantCultureIgnoreCase);

			return new BlogMlContext(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture), embedValue);
		}

		/// <summary>
		/// Returns a strategy object responsible for handling Id conversions 
		/// (for example if they need to be converted to guids).  
		/// If Ids do not need to be converted, this should return 
		/// IdConversionStrategy.NullConversionStrategy
		/// </summary>
		public override IdConversionStrategy IdConversion
		{
			get
			{
				return conversion;
			}
		}
	}
}
