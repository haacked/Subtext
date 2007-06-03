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
// This class adapted from Rama Krishna Vavilala's Tag Cloud control article on 
// Code Project: http://www.codeproject.com/useritems/cloud.asp
#endregion

using System;
using System.Collections.Generic;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Statistical functions
	/// </summary>
	public static class Statistics
	{
		public static double Mean(IEnumerable<double> values)
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
		
		public static double StdDev(IEnumerable<double> values, out double mean)
		{
			mean = Mean(values);
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

        public static double StdDev<TValue>(IEnumerable<TValue> values, out double mean)
        {
            List<double> converted = new List<double>();
            foreach (TValue value in values)
            {
                converted.Add(Convert.ToDouble(value));
            }
            return StdDev(converted, out mean);
        }

		public static double StdDev(IEnumerable<double> values)
		{
			double mean;
			return StdDev(values, out mean);
		}	
	}
}
