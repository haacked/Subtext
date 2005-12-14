using System;
using System.Collections;
using System.Data;
using System.Data.SqlClient;
using System.Web;
using BlogML;
using log4net;
using Subtext.Framework.Configuration;
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
			
		private int _BlogID;
		private bool _IsUseGuids;
		private string _Host;
		private Hashtable _Categories = new Hashtable();
		private readonly static ILog log = new Log();
		
		// Used by Sql Data Handlers //////////////////////////
		private string _connectionString;
		private SqlConnection _connection;
		private bool _connectionIsReady = false;
		///////////////////////////////////////////////////////
		
		
		#endregion

		#region Constructor

		/// <summary>
		/// Creates new instance of the SubtextBlogMLWriter.
		/// </summary>
		/// <param name="connectionString">Connection string to use to access .TEXT data store.</param>
		/// <param name="blogID">The ID of you're .TEXT blog.</param>
		/// <param name="isUseGuids">True if you want the writer to convert id's to Guids.
		/// If you specify false the .TEXT int ID's will be retained.</param>
		public SubtextBlogMLWriter( string connectionString, 
									int blogID, 
									bool isUseGuids)
		{
			#region Parameter Checking
			
			if( connectionString == null )
			{
				throw( new ArgumentNullException( "connectionString", 
												  "Unable to create new DOTTextBlogMLWriter. Connection String cannot be null.") );
			}

			if( connectionString == string.Empty )
			{
				throw( new ArgumentException( "Unable to create new DOTTextBlogMLWriter. Connection String cannot be empty.", 
											  "connectionString") );
			}

			#endregion

			_BlogID = blogID;
			_connectionString = connectionString;			
			_IsUseGuids = isUseGuids;			
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
			catch (Exception ex)
			{
				throw(new Exception("Unable output to BlogML. Please Refere to exception for details.", ex));
			}
			finally
			{
				if( _connection != null )
				{
					CloseConnection();
				}
			}
		}

		// Writes StartBlog and Author.
		private void WriteFromBlogConfig()
		{
			using( SqlDataReader reader = GetBlogConfig() )
			{
				if( reader.HasRows )
				{
					reader.Read();
					
					// get host from config
					_Host = reader["Host"] as string;

					WriteStartBlog(reader["Title"] as string, 
								   reader["SubTitle"] as string,
								   _Host, 
								   DateTime.Now);

					WriteAuthor(reader["Author"] as string, reader["Email"] as string);

				}
				else
				{
					throw( new Exception("Unable to get config for supplied Blog ID.") );
				}
			}
		}

		// Write Categories
		private void WriteCategories()
		{
			string categoryID = string.Empty;		

            try
			{
				WriteStartCategories();

				using( SqlDataReader reader = GetCategories() )
				{
					if( reader.HasRows )
					{
						while( reader.Read() )
						{
							if( !_Categories.ContainsKey(reader["CategoryID"].ToString()) )
							{
								if( _IsUseGuids )
								{
									categoryID = Guid.NewGuid().ToString();
								}
								else
								{
									categoryID = reader["CategoryID"].ToString();
								}
								
								// tracks categories
								// if we are using guids then we need to track categories
								// when adding them to posts elements.
								_Categories.Add(reader["CategoryID"].ToString(), categoryID);
							}
							
							WriteCategory(categoryID,
										  reader["Title"] as string,
										  DateTime.Now,
										  DateTime.Now,
										  true,
										  reader["Description"] as string,
										  null);

							Writer.Flush();
						}
					}
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
			DataSet dsPosts = null;

			try
			{
				dsPosts = GetPosts();
                
				WriteStartPosts();

				foreach( DataRow post in dsPosts.Tables[0].Rows )
				{
					WritePost( post);
					Writer.Flush();
				}

				WriteEndElement(); // End Posts Element
				Writer.Flush();
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to write posts.", ex));
			}
			finally
			{
				dsPosts = null;
			}
		}

		private void WritePost( DataRow post )
		{
			int currentPostId = -1;
			string newPostID = string.Empty;
			string postContent = post["Text"] as string;
			
			try
			{
				currentPostId = int.Parse(post["ID"].ToString());

                if( _IsUseGuids )
                {
                	newPostID = Guid.NewGuid().ToString();
                }
				else
                {
                	newPostID = currentPostId.ToString(); 
                }

				WriteStartPost(newPostID, 
							   post["Title"] as string,
							   DateTime.Parse(post["DateAdded"].ToString()).ToUniversalTime(),
							   DateTime.Parse(post["DateUpdated"].ToString()).ToUniversalTime(),
							   true,
							   postContent, 
							   GetPostUrl(newPostID),
							   false);
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
			string imageURL = null;
			string appFullRootUrl = "http://" + Config.CurrentBlog.Host.ToLower() 
				+ StringHelper.ReturnCheckForNull(HttpContext.Current.Request.ApplicationPath);
			
			if(imagesURLs.Length > 0)
			{
				WriteStartAttachments();
				for(int i=0; i < imagesURLs.Length; i++)
				{
					imageURL = imagesURLs[i].ToLower();

					// now we need to determine if the URL is local
					if(SgmlUtil.IsRootUrlOf(appFullRootUrl, imageURL))
					{
						try
						{
							// make sure to write the imageURL as-is in the post so it can be
							// found and fixed when un-serializing the blog later.
							WriteAttachment(
								imageURL, 
								UrlFormats.GetImageFullUrl(imageURL));
							Writer.Flush();
						}
						catch(Exception e)
						{
							// lets do some error logging!
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
		
		private void WritePostComments( int postID )
		{
			DataSet dsComments = null;
			string commentID = string.Empty;

			try
			{
				dsComments = GetPostComments(postID);
             
				if( dsComments.Tables[0].Rows.Count > 0 )
				{
					WriteStartComments();
                   
					foreach( DataRow comment in dsComments.Tables[0].Rows )
					{
						if( _IsUseGuids )
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
									 comment["Text"] as string,
									 false);
					
						Writer.Flush();
					}

					WriteEndElement(); // End Comments Element

					Writer.Flush();
				}
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to Write Post Comments.", ex));
			}
			finally
			{
				dsComments = null;
			}
		}

		private void WritePostCategories( int postID )
		{
			DataSet dsCategories = null;

			try
			{
				dsCategories = GetPostCategories(postID);
				
				if( dsCategories.Tables[0].Rows.Count > 0 )
				{
					WriteStartCategories();

					foreach( DataRow postCategoryId in dsCategories.Tables[0].Rows )
					{
						WriteCategoryReference( _Categories[postCategoryId["CategoryID"].ToString()].ToString());

						Writer.Flush();
					}

					WriteEndElement();

					Writer.Flush();
				}
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to write Post Categories.", ex));
			}
			finally
			{
				dsCategories = null;
			}
		}

		private void WritePostTrakbacks( int postID )
		{
			DataSet dsTrackBacks = null;
			string trackbackID = string.Empty;

			try
			{
				dsTrackBacks = GetPosTrackbacks( postID );

				if( dsTrackBacks.Tables[0].Rows.Count > 0 )
				{
					WriteStartTrackbacks();

					foreach( DataRow trackback in dsTrackBacks.Tables[0].Rows )
					{
						if( _IsUseGuids )
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
			catch (Exception ex)
			{
				throw(new Exception("Unable to write Post Trackbacks.", ex));
			}
			finally
			{
				dsTrackBacks = null;
			}
		}

		#endregion

		#region Util Methods

		private string GetPostUrl( string postID )
		{
			return string.Format("http://{0}/Posts/Post.aspx?postID={1}", _Host, postID );
		}

		#endregion

		#region subText Data Access Methods

		private SqlDataReader GetBlogConfig()
		{
			SqlDataReader reader = null;
			SqlCommand cmd = null;

			try
			{
				cmd = new SqlCommand(string.Format("SELECT Title, SubTitle, Host, Author, Email FROM subtext_config WHERE BlogID = {0}", _BlogID) );
				cmd.CommandType = CommandType.Text;
				
	            reader = ExecuteReader(cmd);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get Blog Config.", ex));
			}

			return reader;
		}

		private SqlDataReader GetCategories()
		{
			SqlDataReader reader = null;
			SqlCommand cmd = null;

			try
			{		
				cmd = new SqlCommand("subtext_GetAllCategories");
				cmd.CommandType = CommandType.StoredProcedure;
				cmd.Parameters.Add("@BlogID", SqlDbType.Int).Value = _BlogID;
				cmd.Parameters.Add("@IsActive", SqlDbType.Bit).Value = 1;
				cmd.Parameters.Add("@CategoryType", SqlDbType.Int).Value = 1;

				reader = ExecuteReader(cmd);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get categories", ex));
			}
			
			return reader;
		}

		private DataSet GetPosts()
		{
			DataSet ds = null;
			SqlCommand cmd = null;
			// not stored procedure that retreives all posts.
			// use sql statement.
			string sql = "select * from subtext_content " +
						 "where blogid = " + _BlogID + " and " +
						 "posttype = 1 and " +
						 "subtext_Content.PostConfig & 1 <> Case 1 When 1 then 0 Else -1 End";

			try
			{
				cmd = new SqlCommand(sql);
				cmd.CommandType = CommandType.Text;

				ds = ExecuteDataSet(cmd);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get Posts.", ex));
			}


			return ds;
		}

		private DataSet GetPostComments( int postID )
		{
			DataSet ds = null;
			//SqlCommand cmd = null;

			try
			{
				
				// subtext_GetFeedBack gets both comments and trackbacks
				// use sql statement
				//
				//cmd = new SqlCommand("subtext_GetFeedBack");
				//cmd.CommandType = CommandType.StoredProcedure;
				//cmd.Parameters.Add("@ParentID", SqlDbType.Int).Value = postID;
				//cmd.Parameters.Add("@BlogID", SqlDbType.Int).Value = _BlogID;

				ds = GetFeedBackItems( postID, 3);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get Post Comments", ex));
			}

			return ds;
		}

		private DataSet GetPosTrackbacks( int postID )
		{
			DataSet ds = null;
			
			try
			{
				ds = GetFeedBackItems( postID, 4);
			}
			catch (Exception ex)
			{
				throw(new Exception("Unable to get Post Trackbaks", ex));
			}

			return ds;
		}

		// Gets a dataset containing trackbacks or comments 
		// for a specified post.
		// postType = 3 Comment
		// postType = 4 Trackback
		private DataSet GetFeedBackItems( int PostID, int postType )
		{
			DataSet dsFeedBackItems = null;
            SqlCommand cmd = null;
			// no procedure exists to get only comments for a post.
			string sql = string.Format("SELECT * FROM subtext_Content WHERE BlogID ={0} and PostType ={1} AND subtext_Content.PostConfig & 1 = 1 and subtext_Content.ParentID ={2} ORDER BY [ID]",
									   _BlogID, postType, PostID);

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

		private DataSet GetPostCategories( int PostID )
		{
			DataSet ds = null;
			SqlCommand cmd = null;
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
			if(!_connectionIsReady) 
			{
				//_connectionString = System.Configuration.ConfigurationSettings.AppSettings["ConnectionString"] as string;
				
				if(_connectionString == null)
				{
					_connectionIsReady = false;
					throw new ArgumentNullException("Connection String", "The connection string could not be loaded to access the SQL Server.");
				}
				else
				{
					_connection = new SqlConnection(_connectionString);
					_connectionIsReady = true;
				}
			}
		}


		void CloseConnection() 
		{
			if( this._connection.State != ConnectionState.Closed ) 
			{
				this._connection.Close() ;
			}
		}



		/// <summary>
		/// this method is called internally in order to retrieve some data from the database. 
		/// </summary>
		/// <param name="cmd"></param>
		/// <returns></returns>
		private SqlDataReader ExecuteReader(SqlCommand cmd) 
		{
			SqlDataReader reader = null;
			try 
			{
				if(!_connectionIsReady)
					this.InitConnection();
				cmd.Connection = _connection;
				_connection.Open();
				reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
				return reader;
			}
			catch(Exception ex) 
			{
				_connection.Close();
				throw new ApplicationException("An error occured trying to excute a query on the data store. Please see inner exception for details. ", ex);
			}

		}



		/// <summary>
		/// This method is called internally to retrieve single field data from the data store. 
		/// </summary>
		/// <param name="cmd">A valid SqlCommand object complete with parameters to execute the update
		/// action. </param>
		/// <returns>Int - The number of rows affected. </returns>
		private string ExecuteScalar(SqlCommand cmd) 
		{
			try 
			{
				if(!_connectionIsReady)
					this.InitConnection();
				cmd.Connection = _connection;
				_connection.Open();
				string scalarValue = (string)cmd.ExecuteScalar();
				this.CloseConnection() ;
				return scalarValue;
			}
			catch(Exception ex) 
			{
				throw new ApplicationException("An error occured trying to excute a query on the data store. Please see inner exception for details. ", ex);
			}
			finally 
			{
				_connection.Close();
			}
		}



		/// <summary>
		/// This method is called internally to update or delete data in the data store. 
		/// </summary>
		/// <param name="cmd">A valid SqlCommand object complete with parameters to execute the update
		/// or delete action. </param>
		/// <returns>Int - The number of rows affected. </returns>
		private int ExecuteNonQuery(SqlCommand cmd) 
		{
			try 
			{
				if(!_connectionIsReady)
					this.InitConnection();
				cmd.Connection = _connection;
				_connection.Open();
				int recsAffected = cmd.ExecuteNonQuery();
				this.CloseConnection() ;
				return recsAffected;
			}
			catch(Exception ex) 
			{
				throw new ApplicationException("An error occured trying to excute a query on the data store. Please see inner exception for details. ", ex);
			}
			finally 
			{
				_connection.Close();
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
			SqlDataAdapter adapter = null;

			try
			{
				if(!_connectionIsReady)
				{
					InitConnection();
				}
				cmd.Connection = _connection;
				
				if( _connection.State != ConnectionState.Open )
				{
					_connection.Open();	
				}
				
				adapter = new SqlDataAdapter(cmd);

				adapter.Fill(ds);
			}
			catch (Exception ex)
			{
				throw(new Exception("An error occured trying to excute a query on the data store. Please see inner exception for details. ", ex));
			}

			return ds;
		}

		#endregion
	}
}
