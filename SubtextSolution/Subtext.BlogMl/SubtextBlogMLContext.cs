using System;
using Subtext.BlogML.Interfaces;

namespace Subtext.BlogML
{
	/// <summary>
	/// Base implementation of the BlogMl context.
	/// </summary>
	public class BlogMLContext : IBlogMLContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogMLContext"/> class.
		/// </summary>
		/// <param name="blogId">The blog id.</param>
		public BlogMLContext(string blogId, bool embedAttachments)
		{
			this.blogId = blogId;
			this.embedAttachments = embedAttachments;
		}

		/// <summary>
		/// The id of the blog for which to import/export the blogml.
		/// </summary>
		public string BlogId
		{
			get { return this.blogId; }
		}

		private string blogId;

		public bool EmbedAttachments
		{
			get
			{
				return this.embedAttachments;
			}
		}

		bool embedAttachments;
	}
}
