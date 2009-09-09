using System;

namespace Subtext.Framework.Tracking
{
    /// <summary>
    /// Event arguments for the SourceVerification event.
    /// </summary>
    public class SourceVerificationEventArgs : EventArgs
    {
        Uri entryUrl;
        Uri sourceUrl;
        bool verified;

        /// <summary>
        /// Initializes a new instance of the <see cref="SourceVerificationEventArgs"/> class.
        /// </summary>
        /// <param name="sourceUrl">The source URL.</param>
        /// <param name="entryUrl">The entry URL.</param>
        public SourceVerificationEventArgs(Uri sourceUrl, Uri entryUrl)
            : base()
        {
            this.sourceUrl = sourceUrl;
            this.entryUrl = entryUrl;
        }

        /// <summary>
        /// Gets the source URL.
        /// </summary>
        /// <value>The source URL.</value>
        public Uri SourceUrl
        {
            get { return sourceUrl; }
        }

        /// <summary>
        /// Gets the entry URL.
        /// </summary>
        /// <value>The entry URL.</value>
        public Uri EntryUrl
        {
            get { return entryUrl; }
        }

        /// <summary>
        /// Gets a value indicating whether this <see cref="SourceVerificationEventArgs"/> is verified.
        /// </summary>
        /// <value><c>true</c> if verified; otherwise, <c>false</c>.</value>
        public bool Verified
        {
            get { return verified; }
            set { verified = value; }
        }
    }
}