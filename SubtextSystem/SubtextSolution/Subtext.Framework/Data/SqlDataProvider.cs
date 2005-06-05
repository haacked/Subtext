#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
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
				return  SqlHelper.MakeInParam("@BlogID", SqlDbType.Int, 4, Config.CurrentBlog.BlogID);
			}
		}

		#region DbProvider Methods
		#region Statistics

		public override bool TrackEntry(EntryViewCollection evc)
		{
			if(evc != null)
			{
				//				System.IO.StringWriter sw = new System.IO.StringWriter();
				//				System.Xml.XmlTextWriter tw = new System.Xml.XmlTextWriter(sw);
				//				tw.WriteStartElement("EntryViews");
				//
				//				
				//				foreach(EntryView ev in evc)
				//				{
				//					tw.WriteStartElement("EV");
				//					tw.WriteAttributeString("E",ev.EntryID.ToString());
				//					tw.WriteAttributeString("B",ev.BlogID.ToString());
				//					//tw.WriteAttributeString("U",ev.ReferralUrl);
				//					tw.WriteAttributeString("W",((int)ev.PageViewType).ToString());
				//					tw.WriteEndElement();
				//					
				//
				//				}
				//				tw.WriteEndElement();
				
				

				SqlConnection conn = new SqlConnection(this.ConnectionString);
				try
				{
									
					//conn.Open();
					foreach(EntryView ev in evc)
					{
				
						SqlParameter[] p =	
										{
											SqlHelper.MakeInParam("@EntryID",SqlDbType.Int,4,ev.EntryID),
											SqlHelper.MakeInParam("@BlogID",SqlDbType.Int,4,ev.BlogID),
											SqlHelper.MakeInParam("@URL",SqlDbType.NVarChar,255,DataHelper.CheckNull(ev.ReferralUrl)),
											SqlHelper.MakeInParam("@IsWeb",SqlDbType.Bit,1,ev.PageViewType)
										};
						SqlHelper.ExecuteNonQuery(conn,CommandType.StoredProcedure,"blog_TrackEntry",p);
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

		public override bool TrackEntry(EntryView ev)
		{
			//blog_TrackEntry
			SqlParameter[] p =	
					{
						SqlHelper.MakeInParam("@EntryID",SqlDbType.Int,4,ev.EntryID),
						SqlHelper.MakeInParam("@BlogID",SqlDbType.Int,4,ev.BlogID),
						SqlHelper.MakeInParam("@URL",SqlDbType.NVarChar,255,DataHelper.CheckNull(ev.ReferralUrl)),
						SqlHelper.MakeInParam("@IsWeb",SqlDbType.Bit,1,ev.PageViewType)
			};
			return this.NonQueryBool("blog_TrackEntry",p);
		}
		
		#endregion

		#region Images

		public override IDataReader GetImagesByCategoryID(int catID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,catID),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};

			return GetReader("blog_GetImageCategory",p);
		}

		public override IDataReader GetSingleImage(int imageID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ImageID",SqlDbType.Int,4,imageID),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("blog_GetSingleImage",p);
		}

		public override int InsertImage(Image _image)
		{
			SqlParameter outParam = SqlHelper.MakeOutParam("@ImageID", SqlDbType.Int, 4);
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,250,_image.Title),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,_image.CategoryID),
				SqlHelper.MakeInParam("@Width",SqlDbType.Int,4,_image.Width),
				SqlHelper.MakeInParam("@Height",SqlDbType.Int,4,_image.Height),
				SqlHelper.MakeInParam("@File",SqlDbType.NVarChar,50,_image.File),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,_image.IsActive),
				BlogIDParam,
				outParam
			};
			NonQueryInt("blog_InsertImage",p);
			return (int)outParam.Value;
		}

		public override bool UpdateImage(Image _image)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,250,_image.Title),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,_image.CategoryID),
				SqlHelper.MakeInParam("@Width",SqlDbType.Int,4,_image.Width),
				SqlHelper.MakeInParam("@Height",SqlDbType.Int,4,_image.Height),
				SqlHelper.MakeInParam("@File",SqlDbType.NVarChar,50,_image.File),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,_image.IsActive),
				BlogIDParam,
				SqlHelper.MakeInParam("@ImageID",SqlDbType.Int,4,_image.ImageID)
			};
			return NonQueryBool("blog_UpdateImage",p);
		}

		public override bool DeleteImage(int imageID)
		{
			SqlParameter[] p = 
			{
				BlogIDParam,
				SqlHelper.MakeInParam("@ImageID",SqlDbType.Int,4,imageID)
			};
			return NonQueryBool("blog_DeleteImage",p);
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
			string sql = "blog_GetPageableBlogs";

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
			string sql = "blog_GetBlogById";

			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql, conn);
			
			command.CommandType = CommandType.StoredProcedure;
			command.Parameters.Add(SqlHelper.MakeInParam("@BlogId", SqlDbType.Int, 4, blogId));

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
			string sql = "blog_GetBlogsByHost";

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
			string sql = useCategory ? "blog_GetPageableLinksByCategoryID" : "blog_GetPageableLinks";
			
			SqlConnection conn = new SqlConnection(ConnectionString);
			SqlCommand command = new SqlCommand(sql,conn);
			command.CommandType = CommandType.StoredProcedure;			
		
			command.Parameters.Add(SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex));
			command.Parameters.Add(SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize));
			command.Parameters.Add(SqlHelper.MakeInParam("@SortDesc", SqlDbType.Bit, 1, sortDescending));
			command.Parameters.Add(BlogIDParam);
		
			if (useCategory)
			{
				command.Parameters.Add(SqlHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, CategoryID));
			}

			conn.Open();
			return command.ExecuteReader(CommandBehavior.CloseConnection);
			
		}
		public override IDataReader GetPagedEntries(PostType postType, int categoryID, int pageIndex, int pageSize, bool sortDescending)
		{
			// default setup is for unfiltered pageable results
			bool useCategoryID = categoryID > -1;

			string sql = useCategoryID ? "blog_GetPageableEntriesByCategoryID" : "blog_GetPageableEntries";

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
					command.Parameters.Add(SqlHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, categoryID));
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
			return GetReader("blog_GetPageableViewStats",p);
		}

		public override IDataReader GetPagedReferrers(int pageIndex, int pageSize)
		{
			SqlParameter[] p =
			{
				BlogIDParam,
				SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};
			return GetReader("blog_GetPageableReferrers",p);

		}

		public override IDataReader GetPagedReferrers(int pageIndex, int pageSize, int EntryID)
		{
			SqlParameter[] p =
			{
				BlogIDParam,
				SqlHelper.MakeInParam("@EntryID", SqlDbType.Int, 4, EntryID),
				SqlHelper.MakeInParam("@PageIndex", SqlDbType.Int, 4, pageIndex),
				SqlHelper.MakeInParam("@PageSize", SqlDbType.Int, 4, pageSize)
			};
			
			return GetReader("blog_GetPageableReferrersByEntryID",p);

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
			return GetReader("blog_GetPageableFeedback",p);

		}

		#endregion	

		#region Get Blog Data

		
		
		public override IDataReader GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount", SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@PostType",SqlDbType.Int,4,pt),
				SqlHelper.MakeInParam("@PostConfig",SqlDbType.Int,4,pc),
				BlogIDParam				
			};

			return GetReader("blog_GetConditionalEntries",p);
		}
		
		public override IDataReader GetConditionalEntries(int ItemCount, PostType pt, PostConfig pc, DateTime DateUpdated)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount", SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@PostType",SqlDbType.Int,4,pt),
				SqlHelper.MakeInParam("@PostConfig",SqlDbType.Int,4,pc),
				SqlHelper.MakeInParam("@DateUpdated",SqlDbType.DateTime,8,DateUpdated),
				BlogIDParam				
			};

			return GetReader("blog_GetConditionalEntriesByDateUpdated",p);
		}
			
		public override IDataReader GetEntriesByDateRangle(DateTime start, DateTime stop, PostType postType, bool ActiveOnly)
		{
			SqlParameter[] p =	
			{
				SqlHelper.MakeInParam("@StartDate",SqlDbType.DateTime,8,start),
				SqlHelper.MakeInParam("@StopDate",SqlDbType.DateTime,8,stop),
				SqlHelper.MakeInParam("@PostType",SqlDbType.Int,4,postType),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("blog_GetEntriesByDayRange",p);
		}

		public override DataSet GetRecentPostsWithCategories(int ItemCount, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString,CommandType.StoredProcedure,"blog_GetRecentEntriesWithCategoryTitles",p);
			DataRelation dr = new DataRelation("cats",ds.Tables[0].Columns["ID"],ds.Tables[1].Columns["PostID"],false);
			ds.Relations.Add(dr);
			return ds;
		}

		public override IDataReader GetCategoryEntry(int postID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@PostID",SqlDbType.Int,4,postID),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("blog_GetEntryWithCategoryTitles",p);
			
		}

			 public override IDataReader GetCategoryEntry(string EntryName, bool ActiveOnly)
			 {
				 SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@EntryName",SqlDbType.NVarChar,150,EntryName),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
				 return GetReader("blog_GetEntryWithCategoryTitlesByEntryName",p);
			
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
			return GetReader("blog_GetRecentEntries",p);
		}

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
			return GetReader("blog_GetRecentEntriesByDateUpdated",p);
		}

		public override IDataReader GetEntry(string entryName, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@EntryName",SqlDbType.NVarChar,150,entryName),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("blog_GetSingleEntryByName",p);
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
			return GetReader("blog_GetCommentByChecksumHash", p);
		}

		public override IDataReader GetEntry(int postID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ID",SqlDbType.Int,4,postID),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("blog_GetSingleEntry" ,p);
		}

		public override IDataReader GetSingleDay(DateTime dt)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@Date",SqlDbType.DateTime,8,dt),
				BlogIDParam
			};
			return GetReader("blog_GetSingleDay",p);
		}

		public override IDataReader GetEntriesByCategory(int ItemCount, int catID, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,catID),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("blog_GetPostsByCategoryID",p);
	
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
			return GetReader("blog_GetPostsByCategoryName",p);
	
		}

		public override IDataReader GetEntriesByCategory(int ItemCount, int catID, DateTime DateUpdated, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,catID),
				SqlHelper.MakeInParam("@DateUpdated",SqlDbType.DateTime,8,DateUpdated),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				BlogIDParam
			};
			return GetReader("blog_GetPostsByCategoryIDByDateUpdated",p);
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
			return GetReader("blog_GetPostsByCategoryIDByDateUpdated",p);
		}

		public override IDataReader GetPostsByCategoryID(int ItemCount, int catID)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,catID),
				BlogIDParam};
			return GetReader("blog_GetPostsByCategoryID",p);
		}

		//Should power both EntryCollection and EntryDayCollection
		public override IDataReader GetPostCollectionByMonth(int month, int year)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@Month",SqlDbType.Int,4,month),
				SqlHelper.MakeInParam("@Year",SqlDbType.Int,4,year),
				BlogIDParam};
			return GetReader("blog_GetPostsByMonth",p);
		}

//		//Should power both EntryCollection and EntryDayCollection
//		public override IDataReader GetPostsByMonth(int month, int year)
//		{
//			SqlParameter[] p =
//			{
//				SqlHelper.MakeInParam("@Month",SqlDbType.Int,4,month),
//				SqlHelper.MakeInParam("@Year",SqlDbType.Int,4,year),
//				BlogIDParam};
//			return GetReader("blog_GetPostsByMonth",p);
//		}

		public override IDataReader GetRecentDayPosts(int ItemCount, bool ActiveOnly)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ItemCount",SqlDbType.Int,4,ItemCount),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
				SqlHelper.MakeInParam("@PostType",SqlDbType.Int,4,PostType.BlogPost),
				BlogIDParam};
			return GetReader("blog_GetRecentEntries",p);
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
			return NonQueryBool("blog_DeletePost",p);
		}

		public override int InsertCategoryEntry(CategoryEntry ce)
		{
			int PostID = InsertEntry(ce);
			if(PostID > -1 && ce.Categories != null && ce.Categories.Length > 0)
			{
				SqlConnection conn = new SqlConnection(ConnectionString);
				SqlParameter[] p = new SqlParameter[3];
				p[0] = new SqlParameter("@Title",SqlDbType.NVarChar,150);
				p[1] = SqlHelper.MakeInParam("@PostID",SqlDbType.Int,4,PostID);
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
			string sql = "blog_InsertPostCategoryByName";
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
				p[1] = SqlHelper.MakeInParam("@PostID",SqlDbType.Int,4,ce.EntryID);
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
				SqlHelper.MakeInParam("@PostID",SqlDbType.Int,4,PostID),
				BlogIDParam,
				SqlHelper.MakeInParam("@CategoryList",SqlDbType.NVarChar,4000,catList)
			};
			return NonQueryBool("blog_InsertLinkCategoryList",p);

		}

		//maps to blog_InsertBlog
		public override int InsertEntry(Entry entry)
		{
			SqlParameter outIdParam = SqlHelper.MakeOutParam("@ID", SqlDbType.Int, 4);
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
				SqlHelper.MakeInParam("@TitleUrl",  SqlDbType.NVarChar, 255, DataHelper.CheckNull(entry.TitleUrl)), 
				SqlHelper.MakeInParam("@Text", SqlDbType.Text, 0, entry.Body), 
				SqlHelper.MakeInParam("@SourceUrl", SqlDbType.NVarChar, 200, DataHelper.CheckNull(entry.SourceUrl)), 
				SqlHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
				SqlHelper.MakeInParam("@Author", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Author)), 
				SqlHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Email)), 
				SqlHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, DataHelper.CheckNull(entry.Description)), 
				SqlHelper.MakeInParam("@SourceName", SqlDbType.NVarChar, 200, DataHelper.CheckNull(entry.SourceName)), 
				SqlHelper.MakeInParam("@DateAdded", SqlDbType.DateTime, 8, entry.DateCreated), 
				SqlHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
				SqlHelper.MakeInParam("@ParentID", SqlDbType.Int, 4, entry.ParentID), 
				SqlHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, DataHelper.CheckNull(entry.EntryName)), 
				SqlHelper.MakeInParam("@ContentChecksumHash", SqlDbType.VarChar, 32, DataHelper.CheckNull(entry.ContentChecksumHash)), 
				BlogIDParam,
				outIdParam
				
			};

			NonQueryInt("blog_InsertEntry",p);
			return (int)outIdParam.Value;
		}

		

		public override bool UpdateEntry(Entry entry)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@ID", SqlDbType.Int, 4, entry.EntryID), 
				SqlHelper.MakeInParam("@Title",  SqlDbType.NVarChar, 255, entry.Title), 
				SqlHelper.MakeInParam("@TitleUrl",  SqlDbType.NVarChar, 255, DataHelper.CheckNull(entry.TitleUrl)),
				SqlHelper.MakeInParam("@Text", SqlDbType.Text, 0, entry.Body), 
				SqlHelper.MakeInParam("@SourceUrl", SqlDbType.NVarChar, 200, DataHelper.CheckNull(entry.SourceUrl)), 
				SqlHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
				SqlHelper.MakeInParam("@Author", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Author)), 
				SqlHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Email)), 
				SqlHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, DataHelper.CheckNull(entry.Description)), 
				SqlHelper.MakeInParam("@SourceName", SqlDbType.NVarChar, 200, DataHelper.CheckNull(entry.SourceName)), 
				SqlHelper.MakeInParam("@DateUpdated", SqlDbType.SmallDateTime, 4, entry.DateUpdated), 
				SqlHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
				SqlHelper.MakeInParam("@ParentID", SqlDbType.Int, 4, entry.ParentID), 
				SqlHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, DataHelper.CheckNull(entry.EntryName)), 
				SqlHelper.MakeInParam("@ContentChecksumHash", SqlDbType.VarChar, 32, DataHelper.CheckNull(entry.ContentChecksumHash)), 
				BlogIDParam
			};
			return NonQueryBool("blog_UpdateEntry", p);
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
					SqlHelper.MakeInParam("@TitleUrl",  SqlDbType.NVarChar, 255, DataHelper.CheckNull(entry.TitleUrl)), 
					SqlHelper.MakeInParam("@Text", SqlDbType.Text, 0, entry.Body), 
					SqlHelper.MakeInParam("@SourceUrl", SqlDbType.NVarChar, 200, DataHelper.CheckNull(entry.SourceUrl)), 
					SqlHelper.MakeInParam("@PostType", SqlDbType.Int, 4, entry.PostType), 
					SqlHelper.MakeInParam("@Author", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Author)), 
					SqlHelper.MakeInParam("@Email", SqlDbType.NVarChar, 50, DataHelper.CheckNull(entry.Email)), 
					SqlHelper.MakeInParam("@Description", SqlDbType.NVarChar, 500, DataHelper.CheckNull(entry.Description)), 
					SqlHelper.MakeInParam("@SourceName", SqlDbType.NVarChar, 200, DataHelper.CheckNull(entry.SourceName)), 
					SqlHelper.MakeInParam("@DateAdded", SqlDbType.DateTime, 8, entry.DateCreated), 
					SqlHelper.MakeInParam("@PostConfig", SqlDbType.Int, 4, entry.PostConfig), 
					SqlHelper.MakeInParam("@ContentChecksumHash", SqlDbType.VarChar, 32, entry.ContentChecksumHash), 
					SqlHelper.MakeInParam("@ParentID", SqlDbType.Int, 4, entry.ParentID), 
					SqlHelper.MakeInParam("@EntryName", SqlDbType.NVarChar, 150, DataHelper.CheckNull(entry.EntryName)), 
					BlogIDParam, 
					outParam
				};

					NonQueryInt("blog_InsertPingTrackEntry",p);
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
				SqlHelper.MakeInParam("@PostID",SqlDbType.Int,4,PostID),
				BlogIDParam
			};
			return GetReader("blog_GetLinkCollectionByPostID", p);
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
				string sql = "blog_InsertLink";
				
				Link link = lc[0];
				SqlParameter[] p = 
				{
					SqlHelper.MakeInParam("@Title", SqlDbType.NVarChar, 150, DataHelper.CheckNull(link.Title)), 
					SqlHelper.MakeInParam("@Url", SqlDbType.NVarChar, 255, DataHelper.CheckNull(link.Url)), 
					SqlHelper.MakeInParam("@Rss", SqlDbType.NVarChar, 255, DataHelper.CheckNull(link.Rss)), 
					SqlHelper.MakeInParam("@Active", SqlDbType.Bit, 1, link.IsActive), 
					SqlHelper.MakeInParam("@NewWindow", SqlDbType.Bit, 1, link.NewWindow), 
					SqlHelper.MakeInParam("@CategoryID", SqlDbType.Int, 4, link.CategoryID), 
					SqlHelper.MakeInParam("@PostID", SqlDbType.Int, 4, link.PostID), 
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
			return NonQueryBool("blog_DeleteLink",p);

		}

		public override IDataReader GetSingleLink(int linkID)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@LinkID",SqlDbType.Int,4,linkID),
				BlogIDParam
			};
			return GetReader("blog_GetSingleLink",p);
		}

		public override int InsertLink(Link link)
		{
			SqlParameter outParam = SqlHelper.MakeOutParam("@LinkID",SqlDbType.Int,4);
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,link.Title),
				SqlHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,link.Url),
				SqlHelper.MakeInParam("@Rss",SqlDbType.NVarChar,255,DataHelper.CheckNull(link.Rss)),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,link.IsActive),
				SqlHelper.MakeInParam("@NewWindow",SqlDbType.Bit,1,link.NewWindow),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,link.CategoryID),
				SqlHelper.MakeInParam("@PostID",SqlDbType.Int,4,link.PostID),
				BlogIDParam,
				outParam
			};
			NonQueryInt("blog_InsertLink",p);
			return (int)outParam.Value;

		}

		public override bool UpdateLink(Link link)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,link.Title),
				SqlHelper.MakeInParam("@Url",SqlDbType.NVarChar,255,link.Url),
				SqlHelper.MakeInParam("@Rss",SqlDbType.NVarChar,255,DataHelper.CheckNull(link.Rss)),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,link.IsActive),
				SqlHelper.MakeInParam("@NewWindow",SqlDbType.Bit,1,link.NewWindow),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,link.CategoryID),
				SqlHelper.MakeInParam("@LinkID",SqlDbType.Int,4,link.LinkID),
				BlogIDParam
			};
			return NonQueryBool("blog_UpdateLink",p);
		}


		public override IDataReader GetCategories(CategoryType catType, bool ActiveOnly)
		{
			SqlParameter[] p ={SqlHelper.MakeInParam("@CategoryType",SqlDbType.TinyInt,1,catType),
							  SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,ActiveOnly),
								  BlogIDParam};
			return GetReader("blog_GetAllCategories",p);
		}

		//maps to blog_GetActiveCategoriesWithLinkCollection
		public override DataSet GetActiveCategories()
		{
			SqlParameter[] p ={BlogIDParam};
			DataSet ds = SqlHelper.ExecuteDataset(ConnectionString,CommandType.StoredProcedure,"blog_GetActiveCategoriesWithLinkCollection",p);

			DataRelation dl = new DataRelation("CategoryID",ds.Tables[0].Columns["CategoryID"],ds.Tables[1].Columns["CategoryID"],false);
			ds.Relations.Add(dl);

			return ds;
		}

		//maps to blog_GetLinksByCategoryID
		public override IDataReader GetLinksByCategoryID(int catID, bool ActiveOnly)
		{
			string sql = ActiveOnly ? "blog_GetLinksByActiveCategoryID" : "blog_GetLinksByCategoryID";
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4 ,catID),
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
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,CatID),
				BlogIDParam
			};
			return NonQueryBool("blog_DeleteCategory",p);

		}

		//maps to blog_GetCategory
		public override IDataReader GetLinkCategory(int catID, bool IsActive)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,catID),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,IsActive),
				BlogIDParam
			};
			return GetReader("blog_GetCategory",p);
		}
		

		public override IDataReader GetLinkCategory(string categoryName, bool IsActive)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@CategoryName",SqlDbType.NVarChar,150,categoryName),
				SqlHelper.MakeInParam("@IsActive",SqlDbType.Bit,1,IsActive),
				BlogIDParam
			};
			return GetReader("blog_GetCategoryByName",p);
		}

		public override bool UpdateCategory(LinkCategory lc)
		{
			SqlParameter[] p =
			{

				SqlHelper.MakeInParam("@Title",SqlDbType.NVarChar,150,lc.Title),
				SqlHelper.MakeInParam("@Active",SqlDbType.Bit,1,lc.IsActive),
				SqlHelper.MakeInParam("@CategoryID",SqlDbType.Int,4,lc.CategoryID),
				SqlHelper.MakeInParam("@CategoryType",SqlDbType.TinyInt,1,lc.CategoryType),
				SqlHelper.MakeInParam("@Description",SqlDbType.NVarChar,1000,DataHelper.CheckNull(lc.Description)),
				BlogIDParam
			};
			return NonQueryBool("blog_UpdateCategory",p);
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
				SqlHelper.MakeInParam("@Description",SqlDbType.NVarChar,1000,DataHelper.CheckNull(lc.Description)),
				BlogIDParam,
				outParam
			};
			NonQueryInt("blog_InsertCategory",p);
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
				SqlHelper.MakeInParam("@ParentID",SqlDbType.Int,4,PostID),
				BlogIDParam
			};
			return GetReader("blog_GetFeedBack",p);
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
			return NonQueryBool("blog_UTILITY_AddBlog", parameters);
		}

		/// <summary>
		/// Returns a <see cref="IDataReader"/> instance containing 
		/// the configuration settings for the blog specified by the 
		/// Hostname and Application.
		/// </summary>
		/// <param name="host">Hostname.</param>
		/// <param name="application">Application.</param>
		/// <returns></returns>
		public override IDataReader GetBlogInfo(string host, string application)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@Host", SqlDbType.NVarChar, 100, host),
				SqlHelper.MakeInParam("@Application", SqlDbType.NVarChar, 50, application)
			};
			return GetReader("blog_GetConfig", p);
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
			return GetReader("blog_GetConfig", p);
		}

		
		public override IDataReader GetBlogInfo(int BlogID)
		{
			SqlParameter[] p = 
			{
				SqlHelper.MakeInParam("@BlogID",SqlDbType.Int,4,BlogID)
			};
			return GetReader("blog_GetConfigByBlogID",p);
		}

		/// <summary>
		/// Updates the blog configuration in the SQL database 
		/// using the "blog_UpdateConfig" stored proc.
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
					SqlHelper.MakeInParam("@BlogID", SqlDbType.Int,  4, info.BlogID)
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
					,SqlHelper.MakeInParam("@News", SqlDbType.Text, 0, DataHelper.CheckNull(info.News)) 
					,SqlHelper.MakeInParam("@ItemCount", SqlDbType.Int,  4, info.ItemCount) 
					,SqlHelper.MakeInParam("@Flag", SqlDbType.Int,  4, (int)info.Flag) 
					,SqlHelper.MakeInParam("@LastUpdated", SqlDbType.DateTime,  8, info.LastUpdated) 
					,SqlHelper.MakeInParam("@SecondaryCss", SqlDbType.Text, 0, DataHelper.CheckNull(info.Skin.SkinCssText)) 
					,SqlHelper.MakeInParam("@SkinCssFile", SqlDbType.VarChar, 100, DataHelper.CheckNull(info.Skin.SkinCssFile)) 
					,SqlHelper.MakeInParam("@LicenseUrl", SqlDbType.NVarChar, 64, info.LicenseUrl)
					,SqlHelper.MakeInParam("@DaysTillCommentsClose", SqlDbType.Int, 4, daysTillCommentsClose)
					,SqlHelper.MakeInParam("@CommentDelayInMinutes", SqlDbType.Int, 4, commentDelayInMinutes)
				};


			return NonQueryBool("blog_UpdateConfig", p);

		}

		#endregion

		#region Archives

		public override IDataReader GetPostsByMonthArchive()
		{
			SqlParameter[] p = {BlogIDParam};
			return GetReader("blog_GetPostsByMonthArchive",p);
		}

		public override IDataReader GetPostsByYearArchive()
		{
			SqlParameter[] p = {BlogIDParam};
			return GetReader("blog_GetPostsByYearArchive",p);
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
			return GetReader("blog_GetKeyWord",p);
		}

		public override bool DeleteKeyWord(int KeyWordID)
		{
			SqlParameter[] p =
			{
				SqlHelper.MakeInParam("@KeyWordID",SqlDbType.Int,4,KeyWordID),
				BlogIDParam
			};
			return NonQueryBool("blog_DeleteKeyWord",p);
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
			NonQueryInt("blog_InsertKeyWord",p);
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
			return NonQueryBool("blog_UpdateKeyWord",p);
		}



		public override IDataReader GetKeyWords()
		{
			SqlParameter[] p =
			{
				BlogIDParam
			};
			return GetReader("blog_GetBlogKeyWords",p);
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
			return GetReader("blog_GetPageableKeyWords",p);
		}


		#endregion

		#region Helpers

		private IDataReader GetReader(string sql, SqlParameter[] p)
		{
			return SqlHelper.ExecuteReader(ConnectionString,CommandType.StoredProcedure,sql,p);
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
		#endregion
	}
}

