using System;
using System.Collections.Generic;
using System.Text;

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
