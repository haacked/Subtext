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
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.Permissions;
using System.Web.UI;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//
[assembly: AssemblyTitle("Subtext.Web.Controls")]
[assembly: AssemblyDescription("Standalone Reusable ASP.NET Web Controls.  These are designed to have few dependencies and can be reused elsewhere.")]
[assembly: AssemblyCulture("")]

[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: SecurityPermission(SecurityAction.RequestMinimum, Execution = true)]

[assembly: WebResource("Subtext.Web.Controls.Resources.InvisibleCaptcha.js", "text/javascript")]