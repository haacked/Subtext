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
	/// Summary description for EntryView.
	/// </summary>
	[Serializable]
	public class EntryView
	{
		private int _blogID;
		public int BlogId
		{
			get {return this._blogID;}
			set {this._blogID = value;}
		}

		private int _entryID;
		public int EntryId
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
