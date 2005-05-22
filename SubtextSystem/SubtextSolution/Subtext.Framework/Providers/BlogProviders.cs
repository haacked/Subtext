using System;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Summary description for BlogProviders.
	/// </summary>
	[Serializable]
	public class BlogProviders
	{
		private EmailProviderConfiguration _emailProvider;
		public EmailProviderConfiguration EmailProvider
		{
			get {return this._emailProvider;}
			set {this._emailProvider = value;}
		}

		private DTOProviderConfiguration _dTOProvider;
		public DTOProviderConfiguration DTOProvider
		{
			get {return this._dTOProvider;}
			set {this._dTOProvider = value;}
		}

		private DbProviderConfiguration _dbProvider;
		public DbProviderConfiguration DbProvider
		{
			get {return this._dbProvider;}
			set {this._dbProvider = value;}
		}

		private ConfigProviderConfiguration _configProvider;
		public ConfigProviderConfiguration ConfigProvider
		{
			get {return this._configProvider;}
			set {this._configProvider = value;}
		}

		private UrlFormatProviderConfiguration _urlFormatProvider;
		public UrlFormatProviderConfiguration UrlFormatProvider
		{
			get {return this._urlFormatProvider;}
			set {this._urlFormatProvider = value;}
		}
	}
}
