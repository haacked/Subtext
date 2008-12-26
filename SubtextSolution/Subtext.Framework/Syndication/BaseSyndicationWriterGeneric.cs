using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Syndication
{
	public abstract class BaseSyndicationWriter<T> : BaseSyndicationWriter
	{
		/// <summary>
		/// Creates a new <see cref="BaseSyndicationWriter"/> instance.
		/// </summary>
		/// <param name="sw">Sw.</param>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
        protected BaseSyndicationWriter(TextWriter writer, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding, ISubtextContext context)
            : base(writer, dateLastViewedFeedItemPublished, useDeltaEncoding, context)
		{
		}
		
        /// <summary>
		/// Gets or sets the entries to be rendered in the feed.
		/// </summary>
		/// <value>The entries.</value>
		public ICollection<T> Items
		{
			get;
			set;
		}
	}
}
