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
using System.Xml.Serialization;
using System.Xml.XPath;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Util
{
	public class XmlSerializerSectionHandler : IConfigurationSectionHandler 
	{
		public object Create(object parent, object configContext, XmlNode section) 
		{
            if (section == null)
            {
                throw new ArgumentNullException("section", Resources.ArgumentNull_Obj);
            }

			XPathNavigator nav = section.CreateNavigator();
			string typename = (string) nav.Evaluate("string(@type)");
			Type t = Type.GetType(typename);
			XmlSerializer ser = new XmlSerializer(t);
			return ser.Deserialize(new XmlNodeReader(section));
		}

	}

}
