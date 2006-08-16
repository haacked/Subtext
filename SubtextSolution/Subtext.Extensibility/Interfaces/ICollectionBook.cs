using System;
using System.Collections.Generic;

namespace Subtext.Extensibility.Interfaces
{
	public interface ICollectionBook<T> : IEnumerable<IPagedCollection<T>>
	{
	}

	/// <summary>
	/// Method used to retrieve a page of data from the data source.
	/// </summary>
	/// <typeparam name="T"></typeparam>
	/// <param name="pageIndex"></param>
	/// <param name="pageSize"></param>
	/// <returns></returns>
	public delegate IPagedCollection<T> PagedCollectionSource<T>(int pageIndex, int pageSize);
}
