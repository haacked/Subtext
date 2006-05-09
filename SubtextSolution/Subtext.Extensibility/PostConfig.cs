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

namespace Subtext.Extensibility
{
	/// <summary>
	/// Summary description for PostConfig.
	/// </summary>
	[Flags()]
	public enum PostConfig
	{
		None = 0,
		IsActive = 1,
		IsXHTML = 2,
		AllowComments = 4,
		DisplayOnHomePage = 8,
		IncludeInMainSyndication = 16,
		SyndicateDescriptionOnly = 32,
		IsAggregated = 64,
		CommentsClosed = 128,
	}
}
