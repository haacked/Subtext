#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

#define DebugJavaScript

using System;
using System.ComponentModel;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.WebUI
{
	// TODO: Designer, design-time enhancements
	// TODO: Collapsible property, don't add link to ctls if false -- interaction with Collapsed tests?
	// TODO: Properties should not have dependencies on each other; Setting one property should not 
	// affect other properties

	public enum CollapseLinkStyle
	{
		Text,
		Image,
		ImageBeforeText,
		ImageAfterText
	}

	[ToolboxData("<{0}:AdvancedPanel runat=\"server\"></{0}:AdvancedPanel>")]
	public class AdvancedPanel : Panel, INamingContainer
	{	
		#region Member Fields
		protected const string VSKEY_COLLAPSED = "Collapsed";
		protected const string VSKEY_COLLAPSIBLE = "Collapsible";

#if !DebugJavaScript
		protected const string CLIENT_SCRIPT_FILE = "/aspnet_client/xl8_web_controls/uicontrols.js";
#else
		protected string CLIENT_SCRIPT_FILE = Utilities.AbsolutePath("~/admin/resources/uicontrols.js");
#endif
		protected const string CLIENT_SCRIPT_KEY = "uicontrols";
		protected const string CTLID_CONTENTS = "Contents";
		protected const string CTLID_HEADER = "Header";
		protected const string CTLID_LINK = "Link";
		protected const string CTLID_LINK_IMAGE = "LinkImage";
		
		protected string _bodyCssClass;
		protected HyperLink _collapseLink;
		protected Panel _contents;
		protected bool _displayHeader;
		protected Panel _header;
		protected string _headerCssClass;
		protected string _headerText = "Caption";
		protected string _headerTextCssClass;
		protected bool _linkBeforeHeader;
		protected CollapseLinkStyle _linkStyle = CollapseLinkStyle.Text;
		protected string _linkText = "more";
		protected string _linkTextCollapsed = "less";
		protected string _linkCssClass;
		protected string _linkImage;
		protected string _linkImageCollapsed;
		private System.Web.UI.WebControls.Image _image;

		#endregion

		public AdvancedPanel()
		{
			ViewState[VSKEY_COLLAPSED] = false;
			ViewState[VSKEY_COLLAPSIBLE] = false;
		}

		#region Accessors		
		public CollapseLinkStyle LinkStyle
		{
			get { return _linkStyle; }
			set { _linkStyle = value; }
		}

		public string LinkTextCollapsed
		{
			get { return _linkTextCollapsed; }
			set { _linkTextCollapsed = value; }
		}

		public string BodyCssClass
		{
			get { return _bodyCssClass; }
			set { _bodyCssClass = value; }
		}

		public bool Collapsed
		{
			get { return (bool)ViewState[VSKEY_COLLAPSED]; }
			set { ViewState[VSKEY_COLLAPSED] = value; }
		}		

		public override ControlCollection Controls
		{
			get
			{
				EnsureChildControls();
				return base.Controls;
			}
		}

		public bool DisplayHeader
		{
			get { return _displayHeader; }
			set 
			{ 
				_displayHeader = value; 
				if (!value) Collapsible = false;
			}
		}

		public string HeaderCssClass
		{
			get { return _headerCssClass; }
			set { _headerCssClass = value; }
		}

		public string HeaderText
		{
			get { return _headerText; }
			set { _headerText = value; }
		}

		public string HeaderTextCssClass
		{
			get { return _headerTextCssClass; }
			set { _headerTextCssClass = value; }
		}

		public bool Collapsible
		{
			get { return (bool)ViewState[VSKEY_COLLAPSIBLE]; }
			set 
			{ 
				if (value) _displayHeader = true;
				ViewState[VSKEY_COLLAPSIBLE] = value;
			}
		}

		public bool LinkBeforeHeader
		{
			get { return _linkBeforeHeader; }
			set { _linkBeforeHeader = value; }
		}

		public string LinkCssClass
		{
			get { return _linkCssClass; }
			set { _linkCssClass = value; }
		}

		[
			Description("Collapse link image URL"), Bindable(true),
		Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))
		]
		public string LinkImage
		{
			get
			{
				if(_linkImage == null)
				{
					_linkImage = "~/admin/resources/toggle_gray_up.gif";
				}
				return _linkImage;
			}
			set { _linkImage = value; }
		}
		

		[
		DescriptionAttribute("Collapse link image URL"), Bindable(true),
		Editor("System.Web.UI.Design.ImageUrlEditor, System.Design", typeof(System.Drawing.Design.UITypeEditor))
		]
		public string LinkImageCollapsed
		{
			get
			{
				if(_linkImageCollapsed == null)
				{
					_linkImageCollapsed = "~/admin/resources/toggle_gray_down.gif";
				}
				return _linkImageCollapsed;
			}
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
			RebuildControls();
			base.OnInit(e);
		}

		protected override void OnPreRender(EventArgs e)
		{						
			if (Collapsible)
			{
				Page.RegisterClientScriptBlock(CLIENT_SCRIPT_KEY, 
					Utilities.GetClientScriptInclude(CLIENT_SCRIPT_FILE));
			}

			base.OnPreRender(e);
		}

		protected virtual void RebuildControls()
		{
			if (_displayHeader)
			{
			
				_contents = new Panel();
				_contents.ID = CTLID_CONTENTS;
				_contents = (Panel)SetCssClass(_contents, _bodyCssClass);

				while(this.Controls.Count > 0)
				{
					if (this.Controls[0] is IValidator)
						Page.Validators.Add((IValidator)this.Controls[0]);

					_contents.Controls.Add(this.Controls[0]);
					
				}
				this.Controls.Clear();

				_header = new Panel();
				_header.ID = CTLID_HEADER;
				SetCssClass(_header, _headerCssClass);

				Label headerLabel = new Label();
				headerLabel.Text = HeaderText;
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
				CssClass = _bodyCssClass;
			}
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (null != Page) 
				Page.VerifyRenderingInServerForm(this);

			if (null != _collapseLink)
				_collapseLink.Visible = Collapsible;

			if (_displayHeader && Collapsible)
			{
				_collapseLink.Attributes.Add("onclick", 
					String.Format("ToggleVisible('{0}','{1}','{2}','{3}'); return false;", _contents.ClientID, 
					_image != null ? _image.ClientID : String.Empty, Utilities.AbsolutePath(LinkImage), 
					Utilities.AbsolutePath(LinkImageCollapsed)));

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
			HyperLink createdLink = new HyperLink();
			createdLink.ID = CTLID_LINK;
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

		protected virtual System.Web.UI.WebControls.Image CreateCollapseImage()
		{
			System.Web.UI.WebControls.Image img = new System.Web.UI.WebControls.Image();
			// HACK: img.ImageUrl was tinkering with the actual location, so it would vary from what
			// was also being used for the js flip parameters. This is suboptimal, but consistent.
			img.Attributes.Add("src", Utilities.AbsolutePath(LinkImageCollapsed));
			img.ID = CTLID_LINK_IMAGE;

			return img;
		}

		protected virtual WebControl SetCssClass(WebControl ctl, string css)
		{
			if (null != css && css.Length > 0)
				ctl.CssClass = css;

			return ctl;
		}
	}
}

