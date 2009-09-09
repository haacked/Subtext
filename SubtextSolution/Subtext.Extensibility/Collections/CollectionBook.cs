#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
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
        int pageSize;
        Func<int, int, IPagedCollection<T>> pageSource;

        /// <summary>
        /// Initializes a new instance of the <see cref="CollectionBook<T>"/> class.
        /// </summary>
        /// <param name="pageSource">The page source.</param>
        public CollectionBook(Func<int, int, IPagedCollection<T>> pageSource, int pageSize)
        {
            this.pageSource = pageSource;
            this.pageSize = pageSize;
        }

        #region ICollectionBook<T> Members

        ///<summary>
        ///Returns an enumerator that iterates through the collection.
        ///</summary>
        ///<returns>
        ///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
        ///</returns>
        ///<filterpriority>1</filterpriority>
        public IEnumerator<IPagedCollection<T>> GetEnumerator()
        {
            if(pageSize <= 0)
            {
                throw new InvalidOperationException(Resources.InvalidOperation_PageSizeLessThanZero);
            }

            int pageIndex = 0;
            int pageCount = 0;

            if(pageCount == 0)
            {
                IPagedCollection<T> page = pageSource(pageIndex, pageSize);
                pageCount = (int)Math.Ceiling((double)page.MaxItems / pageSize);
                yield return page;
            }

            //We've already yielded page 0, so start at 1
            while(++pageIndex < pageCount)
            {
                yield return pageSource(pageIndex, pageSize);
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

        #endregion
    }
}