using System;
using System.Web;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when creating a blog (or updating a blog) and the 
	/// application directory cannot be created.
	/// </summary>
	[Serializable]
	public class BlogApplicationDirectoryCreateException : BaseBlogConfigurationException
	{
		/// <summary>
		/// Creates a new <see cref="BlogApplicationDirectoryCreateException"/> instance.
		/// </summary>
		/// <param name="innerException">Inner exception.</param>
		public BlogApplicationDirectoryCreateException(Exception innerException) : base(innerException)
		{}

		/// <summary>
		/// Gets the message resource key.
		/// </summary>
		/// <value></value>
		public override string MessageResourceKey
		{
			get
			{
				throw new NotImplementedException("I8N not implemented.");
			}
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				return "For various technical reasons, in order to create this blog with its dynamic URL, "
					+ "Subtext needs write access to create a folder in the application root. "
					+ "Please give the ASPNET user (or Network Services for IIS 6.0) write permissions to the "
					+ "following folder: <em>" + HttpContext.Current.Request.MapPath(HttpContext.Current.Request.ApplicationPath) + "</em>";
			}
		}

	}
}
