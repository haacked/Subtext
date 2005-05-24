using System;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when trying to add a blog that 
	/// duplicates another blog in both host and application.
	/// </summary>
	public class BlogDuplicationException : BaseBlogConfigurationException
	{
		/// <summary>
		/// Creates a new <see cref="BlogDuplicationException"/> instance.
		/// </summary>
		/// <param name="duplicate">Duplicate.</param>
		public BlogDuplicationException(BlogInfo duplicate) : this(duplicate, int.MinValue)
		{
		}

		/// <summary>
		/// Creates a new <see cref="BlogDuplicationException"/> instance.
		/// </summary>
		/// <param name="duplicate">Duplicate.</param>
		/// <param name="blogId">Blog id of the blog we were updating.  If this is .</param>
		public BlogDuplicationException(BlogInfo duplicate, int blogId) : base()
		{
			_duplicateBlog = duplicate;
			_blogId = blogId;
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				if(_blogId == int.MinValue)
				{
					return "Oooh. A blog with the same host and application already exists.";
				}
				else
				{
					return "Sorry, but changing this blog to use that host and application name would conflict with another blog.";
				}
			}
		}

		/// <summary>
		/// Gets the message resource key.
		/// </summary>
		/// <value></value>
		public override string MessageResourceKey
		{
			get
			{
				//TODO: Internationalize...
				throw new NotImplementedException("We have not yet implemented I8N.");
			}
		}

		/// <summary>
		/// Gets the duplicate blog.
		/// </summary>
		/// <value></value>
		public BlogInfo DuplicateBlog
		{
			get { return _duplicateBlog; }
		}

		/// <summary>
		/// Id of the blog being updated that caused this exception.  This 
		/// would be populated if updating a blog to have the same host and 
		/// application as another blog.  Otherwise this is equal to int.MinValue.
		/// </summary>
		/// <value></value>
		public int BlogId
		{
			get { return _blogId; }
		}

		int _blogId = int.MinValue;

		BlogInfo _duplicateBlog;
	}
}
