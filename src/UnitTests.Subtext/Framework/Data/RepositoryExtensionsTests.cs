using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using MbUnit.Framework;
using Moq;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Providers;

namespace UnitTests.Subtext.Framework.Data
{
    [TestFixture]
    public class RepositoryExtensionsTests
    {
        [Test] //TODO Review
        public void GroupByDayUsingDateCreated_WithEntriesOnMultipleDays_GroupsEntriesByDay()
        {
            // arrange
            var entries = new List<Entry>
            {
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/23 12:34", "yyyy/MM/dd hh:mm", CultureInfo.InvariantCulture), Title="First Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/23 09:54", "yyyy/MM/dd hh:mm", CultureInfo.InvariantCulture), Title="Second Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/24", "yyyy/MM/dd", CultureInfo.InvariantCulture), Title="Third Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/25", "yyyy/MM/dd", CultureInfo.InvariantCulture), Title="Fourth Entry"}
            };

            // act
            var entryDays = entries.GroupByDayUsingDateSyndicated();

            // assert
            Assert.AreEqual(3, entryDays.Count());
            var firstDay = entryDays.First();
            Assert.AreEqual(2, firstDay.Count);
            Assert.AreEqual("First Entry", firstDay[0].Title);
            Assert.AreEqual("Second Entry", firstDay[1].Title);
            var secondDay = entryDays.ElementAt(1);
            Assert.AreEqual("Third Entry", secondDay[0].Title);
            var thirdDay = entryDays.ElementAt(2);
            Assert.AreEqual("Fourth Entry", thirdDay[0].Title);
        }

        [Test]
        public void GetBlogPostsForHomePage_GroupsPostsByDateCreated()
        {
            // arrange
            var entries = new List<Entry>
            {
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/23 12:34", "yyyy/MM/dd hh:mm", CultureInfo.InvariantCulture), Title="First Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/23 09:54", "yyyy/MM/dd hh:mm", CultureInfo.InvariantCulture), Title="Second Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/24", "yyyy/MM/dd", CultureInfo.InvariantCulture), Title="Third Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/25", "yyyy/MM/dd", CultureInfo.InvariantCulture), Title="Fourth Entry"}
            };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntries(10, PostType.BlogPost, PostConfig.None, false)).Returns(entries);

            // act
            var entryDays = repository.Object.GetBlogPostsForHomePage(10, PostConfig.None);

            // assert
            Assert.AreEqual(3, entryDays.Count());
            var firstDay = entryDays.First();
            Assert.AreEqual(2, firstDay.Count);
            Assert.AreEqual("First Entry", firstDay[0].Title);
            Assert.AreEqual("Second Entry", firstDay[1].Title);
            var secondDay = entryDays.ElementAt(1);
            Assert.AreEqual("Third Entry", secondDay[0].Title);
            var thirdDay = entryDays.ElementAt(2);
            Assert.AreEqual("Fourth Entry", thirdDay[0].Title);
        }

        [Test] //todo review
        public void GetBlogPostsByCategoryGroupedByDay_GroupsPostsByDateCreated()
        {
            // arrange
            var entries = new List<Entry>
            {
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/23 12:34", "yyyy/MM/dd hh:mm", CultureInfo.InvariantCulture), Title="First Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/23 09:54", "yyyy/MM/dd hh:mm", CultureInfo.InvariantCulture), Title="Second Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/24", "yyyy/MM/dd", CultureInfo.InvariantCulture), Title="Third Entry"},
                new Entry(PostType.BlogPost) {DatePublishedUtc = DateTime.ParseExact("2008/01/25", "yyyy/MM/dd", CultureInfo.InvariantCulture), Title="Fourth Entry"}
            };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.GetEntriesByCategory(10, 1, true)).Returns(entries);

            // act
            var entryDays = repository.Object.GetBlogPostsByCategoryGroupedByDay(10, 1);

            // assert
            Assert.AreEqual(3, entryDays.Count());
            var firstDay = entryDays.First();
            Assert.AreEqual(2, firstDay.Count);
            Assert.AreEqual("First Entry", firstDay[0].Title);
            Assert.AreEqual("Second Entry", firstDay[1].Title);
            var secondDay = entryDays.ElementAt(1);
            Assert.AreEqual("Third Entry", secondDay[0].Title);
            var thirdDay = entryDays.ElementAt(2);
            Assert.AreEqual("Fourth Entry", thirdDay[0].Title);
        }
    }
}
