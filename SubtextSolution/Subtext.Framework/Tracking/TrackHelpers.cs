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
 ///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion
using System;
using System.Globalization;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Contains static helper methods for dealing with Trackbacks and PingBacks.
	/// </summary>
	public static class TrackHelpers
	{
	    //Text to insert into a file with pinkback service location
		public static string PingPackTag
		{
			get
			{
				return string.Format(System.Globalization.CultureInfo.InvariantCulture, "<link rel=\"pingback\" href=\"{0}Services/Pingback.aspx\"></link>", Config.CurrentBlog.RootUrl);
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
			return string.Format(tag, entry.FullyQualifiedUrl, entry.FullyQualifiedUrl, entry.Title, Config.CurrentBlog.RootUrl, entry.Id.ToString(CultureInfo.InvariantCulture));

		}
	}
}