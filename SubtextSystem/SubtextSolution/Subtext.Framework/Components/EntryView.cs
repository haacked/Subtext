using System;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for EntryView.
	/// </summary>
	[Serializable]
	public class EntryView
	{
		public EntryView()
		{

		}

		private int _blogID;
		public int BlogID
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private int _entryID;
		public int EntryID
		{
			get {return this._entryID;}
			set {this._entryID = value;}
		}

		private string _referralUrl;
		public string ReferralUrl
		{
			get {return this._referralUrl;}
			set {this._referralUrl = value;}
		}

		private PageViewType _pageViewType;
		public PageViewType PageViewType
		{
			get {return this._pageViewType;}
			set {this._pageViewType = value;}
		}
	}

	public enum PageViewType : byte
	{
		AggView = 0,
		WebView = 1
	}
}
