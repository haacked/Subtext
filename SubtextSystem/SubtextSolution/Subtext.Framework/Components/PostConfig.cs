using System;

namespace Subtext.Framework.Components
{
	/// <summary>
	/// Summary description for PostConfig.
	/// </summary>
	
	[Flags()]
	public enum PostConfig
	{
		Empty = 0,
		IsActive = 1,
		IsXHTML = 2,
		AllowComments = 4,
		DisplayOnHomePage = 8,
		IncludeInMainSyndication = 16,
		SyndicateDescriptionOnly = 32,
		IsAggregated = 64

	}
}
