using System;
using System.Collections.Specialized;
using NUnit.Framework;
using Subtext.Extensibility.Providers;

namespace UnitTests.Subtext.Framework.Providers
{
	/// <summary>
	/// Tests a few methods of the <see cref="ProviderBase"/> class.
	/// </summary>
	[TestFixture]
	public class ProviderBaseTests
	{
		/// <summary>
		/// Makes sure that we can recognize setting pointers.
		/// </summary>
		[Test]
		public void IsPointerToAppSettingRecognizesPointer()
		{
			UnitTestStubProvider provider = new UnitTestStubProvider();
			Assert.IsTrue(provider.IsPointerToAppSettings("${Setting}"));
		}

		/// <summary>
		/// Makes sure that we can recognize false setting pointers.
		/// </summary>
		[Test]
		public void IsPointerToAppSettingRecognizesIncompletePointer()
		{
			UnitTestStubProvider provider = new UnitTestStubProvider();
			Assert.IsFalse(provider.IsPointerToAppSettings("${Setting"));
		}

		/// <summary>
		/// Makes sure that we can recognize false setting pointers.
		/// </summary>
		[Test]
		public void IsPointerToAppSettingRecognizesBadPointer()
		{
			UnitTestStubProvider provider = new UnitTestStubProvider();
			Assert.IsFalse(provider.IsPointerToAppSettings("{Setting}"));
		}

		/// <summary>
		/// Make sure GetSettingName can follow a pointer and doesn't follow a non-pointer.
		/// </summary>
		[Test]
		public void GetSettingNameReturnsPointerSetting()
		{
			NameValueCollection collection = new NameValueCollection();
			collection.Add("someSetting", "${ConnectionString}");
			collection.Add("anotherSetting", "ConnectionString");

			UnitTestStubProvider provider = new UnitTestStubProvider();
			string settingValue = provider.GetSettingValue("someSetting", collection);
			Assert.AreEqual("Server=localhost;Database=SubtextData;Trusted_Connection=True", settingValue);

			settingValue = provider.GetSettingValue("anotherSetting", collection);
			Assert.AreEqual("ConnectionString", settingValue);
		}

		class UnitTestStubProvider : ProviderBase
		{
			/// <summary>
			/// Initializes the specified provider.
			/// </summary>
			/// <param name="name">Friendly Name of the provider.</param>
			/// <param name="configValue">Config value.</param>
			public override void Initialize(string name, NameValueCollection configValue)
			{
				throw new NotImplementedException();
			}

			/// <summary>
			/// Returns the friendly name of the provider when the provider is initialized.
			/// </summary>
			/// <value></value>
			public override string Name
			{
				get { throw new NotImplementedException(); }
			}
		}
	}
}
