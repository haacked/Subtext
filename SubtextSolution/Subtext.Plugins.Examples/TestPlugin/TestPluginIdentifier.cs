using System;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.TestPlugin
{
	public class TestPluginIdentifier: IPluginIdentifier
	{
		#region IPluginIdentifier Members

		public Guid Guid
		{
			get { return new Guid("{d46657e2-540b-4f5e-ab11-b2b2de3824cb}"); }
		}

		#endregion
	}
}
