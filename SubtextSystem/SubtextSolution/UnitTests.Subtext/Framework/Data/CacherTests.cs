using System;
using System.Globalization;
using System.Threading;
using MbUnit.Framework;
using Subtext.Common.Data;
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
		/// Makes sure that the GetActiveCategories method handles user 
		/// Locale correctly.
		/// </summary>
		[Test]
		[RollBack]
		public void GetActiveCategoriesHandlesLocale()
		{
			string hostName = UnitTestHelper.GenerateUniqueHost();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostName, "");
			Config.CreateBlog("", "username", "thePassword", hostName, "");

			//Start with en-US
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");

			// Add categories to cache.
			LinkCategoryCollection cachedCategories = new LinkCategoryCollection();
			cachedCategories.Add(new LinkCategory(1, "Test 1"));
			cachedCategories.Add(new LinkCategory(2, "Test 2"));

			//Note, this corresponds to a private var in Cacher.
			string ActiveLCCKey = "ActiveLinkCategoryCollection:Blog{0}";
			ActiveLCCKey = String.Format(ActiveLCCKey, Config.CurrentBlog.BlogId);
			ContentCache cache = ContentCache.Instantiate();
			cache[ActiveLCCKey] = cachedCategories;

			LinkCategoryCollection categories = Cacher.GetActiveCategories(CacheDuration.Short);
			Assert.AreEqual(2, categories.Count, "Expected to get the cached categories.");
			Assert.AreSame(cachedCategories, categories, "Categories should have been pulled from cache.");

			//Change to spanish
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es");
			LinkCategoryCollection spanishCachedCategories = new LinkCategoryCollection();
			spanishCachedCategories.Add(new LinkCategory(1, "prueba numero uno"));
			cache[ActiveLCCKey] = spanishCachedCategories;

			LinkCategoryCollection spanishCategories = Cacher.GetActiveCategories(CacheDuration.Short);
			Assert.AreEqual(1, spanishCategories.Count, "Only expected one category for spanish.");
		}
	}
}
