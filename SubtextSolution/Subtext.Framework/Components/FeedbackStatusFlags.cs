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

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Specifies the current status of a piece of feedback.
	/// </summary>
	[Flags]
	public enum FeedbackStatusFlags
	{
		None = 0,
		Approved = 1,
		NeedsModeration = 2,
		ApprovedByModerator = Approved | NeedsModeration,
		FlaggedAsSpam = 4,
		FalsePositive = FlaggedAsSpam | Approved,
		Deleted = 8,
		ConfirmedSpam = FlaggedAsSpam | Deleted,
	}
}
