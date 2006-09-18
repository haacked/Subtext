using System;

namespace Subtext.Extensibility.Interfaces
{
	/// <summary>
	/// Interface for classes that can be identified by an integer ID.
	/// </summary>
	public interface IIdentifiable
	{
		int Id { get; }
	}
}
