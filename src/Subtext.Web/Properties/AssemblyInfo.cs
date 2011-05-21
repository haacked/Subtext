#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Web.UI;
using log4net.Config;

//
// General Information about an assembly is controlled through the following 
// set of attributes. Change these attribute values to modify the information
// associated with an assembly.
//

[assembly: AssemblyTitle("Subtext.Web")]
[assembly: AssemblyDescription("ASP.NET Website implementation implented as a Class Library rather than a Web Project.")
]
[assembly: AssemblyCulture("")]
[assembly: ComVisible(false)]
[assembly: CLSCompliant(false)]
[assembly: XmlConfigurator(ConfigFile = "Log4Net.config", Watch = true)]
[assembly: WebResource("Subtext.Web.Controls.Resources.InvisibleCaptcha.js", "text/javascript")]