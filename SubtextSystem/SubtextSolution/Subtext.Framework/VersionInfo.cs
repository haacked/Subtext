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
using System.Globalization;

namespace Subtext.Framework
{
	/// <summary>
	/// Class used to manage version information regarding 
	/// Subtext.
	/// </summary>
	public sealed class VersionInfo 
	{
		private VersionInfo() {}

		static Version _version = null;
		static Uri _homePageUrl = new Uri("http://SubtextProject.com/");

		/// <summary>
		/// Gets the framework version.
		/// </summary>
		/// <value></value>
		public static Version FrameworkVersion
		{
			get
			{
				if(_version == null)
				{
					_version = typeof(VersionInfo).Assembly.GetName().Version;
				}
				return _version;
			}
		}

		/// <summary>
		/// Gets the home page.
		/// </summary>
		/// <value></value>
		public static Uri HomePageUrl
		{
			get
			{
				return _homePageUrl;
			}
		}

		/// <summary>
		/// Gets version information that is formatted for display.
		/// </summary>
		/// <value></value>
		public static string VersionDisplayText
		{
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "Subtext Version {0}", FrameworkVersion);
			}
		}

		/// <summary>
		/// Returns a display string 
		/// </summary>
		/// <returns></returns>
		public override string ToString()
		{
			return VersionDisplayText;
		}
	
		/// <summary>
		/// Gets the user agent.
		/// </summary>
		/// <value></value>
		public static string UserAgent 
		{ 
			get
			{
				return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", VersionDisplayText, HomePageUrl);
			} 
		}
	}

}

