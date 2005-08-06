using System;

namespace Subtext.Framework.Exceptions
{
	/// <summary>
	/// Exception thrown when a blog does not exist.  This is 
	/// indicative of not having created a blog_config record.
	/// </summary>
	public class BlogDoesNotExistException : BaseSubtextException 
	{
		/// <summary>
		/// Creates a new <see cref="BlogDoesNotExistException"/> instance.
		/// </summary>
		/// <param name="host"></param>
		/// <param name="app"></param>
		/// <param name="anyBlogsExist">Any blogs exist.</param>
		public BlogDoesNotExistException(string host, string app, bool anyBlogsExist) : base()
		{
			_anyBlogsExist = anyBlogsExist;
			_host = host;
			_applicationName = app;
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
		/// Returns true if any blogs exist in the system.
		/// </summary>
		public bool AnyBlogsExist
		{
			get { return _anyBlogsExist; }
		}

		bool _anyBlogsExist;

		/// <summary>
		/// Gets the requested blog host.
		/// </summary>
		/// <value></value>
		public string Host
		{
			get { return _host; }
		}

		string _host;

		/// <summary>
		/// Gets the name of the application.
		/// </summary>
		/// <value></value>
		public string ApplicationName
		{
			get { return _applicationName; }
		}

		string _applicationName;

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "A blog matching the location you requested was not found. Host = [{0}], Application (Blog Name) = [{1}]", Host, ApplicationName);
			}
		}

	}
}
