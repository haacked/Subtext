using System;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Extensibility.Plugins;

namespace UnitTests.Subtext.Extensibility
{
	[TestFixture]
	public class PluginsTests
	{
		[Test]
		[RollBack]
		[Ignore("We need some way to set the plugin id for this test.")]
		public void CanGetAndSetSettings()
		{
			UnitTestHelper.SetupBlog();

			MockRepository mocks = new MockRepository();
			
			using (mocks.Record())
			{
				PluginBase plugin = mocks.DynamicMock<PluginBase>();
				plugin.SetBlogSetting("unit-test-setting-key", "foo-bar");
				Assert.AreEqual("foo-bar", plugin.GetBlogSetting("unit-test-setting-key"));
			}
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
		[RollBack]
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
		[RollBack]
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
}
