using System;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Used 
	/// </summary>
	public class RelatedLink
	{
		private string title;
		private string URL;

		public RelatedLink(string title, string URL)
		{
			this.title = title;
			this.URL = URL;
		}

		public string Title
		{
			get { return title; }
		}

		public string url
		{
			get { return URL; }
		}
	}
}
