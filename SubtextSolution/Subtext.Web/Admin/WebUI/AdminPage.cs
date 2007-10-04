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
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework.Configuration;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Controls;
using Subtext.Framework.Security;

namespace Subtext.Web.Admin.Pages
{
	internal enum CookieSupportType
	{
		Untested,
		Testing,
		Allowed,
		NotAllowed
	}

	/// <summary>
	/// Base Page class for all pages in the admin tool.
	/// </summary>
	public class AdminPage : Page
	{
        private HtmlGenericControl body;
		private ConfirmCommand _command;
		
		protected override void OnLoad(EventArgs e)
		{
            if (this.Page.Master != null)
            {
                this.body = this.Page.Master.FindControl("AdminSection") as HtmlGenericControl;
            }
            
			if(!IsPostBack)
		    {
                ControlHelper.ApplyRecursively(new ControlAction(SetTextBoxStyle), this);
		        DataBind();
		    }
			base.OnLoad(e);
		}
	    
	    protected override void OnPreRender(EventArgs e)
	    {
	        if(this.body != null)
            {
                this.body.Attributes["class"] = this.TabSectionId;
            }
	    }
	    
	    protected AdminPageTemplate AdminMasterPage
	    {
	        get
	        {
                return (AdminPageTemplate)this.Page.Master;
	        }
	    }

		static void SetTextBoxStyle(Control control)
		{
			TextBox textBox = control as TextBox;
			if(textBox != null)
			{
				if(textBox.TextMode == TextBoxMode.SingleLine || textBox.TextMode == TextBoxMode.Password)
                    AddCssClass(textBox, "textinput");
                if (textBox.TextMode == TextBoxMode.MultiLine)
                {
                    AddCssClass(textBox, "textarea");
                }
			}
		}

		protected string CreateAdminRssUrl(string pageName)
		{
			return String.Format("{0}Admin/{1}", Config.CurrentBlog.RootUrl, pageName);
		}
	    private static  void AddCssClass(WebControl control, string cssClass)
	    {
			if (control.CssClass != null && control.CssClass.Length > 0 && !String.Equals(cssClass, control.CssClass, StringComparison.InvariantCultureIgnoreCase))
            {
                control.CssClass += " " + cssClass;
            }
            else
            {
                control.CssClass = cssClass;
            }
	    }

		public ConfirmCommand Command
		{
			get { return _command; }
			set { _command = value; }
		}

        [Category("Page")]
        [Description("Page tab section identifier")]
        [Browsable(true)]
        public string TabSectionId
        {
            get { return tabSectionId; }
            protected set { tabSectionId = value;}
        }
        string tabSectionId;

	}
}

