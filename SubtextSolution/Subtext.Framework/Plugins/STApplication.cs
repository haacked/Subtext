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
using Subtext.Framework.Configuration;
using Subtext.Framework.Components;

namespace Subtext.Extensibility.Plugins
{

	#region Event Delegates
	/// <summary>
	/// Delegate that defines the signature of the Entry Event Handler
	/// </summary>
	/// <param name="entry">The entry being edited</param>
	/// <param name="e">Event Args</param>
	public delegate void EntryEventHandler(Entry entry, STEventArgs e);
	#endregion

	/// <summary>
	/// Singleton class.<br/>
	/// This class is responsible for all the actions related to plugins:<br/>
	/// <ul>
	/// <list type=">">Loading settings from web.config</list>
	/// <list type=">">Loading and initializing plugins from the settings loaded before</list>
	/// <list type=">">Defines all the events for the application</list>
	/// <list type=">">Manages the execution of the plugins, based on their abilitation for the current blog</list>
	/// </ul>
	/// Since the class contains a lot of event definitions, it uses the approach used also by the Form class and by HttpApplication class:<br/>
	/// uses only one EventHandlerList to store all the event subscription instead of one per event as the default behaviour
	/// </summary>
	public sealed class STApplication
	{

		private EventHandlerList Events = new EventHandlerList();
		private static readonly object sync = new object();


		#region Event Keys (static)
		//Using objects so that no cast is performed when accessing the eventhandler list
		private static object EventPreEntryUpdate = new object();
		private static object EventPostEntryUpdate = new object();
		private static object EventPreRenderEntry = new object();
		#endregion


		private static bool _initialized = false;
		private Dictionary<string, IPlugin> _plugins = new Dictionary<string, IPlugin>();

		private PluginSettingsCollection _pluginConfig;

		/// <summary>
		/// List of all available plugins configurations
		/// </summary>
		public PluginSettingsCollection PluginConfig
		{
			get { return _pluginConfig; }
		}


		//private static readonly Log __log = new Log();

		/// <summary>
		/// All avalialbe plugins. These plugins are already initialized
		/// </summary>
		public Dictionary<string, IPlugin> Plugins
		{
			get { return _plugins; }
		}


		private static STApplication _instance = LoadPlugins();

		/// <summary>
		/// Returns the current single instance of the class
		/// </summary>
		public static STApplication Current
		{
			get
			{
				//In case the static initializer has failed
				//I try to reinitialize it again
				if(!_initialized)
					_instance = LoadPlugins();
				return _instance;
			}
		}

		private STApplication()
		{
		}

		/// <summary>
		/// Load all plugins from the web.config file, and return a configured instance of the STApplication, with all the plugins already initialized
		/// </summary>
		/// <returns>Configured instance of the STApplication</returns>
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

		internal void ExecutePreEntryUpdate(Entry entry, STEventArgs e)
		{
			ExecuteEntryEvent(EventPreEntryUpdate, entry, e);
		}

		internal void ExecutePostEntryUpdate(Entry entry, STEventArgs e)
		{
			ExecuteEntryEvent(EventPostEntryUpdate, entry, e);
		}

		internal void ExecutePreRenderEntry(Entry entry, STEventArgs e)
		{
			ExecuteEntryEvent(EventPreRenderEntry, entry, e);
		}

		//List through the subscribed event handlers, and decide weather call them or not
		//based on the current blog enabled plugins
		private void ExecuteEntryEvent(object eventKey, Entry entry, EventArgs e)
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
							del.DynamicInvoke(entry, e);
						}
						catch {}
					}
				}
			}
		}
		#endregion


		#region Helper Functions

		/// <summary>
		/// Get the initialized plugin given its guid
		/// </summary>
		/// <param name="guid">the GUID in string format</param>
		/// <returns>The initialized instance of the plugin</returns>
		public IPlugin GetPluginByGuid(string guid)
		{
			foreach (IPlugin plugin in _plugins.Values)
			{
				if (plugin.Id.Guid.ToString().Equals(guid))
					return plugin;
			}
			return null;
		}

		/// <summary>
		/// Get the initialized plugin given its type
		/// </summary>
		/// <param name="type">the type of the plugin</param>
		/// <returns>The initialized instance of the plugin</returns>
		private IPlugin GetPluginByType(Type type)
		{
			foreach (IPlugin plugin in _plugins.Values)
			{
				if (plugin.GetType() == type)
					return plugin;
			}
			return null;
		}

		/// <summary>
		/// Get the file name of a plugin module given the plugin name and the module key
		/// </summary>
		/// <param name="pluginName">Plugin name</param>
		/// <param name="moduleName">Module key</param>
		/// <returns>Filename of the module</returns>
		public string GetPluginModuleFileName(string pluginName, string moduleName)
		{
			foreach (PluginSettings settings in _pluginConfig)
			{
				if (settings.Name.Equals(pluginName))
					foreach (PluginModule module in settings.Modules)
					{
						if (module.Key.Equals(moduleName))
							return module.FileName;
					}
			}
			return null;
		}

		/// <summary>
		/// Checks if the plugin is enabled for the current blog
		/// </summary>
		/// <param name="type">Type of the plugin</param>
		/// <returns><c>true</c> if the plugin is enabled, <c>false</c> otherwise</returns>
		private bool PluginEnabled(Type type)
		{
			IPlugin currentPlugin = GetPluginByType(type);
			if (Config.CurrentBlog.EnabledPluginGuids.Contains(currentPlugin.Id.Guid))
				return true;
			else
				return false;
		}


		#endregion
	}


}
