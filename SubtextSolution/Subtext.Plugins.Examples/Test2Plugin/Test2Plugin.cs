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

namespace Subtext.Plugins.Examples.Test2Plugin
{
	[Identifier("{3223AC01-DEE1-4351-9198-9700295A6DA1}")]
	[Description("TestPlugin2",
		Author = "Simone Chiaretta",
		Company = "Subtext",
		HomePageUrl = "http://www.subtextproject.com/",
		Version = "0.0.2",
		Description = "Second Plugin used to test the plugin loading process")]
	public class Test2Plugin: PluginBase
	{
		#region PluginBase Members

		public override void Init(SubtextApplication application)
		{
         application.EntryUpdating += new EventHandler<CancellableSubtextEventArgs>(sta_EntryUpdating);
			application.EntryUpdated += new EventHandler<SubtextEventArgs>(sta_EntryUpdated);
			application.EntryRendering += new EventHandler<SubtextEventArgs>(sta_EntryRendering);
			application.SingleEntryRendering += new EventHandler<SubtextEventArgs>(sta_SingleEntryRendering);
		}

		void sta_SingleEntryRendering(object sender, SubtextEventArgs e)
		{
			Entry entry = e.Entry;
			entry.Body += "<br><hr> <b>Test2Plugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		}

		void sta_EntryRendering(object sender, SubtextEventArgs e)
		{
			Entry entry = e.Entry;
			entry.Body += "<br><hr> <b>Test2Plugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		}

		void sta_EntryUpdated(object sender, SubtextEventArgs e)
		{
			Entry entry = e.Entry;
			Console.WriteLine(entry.FullyQualifiedUrl.ToString());
			return;
		}

      void sta_EntryUpdating(object sender, CancellableSubtextEventArgs e)
		{
			Entry entry = e.Entry;
			switch (e.State)
			{
				case ObjectState.Create:
					entry.Body += "<br><hr> <b>Test2Plugin</b>: Created at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
					break;
				case ObjectState.Update:
					entry.Body += "<br><hr> <b>Test2Plugin</b>: Updated at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
					break;
				default:
					break;
			}
		}



		#endregion
	}
}
