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

        #region Getter and Setters

        private int _id;
        private string _title;
        private string _url;
        private long _size;
        private string _mimetype;
        private int _entryId;

        public string Url
        {
            get { return _url; }
            set { _url = value; }
        }

        public string Title
        {
            get { return _title; }
            set { _title = value; }
        }

        public long Size
        {
            get { return _size; }
            set { _size = value; }
        }

        public string MimeType
        {
            get { return _mimetype; }
            set { _mimetype = value; }
        }

        public int Id
        {
            get { return _id; }
            set { _id = value; }
        }

        public int EntryId
        {
            get { return _entryId; }
            set { _entryId = value; }
        }

        private bool _addToFeed;

        public bool AddToFeed
        {
            get { return _addToFeed; }
            set { _addToFeed = value; }
        }

        private bool _showWithPost;

        public bool ShowWithPost
        {
            get { return _showWithPost; }
            set { _showWithPost = value; }
        }

        public string FormattedSize
        {
            get
            {
                if (_size < 1024)
                    return _size + " bytes";
                if (_size < 1024 * 1024)
                    return Math.Round(((double)_size / 1024), 2) + " KB";
                if (_size < 1024 * 1024 * 1024)
                    return Math.Round(((double)_size / (1024 * 1024)), 2) + " MB";

                return Math.Round(((double)_size / (1024 * 1024 * 1024)), 2) + " GB";
            }
        }

        #endregion

        public bool IsValid
        {
            get
            {
                if(_entryId==0)
                {
                    _validationMessage = "Enclosure requires to be bound to a Entry.";
                    return false;
                }

                if (string.IsNullOrEmpty(_url))
                {
                    _validationMessage = "Enclosure requires a Url.";
                    return false;
                }

                if (string.IsNullOrEmpty(_mimetype))
                {
                    _validationMessage = "Enclosure requires a MimeType.";
                    return false;
                }

                if (_size == 0)
                {
                    _validationMessage = "Enclosure size must be greater than zero.";
                    return false;
                }
                


                _validationMessage = null;
                return true;
            }
        }

        private string _validationMessage;
        public string ValidationMessage
        {
            get
            {
                return _validationMessage;
            }
        }
        
    }
}
