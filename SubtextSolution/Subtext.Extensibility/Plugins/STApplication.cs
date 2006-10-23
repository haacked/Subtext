#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Xml;
using System.Configuration;
using System.Web.Configuration;

namespace Subtext.Extensibility.Plugins
{

	public sealed class STApplication
	{
		private static bool _initialized = false;
		private EventHandlerList Events = new EventHandlerList();
		private static readonly object sync = new object();
		private Dictionary<string, IPlugin> _plugins = new Dictionary<string, IPlugin>();

		//private static readonly Log __log = new Log();

		public Dictionary<string, IPlugin> Plugins
		{
			get { return _plugins; }
			set { _plugins = value; }
		}


		private static STApplication _instance = BuildInstance();

		private static STApplication BuildInstance()
		{
			return LoadPlugins();
		}

		public static STApplication Current
		{
			get
			{
				//In case the static initializer has failed
				//I try to reinitialize it again
				if(!_initialized)
					_instance = BuildInstance();
				return _instance;
			}
		}

		private STApplication()
		{
		}

		private static STApplication LoadPlugins()
		{
			STApplication app = new STApplication();
			PluginSectionHandler pluginSection = (PluginSectionHandler)WebConfigurationManager.GetSection("STPluginConfiguration");

			if(pluginSection!=null) 
			{
				PluginSettingsCollection pluginConfig=pluginSection.PluginList;

				if (pluginConfig != null)
				{
					
					foreach (PluginSettings setting in pluginConfig)
					{

						if (String.IsNullOrEmpty(setting.Type))
						{
							//__log.Warn("Cannot load plugin defined at line " + setting.ElementInformation.LineNumber + ":\r\nMissing Type");
							continue;
						}

						if (String.IsNullOrEmpty(setting.Name))
						{
							//__log.Warn("Cannot load plugin defined at line " + setting.ElementInformation.LineNumber + ":\r\nMissing Name");
							continue;
						}

						Type type = Type.GetType(setting.Type);

						if (type == null)
						{
							//__log.Warn("Cannot load plugin defined at line " + setting.ElementInformation.LineNumber + ":\r\nType " + setting.Type + " not found in any of the assembly available inside the \\bin folder");
							continue;
						}
						IPlugin plugin = Activator.CreateInstance(type) as IPlugin;

						if (plugin == null)
						{
							//__log.Warn("Cannot load plugin defined at line " + setting.ElementInformation.LineNumber + ":\r\nType " + setting.Type + " doesn't implements IPlugin interface");
							continue;
						}

						plugin.Init(app);
						app._plugins.Add(setting.Name, plugin);
						
						#if DEBUG
						//__log.Debug("Loaded plugin with name " + setting.Name + " from type " + setting.Type);
						#endif
						
					}
				}
			}
			_initialized = true;
			return app;
		}
	}


}
