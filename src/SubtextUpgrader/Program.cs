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
using System.IO;
using System.Reflection;
using System.Windows.Forms;

namespace SubtextUpgrader {
	/// <summary>
	/// This tool is used to help upgrade existing installations of 
	/// Subtext to the latest version.
	/// </summary>
	public class Program {
		//TODO: Consider Replace Assembly="Subtext.Web.Controls" with Assembly="Subtext.Web" 
		//      in all skin files.
		[STAThread]
		static void Main(string[] args) {
			if (args.Length == 0) {
				Application.EnableVisualStyles();
				Application.SetCompatibleTextRenderingDefault(false);
				Application.Run(new UpgradeForm());
				return;
			}
		}
	}
}
