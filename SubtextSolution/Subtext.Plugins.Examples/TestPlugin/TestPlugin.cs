using System;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.TestPlugin
{
	public class TestPlugin: IPlugin
	{
		#region IPlugin Members

		static readonly Guid guid = new Guid("{DE307000-AFCD-480d-AA37-D85E57EB2D04}");
		
		public Guid Id
		{
			get { return guid; }
		}

		public IImplementationInfo Info
		{
			get { return new TestPluginImplentationInfo(); }
		}

		public void Init(SubtextApplication application)
		{
			application.EntryUpdating += new EventHandler<SubtextEventArgs>(sta_EntryUpdating);
			application.EntryUpdated += new EventHandler<SubtextEventArgs>(sta_EntryUpdated);
			application.EntryRendering += new EventHandler<SubtextEventArgs>(sta_EntryRendering);
			application.SingleEntryRendering += new EventHandler<SubtextEventArgs>(sta_SingleEntryRendering);
		}

		void sta_SingleEntryRendering(Subtext.Framework.Components.Entry entry, SubtextEventArgs e)
		{
			entry.Body += "<br><hr> <b>TestPlugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
		}

		void sta_EntryRendering(Subtext.Framework.Components.Entry entry, SubtextEventArgs e)
		{
			entry.Body += "<br><hr> <b>TestPlugin</b>: Rendered at date: " + DateTime.Now.ToShortDateString() + " " + DateTime.Now.ToShortTimeString();
			entry.Body += e.BlogSettings["value1"];
		}

		void sta_EntryUpdated(Subtext.Framework.Components.Entry entry, SubtextEventArgs e)
		{
			Console.WriteLine(entry.FullyQualifiedUrl.ToString());
			return;
		}

		void sta_EntryUpdating(Subtext.Framework.Components.Entry entry, SubtextEventArgs e)
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
