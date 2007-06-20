using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web;
using System.Web.Caching;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Framework;
using Subtext.TestLibrary;

namespace UnitTests.Subtext.Framework.Data
{
	/// <summary>
	/// Tests of the <see cref="ContentCache"/> class. This is a class 
	/// that is used for caching content and NOT system or admin info.
	/// </summary>
	[TestFixture]
	public class ContentCacheTests
	{
		[Test]
		public void CanInsertIntoCache()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				ContentCache cache = ContentCache.Instantiate();
				Assert.IsNull(cache.Get("NotThere"));
				cache.Insert("IsThereNow", new object());
				Assert.IsNotNull(cache.Get("IsThereNow"));
			}
		}

		[Test]
		public void CanInsertIntoCacheWithCacheDependency()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				ContentCache cache = ContentCache.Instantiate();
				Assert.IsNull(cache.Get("NotThere"));
				MockRepository mocks = new MockRepository();
				CacheDependency cacheDependency;
				using(mocks.Record())
				{
					cacheDependency = mocks.CreateMock<CacheDependency>();
				}
				using(mocks.Playback())
				{
					cache.Insert("IsThereWithDependency", new object(), cacheDependency);
					Assert.IsNotNull(cache.Get("IsThereWithDependency"));
				}
			}
		}

		[Test]
		public void CanRemoveFromCache()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				ContentCache cache = ContentCache.Instantiate();
				cache["IsThereForRemove"] = new object();
				Assert.IsNotNull(cache.Get("IsThereForRemove"));
				cache.Remove("IsThereForRemove");
				Assert.IsNull(cache.Get("IsThereForRemove"));
			}
		}

		[Test]
		public void CanGetItemFromCache()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				ContentCache cache = ContentCache.Instantiate();
				Assert.IsNull(cache.Get("NotThere"));
				cache["IsThereGetIt"] = new object();
				Assert.IsNotNull(cache.Get("IsThereGetIt"));
			}
		}

		/// <summary>
		/// Makes sure that the <see cref="ContentCache"/> <see cref="ContentCache.Instantiate"/> 
		/// method uses the per-request cache if provided.
		/// </summary>
		[Test]
		public void InstantiationOfContentCacheUsesRequestCaching()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				Assert.IsNotNull(HttpContext.Current, "We did not set up the http context correctly.");
				Assert.AreEqual(1, HttpContext.Current.Items.Count, "Did not expect the request cache to have any items.");

				ContentCache cache = ContentCache.Instantiate();

				Assert.AreEqual(2, HttpContext.Current.Items.Count, "Expected two item in the request cache.");

				Assert.AreSame(cache, ContentCache.Instantiate(),
				               "Expected second call to instantiate to return cached ContentCache.");
			}
		}

		/// <summary>
		/// Makes sure the content cache doesn't cache the same information 
		/// for different languages.
		/// </summary>
		[Test]
		public void ContentCacheCachesByLanguage()
		{
			using (new HttpSimulator().SimulateRequest())
			{
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
				foreach (DictionaryEntry item in cache)
				{
					if (item.Value.ToString() == "English" || item.Value.ToString() == "Espanol")
					{
						stringCount++;
					}
				}
				Assert.AreEqual(2, stringCount, "Expected two items in the cache.");
			}
		}

		/// <summary>
		/// Make sure passing in a null value for caching throws an exception.
		/// </summary>
		[Test]
		[ExpectedArgumentNullException]
		public void InsertThrowsArgumentNullExceptionForNullValue()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				ContentCache cache = ContentCache.Instantiate();
				cache.Insert("test", null);
			}
		}

		/// <summary>
		/// Make sure passing in a null value for caching throws an exception.
		/// </summary>
		[Test]
		[ExpectedArgumentNullException]
		public void InsertThrowsArgumentNullExceptionForNullValueWithCacheDurationOverload()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				ContentCache cache = ContentCache.Instantiate();
				cache.Insert("test", null, CacheDuration.Short);
			}
		}

		/// <summary>
		/// Make sure passing in a null value for caching throws an exception.
		/// </summary>
		[Test]
		[ExpectedArgumentNullException]
		public void InsertThrowsArgumentNullExceptionForNullValueWithCacheDependencyOverload()
		{
			using (new HttpSimulator().SimulateRequest())
			{
				ContentCache cache = ContentCache.Instantiate();

				MockRepository mocks = new MockRepository();
				CacheDependency cacheDependency = mocks.CreateMock<CacheDependency>();
				mocks.ReplayAll();
				cache.Insert("test", null, cacheDependency);
			}
		}
	}
}
