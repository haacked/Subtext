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
using System.Web;
using Subtext.Framework;
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
		/// <returns></returns>
		public override BlogInfo GetBlogInfo()
		{
			return _config;	
		}

	}
}
