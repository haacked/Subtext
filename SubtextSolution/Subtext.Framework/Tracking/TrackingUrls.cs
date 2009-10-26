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
using System.Globalization;

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Summary description for TrackingUrls.
    /// </summary>
    public static class TrackingUrls
    {
        private const string ImagePattern = "<img src=\"{0}\" width=\"1\" height=\"1\" />";

        public static string AggBugImage(string url)
        {
            return String.Format(CultureInfo.InvariantCulture, ImagePattern, url);
        }
    }
}