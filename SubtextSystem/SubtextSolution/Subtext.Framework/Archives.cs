using System;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
	/// <summary>
	/// Static class used to get collections of archived posts 
	/// (by month and year) from the data provider.
	/// </summary>
	public sealed class Archives
	{
		private Archives()
		{
		}

		/// <summary>
		/// Gets archived posts by month.
		/// </summary>
		/// <returns></returns>
		public static ArchiveCountCollection GetPostsByMonthArchive()
		{
			return DTOProvider.Instance().GetPostsByMonthArchive();
		}

		/// <summary>
		/// Gets archived posts by year.
		/// </summary>
		/// <returns></returns>
		public static ArchiveCountCollection GetPostsByYearArchive()
		{
			return DTOProvider.Instance().GetPostsByYearArchive();
		}
	}
}
