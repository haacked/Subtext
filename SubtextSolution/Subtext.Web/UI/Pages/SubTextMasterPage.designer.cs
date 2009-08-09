using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Web.UI.Controls;

namespace Subtext.Web.UI.Pages
{
    public partial class SubtextMasterPage
    {
        protected Literal pageTitle;
        protected Literal docTypeDeclaration;
        protected HtmlLink CustomCss;
        protected HtmlLink RSSLink;
        protected HtmlLink Rsd;
        protected HtmlLink AtomLink;
        protected PlaceHolder CenterBodyControl;
        protected Literal versionMetaTag;
        protected Literal authorMetaTag;
        protected Literal scripts;
        protected Literal styles;
        protected Literal virtualRoot;
        protected Literal virtualBlogRoot;
        protected Literal customTrackingCode;
        protected Literal openIDServer;
        protected Literal openIDDelegate;
        protected PlaceHolder metaTagsPlaceHolder;
        protected Comments commentsControl;
        protected PostComment postCommentControl;
    }
}
