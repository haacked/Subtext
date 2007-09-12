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

namespace Subtext.Framework.Security
{
	/// <summary>
	/// Facade for testing the various rights of roles.
	/// </summary>
	public static class Rights
	{
		/// <summary>
		/// If true, the user may post comments without being moderated.
		/// </summary>
		/// <returns></returns>
		public static bool CanPostComment()
		{
			return (SecurityHelper.IsInRole(RoleNames.Commenters)
				|| SecurityHelper.IsInRole(RoleNames.Administrators)
				|| SecurityHelper.IsInRole(RoleNames.Authors)
				|| SecurityHelper.IsInRole(RoleNames.PowerUsers));
		}
	}
}
