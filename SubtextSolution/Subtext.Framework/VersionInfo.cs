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
    public static class VersionInfo
    {
        public static readonly Uri HomePageUrl = new Uri("http://SubtextProject.com/");
        static Version _version;

        /// <summary>
        /// Gets the version of the Subtext assembly.
        /// </summary>
        /// <value></value>
        public static Version CurrentAssemblyVersion
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
        /// Gets version information that is formatted for display.
        /// </summary>
        /// <value></value>
        public static string VersionDisplayText
        {
            get { return string.Format(CultureInfo.InvariantCulture, "Subtext Version {0}", CurrentAssemblyVersion); }
        }

        /// <summary>
        /// Gets the user agent.
        /// </summary>
        /// <value></value>
        public static string UserAgent
        {
            get { return string.Format(CultureInfo.InvariantCulture, "{0} - {1}", VersionDisplayText, HomePageUrl); }
        }
    }
}