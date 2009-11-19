using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;

namespace Subtext.Framework.Data
{
    public partial class StoredProcedures
    {
        public StoredProcedures()
        {
        }

        public StoredProcedures(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public StoredProcedures(SqlTransaction transaction)
        {
            _transaction = transaction;
        }

        private SqlTransaction _transaction;

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
            if(_transaction == null)
            {
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, sql, p);
            }
            return SqlHelper.ExecuteNonQuery(_transaction, CommandType.StoredProcedure, sql, p);
        }

        private bool NonQueryBool(string sql, SqlParameter[] p)
        {
            LogSql(sql, p);
            return NonQueryInt(sql, p) > 0;
        }

#if DEBUG
        private readonly static ILog Log = new Log();
#endif

        static void LogSql(string sql, IEnumerable<SqlParameter> parameters)
        {
#if DEBUG
            string query = sql;
            if(parameters != null)
            {
                foreach(SqlParameter parameter in parameters)
                {
                    query += string.Format(" {0}={1},", parameter.ParameterName, parameter.Value);
                }
                if(query.EndsWith(","))
                {
                    query = query.Left(query.Length - 1);
                }
            }

            if(Log.IsDebugEnabled)
            {
                Log.Debug("SQL: " + query);
            }
#endif
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value></value>
        public string ConnectionString { //TODO: Make this protected.
            get; set; }

        /// <summary>
        /// Returns a Data Reader pointing to the entry specified by the entry name.
        /// Only returns entries for the current blog (Config.CurrentBlog).
        /// </summary>
        public virtual IDataReader GetEntryReader(int blogId, string entryName, bool activeOnly, bool includeCategories)
        {
            int? blogIdentifier = (blogId == NullValue.NullInt32 ? null : (int?)blogId);
            return GetSingleEntry(null, entryName, activeOnly, blogIdentifier, includeCategories);
        }

        /// <summary>
        /// Returns a Data Reader pointing to the entry specified by the entry id. 
        /// Only returns entries for the current blog (Config.CurrentBlog).
        /// </summary>
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
            catch(SqlException)
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