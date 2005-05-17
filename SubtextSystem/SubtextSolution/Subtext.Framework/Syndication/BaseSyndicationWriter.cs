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
        protected BlogConfig config;
		int lastViewedFeedItemId = int.MinValue;
		protected int latestFeedItemId = int.MinValue;
		protected bool clientHasAllFeedItems = false;

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
        protected BaseSyndicationWriter(StringWriter sw) : this(sw, int.MinValue)
        {   
        }

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="lastViewedFeedItem"></param>
        protected BaseSyndicationWriter(int lastViewedFeedItem) : this(new StringWriter(), lastViewedFeedItem)
        {
		}

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
		/// <param name="lastViewedFeedItem">Last viewed feed item.</param>
		protected BaseSyndicationWriter(StringWriter sw, int lastViewedFeedItem) : base(sw)
		{
			lastViewedFeedItemId = lastViewedFeedItem;	
			writer = sw;
			config = Config.CurrentBlog;
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
		/// Gets a value indicating whether [client has all feed items].
		/// </summary>
		/// <value>
		/// 	<c>true</c> if [client has all feed items]; otherwise, <c>false</c>.
		/// </value>
		public bool ClientHasAllFeedItems
		{
			get
			{
				return clientHasAllFeedItems;
			}
		}

		/// <summary>
		/// Gets the latest feed item id that will be returned to 
		/// the client.  This will be placed in the ETag.
		/// </summary>
		/// <value></value>
		public int LatestFeedItemId
		{
			get
			{
				return this.latestFeedItemId;
			}
		}

		/// <summary>
		/// Gets the id of last viewed feed item according 
		/// to the client request.
		/// </summary>
		/// <value></value>
    	public int LastViewedFeedItemId
    	{
    		get { return this.lastViewedFeedItemId; }
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
		protected abstract void Build(int lastIdViewed);
    }
}
