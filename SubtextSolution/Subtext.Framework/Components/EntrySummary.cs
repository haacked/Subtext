using System;

namespace Subtext.Framework.Components
{
    public class EntrySummary
    {
        public int EntryId { 
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

        public DateTime DateAdded { 
            get; 
            set; 
        }
    }
}
