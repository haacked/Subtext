using Subtext.Web.Admin.Pages;

namespace Subtext.Web.Admin.Articles {
    public partial class Edit : ConfirmationPage 
    {
        public Edit() 
        {
            TabSectionId = "Articles";
            this.IsInEdit = true;
        }
    }
}
