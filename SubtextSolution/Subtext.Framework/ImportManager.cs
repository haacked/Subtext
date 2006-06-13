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

using System.Web.UI;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Providers;
using Subtext.Framework.UI.Skinning;
using SkinConfig = Subtext.Framework.Configuration.SkinConfig;

namespace Subtext.Framework
{
	/// <summary>
	/// Summary description for ImportManager.
	/// </summary>
	public static class ImportManager
	{
		/// <summary>
		/// Gets the import information control for the specified import provider.
		/// </summary>
		/// <param name="providerInfo">Provider info.</param>
		/// <returns></returns>
		public static Control GetImportInformationControl(ProviderInfo providerInfo)
		{
			return ImportProvider.Instance(providerInfo).GatherImportInformation();
		}

		/// <summary>
		/// Validates the import information provided by the user.  
		/// Returns a string with error information.  The string is 
		/// empty if there are no errors.
		/// </summary>
		/// <param name="populatedControl">Information.</param>
		/// <param name="providerInfo"></param>
		/// <returns></returns>
		public static string ValidateImportAnswers(Control populatedControl, ProviderInfo providerInfo)
		{
			return ImportProvider.Instance(providerInfo).ValidateImportInformation(populatedControl);
		}

		/// <summary>
		/// Begins the import using the information within the populated Control.
		/// </summary>
		/// <param name="populatedControl">Control containing the user's answers.</param>
		public static void Import(Control populatedControl, ProviderInfo providerInfo)
		{
			ImportProvider.Instance(providerInfo).Import(populatedControl);

            IPagedCollection<BlogInfo> blogs = null;
			ObjectProvider objProvider = ObjectProvider.Instance();

			int totalBlogCount = Config.BlogCount;
			const int pageSize = 100;
			int pages = totalBlogCount/pageSize;
			int currentPage = 1;
			SkinTemplates skins = SkinTemplates.Instance();

			if (totalBlogCount % pageSize > 0)
			{
				pages++;
			}
			
			while (currentPage <= pages)
			{
				blogs = objProvider.GetPagedBlogs(currentPage, pageSize, true);

				foreach(BlogInfo currentBlogInfo in blogs)
				{
					if (skins.GetTemplate(currentBlogInfo.Skin.SkinName) == null)
					{
						currentBlogInfo.Skin = SkinConfig.GetDefaultSkin();
						Config.UpdateConfigData(currentBlogInfo);
					}
				}

				currentPage++;
			}
		}
	}
}
