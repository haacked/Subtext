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
	public class InvalidApplicationNameException : BaseBlogConfigurationException
	{
		string _application;

		/// <summary>
		/// Creates a new <see cref="InvalidApplicationNameException"/> instance.
		/// </summary>
		/// <param name="application">Application.</param>
		public InvalidApplicationNameException(string application) : base()
		{
			_application = application;
		}
		/// <summary>
		/// Gets the message resource key.
		/// </summary>
		/// <value></value>
		public override string MessageResourceKey
		{
			get { throw new NotImplementedException(); }
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				return "Sorry, but the application name &#8220;" + _application + "&#8221; you&#8217;ve chosen is not allowed.";
			}
		}

	}
}
