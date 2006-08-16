using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Globalization;
using System.IO;
using Subtext.BlogMl.Conversion;
using Subtext.Extensibility.Interfaces;
using Subtext.BlogMl.Interfaces;
using Subtext.Framework.Components;

namespace Subtext.BlogMl
{
	public class BlogMlWriter : BlogML.BlogMLWriterBase
	{
		IBlogMlProvider provider;
		IdConversionStrategy conversionStrategy;
		string blogId;
		IBlogMlBlog blog;

		/// <summary>
		/// Creates an instance of the BlogMl Writer.
		/// </summary>
		/// <param name="provider">The provider.</param>
		public static BlogMlWriter Create(IBlogMlProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider", "provider cannot be null");

			IBlogMlContext context = provider.GetBlogMlContext();
			if (context == null)
				throw new InvalidOperationException("The BlogMl provider did not set the context.");

			return new BlogMlWriter(provider, context);
		}
		
		/// <summary>
		/// Constructs an instance of the BlogMlWriter for the specified blogId.
		/// </summary>
		private BlogMlWriter(IBlogMlProvider provider, IBlogMlContext context)
		{			
			this.provider = provider;
			this.blogId = context.BlogId;
			this.conversionStrategy = provider.IdConversion;
			
			if (this.conversionStrategy == null)
				this.conversionStrategy = IdConversionStrategy.Empty;
		}		

		/// <summary>
		/// Writes the blog.
		/// </summary>
		protected override void InternalWriteBlog()
		{
			blog = this.provider.GetBlog(this.blogId);
			WriteBlogStart();

			ICollection<IBlogMlCategory> categories = provider.GetAllCategories(this.blogId);
			WriteCategories(categories);

			CollectionBook<IBlogMlPost> allPosts = new CollectionBook<IBlogMlPost>
				(
					delegate(int pageIndex, int pageSize)
					{
						return provider.GetBlogPosts(this.blogId, pageIndex, pageSize);
					}, provider.PageSize
				);
			WritePosts(allPosts);

			WriteEndElement(); // End Blog Element
			Writer.Flush();
		}

		private void WritePosts(CollectionBook<IBlogMlPost> allPosts)
		{
			WriteStartPosts();
			
			foreach(IPagedCollection<IBlogMlPost> pageOfPosts in allPosts)
				WritePostsPage(pageOfPosts);

			WriteEndElement(); // </posts>
		}

		/// <summary>
		/// Writes information about the blog itself.
		/// </summary>
		private void WriteBlogStart()
		{
			WriteStartBlog(blog.Title, blog.TitleContentType, blog.Subtitle, blog.SubTitleContentType, blog.RootUrl, blog.DateCreated);
			WriteAuthor(blog.Author, blog.Email);
		}

		protected void WritePostsPage(IPagedCollection<IBlogMlPost> posts)
		{
			foreach (IBlogMlPost post in posts)
			{
				WritePost(post);
			}
			Writer.Flush(); //Flushes this page of posts.
		}
		
		private void WritePost(IBlogMlPost post)
		{
			string postId = this.conversionStrategy.GetConvertedId(IdScopes.Posts, post.Id);
			WriteStartPost(postId, post.Title, post.DateCreated, post.DateModified, post.Approved, post.Content, post.Url);

			WritePostAttachments(post);
			WritePostComments(post.Comments);
			WritePostCategories(post.CategoryIds);
			WritePostTrackbacks(post.Trackbacks);

			WriteEndElement();	// </post>
		}
		
		protected void WritePostCategories(StringCollection categoryIds)
		{
			WriteStartCategories();
			foreach (string categoryId in categoryIds)
			{
				WritePostCategory(categoryId);
			}
			WriteEndElement();
		}
		
		private void WritePostCategory(string categoryId)
		{
			string categoryRef = this.conversionStrategy.GetConvertedId(IdScopes.Categories, categoryId);
			WriteCategoryReference(categoryRef);
		}

		private void WritePostComments(ICollection<IBlogMlComment> comments)
		{
			WriteStartComments();
			foreach (IBlogMlComment comment in comments)
			{
				WritePostComment(comment);
			}
			WriteEndElement();
		}

		private void WritePostComment(IBlogMlComment comment)
		{
			string commentId = this.conversionStrategy.GetConvertedId(IdScopes.Comments, comment.Id);
			WriteComment(commentId, comment.Title, comment.TitleContentType, comment.DateCreated, comment.DateModified, comment.Approved, comment.UserName, comment.Email, comment.Url, comment.Content, comment.CommentContentType);
		}
		
		private void WriteCategories(ICollection<IBlogMlCategory> categories)
		{
			WriteStartCategories();
			foreach(IBlogMlCategory category in categories)
			{
				string categoryId = this.conversionStrategy.GetConvertedId(IdScopes.Categories, category.Id);
				string parentId = this.conversionStrategy.GetConvertedId(IdScopes.CategoryParents, category.ParentId);
				WriteCategory(categoryId, category.Title, category.TitleContentType, category.DateCreated, category.DateModified, category.Approved, category.Description, parentId);
			}
			WriteEndElement();
		}
		
		private void WritePostTrackbacks(ICollection<IBlogMlTrackback> trackbacks)
		{
			WriteStartTrackbacks();
			
			foreach(IBlogMlTrackback trackback in trackbacks)
			{
				string trackBackId = this.conversionStrategy.GetConvertedId(IdScopes.TrackBacks, trackback.Id);
				WriteTrackback(trackBackId, trackback.Title, trackback.TitleContentType, trackback.DateCreated, trackback.DateModified, trackback.Approved, trackback.Url);
			}
			
			WriteEndElement();
		}

		private void WritePostAttachments(IBlogMlPost post)
		{
			string content = post.Content;
			
			string[] imagesURLs = SgmlUtil.GetAttributeValues(content, "img", "src");
			string appFullRootUrl = this.blog.RootUrl.ToLower(CultureInfo.InvariantCulture);

			if (imagesURLs.Length > 0)
			{
				WriteStartAttachments();
				foreach(string imageURL in imagesURLs)
				{
					string loweredImageURL = imageURL.ToLower(CultureInfo.InvariantCulture);
					// now we need to determine if the URL is local
					if (SgmlUtil.IsRootUrlOf(appFullRootUrl, loweredImageURL))
					{
						WriteAttachment(imageURL, GetMimeType(imageURL), imageURL);
						Writer.Flush();						
					}
				}
				WriteEndElement(); // End Attachments Element
				Writer.Flush();
			}
		}

		private static string GetMimeType(string fullUrl)
		{
			string extension = Path.GetExtension(fullUrl);
			string retVal;

			if (extension == null || extension.Length == 0)
			{
				return string.Empty;
			}

			extension = extension.TrimStart(new char[] { '.' });

			switch (extension.ToLower())
			{
				case "png":
					retVal = "png";
					break;
				case "jpg":
				case "jpeg":
					retVal = "jpg";
					break;
				case "bmp":
					retVal = "bmp";
					break;
				default:
					retVal = "none";
					break;
			}

			return retVal;
		}
	}
}
