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
using System.ComponentModel;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.HtmlControls;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;

namespace Subtext.Web.UI.WebControls
{
	/// <summary>
	/// <p>Serves as the master template for the Subtext site.</p>
	/// <p>
	/// The MasterPage controls (MasterPage and ContentRegion) are almost entirely based off 
	/// of Paul Wilson's excellent demo found
	/// here: http://authors.aspalliance.com/paulwilson/Articles/?id=14
	/// Very MINOR changes were made here. Thanks Paul.
	/// </p>
	/// </summary>
	[ToolboxData("<{0}:MasterPage runat=server></{0}:MasterPage>"),
		ToolboxItem(typeof(WebControlToolboxItem)),
		Designer(typeof(ContainerControlDesigner))]
	public class MasterPage : HtmlContainerControl
	{
		Log Log = new Log();
		private string templateFile;
		private Control template;

		private ArrayList contents = new ArrayList();
		private const string skinPath = "~/Skins/{0}/PageTemplate.ascx";

		/// <summary>
		/// Gets or sets the template file from the Skins directory.
		/// </summary>
		/// <value></value>
		[Category("MasterPage"), Description("Path of Template User Control")] 
		public string TemplateFile 
		{
			get 
			{ 
				if(this.templateFile == null)
				{
					this.templateFile = string.Format(skinPath, Globals.Skin());
				}
				return this.templateFile;
			}
			set { this.templateFile = value; }
		}

		protected override void AddParsedSubObject(object obj) 
		{
			if (obj is ContentRegion) {
				this.contents.Add(obj);
			}
		}

		protected override void OnInit(EventArgs e) 
		{
			this.BuildMasterPage();
			this.BuildContents();
			base.OnInit(e);
		}

		private void BuildMasterPage() 
		{
			if (this.TemplateFile == null || this.TemplateFile.Length == 0) 
			{
				throw new InvalidOperationException("TemplateFile Property for MasterPage must be Defined");
			}
			try
			{
				this.template = this.Page.LoadControl(this.TemplateFile);
			}
			catch(HttpException e)
			{
				Log.Warn("The configured skin '" + Config.CurrentBlog.Skin.TemplateFolder + "' does not exist.  Reverting to a default skin.", e);
				Config.CurrentBlog.Skin = SkinConfig.GetDefaultSkin();
				this.templateFile = null;
				this.template = this.Page.LoadControl(this.TemplateFile);
			}
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

		private void BuildContents() 
		{
//			if (this.defaults.HasControls()) {
//				this.defaults.ID = this.defaultContent;
//				this.contents.Add(this.defaults);
//			}

			foreach (ContentRegion content in this.contents) 
			{
				Control region = this.FindControl(content.ID);
				if (region == null || !(region is ContentRegion)) 
				{
					throw new Exception("ContentRegion with ID '" + content.ID + "' must be Defined");
				}
				region.Controls.Clear();
				
				int count = content.Controls.Count;
				for (int index = 0; index < count; index++) 
				{
					Control control = content.Controls[0];
					content.Controls.Remove(control);
					region.Controls.Add(control);
				}
			}
		}

		//removes this controls ability to render its own start tag
		protected override void RenderBeginTag(HtmlTextWriter writer) {}
		//removes this controls ability to render its own end tag
		protected override void RenderEndTag(HtmlTextWriter writer) {}
	}
}
