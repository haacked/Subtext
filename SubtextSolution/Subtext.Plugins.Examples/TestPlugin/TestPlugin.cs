using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.TestPlugin
{
	public class TestPlugin: IPlugin
	{
		#region IPlugin Members

		public IPluginIdentifier Id
		{
			get { return new TestPluginIdentifier(); }
		}

		public IImplementationInfo Info
		{
			get { return new TestPluginImplentationInfo(); }
		}

		public void Init(STApplication sta)
		{
			sta.PreEntryUpdate += new EntryEventHandler(sta_PreEntryUpdate);
			sta.PostEntryUpdate += new EntryEventHandler(sta_PostEntryUpdate);
			sta.PreRenderEntry += new EntryEventHandler(sta_PreRenderEntry);
		}

		void sta_PreRenderEntry(Subtext.Framework.Components.Entry entry, STEventArgs e)
		{
			entry.Body += "<br><hr> <b>TestPlugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		}

		void sta_PostEntryUpdate(Subtext.Framework.Components.Entry entry, STEventArgs e)
		{
			string url = entry.FullyQualifiedUrl.ToString();
			return;
		}

		void sta_PreEntryUpdate(Subtext.Framework.Components.Entry entry, STEventArgs e)
		{
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
		}

		#endregion
	}
}
