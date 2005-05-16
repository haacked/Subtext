using System;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for PagedViewStatCollection.
	/// </summary>
	public class PagedViewStatCollection : ViewStatCollection, IPagedResults
	{
		private int _maxItems;
		public int MaxItems
		{
			get {return this._maxItems;}
			set {this._maxItems = value;}
		}

	}
}
