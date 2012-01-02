#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using Subtext.Extensibility.Interfaces;
using Subtext.Extensibility.Properties;

namespace Subtext.Extensibility.Collections
{
    public class CollectionBook<T> : ICollectionBook<T>
    {
        readonly int _pageSize;
        readonly Func<int, int, IPagedCollection<T>> _pageSource;

        public CollectionBook(Func<int, int, IPagedCollection<T>> pageSource, int pageSize)
        {
            _pageSource = pageSource;
            _pageSize = pageSize;
        }

        public IEnumerator<IPagedCollection<T>> GetEnumerator()
        {
            if (_pageSize <= 0)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_PageSizeLessThanZero);
            }

            int pageIndex = 0;
            int pageCount = 0;

            if (pageCount == 0)
            {
                IPagedCollection<T> page = _pageSource(pageIndex, _pageSize);
                pageCount = (int)Math.Ceiling((double)page.MaxItems / _pageSize);
                yield return page;
            }

            //We've already yielded page 0, so start at 1
            while (++pageIndex < pageCount)
            {
                yield return _pageSource(pageIndex, _pageSize);
            }
        }

        ///<summary>
        ///Returns an enumerator that iterates through a collection.
        ///</summary>
        ///<returns>
        ///An <see cref="T:System.Collections.IEnumerator"></see> object that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>2</filterpriority>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<T> AsFlattenedEnumerable()
        {
            foreach (var page in this)
            {
                foreach (var item in page)
                {
                    yield return item;
                }
            }
        }
    }
}