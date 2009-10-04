using System.Collections.Generic;

namespace Subtext.Extensibility.Interfaces
{
    /// <summary>
    /// Base interface for generic paged collections.
    /// </summary>
    public interface IPagedCollection<T> : IList<T>
    {
        int MaxItems { get; set; }
    }
}