using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using BlogML;
using BlogML.Xml;
using Subtext.BlogML.Conversion;
using Subtext.Extensibility.Collections;
using Subtext.Extensibility.Interfaces;
using Subtext.BlogML.Interfaces;

namespace Subtext.BlogML
{
	public class BlogMLWriter : BlogMLWriterBase
	{
		IBlogMLProvider provider;
		IdConversionStrategy conversionStrategy;
		string blogId;
		BlogMLBlog blog;

		/// <summary>
		/// Creates an instance of the BlogMl Writer.
		/// </summary>
		/// <param name="provider">The provider.</param>
		public static BlogMLWriter Create(IBlogMLProvider provider)
		{
			if (provider == null)
				throw new ArgumentNullException("provider", "provider cannot be null");

			IBlogMLContext context = provider.GetBlogMlContext();
			if (context == null)
				throw new InvalidOperationException("The BlogMl provider did not set the context.");

			return new BlogMLWriter(provider, context);
		}
		
		/// <summary>
		/// Constructs an instance of the BlogMlWriter for the specified blogId.
		/// </summary>
		private BlogMLWriter(IBlogMLProvider provider, IBlogMLContext context)
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

			ICollection<BlogMLCategory> categories = provider.GetAllCategories(this.blogId);
			WriteCategories(categories);

			ICollectionBook<BlogMLPost> allPosts = new CollectionBook<BlogMLPost>
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

		private void WritePosts(ICollectionBook<BlogMLPost> allPosts)
		{
			WriteStartPosts();
			
			foreach(IPagedCollection<BlogMLPost> pageOfPosts in allPosts)
				WritePostsPage(pageOfPosts);

			WriteEndElement(); // </posts>
		}

		/// <summary>
		/// Writes information about the blog itself.
		/// </summary>
		private void WriteBlogStart()
		{
			WriteStartBlog(blog.Title, ContentTypes.Text, blog.SubTitle, ContentTypes.Text, blog.RootUrl, blog.DateCreated);
            WriteAuthor(blog.Authors[0].ID, blog.Authors[0].Title, blog.Authors[0].Email, blog.DateCreated, DateTime.MinValue, true);
		}


		protected void WritePostsPage(IPagedCollection<BlogMLPost> posts)
		{
			foreach (BlogMLPost post in posts)
			{
				WritePost(post);
			}
			Writer.Flush(); //Flushes this page of posts.
		}
		
		private void WritePost(BlogMLPost post)
		{
			string postId = this.conversionStrategy.GetConvertedId(IdScopes.Posts, post.ID);
		    WriteStartPost(postId, post.Title, post.DateCreated, post.DateModified, post.Approved, post.Content.Text,
		                   post.PostUrl, post.Views, post.PostType, post.PostName);

			WritePostAttachments(post);
			WritePostComments(post.Comments);
			WritePostCategories(post.Categories);
			WritePostTrackbacks(post.Trackbacks);

			WriteEndElement();	// </post>
		}
		
		protected void WritePostCategories(BlogMLPost.CategoryReferenceCollection categoryRefs)
		{
			WriteStartCategories();
			if (categoryRefs != null)
			{
				foreach (BlogMLCategoryReference categoryRef in categoryRefs)
				{
					WritePostCategory(categoryRef.Ref);
				}
			}
			WriteEndElement();
		}
		
		private void WritePostCategory(string categoryId)
		{
			string categoryRef = this.conversionStrategy.GetConvertedId(IdScopes.Categories, categoryId);
			WriteCategoryReference(categoryRef);
		}

		private void WritePostComments(BlogMLPost.CommentCollection comments)
		{
			WriteStartComments();
			foreach (BlogMLComment comment in comments)
			{
				WritePostComment(comment);
			}
			WriteEndElement();
		}

		private void WritePostComment(BlogMLComment comment)
		{
			string commentId = this.conversionStrategy.GetConvertedId(IdScopes.Comments, comment.ID);
			WriteComment(commentId, comment.Title, ContentTypes.Text, comment.DateCreated, comment.DateModified, comment.Approved, comment.UserName, comment.UserEMail, comment.UserUrl, comment.Content.Text, ContentTypes.Text);
		}
		
		private void WriteCategories(ICollection<BlogMLCategory> categories)
		{
			WriteStartCategories();
			foreach(BlogMLCategory category in categories)
			{
				string categoryId = this.conversionStrategy.GetConvertedId(IdScopes.Categories, category.ID);
				string parentId = this.conversionStrategy.GetConvertedId(IdScopes.CategoryParents, category.ParentRef);
				WriteCategory(categoryId, category.Title, ContentTypes.Text, category.DateCreated, category.DateModified, category.Approved, category.Description, parentId);
			}
			WriteEndElement();
		}
		
		private void WritePostTrackbacks(BlogMLPost.TrackbackCollection trackbacks)
		{
			WriteStartTrackbacks();
			
			foreach(BlogMLTrackback trackback in trackbacks)
			{
				string trackBackId = this.conversionStrategy.GetConvertedId(IdScopes.TrackBacks, trackback.ID);
				WriteTrackback(trackBackId, trackback.Title, ContentTypes.Text, trackback.DateCreated, trackback.DateModified, trackback.Approved, trackback.Url);
			}
			
			WriteEndElement();
		}

		private void WritePostAttachments(BlogMLPost post)
		{
			string content = post.Content.Text;
		    
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
					    WriteAttachment(imageURL, 0, GetMimeType(imageURL), imageURL, provider.GetBlogMlContext().EmbedAttachments, null);
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

			switch (extension.ToLower(CultureInfo.InvariantCulture))
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
