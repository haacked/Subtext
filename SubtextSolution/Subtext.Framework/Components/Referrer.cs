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

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Summary description for Referrer.
    /// </summary>
    [Serializable]
    public class Referrer
    {
        public string ReferrerURL
        {
            get
            {
                if (!_referrerURL.StartsWith("http://"))
                {
                    return "http://" + _referrerURL;
                }
                return _referrerURL;
            }
            set { _referrerURL = value; }
        }
        private string _referrerURL;

        public int Count
        {
            get;
            set;
        }

        public int EntryID
        {
            get;
            set;
        }

        public string PostTitle
        {
            get;
            set;
        }

        public DateTime LastReferDate
        {
            get;
            set;
        }

        public int BlogId
        {
            get;
            set;
        }
    }
}
