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

using System.Configuration.Provider;

namespace Subtext.Framework.Providers
{
/// <summary>
	/// This class is now obsolete. We're keeping this around for backwards compat, but it will get removed.
	/// </summary>
    //[Obsolete("DbProvider will be completely removed in a future version of Subtext. Remove the related sections for Web.config")]
    public abstract class DbProvider : ProviderBase
	{
	}
}
