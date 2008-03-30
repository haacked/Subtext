using System;
using System.Collections.ObjectModel;
using MbUnit.Framework;
using Subtext.Extensibility.Collections;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components
{
	[TestFixture]
	public class CollectionBookTests
	{
		[Test]
		public void CanIteratePagedCollectionsInABook()
		{
			Collection<IPagedCollection<string>> pages = new Collection<IPagedCollection<string>>();
			IPagedCollection<string> pageZero = new PagedCollection<string>();
			pageZero.MaxItems = 8;
			pageZero.Add("zero");
			pageZero.Add("one");
			pageZero.Add("two");
			
			pages.Add(pageZero);
			IPagedCollection<string> pageOne = new PagedCollection<string>();
			pageOne.MaxItems = 8;
			pageOne.Add("three");
			pageOne.Add("four");
			pageOne.Add("five");
			pages.Add(pageOne);

			IPagedCollection<string> pageTwo = new PagedCollection<string>();
			pageTwo.MaxItems = 8;
			pageTwo.Add("six");
			pageTwo.Add("seven");
			pages.Add(pageTwo);

			CollectionBook<string> book = new CollectionBook<string>(
				delegate(int pageIndex, int pageSize)
				{
					return pages[pageIndex];
				}, 3);

			string concatenation = string.Empty;
			int currentPageIndex = 0;
			foreach(IPagedCollection<string> page in book)
			{
				concatenation += currentPageIndex;
				foreach(string item in page)
				{
					concatenation += item;
				}
				currentPageIndex++;
			}
			Assert.AreEqual("0zeroonetwo1threefourfive2sixseven", concatenation, "We iterated correctly.");
		}
	}
}
