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
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using Subtext.Extensibility;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
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
		private SqlParameter BlogIDParam
		{
			get
			{
				return  SqlHelper.MakeInParam("@BlogID", SqlDbType.Int, 4, SqlHelper.CheckNull(Config.CurrentBlog.BlogID));
			}
		}

		#region DbProvider Methods
		#region Statistics

		public override bool TrackEntry(EntryViewCollection evc)
		{
			if(evc != null)
			{
				SqlConnection conn = new SqlConnection(this.ConnectionString);
				try
				{	
					foreach(EntryView ev in evc)
					{
						TrackEntry(ev);
					}
					return true;
				
				}
				finally
				{
					if(conn.State == ConnectionState.Open)
					{
						conn.Close();
					}
				}
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
						SqlHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, SqlHelper.CheckNull(ev.EntryID)),
						SqlHelper.MakeInParam("@BlogID", SqlDbType.Int, 4, SqlHelper.CheckNull(ev.BlogID)),
						SqlHelper.MakeInParam("@URL", SqlDbType.NVarChar, 255, ev.ReferralUrl),
						SqlHelper.MakeInParam("@IsWeb", SqlDbType.Bit,1, ev.PageViewType)
			};
			return this.NonQueryBool("subtext_TrackEntry",p);
		}
		
		#endregion

		#region Images

		public override IDataReader GetImagesByCategoryID(int catID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int, 4, SqlHelper.CheckNull(catID)),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};

			return GetReader("subtext_GetImageCategory",p);
		}

		public override IDataReader GetSingleImage(int imageID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ImageID",SqlDbType.Int,4,imageID),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetSingleImage",p);
		}

		public override int InsertImage(Image _image)
		{
			SqlParameter outParam = SqlHelper.MakeOutParam("@ImageID", SqlDbType.Int, 4);
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,250,_image.Title),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,SqlHelper.CheckNull(_image.CategoryID)),
				SqlHelper.MakeInParam("@Width",SqlDbType.Int,4,_image.Width),
				SqlHelper.MakeInParam("@Height",SqlDbType.Int,4,_image.Height),
				SqlHelper.MakeInParam("@File",SqlDbType.NVarChar,50,_image.File),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,_image.IsActive),
				BlogIDParam,
				outParam
			};
			NonQueryInt("subtext_InsertImage",p);
			return (int)outParam.Value;
		}

		public override bool UpdateImage(Image _image)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,250,_image.Title),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,SqlHelper.CheckNull(_image.CategoryID)),
				SqlHelper.MakeInParam("@Width",SqlDbType.Int,4,_image.Width),
				SqlHelper.MakeInParam("@Height",SqlDbType.Int,4,_image.Height),
				SqlHelper.MakeInParam("@File",SqlDbType.NVarChar,50,_image.File),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,_image.IsActive),
				BlogIDParam,
				SqlHelper.MakeInParam("@ImageID",SqlDbType.Int,4,_image.ImageID)
			};
			return NonQueryBool("subtext_UpdateImage",p);
		}

		public override bool DeleteImage(int imageID)
		{
			SqlParameter[] p = 
			{
				BlogIDParam,
				SqlHelper.MakeInParam("@ImageID",SqlDbType.Int,4,imageID)
			};
			return NonQueryBool("subtext_DeleteImage",p);
		}

		#endregion

		#region Admin 

		/// <summary>
		/// Returns a list of all the blogs within the specified range.
		/// </summary>
		/// <param name="pageIndex">Page index.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDescending">Sort descending.</param>
		/// <returns></returns>
		public override IDataReader GetPagedBlogs(int pageIndex, int pageSize, bool sortDescending)
		{
			string sql = "subtext_GetPageableBlogs";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);
			
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(SqlHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, sortDescending));

			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Gets the blog by id.
		/// </summary>
		/// <param name="blogId">Blog id.</param>
		/// <returns></returns>
		public override IDataReader GetBlogById(int blogId)
		{
			string sql = "subtext_GetBlogById";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);
			
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(SqlHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, SqlHelper.CheckNull(blogId)));

			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		/// <summary>
		/// Returns an instance of <see cref="IDataReader"/> used to 
		/// iterate through a result set containing blog_config rows 
		/// with the specified host.
		/// </summary>
		/// <param name="host">Host.</param>
		/// <returns></returns>
		public override IDataReader GetBlogsByHost(string host)
		{
			string sql = "subtext_GetBlogsByHost";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);
			
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(SqlHelper.MakeInParam("@host", SqlDbType.NVarChar, 100, host));

			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		public override IDataReader GetPagedLinks(int CategoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			bool useCategory = CategoryID > -1;
			string sql = useCategory ? "subtext_GetPageableLinksByCategoryID" : "subtext_GetPageableLinks";
			
			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql,conn);
			command.CommandType = CommandType.StoredProcedure;			
		
			command.Parameters.Add(SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(SqlHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, sortDescending));
			command.Parameters.Add(BlogIDParam);
		
			if (useCategory)
			{
				command.Parameters.Add(SqlHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, SqlHelper.CheckNull(CategoryID)));
			}

			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
			
		}
		public override IDataReader GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			// default setup is for unfiltered pageable results
			bool useCategoryID = categoryID > -1;

			string sql = useCategoryID ? "subtext_GetPageableEntriesByCategoryID" : "subtext_GetPageableEntries";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql,conn);
			command.CommandType = CommandType.StoredProcedure;
						
			command.Parameters.Add(SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(SqlHelper.MakeInParam("@PostType", SqlDbType.Int, 4, postType));
			command.Parameters.Add(SqlHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, sortDescending));
			command.Parameters.Add(BlogIDParam);
				
			if(useCategoryID)
			{
					command.Parameters.Add(SqlHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, SqlHelper.CheckNull(categoryID)));
			}
			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
		}

		
		public override IDataReader GetPagedViewStats(int pageIndex, int pageSize, DateTime beginDate, DateTime endDate)
		{
			SqlParameter[] p =
			{
				BlogIDParam,
				SqlHelper.MakeInParam("@BeginDate", SqlDbType.DateTime, 4, beginDate),
				SqlHelper.MakeInParam("@EndDate", SqlDbType.DateTime, 4, endDate),
				SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};
			return GetReader("subtext_GetPageableViewStats",p);
		}

		public override IDataReader GetPagedReferrers(int pageIndex, int pageSize)
		{
			SqlParameter[] p =
			{
				BlogIDParam,
				SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};
			return GetReader("subtext_GetPageableReferrers",p);

		}

		public override IDataReader GetPagedReferrers(int pageIndex, int pageSize, int EntryID)
		{
			SqlParameter[] p =
			{
				BlogIDParam,
				SqlHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, SqlHelper.CheckNull(EntryID)),
				SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};
			
			
			return GetReader("subtext_GetPageableReferrersByEntryID",p);

		}
		
		//Did not really experiment why, but sqlhelper does not seem to like the output parameter after the reader
		public override IDataReader GetPagedFeedback(int pageIndex, int pageSize, bool sortDescending)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
				SqlHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, sortDescending),
				BlogIDParam
			};
			return GetReader("subtext_GetPageableFeedback", p);

		}

		/// <summary>
		/// Gets the specified page of log entries.
		/// </summary>
		/// <param name="pageIndex">Index of the page.</param>
		/// <param name="pageSize">Size of the page.</param>
		/// <param name="sortDirection">The sort direction.</param>
		/// <returns></returns>
		public override IDataReader GetPagedLogEntries(int pageIndex, int pageSize, SortDirection sortDirection)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
				SqlHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, sortDirection == SortDirection.Descending),
				BlogIDParam
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
				BlogIDParam
			};
			this.NonQueryInt("subtext_LogClear", p);
		}


		#endregion	

		#region Get Blog Data
			
		public override IDataReader GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount", SqlDbType.Int, 4,ItemCount),
				SqlHelper.MakeInParam("@PostType", SqlDbType.Int, 4,pt),
				SqlHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4,pc),
				BlogIDParam				
			};

			return GetReader("subtext_GetConditionalEntries",p);
		}
				
		public override IDataReader GetEntriesByDateRangle(DateTime start, DateTime stop, PostType postType, bool ActiveOnly)
		{
			SqlParameter[] p =	
			{
				SqlHelper.MakeInParam("@StartDate",SqlDbType.DateTime, 8, start),
				SqlHelper.MakeInParam("@StopDate",SqlDbType.DateTime, 8, stop),
				SqlHelper.MakeInParam("@PostType",SqlDbType.Int, 4, postType),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1, ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetEntriesByDayRange",p);
		}

		public override DataSet GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString,CommandType.StoredProcedure,"subtext_GetRecentEntriesWithCategoryTitles",p);
			DataRelation dr = new DataRelation("cats",ds.Tables[0].Columns["ID"],ds.Tables[1].Columns["PostID"],false);
			ds.Relations.Add(dr);
			return ds;
		}

		public override IDataReader GetCategoryEntry(int postID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@PostID", SqlDbType.Int, 4, SqlHelper.CheckNull(postID)),
				SqlHelper.MakeInParam("@IsActive", SqlDbType.Bit, 1, ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetEntryWithCategoryTitles",p);
			
		}

			 public override IDataReader GetCategoryEntry(string EntryName, bool ActiveOnly)
			 {
				 SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@EntryName",SqlDbType.NVarChar,150,EntryName),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
				 return GetReader("subtext_GetEntryWithCategoryTitlesByEntryName",p);
			
			 }

		public override IDataReader GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@PostType",SqlDbType.Int,4,postType),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetRecentEntries",p);
		}

		/// <summary>
		/// Gets recent posts used to support the MetaBlogAPI. 
		/// Could be used for a Recent Posts control as well.
		/// </summary>
		/// <param name="ItemCount">Item count.</param>
		/// <param name="postType">Post type.</param>
		/// <param name="ActiveOnly">Active only.</param>
		/// <param name="DateUpdated"></param>
		/// <returns></returns>
		public override IDataReader GetRecentPosts(int ItemCount, PostType postType, bool ActiveOnly, DateTime DateUpdated)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@PostType",SqlDbType.Int,4,postType),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				SqlHelper.MakeInParam("@DateUpdated",SqlDbType.DateTime,8,DateUpdated),
				BlogIDParam
			};
			return GetReader("subtext_GetRecentEntriesByDateUpdated",p);
		}

		public override IDataReader GetEntry(string entryName, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@EntryName",SqlDbType.NVarChar,150,entryName),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetSingleEntryByName",p);
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
				SqlHelper.MakeInParam("@ContentChecksumHash", SqlDbType.VarChar, 32, checksumHash),
				BlogIDParam
			};
			return GetReader("subtext_GetCommentByChecksumHash", p);
		}

		public override IDataReader GetEntry(int postID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ID",SqlDbType.Int,4,postID),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetSingleEntry" ,p);
		}

		public override IDataReader GetSingleDay(DateTime dt)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@Date",SqlDbType.DateTime,8,dt),
				BlogIDParam
			};
			return GetReader("subtext_GetSingleDay",p);
		}

		public override IDataReader GetEntriesByCategory(int ItemCount, int catID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,SqlHelper.CheckNull(catID)),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetPostsByCategoryID",p);
	
		}

		public override IDataReader GetEntriesByCategory(int ItemCount, string CategoryName, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@CategoryName",SqlDbType.NVarChar, 150,CategoryName),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetPostsByCategoryName",p);
	
		}

		public override IDataReader GetEntriesByCategory(int ItemCount, int catID, DateTime DateUpdated, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,(catID)),
				SqlHelper.MakeInParam("@DateUpdated",SqlDbType.DateTime,8,DateUpdated),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetPostsByCategoryIDByDateUpdated",p);
		}

		public override IDataReader GetEntriesByCategory(int ItemCount, string CategoryName, DateTime DateUpdated, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@CategoryName",SqlDbType.NVarChar, 150,CategoryName),
				SqlHelper.MakeInParam("@DateUpdated",SqlDbType.DateTime,8,DateUpdated),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("subtext_GetPostsByCategoryIDByDateUpdated",p);
		}

		public override IDataReader GetPostsByCategoryID(int ItemCount, int catID)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,(catID)),
				BlogIDParam};
			return GetReader("subtext_GetPostsByCategoryID",p);
		}

		//Should power both EntryCollection and EntryDayCollection
		public override IDataReader GetPostCollectionByMonth(int month, int year)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@Month",SqlDbType.Int,4,month),
				SqlHelper.MakeInParam("@Year",SqlDbType.Int,4,year),
				BlogIDParam};
			return GetReader("subtext_GetPostsByMonth",p);
		}

//		//Should power both EntryCollection and EntryDayCollection
//		public override IDataReader GetPostsByMonth(int month, int year)
//		{
//			SqlParameter[] p =
//			{
//				SqlHelper.MakeInParam("@Month",SqlDbType.Int,4,month),
//				SqlHelper.MakeInParam("@Year",SqlDbType.Int,4,year),
//				BlogIDParam};
//			return GetReader("subtext_GetPostsByMonth",p);
//		}

		public override IDataReader GetRecentDayPosts(int ItemCount, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				SqlHelper.MakeInParam("@PostType",SqlDbType.Int,4,PostType.BlogPost),
				BlogIDParam};
			return GetReader("subtext_GetRecentEntries",p);
		}

		#endregion

		#region Update Blog Data

		public override bool DeleteEntry(int PostID)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ID",SqlDbType.Int,4,PostID),
				BlogIDParam
			};
			return NonQueryBool("subtext_DeletePost",p);
		}

		public override int InsertCategoryEntry(CategoryEntry ce)
		{
			int PostID = InsertEntry(ce);
			if(PostID > -1 && ce.Categories != null && ce.Categories.Length > 0)
			{
				SqlConnection conn = new SqlConnection(ConnectionString);
				SqlParameter[] p = new SqlParameter[3];
				p[0] = new SqlParameter("@Title",SqlDbType.NVarChar,150);
				p[1] = SqlHelper.MakeInParam("@PostID", SqlDbType. Int, 4, SqlHelper.CheckNull(PostID));
				p[2] = BlogIDParam;
				conn.Open();
				foreach(string s in ce.Categories)
				{
					p[0].Value = s;
					InsertLinkByCategoryName(p,conn);
				}
				conn.Close();
			}
			return PostID;
		}

		private void InsertLinkByCategoryName(SqlParameter[] p, SqlConnection conn)
		{
			string sql = "subtext_InsertPostCategoryByName";
			SqlHelper.ExecuteNonQuery(conn,CommandType.StoredProcedure,sql,p);

		}

		//use interate functions
		public override bool UpdateCategoryEntry(CategoryEntry ce)
		{
			bool result = UpdateEntry(ce);
			if(ce.Categories != null && ce.Categories.Length > 0)
			{
				SqlConnection conn = new SqlConnection(ConnectionString);
				SqlParameter[] p = new SqlParameter[3];
				p[0] = new SqlParameter("@Title",SqlDbType.NVarChar,150);
				p[1] = SqlHelper.MakeInParam("@PostID", SqlDbType.Int, 4, SqlHelper.CheckNull(ce.EntryID));
				p[2] = BlogIDParam;
				conn.Open();
				//DeleteCategoriesByPostID(ce.EntryID,conn);
				foreach(string s in ce.Categories)
				{
					p[0].Value = s;
					//InsertLinkByCategoryName(p,conn);
				}
				conn.Close();
			}
			return result;
		}

		public override bool SetEntryCategoryList(int PostID, int[] Categories)
		{
			if(Categories == null || Categories.Length == 0)
			{
				return false;
			}

			string[] cats = new string[Categories.Length];
			for(int i = 0; i<Categories.Length;i++)
			{
				cats[i] = Categories[i].ToString(CultureInfo.InvariantCulture);
			}
			string catList = string.Join(",",cats);

			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@PostID", SqlDbType.Int, 4, SqlHelper.CheckNull(PostID)),
				BlogIDParam,
				SqlHelper.MakeInParam("@CategoryList",SqlDbType.NVarChar,4000,catList)
			};
			return NonQueryBool("subtext_InsertLinkCategoryList",p);

		}

		/// <summary>
		/// Adds a new entry to the blog.  Whether the entry be a blog post, article,
		/// a comment, trackback, etc...
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public override int InsertEntry(Entry entry)
		{
			SqlParameter outIdParam = SqlHelper.MakeOutParam("@ID", SqlDbType.Int, 4);
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
				SqlHelper.MakeInParam("@TitleUrl",  SqlDbType.NVarChar, 255, StringHelper.ReturnNullForEmpty(entry.TitleUrl)), 
				SqlHelper.MakeInParam("@Text", SqlDbType.Text, 0, entry.Body), 
				SqlHelper.MakeInParam("@SourceUrl", SqlDbType.NVarChar, 200, SqlHelper.CheckNull(entry.SourceUrl)), 
				SqlHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
				SqlHelper.MakeInParam("@Author", SqlDbType.NVarChar, 50, SqlHelper.CheckNull(entry.Author)), 
				SqlHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, SqlHelper.CheckNull(entry.Email)), 
				SqlHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, SqlHelper.CheckNull(entry.Description)), 
				SqlHelper.MakeInParam("@SourceName", SqlDbType.NVarChar, 200, SqlHelper.CheckNull(entry.SourceName)), 
				SqlHelper.MakeInParam("@DateAdded", SqlDbType.DateTime, 8, entry.DateCreated), 
				SqlHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
				SqlHelper.MakeInParam("@ParentID", SqlDbType.Int, 4, SqlHelper.CheckNull(entry.ParentID)), 
				SqlHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, StringHelper.ReturnNullForEmpty(entry.EntryName)), 
				SqlHelper.MakeInParam("@ContentChecksumHash", SqlDbType.VarChar, 32, SqlHelper.CheckNull(entry.ContentChecksumHash)), 
				SqlHelper.MakeInParam("@DateSyndicated", SqlDbType.DateTime, 8, SqlHelper.CheckNull(entry.DateSyndicated)), 
				BlogIDParam,
				outIdParam
				
			};

			NonQueryInt("subtext_InsertEntry", p);
			return (int)outIdParam.Value;
		}
	
		/// <summary>
		/// Updates the specified entry in the database.
		/// </summary>
		/// <param name="entry">Entry.</param>
		/// <returns></returns>
		public override bool UpdateEntry(Entry entry)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ID", SqlDbType.Int, 4, entry.EntryID), 
				SqlHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
				SqlHelper.MakeInParam("@TitleUrl",  SqlDbType.NVarChar, 255, SqlHelper.CheckNull(entry.TitleUrl)),
				SqlHelper.MakeInParam("@Text", SqlDbType.Text, 0, entry.Body), 
				SqlHelper.MakeInParam("@SourceUrl", SqlDbType.NVarChar, 200, SqlHelper.CheckNull(entry.SourceUrl)), 
				SqlHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
				SqlHelper.MakeInParam("@Author", SqlDbType.NVarChar, 50, SqlHelper.CheckNull(entry.Author)), 
				SqlHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, SqlHelper.CheckNull(entry.Email)), 
				SqlHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, SqlHelper.CheckNull(entry.Description)), 
				SqlHelper.MakeInParam("@SourceName", SqlDbType.NVarChar, 200, SqlHelper.CheckNull(entry.SourceName)), 
				SqlHelper.MakeInParam("@DateUpdated", SqlDbType.SmallDateTime, 4, entry.DateUpdated), 
				SqlHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
				SqlHelper.MakeInParam("@ParentID", SqlDbType.Int, 4, SqlHelper.CheckNull(entry.ParentID)), 
				SqlHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, SqlHelper.CheckNull(entry.EntryName)), 
				SqlHelper.MakeInParam("@ContentChecksumHash", SqlDbType.VarChar, 32, SqlHelper.CheckNull(entry.ContentChecksumHash)), 
				SqlHelper.MakeInParam("@DateSyndicated", SqlDbType.DateTime, 8, SqlHelper.CheckNull(entry.DateSyndicated)), 
				BlogIDParam
			};
			return NonQueryBool("subtext_UpdateEntry", p);
		}

		//Not that efficent, but maybe we should just iterage over feedback items?
		public override int InsertPingTrackEntry(Entry entry)
		{
			if(entry.PostType == PostType.PingTrack)
			{
				SqlParameter outParam = SqlHelper.MakeOutParam("@ID", SqlDbType.Int, 4);
				SqlParameter[] p =
				{
					SqlHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
					SqlHelper.MakeInParam("@TitleUrl",  SqlDbType.NVarChar, 255, SqlHelper.CheckNull(entry.TitleUrl)), 
					SqlHelper.MakeInParam("@Text", SqlDbType.Text, 0, entry.Body), 
					SqlHelper.MakeInParam("@SourceUrl", SqlDbType.NVarChar, 200, SqlHelper.CheckNull(entry.SourceUrl)), 
					SqlHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
					SqlHelper.MakeInParam("@Author", SqlDbType.NVarChar, 50, SqlHelper.CheckNull(entry.Author)), 
					SqlHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, SqlHelper.CheckNull(entry.Email)), 
					SqlHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, SqlHelper.CheckNull(entry.Description)), 
					SqlHelper.MakeInParam("@SourceName", SqlDbType.NVarChar, 200, SqlHelper.CheckNull(entry.SourceName)), 
					SqlHelper.MakeInParam("@DateAdded", SqlDbType.DateTime, 8, entry.DateCreated), 
					SqlHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
					SqlHelper.MakeInParam("@ContentChecksumHash", SqlDbType.VarChar, 32, entry.ContentChecksumHash), 
					SqlHelper.MakeInParam("@ParentID", SqlDbType.Int, 4, SqlHelper.CheckNull(entry.ParentID)), 
					SqlHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, SqlHelper.CheckNull(entry.EntryName)), 
					BlogIDParam, 
					outParam
				};

					NonQueryInt("subtext_InsertPingTrackEntry",p);
					return (int)outParam.Value;
			}
			else
			{
				throw new ArgumentException("PingTracks is the only valid PostType for InsertPingTrackEntry","entry.PostType");
			}

		}


		#endregion

		#region Links

		public override IDataReader GetLinkCollectionByPostID(int PostID)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@PostID", SqlDbType.Int, 4, SqlHelper.CheckNull(PostID)),
				BlogIDParam
			};
			return GetReader("subtext_GetLinkCollectionByPostID", p);
		}

		/// <summary>
		/// Adds the entry to categories specified in the <see cref="LinkCollection"/>.
		/// </summary>
		/// <param name="PostID">Post ID.</param>
		/// <param name="lc">Lc.</param>
		/// <returns></returns>
		public override bool AddEntryToCategories(int PostID, LinkCollection lc)
		{
			int count = 0;
			if(lc != null)
			{
				count = lc.Count;
			}
			SqlConnection conn = new SqlConnection(ConnectionString);
			conn.Open();

			//DeleteCategoriesByPostID(PostID,conn);
			//we should use the iter_charlist_to_table function instead
			if(count > 0)
			{
				string sql = "subtext_InsertLink";
				
				Link link = lc[0];
				SqlParameter[] p = 
				{
					SqlHelper.MakeInParam("@Title", SqlDbType.NVarChar, 150, SqlHelper.CheckNull(link.Title)), 
					SqlHelper.MakeInParam("@Url", SqlDbType.NVarChar, 255, SqlHelper.CheckNull(link.Url)), 
					SqlHelper.MakeInParam("@Rss", SqlDbType.NVarChar, 255, SqlHelper.CheckNull(link.Rss)), 
					SqlHelper.MakeInParam("@Active", SqlDbType.Bit, 1, link.IsActive), 
					SqlHelper.MakeInParam("@NewWindow", SqlDbType.Bit, 1, link.NewWindow), 
					SqlHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, SqlHelper.CheckNull(link.CategoryID)), 
					SqlHelper.MakeInParam("@PostID", SqlDbType.Int, 4, SqlHelper.CheckNull(link.PostID)), 
					BlogIDParam, 
					SqlHelper.MakeOutParam("@LinkID", SqlDbType.Int, 4)
				};
				return NonQueryBool(sql,p);
               }
			conn.Close();
			return false;
		}

		public override bool DeleteLink(int LinkID)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@LinkID",SqlDbType.Int,4,LinkID),
				BlogIDParam
			};
			return NonQueryBool("subtext_DeleteLink",p);

		}

		public override IDataReader GetSingleLink(int linkID)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@LinkID",SqlDbType.Int,4,linkID),
				BlogIDParam
			};
			return GetReader("subtext_GetSingleLink",p);
		}

		public override int InsertLink(Link link)
		{
			SqlParameter outParam = SqlHelper.MakeOutParam("@LinkID",SqlDbType.Int,4);
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,link.Title),
				SqlHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,link.Url),
				SqlHelper.MakeInParam("@Rss",SqlDbType.NVarChar,255,SqlHelper.CheckNull(link.Rss)),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,link.IsActive),
				SqlHelper.MakeInParam("@NewWindow",SqlDbType.Bit,1,link.NewWindow),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,SqlHelper.CheckNull(link.CategoryID)),
				SqlHelper.MakeInParam("@PostID", SqlDbType.Int, 4, SqlHelper.CheckNull(link.PostID)),
				BlogIDParam,
				outParam
			};
			NonQueryInt("subtext_InsertLink",p);
			return (int)outParam.Value;

		}

		public override bool UpdateLink(Link link)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,link.Title),
				SqlHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,link.Url),
				SqlHelper.MakeInParam("@Rss",SqlDbType.NVarChar,255,SqlHelper.CheckNull(link.Rss)),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,link.IsActive),
				SqlHelper.MakeInParam("@NewWindow",SqlDbType.Bit,1,link.NewWindow),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,SqlHelper.CheckNull(link.CategoryID)),
				SqlHelper.MakeInParam("@LinkID",SqlDbType.Int,4,link.LinkID),
				BlogIDParam
			};
			return NonQueryBool("subtext_UpdateLink",p);
		}


		public override IDataReader GetCategories(CategoryType catType, bool ActiveOnly)
		{
			SqlParameter[] p ={SqlHelper.MakeInParam("@CategoryType",SqlDbType.TinyInt,1,catType),
							  SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
								  BlogIDParam};
			return GetReader("subtext_GetAllCategories",p);
		}

		//maps to blog_GetActiveCategoriesWithLinkCollection
		public override DataSet GetActiveCategories()
		{
			SqlParameter[] p ={BlogIDParam};
			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString,CommandType.StoredProcedure,"subtext_GetActiveCategoriesWithLinkCollection",p);

			DataRelation dl = new DataRelation("CategoryID",ds.Tables[0].Columns["CategoryID"],ds.Tables[1].Columns["CategoryID"],false);
			ds.Relations.Add(dl);

			return ds;
		}

		//maps to blog_GetLinksByCategoryID
		public override IDataReader GetLinksByCategoryID(int catID, bool ActiveOnly)
		{
			string sql = ActiveOnly ? "subtext_GetLinksByActiveCategoryID" : "subtext_GetLinksByCategoryID";
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4 ,SqlHelper.CheckNull(catID)),
				BlogIDParam
			};
			//return SqlHelper.ExecuteDataset(ConnectionString,CommandType.StoredProcedure,sql,p);
			return GetReader(sql,p);
		}

		#endregion

		#region Categories


		public override bool DeleteCategory(int CatID)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,SqlHelper.CheckNull(CatID)),
				BlogIDParam
			};
			return NonQueryBool("subtext_DeleteCategory",p);

		}

		//maps to blog_GetCategory
		public override IDataReader GetLinkCategory(int catID, bool IsActive)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,SqlHelper.CheckNull(catID)),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,IsActive),
				BlogIDParam
			};
			return GetReader("subtext_GetCategory",p);
		}
		

		public override IDataReader GetLinkCategory(string categoryName, bool IsActive)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@CategoryName",SqlDbType.NVarChar,150,categoryName),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,IsActive),
				BlogIDParam
			};
			return GetReader("subtext_GetCategoryByName",p);
		}

		public override bool UpdateCategory(LinkCategory lc)
		{
			SqlParameter[] p =
			{

				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,lc.Title),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,lc.IsActive),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,SqlHelper.CheckNull(lc.CategoryID)),
				SqlHelper.MakeInParam("@CategoryType",SqlDbType.TinyInt,1,lc.CategoryType),
				SqlHelper.MakeInParam("@Description",SqlDbType.NVarChar,1000,SqlHelper.CheckNull(lc.Description)),
				BlogIDParam
			};
			return NonQueryBool("subtext_UpdateCategory",p);
		}

		//maps to blog_LinkCategory
		public override int InsertCategory(LinkCategory lc)
		{
			SqlParameter outParam = SqlHelper.MakeOutParam("@CategoryID",SqlDbType.Int,4);
			SqlParameter[] p =
			{

				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,lc.Title),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,lc.IsActive),
				SqlHelper.MakeInParam("@CategoryType",SqlDbType.TinyInt,1,lc.CategoryType),
				SqlHelper.MakeInParam("@Description",SqlDbType.NVarChar,1000,SqlHelper.CheckNull(lc.Description)),
				BlogIDParam,
				outParam
			};
			NonQueryInt("subtext_InsertCategory",p);
			return (int)outParam.Value;
		}

		#endregion

		#region FeedBack

		//we could pass ParentID with the rest of the sprocs
		//one interface for entry data?
		public override IDataReader GetFeedBack(int PostID)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ParentID", SqlDbType.Int, 4, SqlHelper.CheckNull(PostID)),
				BlogIDParam
			};
			return GetReader("subtext_GetFeedBack",p);
		}

		#endregion

		#region Configuration

		/// <summary>
		/// Adds the initial blog configuration.  This is a convenience method for 
		/// allowing a user with a freshly installed blog to immediately gain access 
		/// to the admin section to edit the blog.
		/// </summary>
		/// <param name="userName">Name of the user.</param>
		/// <param name="password">Password.</param>
		/// <returns></returns>
		public override bool AddBlogConfiguration(string title, string userName, string password, string host, string application)
		{
			SqlParameter[] parameters = 
			{
				SqlHelper.MakeInParam("@Title", SqlDbType.NVarChar, 100, userName)
				, SqlHelper.MakeInParam("@UserName", SqlDbType.NVarChar, 50, userName)
				, SqlHelper.MakeInParam("@Password", SqlDbType.NVarChar, 50, password)
				, SqlHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, string.Empty)
				, SqlHelper.MakeInParam("@Host", SqlDbType.NVarChar, 50, host)
				, SqlHelper.MakeInParam("@Application", SqlDbType.NVarChar, 50, application)
				, SqlHelper.MakeInParam("@IsHashed", SqlDbType.Bit, 1, Config.Settings.UseHashedPasswords)
				
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
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public override IDataReader GetBlogInfo(string host, string application)
		{
			return GetBlogInfo(host, application, false); //Not Strict.
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
		/// <param name="application">Application.</param>
		/// <param name="strict">If false, then this will return a blog record if 
		/// there is only one blog record, regardless if the application and hostname match.</param>
		/// <returns></returns>
		public override IDataReader GetBlogInfo(string host, string application, bool strict)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, host)
				,SqlHelper.MakeInParam("@Application", SqlDbType.NVarChar, 50, application)
				,SqlHelper.MakeInParam("@Strict", SqlDbType.Bit, 1, strict)
			};
			return GetReader("subtext_GetConfig", p);
		}

		
		/// <summary>
		/// Gets information about a specific blog via its Id.
		/// </summary>
		/// <param name="blogId">Blog ID.</param>
		/// <returns></returns>
		public override IDataReader GetBlogInfo(int blogId)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@BlogID", SqlDbType.Int, 4, SqlHelper.CheckNull(blogId))
			};
			return GetReader("subtext_GetConfigByBlogID",p);
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

			SqlParameter[] p = 
				{
					SqlHelper.MakeInParam("@BlogID", SqlDbType.Int,  4, SqlHelper.CheckNull(info.BlogID))
					,SqlHelper.MakeInParam("@UserName", SqlDbType.NVarChar, 50, info.UserName) 
					,SqlHelper.MakeInParam("@Password", SqlDbType.NVarChar, 50, info.Password) 
					,SqlHelper.MakeInParam("@Author", SqlDbType.NVarChar, 100, info.Author) 
					,SqlHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, info.Email) 
					,SqlHelper.MakeInParam("@Title", SqlDbType.NVarChar, 100, info.Title) 
					,SqlHelper.MakeInParam("@SubTitle", SqlDbType.NVarChar, 250, info.SubTitle) 
					,SqlHelper.MakeInParam("@Skin", SqlDbType.NVarChar, 50, info.Skin.SkinName) 
					,SqlHelper.MakeInParam("@Application", SqlDbType.NVarChar, 50, info.CleanApplication) 
					,SqlHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, info.Host) 
					,SqlHelper.MakeInParam("@TimeZone", SqlDbType.Int, 4, info.TimeZone) 
					,SqlHelper.MakeInParam("@Language", SqlDbType.NVarChar, 10, info.Language) 
					,SqlHelper.MakeInParam("@News", SqlDbType.Text, 0, SqlHelper.CheckNull(info.News)) 
					,SqlHelper.MakeInParam("@ItemCount", SqlDbType.Int,  4, info.ItemCount) 
					,SqlHelper.MakeInParam("@Flag", SqlDbType.Int,  4, (int)info.Flag) 
					,SqlHelper.MakeInParam("@LastUpdated", SqlDbType.DateTime,  8, info.LastUpdated) 
					,SqlHelper.MakeInParam("@SecondaryCss", SqlDbType.Text, 0, SqlHelper.CheckNull(info.Skin.SkinCssText)) 
					,SqlHelper.MakeInParam("@SkinCssFile", SqlDbType.VarChar, 100, SqlHelper.CheckNull(info.Skin.SkinCssFile)) 
					,SqlHelper.MakeInParam("@LicenseUrl", SqlDbType.NVarChar, 64, info.LicenseUrl)
					,SqlHelper.MakeInParam("@DaysTillCommentsClose", SqlDbType.Int, 4, daysTillCommentsClose)
					,SqlHelper.MakeInParam("@CommentDelayInMinutes", SqlDbType.Int, 4, commentDelayInMinutes)
				};


			return NonQueryBool("subtext_UpdateConfig", p);

		}

		#endregion

		#region Archives

		public override IDataReader GetPostsByMonthArchive()
		{
			SqlParameter[] p = {BlogIDParam};
			return GetReader("subtext_GetPostsByMonthArchive",p);
		}

		public override IDataReader GetPostsByYearArchive()
		{
			SqlParameter[] p = {BlogIDParam};
			return GetReader("subtext_GetPostsByYearArchive",p);
		}

		#endregion

		#region KeyWords

		public override IDataReader GetKeyWord(int KeyWordID)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@KeyWordID",SqlDbType.Int,4,KeyWordID),
				BlogIDParam
			};
			return GetReader("subtext_GetKeyWord",p);
		}

		public override bool DeleteKeyWord(int KeyWordID)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@KeyWordID",SqlDbType.Int,4,KeyWordID),
				BlogIDParam
			};
			return NonQueryBool("subtext_DeleteKeyWord",p);
		}



		public override int InsertKeyWord(KeyWord kw)
		{
			SqlParameter outParam = SqlHelper.MakeOutParam("@KeyWordID",SqlDbType.Int,4);
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@Word",SqlDbType.NVarChar,100,kw.Word),
				SqlHelper.MakeInParam("@Text",SqlDbType.NVarChar,100,kw.Text),
				SqlHelper.MakeInParam("@ReplaceFirstTimeOnly",SqlDbType.Bit,1,kw.ReplaceFirstTimeOnly),
				SqlHelper.MakeInParam("@OpenInNewWindow",SqlDbType.Bit,1,kw.OpenInNewWindow),
				SqlHelper.MakeInParam("@CaseSensitive",SqlDbType.Bit,1,kw.CaseSensitive),
				SqlHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,kw.Url),
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,100,kw.Title),
				BlogIDParam,
				outParam
			};
			NonQueryInt("subtext_InsertKeyWord",p);
			return (int)outParam.Value;
		}

		public override bool UpdateKeyWord(KeyWord kw)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@KeyWordID",SqlDbType.Int,4,kw.KeyWordID),
				SqlHelper.MakeInParam("@Word",SqlDbType.NVarChar,100,kw.Word),
				SqlHelper.MakeInParam("@Text",SqlDbType.NVarChar,100,kw.Text),
				SqlHelper.MakeInParam("@ReplaceFirstTimeOnly",SqlDbType.Bit,1,kw.ReplaceFirstTimeOnly),
				SqlHelper.MakeInParam("@OpenInNewWindow",SqlDbType.Bit,1,kw.OpenInNewWindow),
				SqlHelper.MakeInParam("@CaseSensitive",SqlDbType.Bit,1,kw.CaseSensitive),
				SqlHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,kw.Url),
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,100,kw.Title),
				BlogIDParam
			};
			return NonQueryBool("subtext_UpdateKeyWord",p);
		}



		public override IDataReader GetKeyWords()
		{
			SqlParameter[] p =
			{
				BlogIDParam
			};
			return GetReader("subtext_GetBlogKeyWords",p);
		}

		public override IDataReader GetPagedKeyWords(int pageIndex, int pageSize,bool sortDescending)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize),
				SqlHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, sortDescending),
				BlogIDParam
			};
			return GetReader("subtext_GetPageableKeyWords",p);
		}


		#endregion

		#region Helpers

		private IDataReader GetReader(string sql)
		{
			return SqlHelper.ExecuteReader(ConnectionString,CommandType.StoredProcedure, sql);
		}

		private IDataReader GetReader(string sql, SqlParameter[] p)
		{
			return SqlHelper.ExecuteReader(ConnectionString,CommandType.StoredProcedure, sql, p);
		}

		private int NonQueryInt(string sql, SqlParameter[] p)
		{
			return SqlHelper.ExecuteNonQuery(ConnectionString,CommandType.StoredProcedure, sql, p);
		}

		private bool NonQueryBool(string sql, SqlParameter[] p)
		{
			return NonQueryInt(sql, p) > 0;
		}

		#endregion

		#region Host Data

		/// <summary>
		/// Returns the data for the Host.
		/// </summary>
		public override IDataReader GetHost()
		{
			return GetReader("[dbo].[subtext_GetHost]");
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
				SqlHelper.MakeInParam("@HostUserName", SqlDbType.NVarChar,  64, host.HostUserName)
				, SqlHelper.MakeInParam("@Password", SqlDbType.NVarChar,  32, host.Password)
				, SqlHelper.MakeInParam("@Salt", SqlDbType.NVarChar,  32, host.Salt)
			};

			return NonQueryBool("subtext_UpdateHost", p);
		}
		#endregion Host Data
		#endregion
	}
}

