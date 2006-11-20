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
		BlogMLBlog bmlBlog;

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
			bmlBlog = this.provider.GetBlog(this.blogId);
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
			WriteStartBlog(bmlBlog.Title, ContentTypes.Text, bmlBlog.SubTitle, ContentTypes.Text, bmlBlog.RootUrl, bmlBlog.DateCreated);

		    foreach (BlogMLAuthor bmlAuthor in bmlBlog.Authors)
		    {
                WriteAuthor(
                    conversionStrategy.GetConvertedId(IdScopes.Authors, bmlAuthor.ID), 
                    bmlAuthor.Title, 
                    bmlAuthor.Email, 
                    bmlAuthor.DateCreated, 
                    bmlAuthor.DateModified, 
                    bmlAuthor.Approved);
		    }
		}


		protected void WritePostsPage(IPagedCollection<BlogMLPost> posts)
		{
			foreach (BlogMLPost bmlPost in posts)
			{
				WritePost(bmlPost);
			}
			Writer.Flush(); //Flushes this page of posts.
		}
		
		private void WritePost(BlogMLPost bmlPost)
		{
			string postId = this.conversionStrategy.GetConvertedId(IdScopes.Posts, bmlPost.ID);
		    WriteStartPost(postId, bmlPost.Title, bmlPost.DateCreated, bmlPost.DateModified, bmlPost.Approved, bmlPost.Content.Text,
		                   bmlPost.PostUrl, bmlPost.Views, bmlPost.PostType, bmlPost.PostName);

			WritePostAttachments(bmlPost);
			WritePostComments(bmlPost.Comments);
			WritePostCategories(bmlPost.Categories);
			WritePostTrackbacks(bmlPost.Trackbacks);

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
			foreach (BlogMLComment bmlComment in comments)
			{
				WritePostComment(bmlComment);
			}
			WriteEndElement();
		}

		private void WritePostComment(BlogMLComment bmlComment)
		{
			string commentId = this.conversionStrategy.GetConvertedId(IdScopes.Comments, bmlComment.ID);
			WriteComment(commentId, bmlComment.Title, ContentTypes.Text, bmlComment.DateCreated, bmlComment.DateModified, bmlComment.Approved, bmlComment.UserName, bmlComment.UserEMail, bmlComment.UserUrl, bmlComment.Content.Text, ContentTypes.Text);
		}
		
		private void WriteCategories(ICollection<BlogMLCategory> bmlCategories)
		{
			WriteStartCategories();
			foreach(BlogMLCategory bmlCategory in bmlCategories)
			{
				string categoryId = this.conversionStrategy.GetConvertedId(IdScopes.Categories, bmlCategory.ID);
				string parentId = this.conversionStrategy.GetConvertedId(IdScopes.CategoryParents, bmlCategory.ParentRef);
				WriteCategory(categoryId, bmlCategory.Title, ContentTypes.Text, bmlCategory.DateCreated, bmlCategory.DateModified, bmlCategory.Approved, bmlCategory.Description, parentId);
			}
			WriteEndElement();
		}
		
		private void WritePostTrackbacks(BlogMLPost.TrackbackCollection trackbacks)
		{
			WriteStartTrackbacks();
			
			foreach(BlogMLTrackback bmlTrackback in trackbacks)
			{
				string trackBackId = this.conversionStrategy.GetConvertedId(IdScopes.TrackBacks, bmlTrackback.ID);
				WriteTrackback(trackBackId, bmlTrackback.Title, ContentTypes.Text, bmlTrackback.DateCreated, bmlTrackback.DateModified, bmlTrackback.Approved, bmlTrackback.Url);
			}
			
			WriteEndElement();
		}

		private void WritePostAttachments(BlogMLPost bmlPost)
		{
			string content = bmlPost.Content.Text;
		    
			string[] imagesURLs = SgmlUtil.GetAttributeValues(content, "img", "src");
			string appFullRootUrl = this.bmlBlog.RootUrl.ToLower(CultureInfo.InvariantCulture);

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
