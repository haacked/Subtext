using System;
using System.Xml.Serialization;

namespace Subtext.Framework.Providers
{
	/// <summary>
	/// Summary description for ConfigProvider.
	/// </summary>
	[XmlRoot("ConfigProvider")]
	public class ConfigProviderConfiguration : BaseProvider 
	{
		public ConfigProviderConfiguration(){}

		private string _host;
		[XmlAttribute("host")]
		public string Host
		{
			get {return this._host;}
			set {this._host = value;}
		}

		private string _application;
		[XmlAttribute("application")]
		public string Application
		{
			get {return this._application;}
			set {this._application = value;}
		}

		private string _imageDirectory;
		[XmlAttribute("imageDirectory")]
		public string ImageDirectory
		{
			get {return this._imageDirectory;}
			set {this._imageDirectory = value;}
		}

		private int _blogID;
		[XmlAttribute("blogID")]
		public int BlogID
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private int _cacheTime = 120;
		[XmlAttribute("cacheTime")]
		public int CacheTime
		{
			get {return this._cacheTime;}
			set {this._cacheTime = value;}
		}


	}
}
