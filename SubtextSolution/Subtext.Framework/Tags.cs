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
// Logic adapted from Rama Krishna Vavilala's tag cloud article on Code Project
// http://www.codeproject.com/useritems/cloud.asp
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

#region Using
using System;
using System.Collections.Generic;
using Subtext.Framework.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

#endregion

namespace Subtext.Framework
{
    /// <summary>
    /// Static class used to retrieve tags from the data store. Tags are simple enough 
    /// that generic collections are used instead of custom Tag objects.
    /// </summary>
    public static class Tags
    {
		/// <summary>
		/// Parses and sets set of tags on the entry.
		/// </summary>
		/// <param name="entry"></param>
		public static void SetTagsOnEntry(Entry entry)
		{
			ObjectProvider.Instance().SetEntryTagList(entry.Id, HtmlHelper.ParseTags(entry.Body));
		}

		/// <summary>
		/// Sets the tags on entry.
		/// </summary>
		/// <param name="entryId">The entry id.</param>
		/// <param name="tags">The tags.</param>
		public static void SetTagsOnEntry(int entryId, IList<string> tags)
		{
			ObjectProvider.Instance().SetEntryTagList(entryId, tags);
		}

    	/// <summary>
		/// Gets the top tags.
		/// </summary>
		/// <param name="itemCount">The item count.</param>
		/// <returns></returns>
        public static IList<Tag> GetTopTags(int itemCount)
        {
			if (itemCount < 0)
				throw new ArgumentNullException("itemCount", "Cannot request negative tags. Pass in 0 to get all tags.");
            IDictionary<string, int> topTags = ObjectProvider.Instance().GetTopTags(itemCount);

            double mean;
            double stdDev = Statistics.StdDev(topTags.Values, out mean);

            IList<Tag> tags = new List<Tag>();
            foreach (KeyValuePair<string, int> tag in topTags)
            {
                Tag t = new Tag(tag);
                t.Factor = (t.Count - mean) / stdDev;
                t.Weight = ComputeWeight(t.Factor, stdDev);
                tags.Add(t);
            }

            return tags;
        }

        public static int ComputeWeight(double factor, double standardDeviation)
        {
            if (factor <= -0.25 * standardDeviation)
                return 1;
            if (factor <= 0 * standardDeviation)
                return 2;
            if (factor <= 0.25 * standardDeviation)
                return 3;
            if (factor < 0.5 * standardDeviation)
                return 4;
            if (factor < 1 * standardDeviation)
                return 5;
            if (factor < 2 * standardDeviation)
                return 6;
            return 7;
        }
        
    }
}
