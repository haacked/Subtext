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

namespace Subtext.Framework.Services.SearchEngine
{
    public class SearchEngineEntry
    {
        public int EntryId { get; set; }
        public string EntryName { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
        public string Tags { get; set; }
        public int BlogId { get; set; }
        public bool IsPublished { get; set; }
        public DateTime PublishDate { get; set; }
        public string BlogName { get; set; }
        public int GroupId { get; set; }
    }
}
