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
			int i = 1;
		}

		#endregion
	}
}
