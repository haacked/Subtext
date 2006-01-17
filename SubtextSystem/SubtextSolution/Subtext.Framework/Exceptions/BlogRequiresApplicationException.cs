using System;

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
	[Serializable]
	public class BlogRequiresApplicationException : BaseBlogConfigurationException
	{
		int _blogsWithSameHostCount;
		int _blogId = NullValue.NullInt32;
		string _host;

		/// <summary>
		/// Creates a new <see cref="BlogRequiresApplicationException"/> instance.
		/// </summary>
		/// <param name="blogsWithSameHostCount">The number of blogs with this 
		/// host name (not counting the blog being modified).</param>
		/// <param name="blogId">The blog that is being modified and is conflicting with a pre-existing blog.</param>
		public BlogRequiresApplicationException(string hostName, int blogsWithSameHostCount, int blogId) : base()
		{
			_host = hostName;
			_blogsWithSameHostCount = blogsWithSameHostCount;
			_blogId = blogId;
		}

		/// <summary>
		/// Creates a new <see cref="BlogRequiresApplicationException"/> instance.
		/// </summary>
		/// <param name="blogsWithSameHostCount">The number of blogs with this host name.</param>
		public BlogRequiresApplicationException(string hostName, int blogsWithSameHostCount) : this(hostName, blogsWithSameHostCount, NullValue.NullInt32)
		{
		}

		/// <summary>
		/// Gets the blogs with same host count.
		/// </summary>
		/// <value></value>
		public int BlogsWithSameHostCount
		{
			get { return _blogsWithSameHostCount; }
		}

		/// <summary>
		/// Gets the blog id.
		/// </summary>
		/// <value></value>
		public int BlogId
		{
			get { return _blogId; }
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

		/// <summary>
		/// Gets a message that describes the current exception.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				string blogCountClause = "is another blog";
				if(_blogsWithSameHostCount >= 1)
					blogCountClause = "are " + _blogsWithSameHostCount + " blogs";

				return String.Format("Sorry, but there {0} with the specified hostname '{1}'.  To set up another blog with the same hostname, you must provide an application name.  Please click on 'Host Domain' below for more information.", blogCountClause, _host);
			}
		}


	}
}
