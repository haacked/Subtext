using System;
using System.Collections.Generic;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Base interface for paged collections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IPagedCollection<T> : IList<T>
    {
        int MaxItems
        {
            get;
            set;
        }
    }
}
