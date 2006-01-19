using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// Static class containing helper methods for various controls 
	/// that can't be placed within the control hierarchy.
	/// </summary>
	public sealed class ControlHelper
	{
		private ControlHelper()
		{}

		/// <summary>
		/// If the URL is is the format ~/SomePath, this 
		/// method expands the tilde using the app path.
		/// </summary>
		/// <param name="path"></param>
		public static string ExpandTildePath(string path)
		{
			string reference = path;
			if(reference.Substring(0, 2) == "~/")
			{
				string appPath = HttpContext.Current.Request.ApplicationPath;
				if(appPath == null)
					appPath = string.Empty;
				if(appPath.EndsWith("/"))
				{
					appPath = StringHelper.Left(appPath, appPath.Length - 1);
				}
				return appPath + reference.Substring(1);
			}
			return path;
		}

		/// <summary>
		/// Returns true if the specified attribute is defined 
		/// on the control.
		/// </summary>
		/// <param name="control">Control.</param>
		/// <param name="name">Name.</param>
		/// <returns></returns>
		public static bool IsAttributeDefined(HtmlControl control, string name)
		{
			return control.Attributes[name] != null && control.Attributes[name].Length > 0;
		}

		/// <summary>
		/// Returns true if the specified attribute is defined 
		/// on the control.
		/// </summary>
		/// <param name="control">Control.</param>
		/// <param name="name">Name.</param>
		/// <returns></returns>
		public static bool IsAttributeDefined(WebControl control, string name)
		{
			return control.Attributes[name] != null && control.Attributes[name].Length > 0;
		}

		/// <summary>
		/// Applies the specified control action recursively.
		/// </summary>
		/// <param name="controlAction">The control action.</param>
		public static void ApplyRecursively(ControlAction controlAction, Control root)
		{
			foreach(Control control in root.Controls)
			{
				controlAction(control);
				ApplyRecursively(controlAction, control);
			}
		}
	}

	public delegate void ControlAction(Control control);
}
