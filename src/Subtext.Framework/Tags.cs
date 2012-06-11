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
// Logic adapted from Rama Krishna Vavilala's tag cloud article on Code Project
// http://www.codeproject.com/useritems/cloud.asp
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using Subtext.Framework.Components;
using Subtext.Framework.Properties;
using Subtext.Framework.Providers;
using Subtext.Framework.Util;

namespace Subtext.Framework
{
    /// <summary>
    /// Static class used to retrieve tags from the data store. Tags are simple enough 
    /// that generic collections are used instead of custom Tag objects.
    /// </summary>
    public static class Tags
    {
        /// <summary>
        /// Gets the top tags.
        /// </summary>
        public static ICollection<Tag> GetMostUsedTags(this ObjectRepository repository, int itemCount)
        {
            if (itemCount < 0)
            {
                throw new ArgumentOutOfRangeException("itemCount", itemCount,
                                                      Resources.ArgumentOutOfRange_NegativeTagItemCount);
            }
            var topTags = repository.GetTopTags(itemCount);

            double mean;
            double stdDev = topTags.Values.StandardDeviation(out mean);

            var tags = new List<Tag>();
            foreach (var tag in topTags)
            {
                var t = new Tag(tag);
                t.Factor = (t.Count - mean) / stdDev;
                t.Weight = ComputeWeight(t.Factor, stdDev);
                tags.Add(t);
            }

            return tags;
        }

        public static int ComputeWeight(double factor, double standardDeviation)
        {
            if (factor <= -0.25 * standardDeviation)
            {
                return 1;
            }
            if (factor <= 0 * standardDeviation)
            {
                return 2;
            }
            if (factor <= 0.25 * standardDeviation)
            {
                return 3;
            }
            if (factor < 0.5 * standardDeviation)
            {
                return 4;
            }
            if (factor < 1 * standardDeviation)
            {
                return 5;
            }
            if (factor < 2 * standardDeviation)
            {
                return 6;
            }
            return 7;
        }
    }
}