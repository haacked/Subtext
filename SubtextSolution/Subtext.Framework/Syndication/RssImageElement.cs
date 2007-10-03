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
using System.Globalization;
using System.Xml;
using Subtext.Framework.Text;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// &lt;image&gt; is an optional sub-element of &lt;channel&gt;, which contains three required and three optional sub-elements.
	/// </summary>
	public class RssImageElement
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="RssImageElement"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="title">The title.</param>
		/// <param name="link">The link.</param>
		public RssImageElement(Uri url, string title, Uri link)
			: this(url, title, link, null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RssImageElement"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="title">The _title.</param>
		/// <param name="link">The _link.</param>
		/// <param name="width">The _width.</param>
		/// <param name="height">The _height.</param>
		/// <param name="description">The _description.</param>
		public RssImageElement(Uri url, string title, Uri link, int? width, int? height, string description)
		{
			_url = url;
			_title = HtmlHelper.RemoveHtml(title);
			_link = link;
			_width = width;
			_height = height;
			_description = description;
		}
        readonly Uri _url;

		/// <summary>
		/// The URL of a GIF, JPEG or PNG image that represents the channel. 
		/// </summary>
		/// <value>The URL.</value>
		public Uri Url
		{
			get { return _url; }
		}
        readonly string _title;

        /// <summary>
		/// Describes the image, it's used in the ALT attribute of the HTML &lt;img&gt; tag when the channel is rendered in HTML.
		/// </summary>
		/// <value>The _title.</value>
		public string Title
		{
			get { return _title; }
		}
        readonly Uri _link;

        /// <summary>
		/// The URL of the site, when the channel is rendered, the image is a _link to the site. 
		/// (Note, in practice the image &lt;_title&gt; and &lt;_link&gt; should have the same value as the channel's &lt;_title&gt; and &lt;_link&gt;.
		/// </summary>
		/// <value>The _link.</value>
		public Uri Link
		{
			get { return _link; }
		}
        int? _width;

		/// <summary>
		/// Gets the _width.
		/// </summary>
		/// <value>The _width.</value>
		public int? Width
		{
			get { return _width; }
		}

        int? _height;
		/// <summary>
		/// Gets the _height.
		/// </summary>
		/// <value>The _height.</value>
		public int? Height
		{
			get { return _height; }
		}

        readonly string _description;
		/// <summary>
		/// Gets the _description.
		/// </summary>
		/// <value>The _description.</value>
		public string Description
		{
			get { return _description; }
		}

		/// <summary>
		/// Writes this RssImage element to the specified XmlWriter.
		/// </summary>
		/// <param name="writer"></param>
		public void WriteToXmlWriter(XmlWriter writer)
		{
			writer.WriteStartElement("image");
			writer.WriteElementString("title", _title);
			writer.WriteElementString("url", _url.ToString());
			writer.WriteElementString("link", _link.ToString());

			if (_width != null)
				writer.WriteElementString("width", _width.Value.ToString(CultureInfo.InvariantCulture));

			if (_height != null)
				writer.WriteElementString("height", _height.Value.ToString(CultureInfo.InvariantCulture));

			if (_description != null)
				writer.WriteElementString("description", _description); //Used in the alt tag.
			writer.WriteEndElement();
		}
	}
}
