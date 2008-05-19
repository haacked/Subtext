using Subtext.Web.Admin.Pages;

namespace Subtext.Web.Admin.Posts {
    public partial class Default : AdminPage 
    {
        public Default() 
        {
            TabSectionId = "Posts";
        }

        protected override void OnLoad(System.EventArgs e) 
        {
            entries.HeaderText = "Posts";
            string message = Request.QueryString["message"];
            if (!string.IsNullOrEmpty(message)) 
            {
                this.Messages.ShowMessage(message);
            }
            base.OnLoad(e);
        }
    }
}
