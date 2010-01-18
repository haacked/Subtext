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
using System.IO;
using System.Xml;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Base class for writing RSS and ATOM feeds.
    /// </summary>
    public abstract class BaseSyndicationWriter : XmlTextWriter
    {
        private readonly TextWriter _writer;

        /// <summary>
        /// Creates a new <see cref="BaseSyndicationWriter"/> instance.
        /// </summary>
        protected BaseSyndicationWriter(TextWriter writer, DateTime dateLastViewedFeedItemPublished,
                                        bool useDeltaEncoding, ISubtextContext context) : base(writer)
        {
            LatestPublishDate = NullValue.NullDateTime;

            DateLastViewedFeedItemPublished = dateLastViewedFeedItemPublished;
            _writer = writer;
            SubtextContext = context;
            Blog = context.Blog;
            UrlHelper = context.UrlHelper;
            UseDeltaEncoding = useDeltaEncoding;
            Formatting = Formatting.Indented;
            Indentation = 4;
        }

        public ISubtextContext SubtextContext { get; private set; }

        public UrlHelper UrlHelper { get; private set; }

        public Blog Blog { get; private set; }

        public bool UseDeltaEncoding { get; protected set; }

        /// <summary>
        /// Gets the underlying text writer.
        /// </summary>
        /// <value></value>
        public TextWriter TextWriter
        {
            get
            {
                Build();
                return _writer;
            }
        }

        /// <summary>
        /// Gets the XML.
        /// </summary>
        /// <value></value>
        public string Xml
        {
            get
            {
                var xml = TextWriter.ToString();
                return xml;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the feed client has all the feed items.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the client has all feed items; otherwise, <c>false</c>.
        /// </value>
        public bool ClientHasAllFeedItems { get; protected set; }

        /// <summary>
        /// Gets the publish date of the latest syndicated item.
        /// </summary>
        /// <value></value>
        public DateTime LatestPublishDate { get; protected set; }

        /// <summary>
        /// Gets the publish date of the last syndicated feed item 
        /// that the client aggregator viewed.  This is sent as 
        /// the ETag.
        /// </summary>
        /// <value></value>
        public DateTime DateLastViewedFeedItemPublished { get; private set; }

        public bool UseAggBugs { get; set; }

        public bool AllowComments { get; set; }

        /// <summary>
        /// Returns the XML
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return Xml;
        }

        /// <summary>
        /// Builds the feed.
        /// </summary>
        protected abstract void Build();

        /// <summary>
        /// Builds the feed with delta-encoding possible.
        /// </summary>
        /// <param name="dateLastViewedFeedItemPublished">The date last viewed feed item published.</param>
        protected abstract void Build(DateTime dateLastViewedFeedItemPublished);
    }
}