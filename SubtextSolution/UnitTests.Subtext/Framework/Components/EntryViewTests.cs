using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Components
{
    [TestFixture]
    public class EntryViewTests
    {
        [Test]
        public void CanSetAndGetSimpleProperties()
        {
            EntryView view = new EntryView();
            UnitTestHelper.AssertSimpleProperties(view);
        }

        [Test]
        [RollBack2]
        public void CanSetAndGetSimpleEntryStatsViewProperties()
        {
            string host = UnitTestHelper.GenerateRandomString();
            Config.CreateBlog("title", "blah", "blah", host, string.Empty);
            UnitTestHelper.SetHttpContextWithBlogRequest(host, string.Empty);
            EntryStatsView view = new EntryStatsView();
            UnitTestHelper.AssertSimpleProperties(view);
        }

        [Test]
        public void CanSetAndGetSimpleEntryDayProperties()
        {
            EntryDay day = new EntryDay();
            UnitTestHelper.AssertSimpleProperties(day);
        }
    }
}
