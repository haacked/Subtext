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
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Framework.Services.SearchEngine
{
    public class SearchEngineResult : IEntryIdentity
    {
        public int EntryId { get; set; }
        public string Title { get; set; }
        public DateTime PublishDate { get; set; }
        public string BlogName { get; set; }
        public float Score { get; set; }

        public string EntryName { get; set; }

        public DateTime DateSyndicated
        {
            get { return PublishDate; }
        }

        public DateTime DatePublishedUtc { get; set; }

        public PostType PostType
        {
            get { return PostType.BlogPost; }
        }

        public int Id
        {
            get { return EntryId; }
        }
    }
}
