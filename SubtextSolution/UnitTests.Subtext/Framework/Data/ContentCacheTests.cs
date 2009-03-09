using System;
using System.Collections;
using System.Globalization;
using System.Threading;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;
using Moq;

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
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext();

            //act
            ContentCache cache = ContentCache.Instantiate(subtextContext.Object);

            //assert
			Assert.AreEqual(1, subtextContext.Object.RequestContext.HttpContext.Items.Count);
			Assert.AreSame(cache, ContentCache.Instantiate(subtextContext.Object), "Expected second call to instantiate to return cached ContentCache.");
		}

		/// <summary>
		/// Makes sure the content cache doesn't cache the same information 
		/// for different languages.
		/// </summary>
		[Test]
		public void ContentCacheCachesByLanguage()
		{
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext();

			//Start with en-US
			Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("en-US");
			ContentCache cache = ContentCache.Instantiate(subtextContext.Object);
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
			foreach(string key in cache)
			{
                //Get rid of colon in key...
                string rootKey = key.Split(':')[0];

                string itemValue = cache.Get(rootKey) as string;

				if(itemValue == "English" || itemValue == "Espanol")
				{
					stringCount++;
				}
			}
			Assert.AreEqual(2, stringCount, "Expected two items in the cache.");
		}

		/// <summary>
		/// Make sure passing in a null value for caching throws an exception.
		/// </summary>
		[Test]
		public void CannotInsertNullTest()
		{
            //arrange
            var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext();
			ContentCache cache = ContentCache.Instantiate(subtextContext.Object);
			
            //act, assert
            UnitTestHelper.AssertThrows<ArgumentNullException>(
                () => cache.Insert("test", null)
            );
		}

		/// <summary>
		/// Make sure passing in a null value for caching throws an exception.
		/// </summary>
		[Test]
		public void CannotInsertNullWithCacheDurationTest()
		{
            //arrange
			var subtextContext = new Mock<ISubtextContext>();
            subtextContext.SetupRequestContext();
			ContentCache cache = ContentCache.Instantiate(subtextContext.Object);

            //act, assert
            UnitTestHelper.AssertThrows<ArgumentNullException>(
                () => cache.Insert("test", null, CacheDuration.Short)
            );
		}
	}
}
