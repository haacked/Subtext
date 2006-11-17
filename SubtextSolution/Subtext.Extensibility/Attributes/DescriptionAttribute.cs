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

namespace Subtext.Extensibility.Attributes
{
	[System.AttributeUsage(System.AttributeTargets.Class)]
	public sealed class DescriptionAttribute: Attribute
	{
		public DescriptionAttribute(string name)
		{
			_name = name;
		}

		private readonly string _name;

		public string Name
		{
			get { return _name; }
		}

		private string _author;

		public string Author
		{
			get { return _author; }
			set { _author = value; }
		}

		private string _company;

		public string Company
		{
			get { return _company; }
			set { _company = value; }
		}

		private string _version;

		public string Version
		{
			get { return _version; }
			set { _version = value; }
		}

		private string _copyright;

		public string Copyright
		{
			get { return _copyright; }
			set { _copyright = value; }
		}

		private string _description;

		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		private string _homePageUrl;

		public string HomePageUrl
		{
			get { return _homePageUrl; }
			set { _homePageUrl = value; }
		}


	}
}
