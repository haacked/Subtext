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
