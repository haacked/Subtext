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
using System.Web.UI;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Configuration;
using Subtext.Framework.UI.Skinning;
using SkinConfig = Subtext.Framework.Configuration.SkinConfig;
using Subtext.Framework.Properties;

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
		/// <param name="provider">Provider info.</param>
		/// <returns></returns>
		public static Control GetImportInformationControl(ImportProvider provider)
		{
            if (provider == null)
                throw new ArgumentNullException("provider", Resources.ArgumentNull_Generic);

            return provider.GatherImportInformation();
		}

		/// <summary>
		/// Validates the import information provided by the user.  
		/// Returns a string with error information.  The string is 
		/// empty if there are no errors.
		/// </summary>
		/// <param name="populatedControl">Information.</param>
		/// <param name="provider"></param>
		/// <returns></returns>
        public static string ValidateImportAnswers(Control populatedControl, ImportProvider provider)
		{
            if (populatedControl == null)
                throw new ArgumentNullException("populatedControl", Resources.ArgumentNull_Generic);

            if (provider == null)
				throw new ArgumentNullException("provider", Resources.ArgumentNull_Generic);

            return provider.ValidateImportInformation(populatedControl);
		}

		/// <summary>
		/// Begins the import using the information within the populated Control.
		/// </summary>
		/// <param name="populatedControl">Control containing the user's answers.</param>
		/// <param name="provider">The provider.</param>
        public static void Import(Control populatedControl, ImportProvider provider)
		{
            provider.Import(populatedControl);

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
				IPagedCollection<BlogInfo> blogs = BlogInfo.GetBlogs(currentPage, pageSize, ConfigurationFlags.IsActive);

				foreach(BlogInfo currentBlogInfo in blogs)
				{
					if (skins.GetTemplate(currentBlogInfo.Skin.TemplateFolder) == null)
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
