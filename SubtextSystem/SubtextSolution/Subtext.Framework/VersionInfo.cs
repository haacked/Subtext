#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

#region Notes
 ///////////////////////////////////////////////////////////////////////////////////////////////////
 // The code in this file is freely distributable.
 // 
 // ASPNetWeblog is not responsible for, shall have no liability for 
 // and disclaims all warranties whatsoever, expressed or implied, related to this code,
 // including without limitation any warranties related to performance, security, stability,
 // or non-infringement of title of the control.
 // 
 // If you have any questions, comments or concerns, please contact
 // Scott Watermasysk, Scott@TripleASP.Net.
 // 
 // For more information on this control, updates, and other tools to integrate blogging 
 // into your existing applications, please visit, http://aspnetweblog.com
 // 
 // 
 ///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;

namespace Subtext.Framework
{
	/// <summary>
	/// Summary description for VersionInfo.
	/// </summary>
	public sealed class VersionInfo 
	{
		private VersionInfo() {}

		private static Version _version = null;

		public static Version FrameworkVersion
		{
			get
			{
				if(_version == null)
				{
					string  a = Type.GetType("Subtext.Framework.VersionInfo").Assembly.Location;
					_version = System.Reflection.AssemblyName.GetAssemblyName(a).Version;

				}
				return _version;
			}

		}

		public static string HomePage
		{
			get
			{
				return "http://haacked.com/";
			}
		}

		public static string Version
		{
			get
			{
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "Subtext Version {0}", FrameworkVersion);
			}
		}

		public override string ToString()
		{
			return Version;
		}
	
		public static string UserAgent 
		{ 
			get
			{
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0} - {1}",Version,HomePage);
			} 
		}
		
		public static string Display
		{
			get
			{
				return "<a href = \"http://Subtext.com\">" + Version + "</a>";
			}
		}
	}

}

