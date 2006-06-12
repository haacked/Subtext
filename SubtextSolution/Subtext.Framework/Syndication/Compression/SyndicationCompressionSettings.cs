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
using System.Xml;
using blowery.Web.HttpCompress;

namespace Subtext.Framework.Syndication.Compression
{
	public class SyndicationCompressionSettings
	{
		private Algorithms _type;
		private CompressionLevels _level;
		private static readonly SyndicationCompressionSettings DefaultSettings = new SyndicationCompressionSettings();
		
		/*-- Constructors --*/

		#region -- Constructor(XmlNode) --
		public SyndicationCompressionSettings(XmlNode node) : this()
		{
			if(node == null)
			{
				return;
			}

			_type = (Algorithms)this.RetrieveEnumFromAttribute(node.Attributes["type"], typeof(Algorithms));
			_level = (CompressionLevels)this.RetrieveEnumFromAttribute(node.Attributes["level"], typeof(CompressionLevels));
		}
		#endregion

		#region -- Constructor() --
		private SyndicationCompressionSettings()
		{
			_type = Algorithms.Deflate;
			_level = CompressionLevels.Normal;
		}
		#endregion

		/*-- Properties --*/

		#region -- CompressionLevel Property --
		public CompressionLevels CompressionLevel
		{
			get
			{
				return _level;
			}
		}
		#endregion

		#region -- CompressionType Property --
		public Algorithms CompressionType
		{
			get
			{
				return _type;
			}
		}
		#endregion

		/*-- Methods --*/

		#region -- RetrieveEnumFromAttribute(XmlAttribute, Type) Method --
		protected Enum RetrieveEnumFromAttribute(XmlAttribute attribute, System.Type type)
		{
			return (Enum)Enum.Parse(type, attribute.Value, true);
		}
		#endregion

		/*-- Static Methods --*/

		#region -- GetSettings() Method --
		public static SyndicationCompressionSettings GetSettings()
		{
			SyndicationCompressionSettings settings;

            settings = (SyndicationCompressionSettings)ConfigurationManager.GetSection("SyndicationCompression");

			if(settings == null)
			{
				settings = SyndicationCompressionSettings.DefaultSettings;
			}
			
			return settings;
		}
		#endregion
	}
}
