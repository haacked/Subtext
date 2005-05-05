
using System;
using System.Collections;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.Design;

#region Code Notes
/*
	The MasterPage controls (MasterPage and ContentRegion) are almost entirely based off of Paul Wilson's excellect demo found
	here: http://authors.aspalliance.com/paulwilson/Articles/?id=14
	
	Very MINOR changes were made here. Thanks Paul.
*/
#endregion

namespace Subtext.Web.UI.WebControls
{
	[ToolboxData("<{0}:MasterPage runat=server></{0}:MasterPage>"),
		ToolboxItem(typeof(WebControlToolboxItem)),
		Designer(typeof(ReadWriteControlDesigner))]
	public class MasterPage : System.Web.UI.HtmlControls.HtmlContainerControl
	{
		private string templateFile = null;
		private Control template = null;

		private ArrayList contents = new ArrayList();
		private const string SkinPath = "~/Skins/{0}/PageTemplate.ascx";

		[Category("MasterPage"), Description("Path of Template User Control")] 
		public string TemplateFile {
			get 
			{ 
				if(this.templateFile == null)
				{
					this.templateFile =  string.Format(SkinPath,Globals.Skin(Context));
				}
				return this.templateFile;
			}
			set { this.templateFile = value; }
		}

		public MasterPage() {}

		protected override void AddParsedSubObject(object obj) {
			if (obj is ContentRegion) {
				this.contents.Add(obj);
			}
		}

		protected override void OnInit(EventArgs e) {
			this.BuildMasterPage();
			this.BuildContents();
			base.OnInit(e);
		}

		private void BuildMasterPage() {
			if (this.TemplateFile == null || this.TemplateFile == string.Empty) 
			{
				throw new ApplicationException("TemplateFile Property for MasterPage must be Defined");
			}
			this.template = this.Page.LoadControl(this.TemplateFile);
			this.template.ID = this.ID + "_Template";
			
			int count = this.template.Controls.Count;
			for (int index = 0; index < count; index++) {
				Control control = this.template.Controls[0];
				this.template.Controls.Remove(control);
				if (control.Visible) {
					this.Controls.Add(control);
				}
			}
			this.Controls.AddAt(0, this.template);
		}

		private void BuildContents() {
//			if (this.defaults.HasControls()) {
//				this.defaults.ID = this.defaultContent;
//				this.contents.Add(this.defaults);
//			}

			foreach (ContentRegion content in this.contents) {
				Control region = this.FindControl(content.ID);
				if (region == null || !(region is ContentRegion)) {
					throw new Exception("ContentRegion with ID '" + content.ID + "' must be Defined");
				}
				region.Controls.Clear();
				
				int count = content.Controls.Count;
				for (int index = 0; index < count; index++) {
					Control control = content.Controls[0];
					content.Controls.Remove(control);
					region.Controls.Add(control);
				}
			}
		}

		//removes this controls ability to render its own start tag
		protected override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer) {}
		//removes this controls ability to render its own end tag
		protected override void RenderEndTag(System.Web.UI.HtmlTextWriter writer) {}
	}
}
