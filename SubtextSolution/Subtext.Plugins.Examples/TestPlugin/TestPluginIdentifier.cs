using System;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.TestPlugin
{
	public class TestPluginIdentifier: IPluginIdentifier
	{
		#region IPluginIdentifier Members

		public string Name
		{
			get { return "Subtext.Plugins.Core.TestPlugin.TestPlugin"; }
		}

		#endregion
	}
}
