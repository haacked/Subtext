#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using log4net;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Data
{
    public partial class StoredProcedures
    {
        private readonly static ILog Log = new Log();
        
        public StoredProcedures(string connectionString)
        {
            ConnectionString = connectionString;
        }

        public StoredProcedures(SqlTransaction transaction)
        {
            _transaction = transaction;
        }

        private readonly SqlTransaction _transaction;

        private IDataReader GetReader(string sql)
        {
            return ExecuteQueryAndLogError((sqlStatement, sqlParams) =>
                SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, sqlStatement), sql, null);

        }

        private IDataReader GetReader(string sql, SqlParameter[] parameters)
        {
            return ExecuteQueryAndLogError((sqlStatement, sqlParams) => 
                SqlHelper.ExecuteReader(ConnectionString, CommandType.StoredProcedure, sqlStatement, sqlParams), sql, parameters);
        }

        private int NonQueryInt(string sql, SqlParameter[] parameters)
        {
            var transaction = _transaction;
            return ExecuteQueryAndLogError((sqlStatement, sqlParams) => 
            {
                if(transaction != null)
                {
                    return SqlHelper.ExecuteNonQuery(transaction, CommandType.StoredProcedure, sqlStatement, sqlParams);
                }
                return SqlHelper.ExecuteNonQuery(ConnectionString, CommandType.StoredProcedure, sqlStatement, sqlParams);
            }, sql, parameters);
        }

        private static TResult ExecuteQueryAndLogError<TResult>(Func<string, SqlParameter[], TResult> query, string sql, SqlParameter[] parameters)
        {
            try
            {
                return query(sql, parameters);
            }
            catch(Exception)
            {
                LogSqlStatement(sql, parameters);
                throw; // Let the caller determine how to handle the exception.
            }
        }

        private static void LogSqlStatement(string sql, IEnumerable<SqlParameter> parameters)
        {
            string sqlStatement = sql;
            if(parameters != null)
            {
                sqlStatement += " ";
                foreach(var parameter in parameters)
                {
                    sqlStatement += parameter.ParameterName + "=" + parameter.Value + ", ";
                }
            }
            Log.Error("Error executing SQL: " + sqlStatement);
        }

        private bool NonQueryBool(string sql, SqlParameter[] p)
        {
            return NonQueryInt(sql, p) > 0;
        }

        /// <summary>
        /// Gets or sets the connection string.
        /// </summary>
        /// <value></value>
        protected string ConnectionString {
            get; 
            set;
        }

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