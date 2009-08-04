using Subtext.Web.Admin.Pages;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Posts {
    public partial class Default : AdminPage 
    {
        public Default() 
        {
            TabSectionId = "Posts";
        }

        protected override void OnLoad(System.EventArgs e) 
        {
            entries.HeaderText = Resources.Label_Entries;
            string message = Request.QueryString["message"];
            if (!string.IsNullOrEmpty(message)) 
            {
                this.Messages.ShowMessage(message);
            }
            base.OnLoad(e);
        }
    }
}
