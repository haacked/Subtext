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
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Web;
using BlogML;
using log4net;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;

namespace Subtext.Framework.Import
{
	/// <summary>
	/// Exports a blog to a BlogML file.  This is based on the BlogML standard 
	/// proposed by Darren Neimke in <see href="http://markitup.com/Posts/PostsByCategory.aspx?categoryId=5751cee9-5b20-4db1-93bd-7e7c66208236">http://markitup.com/Posts/PostsByCategory.aspx?categoryId=5751cee9-5b20-4db1-93bd-7e7c66208236</see>
	/// </summary>
	public sealed class SubtextBlogMLWriter : BlogMLWriterBase
	{
		#region Private Members
			
		private int blogId;
		private bool isUseGuids;
		private string host;
		private Dictionary<int, string> categoryIdMap;
		private readonly static ILog log = new Log();
		
		// Used by Sql Data Handlers //////////////////////////
		private string connectionString;
		private SqlConnection connection;
		private bool isConnectionReady = false;
		
		#endregion

		#region Constructor

		/// <summary>
		/// Creates new instance of the SubtextBlogMLWriter.
		/// </summary>
		/// <param name="connectionString">Connection string to use to access .TEXT data store.</param>
		/// <param name="blogId">The ID of you're .TEXT blog.</param>
		/// <param name="isUseGuids">True if you want the writer to convert id's to Guids.
		/// If you specify false the .TEXT int ID's will be retained.</param>
		public SubtextBlogMLWriter(string connectionString, 
									int blogId, 
									bool isUseGuids)
		{
			#region Parameter Checking
			
			if (connectionString == null)
			{
				throw(new ArgumentNullException("connectionString", 
												  "Unable to create new DOTTextBlogMLWriter. Connection String cannot be null."));
			}

			if (connectionString.Length == 0)
			{
				throw(new ArgumentException("Unable to create new DOTTextBlogMLWriter. Connection String cannot be empty.", 
											  "connectionString"));
			}

			#endregion

			this.blogId = blogId;
			this.connectionString = connectionString;
			this.isUseGuids = isUseGuids;
			this.categoryIdMap = new Dictionary<int, string>();
		}

		#endregion

		#region BlogMLWriterBase Implementations

		protected override void InternalWriteBlog()
		{
			WriteBlog();
		}

		#endregion		

		#region Private BlogML Writing Methods
		
		private void WriteBlog()
		{
			try
			{
				WriteFromBlogConfig();
				WriteCategories();
				WritePosts();

				WriteEndElement(); // End Blog Element
				
			}
			finally
			{
				if (this.connection != null)
				{
					CloseConnection();
				}
			}
		}

		// Writes StartBlog and Author.
		private void WriteFromBlogConfig()
		{
			BlogInfo blog = BlogInfo.GetBlogById(this.blogId);

			if (blog == null)
				throw new BlogDoesNotExistException(this.blogId);
			
			this.host = blog.Host;
					
			WriteStartBlog(blog.Title, blog.SubTitle, this.host, DateTime.Now);
			WriteAuthor(blog.Author, blog.Email);
		}

		// Write Categories
		private void WriteCategories()
		{
			string blogMlCategoryId = string.Empty;		

            try
			{
				WriteStartCategories();

				ICollection<LinkCategory> categories = Links.GetCategories(CategoryType.None, ActiveFilter.ActiveOnly);

				foreach (LinkCategory category in categories)
				{
					if (!this.categoryIdMap.ContainsKey(category.Id))
					{
						if (this.isUseGuids)
						{
							blogMlCategoryId = Guid.NewGuid().ToString();
						}
						else
						{
							blogMlCategoryId = category.Id.ToString(CultureInfo.InvariantCulture);
						}

						// tracks categories
						// if we are using guids then we need to track categories
						// when adding them to posts elements.
						this.categoryIdMap.Add(category.Id, blogMlCategoryId);
					}

					WriteCategory(blogMlCategoryId,
					              category.Title,
					              DateTime.Now,
					              DateTime.Now,
					              true,
					              category.Description,
					              null);

					Writer.Flush();
				}
				
				WriteEndElement(); //End Categories Element
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to write categories.", ex));
			}
		}

		private void WritePosts()
		{
			IPagedCollection<Entry> entries = Entries.GetPagedEntries(PostType.BlogPost, -1, 0, int.MaxValue);
			WriteStartPosts();

			foreach(Entry entry in entries)
			{
				WritePost(entry);
				Writer.Flush();
			}

			WriteEndElement(); // End Posts Element
			Writer.Flush();
		}

		private void WritePost(Entry post)
		{
			int currentPostId;
			string newPostID;
			string postContent = post.Body;
			
			try
			{
				currentPostId = post.Id;

                if (this.isUseGuids)
                {
                	newPostID = Guid.NewGuid().ToString();
                }
				else
                {
                	newPostID = currentPostId.ToString(); 
                }

				WriteStartPost(newPostID, post.Title, post.DateCreated.ToUniversalTime(), post.DateUpdated.ToUniversalTime(), true, postContent, GetPostUrl(newPostID));
				Writer.Flush();

				WritePostAttachments(postContent);
				WritePostComments(currentPostId);
				WritePostCategories(currentPostId);
				WritePostTrakbacks(currentPostId);
				
				WriteEndElement();	//End Post Element
				Writer.Flush();

			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to write individual post.", ex));
			}
		}

		private void WritePostAttachments(string content)
		{
			string[] imagesURLs = SgmlUtil.GetAttributeValues(content, "img", "src");
			string imageURL;
			string fullImageURL;
			string appFullRootUrl = "http://" + Config.CurrentBlog.Host.ToLower() 
				+ StringHelper.ReturnCheckForNull(HttpContext.Current.Request.ApplicationPath).ToLower();
			
			if (imagesURLs.Length > 0)
			{
				WriteStartAttachments();
				for (int i=0; i < imagesURLs.Length; i++)
				{
					imageURL = imagesURLs[i].ToLower();

					// now we need to determine if the URL is local
					if (SgmlUtil.IsRootUrlOf(appFullRootUrl, imageURL))
					{
						try
						{
							// make sure to write the imageURL as-is in the post so it can be
							// found and fixed when un-serializing the blog later.
							fullImageURL = UrlFormats.GetImageFullUrl(imageURL);

							WriteAttachment(imageURL, GetMimeType(imageURL), fullImageURL);
							Writer.Flush();
						}
						catch (Exception e)
						{
							/* REVIEW:
							 * We should only catch exceptions we can really handle...
							 * In this case, we should wrap this in an exception that 
							 * gives this more context (ex... BlogMLAttachmentException) and 
							 * throw that. Let the main unhandled exception handler do the logging.
							 */

							log.Error(string.Format(
								"An error occured while trying to write an attachment for this blog. Error: {0}", e.Message),
								e);
						}
					}
				}
				WriteEndElement(); // End Attachments Element
				Writer.Flush();
			}
		}
		
		private void WritePostComments(int postID)
		{
			DataSet dsComments;
			string commentID;

		
			dsComments = GetPostComments(postID);
            
			if (dsComments.Tables[0].Rows.Count > 0)
			{
				WriteStartComments();
                
				foreach(DataRow comment in dsComments.Tables[0].Rows)
				{
					if (this.isUseGuids)
					{
						commentID = Guid.NewGuid().ToString();
					}
					else
					{
						commentID = comment["ID"].ToString();
					}

					WriteComment(commentID, 
					             comment["Title"] as string,
					             DateTime.Parse(comment["DateAdded"].ToString()),
					             DateTime.Parse(comment["DateUpdated"].ToString()),
					             true,
					             comment["Author"] as string,
					             comment["Email"] as string,
					             comment["TitleUrl"] as string,
					             comment["Text"] as string);
				
					Writer.Flush();
				}

				WriteEndElement(); // End Comments Element
				Writer.Flush();
			}
		}

		private void WritePostCategories(int postID)
		{
			DataSet dsCategories;
			
			dsCategories = GetPostCategories(postID);
			
			if (dsCategories.Tables[0].Rows.Count > 0)
			{
				WriteStartCategories();

				foreach(DataRow postCategoryId in dsCategories.Tables[0].Rows)
				{
					/*if (postCategoryId["CategoryID"] != null && typeof(DBNull) != postCategoryId["CategoryID"].GetType())
					{*/
						int postCatId = (int)postCategoryId["CategoryID"];
						WriteCategoryReference(this.categoryIdMap[postCatId].ToString());
						Writer.Flush();
					/*}*/
				}

				WriteEndElement();
				Writer.Flush();
			}
		}

		private void WritePostTrakbacks(int postID)
		{
			DataSet dsTrackBacks;
			string trackbackID;
		
			dsTrackBacks = GetPostTrackbacks(postID);

			if (dsTrackBacks.Tables[0].Rows.Count > 0)
			{
				WriteStartTrackbacks();

				foreach (DataRow trackback in dsTrackBacks.Tables[0].Rows)
				{
					if (this.isUseGuids)
					{
						trackbackID = Guid.NewGuid().ToString();
					}
					else
					{
						trackbackID = trackback["ID"].ToString();
					}

					WriteTrackback(trackbackID, 
									trackback["Title"] as string, 
									DateTime.Parse(trackback["DateAdded"].ToString()),
									DateTime.Parse(trackback["DateUpdated"].ToString()),
									true,
									trackback["TitleUrl"] as string);
					Writer.Flush();
				}

                WriteEndElement(); // End Trackbacks element
				Writer.Flush();
			}
		}

		#endregion

		#region Util Methods

		private string GetPostUrl(string postID)
		{
			return string.Format("http://{0}/Posts/Post.aspx?postID={1}", this.host, postID);
		}

		private static string GetMimeType(string fullUrl) 
		{
			string extension = Path.GetExtension(fullUrl);
			string retVal;

			if (extension == null || extension.Length==0) 
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
		#endregion

		#region subText Data Access Methods

		private DataSet GetPostComments(int postID)
		{
			DataSet ds;

			try
			{
				ds = GetFeedBackItems(postID, 3);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get Post Comments", ex));
			}

			return ds;
		}

		private DataSet GetPostTrackbacks(int postID)
		{
			DataSet ds;
			
			try
			{
				ds = GetFeedBackItems(postID, 4);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get Post Trackbaks", ex));
			}

			return ds;
		}

		/// <summary>
		/// Gets a dataset containing trackbacks or comments for a specified post.
		/// </summary>
		/// <param name="PostID"></param>
		/// <param name="postType">
		///		postType = 3 Comment
		///		postType = 4 Trackback
		/// </param>
		/// <returns></returns>
		private DataSet GetFeedBackItems(int PostID, int postType)
		{
			DataSet dsFeedBackItems;
            SqlCommand cmd;
			// no procedure exists to get only comments for a post.
			string sql = string.Format("SELECT * FROM subtext_Content WHERE BlogId ={0} and PostType ={1} AND subtext_Content.PostConfig & 1 = 1 AND subtext_Content.ParentID ={2} ORDER BY [ID]",
			                           this.blogId, postType, PostID);

			try
			{
				cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				dsFeedBackItems = ExecuteDataSet(cmd);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get feedback items.", ex));
			}

			return dsFeedBackItems;
		}

		private DataSet GetPostCategories(int PostID)
		{
			DataSet ds;
			SqlCommand cmd;
			// no procedure exists  to get just the post categories.
			string sql = "select * from subtext_links where PostID = " + PostID.ToString() ;

			try
			{
				cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				ds = ExecuteDataSet(cmd);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get Post Categories.", ex));
			}

			return ds;
		}

		#endregion

		#region SqlServer Data handlers - Written By Rocky Heckman

		/*
		 * Provide access to an Sql Server database.
		 * These methods have been written by Rocky Heckman (http://www.rockyh.net).
		 * 
		 * An ExecuteDataSet method has been added.
		 */
	
		private void InitConnection() 
		{
			if (!this.isConnectionReady) 
			{
				//_connectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"] as string;
				
				if (this.connectionString == null)
				{
					this.isConnectionReady = false;
					throw new ArgumentNullException("Connection String", "The connection string could not be loaded to access the SQL Server.");
				}
				else
				{
					this.connection = new SqlConnection(this.connectionString);
					this.isConnectionReady = true;
				}
			}
		}


		void CloseConnection() 
		{
			if (this.connection.State != ConnectionState.Closed)
			{
				this.connection.Close() ;
			}
		}

		/// <summary>
		/// This method is called internally to retrieve a DataSet from the data store.
		/// </summary>
		/// <param name="cmd">A valid SqlCommand object complete with parameters to execute the update
		/// or delete action. </param>
		/// <remarks>This method is not part of the original data access methods written by Rocky.
		/// Jim V has added this method due to the fact that we are not able to get data while 
		/// reading from a data reader.
		/// </remarks>
		/// <returns>A dataset from the datastore.</returns>
		private DataSet ExecuteDataSet(SqlCommand cmd)
		{
			DataSet ds = new DataSet();
			SqlDataAdapter adapter;

			if (!this.isConnectionReady)
			{
				InitConnection();
			}
			cmd.Connection = this.connection;
			
			if (this.connection.State != ConnectionState.Open)
			{
				this.connection.Open();	
			}
			
			adapter = new SqlDataAdapter(cmd);
			adapter.Fill(ds);
			return ds;
		}

		#endregion
	}
}
