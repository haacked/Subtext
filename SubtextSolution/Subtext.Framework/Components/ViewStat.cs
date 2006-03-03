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

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for ViewStat.
	/// </summary>
	[Serializable]
	public class ViewStat
	{
		private string _pageTitle;
		private int _viewCount;
		private DateTime _viewDate;
		private PageType _pageType = PageType.NotSpecified;


		public string PageTitle
		{
			get { return _pageTitle; }
			set { _pageTitle = value; }
		}

		public int ViewCount
		{
			get { return _viewCount; }
			set { _viewCount = value; }
		}

		public DateTime ViewDate
		{
			get { return _viewDate; }
			set { _viewDate = value; }
		}

		public PageType PageType
		{
			get { return _pageType; }
			set { _pageType = value; }
		}

		private int _blogID;
		public int BlogId
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}
	}
}
