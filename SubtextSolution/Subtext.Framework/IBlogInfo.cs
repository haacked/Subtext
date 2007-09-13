using System;

namespace Subtext.Framework
{
	/// <summary>
	/// Represents a blog in the system.
	/// </summary>
	public interface IBlogInfo
	{
		/// <summary>
		/// Gets the root URL for this blog.  For example, "http://example.com/" or "http://example.com/blog/".
		/// </summary>
		/// <value></value>
		Uri RootUrl { get; }
	}
}
