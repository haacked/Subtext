using System;
using System.Xml.Serialization;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Contains configuration information for the database provider.  
	/// Simply a Connection string.
	/// </summary>
	[XmlRoot("DbProvider")]
	public class DbProviderConfiguration : BaseProvider 
	{
		private string _connectionString;
		/// <summary>
		/// Gets or sets the connection string.
		/// </summary>
		/// <value></value>
		[XmlAttribute("connectionString")]
		public string ConnectionString
		{
			get {return this._connectionString;}
			set {this._connectionString = value;}
		}

	}
}
