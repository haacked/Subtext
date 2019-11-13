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

using System.Xml;

namespace SubtextUpgrader
{
    public static class XmlExtensions
    {
        public static XmlNode SelectConnectionStringsNode(this XmlNode node)
        {
            return node.SelectSingleNode("/configuration/connectionStrings");
        }

        public static XmlNode SelectFriendlyUrlSettingsNode(this XmlNode node)
        {
            return node.SelectSingleNode("/configuration/FriendlyUrlSettings");
        }

        public static XmlNode SelectEnclosureMimetypesNode(this XmlNode node)
        {
            return node.SelectSingleNode("/configuration/EnclosureMimetypes");
        }
        
        public static XmlNode SelectEmailNode(this XmlNode node)
        {
            return node.SelectSingleNode("/configuration/Email");
        }
    }
}
