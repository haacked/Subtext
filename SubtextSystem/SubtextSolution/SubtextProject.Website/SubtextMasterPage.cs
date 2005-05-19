using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using SubtextProject.Website.Controls;
using SubtextProject.Website.Text;

namespace SubtextProject.Website
{
	/// <summary>
	/// Inherits the Wilson MasterPage control and does a little 
	/// bit of processing for our own needs.
	/// </summary>
	public class SubtextMasterPage : HtmlContainerControl
	{
		private ArrayList contents;
		private string defaultContent;
		private ContentRegion defaults;
		private Control template;
		private string templateFile;

		/// <summary>
		/// Creates a new <see cref="SubtextMasterPage"/> instance.
		/// </summary>
		public SubtextMasterPage()
		{
			this.template = null;
			this.defaults = new ContentRegion();
			this.contents = new ArrayList();
			this.templateFile = ConfigurationSettings.AppSettings["Wilson.MasterPages.TemplateFile"];
			this.defaultContent = ConfigurationSettings.AppSettings["Wilson.MasterPages.DefaultContent"];
			if (this.defaultContent == null)
			{
				this.defaultContent = "Content";
			}
		}

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

		[Description("Path of Template User Control"), Category("MasterPage")]
		public string TemplateFile
		{
			get
			{
				return this.templateFile;
			}
			set
			{
				this.templateFile = value;
			}
		}

		protected override void OnInit(EventArgs e)
		{
			this.BuildMasterPage();
			this.BuildContents();
			base.OnInit(e);
		}

		protected override void AddParsedSubObject(object obj)
		{		
			if (obj is ContentRegion)
			{
				this.contents.Add(obj);
			}
			else
			{
				this.defaults.Controls.Add((Control) obj);
			}
		}

		private void BuildContents()
		{
			if (this.defaults.HasControls())
			{
				this.defaults.ID = this.defaultContent;
				this.contents.Add(this.defaults);
			}
			foreach (ContentRegion region in this.contents)
			{
				Control control1 = this.FindControl(region.ID);
				if ((control1 == null) || !(control1 is ContentRegion))
				{
					throw new Exception("ContentRegion with ID '" + region.ID + "' must be Defined");
				}
				control1.Controls.Clear();
				int controlCount = region.Controls.Count;
				for (int i = 0; i < controlCount; i++)
				{
					Control control2 = region.Controls[0];
					region.Controls.Remove(control2);

					CorrectReferences(control2);
					control1.Controls.Add(control2);
				}
			}
		}

		private void BuildMasterPage()
		{
			if (this.templateFile == "")
			{
				throw new Exception("TemplateFile Property for MasterPage must be Defined");
			}
			this.template = this.Page.LoadControl(this.templateFile);
			this.template.ID = this.ID + "_Template";
			int controlCount = this.template.Controls.Count;
			
			for (int i = 0; i < controlCount; i++)
			{
				Control control1 = this.template.Controls[0];
				this.template.Controls.Remove(control1);
				if (control1.Visible)
				{
					CorrectReferences(control1);
					this.Controls.Add(control1);
				}
			}
			this.Controls.AddAt(0, this.template);
		}

		protected override void RenderBeginTag(HtmlTextWriter writer)
		{
		}
 
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
				CorrectReferenceAttribute((HtmlContainerControl)obj);
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
						appPath = StringHelper.Left(appPath, appPath.Length - 1);
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
