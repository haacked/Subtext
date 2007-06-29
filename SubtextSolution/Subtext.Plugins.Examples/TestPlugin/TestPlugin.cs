#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using Subtext.Extensibility.Attributes;
using Subtext.Extensibility.Plugins;
using Subtext.Framework.Components;

namespace Subtext.Plugins.Examples.TestPlugin
{
	[Identifier("{DE307000-AFCD-480d-AA37-D85E57EB2D04}")]
	[Description("TestPlugin",
		Author = "Simone Chiaretta",
		Company = "Subtext",
		HomePageUrl = "http://www.subtextproject.com/",
		Version = "0.0.2",
		Description = "Plugin used to test the plugin loading process")]
	public class TestPlugin: PluginBase
	{
		#region PluginBase Members

		public override void Init(SubtextApplication application)
		{
			application.EntryUpdating += new EventHandler<CancellableEntryEventArgs>(sta_EntryUpdating);
			application.EntryUpdated += new EventHandler<EntryEventArgs>(sta_EntryUpdated);
			application.EntryRendering += new EventHandler<EntryEventArgs>(sta_EntryRendering);
			application.SingleEntryRendering += new EventHandler<EntryEventArgs>(sta_SingleEntryRendering);
		}

		void sta_SingleEntryRendering(object sender, EntryEventArgs e)
		{
			Entry entry = e.Entry;
			entry.Body += "<br><hr> <b>TestPlugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		}

		void sta_EntryRendering(object sender, EntryEventArgs e)
		{
			Entry entry = e.Entry;
			entry.Body += "<br><hr> <b>TestPlugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
			entry.Body += GetBlogSetting("value1");
		}

		void sta_EntryUpdated(object sender, EntryEventArgs e)
		{
			Entry entry = e.Entry;
			Console.WriteLine(entry.FullyQualifiedUrl.ToString());
			return;
		}

		void sta_EntryUpdating(object sender, CancellableEntryEventArgs e)
		{
			Entry entry = e.Entry;
			switch (e.State)
			{
				case ObjectState.Create:
					entry.Body += "<br><hr> <b>TestPlugin</b>: Created at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
					break;
				case ObjectState.Update:
					entry.Body += "<br><hr> <b>TestPlugin</b>: Updated at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
					break;
				default:
					break;
			}

			e.Cancel = true;
			//((INotifiableControl)sender).ShowMessage("Updated by the plugin");
		}

		#endregion
	}
}
