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

	#region Event Delegates
	public delegate void EntryEventHandler(object sender, EventArgs e);
	#endregion

	public sealed class STApplication
	{

		private EventHandlerList Events = new EventHandlerList();
		private static readonly object sync = new object();

		#region Event Keys (static)
		private static object EventPreEntryUpdate = new object();
		private static object EventPostEntryUpdate = new object();
		private static object EventPreRenderEntry = new object();
		#endregion


		private static bool _initialized = false;
		private Dictionary<string, IPlugin> _plugins = new Dictionary<string, IPlugin>();

		private PluginSettingsCollection _pluginConfig;

		public PluginSettingsCollection PluginConfig
		{
			get { return _pluginConfig; }
		}


		//private static readonly Log __log = new Log();

		public Dictionary<string, IPlugin> Plugins
		{
			get { return _plugins; }
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
				app._pluginConfig = pluginSection.PluginList;

				if (app._pluginConfig != null)
				{

					foreach (PluginSettings setting in app._pluginConfig)
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

		public IPlugin GetPluginByGuid(string guid)
		{
			foreach (IPlugin plugin in _plugins.Values)
			{
				if (plugin.Id.Guid.ToString().Equals(guid))
					return plugin;
			}
			return null;
		}

		public string GetPluginModuleFileName(string pluginName, string moduleName)
		{
			foreach (PluginSettings settings in _pluginConfig)
			{
				if(settings.Name.Equals(pluginName))
					foreach (PluginModule module in settings.Modules)
					{
						if (module.Key.Equals(moduleName))
							return module.FileName;
					}
			}
			return null;
		}

		#region Event definitions

		/// <summary>
		/// Raised before changes to the Entry are committed to the datastore
		/// </summary>
		public event EntryEventHandler PreEntryUpdate
		{
			add
			{
				Events.AddHandler(EventPreEntryUpdate, value);
			}
			remove
			{
				Events.RemoveHandler(EventPreEntryUpdate, value);
			}
		}

		/// <summary>
		/// Raised after the changes has been committed to the datastore
		/// </summary>
		public event EntryEventHandler PostEntryUpdate
		{
			add
			{
				Events.AddHandler(EventPostEntryUpdate, value);
			}
			remove
			{
				Events.RemoveHandler(EventPostEntryUpdate, value);
			}
		}

		/// <summary>
		/// Raised an individual entry is rendered
		/// </summary>
		public event EntryEventHandler PreRenderEntry
		{
			add
			{
				Events.AddHandler(EventPreRenderEntry, value);
			}
			remove
			{
				Events.RemoveHandler(EventPreRenderEntry, value);
			}
		}

		#endregion

		#region Event Execution

		public void OnPreEntryUpdate()
		{
			ExecuteEntryEvent(EventPreEntryUpdate);
		}

		public void OnPostEntryUpdate()
		{
			ExecuteEntryEvent(EventPostEntryUpdate);
		}

		public void OnPreRenderEntry()
		{
			ExecuteEntryEvent(EventPreRenderEntry);
		}

		private void ExecuteEntryEvent(object eventKey)
		{
			EntryEventHandler handler = Events[eventKey] as EntryEventHandler;
			if (handler != null)
			{
				Delegate[] delegates = handler.GetInvocationList();
				foreach (Delegate del in delegates)
				{
					if (PluginEnabled(del.Method.DeclaringType))
					{
						try
						{
							del.DynamicInvoke();
						}
						catch {}
					}
				}
			}
		}

		private bool PluginEnabled(Type type)
		{
			
		}

		#endregion
	}


}
