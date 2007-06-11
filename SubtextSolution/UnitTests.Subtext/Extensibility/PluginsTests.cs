using System;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Extensibility.Attributes;
using Subtext.Extensibility.Plugins;

namespace UnitTests.Subtext.Extensibility
{
	[TestFixture]
	public class PluginsTests
	{
		[Test]
		[RollBack2]
		[Ignore("Need to improve this")]
		public void CanGetAndSetSettings()
		{
			UnitTestHelper.SetupBlog();

			PluginBase plugin = new PluginFake();
			plugin.SetBlogSetting("unit-test-setting-key", "foo-bar");
			Assert.AreEqual("foo-bar", plugin.GetBlogSetting("unit-test-setting-key"));
		}

		[Test]
		public void CanGetPluginIdentifier()
		{
			PluginBase plugin = new PluginFake();
			Assert.AreEqual(new Guid("a827d201-c510-443c-9675-5c7960102593"), plugin.Id);
		}

		[Test]
		public void IdReturnsEmptyGuidWhenNoAttributeSet()
		{
			MockRepository mocks = new MockRepository();
			using (mocks.Record())
			{
				PluginBase plugin = mocks.DynamicMock<PluginBase>();
				Assert.AreEqual(Guid.Empty, plugin.Id);
			}
		}

		[Test]
		public void InfoReturnsNullNoAttributeSet()
		{
			MockRepository mocks = new MockRepository();
			using (mocks.Record())
			{
				PluginBase plugin = mocks.DynamicMock<PluginBase>();
				Assert.IsNull(plugin.Info);
			}
		}

		[Test]
		public void CanInstantiatePlugin()
		{
			MockRepository mocks = new MockRepository();
			PluginBase plugin;
			using (mocks.Record())
			{
				plugin = mocks.DynamicMock<PluginBase>();
				
			}
			Assert.IsNotNull(plugin);
		}

		#region Exception Tests
		[Test]
		[RollBack2]
		[ExpectedException(typeof(InvalidOperationException))]
		public void GetBlogSettingThrowsInvalidOperationForPluginWithNoGuid()
		{
			MockRepository mocks = new MockRepository();

			using (mocks.Record())
			{
				PluginBase plugin = mocks.DynamicMock<PluginBase>();
				plugin.GetBlogSetting("unit-test-setting-key");
			}
		}

		[Test]
		[RollBack2]
		[ExpectedException(typeof(InvalidOperationException))]
		public void SetBlogSettingThrowsInvalidOperationForPluginWithNoGuid()
		{
			MockRepository mocks = new MockRepository();

			using (mocks.Record())
			{
				PluginBase plugin = mocks.DynamicMock<PluginBase>();
				plugin.SetBlogSetting("unit-test-setting-key", "test");
			}
		}
		#endregion
	}

	[Identifier("{a827d201-c510-443c-9675-5c7960102593}")]
	[Description("PluginFake",
		Author = "Phil Haack",
		Company = "Subtext",
		Copyright = "(c) 2007",
		HomePageUrl = "http://www.subtextproject.com/",
		Version = "0.0.1",
		Description = "This plugin is a fake")]
	public class PluginFake : PluginBase
	{
		/// <summary>
		/// Initialize the plugin.<br />
		/// This is the only method that must be overridden since all actions are performed inside Event Handlers<br />
		/// The implementation of this method should only subscribe to the events raised by the SubtextApplication
		/// </summary>
		public override void Init(SubtextApplication application)
		{
		}
	}
}
