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

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when creating an application
	/// </summary>
	[Serializable]
	public class InvalidSubfolderNameException : BaseBlogConfigurationException
	{
		string subfolder;

		/// <summary>
		/// Creates a new <see cref="InvalidSubfolderNameException"/> instance.
		/// </summary>
		/// <param name="subfolder">Subfolder.</param>
		public InvalidSubfolderNameException(string subfolder) : base()
		{
			this.subfolder = subfolder;
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				return "Sorry, but the subfolder name &#8220;" + subfolder + "&#8221; you&#8217;ve chosen is not allowed.";
			}
		}

	}
}
