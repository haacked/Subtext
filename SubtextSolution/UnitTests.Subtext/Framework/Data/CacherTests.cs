using System;
using System.Collections.Generic;
using System.Globalization;
using System.Threading;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework.Data;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Data
{
	/// <summary>
	/// Unit tests of the <see cref="Cacher"/> class.
	/// </summary>
	[TestFixture]
	public class CacherTests
	{
		/// <summary>
		/// This test is to make sure a bug I introduced never happens again.
		/// </summary>
		[Test]
		[RollBack]
		public void GetEntryFromRequestDoesNotThrowNullReferenceException()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/archive/999999.aspx");
			Assert.IsNull(Cacher.GetEntryFromRequest(CacheDuration.Short));
		}

		[Test]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SingleCategoryThrowsExceptionIfContextNull()
		{
			HttpContext.Current = null;
			Cacher.SingleCategory(CacheDuration.Short);
		}

		[Test]
		[RollBack]
		public void SingleCategoryReturnsNullForNonExistentCategory()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/99.aspx");
			Assert.IsNull(Cacher.SingleCategory(CacheDuration.Short));
		}

		[Test]
		[RollBack]
		public void CanGetCategoryByIdRequest()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/");
			int categoryId = UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "This Is a Test");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/" + categoryId + ".aspx");

			LinkCategory category = Cacher.SingleCategory(CacheDuration.Short);
			Assert.AreEqual("This Is a Test", category.Title);
		}

		[Test]
		[RollBack]
		public void CanGetCategoryByNameRequest()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/This Is a Test.aspx");
			UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "This Is a Test");

			LinkCategory category = Cacher.SingleCategory(CacheDuration.Short);
			Assert.AreEqual("This Is a Test", category.Title);
		}

		[Test]
		[RollBack]
		public void CanGetCategoryByNameWithWordDelimitersRequest()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("test", UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), host, "");
			UnitTestHelper.SetHttpContextWithBlogRequest(host, "", "", "/category/This_Is_a_Test.aspx");
			UnitTestHelper.CreateCategory(Config.CurrentBlog.Id, "This Is a Test");

			LinkCategory category = Cacher.SingleCategory(CacheDuration.Short);
			Assert.AreEqual("This Is a Test", category.Title);
		}

		/// <summary>
		/// Makes sure that the GetActiveCategories method handles user 
		/// Locale correctly.
		/// </summary>
		[Test]
		[RollBack]
		public void GetActiveCategoriesHandlesLocale()
		{
			string hostName = UnitTestHelper.GenerateUniqueString();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "");
			Config.CreateBlog("", "username", "thePassword", hostName, "");

			//Start with en-US
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

			// Add categories to cache.
			ICollection<LinkCategory> cachedCategories = new List<LinkCategory>();
			cachedCategories.Add(new LinkCategory(1, "Test 1"));
			cachedCategories.Add(new LinkCategory(2, "Test 2"));

			//Note, this corresponds to a private var in Cacher.
			string ActiveLCCKey = "ActiveLinkCategoryCollection:Blog{0}";
			ActiveLCCKey = String.Format(ActiveLCCKey, Config.CurrentBlog.Id);
			ContentCache cache = ContentCache.Instantiate();
			cache[ActiveLCCKey] = cachedCategories;

			ICollection<LinkCategory> categories = Cacher.GetActiveCategories(CacheDuration.Short);
			Assert.AreEqual(2, categories.Count, "Expected to get the cached categories.");
			Assert.AreSame(cachedCategories, categories, "Categories should have been pulled from cache.");

			//Change to spanish
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es");
			ICollection<LinkCategory> spanishCachedCategories = new List<LinkCategory>();
			spanishCachedCategories.Add(new LinkCategory(1, "prueba numero uno"));
			cache[ActiveLCCKey] = spanishCachedCategories;

            ICollection<LinkCategory> spanishCategories = Cacher.GetActiveCategories(CacheDuration.Short);
			Assert.AreEqual(1, spanishCategories.Count, "Only expected one category for spanish.");
		}
	}
}
