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
	/// Represents the stats for one view of an entry.
	/// </summary>
	[Serializable]
	public class EntryView
	{
		public int BlogId
		{
			get {return this.blogId;}
			set {this.blogId = value;}
		}
		private int blogId = NullValue.NullInt32;

		public int EntryId
		{
			get {return this.entryId;}
			set {this.entryId = value;}
		}
		private int entryId = NullValue.NullInt32;

		
		public string ReferralUrl
		{
			get {return this.referralUrl;}
			set {this.referralUrl = value;}
		}
		private string referralUrl;

		public PageViewType PageViewType
		{
			get {return this.pageViewType;}
			set {this.pageViewType = value;}
		}
		private PageViewType pageViewType;
	}
}
