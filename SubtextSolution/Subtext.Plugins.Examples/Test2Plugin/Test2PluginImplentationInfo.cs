using System;
using Subtext.Extensibility.Plugins;

namespace Subtext.Plugins.Examples.Test2Plugin
{
	public class Test2PluginImplentationInfo: IImplementationInfo
	{
		#region IImplementationInfo Members

		public string Name
		{
			get { return "Test2Plugin"; }
		}

		public string Author
		{
			get { return "Simone Chiaretta"; }
		}

		public string Company
		{
			get { return "SubText"; }
		}

		public string Copyright
		{
			get { return "(C) 2006"; }
		}

		public string Description
		{
			get { return "Second Plugin used to test the plugin loading process"; }
		}

		public Uri HomePageUrl
		{
			get { return new Uri("http://www.subtextproject.com/"); }
		}

		public Version Version
		{
			get { return new Version(0,0,1); }
		}

		#endregion

	}
}
