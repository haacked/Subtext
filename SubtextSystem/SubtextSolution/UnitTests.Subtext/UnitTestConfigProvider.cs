using System;
using System.Web;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Summary description for UnitTestConfigProvider.
	/// </summary>
	public class UnitTestConfigProvider : UrlBasedBlogInfoProvider
	{
		static BlogInfo _config = new BlogInfo();

		/// <summary>
		/// Gets a dummy config object for the purpose of unit testing.
		/// </summary>
		/// <remarks>
		/// Will look for the configuration in the cache first using the 
		/// key "BlogConfig-".
		/// </remarks>
		/// <param name="context">Context.</param>
		/// <returns></returns>
		public override BlogInfo GetBlogInfo(HttpContext context)
		{
			return _config;	
		}

	}
}
