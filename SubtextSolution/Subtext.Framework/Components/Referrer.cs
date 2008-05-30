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
        private string _referrerURL;
        private int _entryId;
        private string _postTitle;
        private DateTime _lastreferDate;
        private int _count;

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

        public int Count
        {
            get { return _count; }
            set { _count = value; }
        }

        public int EntryID
        {
            get { return _entryId; }
            set { _entryId = value; }
        }

        public string PostTitle
        {
            get { return _postTitle; }
            set { _postTitle = value; }
        }

        public DateTime LastReferDate
        {
            get { return _lastreferDate; }
            set { _lastreferDate = value; }
        }

        private int _blogId;
        public int BlogId
        {
            get { return _blogId; }
            set { _blogId = value; }
        }

    }
}
