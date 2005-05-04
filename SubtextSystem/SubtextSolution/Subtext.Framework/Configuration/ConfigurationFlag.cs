using System;

namespace Subtext.Framework.Configuration
{

	[Flags()]
	public enum ConfigurationFlag
	{
		Empty = 0,
		IsActive = 1,
		IsAggregated = 2,
		EnableServiceAccess = 4,
		IsPasswordHashed = 8,
		EnableComments = 16,
		PublishAsNew = 32
	};
}
