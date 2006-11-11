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
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework.Text;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// Static class containing helper methods for various controls 
	/// that can't be placed within the control hierarchy.
	/// </summary>
	public static class ControlHelper
	{
		/// <summary>
		/// If the URL is is the format ~/SomePath, this 
		/// method expands the tilde using the app path.
		/// </summary>
		/// <param name="path"></param>
		public static string ExpandTildePath(string path)
		{
			if (String.IsNullOrEmpty(path))
				throw new ArgumentNullException("path", "Cannot expand a null or empty path");

			if(path.Substring(0, 2) == "~/")
			{
				string appPath = HttpContext.Current.Request.ApplicationPath;
				if(appPath == null)
					appPath = string.Empty;
				if(appPath.EndsWith("/"))
				{
					appPath = StringHelper.Left(appPath, appPath.Length - 1);
				}
				return appPath + path.Substring(1);
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
			if (control == null)
				throw new ArgumentNullException("Cannot check a null control for an attribute.");
			
			if (name == null)
				throw new ArgumentNullException("name", "Attribute name is null.");

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
			if (control == null)
				throw new ArgumentNullException("Cannot check a null control for an attribute.");
			
			if (name == null)
				throw new ArgumentNullException("name", "Attribute name is null.");
			
			return control.Attributes[name] != null && control.Attributes[name].Length > 0;
		}

		/// <summary>
		/// Applies the specified control action recursively.
		/// </summary>
		/// <param name="controlAction">The control action.</param>
		public static void ApplyRecursively(ControlAction controlAction, Control root)
		{
			if (controlAction == null)
				throw new ArgumentNullException("Cannot apply a null action to every control. Just don't call this method.");

			if (root == null)
				throw new ArgumentNullException("Cannot apply an action to a null control root.");
			
			foreach(Control control in root.Controls)
			{
				controlAction(control);
				ApplyRecursively(controlAction, control);
			}
		}

		/// <summary>
		/// Recursively searches for the server form.
		/// </summary>
		/// <param name="control">The parent to start the recursive search from.</param>
		/// <param name="id">Id of the control to find.</param>
		/// <returns></returns>
		public static Control FindControlRecursively(Control control, string id)
		{
			if (control == null)
				throw new ArgumentNullException("control", "Cannot search a null control.");

			if (id == null)
				throw new ArgumentNullException("id", "Cannot search for a null id.");
			
			if(control.ID == id)
				return control;
			
			foreach (Control child in control.Controls)
			{                        
				Control foundControl = FindControlRecursively(child, id);
				if(foundControl != null)
				{
					return foundControl;
				}
			}

			return null;
		}

		/// <summary>
		/// Recursively searches for the server form's client id.
		/// </summary>
		/// <param name="control">The root control to start the search at.</param>
		/// <returns></returns>
		public static string GetPageFormClientId(Control control)
		{
			if (control == null)
				throw new ArgumentNullException("parent", "Cannot find form for a null parent control");

			if (control is HtmlForm)
				return control.ClientID;
			
			string id;
			foreach (Control child in control.Controls)
			{                        
				id = GetPageFormClientId(child);
			    if(id != null)
			    {
			        return id;
			    }
			}
         
			return null;
		}

		/// <summary>
		/// Exports the contents of the specified control to excel.
		/// </summary>
		/// <param name="control">The control to render as an excel file.</param>
		/// <param name="filename">The filename to export it to.</param>
		public static void ExportToExcel(Control control, string filename)
		{
			if (control == null)
				throw new ArgumentNullException("control", "Cannot export a null control to Excel");

			if (filename == null)
				throw new ArgumentNullException("filename", "Cannot export to a null filename");
			
			// Set the content type to Excel
			HttpContext.Current.Response.AddHeader( "Content-Disposition", "filename=" + filename); 
			HttpContext.Current.Response.ContentType = "application/vnd.ms-excel";

			control.Page.EnableViewState = false;

			//Remove the charset from the Content-Type header
			HttpContext.Current.Response.Charset = String.Empty;

			StringWriter writer = new StringWriter();
			HtmlTextWriter htmlWriter = new HtmlTextWriter(writer);
		
			//Get the HTML for the control
			control.RenderControl(htmlWriter);

			HttpContext.Current.Response.Write(writer.ToString());
			HttpContext.Current.Response.End();
		}

		public static void SetTitleIfNone(LinkButton link, string title)
		{
			SetTitleIfNoneInternal(link, title);
		}

		public static void SetTitleIfNone(HyperLink link, string title)
		{
			SetTitleIfNoneInternal(link, title);
		}

		/// <summary>
		/// Sets the title attribute for the hyperlink if none exists.
		/// </summary>
		/// <param name="link">The link.</param>
		/// <param name="title">The title.</param>
		private static void SetTitleIfNoneInternal(WebControl link, string title)
		{
			if(link == null)
				throw new ArgumentNullException("link", "Cannot set the title for a null Hyperlink.");

			if(link.Attributes["title"] == null || link.Attributes["title"].Length == 0)
			{
				link.Attributes["title"] = title;
			}
		}
	}

	public delegate void ControlAction(Control control);
}