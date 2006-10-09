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
	/// Think of PostConfig items as filters. These values are often used in the 
	/// WHERE clause of stored procedures, for example.
	/// </summary>
	[Flags()]
	public enum PostConfig
	{
		None = 0,	//no filter. Therefore if getting items, all items will be gotten.
		IsActive = 1,	//filter returns only the active items
		IsXHTML = 2,
		AllowComments = 4,
		DisplayOnHomePage = 8,
		IncludeInMainSyndication = 16,
		SyndicateDescriptionOnly = 32,
		IsAggregated = 64,
		CommentsClosed = 128,
		NeedsModeratorApproval = 256,
	}
}
