using System;
using System.Collections.Generic;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Framework.Components
{
    /// <summary>
    /// Concrete generic base class for paged collections.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class PagedCollection<T> : List<T>, IPagedCollection<T>
    {
        private int maxItems;

        /// <summary>
        /// Returns the max number of items to display on a page.
        /// </summary>
        public int MaxItems
        {
            get { return this.maxItems; }
            set { this.maxItems = value; }
        }
    }
}
