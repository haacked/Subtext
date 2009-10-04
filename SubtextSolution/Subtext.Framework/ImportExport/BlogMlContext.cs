namespace Subtext.BlogML
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