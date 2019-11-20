using System;
using System.Data;
using System.Globalization;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Microsoft.ApplicationBlocks.Data;
using Moq;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Routing;
using Subtext.Web.UI;

namespace UnitTests.Subtext.SubtextWeb.UI
{
    [TestClass]
    public class UIHelpersTests
    {
        [DatabaseIntegrationTestMethod]
        public void ArchiveMonth_ReturnsLinksToMonthlyArchives()
        {
            // Arrange
            UnitTestHelper.CreateBlogAndSetupContext();
            var blogUrlHelper = new Mock<BlogUrlHelper>();
            blogUrlHelper.Setup(u => u.MonthUrl(It.IsAny<DateTime>())).Returns<DateTime>(dt => "/year/" + dt.Year + "/month/" + dt.Month);
            var firstDate = DateTimeOffset.ParseExact("2011/01/05 06:15 AM +08:00", "yyyy/MM/dd hh:mm tt zzz", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).DateTime.ToUniversalTime();
            var entry = UnitTestHelper.CreateEntryInstanceForSyndication("joe", "title1", "body1", "foo1", firstDate, firstDate);
            UnitTestHelper.Create(entry);
            var entry2 = UnitTestHelper.CreateEntryInstanceForSyndication("joe", "title2", "body2", "foo2", firstDate.AddMonths(1), firstDate.AddMonths(1));
            UnitTestHelper.Create(entry2);
            var entry3 = UnitTestHelper.CreateEntryInstanceForSyndication("joe", "title3", "body3", "foo3", firstDate.AddMonths(2), firstDate.AddMonths(2));
            UnitTestHelper.Create(entry3);

            // Act
            var linkCategory = new DatabaseObjectProvider().ArchiveMonth(blogUrlHelper.Object, Config.CurrentBlog);

            // Assert
            Assert.AreEqual("Archives", linkCategory.Title);
            Assert.AreEqual(3, linkCategory.Links.Count);
            Assert.AreEqual("/year/2011/month/3", linkCategory.Links.First().Url);
            Assert.AreEqual("/year/2011/month/2", linkCategory.Links.ElementAt(1).Url);
            Assert.AreEqual("/year/2011/month/1", linkCategory.Links.ElementAt(2).Url);
        }

        [DatabaseIntegrationTestMethod]
        public void ArchiveMonth_ReturnsLinksToMonthlyArchives_UsingBlogTimeZone()
        {
            // Arrange
            UnitTestHelper.CreateBlogAndSetupContext();

            var blog = Config.CurrentBlog;
            Console.WriteLine("blog.DateCreatedUtc: " + blog.DateCreatedUtc);
            Console.WriteLine("blog.DateCreatedUtc.ToUniversal(): " + blog.DateCreatedUtc.ToUniversalTime());
            using (var reader = SqlHelper.ExecuteReader(Config.ConnectionString, CommandType.Text, "SELECT * FROM subtext_Config"))
            {
                while (reader.Read())
                {
                    Console.WriteLine("subtext_Config.DateCreatedUtc: " + reader["DateCreatedUtc"]);
                }
            }

            Config.CurrentBlog.TimeZoneId = "Singapore Standard Time";
            Config.UpdateConfigData(new DatabaseObjectProvider(), Config.CurrentBlog);
            var blogUrlHelper = new Mock<BlogUrlHelper>();
            blogUrlHelper.Setup(u => u.MonthUrl(It.IsAny<DateTime>())).Returns<DateTime>(dt => "/year/" + dt.Year + "/month/" + dt.Month);
            var firstDate = DateTimeOffset.ParseExact("2011/01/01 06:15 AM +08:00", "yyyy/MM/dd hh:mm tt zzz", CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal).DateTime.ToUniversalTime();
            var entry = UnitTestHelper.CreateEntryInstanceForSyndication("joe", "title1", "body1", "foo1", firstDate, firstDate);
            UnitTestHelper.Create(entry);
            var entry2 = UnitTestHelper.CreateEntryInstanceForSyndication("joe", "title2", "body2", "foo2", firstDate.AddDays(1), firstDate.AddDays(1));
            UnitTestHelper.Create(entry2);
            var entry3 = UnitTestHelper.CreateEntryInstanceForSyndication("joe", "title3", "body3", "foo3", firstDate.AddMonths(2), firstDate.AddMonths(2));
            UnitTestHelper.Create(entry3);

            // Act
            var linkCategory = new DatabaseObjectProvider().ArchiveMonth(blogUrlHelper.Object, Config.CurrentBlog);

            // Assert
            Assert.AreEqual("Archives", linkCategory.Title);
            Assert.AreEqual(2, linkCategory.Links.Count);
            Assert.AreEqual("/year/2011/month/3", linkCategory.Links.First().Url);
            Assert.AreEqual("/year/2011/month/1", linkCategory.Links.ElementAt(1).Url);
        }
    }
}
