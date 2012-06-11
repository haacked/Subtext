using Subtext.Web.Admin.Pages;

namespace Subtext.Web.Admin.Posts
{
    public partial class Edit : ConfirmationPage
    {
        public Edit()
        {
            TabSectionId = "Posts";
            IsInEdit = true;
        }
    }
}