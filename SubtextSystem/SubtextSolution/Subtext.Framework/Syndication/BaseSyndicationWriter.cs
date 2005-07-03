using System;
using System.IO;
using System.Xml;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication
{
    /// <summary>
    /// Summary description for BaseSyndicationWriter.
    /// </summary>
    public abstract class BaseSyndicationWriter : XmlTextWriter
    {
        private StringWriter writer  = null;
        protected BlogInfo info;
		DateTime dateLastViewedFeedItemPublished = DateTime.MinValue;
		protected DateTime latestPublishDate = DateTime.MinValue;
		protected bool useDeltaEncoding = false;
		protected bool clientHasAllFeedItems = false;

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
        protected BaseSyndicationWriter(StringWriter sw) : this(sw, DateTime.MinValue, false)
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
			this.dateLastViewedFeedItemPublished = dateLastViewedFeedItemPublished;
			writer = sw;
			info = Config.CurrentBlog;
			this.useDeltaEncoding = useDeltaEncoding;
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
                return writer;
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
			get
			{
				return clientHasAllFeedItems;
			}
		}

		/// <summary>
		/// Gets the publish date of the latest syndicated item.
		/// </summary>
		/// <value></value>
		public DateTime LatestPublishDate
		{
			get
			{
				return latestPublishDate;
			}
		}
		
		/// <summary>
		/// Gets the publish date of the last syndicated feed item 
		/// that the client aggregator viewed.  This is sent as 
		/// the ETag.
		/// </summary>
		/// <value></value>
		public DateTime DateLastViewedFeedItemPublished
		{
			get {return this.dateLastViewedFeedItemPublished;}
		}

    	private bool _useAggBugs = false;
        public bool UseAggBugs
        {
            get {return this._useAggBugs;}
            set {this._useAggBugs = value;}
        }

        private bool _allowComments = true;
        public bool AllowComments
        {
            get {return this._allowComments;}
            set {this._allowComments = value;}
        }

        private EntryCollection _entries;
        public EntryCollection Entries
        {
            get {return this._entries;}
            set {this._entries = value;}
        }

        protected abstract void Build();
		protected abstract void Build(DateTime dateLastViewedFeedItemPublished);
    }
}
