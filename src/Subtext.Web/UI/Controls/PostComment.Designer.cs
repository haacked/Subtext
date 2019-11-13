using Subtext.Web.Controls.Captcha;

namespace Subtext.Web.UI.Controls
{
    public partial class PostComment
    {
        protected System.Web.UI.WebControls.TextBox tbTitle;
        protected System.Web.UI.WebControls.TextBox tbName;
        protected System.Web.UI.WebControls.TextBox tbUrl;
        protected System.Web.UI.WebControls.TextBox tbComment;
        protected System.Web.UI.WebControls.TextBox tbEmail;
        protected System.Web.UI.WebControls.Button btnSubmit;
        protected System.Web.UI.WebControls.Button btnConfirm;
        protected Subtext.Web.Controls.CompliantButton btnCompliantSubmit;
        protected System.Web.UI.WebControls.Label Message;
        protected System.Web.UI.WebControls.CheckBox chkRemember;
        protected InvisibleCaptcha invisibleCaptchaValidator;
        protected CaptchaControl captcha;
        protected SubtextCoComment coComment;
    }
}
