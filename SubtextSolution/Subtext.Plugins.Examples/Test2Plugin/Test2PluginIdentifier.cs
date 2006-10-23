using System;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.Test2Plugin
{
	public class Test2PluginIdentifier: IPluginIdentifier
	{
		#region IPluginIdentifier Members

		public Guid Guid
		{
			get { return new Guid("{9dc8f6a3-f9e7-4fe3-a91e-e2fe8f063c7f}"); }
		}

		#endregion
	}
}
