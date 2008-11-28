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
    public class Enclosure
    {
        public Enclosure()
        {
        }

        public string Url
        {
            get;
            set;
        }

        public string Title
        {
            get;
            set;
        }

        public long Size
        {
            get;
            set;
        }

        public string MimeType
        {
            get;
            set;
        }

        public int Id
        {
            get;
            set;
        }

        public int EntryId
        {
            get;
            set;
        }

        public bool AddToFeed
        {
            get;
            set;
        }

        public bool ShowWithPost
        {
            get;
            set;
        }

        public string FormattedSize
        {
            get
            {
                if (Size < 1024)
                    return Size + " bytes";
                if (Size < 1024 * 1024)
                    return Math.Round(((double)Size / 1024), 2) + " KB";
                if (Size < 1024 * 1024 * 1024)
                    return Math.Round(((double)Size / (1024 * 1024)), 2) + " MB";

                return Math.Round(((double)Size / (1024 * 1024 * 1024)), 2) + " GB";
            }
        }

        public bool IsValid
        {
            get
            {
                if(EntryId == 0)
                {
                    ValidationMessage = "Enclosure requires to be bound to a Entry.";
                    return false;
                }

                if (string.IsNullOrEmpty(Url))
                {
                    ValidationMessage = "Enclosure requires a Url.";
                    return false;
                }

                if (string.IsNullOrEmpty(MimeType))
                {
                    ValidationMessage = "Enclosure requires a MimeType.";
                    return false;
                }

                if (Size == 0)
                {
                    ValidationMessage = "Enclosure size must be greater than zero.";
                    return false;
                }

                ValidationMessage = null;
                return true;
            }
        }

        public string ValidationMessage
        {
            get;
            private set;
        }
        
    }
}
