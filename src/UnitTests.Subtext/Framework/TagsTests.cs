using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework
{
    [TestClass]
    public class TagsTests
    {
        [DataTestMethod]
        [DataRow(-1, 1, 1)]
        [DataRow(0, 1, 2)]
        [DataRow(.25, 1, 3)]
        [DataRow(.49, 1, 4)]
        [DataRow(.9, 1, 5)]
        [DataRow(1.9, 1, 6)]
        [DataRow(2, 1, 7)]
        public void CanComputeWeight(double factor, double stdDev, int expected)
        {
            Assert.AreEqual(expected, Tags.ComputeWeight(factor, stdDev));
        }

        [TestMethod]
        public void GetTopTagsThrowsArgumentExceptionForNegativeValues()
        {
            var repository = new DatabaseObjectProvider();
            UnitTestHelper.AssertThrows<ArgumentException>(() => repository.GetMostUsedTags(-1));
        }

        [DatabaseIntegrationTestMethod]
        public void GetGetTopTags()
        {
            UnitTestHelper.SetupBlog();
            var repository = new DatabaseObjectProvider();
            Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "the title for this post", "test");
            UnitTestHelper.Create(entry);
            repository.SetEntryTagList(entry.Id, new List<string>(new[] { "tag1", "tag2", "tag3" }));

            entry = UnitTestHelper.CreateEntryInstanceForSyndication("test", "the title for this post",
                                                                     @"<a href=""http://blah/tag3/"" rel=""tag"">test</a>");
            UnitTestHelper.Create(entry);

            ICollection<Tag> topTags = repository.GetMostUsedTags(1);
            Assert.AreEqual("tag3", topTags.First().TagName);
        }
    }
}