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
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
    [TestFixture]
    public class EntryTests
    {
        [Test]
        public void CommentingClosedByAge_WithCurrentDateAfterDaysTillClose_ReturnsTrue()
        {
            // arrange
            var now = DateTime.UtcNow;

            var entry = new Entry(PostType.BlogPost)
            {
                Id = 111,
                EntryName = "entry-slug",
                DatePublishedUtc = now.AddDays(-3),
                Blog = new Blog { DaysTillCommentsClose = 2 }
            };

            // act
            bool closed = entry.CommentingClosedByAge;

            // assert
            Assert.IsTrue(closed);
        }

        [Test]
        public void CommentingClosedByAge_WithCurrentDateBeforeDaysTillClose_ReturnsFalse()
        {
            // arrange
            var now = DateTime.UtcNow;

            var entry = new Entry(PostType.BlogPost)
            {
                Id = 111,
                EntryName = "entry-slug",
                DatePublishedUtc = now.AddDays(-3),
                Blog = new Blog { DaysTillCommentsClose = 4 }
            };

            // act
            bool closed = entry.CommentingClosedByAge;

            // assert
            Assert.IsFalse(closed);
        }

        [Test]
        public void CommentingClosedByAge_WithMaxDaysTillClose_ReturnsFalse()
        {
            // arrange
            var now = DateTime.UtcNow;

            var entry = new Entry(PostType.BlogPost)
            {
                Id = 111,
                EntryName = "entry-slug",
                DatePublishedUtc = now.AddDays(-9000),
                Blog = new Blog { DaysTillCommentsClose = int.MaxValue }
            };

            // act
            bool closed = entry.CommentingClosedByAge;

            // assert
            Assert.IsFalse(closed);
        }
    }
}
