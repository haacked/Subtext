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
using CookComputing.XmlRpc;
using Subtext.Framework.Components;
using Subtext.Framework.Util;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for PingBackService.
	/// </summary>
	public class PingBackService : XmlRpcService
	{
		[XmlRpcMethod("pingback.ping", Description="Pingback server implementation")] 
		public string pingBack(string sourceURI, string targetURI)
		{
		
			int 	postId 		= 0 ;
			string 	pageTitle 	= "" ;
  		
			// GetPostIDFromUrl returns the postID
			postId = Globals.GetPostIDFromUrl(targetURI);
			if ( postId == -1 )
				throw new XmlRpcFaultException(33, "You did not link to a permalink");
  			  		
			// does the sourceURI actually contain the permalink ?
			if ( ! Verifier.SourceContainsTarget(sourceURI, targetURI, out pageTitle) )
				throw new XmlRpcFaultException(17, "Not a valid link.") ;		
  			
			//PTR = Pingback - TrackBack - Referral

			Entry entry = new Entry(PostType.PingTrack);
			entry.ParentID = postId;
			entry.Title = Globals.SafeFormat(pageTitle);
			entry.TitleUrl = Globals.SafeFormat(sourceURI);
			entry.Body = Globals.SafeFormat(pageTitle);
			entry.DateCreated = entry.DateUpdated = DateTime.Now;
			entry.IsActive = true;
			Entries.Create(entry);
  		
			return "thanks for the pingback on " + sourceURI ;
		}
  

	}
}

