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
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using BlogML;
using BlogML.Xml;
using Microsoft.ApplicationBlocks.Data;
using Subtext.BlogML;
using Subtext.BlogML.Conversion;
using Subtext.BlogML.Interfaces;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Text;
using Subtext.ImportExport.Conversion;
using Subtext.Framework.Data;

namespace Subtext.ImportExport
{
	public class SubtextBlogMLProvider : BlogMLProvider
	{
        public override void Initialize(string name, System.Collections.Specialized.NameValueCollection configValue)
        {
            base.Initialize(name, configValue);
            if (string.IsNullOrEmpty(ConnectionString))
            {
                ConnectionString = Config.ConnectionString.RawOriginal;
            }
        }

		bool duplicateCommentsEnabled;
		SubtextConversionStrategy conversion = new SubtextConversionStrategy();
		/// <summary>
		/// Returns a page of fully hydrated blog posts. The blog posts allow the 
		/// user of this method to navigate blog post categories, comments, etc...
		/// </summary>
		/// <param name="blogId"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public override IPagedCollection<BlogMLPost> GetBlogPosts(string blogId, int pageIndex, int pageSize)
		{
			IPagedCollection<BlogMLPost> bmlPosts = new PagedCollection<BlogMLPost>();
			using (IDataReader reader = GetPostsAndArticlesReader(blogId, pageIndex, pageSize))
			{
                IBlogMLContext bmlContext = this.GetBlogMlContext();
				while (reader.Read())
				{
					BlogMLPost bmlPost = ObjectHydrator.LoadPostFromDataReader(reader);
                    bmlPost.Attachments.AddRange(GetPostAttachments(bmlPost, bmlContext));
					bmlPosts.Add(bmlPost);
				}

				if (reader.NextResult() && reader.Read())
					bmlPosts.MaxItems = DataHelper.ReadInt32(reader, "TotalRecords");
					
				if (bmlPosts.Count > 0 && reader.NextResult())
					PopulateCategories(bmlPosts, reader);
				
				if (bmlPosts.Count > 0 && reader.NextResult())
					PopulateComments(bmlPosts, reader);
				
				if (bmlPosts.Count > 0 && reader.NextResult())
					PopulateTrackbacks(bmlPosts, reader);
			    
			    if (bmlPosts.Count > 0 && reader.NextResult())
			        PopulateAuthors(bmlPosts, reader);
				
			}
			return bmlPosts;
		}

        private static IList GetPostAttachments(BlogMLPost bmlPost, IBlogMLContext bmlContext)
        {
            IList attachments = new ArrayList();
            string[] attachmentUrls = BlogMLWriterBase.SgmlUtil.GetAttributeValues(bmlPost.Content.Text, "img", "src");

            if (attachmentUrls.Length > 0)
            {
                bool embed = bmlContext.EmbedAttachments;
                
                foreach (string attachmentUrl in attachmentUrls)
                {
                    string blogHostUrl = Config.CurrentBlog.HostFullyQualifiedUrl.ToString().ToLower(CultureInfo.InvariantCulture);
                    
                    // If the URL for the attachment is local then we'll want to build a new BlogMLAttachment 
                    // add add it to the list of attchements for this post.
                    if (BlogMLWriterBase.SgmlUtil.IsRootUrlOf(blogHostUrl, attachmentUrl.ToLower(CultureInfo.InvariantCulture)))
                    {
                        BlogMLAttachment attachment = new BlogMLAttachment();
                        string attachVirtualPath = attachmentUrl.Replace(blogHostUrl, "/");

                        // If we are embedding attachements then we need to get the data stream 
                        // for the attachment, else the datastream can be null.
                        if (embed)
                        {
                            string attachPhysicalPath = HttpUtility.UrlDecode(HttpContext.Current.Server.MapPath(attachVirtualPath));
                            
                            //using (Stream attachStream = new StreamReader(attachPhysicalPath).BaseStream)
                            using (FileStream attachStream = File.OpenRead(attachPhysicalPath))
                            {
                                using (BinaryReader reader = new BinaryReader(attachStream))
                                {
                                    reader.BaseStream.Position = 0;
                                    byte[] data = reader.ReadBytes((int)attachStream.Length);
                                    attachment.Data = data;
                                }
                            }
                        }

                        attachment.Embedded = embed;
                        attachment.MimeType = BlogMLWriter.GetMimeType(attachmentUrl);
                        attachment.Path = attachVirtualPath;
                        attachment.Url = attachmentUrl;
                        attachments.Add(attachment);
                    }
                }
            }
            return attachments;
        }

        private static void PopulateAuthors(IPagedCollection<BlogMLPost> posts, IDataReader reader)
	    {
            PostChildrenPopulator populator = delegate(BlogMLPost bmlPost)
            {
                bmlPost.Authors.Add(DataHelper.ReadInt32(reader, "AuthorId").ToString(CultureInfo.InvariantCulture));
            };

            ReadAndPopulatePostChildren(posts, reader, "Id", populator);
	    }

		private static void PopulateCategories(IPagedCollection<BlogMLPost> posts, IDataReader reader)
		{
			PostChildrenPopulator populator = delegate(BlogMLPost bmlPost)
			{
				bmlPost.Categories.Add(DataHelper.ReadInt32(reader, "CategoryId").ToString(CultureInfo.InvariantCulture));
			};

			ReadAndPopulatePostChildren(posts, reader, "Id", populator);
		}

		private static void PopulateComments(IPagedCollection<BlogMLPost> bmlPosts, IDataReader reader)
		{
			PostChildrenPopulator populator = delegate(BlogMLPost bmlPost)
			{
				bmlPost.Comments.Add(ObjectHydrator.LoadCommentFromDataReader(reader));
			};
			
			ReadAndPopulatePostChildren(bmlPosts, reader, "EntryId", populator);
		}

		private static void PopulateTrackbacks(IPagedCollection<BlogMLPost> bmlPosts, IDataReader reader)
		{
			PostChildrenPopulator populator = delegate(BlogMLPost bmlPost)
			{
				bmlPost.Trackbacks.Add(ObjectHydrator.LoadTrackbackFromDataReader(reader));
			};

			ReadAndPopulatePostChildren(bmlPosts, reader, "EntryId", populator);
		}

		private static void ReadAndPopulatePostChildren(IPagedCollection<BlogMLPost> bmlPosts, IDataReader reader, string foreignKey, PostChildrenPopulator populatePostChildren)
		{
			for (int i = 0; i < bmlPosts.Count; i++)
			{
				BlogMLPost post = bmlPosts.ElementAt(i);
				int postId = int.Parse(post.ID);
				// We are going to make use of the fact that everything is ordered by Post Id ASC
				// to optimize this...
				while (reader.Read())
				{
					int postIdForeignKey = DataHelper.ReadInt32(reader, foreignKey);

					if (postId < postIdForeignKey)
					{
						while (postId < postIdForeignKey && i < bmlPosts.Count)
						{
							i++;
							post = bmlPosts.ElementAt(i);
							postId = int.Parse(post.ID);
						}
					}

					if (postId > postIdForeignKey)
						continue;

					if (postId == postIdForeignKey)
						populatePostChildren(post);
				}
			}
		}
		
		private delegate void PostChildrenPopulator(BlogMLPost post);

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
		public override BlogMLBlog GetBlog(string blogId)
		{
			int blogIdentifier;
			if (int.TryParse(blogId, out blogIdentifier))
			{
				BlogInfo blog = BlogInfo.GetBlogById(blogIdentifier);
				BlogMLBlog bmlBlog = new BlogMLBlog();
				bmlBlog.Title = blog.Title;
				bmlBlog.SubTitle = blog.SubTitle;
				bmlBlog.RootUrl = blog.RootUrl.ToString();
				bmlBlog.DateCreated = blog.TimeZone.Now;

			    // TODO: in Subtext 2.0 we need to account for multiple authors.
                BlogMLAuthor bmlAuthor = new BlogMLAuthor();
                bmlAuthor.ID = blog.Id.ToString();
                bmlAuthor.Title = blog.Author;
                bmlAuthor.Approved = true;
                bmlAuthor.Email = blog.Email;
                bmlAuthor.DateCreated = blog.LastUpdated;
                bmlAuthor.DateModified = blog.LastUpdated;
			    bmlBlog.Authors.Add(bmlAuthor);
			    
			    // Add Extended Properties
			    Pair<string, string> bmlExtProp = new Pair<string, string>();
                bmlExtProp.Key = BlogMLBlogExtendedProperties.CommentModeration;
                bmlExtProp.Value = blog.ModerationEnabled
                                       ? CommentModerationTypes.Enabled.ToString()
                                       : CommentModerationTypes.Disabled.ToString();
			    bmlBlog.ExtendedProperties.Add(bmlExtProp);

                /* TODO: The blog.TrackbasksEnabled determines if Subtext will ACCEPT and SEND trackbacks.
                 * Perhaps we should separate the two out?
                 * For now, we'll assume that if a BlogML blog allows sending, it will also
                 * allow receiving track/pingbacks.
                 */
                bmlExtProp.Key = BlogMLBlogExtendedProperties.EnableSendingTrackbacks;
                bmlExtProp.Value = blog.TrackbacksEnabled
                                       ? SendTrackbackTypes.Yes.ToString()
                                       : SendTrackbackTypes.No.ToString();
			    
				return bmlBlog;
			}
			return null;
		}

		/// <summary>
		/// Returns every blog category in the blog.
		/// </summary>
		/// <param name="blogId"></param>
		/// <returns></returns>
		public override ICollection<BlogMLCategory> GetAllCategories(string blogId)
		{
			ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.PostCollection, ActiveFilter.None);
			ICollection<BlogMLCategory> bmlCategories = new Collection<BlogMLCategory>();
			
			foreach(LinkCategory category in categories)
			{
				BlogMLCategory bmlCategory = new BlogMLCategory();
                bmlCategory.ID = category.Id.ToString();
				bmlCategory.Title = category.Title;
				bmlCategory.Approved = category.IsActive;
				bmlCategory.DateCreated = DateTime.Now;
				bmlCategory.DateModified = DateTime.Now;
			    if (category.HasDescription)
                    bmlCategory.Description = category.Description;
				
				bmlCategories.Add(bmlCategory);
			}
			return bmlCategories;
		}

		/// <summary>
		/// Returns the context under which blogml import or export is running under.
		/// </summary>
		/// <returns></returns>
		public override IBlogMLContext GetBlogMlContext()
		{
			bool embedValue = false;
			if(HttpContext.Current != null && HttpContext.Current.Request != null)
				embedValue = String.Equals(HttpContext.Current.Request.QueryString["embed"], "true", StringComparison.InvariantCultureIgnoreCase);

			return new BlogMLContext(Config.CurrentBlog.Id.ToString(CultureInfo.InvariantCulture), embedValue);
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

		/// <summary>
		/// Method called before an import begins. Allows the provider to 
		/// initialize any state in the current blog.
		/// </summary>
		public override void PreImport()
		{
			duplicateCommentsEnabled = Config.CurrentBlog.DuplicateCommentsEnabled;
			if (!duplicateCommentsEnabled)
			{
				// Allow duplicate comments temporarily.
				Config.CurrentBlog.DuplicateCommentsEnabled = true;
				Config.UpdateConfigData(Config.CurrentBlog);
			}
		}
		
		/// <summary>
		/// Method called when an import is complete.
		public override void ImportComplete()
		{
			if (Config.CurrentBlog.DuplicateCommentsEnabled != duplicateCommentsEnabled)
			{
				Config.CurrentBlog.DuplicateCommentsEnabled = duplicateCommentsEnabled;
				Config.UpdateConfigData(Config.CurrentBlog);
			}
		}

		/// <summary>
		/// Creates categories from the blog ml.
		/// </summary>
		/// <remarks>
		/// At this time, we only support PostCollection link categories.
		/// </remarks>
		/// <param name="blog"></param>
		public override IDictionary<string, string> CreateCategories(BlogMLBlog blog)
		{
			IDictionary<string, string> idMap = new Dictionary<string, string>();
			foreach (BlogMLCategory bmlCategory in blog.Categories)
			{
				LinkCategory category = new LinkCategory();
				category.BlogId = Config.CurrentBlog.Id;
				category.Title = bmlCategory.Title;
				category.Description = bmlCategory.Description;
				category.IsActive = bmlCategory.Approved;
				category.CategoryType = CategoryType.PostCollection;
				Links.CreateLinkCategory(category);
				idMap.Add(bmlCategory.ID, category.Title);
			}
			return idMap;
		}

		/// <summary>
		/// The physical path to the attachment directory.
		/// </summary>
		/// <remarks>
		/// The attachment is passed in to give the blog engine 
		/// the opportunity to use attachment specific directories 
		/// (ex. based on mime type) should it choose.
		/// </remarks>
		public override string GetAttachmentDirectoryPath(BlogMLAttachment attachment)
		{
			return Config.CurrentBlog.ImageDirectory;
		}

		/// <summary>
		/// The url to the attachment directory
		/// </summary>
		/// <remarks>
		/// The attachment is passed in to give the blog engine 
		/// the opportunity to use attachment specific directories 
		/// (ex. based on mime type) should it choose.
		/// </remarks>
		public override string GetAttachmentDirectoryUrl(BlogMLAttachment attachment)
		{
			return Config.CurrentBlog.ImagePath;
		}

		/// <summary>
		/// Creates a blog post and returns the id.
		/// </summary>
        /// <param name="blog"></param>
		/// <param name="post"></param>
		/// <param name="content"></param>
		/// <param name="categoryIdMap">A dictionary used to map the blogml category id to the internal category id.</param>
		/// <returns></returns>
		public override string CreateBlogPost(BlogMLBlog blog, BlogMLPost post, string content, IDictionary<string, string> categoryIdMap)
		{
            Entry newEntry = new Entry((post.PostType == BlogPostTypes.Article) ? PostType.Story : PostType.BlogPost);
			newEntry.BlogId = Config.CurrentBlog.Id;
			newEntry.Title = post.Title;
			newEntry.DateCreated = post.DateCreated;
			newEntry.DateModified = post.DateModified;
			newEntry.DateSyndicated = post.DateModified;  // is this really the best thing to do?
			newEntry.Body = content;
		    if (post.HasExcerpt)
                newEntry.Description = post.Excerpt.Text;
			newEntry.IsActive = post.Approved;
			newEntry.DisplayOnHomePage = post.Approved;
			newEntry.IncludeInMainSyndication = post.Approved;
			newEntry.IsAggregated = post.Approved;
			newEntry.AllowComments = true;
            if (post.Authors.Count > 0)
            {
                foreach (BlogMLAuthor author in blog.Authors)
                {
                    if (author.ID == post.Authors[0].Ref)
                    {
                        newEntry.Author = author.Title;
                        newEntry.Email = author.Email;
                        break;
                    }
                }
            }

            if (!string.IsNullOrEmpty(post.PostName))
                newEntry.EntryName = Entries.AutoGenerateFriendlyUrl(post.PostName, newEntry.Id);
			
			foreach(BlogMLCategoryReference categoryRef in post.Categories)
			{
				string categoryTitle;
				if(categoryIdMap.TryGetValue(categoryRef.Ref, out categoryTitle))
					newEntry.Categories.Add(categoryTitle);
			}
			
			return Entries.Create(newEntry).ToString(CultureInfo.InvariantCulture);
		}

		/// <summary>
		/// Creates a comment in the system.
		/// </summary>
		/// <param name="bmlComment"></param>
		/// <param name="newPostId"></param>
		public override void CreatePostComment(BlogMLComment bmlComment, string newPostId)
		{
			FeedbackItem newComment = new FeedbackItem(FeedbackType.Comment);
			newComment.BlogId = Config.CurrentBlog.Id;
			newComment.EntryId = int.Parse(newPostId);
			newComment.Title = bmlComment.Title ?? string.Empty;
			newComment.DateCreated = bmlComment.DateCreated;
			newComment.DateModified = bmlComment.DateModified;
			newComment.Body = StringHelper.ReturnCheckForNull(bmlComment.Content.UncodedText);
			newComment.Approved = bmlComment.Approved;
			newComment.Author = StringHelper.ReturnCheckForNull(bmlComment.UserName);
			newComment.Email = bmlComment.UserEMail;
		    
		    if (!string.IsNullOrEmpty(bmlComment.UserUrl))
		    {
		        newComment.SourceUrl = new Uri(bmlComment.UserUrl);
		    }

			FeedbackItem.Create(newComment, null);
		}

		/// <summary>
		/// Creates a trackback for the post.
		/// </summary>
		/// <param name="trackback"></param>
		/// <param name="newPostId"></param>
		public override void CreatePostTrackback(BlogMLTrackback trackback, string newPostId)
		{
			FeedbackItem newPingTrack = new FeedbackItem(FeedbackType.PingTrack);
			newPingTrack.BlogId = Config.CurrentBlog.Id;
			newPingTrack.EntryId = int.Parse(newPostId);
			newPingTrack.Title = trackback.Title;
			newPingTrack.SourceUrl = new Uri(trackback.Url);
			newPingTrack.Approved = trackback.Approved;
			newPingTrack.DateCreated = trackback.DateCreated;
			newPingTrack.DateModified = trackback.DateModified;
			// we use an actual name here, but BlogML doesn't support this, so let's try  
			// to parse the url's host out of the url.
			newPingTrack.Author = UrlFormats.GetHostFromExternalUrl(trackback.Url);
			// so the duplicate Comment Filter doesn't break when computing the checksum
			newPingTrack.Body = string.Empty;

			FeedbackItem.Create(newPingTrack, null);
		}

	    public override void SetBlogMlExtendedProperties(BlogMLBlog.ExtendedPropertiesCollection extendedProperties)
	    {
            if (extendedProperties != null && extendedProperties.Count > 0)
            {
                BlogInfo info = Config.CurrentBlog;
                
                foreach (Pair<string, string> extProp in extendedProperties)
                {
                    if (BlogMLBlogExtendedProperties.CommentModeration.Equals(extProp.Key))
                    {
                        bool modEnabled;
                        
                        if (bool.TryParse(extProp.Value, out modEnabled))
                        {
                            info.ModerationEnabled = modEnabled;
                        }
                    }
                    else if (BlogMLBlogExtendedProperties.EnableSendingTrackbacks.Equals(extProp.Key))
                    {
                        bool tracksEnabled;

                        if (bool.TryParse(extProp.Value, out tracksEnabled))
                        {
                            /* TODO: The blog.TrackbasksEnabled determines if Subtext will ACCEPT and SEND trackbacks.
                             * Perhaps we should separate the two out?
                             * For now, we'll assume that if a BlogML blog allows sending, it will also
                             * allow receiving track/pingbacks.
                             */
                            info.TrackbacksEnabled = tracksEnabled;
                        }
                    }
                }

                Config.UpdateConfigData(info);
            }
	    }

	    /// <summary>
		/// Lets the provider decide how to log errors.
		/// </summary>
		/// <param name="message"></param>
		/// <param name="e"></param>
		public override void LogError(string message, Exception e)
		{
			//TODO:
		}
	}
}
