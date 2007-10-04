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
		private StringWriter writer = null;
		protected BlogInfo info;
		DateTime dateLastViewedFeedItemPublished = NullValue.NullDateTime;
		protected DateTime latestPublishDate = NullValue.NullDateTime;
		protected bool useDeltaEncoding = false;
		protected bool clientHasAllFeedItems = false;

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
		protected BaseSyndicationWriter(StringWriter sw)
			: this(sw, NullValue.NullDateTime, false)
		{
		}

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished"></param>
		protected BaseSyndicationWriter(DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding)
			: this(new StringWriter(), dateLastViewedFeedItemPublished, useDeltaEncoding)
		{
		}

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
		protected BaseSyndicationWriter(StringWriter sw, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding)
			: base(sw)
		{
			this.dateLastViewedFeedItemPublished = dateLastViewedFeedItemPublished;
			writer = sw;
			info = Config.CurrentBlog;
			this.useDeltaEncoding = useDeltaEncoding;
			this.Formatting = System.Xml.Formatting.Indented;
			this.Indentation = 4;
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
			get { return this.dateLastViewedFeedItemPublished; }
		}

		private bool _useAggBugs = false;
		public bool UseAggBugs
		{
			get { return this._useAggBugs; }
			set { this._useAggBugs = value; }
		}

		private bool _allowComments = true;
		public bool AllowComments
		{
			get { return this._allowComments; }
			set { this._allowComments = value; }
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
