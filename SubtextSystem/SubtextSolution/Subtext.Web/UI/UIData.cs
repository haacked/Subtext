using System;
using Subtext.Common.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Format;

namespace Subtext.Web.UI
{
	/// <summary>
	/// Summary description for UIData.
	/// </summary>
	public class UIData
	{
		public static LinkCategory Links(CategoryType catType, UrlFormats formats)
		{
			switch(catType)
			{
				case CategoryType.PostCollection:
					return Transformer.BuildLinks(UIText.PostCollection,CategoryType.PostCollection,formats);
				case CategoryType.ImageCollection:
					return Transformer.BuildLinks(UIText.ImageCollection,CategoryType.ImageCollection,formats);
				case CategoryType.StoryCollection:
					return Transformer.BuildLinks(UIText.ArticleCollection,CategoryType.StoryCollection,formats);
				default:
					throw new ApplicationException(string.Format("Invalid CategoryType: {0} via Subtext.Web.UI.UIData.Links",catType));
			}
		}

		public static LinkCategory ArchiveMonth(UrlFormats formats)
		{
			return Transformer.BuildMonthLinks(UIText.Archives,formats);
		}
	}
}
