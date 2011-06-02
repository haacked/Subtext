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
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Web;

//Adapted from AdvancedPanel

namespace Subtext.Web.Controls
{
    // TODO: Get rid of this.
    /// <summary>
    /// Various styles of collapsing this panel.
    /// </summary>
    public enum CollapseLinkStyle
    {
        Text,
        Image,
        ImageBeforeText,
        ImageAfterText
    }

    /// <summary>
    /// Panel that can be expanded and collapsed.
    /// </summary>
    [ToolboxData("<{0}:CollapsiblePanel runat=\"server\"></{0}:CollapsiblePanel>")]
    public class CollapsiblePanel : Panel, INamingContainer
    {
        const string ClientScriptKey = "Subtext.Web.Controls.CollapsiblePanel";

        private const string CtlidContents = "Contents";
        private const string CtlidHeader = "Header";
        private const string CtlidLink = "Link";
        private const string CtlidLinkImage = "LinkImage";
        private const string VskeyCollapsed = "Collapsed";
        private const string VskeyCollapsible = "Collapsible";

        protected string _bodyCssClass;
        protected HyperLink _collapseLink;
        protected Panel _contents;
        protected bool _displayHeader;
        protected Panel _header;
        protected string _headerCssClass;
        protected string _headerText = "Caption";
        protected string _headerTextCssClass;
        private Image _image;
        protected bool _linkBeforeHeader;
        protected string _linkCssClass;
        protected string _linkImage;
        protected string _linkImageCollapsed;
        protected CollapseLinkStyle _linkStyle = CollapseLinkStyle.Text;
        protected string _linkText = "more";
        protected string _linkTextCollapsed = "less";
        Label headerLabel;

        #region Accessors

        /// <summary>
        /// Gets or sets the link style.
        /// </summary>
        /// <value></value>
        public CollapseLinkStyle LinkStyle
        {
            get { return _linkStyle; }
            set { _linkStyle = value; }
        }

        /// <summary>
        /// Gets or sets the link text displayed when collapsed.
        /// </summary>
        /// <value></value>
        public string LinkTextCollapsed
        {
            get { return _linkTextCollapsed; }
            set { _linkTextCollapsed = value; }
        }

        /// <summary>
        /// Gets or sets the body CSS class.
        /// </summary>
        /// <value></value>
        public string BodyCssClass
        {
            get { return _bodyCssClass; }
            set { _bodyCssClass = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CollapsiblePanel"/> is collapsed.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if collapsed; otherwise, <c>false</c>.
        /// </value>
        public bool Collapsed
        {
            get { return (bool)(ViewState[VskeyCollapsed] ?? false); }
            set { ViewState[VskeyCollapsed] = value; }
        }

        public bool WillSucceed
        {
            get { return (bool)(ViewState["WillSucceed"] ?? false); }
            set { ViewState["WillSucceed"] = value; }
        }


        public override ControlCollection Controls
        {
            get
            {
                EnsureChildControls();
                return base.Controls;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display the header.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if header is to be displayed; otherwise, <c>false</c>.
        /// </value>
        public bool DisplayHeader
        {
            get { return _displayHeader; }
            set
            {
                _displayHeader = value;
                if (!value)
                {
                    Collapsible = false;
                }
            }
        }

        /// <summary>
        /// Gets or sets the header CSS class.
        /// </summary>
        /// <value></value>
        public string HeaderCssClass
        {
            get { return _headerCssClass; }
            set { _headerCssClass = value; }
        }

        /// <summary>
        /// Gets or sets the header text.
        /// </summary>
        /// <value></value>
        public string HeaderText
        {
            get { return _headerText; }
            set
            {
                if (headerLabel != null)
                {
                    headerLabel.Text = value;
                }
                _headerText = value;
            }
        }

        /// <summary>
        /// Gets or sets the header text CSS class.
        /// </summary>
        /// <value></value>
        public string HeaderTextCssClass
        {
            get { return _headerTextCssClass; }
            set { _headerTextCssClass = value; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="CollapsiblePanel"/> is collapsible.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if collapsible; otherwise, <c>false</c>.
        /// </value>
        public bool Collapsible
        {
            get { return (bool)(ViewState[VskeyCollapsible] ?? false); }
            set
            {
                if (value)
                {
                    _displayHeader = true;
                }
                ViewState[VskeyCollapsible] = value;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the link goes before the header.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if the link goes before the header; otherwise, <c>false</c>.
        /// </value>
        public bool LinkBeforeHeader
        {
            get { return _linkBeforeHeader; }
            set { _linkBeforeHeader = value; }
        }

        /// <summary>
        /// Gets or sets the link CSS class.
        /// </summary>
        /// <value></value>
        public string LinkCssClass
        {
            get { return _linkCssClass; }
            set { _linkCssClass = value; }
        }

        /// <summary>
        /// Gets or sets the link image.
        /// </summary>
        /// <value></value>
        [
            Description("Collapse link image URL")
        ]
        [Bindable(true)]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string LinkImage
        {
            get { return _linkImage; }
            set { _linkImage = value; }
        }

        [
            Description("Collapse link image URL")
        ]
        [Bindable(true)]
        [Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(UITypeEditor))]
        public string LinkImageCollapsed
        {
            get { return _linkImageCollapsed; }
            set { _linkImageCollapsed = value; }
        }

        public string LinkText
        {
            get { return _linkText; }
            set { _linkText = value; }
        }

        #endregion

        protected override void OnInit(EventArgs e)
        {
            RegisterClientScript();
            RebuildControls();
            base.OnInit(e);
        }

        void RegisterClientScript()
        {
            if (Page.ClientScript.IsClientScriptBlockRegistered(ClientScriptKey))
            {
                Type cstype = GetType();
                Page.ClientScript.RegisterClientScriptBlock(cstype, ClientScriptKey,
                                                            ScriptHelper.UnpackScript("CollapsiblePanel.js"));
            }
        }

        /// <summary>
        /// Registers the client script if this is collapsible.
        /// </summary>
        /// <param name="e"></param>
        protected override void OnPreRender(EventArgs e)
        {
            if (Collapsible)
            {
                RegisterClientScript();
            }

            base.OnPreRender(e);
        }

        protected virtual void RebuildControls()
        {
            if (_displayHeader)
            {
                _contents = new Panel { ID = CtlidContents };
                _contents = (Panel)SetCssClass(_contents, _bodyCssClass);

                while (Controls.Count > 0)
                {
                    if (Controls[0] is IValidator)
                    {
                        Page.Validators.Add((IValidator)Controls[0]);
                    }

                    _contents.Controls.Add(Controls[0]);
                }
                Controls.Clear();

                _header = new Panel { ID = CtlidHeader };
                SetCssClass(_header, _headerCssClass);

                headerLabel = new Label { ID = "headerDisplayLabel", Text = HeaderText };
                headerLabel = (Label)SetCssClass(headerLabel, _headerTextCssClass);

                _collapseLink = CreateCollapseLink();

                if (_linkBeforeHeader)
                {
                    _header.Controls.Add(_collapseLink);
                    _header.Controls.Add(headerLabel);
                }
                else
                {
                    _header.Controls.Add(headerLabel);
                    _header.Controls.Add(_collapseLink);
                }

                Controls.Add(_header);
                Controls.Add(_contents);
            }
            else
            {
                if (_bodyCssClass != null)
                {
                    CssClass = _bodyCssClass;
                }
            }
        }

        protected override void Render(HtmlTextWriter writer)
        {
            if (null != Page)
            {
                Page.VerifyRenderingInServerForm(this);
            }

            if (null != _collapseLink)
            {
                _collapseLink.Visible = Collapsible;
            }

            if (_displayHeader && Collapsible)
            {
                _collapseLink.Attributes.Add("onclick",
                                             String.Format(CultureInfo.InvariantCulture,
                                                           "ToggleVisible('{0}','{1}','{2}','{3}'); return false;",
                                                           _contents.ClientID,
                                                           _image != null ? _image.ClientID : String.Empty,
                                                           HttpHelper.ExpandTildePath(LinkImage),
                                                           HttpHelper.ExpandTildePath(LinkImageCollapsed)));

                if (Collapsed)
                {
                    _contents.Style.Add("display", "none");
                    _image.ImageUrl = LinkImageCollapsed;
                }
            }

            base.Render(writer);
        }

        protected virtual HyperLink CreateCollapseLink()
        {
            var createdLink = new HyperLink { ID = CtlidLink };
            createdLink = (HyperLink)SetCssClass(createdLink, _linkCssClass);
            createdLink.NavigateUrl = "#";

            switch (LinkStyle)
            {
                case CollapseLinkStyle.Text:
                    createdLink.Text = _linkText;
                    break;

                case CollapseLinkStyle.Image:
                    _image = CreateCollapseImage();
                    createdLink.Controls.Add(_image);
                    break;

                case CollapseLinkStyle.ImageAfterText:
                    createdLink.Controls.Add(new LiteralControl(_linkText));
                    _image = CreateCollapseImage();
                    createdLink.Controls.Add(_image);
                    break;

                case CollapseLinkStyle.ImageBeforeText:
                    _image = CreateCollapseImage();
                    createdLink.Controls.Add(_image);
                    createdLink.Controls.Add(new LiteralControl(_linkText));
                    break;

                default:
                    createdLink.Text = _linkText;
                    break;
            }

            return createdLink;
        }

        protected virtual Image CreateCollapseImage()
        {
            var img = new Image();
            // HACK: img.ImageUrl was tinkering with the actual location, so it would vary from what
            // was also being used for the js flip parameters. This is suboptimal, but consistent.
            img.Attributes.Add("src", HttpHelper.ExpandTildePath(LinkImageCollapsed));
            img.ID = CtlidLinkImage;

            return img;
        }

        protected virtual WebControl SetCssClass(WebControl webControl, string css)
        {
            if (!string.IsNullOrEmpty(css))
            {
                webControl.CssClass = css;
            }

            return webControl;
        }
    }
}