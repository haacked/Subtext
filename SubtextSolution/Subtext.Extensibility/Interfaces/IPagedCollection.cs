using System;
using System.Collections.Generic;

namespace Subtext.Extensibility.Interfaces
{
    /// <summary>
    /// Base interface for paged collections.
    /// </summary>
    public interface IPagedCollection
    {
    	/// <summary>
    	/// The Total number of items being paged through.
    	/// </summary>
        int MaxItems
        {
            get;
            set;
        }
    }

	/// <summary>
	/// Base interface for generic paged collections.
	/// </summary>
	public interface IPagedCollection<T> : IList<T>, IPagedCollection
	{}
}
