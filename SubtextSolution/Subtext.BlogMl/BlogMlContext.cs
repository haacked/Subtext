using System;
using Subtext.BlogMl.Interfaces;

namespace Subtext.BlogMl
{
	/// <summary>
	/// Base implementation of the BlogMl context.
	/// </summary>
	public class BlogMlContext : IBlogMlContext
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogMlContext"/> class.
		/// </summary>
		/// <param name="blogId">The blog id.</param>
		public BlogMlContext(string blogId, bool embedAttachments)
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
