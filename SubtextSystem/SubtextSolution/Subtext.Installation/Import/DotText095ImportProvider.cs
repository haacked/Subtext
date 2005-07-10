using System;
using System.Collections.Specialized;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;
using Subtext.Web.Controls;

namespace Subtext.Installation.Import
{
	/// <summary>
	/// Imports blog data from a .TEXT 0.95 database.
	/// </summary>
	public class DotText095ImportProvider : ImportProvider
	{
		string _subtextConnectionString = null;
		string _dotTextConnectionString = null;

		/// <summary>
		/// Initializes this import provider.
		/// </summary>
		/// <param name="name">Name.</param>
		/// <param name="configValue">Config value.</param>
		public override void Initialize(string name, NameValueCollection configValue)
		{
			_subtextConnectionString = GetSettingValue("connectionString", configValue);
		}

		/// <summary>
		/// Imports data into this instance.
		/// </summary>
		/// <returns></returns>
		public override bool Import()
		{
			return false;
		}

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
			if(builder.ID == null || builder.ID.Length == 0)
				builder.ID = "ctlConnectionStringBuilder";
			builder.Title = ".TEXT Connection String";
			builder.Description = "A SQL Server Connection String that can connect to and read from your .TEXT database.";

			ConnectionStringBuilder subtextBuilder = new ConnectionStringBuilder();
			if(subtextBuilder.ID == null || subtextBuilder.ID.Length == 0)
				subtextBuilder.ID = "ctlSubtextConnectionStringBuilder";
			subtextBuilder.Title = "Subtext Connection String";
			subtextBuilder.Description = "A SQL Server Connection String that can connect to your Subtext database and has permissions to directly DELETE and INSERT records.";
			
			Panel panel = new Panel();
			panel.Controls.Add(builder);
			panel.Controls.Add(subtextBuilder);
            
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
		public override void ProvideImportInformation(Control populatedControl)
		{
			ConnectionStringBuilder control = populatedControl as ConnectionStringBuilder;
			if(control != null)
			{
				_dotTextConnectionString = control.ConnectionString;
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
			if(populatedControl == null)
				throw new ArgumentNullException("populatedControl", "Hello, sorry, but we really can't validate a null control.");

			ConnectionStringBuilder control = populatedControl.FindControl("ctlConnectionStringBuilder") as ConnectionStringBuilder;
			_dotTextConnectionString = control.ConnectionString;

			control = populatedControl.FindControl("ctlSubtextConnectionStringBuilder") as ConnectionStringBuilder;
			_subtextConnectionString = control.ConnectionString;

			if(_dotTextConnectionString == null || _dotTextConnectionString.Length == 0)
				return "Please specify a valid connection string to the .TEXT 0.95 database.";

			if(_subtextConnectionString == null || _subtextConnectionString.Length == 0)
				return "Please specify a valid connection string to the Subtext database.";

			try
			{
				if(!DoesTableExist("blog_config"))
				{
					string errorMessage = "I&#8217;m sorry, but it does not appear that " 
						+ "there is a .TEXT database corresponding to the connection string provided. " 
						+ "Please double check that the &#8220;blog_config&#8221; table exists.  If it does, " 
						+ "double check that it was created using the [dbo] account OR by the same user " 
						+ "specified in the connection string you provided.";
					return errorMessage;
				}

				if(!DoesTableExist("subtext_config"))
				{
					string errorMessage = "I&#8217;m sorry, but it does not appear that " 
						+ "there is a Subtext database corresponding to the connection string provided. " 
						+ "Please double check that the &#8220;subtext_config&#8221; table exists.  If it does, " 
						+ "double check that it was created using the [dbo] account OR by the same user " 
						+ "specified in the connection string you provided.";
					return errorMessage;
				}
			}
			catch(SqlException exception)
			{
				return "There was an error while trying to connect to the database.  The error is &#8220;" + exception.Message + "&#8221;";
			}
			catch(System.ArgumentException)
			{
				return "The format for the connection string is incorrect. Please double check it and try again.";
			}

			return string.Empty;
		}

		bool DoesTableExist(string tableName)
		{
			return 0 < GetTableCount(tableName);
		}

		int GetTableCount(string tableName)
		{
			const string TableExistsSql = "SELECT COUNT(1) FROM dbo.sysobjects WHERE id = object_id(N'[{0}]') and OBJECTPROPERTY(id, N'IsUserTable') = 1";
			string blogContentTableSql = String.Format(TableExistsSql, tableName);			
			return (int)SqlHelper.ExecuteScalar(this._dotTextConnectionString, CommandType.Text, blogContentTableSql);
		}
	}
}
