#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when creating a blog (or updating a blog) and the 
	/// subfolder directory cannot be created.
	/// </summary>
	/// <remarks>
	/// This is not being used currently.  It was created as an experiment 
	/// and may never be used.
	/// </remarks>
	[Serializable]
	public class BlogApplicationDirectoryCreateException : BaseBlogConfigurationException
	{
		//TODO: Remove this class.

		/// <summary>
		/// Creates a new <see cref="BlogApplicationDirectoryCreateException"/> instance.
		/// </summary>
		/// <param name="innerException">Inner exception.</param>
		public BlogApplicationDirectoryCreateException(Exception innerException) : base(innerException)
		{}

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
