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
using log4net;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Providers;
using Subtext.Framework.Text;

namespace Subtext.Framework.Data
{
	/// <summary>
	/// Provider for using a SQL Server as the back-end data storage 
	/// for Subtext.
	/// </summary>
	public class SqlDataProvider : DbProvider
	{
		private readonly static ILog Log = new Log();
		
		private static SqlParameter BlogIdParam
		{
			get
			{
				int blogId;
				if (InstallationManager.IsInHostAdminDirectory)
					blogId = NullValue.NullInt32;
				else
					blogId = Config.CurrentBlog.Id;

				return DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, DataHelper.CheckNull(blogId));
			}
		}

		#region DbProvider Methods
        public override IDataReader GetPreviousNext(int entryId)
        {
            SqlParameter[] p =
					{
						DataHelper.MakeInParam("@ID", SqlDbType.Int, 4, entryId),
						BlogIdParam
					};

            return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, "subtext_GetEntry_PreviousNext", p);
        }
	    	    
	    public override DataSet GetAggregateHomePageData(int groupId)
	    {
            string sql = "DNW_HomePageData";
            SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, BlogInfo.AggregateBlog.Host),
					DataHelper.MakeInParam("@GroupID", SqlDbType.Int, 4, groupId)
				};

            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, sql, p);
	    }

        public override DataSet GetAggregateStats(int groupId)
        {
            string sql = "DNW_Stats";
            SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, BlogInfo.AggregateBlog.Host),
					DataHelper.MakeInParam("@GroupID", SqlDbType.Int, 4, groupId)
				};

            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, sql, p);
        }

        public override DataSet GetAggregateTotalStats(int groupId)
        {
            string sql = "DNW_Total_Stats";
            SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, BlogInfo.AggregateBlog.Host),
					DataHelper.MakeInParam("@GroupID", SqlDbType.Int, 4, groupId)
				};

            return SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, sql, p);
        }

        public override DataTable GetAggregateRecentPosts(int groupId)
        {
            string sql = "DNW_GetRecentPosts";
            string conn = ConnectionString;

            SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@Host", SqlDbType.NVarChar,100, BlogInfo.AggregateBlog.Host),
					DataHelper.MakeInParam("@GroupID", SqlDbType.Int, 4, groupId)
				};

            return DataHelper.ExecuteDataTable(conn, CommandType.StoredProcedure, sql, p);
        }

        public override DataTable GetAggregateRecentImages(int groupId)
        {
            string sql = "DNW_GetRecentImages";
            string conn = ConnectionString;

            SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@Host", SqlDbType.NVarChar,100, BlogInfo.AggregateBlog.Host),
					DataHelper.MakeInParam("@GroupID", SqlDbType.Int, 4, groupId)
				};

            return DataHelper.ExecuteDataTable(conn, CommandType.StoredProcedure, sql, p);
        }
	    
		#region Statistics

		public override bool TrackEntry(IEnumerable<EntryView> evc)
		{
			if(evc != null)
			{
				foreach(EntryView ev in evc)
				{
					TrackEntry(ev);
				}
				return true;
			}
			
			return false;
		}

		/// <summary>
		/// Inserts a tracking entry for this record.
		/// </summary>
		/// <param name="ev"></param>
		/// <returns></returns>
		public override bool TrackEntry(EntryView ev)
		{
			//Note, for the paramater @URL, do NOT convert null values into empty strings.
			SqlParameter[] p =	
			{
						DataHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, DataHelper.CheckNull(ev.EntryId)),
						DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, DataHelper.CheckNull(ev.BlogId)),
						DataHelper.MakeInParam("@URL", SqlDbType.NVarChar, 255, ev.ReferralUrl),
						DataHelper.MakeInParam("@IsWeb", SqlDbType.Bit,1, ev.PageViewType)
			};
			try
			{
				return NonQueryBool("subtext_TrackEntry", p);
			}
			catch(Exception e)
			{
				Console.WriteLine(e.Message + e.StackTrace);
			}
			return false;
		}
		
		#endregion

		#region Images

		public override IDataReader GetImagesByCategoryID(int catID, bool activeOnly)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int, 4, DataHelper.CheckNull(catID)),
				DataHelper.MakeInParam("@IsActive",SqlDbType.Bit,1, activeOnly),
				BlogIdParam
			};

			return GetReader("subtext_GetImageCategory",p);
		}

		public override IDataReader GetImage(int imageID, bool activeOnly)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ImageID",SqlDbType.Int,4,imageID),
				DataHelper.MakeInParam("@IsActive",SqlDbType.Bit,1, activeOnly),
				BlogIdParam
			};
			return GetReader("subtext_GetSingleImage",p);
		}

		public override int InsertImage(Image image)
		{
			if (image == null)
				throw new ArgumentNullException("image", "Cannot insert a null image.");
			
			SqlParameter outParam = DataHelper.MakeOutParam("@ImageID", SqlDbType.Int, 4);
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 250, image.Title),
				DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(image.CategoryID)),
				DataHelper.MakeInParam("@Width", SqlDbType.Int, 4, image.Width),
				DataHelper.MakeInParam("@Height", SqlDbType.Int, 4, image.Height),
				DataHelper.MakeInParam("@File", SqlDbType.NVarChar, 50, image.FileName),
				DataHelper.MakeInParam("@Active", SqlDbType.Bit, 1, image.IsActive),
				BlogIdParam,
				outParam
			};
			NonQueryInt("subtext_InsertImage",p);
			return (int)outParam.Value;
		}

		public override bool UpdateImage(Image _image)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 250, _image.Title),
				DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(_image.CategoryID)),
				DataHelper.MakeInParam("@Width", SqlDbType.Int, 4, _image.Width),
				DataHelper.MakeInParam("@Height", SqlDbType.Int, 4, _image.Height),
				DataHelper.MakeInParam("@File", SqlDbType.NVarChar, 50, _image.FileName),
				DataHelper.MakeInParam("@Active", SqlDbType.Bit, 1, _image.IsActive),
				BlogIdParam,
				DataHelper.MakeInParam("@ImageID",SqlDbType.Int,4,_image.ImageID)
			};
			return NonQueryBool("subtext_UpdateImage",p);
		}

		public override bool DeleteImage(int imageID)
		{
			SqlParameter[] p = 
			{
				BlogIdParam,
				DataHelper.MakeInParam("@ImageID",SqlDbType.Int,4,imageID)
			};
			return NonQueryBool("subtext_DeleteImage",p);
		}

		#endregion

		#region Admin 

		/// <summary>
		/// Returns a list of all the blogs within the specified range.
		/// </summary>
		/// <param name="host">The hostname for this blog.</param>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="flags">Flags for type of retrieval.</param>
		/// <returns></returns>
		public override IDataReader GetPagedBlogs(string host, int pageIndex, int pageSize, ConfigurationFlags flags)
		{
			string sql = "subtext_GetPageableBlogs";

			SqlConnection conn = new SqlConnection(ConnectionString);
			
			SqlCommand command = new SqlCommand(sql, conn);

			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, host));
			command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(DataHelper.MakeInParam("@ConfigurationFlags", SqlDbType.Int, 4, flags));

			try
			{
				conn.Open();
				return command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			catch (SqlException)
			{
				conn.Open();
				//If we were upgrading, we need to call the old version of the stored proc.
				pageIndex++;
				command.Parameters.Clear();
				command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
				command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
				command.Parameters.Add(DataHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, 0));
				return command.ExecuteReader(CommandBehavior.CloseConnection);
			}
			
		}

		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public override IDataReader GetBlogById(int blogId)
		{
			return SqlHelper.ExecuteReader(ConnectionString, "subtext_GetBlogById", DataHelper.CheckNull(blogId));
		}

		public override IDataReader GetBlogByDomainAlias(string host, string subfolder, bool strict)
		{
			string sql = "subtext_GetBlogByDomainAlias";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);

			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(DataHelper.MakeInParam("@Host", SqlDbType.VarChar, 100, host));
			command.Parameters.Add(DataHelper.MakeInParam("@Application", SqlDbType.VarChar, 50, subfolder));
			command.Parameters.Add(DataHelper.MakeInParam("@Strict", SqlDbType.Bit, 1, strict));

			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection); 
		}

		public override IDataReader GetPagedBlogDomainAliases(int blogId, int pageIndex, int pageSize)
		{
			string sql = "subtext_GetPageableDomainAliases";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);

			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, blogId));
			command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));

			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public override IDataReader GetPagedLinks(int categoryId, int pageIndex, int pageSize, bool sortDescending)
		{
			bool useCategory = categoryId > -1;
			string sql = "subtext_GetPageableLinks";
			
			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql,conn);
			command.CommandType = CommandType.StoredProcedure;			
		
			command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(BlogIdParam);
		
			if (useCategory)
			{
				command.Parameters.Add(DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(categoryId)));
			}

			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
			
		}

		/// <summary>
		/// Returns a data reader (<see cref="IDataReader" />) pointing to all the blog entries 
		/// ordered by ID Descending for the specified page index (0-based) and page size.
		/// </summary>
		/// <param name="postType"></param>
		/// <param name="categoryID"></param>
		/// <param name="pageIndex"></param>
		/// <param name="pageSize"></param>
		/// <returns></returns>
		public override IDataReader GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize)
		{
			// default setup is for unfiltered pageable results
			bool useCategoryID = categoryID > 0;

			string sql = useCategoryID ? "subtext_GetPageableEntriesByCategoryID" : "subtext_GetPageableEntries";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql,conn);
			command.CommandType = CommandType.StoredProcedure;
						
			command.Parameters.Add(DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(DataHelper.MakeInParam("@PostType", SqlDbType.Int, 4, postType));
			command.Parameters.Add(BlogIdParam);
				
			if(useCategoryID)
			{
					command.Parameters.Add(DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(categoryID)));
			}
			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		
		public override IDataReader GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			SqlParameter[] p =
			{
				BlogIdParam,
				DataHelper.MakeInParam("@BeginDate", SqlDbType.DateTime, 4, beginDate),
				DataHelper.MakeInParam("@EndDate", SqlDbType.DateTime, 4, endDate),
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};
			return GetReader("subtext_GetPageableViewStats",p);
		}

		public override IDataReader GetPagedReferrers(int pageIndex, int pageSize, int entryId)
		{
			SqlParameter[] p =
			{
				BlogIdParam,
				DataHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, DataHelper.CheckNull(entryId)),
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};
			
			return GetReader("subtext_GetPageableReferrers", p);

		}

		/// <summary>
		/// Gets the paged feedback.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="status">The status.</param>
		/// <param name="excludeStatusMask">A flag for the statuses to exclude.</param>
		/// <param name="type">Feedback Type (comment, comment api, etc..) for the feedback to return.</param>
		/// <returns></returns>
		public override IDataReader GetPagedFeedback(int pageIndex, int pageSize, FeedbackStatusFlag status, FeedbackStatusFlag excludeStatusMask, FeedbackType type)
		{
			object feedbackType = type;
			if(type == FeedbackType.None)
				feedbackType = null;

			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
				DataHelper.MakeInParam("@StatusFlag", SqlDbType.Int, 4, status),
				DataHelper.MakeInParam("@FeedbackType", SqlDbType.Int, 4, feedbackType),
				DataHelper.MakeInParam("@ExcludeFeedbackStatusMask", SqlDbType.Int, 4, excludeStatusMask),
				BlogIdParam
			};
			return GetReader("subtext_GetPageableFeedback", p);

		}

		/// <summary>
		/// Gets the specified page of log entries.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <returns></returns>
		public override IDataReader GetPagedLogEntries(int pageIndex, int pageSize)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
				BlogIdParam
			};
			return GetReader("subtext_GetPageableLogEntries", p);
		}

		/// <summary>
		/// Clears the log.
		/// </summary>
		public override void ClearLog()
		{
			SqlParameter[] p =
			{
				BlogIdParam
			};
			NonQueryInt("subtext_LogClear", p);
		}

        public override bool ClearBlogContent()
        {
            SqlParameter[] p = {BlogIdParam};
            return NonQueryBool("subtext_ClearBlogContent", p);
        }

		#endregion	

		#region Get Blog Data
			
		/// <summary>
		/// Returns the specified number of blog entries
		/// </summary>
		/// <param name="itemCount"></param>
		/// <param name="postType"></param>
		/// <param name="postConfiguration"></param>
		/// <param name="includeCategories">Whether or not to include categories</param>
		/// <returns></returns>
		public override IDataReader GetConditionalEntries(int itemCount, PostType postType, PostConfig postConfiguration, bool includeCategories)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ItemCount", SqlDbType.Int, 4,itemCount),
				DataHelper.MakeInParam("@PostType", SqlDbType.Int, 4, postType),
				DataHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, postConfiguration),
				DataHelper.MakeInParam("@IncludeCategories", SqlDbType.Bit, 1, includeCategories),
				BlogIdParam				
			};

			return GetReader("subtext_GetConditionalEntries", p);
		}
				
		public override IDataReader GetEntriesByDateRange(DateTime start, DateTime stop, PostType postType, bool activeOnly)
		{
			SqlParameter[] p =	
			{
				DataHelper.MakeInParam("@StartDate",SqlDbType.DateTime, 8, start),
				DataHelper.MakeInParam("@StopDate",SqlDbType.DateTime, 8, stop),
				DataHelper.MakeInParam("@PostType",SqlDbType.Int, 4, postType),
				DataHelper.MakeInParam("@IsActive",SqlDbType.Bit,1, activeOnly),
				BlogIdParam
			};
			return GetReader("subtext_GetEntriesByDayRange",p);
		}

		/// <summary>
		/// Returns a Data Reader pointing to the entry specified by the entry name.
		/// Only returns entries for the current blog (Config.CurrentBlog).
		/// </summary>
		/// <param name="entryName">Url friendly entry name.</param>
		/// <param name="activeOnly"></param>
		/// <param name="includeCategories"></param>
		/// <returns></returns>
		public override IDataReader GetEntryReader(string entryName, bool activeOnly, bool includeCategories)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, entryName),
				DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, activeOnly),
				DataHelper.MakeInParam("@IncludeCategories", SqlDbType.Bit, 1, includeCategories),
				BlogIdParam
			};
			return GetReader("subtext_GetSingleEntry", p);
		}

		/// <summary>
		/// Returns a Data Reader pointing to the entry specified by the entry id. 
		/// Only returns entries for the current blog (Config.CurrentBlog).
		/// </summary>
		/// <param name="id"></param>
		/// <param name="activeOnly"></param>
		/// <param name="includeCategories"></param>
		/// <returns></returns>
        public override IDataReader GetEntryReader(int id, bool activeOnly, bool includeCategories)
        {
            SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ID", SqlDbType.Int, 4, id),
				DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, activeOnly),
				DataHelper.MakeInParam("@IncludeCategories", SqlDbType.Bit, 1, includeCategories),
				BlogIdParam
			};
            return GetReader("subtext_GetSingleEntry", p);
        }

		/// <summary>
		/// Returns a Data Reader pointing to the active entry specified by the entry id no matter 
		/// which blog it belongs to.
		/// </summary>
		/// <param name="id"></param>
		/// <param name="includeCategories"></param>
		/// <returns></returns>
        public override IDataReader GetEntryReader(int id, bool includeCategories)
        {
            SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ID", SqlDbType.Int, 4, id),
				DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, true),
				DataHelper.MakeInParam("@IncludeCategories", SqlDbType.Bit, 1, includeCategories),
				DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, null)
			};
            return GetReader("subtext_GetSingleEntry", p);
        }

		/// <summary>
		/// Searches the data store for the first comment with a 
		/// matching checksum hash.
		/// </summary>
		/// <param name="checksumHash">Checksum hash.</param>
		/// <returns></returns>
		public override IDataReader GetCommentByChecksumHash(string checksumHash)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@FeedbackChecksumHash", SqlDbType.VarChar, 32, checksumHash),
				BlogIdParam
			};
			return GetReader("subtext_GetCommentByChecksumHash", p);
		}

		public override IDataReader GetEntryDayReader(DateTime dt)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Date",SqlDbType.DateTime,8,dt),
				BlogIdParam
			};
			return GetReader("subtext_GetSingleDay",p);
		}

        public override IDataReader GetEntriesByCategory(int itemCount, int catID, bool activeOnly)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ItemCount", SqlDbType.Int, 4, itemCount),
				DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(catID)),
				DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, activeOnly),
				BlogIdParam
			};
			return GetReader("subtext_GetPostsByCategoryID", p);
	
		}

        public override IDataReader GetEntriesByTag(int itemCount, string tagName)
        {
            SqlParameter[] p = 
                {
                    DataHelper.MakeInParam("@ItemCount", SqlDbType.Int, 4, itemCount),
                    DataHelper.MakeInParam("@Tag", SqlDbType.NVarChar, 256, tagName),
                    BlogIdParam
                };
            return GetReader("subtext_GetPostsByTag", p);
        }

		//Should power both EntryCollection and EntryDayCollection
		public override IDataReader GetPostCollectionByMonth(int month, int year)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Month",SqlDbType.Int,4,month),
				DataHelper.MakeInParam("@Year",SqlDbType.Int,4,year),
				BlogIdParam};
			return GetReader("subtext_GetPostsByMonth",p);
		}
	    #endregion

		#region Update Blog Data

		/// <summary>
		/// Deletes the entry from the database.
		/// </summary>
		/// <param name="PostID">The post ID.</param>
		/// <returns></returns>
		public override bool DeleteEntry(int PostID)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ID", SqlDbType.Int, 4, PostID)
			};
			return NonQueryBool("subtext_DeletePost", p);
		}
		
		/// <summary>
		/// Deletes the entry from the database.
		/// </summary>
		/// <param name="id">The post ID.</param>
		/// <returns></returns>
		public override void DestroyFeedback(int id)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, id),
			};
			NonQueryBool("subtext_DeleteFeedback", p);
		}

		/// <summary>
		/// Deletes the entry from the database.
		/// </summary>
		/// <param name="status">The post ID.</param>
		/// <returns></returns>
		public override void DestroyFeedback(FeedbackStatusFlag status)
		{
			SqlParameter[] p =
			{
				BlogIdParam
				, DataHelper.MakeInParam("@StatusFlag", SqlDbType.Int, 4, status),
			};
			NonQueryBool("subtext_DeleteFeedbackByStatus", p);
		}
		
	    /// <summary>
	    /// Saves the categories for the specified post.
	    /// </summary>
	    /// <param name="postId"></param>
	    /// <param name="categoryIds"></param>
	    /// <returns></returns>
		public override bool SetEntryCategoryList(int postId, int[] categoryIds)
		{
			if(categoryIds == null)
				return false;
			
			string[] cats = new string[categoryIds.Length];
			for(int i = 0; i< categoryIds.Length; i++)
			{
				cats[i] = categoryIds[i].ToString(CultureInfo.InvariantCulture);
			}
			string catList = string.Join(",", cats);

			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@PostID", SqlDbType.Int, 4, DataHelper.CheckNull(postId)),
				BlogIdParam,
				DataHelper.MakeInParam("@CategoryList", SqlDbType.NVarChar, 4000, catList)
			};
			return NonQueryBool("subtext_InsertLinkCategoryList", p);
		}

        /// <summary>
        /// Saves the tags for the specified post
        /// </summary>
        /// <param name="postId">The EntryId for the post to update</param>
        /// <param name="tags">
        /// An array of tag strings for the associated post. If there are no tags
        /// associated with the post, pass tags with length zero to remove post tags
        /// if present.
        /// </param>
        /// <returns></returns>
        public override bool SetEntryTagList(int postId, IEnumerable<string> tags)
        {
			if (tags == null)
				throw new ArgumentNullException("tags", "Tags cannot be null.");

            SqlParameter tagParam = new SqlParameter("@TagList", SqlDbType.NText);

            string tagList = "";
            foreach (string tag in tags)
            {
                tagList += tag + ",";
            }
            if (tagList.Length > 0)
                tagList = tagList.Substring(0, tagList.Length - 1);

            tagParam.Value = tagList;

            SqlParameter[] p =
                {
                    DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, DataHelper.CheckNull(postId)),
                    BlogIdParam,
                    tagParam
                };
            return NonQueryBool("subtext_InsertEntryTagList", p);
        }

        /// <summary>
		/// Adds a new entry to the blog.  Whether the entry be a blog post, article,
		/// </summary>
		/// <remarks>
        /// The method <see cref="SetEntryCategoryList" /> is used to save the entry's categories.
		/// </remarks>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public override int InsertEntry(Entry entry)
		{
			if (entry == null)
				throw new ArgumentNullException("entry", "Cannot insert a null entry.");
        	
			SqlParameter outIdParam = DataHelper.MakeOutParam("@ID", SqlDbType.Int, 4);
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
				DataHelper.MakeInParam("@Text", SqlDbType.NText, 0, entry.Body), 
				DataHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
				DataHelper.MakeInParam("@Author", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Author)), 
				DataHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Email)), 
				DataHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, DataHelper.CheckNull(entry.Description)), 
				DataHelper.MakeInParam("@DateAdded", SqlDbType.DateTime, 8, entry.DateCreated), 
				DataHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
				DataHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, StringHelper.ReturnNullForEmpty(entry.EntryName)), 
				DataHelper.MakeInParam("@DateSyndicated", SqlDbType.DateTime, 8, DataHelper.CheckNull(entry.DateSyndicated)), 
				BlogIdParam,
				outIdParam
			};

			NonQueryInt("subtext_InsertEntry", p);
			return (int)outIdParam.Value;
		}

		/// <summary>
		/// Updates an existing feedback.
		/// </summary>
		/// <param name="feedbackItem"></param>
		/// <returns></returns>
		public override bool UpdateFeedback(FeedbackItem feedbackItem)
		{
			string sourceUrl = null;
			if (feedbackItem.SourceUrl != null)
				sourceUrl = feedbackItem.SourceUrl.ToString();
				
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Id",  SqlDbType.Int, 4, feedbackItem.Id), 
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 256, feedbackItem.Title), 
				DataHelper.MakeInParam("@Body", DataHelper.CheckNull(feedbackItem.Body)), 
				DataHelper.MakeInParam("@Author", SqlDbType.NVarChar, 64, DataHelper.CheckNull(feedbackItem.Author)), 
				DataHelper.MakeInParam("@Email", SqlDbType.VarChar, 128, DataHelper.CheckNull(feedbackItem.Email)), 
				DataHelper.MakeInParam("@Url", SqlDbType.VarChar, 256, sourceUrl), 
				DataHelper.MakeInParam("@StatusFlag", SqlDbType.Int, 4, (int)feedbackItem.Status),
				DataHelper.MakeInParam("@FeedbackChecksumHash", SqlDbType.VarChar, 32, feedbackItem.ChecksumHash), 
				DataHelper.MakeInParam("@DateModified", SqlDbType.DateTime, 8, Config.CurrentBlog.TimeZone.Now),
			};
			return NonQueryBool("subtext_UpdateFeedback", p);
		}
		
        /// <summary>
		/// Updates the specified entry in the database.
		/// </summary>
        /// <remarks>
        /// The method <see cref="SetEntryCategoryList" /> is used to save the entry's categories.
        /// </remarks>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public override bool UpdateEntry(Entry entry)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@ID", SqlDbType.Int, 4, entry.Id), 
				DataHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
				DataHelper.MakeInParam("@Text", SqlDbType.NText, 0, entry.Body), 
				DataHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
				DataHelper.MakeInParam("@Author", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Author)), 
				DataHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Email)), 
				DataHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, DataHelper.CheckNull(entry.Description)), 
				DataHelper.MakeInParam("@DateUpdated", SqlDbType.DateTime, 4, entry.DateModified), 
				DataHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
				DataHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, DataHelper.CheckNull(entry.EntryName)), 
				DataHelper.MakeInParam("@DateSyndicated", SqlDbType.DateTime, 8, DataHelper.CheckNull(entry.DateSyndicated)), 
				BlogIdParam
			};
			return NonQueryBool("subtext_UpdateEntry", p);
		}

		/// <summary>
		/// Adds comment or a ping/trackback for an entry in the database.
		/// </summary>
		/// <param name="feedbackItem"></param>
		/// <returns></returns>
		public override int InsertFeedback(FeedbackItem feedbackItem)
		{
			if (feedbackItem == null)
				throw new ArgumentNullException("feedbackItem", "Cannot insert a null feedback item.");

			string ipAddress = null;
			if (feedbackItem.IpAddress != null)
				ipAddress = feedbackItem.IpAddress.ToString();

			string sourceUrl = null;
			if (feedbackItem.SourceUrl != null)
				sourceUrl = feedbackItem.SourceUrl.ToString();
			
			SqlParameter outParam = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 256, feedbackItem.Title), 
				DataHelper.MakeInParam("@Body", DataHelper.CheckNull(feedbackItem.Body)), 
				DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, DataHelper.CheckNull(feedbackItem.EntryId)),
				DataHelper.MakeInParam("@Author", SqlDbType.NVarChar, 64, DataHelper.CheckNull(feedbackItem.Author)), 
				DataHelper.MakeInParam("@IsBlogAuthor", SqlDbType.Bit, 1, feedbackItem.IsBlogAuthor),
				DataHelper.MakeInParam("@Email", SqlDbType.VarChar, 128, DataHelper.CheckNull(feedbackItem.Email)), 
				DataHelper.MakeInParam("@Url", SqlDbType.VarChar, 256, sourceUrl), 
				DataHelper.MakeInParam("@FeedbackType", SqlDbType.Int, 4, (int)feedbackItem.FeedbackType),
				DataHelper.MakeInParam("@StatusFlag", SqlDbType.Int, 4, (int)feedbackItem.Status),
				DataHelper.MakeInParam("@CommentAPI", SqlDbType.Bit, 1, feedbackItem.CreatedViaCommentAPI),
				DataHelper.MakeInParam("@Referrer", SqlDbType.NVarChar, 256, feedbackItem.Referrer), 
				DataHelper.MakeInParam("@IpAddress", SqlDbType.VarChar, 16, ipAddress),
				DataHelper.MakeInParam("@UserAgent", SqlDbType.NVarChar, 128, feedbackItem.UserAgent), 
				DataHelper.MakeInParam("@FeedbackChecksumHash", SqlDbType.VarChar, 32, feedbackItem.ChecksumHash), 
				DataHelper.MakeInParam("@DateCreated", SqlDbType.DateTime, 8, feedbackItem.DateCreated),
				DataHelper.MakeInParam("@DateModified", SqlDbType.DateTime, 8, feedbackItem.DateModified),
				BlogIdParam,
				outParam
			};

			NonQueryInt("subtext_InsertFeedback", p);
			return (int)outParam.Value;
		}


		#endregion

		#region Links

		public override IDataReader GetLinkCollectionByPostID(int postId)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@PostID", SqlDbType.Int, 4, DataHelper.CheckNull(postId)),
				BlogIdParam
			};
			return GetReader("subtext_GetLinkCollectionByPostID", p);
		}

	    public override bool DeleteLink(int linkId)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@LinkID", SqlDbType.Int, 4, linkId),
				BlogIdParam
			};
			return NonQueryBool("subtext_DeleteLink",p);

		}

		public override IDataReader GetLinkReader(int linkID)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@LinkID",SqlDbType.Int,4,linkID),
				BlogIdParam
			};
			return GetReader("subtext_GetSingleLink", p);
		}

		public override int InsertLink(Link link)
		{
			if (link == null)
				throw new ArgumentNullException("link", "Cannot insert a null link.");
			
			SqlParameter outParam = DataHelper.MakeOutParam("@LinkID", SqlDbType.Int, 4);
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 150, link.Title),
				DataHelper.MakeInParam("@Url", SqlDbType.NVarChar, 255, link.Url),
				DataHelper.MakeInParam("@Rss", SqlDbType.NVarChar, 255, DataHelper.CheckNull(link.Rss)),
				DataHelper.MakeInParam("@Active", SqlDbType.Bit, 1, link.IsActive),
				DataHelper.MakeInParam("@NewWindow", SqlDbType.Bit, 1, link.NewWindow),
				DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, DataHelper.CheckNull(link.CategoryID)),
                DataHelper.MakeInParam("@Rel", SqlDbType.NVarChar, 100, link.Relation),
				DataHelper.MakeInParam("@PostID", SqlDbType.Int, 4, DataHelper.CheckNull(link.PostID)),
				BlogIdParam,
				outParam
			};
			NonQueryInt("subtext_InsertLink", p);
			link.Id = (int)outParam.Value;
            return link.Id;
		}

		public override bool UpdateLink(Link link)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,link.Title),
				DataHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,link.Url),
				DataHelper.MakeInParam("@Rss",SqlDbType.NVarChar,255,DataHelper.CheckNull(link.Rss)),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,link.IsActive),
				DataHelper.MakeInParam("@NewWindow",SqlDbType.Bit,1,link.NewWindow),
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(link.CategoryID)),
                 DataHelper.MakeInParam("@Rel",SqlDbType.NVarChar,100,link.Relation),
				DataHelper.MakeInParam("@LinkID",SqlDbType.Int,4,link.Id),
				BlogIdParam
			};
			return NonQueryBool("subtext_UpdateLink", p);
		}

		/// <summary>
		/// Returns a data reader with every category for the specified category type.
		/// </summary>
		/// <param name="catType"></param>
		/// <param name="activeOnly"></param>
		/// <returns></returns>
		public override IDataReader GetCategories(CategoryType catType, bool activeOnly)
		{
			SqlParameter[] p ={DataHelper.MakeInParam("@CategoryType", SqlDbType.TinyInt, 1, catType),
							  DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, activeOnly),
								  BlogIdParam};
			return GetReader("subtext_GetCategory", p);
		}

		//maps to blog_GetActiveCategoriesWithLinkCollection
		public override DataSet GetActiveCategories()
		{
			SqlParameter[] p ={BlogIdParam};
			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString, CommandType.StoredProcedure, "subtext_GetActiveCategoriesWithLinkCollection", p);

			DataRelation dl = new DataRelation("CategoryID", ds.Tables[0].Columns["CategoryID"], ds.Tables[1].Columns["CategoryID"], false);
			ds.Relations.Add(dl);

			return ds;
		}
		#endregion

		#region Categories


		public override bool DeleteCategory(int catId)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(catId)),
				BlogIdParam
			};
			return NonQueryBool("subtext_DeleteCategory",p);

		}

		/// <summary>
		/// Returns a data reader for the specified category id.
		/// </summary>
		/// <param name="categoryId"></param>
		/// <param name="isActive"></param>
		/// <returns></returns>
		public override IDataReader GetLinkCategory(int categoryId, bool isActive)
		{
			return GetLinkCategoryGeneric(categoryId, isActive);
		}

		/// <summary>
		/// Returns a data reader for the specified category name.
		/// </summary>
		/// <param name="categoryName"></param>
		/// <param name="isActive"></param>
		/// <returns></returns>
		public override IDataReader GetLinkCategory(string categoryName, bool isActive)
		{
			return GetLinkCategoryGeneric(categoryName, isActive);
		}

		/// <summary>
		/// Returns a data reader for the specified category. The Category Key should either 
		/// be an Int (category id) or a string (category name).
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="categoryKey"></param>
		/// <param name="isActive"></param>
		/// <returns></returns>
		private IDataReader GetLinkCategoryGeneric<T>(T categoryKey, bool isActive)
		{
			SqlParameter id = DataHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, null);
			SqlParameter name = DataHelper.MakeInParam("@CategoryName", SqlDbType.NVarChar, 150, null);

			if(typeof(T) == typeof(int))
				id.Value = categoryKey;
			if (typeof(T) == typeof(string))
				name.Value = categoryKey;

			SqlParameter[] p = 
			{
				id,
				name,
				DataHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, isActive),
				BlogIdParam
			};
			return GetReader("subtext_GetCategory", p);
		}

		public override bool UpdateCategory(LinkCategory lc)
		{
			SqlParameter[] p =
			{

				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,lc.Title),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,lc.IsActive),
				DataHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,DataHelper.CheckNull(lc.Id)),
				DataHelper.MakeInParam("@CategoryType",SqlDbType.TinyInt,1,lc.CategoryType),
				DataHelper.MakeInParam("@Description",SqlDbType.NVarChar,1000,DataHelper.CheckNull(lc.Description)),
				BlogIdParam
			};
			return NonQueryBool("subtext_UpdateCategory",p);
		}

		//maps to blog_LinkCategory
		public override int InsertCategory(LinkCategory category)
		{
			if (category == null)
				throw new ArgumentNullException("category", "Cannot insert a null category.");

			SqlParameter outParam = DataHelper.MakeOutParam("@CategoryID",SqlDbType.Int,4);
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 150, category.Title),
				DataHelper.MakeInParam("@Active", SqlDbType.Bit, 1, category.IsActive),
				DataHelper.MakeInParam("@CategoryType", SqlDbType.TinyInt, 1, category.CategoryType),
				DataHelper.MakeInParam("@Description", SqlDbType.NVarChar, 1000, DataHelper.CheckNull(category.Description)),
				BlogIdParam,
				outParam
			};
			NonQueryInt("subtext_InsertCategory",p);
			return (int)outParam.Value;
		}

		#endregion

        #region BlogGroups


        public override bool DeleteBlogGroup(int id)
        {
            SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Id",SqlDbType.Int,4,id)
			};
            return NonQueryBool("subtext_DeleteBlogGroup", p);

        }

        /// <summary>
        /// Returns a data reader for the specified blogGroup id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="activeOnly"></param>
        /// <returns></returns>
        public override IDataReader GetBlogGroup(int id, bool activeOnly)
        {
            SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, id),
				DataHelper.MakeInParam("@Active", SqlDbType.Bit, 1, activeOnly)
			};
            return GetReader("subtext_GetBlogGroup", p);
        }

        public override IDataReader ListBlogGroups(bool isActive)
        {
            SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Active", SqlDbType.Bit, 1, isActive)
			};
            return GetReader("subtext_ListBlogGroups", p);
        }

        public override IDataReader SetGroupActive(int id, bool isActive)
        {
            SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, id),
				DataHelper.MakeInParam("@Active", SqlDbType.Bit, 1, isActive)
			};
            return GetReader("subtext_SetGroupActive", p);
        }

        public override bool UpdateBlogGroup(int id, string title, bool isActive, int displayOrder, string description)
        {
            SqlParameter[] p =
			{

				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 150, title),
				DataHelper.MakeInParam("@Active", SqlDbType.Bit, 1, isActive),
				DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, id),
				DataHelper.MakeInParam("@DisplayOrder", SqlDbType.Int, 4, DataHelper.CheckNull(displayOrder)),
				DataHelper.MakeInParam("@Description", SqlDbType.NVarChar, 1000, DataHelper.CheckNull(description))
			};
            return NonQueryBool("subtext_UpdateBlogGroup", p);
        }


        public override int InsertBlogGroup(string title, bool isActive, int displayOrder, string description)
        {
            SqlParameter outParam = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
            SqlParameter[] p =
			{

				DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,title),
				DataHelper.MakeInParam("@Active",SqlDbType.Bit,1,isActive),
				DataHelper.MakeInParam("@DisplayOrder",SqlDbType.Int,4,DataHelper.CheckNull(displayOrder)),
				DataHelper.MakeInParam("@Description",SqlDbType.NVarChar,1000,DataHelper.CheckNull(description)),
				outParam
			};
            NonQueryInt("subtext_InsertBlogGroup", p);
            return (int)outParam.Value;
        }

        #endregion

		#region FeedBack

		/// <summary>
		/// Gets the feed back item by id.
		/// </summary>
		/// <param name="id">The id.</param>
		/// <returns></returns>
		public override IDataReader GetFeedBackItem(int id)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, id),
			};
			return GetReader("subtext_GetFeedBack", p);
		}

		/// <summary>
		/// Gets the feedback counts for the various top level statuses.
		/// </summary>
		/// <param name="approved">The approved.</param>
		/// <param name="needsModeration">The needs moderation.</param>
		/// <param name="flaggedAsSpam">The flagged as spam.</param>
		/// <param name="deleted">The deleted.</param>
		public override void GetFeedbackCounts(out int approved, out int needsModeration, out int flaggedAsSpam, out int deleted)
		{
			SqlParameter[] p =
			{
				BlogIdParam,
				DataHelper.MakeOutParam("@ApprovedCount", SqlDbType.Int, 4),
				DataHelper.MakeOutParam("@NeedsModerationCount", SqlDbType.Int, 4),
				DataHelper.MakeOutParam("@FlaggedSpam", SqlDbType.Int, 4),
				DataHelper.MakeOutParam("@Deleted", SqlDbType.Int, 4)
			};
			NonQueryBool("subtext_GetFeedbackCountsByStatus", p);

			approved = (int)p[1].Value;
			needsModeration = (int)p[2].Value;
			flaggedAsSpam = (int)p[3].Value;
			deleted = (int)p[4].Value;
		}

		//we could pass ParentID with the rest of the sprocs
		//one interface for entry data?
		public override IDataReader GetFeedBackItems(int postId)
		{
			SqlParameter[] p =
			{
				DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, DataHelper.CheckNull(postId))
			};
			return GetReader("subtext_GetFeedbackCollection", p);
		}

		#endregion

		#region Configuration

        /// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="title"></param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <param name="host"></param>
		/// <param name="subfolder"></param>
		/// <returns></returns>
        public override bool AddBlogConfiguration(string title, string userName, string password, string host, string subfolder)
        {
            return AddBlogConfiguration(title, userName, password, host, subfolder, 1);
        }

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for
		/// allowing a user with a freshly installed blog to immediately gain access
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="title">The title.</param>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <param name="host">The host.</param>
		/// <param name="subfolder">The subfolder.</param>
		/// <param name="blogGroupId">The blog group.</param>
		/// <returns></returns>
		public override bool AddBlogConfiguration(string title, string userName, string password, string host, string subfolder, int blogGroupId)
		{
			ConfigurationFlags flag = ConfigurationFlags.IsActive
			                         | ConfigurationFlags.CommentsEnabled
			                         | ConfigurationFlags.CompressSyndicatedFeed
									 | ConfigurationFlags.IsAggregated
									 | ConfigurationFlags.IsPasswordHashed
									 | ConfigurationFlags.AutoFriendlyUrlEnabled
									 | ConfigurationFlags.CommentNotificationEnabled
									 | ConfigurationFlags.RFC3229DeltaEncodingEnabled
									 | ConfigurationFlags.CaptchaEnabled;
			SqlParameter[] parameters = 
			{
				DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 100, title)
				, DataHelper.MakeInParam("@UserName", SqlDbType.NVarChar, 50, userName)
				, DataHelper.MakeInParam("@Password", SqlDbType.NVarChar, 50, password)
				, DataHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, string.Empty)
				, DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 50, host)
				, DataHelper.MakeInParam("@Application", SqlDbType.NVarChar, 50, subfolder)
				, DataHelper.MakeInParam("@Flag", SqlDbType.Int, 4, flag)
                , DataHelper.MakeInParam("@BlogGroupId", SqlDbType.Int, 4, blogGroupId)
			};
			return NonQueryBool("subtext_UTILITY_AddBlog", parameters);
		}

		/// <summary>
		/// Returns a <see cref="IDataReader"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <remarks>
		/// If there are no matches, but there is only one blog_config record, 
		/// this will return that record.
		/// </remarks>
		/// <param name="host">Hostname.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		/// <returns></returns>
		public override IDataReader GetBlogInfo(string host, string subfolder)
		{
			return GetBlogInfo(host, subfolder, false); //Not Strict.
		}

		/// <summary>
		/// Returns a <see cref="IDataReader"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <remarks>
		/// Until Subtext supports multiple blogs again (if ever), 
		/// this will always return the same instance.
		/// </remarks>
		/// <param name="host">Hostname.</param>
		/// <param name="subfolder">Subfolder Name.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the subfolder and hostname match.</param>
		/// <returns></returns>
		public override IDataReader GetBlogInfo(string host, string subfolder, bool strict)
		{
			SqlParameter[] p = 
			{
				DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, host)
				,DataHelper.MakeInParam("@Application", SqlDbType.NVarChar, 50, subfolder)
				,DataHelper.MakeInParam("@Strict", SqlDbType.Bit, 1, strict)
			};
			return GetReader("subtext_GetConfig", p);
		}

		/// <summary>
		/// Updates the blog configuration in the SQL database 
		/// using the "subtext_UpdateConfig" stored proc.
		/// </summary>
		/// <param name="info">Config.</param>
		/// <returns></returns>
		public override bool UpdateBlog(BlogInfo info)
		{
			object daysTillCommentsClose = null;
			if(info.DaysTillCommentsClose > -1 && info.DaysTillCommentsClose < int.MaxValue)
			{
				daysTillCommentsClose = info.DaysTillCommentsClose;
			}

			object commentDelayInMinutes = null;
			if(info.CommentDelayInMinutes > 0 && info.CommentDelayInMinutes < int.MaxValue)
			{
				commentDelayInMinutes = info.CommentDelayInMinutes;
			}

			object numberOfRecentComments = null;
			if(info.NumberOfRecentComments > 0 && info.NumberOfRecentComments < int.MaxValue)
			{
				numberOfRecentComments = info.NumberOfRecentComments;
			}

			object recentCommentsLength = null;
			if(info.RecentCommentsLength > 0 && info.RecentCommentsLength < int.MaxValue)
			{
				recentCommentsLength = info.RecentCommentsLength;
			}

			SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@BlogId", SqlDbType.Int,  4, DataHelper.CheckNull(info.Id))
					,DataHelper.MakeInParam("@UserName", SqlDbType.NVarChar, 50, info.UserName) 
					,DataHelper.MakeInParam("@Password", SqlDbType.NVarChar, 50, info.Password) 
					,DataHelper.MakeInParam("@Author", SqlDbType.NVarChar, 100, info.Author) 
					,DataHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, info.Email) 
					,DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 100, info.Title) 
					,DataHelper.MakeInParam("@SubTitle", SqlDbType.NVarChar, 250, info.SubTitle) 
					,DataHelper.MakeInParam("@Skin", SqlDbType.NVarChar, 50, info.Skin.TemplateFolder) 
					,DataHelper.MakeInParam("@Application", SqlDbType.NVarChar, 50, info.CleanSubfolder) 
					,DataHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, info.Host) 
					,DataHelper.MakeInParam("@TimeZone", SqlDbType.Int, 4, info.TimeZoneId) 
					,DataHelper.MakeInParam("@Language", SqlDbType.NVarChar, 10, info.Language) 
					,DataHelper.MakeInParam("@News", SqlDbType.NText, 0, DataHelper.CheckNull(info.News))
					,DataHelper.MakeInParam("@TrackingCode", SqlDbType.NText, 0, DataHelper.CheckNull(info.TrackingCode)) 
					,DataHelper.MakeInParam("@ItemCount", SqlDbType.Int,  4, info.ItemCount) 
					,DataHelper.MakeInParam("@CategoryListPostCount", SqlDbType.Int,  4, info.CategoryListPostCount) 
					,DataHelper.MakeInParam("@Flag", SqlDbType.Int,  4, (int)info.Flag) 
					,DataHelper.MakeInParam("@LastUpdated", SqlDbType.DateTime,  8, info.LastUpdated) 
					,DataHelper.MakeInParam("@SecondaryCss", SqlDbType.Text, 0, DataHelper.CheckNull(info.Skin.CustomCssText)) 
					,DataHelper.MakeInParam("@SkinCssFile", SqlDbType.VarChar, 100, DataHelper.CheckNull(info.Skin.SkinStyleSheet)) 
					,DataHelper.MakeInParam("@LicenseUrl", SqlDbType.NVarChar, 64, info.LicenseUrl)
					,DataHelper.MakeInParam("@DaysTillCommentsClose", SqlDbType.Int, 4, daysTillCommentsClose)
					,DataHelper.MakeInParam("@CommentDelayInMinutes", SqlDbType.Int, 4, commentDelayInMinutes)
					,DataHelper.MakeInParam("@NumberOfRecentComments", SqlDbType.Int, 4, numberOfRecentComments)
					,DataHelper.MakeInParam("@RecentCommentsLength", SqlDbType.Int, 4, recentCommentsLength)
					,DataHelper.MakeInParam("@AkismetAPIKey", SqlDbType.VarChar, 16, DataHelper.ReturnNullIfEmpty(info.FeedbackSpamServiceKey))
					,DataHelper.MakeInParam("@FeedBurnerName", SqlDbType.NVarChar, 64, DataHelper.ReturnNullIfEmpty(info.FeedBurnerName))
					,DataHelper.MakeInParam("@BlogGroupId", SqlDbType.Int, 4, info.BlogGroupId)
                    ,DataHelper.MakeInParam("@MobileSkin", SqlDbType.NVarChar, 50, DataHelper.CheckNull(info.MobileSkin.TemplateFolder)) 
                    ,DataHelper.MakeInParam("@MobileSkinCssFile", SqlDbType.NVarChar, 50, DataHelper.CheckNull(info.MobileSkin.SkinStyleSheet)) 
                    ,DataHelper.MakeInParam("@OpenIDUrl", SqlDbType.VarChar, 255, info.OpenIDUrl) 
                    ,DataHelper.MakeInParam("@CardSpaceHash", SqlDbType.NVarChar, 512, info.CardSpaceHash) 
                    ,DataHelper.MakeInParam("@OpenIDServer", SqlDbType.VarChar, 255, info.OpenIDServer) 
                    ,DataHelper.MakeInParam("@OpenIDDelegate", SqlDbType.VarChar, 255, info.OpenIDDelegate)                     
				};

			return NonQueryBool("subtext_UpdateConfig", p);
		}

		#endregion
																		
		#region BlogAlias
		public override IDataReader GetBlogAliasById(int aliasId)
		{
			return GetReader("subtext_GetDomainAliasById", new SqlParameter[] { DataHelper.MakeInParam("@Id", aliasId)});
		}
		public override bool AddBlogAlias(BlogAlias alias)
		{
			SqlParameter aliasId = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);
			SqlParameter[] p = 
				{
					 aliasId
					,DataHelper.MakeInParam("@BlogId",SqlDbType.Int,4,alias.BlogId)
					,DataHelper.MakeInParam("@Host",SqlDbType.NVarChar,100,alias.Host)
					,DataHelper.MakeInParam("@Application",SqlDbType.NVarChar,50,alias.Subfolder)
					,DataHelper.MakeInParam("@Active",SqlDbType.Bit,100,alias.IsActive)
				};
			bool retValue = NonQueryBool("subtext_CreateDomainAlias", p);
			if(aliasId.Value!=DBNull.Value)
				alias.Id = (int)aliasId.Value;
			return retValue;
		}

		public override bool DeleteBlogAlias(BlogAlias alias)
		{
			SqlParameter[] p = { DataHelper.MakeInParam("@Id",SqlDbType.Int,4,alias.Id)
			};

			return NonQueryBool("subtext_DeleteDomainAlias", p);
		}

		public override bool UpdateBlogAlias(BlogAlias alias)
		{
			SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@Id",SqlDbType.Int,4,alias.Id)
					,DataHelper.MakeInParam("@BlogId",SqlDbType.Int,4,alias.BlogId)
					,DataHelper.MakeInParam("@Host",SqlDbType.NVarChar,100,alias.Host)
					,DataHelper.MakeInParam("@Application",SqlDbType.NVarChar,50,alias.Subfolder)
					,DataHelper.MakeInParam("@Active",SqlDbType.Bit,100,alias.IsActive)
				};

			return NonQueryBool("subtext_UpdateDomainAlias", p);
		}
		#endregion
		
        #region Archives

		public override IDataReader GetPostsByMonthArchive()
		{
			SqlParameter[] p = {BlogIdParam};
			return GetReader("subtext_GetPostsByMonthArchive", p);
		}

		public override IDataReader GetPostsByYearArchive()
		{
			SqlParameter[] p = {BlogIdParam};
			return GetReader("subtext_GetPostsByYearArchive", p);
		}

        public override IDataReader GetPostsByCategoryArchive()
        {
            SqlParameter[] p = { BlogIdParam };
            return GetReader("subtext_GetPostsByCategoriesArchive", p);
        }

		#endregion

        #region Tags

        public override IDataReader GetTopTags(int ItemCount)
        {
            SqlParameter[] p = 
                {
                    DataHelper.MakeInParam("@ItemCount", SqlDbType.Int, 4, ItemCount),
                    BlogIdParam
                };
            return GetReader("subtext_GetTopTags", p);
        }

        #endregion

		#region Enclosures

        public override int InsertEnclosure(Enclosure enclosure)
        {
            if (enclosure == null)
                throw new ArgumentNullException("enclosure", "Cannon insert a null metaTag");

            SqlParameter id = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);

            SqlParameter[] p =
                {
                    DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 256, DataHelper.CheckNull(enclosure.Title)),
                    DataHelper.MakeInParam("@Url", SqlDbType.NVarChar, 256, enclosure.Url),
                    DataHelper.MakeInParam("@MimeType", SqlDbType.NVarChar, 50, enclosure.MimeType),
                    DataHelper.MakeInParam("@Size", SqlDbType.BigInt, 8, enclosure.Size),
                    DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, enclosure.EntryId),
                    DataHelper.MakeInParam("@ShowWithPost", SqlDbType.Bit, 1, enclosure.ShowWithPost),
                    DataHelper.MakeInParam("@AddToFeed", SqlDbType.Bit, 1, enclosure.AddToFeed),
                    id
                };
            NonQueryInt("subtext_InsertEnclosure", p);

            return (int)id.Value;
        }

	    public override bool UpdateEnclosure(Enclosure enclosure)
	    {
            if (enclosure == null)
                throw new ArgumentNullException("enclosure", "Cannon update a null metaTag");

            SqlParameter[] p =
                {
                    DataHelper.MakeInParam("@Title", SqlDbType.NVarChar, 256, DataHelper.CheckNull(enclosure.Title)),
                    DataHelper.MakeInParam("@Url", SqlDbType.NVarChar, 256, enclosure.Url),
                    DataHelper.MakeInParam("@MimeType", SqlDbType.NVarChar, 50, enclosure.MimeType),
                    DataHelper.MakeInParam("@Size", SqlDbType.BigInt, 8, enclosure.Size),
                    DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, enclosure.EntryId),
                    DataHelper.MakeInParam("@ShowWithPost", SqlDbType.Bit, 1, enclosure.ShowWithPost),
                    DataHelper.MakeInParam("@AddToFeed", SqlDbType.Bit, 1, enclosure.AddToFeed),
                    DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, enclosure.Id),
                };
            return NonQueryBool("subtext_UpdateEnclosure", p);
	    }

	    public override bool DeleteEnclosure (int enclosureId)
	    {
            SqlParameter[] p =
            {
                DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, enclosureId)
            };
            return NonQueryBool("subtext_DeleteEnclosure", p);
	    }

	    #endregion

        #region MetaTags

        public override int InsertMetaTag(MetaTag metaTag)
        {
            if (metaTag == null)
                throw new ArgumentNullException("metaTag", "Cannon insert a null metaTag");

            object entryIdValue = metaTag.EntryId.HasValue ? DataHelper.CheckNull(metaTag.EntryId.Value) : DBNull.Value;
            SqlParameter id = DataHelper.MakeOutParam("@Id", SqlDbType.Int, 4);

            SqlParameter[] p =
                {
                    DataHelper.MakeInParam("@Content", SqlDbType.NVarChar, 512, metaTag.Content),
                    DataHelper.MakeInParam("@Name", SqlDbType.NVarChar, 100, DataHelper.CheckNull(metaTag.Name)),
                    DataHelper.MakeInParam("@HttpEquiv", SqlDbType.NVarChar, 100, DataHelper.CheckNull(metaTag.HttpEquiv)),
                    DataHelper.MakeInParam("@DateCreated", SqlDbType.DateTime, 8, metaTag.DateCreated),
                    DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, metaTag.BlogId),
                    DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, entryIdValue),
                    id
                };
            NonQueryInt("subtext_InsertMetaTag", p);

            return (int)id.Value;
        }

        public override bool UpdateMetaTag(MetaTag metaTag)
        {
            if (metaTag == null)
                throw new ArgumentNullException("metaTag", "Cannon update a null metaTag");

            object entryIdValue = metaTag.EntryId.HasValue ? DataHelper.CheckNull(metaTag.EntryId.Value) : DBNull.Value;

            SqlParameter[] p =
                {
                    DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, metaTag.Id),
                    DataHelper.MakeInParam("@Content", SqlDbType.NVarChar, 512, metaTag.Content),
                    DataHelper.MakeInParam("@Name", SqlDbType.NVarChar, 100, DataHelper.CheckNull(metaTag.Name)),
                    DataHelper.MakeInParam("@HttpEquiv", SqlDbType.NVarChar, 100,
                                           DataHelper.CheckNull(metaTag.HttpEquiv)),
                    DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, metaTag.BlogId),
                    DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, entryIdValue)
                };
            return NonQueryBool("subtext_UpdateMetaTag", p);
        }

        public override IDataReader GetMetaTagsForBlog(BlogInfo blog)
        {
            SqlParameter[] p =
				{
					DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, DataHelper.CheckNull(blog.Id))
				};
            return GetReader("subtext_GetMetaTagsForBlog", p);
        }

        public override IDataReader GetMetaTagsForEntry(Entry entry)
        {
            SqlParameter[] p =
				{
					DataHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, DataHelper.CheckNull(entry.BlogId)),
                    DataHelper.MakeInParam("@EntryId", SqlDbType.Int, 4, DataHelper.CheckNull(entry.Id))
				};
            return GetReader("subtext_GetMetaTagsForEntry", p);
        }

        public override bool DeleteMetaTag(int metaTagId)
        {
            SqlParameter[] p =
			{
				DataHelper.MakeInParam("@Id", SqlDbType.Int, 4, metaTagId)
			};
            return NonQueryBool("subtext_DeleteMetaTag", p);
        }

        #endregion



		#region KeyWords

		public override IDataReader GetKeyWord(int keyWordID)
		{
			SqlParameter[] p =
				{
					DataHelper.MakeInParam("@KeyWordID", SqlDbType.Int, 4, keyWordID),
					BlogIdParam
				};
			return GetReader("subtext_GetKeyWord",p);
		}

		public override bool DeleteKeyWord(int keywordId)
		{
			SqlParameter[] p =
				{
					DataHelper.MakeInParam("@KeyWordID", SqlDbType.Int, 4, keywordId),
					BlogIdParam
				};
			return NonQueryBool("subtext_DeleteKeyWord",p);
		}

		public override int InsertKeyWord(KeyWord keyword)
		{
			if (keyword == null)
				throw new ArgumentNullException("keyword", "Cannot insert a null keyword.");
        	
			SqlParameter outParam = DataHelper.MakeOutParam("@KeyWordID",SqlDbType.Int,4);
			SqlParameter[] p =
				{
					DataHelper.MakeInParam("@Word",SqlDbType.NVarChar,100,keyword.Word),
					DataHelper.MakeInParam("@Rel",SqlDbType.NVarChar,100,keyword.Rel),
					DataHelper.MakeInParam("@Text",SqlDbType.NVarChar,100,keyword.Text),
					DataHelper.MakeInParam("@ReplaceFirstTimeOnly",SqlDbType.Bit,1,keyword.ReplaceFirstTimeOnly),
					DataHelper.MakeInParam("@OpenInNewWindow",SqlDbType.Bit,1,keyword.OpenInNewWindow),
					DataHelper.MakeInParam("@CaseSensitive",SqlDbType.Bit,1,keyword.CaseSensitive),
					DataHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,keyword.Url),
					DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,100,keyword.Title),
					BlogIdParam,
					outParam
				};
			NonQueryInt("subtext_InsertKeyWord",p);
			return (int)outParam.Value;
		}

		public override bool UpdateKeyWord(KeyWord kw)
		{
			SqlParameter[] p =
				{
					DataHelper.MakeInParam("@KeyWordID",SqlDbType.Int,4,kw.Id),
					DataHelper.MakeInParam("@Word",SqlDbType.NVarChar,100,kw.Word),
					DataHelper.MakeInParam("@Rel",SqlDbType.NVarChar,100,kw.Rel),
					DataHelper.MakeInParam("@Text",SqlDbType.NVarChar,100,kw.Text),
					DataHelper.MakeInParam("@ReplaceFirstTimeOnly",SqlDbType.Bit,1,kw.ReplaceFirstTimeOnly),
					DataHelper.MakeInParam("@OpenInNewWindow",SqlDbType.Bit,1,kw.OpenInNewWindow),
					DataHelper.MakeInParam("@CaseSensitive",SqlDbType.Bit,1,kw.CaseSensitive),
					DataHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,kw.Url),
					DataHelper.MakeInParam("@Title",SqlDbType.NVarChar,100,kw.Title),
					BlogIdParam
				};
			return NonQueryBool("subtext_UpdateKeyWord",p);
		}

		public override IDataReader GetKeyWords()
		{
			SqlParameter[] p =
				{
					BlogIdParam
				};
			return GetReader("subtext_GetBlogKeyWords", p);
		}

		public override IDataReader GetPagedKeyWords(int pageIndex, int pageSize)
		{
			SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
					DataHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
					BlogIdParam
				};
			return GetReader("subtext_GetPageableKeyWords",p);
		}


		#endregion

		#region Helpers

		private IDataReader GetReader(string sql)
		{
			LogSql(sql, null); 
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, sql);
		}

		private IDataReader GetReader(string sql, SqlParameter[] p)
		{
			LogSql(sql, p); 
			return SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, sql, p);
		}

		private int NonQueryInt(string sql, SqlParameter[] p)
		{
			LogSql(sql, p);
			return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, sql, p);
		}

		private bool NonQueryBool(string sql, SqlParameter[] p)
		{
			LogSql(sql, p);
			return NonQueryInt(sql, p) > 0;
		}

		#endregion

		#region Host Data

		/// <summary>
		/// Returns the data for the Host.
		/// </summary>
		public override IDataReader GetHost()
		{
			return GetReader("subtext_GetHost");
		}

		/// <summary>
		/// Updates the <see cref="HostInfo"/> instance.  If the host record is not in the 
		/// database, one is created. There should only be one host record.
		/// </summary>
		/// <param name="host">The host information.</param>
		public override bool UpdateHost(HostInfo host)
		{
			SqlParameter[] p = 
				{
					DataHelper.MakeInParam("@HostUserName", SqlDbType.NVarChar,  64, host.HostUserName)
					, DataHelper.MakeInParam("@Password", SqlDbType.NVarChar,  32, host.Password)
					, DataHelper.MakeInParam("@Salt", SqlDbType.NVarChar,  32, host.Salt)
				};

			return NonQueryBool("subtext_UpdateHost", p);
		}
		#endregion Host Data

		#endregion

		static void LogSql(string sql, SqlParameter[] parameters)
		{
#if DEBUG
			string query = sql;
			if (parameters != null)
			{
				foreach (SqlParameter parameter in parameters)
				{
					query += " " + parameter.ParameterName + "=" + parameter.Value + ",";
				}
				if (query.EndsWith(","))
					query = StringHelper.Left(query, query.Length - 1);
			}

			Log.Debug("SQL: " + query);
#endif
		}
	}
}







