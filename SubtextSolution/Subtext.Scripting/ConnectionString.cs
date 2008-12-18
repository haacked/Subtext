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
using System.Text.RegularExpressions;

namespace Subtext.Scripting
{
    //TODO: We screw up SQL EXpress connection strings. We need to fix that up.

	/// <summary>
	/// Class for parsing connection strings.  Will add the ability to 
	/// change connection string properties and have the full string work properly.
	/// </summary>
	[Serializable]
	public class ConnectionString
	{
		private static readonly ConnectionString _emptyConnectionString = new ConnectionString();

		readonly string _connectionFormatString = "{0}={1};{2}={3};User ID={4};Password={5};{6}";
		readonly string _trustedConnectionFormatString = "{0}={1};{2}={3};{4}";
		string _databaseFieldName="Database";
		string _serverFieldName="Server";
		string _securityType;
		string _securityTypeText;
		
		/// <summary>
		/// Return an empty instance of connection string
		/// </summary>
		public static ConnectionString Empty
		{
			get {return _emptyConnectionString;}
		}
		
		/// <summary>
		/// Parses the specified connection string.
		/// </summary>
		/// <param name="connectionString">The connection string.</param>
		/// <returns></returns>
		public static ConnectionString Parse(string connectionString)
		{
			return new ConnectionString(connectionString);
		}

		public static implicit operator string(ConnectionString connectionString)
		{
			return connectionString.ToString();
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
			get;
			set;
		}


		/// <summary>
		/// Gets or sets the database this connection string connects to.
		/// </summary>
		/// <value>The database.</value>
		public string Database
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the user id.
		/// </summary>
		/// <value>The user id.</value>
		public string UserId
		{
			get;
			set;
		}

		/// <summary>
		/// Gets or sets the password.
		/// </summary>
		/// <value>The password.</value>
		public string Password
		{
			get;
			set;
		}

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

			set
			{
				if(value)
				{
					_securityType="true";
					_securityTypeText="Trusted_Connection=true";
				}
				else _securityType=_securityTypeText=String.Empty;
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
			if(TrustedConnection)
				return string.Format(_trustedConnectionFormatString, _serverFieldName, Server, _databaseFieldName, Database, _securityTypeText);
			else
				return string.Format(_connectionFormatString, _serverFieldName, Server, _databaseFieldName, Database, UserId, Password, _securityTypeText);
		}

		private ConnectionString(string connectionString)
		{
            RawOriginal = connectionString;
			ParseServer(connectionString);
			ParseDatabase(connectionString);
			ParseUserId(connectionString);
			ParsePassword(connectionString);
			ParseSecurityType(connectionString);
		}

        public string RawOriginal
        {
            get;
            private set;
        }

		private bool ParseServer(string connectionString)
		{
			Regex regex = new Regex(@"(?<serverField>Data\s+Source|Server)\s*=\s*(?<server>.*?)(;|$|\s)", RegexOptions.IgnoreCase);
			Match match = regex.Match(connectionString);
			if(match.Success)
			{
				Server = match.Groups["server"].Value;
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
				Database = match.Groups["database"].Value;
				_databaseFieldName = match.Groups["databaseField"].Value;
                if(!String.IsNullOrEmpty(Database))
                    return true;
			}

            if (String.IsNullOrEmpty(Database))
            {
                regex = new Regex(@"AttachDbFilename\s*=\s*\|DataDirectory\|\\(?<database>.*?)(;|$|\s)", RegexOptions.IgnoreCase);
                match = regex.Match(connectionString);
                if (match.Success)
                {
                    Database = match.Groups["database"].Value;
                    if (!String.IsNullOrEmpty(Database))
                        return true;
                }
            }

			return false;
		}

		private bool ParseUserId(string connectionString)
		{
			Regex regex = new Regex(@"User\s+Id\s*=\s*(?<userId>.*?)(;|$|\s)", RegexOptions.IgnoreCase);
			Match match = regex.Match(connectionString);
			if(match.Success)
			{
				UserId = match.Groups["userId"].Value;
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
				Password = match.Groups["password"].Value;
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

		/// <summary>
		/// Implicitly converts the string to a connection string.
		/// </summary>
		/// <remarks>
		/// Got the idea from here.  
		/// http://developer810.blogspot.com/2006/02/good-way-to-create-custom-value-type.html
		/// It's not as clear to me as doing .Parse... I'll think about this one.
		/// </remarks>
		/// <param name="connectionString">The state.</param>
		/// <returns></returns>
		public static implicit operator ConnectionString(string connectionString)
		{
			return new ConnectionString(connectionString);
		}
	}
}
