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
using Subtext.Framework.Data;
using Subtext.Framework.Components;
using Subtext.Framework.Format;

namespace Subtext.Web.UI
{
	/// <summary>
	/// Summary description for UIData.
	/// </summary>
	public static class UIData
	{
		public static LinkCategory Links(CategoryType catType, UrlFormats formats)
		{
			switch(catType)
			{
				case CategoryType.PostCollection:
					return Transformer.BuildLinks(UIText.PostCollection, CategoryType.PostCollection, formats);
				
				case CategoryType.ImageCollection:
					return Transformer.BuildLinks(UIText.ImageCollection, CategoryType.ImageCollection, formats);
				
				case CategoryType.StoryCollection:
					return Transformer.BuildLinks(UIText.ArticleCollection, CategoryType.StoryCollection, formats);
				
				default:
					throw new InvalidOperationException(string.Format(System.Globalization.CultureInfo.InvariantCulture, "Invalid CategoryType: {0} via Subtext.Web.UI.UIData.Links",catType));
			}
		}

		/// <summary>
		/// Builds a <see cref="LinkCategory"/> using the specified url formats. 
		/// A LinkCategory is a common item to databind to a web control.
		/// </summary>
		/// <param name="formats">Determines how the links are formatted.</param>
		/// <returns></returns>
		public static LinkCategory ArchiveMonth(UrlFormats formats)
		{
			return Transformer.BuildMonthLinks(UIText.Archives, formats);
		}

    /// <summary>
    /// Builds a <see cref="LinkCategory"/> using the specified url formats. 
    /// A LinkCategory is a common item to databind to a web control.
    /// </summary>
    /// <param name="formats">Determines how the links are formatted.</param>
    /// <returns></returns>
    public static LinkCategory ArchiveCategory(UrlFormats formats)
    {
        return Transformer.BuildCategoriesArchiveLinks(UIText.PostCollection, formats);
    }
	}
}
