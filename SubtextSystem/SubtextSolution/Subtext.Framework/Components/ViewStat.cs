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
		public int BlogID
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}
	}
}
