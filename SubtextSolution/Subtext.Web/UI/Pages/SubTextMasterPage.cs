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
using System.Collections.Generic;
using System.Text;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.UrlManager;
using Subtext.Web.UI.Controls;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework.Routing;
using System.Web.Routing;

namespace Subtext.Web.UI.Pages
{
    /// <summary>
    /// This serves as the master page for every page within the 
    /// site (except for the admin section.  This page contains 
    /// a PlaceHolder in which the PageTemplate.ascx control within 
    /// each skin is loaded.
    /// </summary>
    public class SubtextMasterPage : RoutablePage, ISubtextPage
    {
        #region Declared Controls in DTP.aspx
        private static readonly ScriptElementCollectionRenderer scriptRenderer = new ScriptElementCollectionRenderer(new SkinTemplateCollection());
        private static readonly StyleSheetElementCollectionRenderer styleRenderer = new StyleSheetElementCollectionRenderer(new SkinTemplateCollection());
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
        #endregion

        protected BlogInfo CurrentBlog;
        protected Comments commentsControl;
        protected PostComment postCommentControl;
        protected const string TemplateLocation = "~/Skins/{0}/{1}";
        protected const string ControlLocation = "~/Skins/{0}/Controls/{1}";
        protected const string OpenIDServerLocation = "<link rel=\"openid.server\" href=\"{0}\" />";
        protected const string OpenIDDelegateLocation = "<link rel=\"openid.delegate\" href=\"{0}\" />";

        protected SkinConfig CurrentSkin = Globals.CurrentSkin;

        public static readonly string CommentsPanelId = "commentsUpdatePanelWrapper";

        private void InitializeBlogPage()
        {
            MaintainScrollPositionOnPostBack = true;
            CurrentBlog = Config.CurrentBlog;

            string skinFolder = CurrentSkin.TemplateFolder;

            IEnumerable<string> controls = _controls;
            if (controls != null)
            {
                UpdatePanel apnlCommentsWrapper = new UpdatePanel();
                apnlCommentsWrapper.Visible = true;
                apnlCommentsWrapper.ID = CommentsPanelId;

                foreach (string controlId in controls)
                {
                    Control control = LoadControl(string.Format(ControlLocation, skinFolder, controlId));
                    control.ID = controlId.Replace(".", "_");

                    if (controlId.Equals("Comments.ascx"))
                    {
                        control.Visible = true;
                        commentsControl = control as Comments;
                        apnlCommentsWrapper.ContentTemplateContainer.Controls.Add(control);
                    }
                    else if (controlId.Equals("PostComment.ascx"))
                    {
                        postCommentControl = (PostComment)control;
                        postCommentControl.CommentApproved += postCommentControl_CommentPosted;
                        apnlCommentsWrapper.ContentTemplateContainer.Controls.Add(control);
                        CenterBodyControl.Controls.Add(apnlCommentsWrapper);
                    }
                    else
                    {
                        CenterBodyControl.Controls.Add(control);
                    }
                }
            }

            if (CurrentSkin.HasCustomCssText)
            {
                CustomCss.Attributes.Add("href", CurrentBlog.RootUrl + "customcss.aspx");
            }
            else
            {
                //MAC IE does not like the empy CSS file, plus its a waste :)
                CustomCss.Visible = false;
            }

            if (Rsd != null)
            {
                Rsd.Attributes.Add("href", CurrentBlog.RootUrl + "rsd.xml.ashx");
            }

            if (RSSLink != null)
            {
                RSSLink.Attributes.Add("href", CurrentBlog.UrlFormats.RssUrl.ToString());
            }

            if (AtomLink != null)
            {
                AtomLink.Attributes.Add("href", CurrentBlog.UrlFormats.RssUrl.ToString());
            }

            // if specified, add script elements
            if (scripts != null)
            {
                scripts.Text = scriptRenderer.RenderScriptElementCollection(CurrentSkin.SkinKey);
            }

            if (styles != null)
            {
                styles.Text = styleRenderer.RenderStyleElementCollection(CurrentSkin.SkinKey);
            }

            if (openIDServer != null && !string.IsNullOrEmpty(CurrentBlog.OpenIDServer))
            {
                openIDServer.Text = string.Format(OpenIDServerLocation, CurrentBlog.OpenIDServer);
            }

            if (openIDDelegate != null && !string.IsNullOrEmpty(CurrentBlog.OpenIDDelegate))
            {
                openIDDelegate.Text = string.Format(OpenIDDelegateLocation, CurrentBlog.OpenIDDelegate);
            }

            // Add the per-blog MetaTags to the page Head section.
            IPagedCollection<MetaTag> blogMetaTags = MetaTags.GetMetaTagsForBlog(CurrentBlog, 0, int.MaxValue);
            foreach (MetaTag tag in blogMetaTags)
            {
                HtmlMeta mt = new HtmlMeta();
                mt.Content = tag.Content;

                if (!string.IsNullOrEmpty(tag.Name))
                    mt.Name = tag.Name;
                else
                    mt.HttpEquiv = tag.HttpEquiv;

                Literal newLineLiteral = new Literal();
                newLineLiteral.Text = Environment.NewLine;
                metaTagsPlaceHolder.Controls.Add(newLineLiteral);
                metaTagsPlaceHolder.Controls.Add(mt);
            }
        }

        void postCommentControl_CommentPosted(object sender, EventArgs e)
        {
            commentsControl.BindFeedback(false); //don't get it from cache.
        }


        /// <summary>
        /// Before rendering, turns off ViewState again (why? not sure),  
        /// applies skins and stylesheet references, and sets the page title.
        /// </summary>
        /// <param name="e">E.</param>
        protected override void OnPreRender(EventArgs e)
        {
            Response.ContentEncoding = Encoding.UTF8; //TODO: allow for per/blog config.
            Response.ContentType = "text/html"; //TODO: allow for per/blog config.

            //Is this for extra security?
            EnableViewState = false;
            pageTitle.Text = Globals.CurrentTitle(Context);
            if (!String.IsNullOrEmpty(Config.CurrentBlog.Author))
            {
                authorMetaTag.Text = String.Format(Environment.NewLine + "<meta name=\"author\" content=\"{0}\" />", Config.CurrentBlog.Author);
            }
            versionMetaTag.Text = String.Format(Environment.NewLine + "<meta name=\"Generator\" content=\"{0}\" />" + Environment.NewLine, VersionInfo.VersionDisplayText);

            if (!String.IsNullOrEmpty(Config.CurrentBlog.TrackingCode))
            {
                customTrackingCode.Text = Config.CurrentBlog.TrackingCode;
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Initializes this blog page, applying skins and whatnot.
        /// </summary>
        /// <param name="e">E.</param>
        override protected void OnInit(EventArgs e)
        {
            InitializeBlogPage();
            base.OnInit(e);
        }

        /// <summary>
        /// Loads the page state from persistence medium.  In this case 
        /// this returns null as we are not using ViewState.
        /// </summary>
        /// <returns></returns>
        protected override object LoadPageStateFromPersistenceMedium()
        {
            return null;
        }

        /// <summary>
        /// Saves the page state to persistence medium.  In this case 
        /// this does nothing as we are not using ViewState.
        /// </summary>
        /// <param name="viewState">State of the view.</param>
        protected override void SavePageStateToPersistenceMedium(object viewState) { }

        #region private class ScriptElementCollectionRenderer

        #endregion

        /// <summary>
        /// Returns the text for a javascript array of allowed elements. 
        /// This will be used by other scripts.
        /// </summary>
        /// <value>The allowed HTML javascript declaration.</value>
        protected static string AllowedHtmlJavascriptDeclaration
        {
            get
            {
                string declaration = "var subtextAllowedHtmlTags = [";
                for (int i = 0; i < Config.Settings.AllowedHtmlTags.Count; i++)
                {
                    string tagname = Config.Settings.AllowedHtmlTags.Keys[i];
                    declaration += string.Format("'{0}', ", tagname);
                }
                if (Config.Settings.AllowedHtmlTags.Count > 0)
                {
                    declaration = declaration.Left(declaration.Length - 2);
                }

                return declaration + "];";
            }
        }

        public void SetControls(IEnumerable<string> controls)
        {
            _controls = controls;
        }
        IEnumerable<string> _controls;
    }
}
