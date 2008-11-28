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
    [Serializable]
    public class MetaTag
    {
        public MetaTag()
        {
        }

        public MetaTag(string content)
        {
            Content = content;
        }

        public int Id
        {
            get;
            set;
        }

        public string Content
        {
            get;
            set;
        }

        public string Name
        {
            get;
            set;
        }

        public string HttpEquiv
        {
            get;
            set;
        }

        public int BlogId
        {
            get;
            set;
        }

        public int? EntryId
        {
            get;
            set;
        }

        public DateTime DateCreated
        {
            get;
            set;
        }

        /// <summary>
        /// Validates that this MetaTag is Valid:
        /// - Content must not be null nor empty
        /// - Must have either a name or http-equiv, but not both
        /// </summary>
        public bool IsValid
        {
            get
            {
                if (string.IsNullOrEmpty(this.Content))
                {
                    ValidationMessage = "Meta Tag requires Content.";
                    return false;
                }

                // to be valid, a MetaTag requires etiher the Name or HttpEquiv attribute, but never both.
                if (string.IsNullOrEmpty(this.Name) && string.IsNullOrEmpty(this.HttpEquiv))
                {
                    ValidationMessage = "Meta Tag requires either a Name or Http-Equiv value.";
                    return false;
                }

                if (!string.IsNullOrEmpty(this.Name) && !string.IsNullOrEmpty(this.HttpEquiv))
                {
                    ValidationMessage = "Meta Tag can not have both a Name and Http-Equiv value.";
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