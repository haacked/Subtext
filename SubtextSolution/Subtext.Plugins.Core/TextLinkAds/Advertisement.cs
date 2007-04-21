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
using System.Xml.Serialization;

namespace Subtext.Plugins.Core.TextLinkAds
{
	[XmlRoot("Link")]
	public class Advertisement
	{
		private string url = string.Empty;

		[XmlElement("URL")]
		public virtual string Url
		{
			get { return url; }
			set
			{
				if (value == null) value = string.Empty;
				url = value;
			}
		}

		private string text = string.Empty;

		[XmlElement]
		public virtual string Text
		{
			get { return text; }
			set
			{
				if (value == null) value = string.Empty;
				text = value;
			}
		}

		private string beforeText = string.Empty;

		[XmlElement]
		public virtual string BeforeText
		{
			get { return beforeText; }
			set
			{
				if (value == null) value = string.Empty;
				beforeText = value;
			}
		}

		private string afterText = string.Empty;

		[XmlElement]
		public virtual string AfterText
		{
			get { return afterText; }
			set
			{
				if (value == null) value = string.Empty;
				afterText = value;
			}
		}

		private string rssText = string.Empty;

		[XmlElement]
		public virtual string RssText
		{
			get { return rssText; }
			set
			{
				if (value == null) value = string.Empty;
				rssText = value;
			}
		}

		private string rssBeforeText = string.Empty;

		[XmlElement]
		public virtual string RssBeforeText
		{
			get { return rssBeforeText; }
			set
			{
				if (value == null) value = string.Empty;
				rssBeforeText = value;
			}
		}

		private string rssAfterText = string.Empty;

		[XmlElement]
		public virtual string RssAfterText
		{
			get { return rssAfterText; }
			set
			{
				if (value == null) value = string.Empty;
				rssAfterText = value;
			}
		}

		private string rssPrefix = string.Empty;

		[XmlElement]
		public virtual string RssPrefix
		{
			get { return rssPrefix; }
			set
			{
				if (value == null) value = string.Empty;
				rssPrefix = value;
			}
		}
	}
}