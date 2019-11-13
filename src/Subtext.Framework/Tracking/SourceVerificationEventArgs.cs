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

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Event arguments for the SourceVerification event.
    /// </summary>
    public class SourceVerificationEventArgs : EventArgs
    {
        readonly Uri _entryUrl;
        readonly Uri _sourceUrl;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceVerificationEventArgs"/> class.
        /// </summary>
        /// <param name="sourceUrl">The source URL.</param>
        /// <param name="entryUrl">The entry URL.</param>
        public SourceVerificationEventArgs(Uri sourceUrl, Uri entryUrl)
        {
            _sourceUrl = sourceUrl;
            _entryUrl = entryUrl;
        }

        /// <summary>
        /// Gets the source URL.
        /// </summary>
        /// <value>The source URL.</value>
        public Uri SourceUrl
        {
            get { return _sourceUrl; }
        }

        /// <summary>
        /// Gets the entry URL.
        /// </summary>
        /// <value>The entry URL.</value>
        public Uri EntryUrl
        {
            get { return _entryUrl; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SourceVerificationEventArgs"/> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified { get; set; }
    }
}