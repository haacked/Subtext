using System;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for PagedReferrerCollection.
	/// </summary>
	public class PagedReferrerCollection : ReferrerCollection, IPagedResults
	{
		public PagedReferrerCollection()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		private int _maxItems;
		public int MaxItems
		{
			get {return this._maxItems;}
			set {this._maxItems = value;}
		}
	}
}
