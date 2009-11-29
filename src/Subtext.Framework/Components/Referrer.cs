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

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for Referrer.
    /// </summary>
    [Serializable]
    public class Referrer
    {
        private string _referrerUrl;

        public string ReferrerUrl
        {
            get
            {
                if(!_referrerUrl.StartsWith("http://"))
                {
                    return string.Format("http://{0}", _referrerUrl);
                }
                return _referrerUrl;
            }
            set { _referrerUrl = value; }
        }

        public int Count { get; set; }

        public int EntryId { get; set; }

        public string PostTitle { get; set; }

        public DateTime LastReferDate { get; set; }

        public int BlogId { get; set; }
    }
}