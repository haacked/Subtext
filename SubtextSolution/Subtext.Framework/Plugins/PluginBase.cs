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
using Subtext.Extensibility.Attributes;
using System.Reflection;

namespace Subtext.Extensibility.Plugins
{
	public abstract class PluginBase
	{
		/// <summary>
		/// Initialize the plugin.<br />
		/// This is the only method that must be overridden since all actions are performed inside Event Handlers<br />
		/// The implementation of this method should only subscribe to the events raised by the SubtextApplication
		/// </summary>
		public abstract void Init(SubtextApplication application);

		private Guid _guid;

		/// <summary>
		/// Identifier of the plugin. This value has to be unique, so this interface provides a Guid.
		/// <remarks>It retrieves the value from the Identifier attribute</remarks>
		/// </summary>
		public Guid Id
		{
			get
			{
				if (_guid == Guid.Empty)
				{
					_guid=GetGuidFromAttribute(this.GetType());
				}
				return _guid;
			}
		}

		private PluginImplementationInfo _info;

		/// <summary>
		/// Provides information on the plugin: the author, the company, an url, a description, etc...
		/// <remarks>It retrieves the value from the Description attribute</remarks>
		/// </summary>
		public PluginImplementationInfo Info
		{
			get
			{
				if (_info == null)
				{
					_info = GetInfoFromAttribute(this.GetType());
				}
				return _info;
			}
		}

		private PluginDefaultSettingsCollection _defaultSettings;

		/// <summary>
		/// Sitewide settings for the plugin
		/// </summary>
		public PluginDefaultSettingsCollection DefaultSettings
		{
			get { return _defaultSettings; }
			internal set { _defaultSettings = value; }
		}


		#region Attribute Accessor Helpers
		private PluginImplementationInfo GetInfoFromAttribute(Type type)
		{
			Attribute[] attrs = System.Attribute.GetCustomAttributes(type);
			foreach (Attribute attr in attrs)
			{
				if (attr is DescriptionAttribute)
				{
					DescriptionAttribute descAttr = (DescriptionAttribute)attr;
					PluginImplementationInfo info = new PluginImplementationInfo();

					info._name = descAttr.Name;
					info._author = descAttr.Author;
					info._company = descAttr.Company;
					info._copyright = descAttr.Copyright;
					info._description = descAttr.Description;
					info._homepageUrl = new Uri(descAttr.HomePageUrl);
					info._version = new Version(descAttr.Version);
					return info;
				}
			}
			return null;
		}

		private Guid GetGuidFromAttribute(Type type)
		{
			Attribute[] attrs = System.Attribute.GetCustomAttributes(type);
			foreach (Attribute attr in attrs)
			{
				if (attr is IdentifierAttribute)
				{
					IdentifierAttribute idAttr = (IdentifierAttribute)attr;
					return idAttr.Guid;
				}
			}
			return Guid.Empty;
		} 
		#endregion

		public class PluginImplementationInfo
		{
			internal string _name;
			public string Name
			{
				get { return _name; }
			}

			internal string _author;
			public string Author
			{
				get { return _author; }
			}

			internal string _company;
			public string Company
			{
				get { return _company; }
			}

			internal string _copyright;
			public string Copyright
			{
				get { return _copyright; }
			}

			internal string _description;
			public string Description
			{
				get { return _description; }
			}

			internal Uri _homepageUrl;
			public Uri HomepageUrl
			{
				get { return _homepageUrl; }
			}

			internal Version _version;
			public Version Version
			{
				get { return _version; }
			}
		}

	}


}
