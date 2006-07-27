using System;
using System.Collections.Generic;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Base interface for paged collections.
    /// </summary>
    public interface IPagedCollection
    {
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
