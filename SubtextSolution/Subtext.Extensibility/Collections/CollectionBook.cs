using System;
using System.Collections;
using System.Collections.Generic;
using Subtext.Extensibility.Interfaces;

namespace Subtext.Extensibility.Collections
{
	public class CollectionBook<T> : ICollectionBook<T>
	{
		PagedCollectionSource<T> pageSource;
		int pageSize;

		/// <summary>
		/// Initializes a new instance of the CollectionBook class.
		/// </summary>
		/// <param name="pageSource">The page source.</param>
		public CollectionBook(PagedCollectionSource<T> pageSource, int pageSize)
		{
			this.pageSource = pageSource;
			this.pageSize = pageSize;
		}

		///<summary>
		///Returns an enumerator that iterates through the collection.
		///</summary>
		///<returns>
		///A <see cref="T:System.Collections.Generic.IEnumerator`1"></see> that can be used to iterate through the collection.
		///</returns>
		///<filterpriority>1</filterpriority>
		public IEnumerator<IPagedCollection<T>> GetEnumerator()
		{
			if (this.pageSize <= 0)
				throw new InvalidOperationException("Cannot iterate a page of size zero or less");
						
			int pageIndex = 0;
			int pageCount = 0;

			if (pageCount == 0)
			{
				IPagedCollection<T> page = pageSource(pageIndex, this.pageSize);
				pageCount = (int)Math.Ceiling((double)page.MaxItems / this.pageSize);
				yield return page;
			}

			//We've already yielded page 0, so start at 1
			while(++pageIndex < pageCount)
			{
				yield return pageSource(pageIndex, this.pageSize);
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
	}	
}
