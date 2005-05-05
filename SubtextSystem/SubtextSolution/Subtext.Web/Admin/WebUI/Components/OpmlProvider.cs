#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections;
using System.IO;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;
using Subtext.Framework.Components;

namespace Subtext.Web.Admin
{
	public class OpmlProvider
	{
		protected OpmlProvider()
		{
			//
		}

		public static XmlDocument Export(LinkCollection items)
		{
			#region	DEP: writer		
//			StringWriter sw = new StringWriter();
//
//			XmlWriter writer = new XmlTextWriter(sw);
//
//			writer.WriteStartDocument();
////			writer.WriteAttributeString("encoding", "utf-8");		
//
//			writer.WriteStartElement("opml");
//			writer.WriteElementString("head", String.Empty);
//			writer.WriteStartElement("body");
//
////			foreach (OpmlItem currentItem in items)
////			{				
////				WriteOpmlItem(currentItem, writer);
////			}
//
//			foreach (Link currentItem in items)
//			{				
//				writer.WriteStartElement("outline");
//				writer.WriteAttributeString("title", currentItem.Title);
//				writer.WriteAttributeString("description", currentItem.Title);
//				writer.WriteAttributeString("htmlurl", currentItem.Url);
//				writer.WriteAttributeString("xmlurl", currentItem.Rss);
//				writer.WriteEndElement();
//			}
//
//			writer.WriteEndElement(); // body
//			writer.WriteEndElement(); // opml
//			writer.WriteEndDocument();
//			
//			writer.Close();
			#endregion

			XmlDocument doc = new XmlDocument();
			XmlDeclaration declaration = doc.CreateXmlDeclaration("1.0", "utf-8", null);
			doc.AppendChild(declaration);

			XmlNode rootNode = doc.CreateElement("opml");
			doc.AppendChild(rootNode);
						
			XmlNode headNode = doc.CreateElement("head");
			rootNode.AppendChild(headNode);

			XmlNode bodyNode = doc.CreateElement("body");
			rootNode.AppendChild(bodyNode);
			
			foreach (Link currentItem in items)
			{				
				XmlNode outline = doc.CreateElement("outline");

				XmlAttribute title = doc.CreateAttribute("title");
				title.Value = currentItem.Title;
				outline.Attributes.Append(title);

				XmlAttribute description = doc.CreateAttribute("description");
				description.Value = currentItem.Title;
				outline.Attributes.Append(description);

				XmlAttribute htmlurl = doc.CreateAttribute("htmlurl");
				htmlurl.Value = currentItem.Url;
				outline.Attributes.Append(htmlurl);

				XmlAttribute xmlurl = doc.CreateAttribute("xmlurl");
				xmlurl.Value = currentItem.Rss;
				outline.Attributes.Append(xmlurl);

				bodyNode.AppendChild(outline);
			}			

			return doc;

			//doc.LoadXml(sw.ToString());
			//return doc.CreateNavigator();
		}

		public static void WriteOpmlItem(OpmlItem item, XmlWriter writer)
		{
			item.RenderOpml(writer);
			
			foreach (OpmlItem childItem in item.ChildItems)
				WriteOpmlItem(childItem, writer);
		}

		public static OpmlItemCollection Import(Stream fileStream)
		{
			OpmlItemCollection _currentBatch = new OpmlItemCollection();

			XmlReader reader = new XmlTextReader(fileStream);
			XPathDocument doc = new XPathDocument(reader);
			XPathNavigator nav = doc.CreateNavigator();
			
			XPathNodeIterator outlineItems = nav.Select("/opml/body/outline");

			while (outlineItems.MoveNext())
			{
				_currentBatch.AddRange(DeserializeItem(outlineItems.Current));
			}

			return _currentBatch;
		}

		public static OpmlItem[] DeserializeItem(XPathNavigator nav)
		{
			ArrayList items = new ArrayList();

			if (nav.HasAttributes)
			{
				string title = nav.GetAttribute("title", "");
				if (String.Empty == title)
					title = nav.GetAttribute("text", "");

				string description = nav.GetAttribute("description", "");

				string htmlUrl = nav.GetAttribute("htmlurl", "");
				if (String.Empty == htmlUrl)
					htmlUrl = nav.GetAttribute("htmlUrl", "");

				string xmlUrl = nav.GetAttribute("xmlurl", "");
				if (String.Empty == xmlUrl)
					xmlUrl = nav.GetAttribute("xmlUrl", "");

				OpmlItem currentItem = null;
				if (String.Empty != title && String.Empty != htmlUrl)
					currentItem = new OpmlItem(title, description, xmlUrl, htmlUrl);

				if (null != currentItem)
					items.Add(currentItem);
			}

			if (nav.HasChildren)
			{
				XPathNodeIterator childItems = nav.SelectChildren("outline", "");
				while (childItems.MoveNext())
				{
					OpmlItem[] children = DeserializeItem(childItems.Current);
					if (null != children)
						items.InsertRange(items.Count, children);
				}
			}

			OpmlItem[] result = new OpmlItem[items.Count];
			items.CopyTo(result);
			return result;
		}
	}

	[	
	Serializable,
		XmlRoot(ElementName = "outline", IsNullable=true)
	]
	public class OpmlItem
	{
		private string _title;
		private string _description;
		private string _xmlurl;
		private string _htmlurl;
		private OpmlItemCollection _childItems;

		public OpmlItem()
		{
			_childItems = new OpmlItemCollection(0);
		}

		public OpmlItem(string title, string description, string xmlUrl, string htmlUrl)
			: this()
		{
			_title = title;
			_description = description;
			_xmlurl = xmlUrl;
			_htmlurl = htmlUrl;
		}

		[XmlAttribute("title")]
		public string Title
		{
			get { return _title; }
			set { _title = value; }
		}

		[XmlAttribute("description")]
		public string Description
		{
			get { return _description; }
			set { _description = value; }
		}

		[XmlAttribute("xmlurl")]
		public string XmlUrl
		{
			get { return _xmlurl; }
			set { _xmlurl = value; }
		}

		[XmlAttribute("htmlurl")]
		public string HtmlUrl
		{
			get { return _htmlurl; }
			set { _htmlurl = value; }
		}

		public OpmlItemCollection ChildItems
		{
			get { return _childItems; }
			set { _childItems = value; }
		}

		internal void RenderOpml(XmlWriter writer)
		{
			writer.WriteStartElement("outline");
			writer.WriteAttributeString("title", this.Title);
			writer.WriteAttributeString("description", this.Description);
			writer.WriteAttributeString("htmlurl", this.HtmlUrl);
			writer.WriteAttributeString("xmlurl", this.XmlUrl);
			writer.WriteEndElement();
		}
	}
}

