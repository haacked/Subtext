#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.ComponentModel;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework.Web.Handlers;
using Subtext.Web.Admin.Commands;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Controls;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Base Page class for all pages in the admin tool.
	/// </summary>
	public class AdminPage : SubtextPage
	{
        private HtmlGenericControl body;

        protected override void OnInit(EventArgs e)
        {
            if (!User.IsInRole("Admins"))
            {
                FormsAuthentication.RedirectToLoginPage();
                return;
            }
            base.OnInit(e);
        }
	
		protected override void OnLoad(EventArgs e)
		{
            if (this.Page.Master != null)
            {
                this.body = AdminMasterPage.FindControl("AdminSection") as HtmlGenericControl;
            }
            
			if(!IsPostBack)
		    {
                ControlHelper.ApplyRecursively(SetTextBoxStyle, this);
                ControlHelper.ApplyRecursively(SetButtonStyle, this);
		        DataBind();
		    }
			base.OnLoad(e);
		}
	    
	    protected override void OnPreRender(EventArgs e)
	    {
	        if(this.body != null && !String.IsNullOrEmpty(this.TabSectionId))
            {
                ControlHelper.AddCssClass(this.body, this.TabSectionId);
            }
	    }

	    protected AdminPageTemplate AdminMasterPage
	    {
	        get
	        {
                AdminPageTemplate master = this.Master as AdminPageTemplate;
                if (master == null && this.Master != null && this.Master.Master != null) 
                {
                    master = this.Master.Master as AdminPageTemplate;
                }
                return master;
	        }
	    }

		static void SetTextBoxStyle(Control control)
		{
			TextBox textBox = control as TextBox;
			if(textBox != null)
			{
				if(textBox.TextMode == TextBoxMode.SingleLine || textBox.TextMode == TextBoxMode.Password)
                    ControlHelper.AddCssClass(textBox, "textbox");
                if (textBox.TextMode == TextBoxMode.MultiLine)
                {
                    ControlHelper.AddCssClass(textBox, "textarea");
                }
			}
		}

        static void SetButtonStyle(Control control)
        {
            Button button = control as Button;
            if (button != null)
            {
                ControlHelper.AddCssClass(button, "button");
            }
        }

		public ConfirmCommand Command
		{
			get;
			set;
		}

        [Category("Page")]
        [Description("Page tab section identifier")]
        [Browsable(true)]
        public string TabSectionId
        {
            get;
            protected set;
        }
    }
}

