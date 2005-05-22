using System;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when creating a new blog, or changing an existing 
	/// blog, without an Application value specified, when another blog 
	/// with the same Host name exists.
	/// </summary>
	/// <remarks>
	/// An example of this case is where a system has a blog with the host 
	/// "example.com" and the application name "MyBlog".  Attempting to create 
	/// a new blog with the host name "example.com" and an empty application 
	/// name will result in this exception being thrown.
	/// </remarks>
	public class BlogRequiresApplicationException : BaseSubtextException
	{
		int _blogsWithSameHostCount;
		int _blogId = int.MinValue;

		/// <summary>
		/// Creates a new <see cref="BlogRequiresApplicationException"/> instance.
		/// </summary>
		/// <param name="blogsWithSameHostCount">The number of blogs with this 
		/// host name (not counting the blog being modified).</param>
		/// <param name="blogId">The blog that is being modified and is conflicting with a pre-existing blog.</param>
		public BlogRequiresApplicationException(int blogsWithSameHostCount, int blogId) : base()
		{
			_blogsWithSameHostCount = blogsWithSameHostCount;
			_blogId = blogId;
		}

		/// <summary>
		/// Creates a new <see cref="BlogRequiresApplicationException"/> instance.
		/// </summary>
		/// <param name="blogsWithSameHostCount">The number of blogs with this host name.</param>
		public BlogRequiresApplicationException(int blogsWithSameHostCount) : this(blogsWithSameHostCount, int.MinValue)
		{
		}

		/// <summary>
		/// Gets the message resource key.
		/// </summary>
		/// <value></value>
		public override string MessageResourceKey
		{
			get
			{
				throw new NotImplementedException("I8N Not yet implemented");
			}
		}

	}
}
