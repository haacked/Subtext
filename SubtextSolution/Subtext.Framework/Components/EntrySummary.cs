using System;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Framework.Components
{
    public class EntrySummary : IEntryIdentity
    {
        public int ViewCount { get; set; }

        public string Title { get; set; }

        #region IEntryIdentity Members

        public int Id { get; set; }

        public string EntryName { get; set; }

        public DateTime DateCreated { get; set; }

        public PostType PostType
        {
            get { return PostType.BlogPost; }
        }

        #endregion
    }
}