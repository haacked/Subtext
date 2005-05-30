using System;

namespace Subtext.Framework.Configuration
{
	/// <summary>
	/// <p>Bitmask enumeration used to specify the several properties in one 
	/// value within the database.</p>
	/// </list>
	/// </summary>
	[Flags()]
	public enum ConfigurationFlag
	{
		/// <summary>Nothing is set</summary>
		Empty = 0,
		/// <summary>The Blog is Active</summary>
		IsActive = 1,
		/// <summary>The Blog has a syndicated feed (RSS or ATOM)</summary>
		IsAggregated = 2,
		/// <summary>The Blog can be accessed via XML over HTTP APIs</summary>
		EnableServiceAccess = 4,
		/// <summary>Whether or not the password is hashed.</summary>
		IsPasswordHashed = 8,
		/// <summary>Whether or not Comments are enabled.</summary>
		EnableComments = 16,
		/// <summary>Not sure.</summary>
		PublishAsNew = 32,
		/// <summary>The Blog compresses its syndicated feeds.</summary>
		CompressSyndicatedFeed = 64,
		/// <summary>Whether or not duplicate comments are allowed.</summary>
		EnableDuplicateComments = 128,
	};
}
