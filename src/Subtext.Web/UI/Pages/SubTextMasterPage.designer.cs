#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Web.UI.Controls;
using Subtext.Web.UI.WebControls;

namespace Subtext.Web.UI.Pages
{
    public partial class SubtextMasterPage
    {
        protected Literal pageTitle;
        protected Literal docTypeDeclaration;
        protected HtmlLink CustomCss;
        protected HtmlLink RSSLink;
        protected HtmlLink Rsd;
        protected HtmlLink wlwmanifest;
        protected HtmlLink opensearch;
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
        protected MetaTagsControl metaTags;
    }
}
