using System.Data;
using System.Data.SqlClient;
using log4net;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Data
{
    public partial class StoredProcedures
    {
        public StoredProcedures() { 
        }

        public StoredProcedures(string connectionString) {
            ConnectionString = connectionString;
        }

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

#if DEBUG
        private readonly static ILog Log = new Log();
#endif

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
                    query = query.Left(query.Length - 1);
            }

            Log.Debug("SQL: " + query);
#endif
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value></value>
        public string ConnectionString
        {
            //TODO: Make this protected.
            get;
            set;
        }

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

        /// <summary>
        /// Returns a Data Reader pointing to the entry specified by the entry name.
        /// Only returns entries for the current blog (Config.CurrentBlog).
        /// </summary>
        /// <param name="entryName">Url friendly entry name.</param>
        /// <param name="activeOnly"></param>
        /// <param name="includeCategories"></param>
        /// <returns></returns>
        public virtual IDataReader GetEntryReader(int blogId, string entryName, bool activeOnly, bool includeCategories)
        {
            int? blogIdentifier = (blogId == NullValue.NullInt32 ? null : (int?)blogId);
            return GetSingleEntry(null, entryName, activeOnly, blogIdentifier, includeCategories);
        }

        /// <summary>
        /// Returns a Data Reader pointing to the entry specified by the entry id. 
        /// Only returns entries for the current blog (Config.CurrentBlog).
        /// </summary>
        /// <param name="id"></param>
        /// <param name="activeOnly"></param>
        /// <param name="includeCategories"></param>
        /// <returns></returns>
        public virtual IDataReader GetEntryReader(int blogId, int id, bool activeOnly, bool includeCategories)
        {
            int? blogIdentifier = (blogId == NullValue.NullInt32 ? null : (int?)blogId);
            return GetSingleEntry(id, null, activeOnly, blogIdentifier, includeCategories);
        }

        /// <summary>
        /// Returns a Data Reader pointing to the active entry specified by the entry id no matter 
        /// which blog it belongs to.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="includeCategories"></param>
        /// <returns></returns>
        public virtual IDataReader GetEntryReader(int id, bool includeCategories)
        {
            return GetSingleEntry(id, null, true, null, includeCategories);
        }

        /// <summary>
        /// Returns a list of all the blogs within the specified range.
        /// </summary>
        /// <param name="host">The hostname for this blog.</param>
        /// <param name="pageIndex">Page index.</param>
        /// <param name="pageSize">Size of the page.</param>
        /// <param name="flags">Flags for type of retrieval.</param>
        /// <returns></returns>
        public virtual IDataReader GetPagedBlogs(string host, int pageIndex, int pageSize, ConfigurationFlags flags)
        {
            try
            {
                return GetPageableBlogs(pageIndex, pageSize, host, (int)flags);

            }
            catch (SqlException)
            {
                SqlParameter[] p = {
				    DataHelper.MakeInParam("@PageIndex", pageIndex),		
				    DataHelper.MakeInParam("@PageSize", pageSize),		
				    DataHelper.MakeInParam("@SortDesc", 0),
                };
                return GetReader("subtext_GetPageableBlogs", p);
			}
        }
    }
}
