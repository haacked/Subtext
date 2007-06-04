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
using System.Configuration;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;
using Subtext.Installation.Properties;
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
            builder.Title = Resources.Import_dotTextConnectionStringTitle;
            builder.Description = Resources.Import_dotTextConnectionStringDescription;
			
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

			using(SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString))
			{
				connection.Open();
				using(SqlTransaction transaction = connection.BeginTransaction())
				{
					try
					{
						//Set up script parameters...
						ConnectionString subtextConnection = ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString;
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
		/// <param name="control">control used to provide information.</param>
		/// <returns></returns>
		public override string ValidateImportInformation(Control control)
		{
			if(control == null)
                throw new ArgumentNullException("control", Resources.ArgumentNull_Generic);

			string dotTextConnectionString;
			GetConnectionStringsFromControl(control, out dotTextConnectionString);

			if(dotTextConnectionString == null || dotTextConnectionString.Length == 0)
                return Resources.ImportError_InvalidConnectionString;

			try
			{
			    ConnectionString connStr = ConnectionString.Parse(dotTextConnectionString);
                if (!DoesTableExist("blog_config", connStr))
				{
                    return Resources.ImportError_dotTextDatabaseNotFound;
				}

				if (!DoesTableExist("subtext_config", ConfigurationManager.ConnectionStrings["subtextData"].ConnectionString))
				{
                    return Resources.ImportError_SubtextDatabaseNotFound;
				}
			}
			catch(SqlException exception)
			{
                return String.Format(CultureInfo.CurrentUICulture, Resources.ImportError_SqlException, exception.Message);
			}
			catch(ArgumentException)
			{
                return Resources.ImportError_InvalidConnectionStringFormat;
			}

			return string.Empty;
		}

		private static void GetConnectionStringsFromControl(Control populatedControl, out string dotTextConnectionString)
		{
			ConnectionStringBuilder control = ControlHelper.FindControlRecursively(populatedControl, "ctlConnectionStringBuilder") as ConnectionStringBuilder;
			if (control == null)
				throw new InvalidOperationException("Could not find the control 'ctlConnectionStringBuilder'");
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
			string blogContentTableSql = String.Format(CultureInfo.InvariantCulture, TableExistsSql, tableName);

			using (SqlConnection conn = new SqlConnection(connectionString.ToString()))
			{
				conn.Open();
				using(SqlCommand command = new SqlCommand(blogContentTableSql, conn))
				{
					return (int)command.ExecuteScalar();
				}
			}
		}
	}
}
