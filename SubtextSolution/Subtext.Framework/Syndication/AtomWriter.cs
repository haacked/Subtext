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
using Subtext.Framework.Components;

namespace Subtext.Framework.Syndication
{
	/// <summary>
	/// Generates RSS
	/// </summary>
	public class AtomWriter : Subtext.Framework.Syndication.BaseAtomWriter
	{
		/// <summary>
		/// Creates a new <see cref="AtomWriter"/> instance.
		/// </summary>
		/// <param name="entries">Entries.</param>
		/// <param name="dateLastViewedFeedItemPublished">Last viewed feed item.</param>
        public AtomWriter(ICollection<Entry> entries, DateTime dateLastViewedFeedItemPublished, bool useDeltaEncoding) : base(dateLastViewedFeedItemPublished, useDeltaEncoding)
		{
			this.Items = entries;
			this.UseAggBugs = true;
		}
	}
}

