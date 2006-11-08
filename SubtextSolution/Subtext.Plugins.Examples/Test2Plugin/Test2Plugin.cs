using System;
using System.Collections.Generic;
using System.Text;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.Test2Plugin
{
	public class Test2Plugin: IPlugin
	{
		#region IPlugin Members

		public IPluginIdentifier Id
		{
			get { return new Test2PluginIdentifier(); }
		}

		public IImplementationInfo Info
		{
			get { return new Test2PluginImplentationInfo(); }
		}

		public void Init(STApplication sta)
		{
			sta.PreEntryUpdate += new EntryEventHandler(sta_PreEntryUpdate);
			sta.PostEntryUpdate += new EntryEventHandler(sta_PostEntryUpdate);
			sta.PreRenderEntry += new EntryEventHandler(sta_PreRenderEntry);
			sta.PreRenderSingleEntry += new EntryEventHandler(sta_PreRenderSingleEntry);
		}

		void sta_PreRenderSingleEntry(Subtext.Framework.Components.Entry entry, STEventArgs e)
		{
			entry.Body += "<br><hr> <b>Test2Plugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		}

		void sta_PreRenderEntry(Subtext.Framework.Components.Entry entry, STEventArgs e)
		{
			entry.Body += "<br><hr> <b>Test2Plugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
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
