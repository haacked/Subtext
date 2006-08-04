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
	/// Exception thrown when a blog Import fails. This could be caused by 
	/// a BlogML failure, or any other means of importing data into a 
	/// subText blog. NOTE: this exception may need refactored/renamed 
	/// so it can be used for both import & export failures.
	/// </summary>
	[Serializable]
	public class BlogImportException : Exception 
	{
		string _importMessage;

		/// <summary>
		/// Creates a new <see cref="BlogImportException"/> instance.
		/// </summary>
		/// <param name="importFailureReason">This is our best guess as to why the import failed
		/// and we threw the error. i.e.- Could not find the BlogML file, The BlogML file is not 
		/// valid, etc...</param>
		public BlogImportException(string importFailureReason) : base()
		{
			_importMessage = importFailureReason;
		}
		/// <summary>
		/// Creates a new <see cref="BlogImportException"/> instance.
		/// </summary>
		/// <param name="importFailureReason">This is our best guess as to why the import failed
		/// and we threw the error. i.e.- Could not find the BlogML file, The BlogML file is not 
		/// valid, etc...</param>
		/// <param name="innerException"></param>
		public BlogImportException(string importFailureReason, Exception innerException) : base(null, innerException)
		{
			_importMessage = importFailureReason;
		}

		/// <summary>
		/// Gets the message.
		/// </summary>
		/// <value></value>
		public override string Message
		{
			get
			{
				return string.Format(
					System.Globalization.CultureInfo.InvariantCulture, 
					"There was an error trying to import data into this blog. The issue is most likely due to the following: {0}", ImportMessage);
			}
		}

		public string ImportMessage
		{
			get { return _importMessage; }
		}
	}
}
