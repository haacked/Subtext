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
using System.IO;
using BlogML;
using BlogML.Xml;
using Subtext.BlogML.Conversion;
using Subtext.BlogML.Interfaces;
using Subtext.Extensibility.Collections;
using Subtext.Extensibility.Interfaces;

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
		    
            WriteStartBlog(bmlBlog.Title, ContentTypes.Text, bmlBlog.SubTitle, ContentTypes.Text, bmlBlog.RootUrl, bmlBlog.DateCreated);
            WriteAuthors();
            WriteExtendedProperties();

			ICollection<BlogMLCategory> categories = provider.GetAllCategories(this.blogId);
			WriteCategories(categories);

			ICollectionBook<BlogMLPost> allPosts = new CollectionBook<BlogMLPost>(
					delegate(int pageIndex, int pageSize)
					{
						return provider.GetBlogPosts(this.blogId, pageIndex, pageSize);
					}, provider.PageSize
				);
			WritePosts(allPosts);

			WriteEndElement(); // End Blog Element
			Writer.Flush();
		}

		private void WritePosts(ICollectionBook<BlogMLPost> allPosts) {
			WriteStartPosts();

            foreach (IPagedCollection<BlogMLPost> pageOfPosts in allPosts) {
                WritePostsPage(pageOfPosts);
            }

			WriteEndElement(); // </posts>
		}

	    private void WriteAuthors()
	    {
            WriteStartAuthors();
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
            WriteEndElement(); // </authors>
	    }
	    
	    private void WriteExtendedProperties()
	    {
	        if (bmlBlog.ExtendedProperties.Count > 0)
	        {
	            WriteStartExtendedProperties();
	            foreach (Pair<string, string> extProp in bmlBlog.ExtendedProperties)
	            {
	                WriteExtendedProperty(extProp.Key, extProp.Value);
	            }
	            WriteEndElement(); // </extended-properties>
	        }
	    }

		protected void WritePostsPage(IPagedCollection<BlogMLPost> posts)
		{
			foreach (BlogMLPost bmlPost in posts) {
				WritePost(bmlPost);
			}
			Writer.Flush(); //Flushes this page of posts.
		}
		
		private void WritePost(BlogMLPost bmlPost)
		{
			string postId = this.conversionStrategy.GetConvertedId(IdScopes.Posts, bmlPost.ID);
		    WriteStartPost(postId, 
                bmlPost.Title, 
                bmlPost.DateCreated, 
                bmlPost.DateModified, 
                bmlPost.Approved, 
                bmlPost.Content.Text,
                bmlPost.PostUrl, 
                bmlPost.Views, 
                bmlPost.PostType, 
                bmlPost.PostName);

			WritePostCategories(bmlPost.Categories);
			WritePostComments(bmlPost.Comments);
            WritePostTrackbacks(bmlPost.Trackbacks);
            WritePostAttachments(bmlPost);
		    WritePostAuthors(bmlPost.Authors);

			WriteEndElement();	// </post>
		}
	    
	    private void WritePostAuthors(BlogMLPost.AuthorReferenceCollection authorsRefs)
	    {
	        if (authorsRefs.Count > 0)
	        {
	            WriteStartAuthors();
	            foreach (BlogMLAuthorReference authorRef in authorsRefs)
	            {
	                WritePostAuthor(authorRef.Ref);
	            }
	            WriteEndElement();
	        }
	    }
	    
	    private void WritePostAuthor(string authorId)
	    {
            string authorRef = this.conversionStrategy.GetConvertedId(IdScopes.Authors, authorId);
	        WriteAuthorReference(authorRef);
	    }
		
		protected void WritePostCategories(BlogMLPost.CategoryReferenceCollection categoryRefs)
		{
		    if (categoryRefs.Count > 0)
		    {
		        WriteStartCategories();
		        foreach (BlogMLCategoryReference categoryRef in categoryRefs)
		        {
		            WritePostCategory(categoryRef.Ref);
		        }
		        WriteEndElement();
		    }
		}
		
		private void WritePostCategory(string categoryId)
		{
			string categoryRef = this.conversionStrategy.GetConvertedId(IdScopes.Categories, categoryId);
			WriteCategoryReference(categoryRef);
		}

		private void WritePostComments(BlogMLPost.CommentCollection comments)
		{
		    if (comments.Count > 0)
		    {
		        WriteStartComments();
		        foreach (BlogMLComment bmlComment in comments)
		        {
		            WritePostComment(bmlComment);
		        }
		        WriteEndElement();
		    }
		}

		private void WritePostComment(BlogMLComment bmlComment)
		{
			string commentId = this.conversionStrategy.GetConvertedId(IdScopes.Comments, bmlComment.ID);
            string userName = string.IsNullOrEmpty(bmlComment.UserName) ? "Anonymous" : bmlComment.UserName;
			WriteComment(commentId, bmlComment.Title, ContentTypes.Text, bmlComment.DateCreated, bmlComment.DateModified, bmlComment.Approved, userName, bmlComment.UserEMail, bmlComment.UserUrl, bmlComment.Content.Text, ContentTypes.Text);
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
		    if (trackbacks.Count > 0)
		    {
		        WriteStartTrackbacks();
		        foreach(BlogMLTrackback bmlTrackback in trackbacks)
		        {
		            string trackBackId = this.conversionStrategy.GetConvertedId(IdScopes.TrackBacks, bmlTrackback.ID);
                    if (!String.IsNullOrEmpty(bmlTrackback.Url))
                    {
                        WriteTrackback(trackBackId, bmlTrackback.Title, ContentTypes.Text, bmlTrackback.DateCreated, bmlTrackback.DateModified, bmlTrackback.Approved, bmlTrackback.Url);
                    }
		        }
		        WriteEndElement();
		    }
		}

		private void WritePostAttachments(BlogMLPost bmlPost)
		{
			if (bmlPost.Attachments.Count > 0)
			{
				WriteStartAttachments();
				foreach(BlogMLAttachment attachment in bmlPost.Attachments)
				{
				    if (attachment.Embedded)
				    {
				        WriteAttachment(attachment.Url, attachment.Data.Length, attachment.MimeType, attachment.Path, attachment.Embedded, attachment.Data);
				    }
				    else
				    {
				        WriteAttachment(attachment.Path, attachment.MimeType, attachment.Url);
				    }
				}
				WriteEndElement(); // End Attachments Element
				Writer.Flush();
			}
		}
	    
        /// <summary>
        /// Returns a MimeType from a URL
        /// </summary>
        /// <param name="fullUrl">The URL to check for a mime type</param>
        /// <returns>A string representation of the mimetype</returns>
	    public static string GetMimeType(string fullUrl)
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
                    retVal = "image/png";
					break;
				case "jpg":
				case "jpeg":
                    retVal = "image/jpeg";
					break;
				case "bmp":
                    retVal = "image/bmp";
					break;
			    case "gif":
			        retVal = "image/gif";
			        break;
				default:
					retVal = "none";
					break;
			}

			return retVal;
		}
	}
}
