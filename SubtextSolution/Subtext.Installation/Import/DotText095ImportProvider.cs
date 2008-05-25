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
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Microsoft.ApplicationBlocks.Data;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Configuration;
using Subtext.Scripting;
using Subtext.Web.Controls;

namespace Subtext.Installation.Import
{
	/// <summary>
	/// Imports blog data from a .TEXT 0.95 database.
	/// </summary>
	public class DotText095ImportProvider : ImportProvider
	{
		/// <summary>
		/// Initializes this import provider.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="configValue">Config value.</param>
        //public override void Initialize(string name, NameValueCollection configValue)
        //{
        //    base.Initialize(name, configValue);
        //}
		/// <summary>
		/// <p>
		/// This method is called by the import engine in order to ask the 
		/// provider what pieces of information it needs from the user in order 
		/// to proceed with the import.
		/// </p>
		/// <p>
		/// This method returns a <see cref="Control"/> used to gather 
		/// the required installation information.  This control will be returned 
		/// back to the provider after the user provides the information.
		/// </p>
		/// </summary>
		/// <returns></returns>
		public override Control GatherImportInformation()
		{
			ConnectionStringBuilder builder = new ConnectionStringBuilder();
			builder.AllowWebConfigOverride = false;
			if(builder.ID == null || builder.ID.Length == 0)
				builder.ID = "ctlConnectionStringBuilder";
			builder.Title = ".TEXT Connection String";
			builder.Description = "A SQL Server Connection String that can connect to and " 
				+ "read from your .TEXT database.";
			
			Panel panel = new Panel();
			panel.Controls.Add(builder);
            
			return panel;
		}

		/// <summary>
		/// Provides the import information as provided by the user back 
		/// into the import provider. 
		/// The control passed in should be the same as that provided in 
		/// <see cref="GatherImportInformation"/>, but with user values 
		/// supplied within it.
		/// </summary>
		/// <param name="populatedControl">Populated control.</param>
		public override void Import(Control populatedControl)
		{
			string dotTextConnectionString;
			GetConnectionStringsFromControl(populatedControl, out dotTextConnectionString);

			using(SqlConnection connection = new SqlConnection(Config.ConnectionString.ToString()))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						//Set up script parameters...
						ConnectionString subtextConnection = Config.ConnectionString;
						ConnectionString dotTextConnection = ConnectionString.Parse(dotTextConnectionString);

						Stream stream = ScriptHelper.UnpackEmbeddedScript("ImportDotText095.sql");
						SqlScriptRunner runner = new SqlScriptRunner(stream, Encoding.UTF8);
						runner.TemplateParameters["subtext_db_name"].Value = subtextConnection.Database;
						runner.TemplateParameters["dottext_db_name"].Value = dotTextConnection.Database;
						
						if(!dotTextConnection.TrustedConnection)
						{
							// We need to determine if we should be using the username we got from 
							// the dotTextConnection, or use the default template value (dbo).

							if (DoesTableExist("blog_Config", dotTextConnection.UserId, dotTextConnection))
							{
								// we have the correct user for the path to the dotText tables
								runner.TemplateParameters["dotTextDbUser"].Value = dotTextConnection.UserId;
							}
						}

						runner.Execute(transaction);
						transaction.Commit();
					}
					catch(Exception)
					{
						transaction.Rollback();
						throw;
					}
				}
			}
		}

		/// <summary>
		/// Validates the installation information provided by the user.  
		/// Returns a string with an explanation of why it is incorrect.
		/// </summary>
		/// <param name="populatedControl">control used to provide information.</param>
		/// <returns></returns>
		public override string ValidateImportInformation(Control populatedControl)
		{
            if (populatedControl == null)
            {
                throw new ArgumentNullException("populatedControl", "Hello, sorry, but we really can't validate a null control.");
            }

			string dotTextConnectionString;
			GetConnectionStringsFromControl(populatedControl, out dotTextConnectionString);

            if ( !String.IsNullOrEmpty(dotTextConnectionString) )
            {
                return "Please specify a valid connection string to the .TEXT 0.95 database.";
            }

			try
			{
			    ConnectionString connStr = ConnectionString.Parse(dotTextConnectionString);
                if (!DoesTableExist("blog_config", connStr))
				{
					return "I&#8217;m sorry, but it does not appear that " 
						+ "there is a .TEXT database corresponding to the connection string provided. " 
						+ "Please double check that the &#8220;blog_config&#8221; table exists.  If it does, " 
						+ "double check that it was created using the [dbo] account OR by the same user " 
						+ "specified in the .TEXT connection string below.";
				}

				if(!DoesTableExist("subtext_config", Config.ConnectionString))
				{
				    return "I&#8217;m sorry, but it does not appear that " 
						+ "there is a Subtext database corresponding to the connection string within web.config. " 
						+ "Please double check that the &#8220;subtext_config&#8221; table exists.  If it does, " 
						+ "double check that it was created using the [dbo] account OR by the same user " 
						+ "specified in the Subtext connection string.";
				}
			}
			catch(SqlException exception)
			{
				return "There was an error while trying to connect to the database.  The error is &#8220;" + exception.Message + "&#8221;";
			}
			catch(ArgumentException)
			{
				return "The format for the connection string is incorrect. Please double check it and try again.";
			}

			return string.Empty;
		}

		private static void GetConnectionStringsFromControl(Control populatedControl, out string dotTextConnectionString)
		{
			ConnectionStringBuilder control = populatedControl.FindControl("ctlConnectionStringBuilder") as ConnectionStringBuilder;
			dotTextConnectionString = control.ConnectionString;
		}

		static bool DoesTableExist(string tableName, string ownerName, ConnectionString connectionString)
		{	
			return DoesTableExist(ownerName+"."+tableName, connectionString);
		}

		static bool DoesTableExist(string tableName, ConnectionString connectionString)
		{
            return 0 < GetTableCount(tableName, connectionString);
		}

		static int GetTableCount(string tableName, ConnectionString connectionString)
		{
            const string TableExistsSql = "SELECT COUNT(1) FROM [INFORMATION_SCHEMA].[TABLES] WHERE [TABLE_TYPE]='BASE TABLE' AND [TABLE_NAME]='{0}'";
			string blogContentTableSql = String.Format(TableExistsSql, tableName);			
			return (int)SqlHelper.ExecuteScalar(connectionString.ToString(), CommandType.Text, blogContentTableSql);
		}
	}
}
