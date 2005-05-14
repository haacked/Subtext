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
 ///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Collections.Specialized;
using System.Text.RegularExpressions;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackHelpers.
	/// </summary>
	public class TrackHelpers
	{
		private TrackHelpers()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		//Text to insert into a file with pinkback service location
		public static string PingPackTag
		{
			get
			{
				return string.Format("<link rel=\"pingback\" href=\"{0}Services/Pingback.aspx\" />", Config.CurrentBlog.FullyQualifiedUrl);
			}
		}

		//Body of text to insert into a post with Trackback
		public static string TrackBackTag(Entry entry)
		{
			string nl = System.Environment.NewLine;
			string tag =	nl + "<!--" + nl + 
							"<rdf:RDF xmlns:rdf=\"http://www.w3.org/1999/02/22-rdf-syntax-ns#\"" + nl +
							"xmlns:dc=\"http://purl.org/dc/elements/1.1/\"" + "\n" + 
							"xmlns:trackback=\"http://madskills.com/public/xml/rss/module/trackback/\">" + nl + 
							"<rdf:Description" + nl + 
							"rdf:about=\"{0}\"" + nl + 
							"dc:identifier=\"{1}\"" + nl + 
							"dc:title=\"{2}\"" + nl + 
							"trackback:ping=\"{3}services/trackbacks/{4}.aspx\" />" + nl + 
							"</rdf:RDF>" + nl + 
							"-->" + nl;
			return string.Format(tag,entry.Link,entry.Link,entry.Title,Config.CurrentBlog.FullyQualifiedUrl,entry.EntryID.ToString());

		}

		//Gets a list of all of the valid html links from a string
		public static StringCollection GetLinks(string text)
		{			
			StringCollection links = new StringCollection();
			string sPattern = @"(?:[hH][rR][eE][fF]\s*=)" +
				@"(?:[\s""']*)(?!#|[Mm]ailto|[lL]ocation.|[jJ]avascript|.*css|.*this\.)" +
				@"(.*?)(?:[\s>""'])";

			Regex r = new Regex(sPattern,RegexOptions.IgnoreCase);
			Match m;
			string link = null;
			for (m = r.Match(text); m.Success; m = m.NextMatch()) 
			{
				
				if(m.Groups.ToString().Length > 0 )
				{
					
					link = 	m.Groups[1].ToString();	
					if(!links.Contains(link))
					{
						links.Add(link);
					}
				}
			}
			return links;	
		}
	}
}

