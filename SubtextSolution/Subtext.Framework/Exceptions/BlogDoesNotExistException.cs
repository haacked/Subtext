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
	/// Exception thrown when a blog does not exist.  This is 
	/// indicative of not having created a blog_config record.
	/// </summary>
	[Serializable]
	public class BlogDoesNotExistException : Exception 
	{
		int id = NullValue.NullInt32;
		
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
			subfolder = app;
		}
		
		public BlogDoesNotExistException(int id) : base()
		{
			this.id = id;
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
		public string Subfolder
		{
			get { return subfolder; }
		}

		string subfolder;

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				if (id == NullValue.NullInt32)
				{
					return string.Format(System.Globalization.CultureInfo.InvariantCulture, "A blog matching the location you requested was not found. Host = [{0}], Subfolder = [{1}]", Host, Subfolder);
				}
				else
				{
					return string.Format(System.Globalization.CultureInfo.InvariantCulture, "There is no blog with the id {0}", this.id);
				}
			}
		}

		public override string ToString()
		{
			return Message + Environment.NewLine + base.ToString ();
		}


	}
}
