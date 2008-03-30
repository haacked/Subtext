using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;

namespace UnitTests.Subtext.Framework.Data
{
	/// <summary>
	/// Tests of the <see cref="ContentCache"/> class. This is a class 
	/// that is used for caching content and NOT system or admin info.
	/// </summary>
	[TestFixture]
	public class ContentCacheTests
	{
		/// <summary>
		/// Makes sure that the <see cref="ContentCache"/> <see cref="ContentCache.Instantiate"/> 
		/// method uses the per-request cache if provided.
		/// </summary>
		[Test]
		public void InstantiationOfContentCacheUsesRequestCaching()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(UnitTestHelper.GenerateRandomString(), "");
			Assert.IsNotNull(HttpContext.Current, "We did not set up the http context correctly.");
			Assert.AreEqual(1, HttpContext.Current.Items.Count, "Did not expect the request cache to have any items.");
			
			ContentCache cache = ContentCache.Instantiate();

			Assert.AreEqual(2, HttpContext.Current.Items.Count, "Expected two item in the request cache.");

			Assert.AreSame(cache, ContentCache.Instantiate(), "Expected second call to instantiate to return cached ContentCache.");

			HttpContext.Current = null;
		}

		/// <summary>
		/// Makes sure the content cache doesn't cache the same information 
		/// for different languages.
		/// </summary>
		[Test]
		public void ContentCacheCachesByLanguage()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(UnitTestHelper.GenerateRandomString(), "");

			//Start with en-US
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			ContentCache cache = ContentCache.Instantiate();
			cache["test"] = "English";
			Assert.AreEqual("English", cache["test"], "Did not store the value in the cache properly.");

			//CHange to spanish
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("es");
			cache["test"] = "Espanol";
			Assert.AreEqual("Espanol", cache["test"], "Did not store the value in the cache properly.");

			//Change back to English.
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			Assert.AreEqual("English", cache["test"], "Should have changed the value based on language code.");

			int stringCount = 0;
			foreach(DictionaryEntry item in cache)
			{
				if(item.Value.ToString() == "English" || item.Value.ToString() == "Espanol")
				{
					stringCount++;
				}
			}
			Assert.AreEqual(2, stringCount, "Expected two items in the cache.");

			HttpContext.Current = null;
		}

		/// <summary>
		/// Make sure passing in a null value for caching throws an exception.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CannotInsertNullTest()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(UnitTestHelper.GenerateRandomString(), "");
			ContentCache cache = ContentCache.Instantiate();
			cache.Insert("test", null);
		}

		/// <summary>
		/// Make sure passing in a null value for caching throws an exception.
		/// </summary>
		[Test]
		[ExpectedException(typeof(ArgumentNullException))]
		public void CannotInsertNullWithCacheDurationTest()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(UnitTestHelper.GenerateRandomString(), "");
			ContentCache cache = ContentCache.Instantiate();
			cache.Insert("test", null, CacheDuration.Short);
		}
	}
}
