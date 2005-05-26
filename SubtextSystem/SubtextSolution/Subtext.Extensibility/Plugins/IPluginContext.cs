using System;
using System.Web;

namespace Subtext.Extensibility.Plugins
{
	/// <summary>
	/// Summary description for IModuleContext.
	/// </summary>
	public interface IPluginContext
	{
		HttpContext HttpContext {get;}
	}
}
