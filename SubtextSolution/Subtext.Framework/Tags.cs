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
        public static ICollection<Tag> GetTopTags(int ItemCount)
        {
            IDictionary<string, int> topTags = ObjectProvider.Instance().GetTopTags(ItemCount);

            double mean;
            double stdDev = Statistics.StdDev(topTags.Values, out mean);

            List<Tag> tags = new List<Tag>();
            foreach (KeyValuePair<string, int> tag in topTags)
            {
                Tag t = new Tag(tag);
                t.Factor = (t.Count - mean) / stdDev;
                t.Weight = computeWeight(t.Factor, stdDev);
                tags.Add(t);
            }

            return tags;
        }

        private static int computeWeight(double factor, double stdDev)
        {
            if (factor <= -0.25 * stdDev)
                return 1;
            if (factor <= 0 * stdDev)
                return 2;
            if (factor <= 0.25 * stdDev)
                return 3;
            if (factor < 0.5 * stdDev)
                return 4;
            if (factor < 1 * stdDev)
                return 5;
            if (factor < 2 * stdDev)
                return 6;
            return 7;
        }
        
    }
}
