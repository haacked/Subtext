using System;
using System.Collections.Generic;
using System.IO;

namespace Subtext.Framework.Syndication
{
	public abstract class BaseSyndicationWriter<T> : BaseSyndicationWriter
	{

				/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
        protected BaseSyndicationWriter(StringWriter sw) : base(sw, NullValue.NullDateTime, false)
        {
        }

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="dateLastViewedFeedItemPublished"></param>
        protected BaseSyndicationWriter(DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding) : base(new StringWriter(), dateLastViewedFeedItemPublished, useDeltaEncoding)
        {
		}

		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
		protected BaseSyndicationWriter(StringWriter sw, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding) : base(sw,dateLastViewedFeedItemPublished,useDeltaEncoding)
		{
		}
		/// <summary>
		/// Gets or sets the entries to be rendered in the feed.
		/// </summary>
		/// <value>The entries.</value>
		public IList<T> Items
		{
			get { return this.items; }
			set { this.items = value; }
		}
		private IList<T> items;
	}
}
