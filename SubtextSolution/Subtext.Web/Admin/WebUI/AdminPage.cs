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
using System.Security.Permissions;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework.Configuration;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Controls;
using Subtext.Framework.Routing;

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
	[PrincipalPermission(SecurityAction.Demand, Role = "Admins")]
	public class AdminPage : RoutablePage
	{
        private HtmlGenericControl body;
		private ConfirmCommand _command;
		
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
	        if(this.body != null)
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

		protected static string CreateAdminRssUrl(string pageName)
		{
			return String.Format("{0}Admin/{1}", Config.CurrentBlog.RootUrl, pageName);
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

