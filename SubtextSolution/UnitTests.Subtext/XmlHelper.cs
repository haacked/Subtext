using System;
using System.Text;
using System.Xml;

namespace UnitTests.Subtext
{
	public static class XmlHelper
	{
		/// <summary>
		/// Replaces the value of the node specified by the xPath with the 
		/// newValue.
		/// </summary>
		/// <param name="filename"></param>
		/// <param name="xpath"></param>
		/// <param name="newValue"></param>
		public static void Poke(string filename, string xpath, string newValue)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(filename);
			XmlNode node = doc.SelectSingleNode(xpath);
			if(node != null)
			{
				node.Value = newValue;
			}
			using(XmlWriter writer = new XmlTextWriter(filename, Encoding.UTF8))
			{
				doc.WriteTo(writer);
			}
		}
	}
}
