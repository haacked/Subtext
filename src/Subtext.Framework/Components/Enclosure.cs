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
using System.Xml.Serialization;
using Subtext.Framework.Properties;

namespace Subtext.Framework.Components
{
    [XmlRoot("Enclosure", Namespace = "urn-Subtext")]
    public class Enclosure
    {
        public string Url { get; set; }

        public string Title { get; set; }

        public long Size { get; set; }

        public string MimeType { get; set; }

        public int Id { get; set; }

        public int EntryId { get; set; }

        public bool AddToFeed { get; set; }

        public bool ShowWithPost { get; set; }

        public string FormattedSize
        {
            get
            {
                if (Size < 1024)
                {
                    return Size + " bytes";
                }
                if (Size < 1024 * 1024)
                {
                    return Math.Round(((double)Size / 1024), 2) + " KB";
                }
                if (Size < 1024 * 1024 * 1024)
                {
                    return Math.Round(((double)Size / (1024 * 1024)), 2) + " MB";
                }

                return Math.Round(((double)Size / (1024 * 1024 * 1024)), 2) + " GB";
            }
        }

        public bool IsValid
        {
            get
            {
                if (EntryId == 0)
                {
                    ValidationMessage = Resources.Enclosure_NeedsAnEntry;
                    return false;
                }

                if (string.IsNullOrEmpty(Url))
                {
                    ValidationMessage = Resources.Enclosure_UrlRequired;
                    return false;
                }

                if (string.IsNullOrEmpty(MimeType))
                {
                    ValidationMessage = Resources.Enclosure_MimeTypeRequired;
                    return false;
                }

                if (Size == 0)
                {
                    ValidationMessage = Resources.Enclosure_SizeGreaterThanZero;
                    return false;
                }

                ValidationMessage = null;
                return true;
            }
        }

        public string ValidationMessage { get; private set; }
    }
}