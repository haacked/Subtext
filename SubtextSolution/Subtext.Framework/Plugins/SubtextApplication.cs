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
using System.Web.Configuration;
using Subtext.Framework.Configuration;
using Subtext.Framework.Components;
using Subtext.Framework.Logging;

namespace Subtext.Extensibility.Plugins
{

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
	public sealed class SubtextApplication
	{
		private EventHandlerList Events = new EventHandlerList();
		
		#region Event Keys (static)
		private static object EventEntryUpdating = new object();
		private static object EventEntryUpdated = new object();

		private static object EventEntryRendering = new object();
		private static object EventEntryRendered = new object();

		private static object EventSingleEntryRendering = new object();
		private static object EventSingleEntryRendered = new object();

		private static object EventEntrySyndicating = new object();
		private static object EventEntrySyndicated = new object();

		private static object EventCommentUpdating = new object();
		private static object EventCommentUpdated = new object();

		private static object EventImageUpdating = new object();
		private static object EventImageUpdated = new object();

		private static object EventCommentRendering = new object();
		private static object EventCommentRendered = new object();

		private static object EventImageRendering = new object();
		private static object EventImageRendered = new object();
		#endregion


		private static bool _initialized;
		private Dictionary<Guid, PluginBase> _plugins = new Dictionary<Guid, PluginBase>();
		private List<string> _pluginLoadingErrors = new List<string>();

		private static readonly Log __log = new Log();
		private static SubtextApplication _instance = LoadPlugins();

		private PluginSettingsCollection _pluginConfig;

		/// <summary>
		/// List of all available plugins configurations
		/// </summary>
		public PluginSettingsCollection PluginConfig
		{
			get { return _pluginConfig; }
		}

		/// <summary>
		/// All avalialbe plugins. These plugins are already initialized
		/// </summary>
		public Dictionary<Guid, PluginBase> Plugins
		{
			get { return _plugins; }
		}

		/// <summary>
		/// 
		/// </summary>
		public List<string> PluginLoadingErrors
		{
			get { return _pluginLoadingErrors; }
		}

		/// <summary>
		/// Returns the current single instance of the class
		/// </summary>
		public static SubtextApplication Current
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

		private SubtextApplication()
		{
		}

		/// <summary>
		/// Load all plugins from the web.config file, and return a configured instance of the SubtextApplication, with all the plugins already initialized
		/// </summary>
		/// <returns>Configured instance of the SubtextApplication</returns>
		private static SubtextApplication LoadPlugins()
		{
			SubtextApplication app = new SubtextApplication();
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
							notifyPluginLoadingProblem("Cannot load plugin defined at line " + setting.ElementInformation.LineNumber + ":<br>\r\nMissing Type", false, app._pluginLoadingErrors);
							continue;
						}

						if (String.IsNullOrEmpty(setting.Name))
						{
							notifyPluginLoadingProblem("Cannot load plugin defined at line " + setting.ElementInformation.LineNumber + ":<br>\r\nMissing Name", false, app._pluginLoadingErrors);
							continue;
						}

						Type type = Type.GetType(setting.Type);

						if (type == null)
						{
							notifyPluginLoadingProblem("Cannot load plugin defined at line " + setting.ElementInformation.LineNumber + ":<br>\r\nType \"" + setting.Type + "\" not found in any of the assembly available inside the \\bin folder", false, app._pluginLoadingErrors);
							continue;
						}
						PluginBase plugin = Activator.CreateInstance(type) as PluginBase;

						if (plugin == null)
						{
							notifyPluginLoadingProblem("Cannot load plugin defined at line " + setting.ElementInformation.LineNumber + ":<br>\r\nType \"" + setting.Type + "\" doesn't inherits from PluginBase abstract class", false, app._pluginLoadingErrors);
							continue;
						}

						if (plugin.Id == Guid.Empty)
						{
							notifyPluginLoadingProblem("Cannot load plugin with name " + setting.Name + ":<br>\r\nthe plugin doesn't provide a Guid", false, app._pluginLoadingErrors);
							continue;
						}

						if (plugin.Info == null)
						{
							notifyPluginLoadingProblem("Cannot load plugin with name " + setting.Name + ":<br>\r\nthe plugin doesn't provide at least one descriptive metadata", false, app._pluginLoadingErrors);
							continue;
						}

						try
						{

							if (app._plugins.ContainsKey(plugin.Id))
							{
								notifyPluginLoadingProblem("Cannot load plugin with name " + setting.Name + ":<br>\r\na plugin with the same Guid has already been added to the system", false, app._pluginLoadingErrors);
								continue;
							}
							else
							{
								plugin.Init(app);
								plugin.DefaultSettings = setting.DefaultSettings;
								app._plugins.Add(plugin.Id, plugin);
							}
						}
						catch (Exception ex)
						{
							notifyPluginLoadingProblem("Error initializing plugin with name " + setting.Name + " from type " + setting.Type + "<br>\r\nThe Init method threw the following exception:<br>\r\n" + ex.Message, true, app._pluginLoadingErrors);
							continue;
						}
						
						#if DEBUG
						__log.Debug("Loaded plugin with name " + setting.Name + " from type " + setting.Type);
						#endif
						
					}
				}
			}
			_initialized = true;
			return app;
		}

		private static void notifyPluginLoadingProblem(string message, bool isError, ICollection<string> errorList)
		{
			if (isError)
				__log.Error(message);
			else
				__log.Warn(message);

			errorList.Add(message);

		}

		#region Event definitions

		/// <summary>
		/// Raised before changes to the Entry are committed to the datastore
		/// </summary>
		public event EventHandler<CancellableEntryEventArgs> EntryUpdating
		{
			add
			{
				Events.AddHandler(EventEntryUpdating, value);
			}
			remove
			{
				Events.RemoveHandler(EventEntryUpdating, value);
			}
		}

		/// <summary>
		/// Raised after the changes has been committed to the datastore
		/// </summary>
		public event EventHandler<EntryEventArgs> EntryUpdated
		{
			add
			{
				Events.AddHandler(EventEntryUpdated, value);
			}
			remove
			{
				Events.RemoveHandler(EventEntryUpdated, value);
			}
		}

		/// <summary>
		/// Raised an individual entry is rendered
		/// </summary>
		public event EventHandler<EntryEventArgs> SingleEntryRendering
		{
			add
			{
				Events.AddHandler(EventSingleEntryRendering, value);
			}
			remove
			{
				Events.RemoveHandler(EventSingleEntryRendering, value);
			}
		}

		/// <summary>
		/// Raised when entry is rendered in the homepage
		/// </summary>
		public event EventHandler<EntryEventArgs> EntryRendering
		{
			add
			{
				Events.AddHandler(EventEntryRendering, value);
			}
			remove
			{
				Events.RemoveHandler(EventEntryRendering, value);
			}
		}
		
		/// <summary>
		/// Raised when entry is syndicated as RSS or Atom feed
		/// </summary>
		public event EventHandler<EntryEventArgs> EntrySyndicating
		{
			add
			{
				Events.AddHandler(EventEntrySyndicating, value);
			}
			remove
			{
				Events.RemoveHandler(EventEntrySyndicating, value);
			}
		}

		#endregion

		#region Event Execution

		internal void ExecuteEntryUpdating(object sender, CancellableEntryEventArgs e)
		{
			ExecuteEvent(EventEntryUpdating, sender, e);
		}

		internal void ExecuteEntryUpdated(object sender, EntryEventArgs e)
		{
			ExecuteEvent(EventEntryUpdated, sender, e);
		}

		internal void ExecuteEntryRendering(object sender, EntryEventArgs e)
		{
			ExecuteEvent(EventEntryRendering, sender, e);
		}

		internal void ExecuteEntryRendered(object sender, EntryEventArgs e)
		{
			ExecuteEvent(EventEntryRendered, sender, e);
		}

		internal void ExecuteSingleEntryRendering(object sender, EntryEventArgs e)
		{
			ExecuteEvent(EventSingleEntryRendering, sender, e);
		}

		internal void ExecuteSingleEntryRendered(object sender, EntryEventArgs e)
		{
			ExecuteEvent(EventSingleEntryRendered, sender, e);
		}
		
		internal void ExecuteEntrySyndicating(object sender, EntryEventArgs e)
		{
			ExecuteEvent(EventEntrySyndicating, sender, e);
		}

		internal void ExecuteEntrySyndicated(object sender, EntryEventArgs e)
		{
			ExecuteEvent(EventEntrySyndicated, sender, e);
		}

		internal void ExecuteCommentUpdating(object sender, FeedbackEventArgs e)
		{
			ExecuteEvent(EventCommentUpdating, sender, e);
		}

		internal void ExecuteCommentUpdated(object sender, FeedbackEventArgs e)
		{
			ExecuteEvent(EventCommentUpdated, sender, e);
		}

		internal void ExecuteCommentRendering(object sender, FeedbackEventArgs e)
		{
			ExecuteEvent(EventCommentRendering, sender, e);
		}

		internal void ExecuteCommentRendered(object sender, FeedbackEventArgs e)
		{
			ExecuteEvent(EventCommentRendered, sender, e);
		}

		internal void ExecuteImageUpdating(object sender, ImageEventArgs e)
		{
			ExecuteEvent(EventImageUpdating, sender, e);
		}

		internal void ExecuteImageUpdated(object sender, ImageEventArgs e)
		{
			ExecuteEvent(EventImageUpdated, sender, e);
		}

		internal void ExecuteImageRendering(object sender, ImageEventArgs e)
		{
			ExecuteEvent(EventImageRendering, sender, e);
		}

		internal void ExecuteImageRendered(object sender, ImageEventArgs e)
		{
			ExecuteEvent(EventImageRendered, sender, e);
		}

		//List through the subscribed event handlers, and decide weather call them or not
		//based on the current blog enabled plugins
		private void ExecuteEvent(object eventKey, object sender, SubtextEventArgs e)
		{
			EventHandler<SubtextEventArgs> handler = Events[eventKey] as EventHandler<SubtextEventArgs>;
			if (handler != null)
			{
				Delegate[] delegates = handler.GetInvocationList();
				foreach (Delegate del in delegates)
				{
					PluginBase currentPlugin = GetPluginByType(del.Method.DeclaringType);
					if (PluginEnabled(currentPlugin))
					{
						try
						{
							e.CallingPluginGuid = currentPlugin.Id;
							del.DynamicInvoke(sender, e);
						}
						catch(Exception ex)
						{
							if (IsCritical(ex)) throw;
							__log.Error(String.Format("Plugin: {0}<br />\r\nEvent: {1}<br />\r\nError message:<br />\r\n{2}", currentPlugin.Info.Name, del.Method.Name, ex.Message), ex);
						}
					}
				}
			}
		}

		#endregion


		#region Helper Functions

		/// <summary>
		/// Get the initialized plugin given its guid in string format
		/// </summary>
		/// <param name="guid">the GUID in string format</param>
		/// <returns>The initialized instance of the plugin</returns>
		public PluginBase GetPluginByGuid(string guid)
		{
			return GetPluginByGuid(new Guid(guid));
		}

		/// <summary>
		/// Get the initialized plugin given its guid
		/// </summary>
		/// <param name="guid">the GUID</param>
		/// <returns>The initialized instance of the plugin</returns>
		public PluginBase GetPluginByGuid(Guid guid)
		{
			if (_plugins.ContainsKey(guid))
				return _plugins[guid];
			else
				return null;
		}


		/// <summary>
		/// Get the initialized plugin given its type
		/// </summary>
		/// <param name="type">the type of the plugin</param>
		/// <returns>The initialized instance of the plugin</returns>
		private PluginBase GetPluginByType(Type type)
		{
			foreach (PluginBase plugin in _plugins.Values)
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
		/// <param name="plugin">The plugin</param>
		/// <returns><c>true</c> if the plugin is enabled, <c>false</c> otherwise</returns>
		public static bool PluginEnabled(PluginBase plugin)
		{
			foreach (Plugin blogPlugin in Config.CurrentBlog.EnabledPlugins.Values)
			{
				if (blogPlugin.InitializedPlugin == plugin)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Check if the exception is critical or not.
		/// Inspired by:
		/// http://www.codeproject.com/csharp/csmverrorhandling.asp
		/// </summary>
		/// <param name="ex">exception to check</param>
		/// <returns>true if the ex is critical, false if not</returns>
		private static bool IsCritical(Exception ex)
		{
			if (ex is OutOfMemoryException) return true;
			if (ex is AppDomainUnloadedException) return true;
			if (ex is BadImageFormatException) return true;
			if (ex is CannotUnloadAppDomainException) return true;
			if (ex is ExecutionEngineException) return true;
			if (ex is InvalidProgramException) return true;
			if (ex is System.Threading.ThreadAbortException)
				return true;
			return false;
		}

		#endregion


	}


}
