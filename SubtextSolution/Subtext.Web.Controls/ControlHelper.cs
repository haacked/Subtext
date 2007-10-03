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
using System.Collections;
using System.IO;
using System.Text.RegularExpressions;
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
	public static class ControlHelper
	{
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
		/// If the URL is is the format ~/SomePath, this 
		/// method expands the tilde using the app path.
		/// </summary>
		/// <param name="path"></param>
		public static string ExpandTildePath(string path)
		{
			string reference = path;
			if (reference.Substring(0, 2) == "~/")
			{
				string appPath = HttpContext.Current.Request.ApplicationPath;
				if (appPath == null)
					appPath = string.Empty;
				if (appPath.EndsWith("/"))
				{
					appPath = appPath.Substring(0, appPath.Length - 1);
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
		/// <param name="root">The root control.</param>
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
			
			foreach (Control child in control.Controls)
			{                        
				string id = GetPageFormClientId(child);
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

			// Use the ToolTip propety since that is rendered as the title attribute.
			if (link.ToolTip.Length == 0)
			{
				link.ToolTip = title;
			}
		}

        /// <summary>
        /// Adds the given className to the control's CssClass attribute 
        /// w/o overwriting the control's existing CSS Classes.
        /// </summary>
        /// <param name="control">The WebControl to add a CSS Class to.</param>
        /// <param name="className">The CSS Class Name to add.</param>
        public static void AddClass(WebControl control, string className)
        {
            IList classes = GetClasses(control);

            if (!classes.Contains(className))
            {
                classes.Add(className);
                control.CssClass = string.Join(" ", (string[])ArrayList.Adapter(classes).ToArray(typeof(string)));
            }
        }

        /// <summary>
        /// Removes the given className from the control's CssClass attribute 
        /// w/o removing the control's other existing CSS Classes. If the given 
        /// className doesn't exist on the control, nothing is done.
        /// </summary>
        /// <param name="control">The WebControl to remove a CSS Class from.</param>
        /// <param name="className">The CSS Class Name to remove.</param>
        public static void RemoveClass(WebControl control, string className)
        {
            IList classes = GetClasses(control);

            if (classes.Contains(className))
            {
                classes.Remove(className);
                control.CssClass = string.Join(" ", (string[])ArrayList.Adapter(classes).ToArray(typeof(string)));
            }
        }

        /// <summary>
        /// Gets an IList of CSS Class Names for the given control.
        /// </summary>
        /// <param name="control">The WebControl</param>
        /// <returns>IList of CSS Class Names (strings)</returns>
        public static IList GetClasses(WebControl control)
        {
            IList classes = new ArrayList();

            if (control.CssClass.Length > 0)
            {
                foreach (string className in Regex.Split(control.CssClass, @"\s+"))
                {
                    classes.Add(className);
                }
            }

            return classes;
        }
	}

	public delegate void ControlAction(Control control);
}
