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
using System.Globalization;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Format;

namespace Subtext.Common.Data
{
	/// <summary>
	/// Common Subtext object transformations
	/// </summary>
	public sealed class Transformer
	{
		private Transformer(){}

		/// <summary>
		/// Converts a LinkCategoryCollection into a single LinkCategory with its own LinkCollection.
		/// </summary>
		/// <param name="title">title for the LinkCategory</param>
		/// <param name="catType">Type of Categories to transform</param>
		/// <param name="formats">Determines how the Urls are formated</param>
		/// <returns></returns>
		public static LinkCategory BuildLinks(string title, CategoryType catType, UrlFormats formats)
		{
			LinkCategoryCollection lcc = Links.GetCategories(catType,true);
			LinkCategory lc = null;
			if(lcc != null && lcc.Count > 0)
			{
				lc = new LinkCategory();
				int count = lcc.Count;
				lc.Title = title;
				lc.Links = new LinkCollection();
				Link link;
				for(int i = 0; i < count; i++)
				{
					link = new Link();
					
					link.Title = lcc[i].Title;
					switch(catType)
					{
						case CategoryType.StoryCollection:
							link.Url =  formats.ArticleCategoryUrl(link.Title, lcc[i].CategoryID);
							break;
						
						case CategoryType.PostCollection:
							link.Url = formats.PostCategoryUrl(link.Title, lcc[i].CategoryID);
							link.Rss = link.Url + "/rss";
							break;
						
						case CategoryType.ImageCollection:
							link.Url = formats.GalleryUrl(link.Title, lcc[i].CategoryID);
							break;

					}
					link.NewWindow = false;
					lc.Links.Add(link);
				}
			}				
			return lc;
		}

		/// <summary>
		/// Will convert ArchiveCountCollection method from Archives.GetPostsByMonthArchive()
		/// into a <see cref="LinkCategory"/>. LinkCategory is a common item to databind to a web control.
		/// </summary>
		/// <param name="title">title for the Category</param>
		/// <param name="formats">Determines how the Urls are formated</param>
		/// <returns>A LinkCategory object with a Url (via LinkCollection) for each month in ArchiveCountCollection</returns>
		public static LinkCategory BuildMonthLinks(string title, UrlFormats formats)
		{
			ArchiveCountCollection acc = Archives.GetPostsByMonthArchive();

			LinkCategory lc = new LinkCategory();
			lc.Title = title;
			lc.Links = new LinkCollection();
			foreach(ArchiveCount ac in acc)
			{
				Link link = new Link();
				link.NewWindow = false;
				link.Title = ac.Date.ToString("y") + " (" + ac.Count.ToString(CultureInfo.InvariantCulture) + ")";
				link.Url = formats.MonthUrl(ac.Date);
				link.NewWindow = false;
				link.IsActive = true;

				lc.Links.Add(link);
			}
			return lc;
		}
	}
}
