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

using System.Collections.Generic;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
	/// <summary>
	/// Static class used to get collections of archived posts 
	/// (by month and year and category) from the data provider.
	/// </summary>
	public static class Archives
	{
		/// <summary>
		/// Gets archived posts by month.
		/// </summary>
		/// <returns></returns>
		public static IList<ArchiveCount> GetPostCountByMonth()
		{
			return ObjectProvider.Instance().GetPostsByMonthArchive();
		}

		/// <summary>
		/// Gets archived posts by year.
		/// </summary>
		/// <returns></returns>
        public static IList<ArchiveCount> GetPostCountByYear()
		{
			return ObjectProvider.Instance().GetPostsByYearArchive();
		}

        /// <summary>
        /// Gets archived posts by category.
        /// </summary>
        /// <returns></returns>
        public static IList<ArchiveCount> GetPostCountByCategory()
        {
            return ObjectProvider.Instance().GetPostsByCategoryArchive();
        }
	}
}
