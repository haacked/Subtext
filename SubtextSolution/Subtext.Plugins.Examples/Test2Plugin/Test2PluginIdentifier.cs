using System;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.Test2Plugin
{
	public class Test2PluginIdentifier: IPluginIdentifier
	{
		#region IPluginIdentifier Members

		public string Name
		{
			get { return "Subtext.Plugins.Core.Test2Plugin.Test2Plugin"; }
		}

		#endregion
	}
}
