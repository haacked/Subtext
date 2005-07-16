using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// The MasterPage controls (MasterPage and ContentRegion) are almost entirely based off 
	/// of Paul Wilson's excellent demo found
	/// here: http://authors.aspalliance.com/paulwilson/Articles/?id=14
	/// Very MINOR changes were made here. Thanks Paul.
	/// </summary>
	public class MasterPage : HtmlContainerControl
	{
		private ArrayList _contents;
		private string defaultContent;
		private ContentRegion _defaults;
		private Control _template;
		private string _templateFile;

		/// <summary>
		/// Creates a new <see cref="MasterPage"/> instance.
		/// </summary>
		public MasterPage()
		{
			this._template = null;
			this._defaults = new ContentRegion();
			this._contents = new ArrayList();
			this._templateFile = ConfigurationSettings.AppSettings["Subtext.MasterPages.TemplateFile"];
			this.defaultContent = ConfigurationSettings.AppSettings["Subtext.MasterPages.DefaultContent"];
			if (this.defaultContent == null)
			{
				this.defaultContent = "Content";
			}
		}

		/// <summary>
		/// Cotnrol ID for the default content
		/// </summary>
		/// <value></value>
		[Description("Control ID for Default Content"), Category("MasterPage")]
		public string DefaultContent
		{
			get
			{
				return this.defaultContent;
			}
			set
			{
				this.defaultContent = value;
			}
		}

		/// <summary>
		/// Gets or sets the path to the template user control.
		/// </summary>
		/// <value></value>
		[Description("Path of Template User Control"), Category("MasterPage")]
		public string TemplateFile
		{
			get
			{
				return this._templateFile;
			}
			set
			{
				this._templateFile = value;
			}
		}

		/// <summary>
		/// Builds the master page and contents.
		/// </summary>
		/// <exception cref="FormatException">
		/// Thrown if a sub-page does not define a <see cref="ContentRegion"/> 
		/// on its master page.
		/// </exception>
		/// <param name="e">Event arguments.</param>
		protected override void OnInit(EventArgs e)
		{
			this.BuildMasterPage();
			this.BuildContents();
			base.OnInit(e);
		}

		/// <summary>
		/// Method called when an HTML or XML element has been parsed 
		/// and is being added to the controls collection.
		/// </summary>
		/// <remarks>
		///	In our case, content regions are added to a private collection of items. 
		///	Other controls are added to the controls collection for the defaults.
		/// </remarks>
		/// <param name="obj"></param>
		protected override void AddParsedSubObject(object obj)
		{		
			if (obj is ContentRegion)
			{
				this._contents.Add(obj);
			}
			else
			{
				this._defaults.Controls.Add((Control) obj);
			}
		}

		/// <exception cref="FormatException">
		/// Thrown if a sub-page does not define a <see cref="ContentRegion"/> 
		/// on its master page.
		/// </exception>
		private void BuildContents()
		{
			if (this._defaults.HasControls())
			{
				this._defaults.ID = this.defaultContent;
				this._contents.Add(this._defaults);
			}

			foreach (ContentRegion contentRegion in this._contents)
			{
				Control region = this.FindControl(contentRegion.ID);
				if ((region == null) || !(region is ContentRegion))
				{
					throw new FormatException("ContentRegion with ID '" + contentRegion.ID + "' must be Defined");
				}
				region.Controls.Clear();
				int controlCount = contentRegion.Controls.Count;
				for (int i = 0; i < controlCount; i++)
				{
					Control control = contentRegion.Controls[0];
					contentRegion.Controls.Remove(control);
					region.Controls.Add(control);
					CorrectReferences(control); //TODO: Make sure this is appropriately placed.
				}
			}
		}

		private void BuildMasterPage()
		{
			if (this._templateFile.Length == 0)
			{
				throw new Exception("TemplateFile Property for MasterPage must be Defined");
			}
			this._template = this.Page.LoadControl(this._templateFile);
			this._template.ID = this.ID + "_Template";
			int controlCount = this._template.Controls.Count;
			
			for (int i = 0; i < controlCount; i++)
			{
				Control control1 = this._template.Controls[0];
				this._template.Controls.Remove(control1);
				if (control1.Visible)
				{
					CorrectReferences(control1);
					this.Controls.Add(control1);
				}
			}
			this.Controls.AddAt(0, this._template);
		}

		/// <summary>
		/// Renders the begin tag. In this case, a no-op.
		/// </summary>
		/// <param name="writer">Writer.</param>
		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
		}
 
		/// <summary>
		/// Renders the end tag.  In this case, a no-op.
		/// </summary>
		/// <param name="writer">Writer.</param>
		protected override void RenderEndTag(HtmlTextWriter writer)
		{
		}

		// Corrects "~/path" references in 
		// HtmlGenericControls with runat="server".
		void CorrectReferences(object obj)
		{
			if(obj is MenuItem)
				return;

			HtmlContainerControl htmlControl = obj as HtmlContainerControl;
			if(htmlControl != null)
			{
				CorrectReferenceAttribute(htmlControl);
				return;
			}

			Control control = obj as Control;
			if(control != null)
			{
				foreach(Control subControl in control.Controls)
				{
					HtmlContainerControl htmlSubControl = subControl as HtmlContainerControl;
					if(htmlControl != null)
					{
						CorrectReferenceAttribute(htmlSubControl);
					}
					else
					{
						CorrectReferences(subControl);
					}
				}
			}
		}

		void CorrectReferenceAttribute(HtmlContainerControl control)
		{
			if(control is MenuItem)
				return;

			if(CorrectReferenceAttribute(control, "href"))
				return;

			CorrectReferenceAttribute(control, "src");
		}

		bool CorrectReferenceAttribute(HtmlContainerControl control, string attributeName)
		{
			if(control is MenuItem)
				return true;

			if(control.Attributes[attributeName] != null 
				&& 	control.Attributes[attributeName].Length > 0)
			{
				string reference = control.Attributes[attributeName];
				if(reference.Substring(0, 2) == "~/")
				{
					string appPath = Context.Request.ApplicationPath;
					if(appPath.EndsWith("/"))
					{
						appPath = appPath.Substring(0, appPath.Length - 1);
					}
					reference = appPath + reference.Substring(1);
					control.Attributes[attributeName] = reference;

					return true;
				}
			}
			return false;
		}
	}
}
