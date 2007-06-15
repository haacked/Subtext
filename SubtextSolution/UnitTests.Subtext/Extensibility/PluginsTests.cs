using System;
using System.Collections.Specialized;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Extensibility.Attributes;
using Subtext.Extensibility.Plugins;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Extensibility
{
	[TestFixture]
	public class PluginsTests
	{
		[Test]
		[RollBack2]
		public void CanEnableDisablePluginForBlog()
		{
			UnitTestHelper.SetupBlog();
			Config.CurrentBlog.EnabledPlugins.Clear();
			PluginBase plugin = new PluginFake();
			Plugin.EnablePlugin(plugin.Id);

			Assert.IsTrue(Config.CurrentBlog.EnabledPlugins.ContainsKey(plugin.Id), "The plugin we expect to find is not enabled");

			Plugin.DisablePlugin(plugin.Id);

			Assert.IsFalse(Config.CurrentBlog.EnabledPlugins.ContainsKey(plugin.Id), "The plugin we expect to be disabled is still enabled");
		}

		[Test]
		[RollBack2]
		public void CanGetAndSetBlogSettings()
		{
			UnitTestHelper.SetupBlog();

			PluginBase plugin = new PluginFake();
			Plugin.EnablePlugin(plugin.Id);

			NameValueCollection settings = new NameValueCollection();
			Plugin pluginInfo = new Plugin(plugin, settings);
			Config.CurrentBlog.EnabledPlugins.Clear();
			Config.CurrentBlog.EnabledPlugins.Add(plugin.Id, pluginInfo);
			plugin.SetBlogSetting("unit-test-setting-key", "foo-bar");
			Assert.AreEqual("foo-bar", plugin.GetBlogSetting("unit-test-setting-key"));
		}

		[Test]
		[RollBack2]
		public void CanGetAndSetEntrySettings()
		{
			UnitTestHelper.SetupBlog();

			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "yet another dull blog post",
			                                                 "yawn. nothing to report today. Fed a cat.");
			Entries.Create(entry);

			PluginBase plugin = new PluginFake();
			Plugin.EnablePlugin(plugin.Id);
			
			NameValueCollection settings = new NameValueCollection();
			Plugin pluginInfo = new Plugin(plugin, settings);
			entry.EnabledPlugins.Clear();
			entry.EnabledPlugins.Add(plugin.Id, pluginInfo);
			Config.CurrentBlog.EnabledPlugins.Clear();
			Config.CurrentBlog.EnabledPlugins.Add(plugin.Id, pluginInfo);
			plugin.SetEntrySetting(entry, "unit-test-setting-key", "foo-bar");
			Assert.AreEqual("foo-bar", plugin.GetEntrySetting(entry, "unit-test-setting-key"));
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
