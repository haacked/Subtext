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
        readonly string _description;
        int? _height;
        readonly Uri _link;
        readonly string _title;
        readonly Uri _url;
        int? _width;

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
        /// <param name="title">The title.</param>
        /// <param name="link">The link.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="description">The description.</param>
        public RssImageElement(Uri url, string title, Uri link, int? width, int? height, string description)
        {
            _url = url;
            _title = HtmlHelper.RemoveHtml(title);
            _link = link;
            _width = width;
            _height = height;
            _description = description;
        }

        /// <summary>
        /// The URL of a GIF, JPEG or PNG image that represents the channel. 
        /// </summary>
        /// <value>The URL.</value>
        public Uri Url
        {
            get { return _url; }
        }

        /// <summary>
        /// Describes the image, it's used in the ALT attribute of the HTML &lt;img&gt; tag when the channel is rendered in HTML.
        /// </summary>
        /// <value>The title.</value>
        public string Title
        {
            get { return _title; }
        }

        /// <summary>
        /// The URL of the site, when the channel is rendered, the image is a link to the site. 
        /// (Note, in practice the image &lt;title&gt; and &lt;link&gt; should have the same value as the channel's &lt;title&gt; and &lt;link&gt;.
        /// </summary>
        /// <value>The link.</value>
        public Uri Link
        {
            get { return _link; }
        }

        /// <summary>
        /// Gets the width.
        /// </summary>
        /// <value>The width.</value>
        public int? Width
        {
            get { return _width; }
        }

        /// <summary>
        /// Gets the height.
        /// </summary>
        /// <value>The height.</value>
        public int? Height
        {
            get { return Height; }
        }

        /// <summary>
        /// Gets the description.
        /// </summary>
        /// <value>The description.</value>
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
            {
                writer.WriteElementString("width", _width.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (_height != null)
            {
                writer.WriteElementString("height", _height.Value.ToString(CultureInfo.InvariantCulture));
            }

            if (_description != null)
            {
                writer.WriteElementString("description", _description); //Used in the alt tag.
            }
            writer.WriteEndElement();
        }
    }
}