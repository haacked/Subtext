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
			int i = 1;
		}

		#endregion
	}
}
