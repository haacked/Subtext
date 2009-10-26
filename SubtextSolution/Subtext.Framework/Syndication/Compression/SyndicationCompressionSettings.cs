#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
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
        private static readonly SyndicationCompressionSettings DefaultSettings = new SyndicationCompressionSettings();
        private readonly CompressionLevels _level;
        private readonly Algorithms _type;

        #region -- Constructor(XmlNode) --

        public SyndicationCompressionSettings(XmlNode node) : this()
        {
            if(node == null)
            {
                return;
            }

            _type = (Algorithms)RetrieveEnumFromAttribute(node.Attributes["type"], typeof(Algorithms));
            _level = (CompressionLevels)RetrieveEnumFromAttribute(node.Attributes["level"], typeof(CompressionLevels));
        }

        #endregion

        #region -- Constructor() --

        private SyndicationCompressionSettings()
        {
            _type = Algorithms.Deflate;
            _level = CompressionLevels.Normal;
        }

        #endregion

        #region -- CompressionLevel Property --

        public CompressionLevels CompressionLevel
        {
            get { return _level; }
        }

        #endregion

        #region -- CompressionType Property --

        public Algorithms CompressionType
        {
            get { return _type; }
        }

        #endregion

        #region -- RetrieveEnumFromAttribute(XmlAttribute, Type) Method --

        protected Enum RetrieveEnumFromAttribute(XmlAttribute attribute, Type type)
        {
            return (Enum)Enum.Parse(type, attribute.Value, true);
        }

        #endregion

        #region -- GetSettings() Method --

        public static SyndicationCompressionSettings GetSettings()
        {
            SyndicationCompressionSettings settings;

            settings = (SyndicationCompressionSettings)ConfigurationManager.GetSection("SyndicationCompression");

            if(settings == null)
            {
                settings = DefaultSettings;
            }

            return settings;
        }

        #endregion

        /*-- Constructors --*/

        /*-- Properties --*/

        /*-- Methods --*/

        /*-- Static Methods --*/
    }
}