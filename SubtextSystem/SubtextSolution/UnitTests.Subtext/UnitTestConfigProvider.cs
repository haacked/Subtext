using System;
using System.Web;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Summary description for UnitTestConfigProvider.
	/// </summary>
	public class UnitTestConfigProvider : UrlBasedConfigProvider
	{
		static BlogConfig _config = new BlogConfig();

		/// <summary>
		/// Gets a dummy config object for the purpose of unit testing.
		/// </summary>
		/// <remarks>
		/// Will look for the configuration in the cache first using the 
		/// key "BlogConfig-".
		/// </remarks>
		/// <param name="context">Context.</param>
		/// <returns></returns>
		public override BlogConfig GetConfig(HttpContext context)
		{
			return _config;	
		}

	}
}
