using System;
using System.Collections.ObjectModel;
using System.Linq;
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
        public void CollectionBook_WithThreePages_IteratesCorrectly()
        {
            var pages = new Collection<IPagedCollection<string>>();
            IPagedCollection<string> pageZero = new PagedCollection<string> {MaxItems = 8};
            pageZero.Add("zero");
            pageZero.Add("one");
            pageZero.Add("two");
            pages.Add(pageZero);
            
            IPagedCollection<string> pageOne = new PagedCollection<string> {MaxItems = 8};
            pageOne.Add("three");
            pageOne.Add("four");
            pageOne.Add("five");
            pages.Add(pageOne);

            IPagedCollection<string> pageTwo = new PagedCollection<string> {MaxItems = 8};
            pageTwo.Add("six");
            pageTwo.Add("seven");
            pages.Add(pageTwo);

            var book = new CollectionBook<string>((pageIndex, pageSize) => pages[pageIndex], 3);

            string concatenation = string.Empty;
            int currentPageIndex = 0;
            
            // act
            foreach(var page in book)
            {
                concatenation += currentPageIndex;
                foreach(string item in page)
                {
                    concatenation += item;
                }
                currentPageIndex++;
            }

            // assert
            Assert.AreEqual("0zeroonetwo1threefourfive2sixseven", concatenation);
        }

        [Test]
        public void AsFlattenedEnumerable_AllowsEnumeratingAllPagesAsSingleEnumeration()
        {
            var pages = new Collection<IPagedCollection<string>>();
            IPagedCollection<string> pageZero = new PagedCollection<string> { MaxItems = 8 };
            pageZero.Add("zero");
            pageZero.Add("one");
            pageZero.Add("two");

            pages.Add(pageZero);
            IPagedCollection<string> pageOne = new PagedCollection<string> { MaxItems = 8 };
            pageOne.Add("three");
            pageOne.Add("four");
            pageOne.Add("five");
            pages.Add(pageOne);

            IPagedCollection<string> pageTwo = new PagedCollection<string> { MaxItems = 8 };
            pageTwo.Add("six");
            pageTwo.Add("seven");
            pages.Add(pageTwo);

            var book = new CollectionBook<string>((pageIndex, pageSize) => pages[pageIndex], 3);

            // act
            string concatenation = String.Join("", book.AsFlattenedEnumerable().ToArray());

            // assert
            Assert.AreEqual("zeroonetwothreefourfivesixseven", concatenation);
        }

    }
}