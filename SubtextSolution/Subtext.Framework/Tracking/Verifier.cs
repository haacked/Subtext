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

#region Notes
 ///////////////////////////////////////////////////////////////////////////////////////////////////
 // The code in this file is freely distributable.
 // 
 // ASPNetWeblog isnot responsible for, shall have no liability for 
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
 // Originally based off of code by Simon Fell http://www.pocketsoap.com/weblog/ 
 // 
 ///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Text.RegularExpressions;
using Subtext.Framework.Util;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for Verifier.
	/// </summary>
	public class Verifier
	{
		private Verifier()
		{
		}
		
		public static  bool SourceContainsTarget(string sURI, string tURI, out string pageTitle)
		{
			pageTitle = string.Empty ;
			try
			{


				string page = BlogRequest.GetPageText(sURI);
				if (page == null ||  page.IndexOf(tURI) < 0 )
					return false ;
						
				string pat = @"<head.*?>.*<title.*?>(.*)</title.*?>.*</head.*?>" ;
				Regex reg = new Regex(pat, RegexOptions.IgnoreCase | RegexOptions.Singleline  ) ;
				Match m = reg.Match(page) ;
				if ( m.Success )
				{
					pageTitle = m.Result("$1") ;
					return true ;
				}
			}
			finally
			{
			}
			return false ;
		}
	}
}

