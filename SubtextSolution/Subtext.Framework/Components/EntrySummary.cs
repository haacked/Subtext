using System;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility;

namespace Subtext.Framework.Components
{
    public class EntrySummary : IEntryIdentity
    {
        public int Id { 
            get; 
            set; 
        }

        public string EntryName {
            get;
            set;
        }

        public int ViewCount { 
            get; 
            set;
        }

        public string Title { 
            get; 
            set; 
        }

        public DateTime DateCreated { 
            get; 
            set; 
        }

        public PostType PostType {
            get {
                return PostType.BlogPost;
            }
        }
    }
}
