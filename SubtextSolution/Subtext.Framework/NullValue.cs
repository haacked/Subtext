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

#endregion

using System;

namespace Subtext.Framework
{
    /// <summary>
    /// Constants used to represent null value type.
    /// </summary>
    public static class NullValue
    {
        /// <summary>Represents a null double.</summary>
        public const double NullDouble = double.NaN;

        /// <summary>Represents a null integer.</summary>
        public const int NullInt32 = Int32.MinValue;

        /// <summary>Represents a null DateTime</summary>
        public static DateTime NullDateTime
        {
            get { return DateTime.MinValue; }
        }

        /// <summary>
        /// Determines whether the specified value is null.
        /// </summary>
        /// <param name="number">The value.</param>
        /// <returns>
        /// 	<c>true</c> if the specified value is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(int number)
        {
            return number == NullInt32;
        }

        public static bool IsNull(Guid guid)
        {
            return guid == Guid.Empty;
        }

        /// <summary>
        /// Determines whether the specified number is null.
        /// </summary>
        /// <param name="number">The number.</param>
        /// <returns>
        /// 	<c>true</c> if the specified number is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(double number)
        {
            return number.Equals(NullDouble);
        }

        /// <summary>
        /// Determines whether the specified date time is null.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>
        /// 	<c>true</c> if the specified date time is null; otherwise, <c>false</c>.
        /// </returns>
        public static bool IsNull(DateTime dateTime)
        {
            return dateTime == NullDateTime;
        }
    }
}