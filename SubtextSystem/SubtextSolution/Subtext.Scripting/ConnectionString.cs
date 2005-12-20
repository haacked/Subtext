using System;
using System.Text.RegularExpressions;

namespace Subtext.Scripting
{
	/// <summary>
	/// Class for parsing connection strings.  Will add the ability to 
	/// change connection string properties and have the full string work properly.
	/// </summary>
	public class ConnectionString
	{
		string _connectionFormatString = "{0}={1};{2}={3};User ID={4};Password={5};{6}";
		string _databaseFieldName;
		string _serverFieldName;
		string _securityType;
		string _securityTypeText;
		
		/// <summary>
		/// Parses the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <returns></returns>
		public static ConnectionString Parse(string connectionString)
		{
			return new ConnectionString(connectionString);
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="ConnectionString"/> class.
		/// </summary>
		private ConnectionString()
		{
		}

		/// <summary>
		/// Gets the server.
		/// </summary>
		/// <value>The server.</value>
		public string Server
		{
			get
			{
				return _server;
			}
		}

		string _server;

		/// <summary>
		/// Gets or sets the database this connection string connects to.
		/// </summary>
		/// <value>The database.</value>
		public string Database
		{
			get { return _database; }
		}

		string _database;

		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		/// <value>The user id.</value>
		public string UserId
		{
			get { return _userId; }
			set { _userId = value; }
		}

		string _userId;

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password
		{
			get { return _password; }
			set { _password = value; }
		}

		string _password;

		/// <summary>
		/// Gets a value indicating whether [trusted connection].
		/// </summary>
		/// <value><c>true</c> if [trusted connection]; otherwise, <c>false</c>.</value>
		public bool TrustedConnection
		{
			get
			{
				return String.Compare(_securityType, "sspi", true) == 0 
					|| String.Compare(_securityType, "true", true) == 0;
			}
		}

		/// <summary>
		/// Returns a <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </summary>
		/// <returns>
		/// A <see cref="T:System.String"/> that represents the current <see cref="T:System.Object"/>.
		/// </returns>
		public override string ToString()
		{
			return string.Format(_connectionFormatString, _serverFieldName, _server, _databaseFieldName, _database, _userId, _password, _securityTypeText);
		}

		private ConnectionString(string connectionString)
		{
			ParseServer(connectionString);
			ParseDatabase(connectionString);
			ParseUserId(connectionString);
			ParsePassword(connectionString);
			ParseSecurityType(connectionString);
		}

		private bool ParseServer(string connectionString)
		{
			Regex regex = new Regex(@"(?<serverField>Data\s+Source|Server)\s*=\s*(?<server>.*?)(;|$|\s)", RegexOptions.IgnoreCase);
			Match match = regex.Match(connectionString);
			if(match.Success)
			{
				_server = match.Groups["server"].Value;
				_serverFieldName = match.Groups["serverField"].Value;
				return true;
			}

			return false;
		}

		private bool ParseDatabase(string connectionString)
		{
			Regex regex = new Regex(@"(?<databaseField>Database|Initial Catalog)\s*=\s*(?<database>.*?)(;|$|\s)", RegexOptions.IgnoreCase);
			Match match = regex.Match(connectionString);
			if(match.Success)
			{
				_database = match.Groups["database"].Value;
				_databaseFieldName = match.Groups["databaseField"].Value;
				return true;
			}

			return false;
		}

		private bool ParseUserId(string connectionString)
		{
			Regex regex = new Regex(@"User\s+Id\s*=\s*(?<userId>.*?)(;|$|\s)", RegexOptions.IgnoreCase);
			Match match = regex.Match(connectionString);
			if(match.Success)
			{
				_userId = match.Groups["userId"].Value;
				return true;
			}

			return false;
		}

		private bool ParsePassword(string connectionString)
		{
			Regex regex = new Regex(@"Password\s*=\s*(?<password>.*?)(;|$|\s)", RegexOptions.IgnoreCase);
			Match match = regex.Match(connectionString);
			if(match.Success)
			{
				_password = match.Groups["password"].Value;
				return true;
			}

			return false;
		}

		private bool ParseSecurityType(string connectionString)
		{
			Regex regex = new Regex(@"(?<securityTypeField>Integrated\s+Security|Trusted_Connection)\s*=\s*(?<securityType>.*?)(;|$|\s)", RegexOptions.IgnoreCase);
			Match match = regex.Match(connectionString);
			if(match.Success)
			{
				_securityType = match.Groups["securityType"].Value;
				_securityTypeText = match.Groups["securityTypeField"].Value + "=" + _securityType + ";";
				return true;
			}

			return false;
		}
	}
}
