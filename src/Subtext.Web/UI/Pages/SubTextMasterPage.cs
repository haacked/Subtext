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

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Routing;
using Subtext.Framework.Text;
using Subtext.Framework.UI.Skinning;
using Subtext.Framework.Web.Handlers;
using Subtext.Web.UI.Controls;

namespace Subtext.Web.UI.Pages
{
    /// <summary>
    /// This serves as the master page for every page within the 
    /// site (except for the admin section.  This page contains 
    /// a PlaceHolder in which the PageTemplate.ascx control within 
    /// each skin is loaded.
    /// </summary>
    public partial class SubtextMasterPage : SubtextPage, IPageWithControls
    {
        protected const string ControlLocation = "~/Skins/{0}/Controls/{1}";
        protected const string OpenIdDelegateLocation = "<link rel=\"openid.delegate\" href=\"{0}\" />";
        protected const string OpenIdServerLocation = "<link rel=\"openid.server\" href=\"{0}\" />";
        protected const string TemplateLocation = "~/Skins/{0}/{1}";

        public static readonly string CommentsPanelId = "commentsUpdatePanelWrapper";

        private static readonly ScriptElementCollectionRenderer ScriptRenderer =
            new ScriptElementCollectionRenderer(new SkinEngine());

        private static readonly StyleSheetElementCollectionRenderer StyleRenderer =
            new StyleSheetElementCollectionRenderer(new SkinEngine());

        IEnumerable<string> _controls;
        protected SkinConfig CurrentSkin = Globals.CurrentSkin;

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
                for(int i = 0; i < Config.Settings.AllowedHtmlTags.Count; i++)
                {
                    string tagname = Config.Settings.AllowedHtmlTags.Keys[i];
                    declaration += string.Format(CultureInfo.InvariantCulture, "'{0}', ", tagname);
                }
                if(Config.Settings.AllowedHtmlTags.Count > 0)
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

        private void InitializeBlogPage()
        {
            MaintainScrollPositionOnPostBack = true;

            

            string skinFolder = CurrentSkin.TemplateFolder;
            IEnumerable<string> controls = _controls;
            if(controls != null)
            {
                var apnlCommentsWrapper = new UpdatePanel {Visible = true, ID = CommentsPanelId};

                foreach(string controlId in controls)
                {
                    Control control;
                    try
                    {
                        control = LoadControl(string.Format(ControlLocation, skinFolder, controlId));
                    }
                    catch(HttpException)
                    {
                        // fallback behavior
                        // todo: cache that we found it here.
                        control = LoadControl(string.Format(ControlLocation, "_System", controlId));
                    }
                    control.ID = controlId.Replace(".", "_");

                    if(controlId.Equals("Comments.ascx", StringComparison.OrdinalIgnoreCase))
                    {
                        control.Visible = true;
                        commentsControl = control as Comments;
                        apnlCommentsWrapper.ContentTemplateContainer.Controls.Add(control);
                    }
                    else if(controlId.Equals("PostComment.ascx", StringComparison.OrdinalIgnoreCase))
                    {
                        postCommentControl = control as PostComment;
                        if(postCommentControl != null)
                        {
                            postCommentControl.CommentApproved += OnCommentPosted;
                        }
                        apnlCommentsWrapper.ContentTemplateContainer.Controls.Add(control);
                        CenterBodyControl.Controls.Add(apnlCommentsWrapper);
                    }
                    else
                    {
                        CenterBodyControl.Controls.Add(control);
                    }
                }
            }

            if(CurrentSkin.HasCustomCssText)
            {
                CustomCss.Attributes.Add("href", Url.CustomCssUrl());
            }
            else
            {
                //MAC IE does not like the empy CSS file, plus its a waste :)
                CustomCss.Visible = false;
            }

            if(Rsd != null)
            {
                Rsd.Attributes.Add("href", Url.RsdUrl(Blog).ToString());
            }

            if(wlwmanifest != null)
            {
                wlwmanifest.Attributes.Add("href", Url.WlwManifestUrl());
            }

            if(RSSLink != null)
            {
                RSSLink.Attributes.Add("href", Url.RssUrl(Blog).ToString());
            }

            // if specified, add script elements
            if(scripts != null)
            {
                scripts.Text = ScriptRenderer.RenderScriptElementCollection(CurrentSkin.SkinKey);
            }

            if(styles != null)
            {
                styles.Text = StyleRenderer.RenderStyleElementCollection(CurrentSkin.SkinKey);
            }

            if(openIDServer != null && !string.IsNullOrEmpty(Blog.OpenIdServer))
            {
                openIDServer.Text = string.Format(OpenIdServerLocation, Blog.OpenIdServer);
            }

            if(openIDDelegate != null && !string.IsNullOrEmpty(Blog.OpenIdDelegate))
            {
                openIDDelegate.Text = string.Format(OpenIdDelegateLocation, Blog.OpenIdDelegate);
            }

            if(metaTags != null)
            {
                metaTags.Blog = Blog;
            }
        }

        void OnCommentPosted(object sender, EventArgs e)
        {
            commentsControl.InvalidateFeedbackCache();
            commentsControl.BindFeedback(true); //don't get it from cache.
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
            if(!String.IsNullOrEmpty(Blog.Author))
            {
                authorMetaTag.Text = String.Format(Environment.NewLine + "<meta name=\"author\" content=\"{0}\" />",
                                                   Blog.Author);
            }
            versionMetaTag.Text =
                String.Format("{0}<meta name=\"Generator\" content=\"{1}\" />{0}", Environment.NewLine, VersionInfo.VersionDisplayText);

            if(!String.IsNullOrEmpty(Blog.TrackingCode))
            {
                customTrackingCode.Text = Blog.TrackingCode;
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// Initializes this blog page, applying skins and whatnot.
        /// </summary>
        /// <param name="e">E.</param>
        override protected void OnInit(EventArgs e)
        {
            MaintainScrollPositionOnPostBack = true;
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
        protected override void SavePageStateToPersistenceMedium(object viewState)
        {
        }
    }
}