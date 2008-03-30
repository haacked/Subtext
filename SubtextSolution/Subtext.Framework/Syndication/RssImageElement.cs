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
		public RssImageElement(Uri url, string title, Uri link) : this(url, title, link, null, null, null)
		{
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="RssImageElement"/> class.
		/// </summary>
		/// <param name="url">The URL.</param>
		/// <param name="title">The title.</param>
		/// <param name="link">The link.</param>
		/// <param name="width">The width.</param>
		/// <param name="height">The height.</param>
		/// <param name="description">The description.</param>
		public RssImageElement(Uri url, string title, Uri link, int? width, int? height, string description)
		{
			this.url = url;
			this.title = HtmlHelper.RemoveHtml(title);
			this.link = link;
			this.width = width;
			this.height = height;
			this.description = description;
		}

		/// <summary>
		/// The URL of a GIF, JPEG or PNG image that represents the channel. 
		/// </summary>
		/// <value>The URL.</value>
		public Uri Url
		{
			get { return this.url; }
		}

		Uri url;

		/// <summary>
		/// Describes the image, it's used in the ALT attribute of the HTML &lt;img&gt; tag when the channel is rendered in HTML.
		/// </summary>
		/// <value>The title.</value>
		public string Title
		{
			get { return this.title; }
		}

		string title;

		/// <summary>
		/// The URL of the site, when the channel is rendered, the image is a link to the site. 
		/// (Note, in practice the image &lt;title&gt; and &lt;link&gt; should have the same value as the channel's &lt;title&gt; and &lt;link&gt;.
		/// </summary>
		/// <value>The link.</value>
		public Uri Link
		{
			get { return this.link; }
		}

		Uri link;

		/// <summary>
		/// Gets the width.
		/// </summary>
		/// <value>The width.</value>
		public int? Width
		{
			get { return this.width; }
		}

		int? width;

		/// <summary>
		/// Gets the height.
		/// </summary>
		/// <value>The height.</value>
		public int? Height
		{
			get { return this.Height; }
		}

		int? height;

		/// <summary>
		/// Gets the description.
		/// </summary>
		/// <value>The description.</value>
		public string Description
		{
			get { return this.description; }
		}

		string description;
		
		/// <summary>
		/// Writes this RssImage element to the specified XmlWriter.
		/// </summary>
		/// <param name="writer"></param>
		public void WriteToXmlWriter(XmlWriter writer)
		{
			writer.WriteStartElement("image");
			writer.WriteElementString("title", title);
			writer.WriteElementString("url", this.url.ToString());
			writer.WriteElementString("link", this.link.ToString());
			
			if (this.width != null)
				writer.WriteElementString("width", this.width.Value.ToString(CultureInfo.InvariantCulture));
			
			if(this.height != null)
				writer.WriteElementString("height", this.height.Value.ToString(CultureInfo.InvariantCulture));
			
			if(description != null)
				writer.WriteElementString("description", description); //Used in the alt tag.
			writer.WriteEndElement();
		}
	}
}
