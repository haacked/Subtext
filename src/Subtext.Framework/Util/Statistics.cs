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
// This class adapted from Rama Krishna Vavilala's Tag Cloud control article on 
// Code Project: http://www.codeproject.com/useritems/cloud.asp

#endregion

using System;
using System.Collections.Generic;
using System.Globalization;

namespace Subtext.Framework.Util
{
    /// <summary>
    /// Statistical functions
    /// </summary>
    public static class Statistics
    {
        public static double Mean(this IEnumerable<double> values)
        {
            double sum = 0;
            int count = 0;

            foreach (double d in values)
            {
                sum += d;
                count++;
            }

            return sum / count;
        }

        public static double StandardDeviation(this IEnumerable<double> values, out double mean)
        {
            mean = values.Mean();
            double sumOfDiffSquares = 0;
            int count = 0;

            foreach (double d in values)
            {
                double diff = (d - mean);
                sumOfDiffSquares += diff * diff;
                count++;
            }

            return Math.Sqrt(sumOfDiffSquares / count);
        }

        public static double StandardDeviation<TValue>(this IEnumerable<TValue> values, out double mean)
        {
            var converted = new List<double>();
            foreach (TValue value in values)
            {
                converted.Add(Convert.ToDouble(value, CultureInfo.InvariantCulture));
            }
            return StandardDeviation(converted, out mean);
        }
    }
}