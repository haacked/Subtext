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
using System.Collections.Generic;
using System.IO;
using System.Xml;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Base class for writing RSS and ATOM feeds.
    /// </summary>
    public abstract class BaseSyndicationWriter : XmlTextWriter
    {
        private StringWriter _writer  = null;

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
        protected BaseSyndicationWriter(StringWriter sw) : this(sw, NullValue.NullDateTime, false)
        {
        }

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished"></param>
        protected BaseSyndicationWriter(DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding) : this(new StringWriter(), dateLastViewedFeedItemPublished, useDeltaEncoding)
        {
		}

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
		protected BaseSyndicationWriter(StringWriter sw, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding) : base(sw)
		{
            LatestPublishDate = NullValue.NullDateTime;

			DateLastViewedFeedItemPublished = dateLastViewedFeedItemPublished;
			_writer = sw;
			Blog = Config.CurrentBlog;
			UseDeltaEncoding = useDeltaEncoding;
			Formatting = System.Xml.Formatting.Indented;
			Indentation = 4;
		}

        public BlogInfo Blog
        {
            get;
            protected set;
        }

        public bool UseDeltaEncoding
        {
            get;
            protected set;
        }

		/// <summary>
		/// Gets the string writer.
		/// </summary>
		/// <value></value>
        public StringWriter StringWriter
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
				return this.StringWriter.ToString();
			}
        }

		/// <summary>
		/// Returns the XML
		/// </summary>
		/// <returns></returns>
        public override string ToString()
        {
            return Xml;
        }

		/// <summary>
		/// Gets a value indicating whether the feed client has all the feed items.
		/// </summary>
		/// <value>
		/// 	<c>true</c> if the client has all feed items; otherwise, <c>false</c>.
		/// </value>
		public bool ClientHasAllFeedItems
		{
			get;
            protected set;
		}

		/// <summary>
		/// Gets the publish date of the latest syndicated item.
		/// </summary>
		/// <value></value>
		public DateTime LatestPublishDate
		{
			get;
            protected set;
		}
		
		/// <summary>
		/// Gets the publish date of the last syndicated feed item 
		/// that the client aggregator viewed.  This is sent as 
		/// the ETag.
		/// </summary>
		/// <value></value>
        public DateTime DateLastViewedFeedItemPublished
        {
            get;
            private set;
        }

        public bool UseAggBugs
        {
            get;
            set;
        }

        public bool AllowComments
        {
            get;
            set;
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
