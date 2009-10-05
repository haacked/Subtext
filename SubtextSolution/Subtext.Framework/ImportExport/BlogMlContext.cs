#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

namespace Subtext.ImportExport
{
    /// <summary>
    /// Base implementation of the BlogMl context.
    /// </summary>
    public class BlogMLContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BlogMLContext"/> class.
        /// </summary>
        public BlogMLContext(string blogId, bool embedAttachments)
        {
            BlogId = blogId;
            EmbedAttachments = embedAttachments;
        }

        /// <summary>
        /// The id of the blog for which to import/export the blogml.
        /// </summary>
        public string BlogId { get; private set; }

        public bool EmbedAttachments { get; private set; }
    }
}