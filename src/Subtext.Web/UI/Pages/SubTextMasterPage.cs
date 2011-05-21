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
using System.Linq;
using System.Text;
using System.Web.UI;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Routing;
using Subtext.Framework.Services.SearchEngine;
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
    public partial class SubtextMasterPage : SubtextPage, IPageWithControls, IContainerControl
    {
        protected const string ControlLocation = "~/Skins/{0}/Controls/{1}";
        protected const string OpenIdDelegateLocation = "<link rel=\"openid.delegate\" href=\"{0}\" />";
        protected const string OpenIdServerLocation = "<link rel=\"openid.server\" href=\"{0}\" />";
        protected const string TemplateLocation = "~/Skins/{0}/{1}";

        public static readonly string CommentsPanelId = "commentsUpdatePanelWrapper";

        private static readonly ScriptElementCollectionRenderer ScriptRenderer = new ScriptElementCollectionRenderer(new SkinEngine());

        private static readonly StyleSheetElementCollectionRenderer StyleRenderer =
            new StyleSheetElementCollectionRenderer(new SkinEngine());

        IEnumerable<string> _controls;

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
                    declaration += string.Format(CultureInfo.InvariantCulture, "'{0}', ", tagname);
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

        public void InitializeControls(ISkinControlLoader controlLoader)
        {
            IEnumerable<string> controlNames = _controls;
            if (controlNames != null)
            {
                var apnlCommentsWrapper = new UpdatePanel { Visible = true, ID = CommentsPanelId };
                if (!controlNames.Contains("HomePage", StringComparer.OrdinalIgnoreCase) && !String.IsNullOrEmpty(Query))
                {
                    int entryId = -1;
                    Entry entry = Cacher.GetEntryFromRequest(true, SubtextContext);
                    if (entry != null)
                    {
                        entryId = entry.Id;
                    }
                    var query = Query;
                    if (!String.IsNullOrEmpty(query))
                    {
                        var searchResults = SearchEngineService.Search(query, 5, Blog.Id, entryId);
                        if (searchResults.Any())
                        {
                            AddMoreResultsControl(searchResults, controlLoader, apnlCommentsWrapper);
                        }
                    }
                }

                foreach (string controlName in controlNames)
                {
                    Control control = controlLoader.LoadControl(controlName);
                    AddControlToBody(controlName, control, apnlCommentsWrapper, CenterBodyControl);
                }
            }
        }

        private void AddMoreResultsControl(IEnumerable<SearchEngineResult> searchResults, ISkinControlLoader controlLoader, UpdatePanel apnlCommentsWrapper)
        {
            var moreResults = controlLoader.LoadControl("MoreResults");
            if (moreResults != null)
            {
                var moreSearchResults = moreResults as MoreResultsLikeThis;
                if (moreSearchResults != null)
                {
                    moreSearchResults.SearchResults = searchResults;
                }
                AddControlToBody("MoreResults", moreResults, apnlCommentsWrapper, CenterBodyControl);
            }
        }

        public void AddControlToBody(string controlName, Control control, UpdatePanel apnlCommentsWrapper, Control centerBodyControl)
        {
            if (controlName.Equals("Comments", StringComparison.OrdinalIgnoreCase))
            {
                control.Visible = true;
                commentsControl = control as Comments;
                apnlCommentsWrapper.ContentTemplateContainer.Controls.Add(control);
            }
            else if (controlName.Equals("PostComment", StringComparison.OrdinalIgnoreCase))
            {
                postCommentControl = control as PostComment;
                if (postCommentControl != null)
                {
                    postCommentControl.CommentApproved += OnCommentPosted;
                }
                apnlCommentsWrapper.ContentTemplateContainer.Controls.Add(control);
                centerBodyControl.Controls.Add(apnlCommentsWrapper);
            }
            else
            {
                if (centerBodyControl != null)
                {
                    centerBodyControl.Controls.Add(control);
                }
            }
        }

        public string Query
        {
            get
            {
                if (_query == null)
                {
                    var request = SubtextContext.HttpContext.Request;
                    var referrer = request.UrlReferrer;
                    if (referrer == null)
                    {
                        if (request.IsLocal)
                        {
                            string referrerInQuery = request.QueryString["referrer"];
                            if (!String.IsNullOrEmpty(referrerInQuery))
                            {
                                Uri.TryCreate(referrerInQuery, UriKind.Absolute, out referrer);
                            }
                        }
                    }
                    if (referrer == null)
                    {
                        return null;
                    }
                    _query = BlogUrlHelper.ExtractKeywordsFromReferrer(referrer, request.Url);
                }
                return _query;
            }
        }
        private string _query;


        public void InitializeBlogPage()
        {
            var skin = SkinConfig.GetCurrentSkin(Blog, SubtextContext.HttpContext);
            var skinControlLoader = new SkinControlLoader(this, skin);

            InitializeControls(skinControlLoader);

            if (skin.HasCustomCssText)
            {
                CustomCss.Attributes.Add("href", Url.CustomCssUrl());
            }
            else
            {
                //MAC IE does not like the empy CSS file, plus its a waste :)
                CustomCss.Visible = false;
            }

            if (Rsd != null)
            {
                Rsd.Attributes.Add("href", Url.RsdUrl(Blog).ToString());
            }

            if (wlwmanifest != null)
            {
                wlwmanifest.Attributes.Add("href", Url.WlwManifestUrl());
            }

            if (opensearch != null)
            {
                opensearch.Attributes.Add("href", Url.OpenSearchDescriptorUrl());
                opensearch.Attributes.Add("Title", Blog.Title);
            }

            if (RSSLink != null)
            {
                RSSLink.Attributes.Add("href", Url.RssUrl(Blog).ToString());
            }

            // if specified, add script elements
            if (scripts != null)
            {
                scripts.Text = ScriptRenderer.RenderScriptElementCollection(skin.SkinKey);
            }

            if (styles != null)
            {
                styles.Text = StyleRenderer.RenderStyleElementCollection(skin.SkinKey);
            }

            if (openIDServer != null && !string.IsNullOrEmpty(Blog.OpenIdServer))
            {
                openIDServer.Text = string.Format(OpenIdServerLocation, Blog.OpenIdServer);
            }

            if (openIDDelegate != null && !string.IsNullOrEmpty(Blog.OpenIdDelegate))
            {
                openIDDelegate.Text = string.Format(OpenIdDelegateLocation, Blog.OpenIdDelegate);
            }

            if (metaTags != null)
            {
                metaTags.Blog = Blog;
            }
        }

        void OnCommentPosted(object sender, EventArgs e)
        {
            if (commentsControl != null)
            {
                commentsControl.InvalidateFeedbackCache();
                commentsControl.BindFeedback(true); //don't get it from cache.
            }
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
            if (!String.IsNullOrEmpty(Blog.Author))
            {
                authorMetaTag.Text = String.Format(Environment.NewLine + "<meta name=\"author\" content=\"{0}\" />",
                                                   Blog.Author);
            }
            versionMetaTag.Text =
                String.Format("{0}<meta name=\"Generator\" content=\"{1}\" />{0}", Environment.NewLine, VersionInfo.VersionDisplayText);

            if (!String.IsNullOrEmpty(Blog.TrackingCode))
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