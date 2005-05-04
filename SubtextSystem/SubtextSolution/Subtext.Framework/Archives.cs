using System;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace Subtext.Framework
{
	/// <summary>
	/// Summary description for Archives.
	/// </summary>
	public class Archives
	{
		private Archives()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		public static ArchiveCountCollection GetPostsByMonthArchive()
		{
			
			return DTOProvider.Instance().GetPostsByMonthArchive();
		}

		public static ArchiveCountCollection GetPostsByYearArchive()
		{
			return DTOProvider.Instance().GetPostsByYearArchive();
		}
	}
}
