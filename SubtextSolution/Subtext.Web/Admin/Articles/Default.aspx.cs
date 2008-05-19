using Subtext.Web.Admin.Pages;

namespace Subtext.Web.Admin.Articles {
    public partial class Default : AdminPage 
    {
        public Default() 
        {
            TabSectionId = "Articles";
        }

        protected override void OnLoad(System.EventArgs e) 
        {
            entries.HeaderText = "Articles";
            string message = Request.QueryString["message"];
            if (!string.IsNullOrEmpty(message)) 
            {
                this.Messages.ShowMessage(message);
            }
            base.OnLoad(e);
        }
    }
}
