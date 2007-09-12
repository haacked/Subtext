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

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Used 
	/// </summary>
	public class RelatedLink
	{
		private string _title;
		private string _url;

		public RelatedLink(string title, string URL)
		{
			_title = title;
			_url = URL;
		}

		public string Title
		{
			get { return _title; }
		}

		public string url
		{
			get { return _url; }
		}
	}
}
