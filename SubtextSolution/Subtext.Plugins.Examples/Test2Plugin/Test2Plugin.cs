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

		public void Init(SubtextApplication sta)
		{
			sta.EntryUpdating += new EntryEventHandler(sta_EntryUpdating);
			sta.EntryUpdated += new EntryEventHandler(sta_EntryUpdated);
			sta.EntryRendering += new EntryEventHandler(sta_EntryRendering);
			sta.SingleEntryRendering += new EntryEventHandler(sta_SingleEntryRendering);
		}

		void sta_SingleEntryRendering(Subtext.Framework.Components.Entry entry, SubtextEventArgs e)
		{
			entry.Body += "<br><hr> <b>Test2Plugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		}

		void sta_EntryRendering(Subtext.Framework.Components.Entry entry, SubtextEventArgs e)
		{
			entry.Body += "<br><hr> <b>Test2Plugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		}

		void sta_EntryUpdated(Subtext.Framework.Components.Entry entry, SubtextEventArgs e)
		{
			string url = entry.FullyQualifiedUrl.ToString();
			return;
		}

		void sta_EntryUpdating(Subtext.Framework.Components.Entry entry, SubtextEventArgs e)
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
