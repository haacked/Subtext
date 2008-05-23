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
using System.Diagnostics;
using System.Globalization;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.Web;
using Subtext.Web.Controls;
using System.Web;

namespace Subtext.Web.Admin.Pages
{
    /// <summary>
    /// Displays comments posted to the blog and allows the 
    /// admin to delete comments.
    /// </summary>
    public partial class Feedback : ConfirmationPage
    {
        private const string VSKEY_FEEDBACKID = "FeedbackID";
        private int pageIndex = 0;
        private bool _isListHidden = false;
        private bool _hasViewChanged = false;

        /// <summary>
        /// Constructs an image of this page. Sets the tab section to "Feedback".
        /// </summary>
        public Feedback()
        {
            this.TabSectionId = "Feedback";
        }

        /// <summary>
        /// Whether or not to moderate comments.
        /// </summary>
        protected FeedbackStatusFlag FeedbackStatusFilter
        {
            get
            {
                return (FeedbackStatusFlag)(ViewState["FeedbackStatusFilter"] ?? FeedbackStatusFlag.Approved);
            }
            set
            {
                ViewState["FeedbackStatusFilter"] = value;
            }
        }



        #region Web Form Designer generated code
        override protected void OnInit(EventArgs e)
        {
            //
            // CODEGEN: This call is required by the ASP.NET Web Form Designer.
            //
            InitializeComponent();
            base.OnInit(e);
        }

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {

        }
        #endregion




    }
}





