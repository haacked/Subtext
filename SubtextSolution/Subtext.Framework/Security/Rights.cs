using System;

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
