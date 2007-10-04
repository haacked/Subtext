using System;
using System.Security.Permissions;

namespace Subtext.Web.HostAdmin
{
	[PrincipalPermission(SecurityAction.Demand, Role = "HostAdmins")]
	public class HostAdminPage : System.Web.UI.Page
	{
	}
}
