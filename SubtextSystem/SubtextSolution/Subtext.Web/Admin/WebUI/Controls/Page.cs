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

// Aped from: Paul Wilson @ www.wilsondotnet.com

using System;
using System.Collections;
using System.ComponentModel;
using System.Configuration;
using System.Web;
using System.Web.UI;
using System.Web.UI.Design;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Web.Admin.WebUI
{
	// TODO: abstract out categories and actions (links) into their own controls. Switch on tab type 
	// add appropriate actions.
	[
		ToolboxData("<{0}:AdminPage runat=server></{0}:AdminPage>"),
		ToolboxItem(typeof(WebControlToolboxItem)),
	Designer(typeof(ReadWriteControlDesigner))
	]
	public class Page : System.Web.UI.HtmlControls.HtmlContainerControl
	{
		private const int NULL_CATEGORY = -1;

		private const string SITEMAP_CONFIG_LOCATION = "~/admin/navigation.config";
		private const string TITLE_PREFIX = "SubText Admin";

		private const string TEMPLATE_DEFAULT = "Admin.DefaultTemplate";
		private const string TEMPLATE_DOWNLEVEL = "Admin.DownlevelTemplate";
		private const string DEFAULT_CONTENT = "Admin.DefaultContent";
		private const string CONTROL_BODY = "AdminSection";
		private const string CONTROL_LOGOUT_LINK = "LogoutLink";
		private const string CONTROL_BLOGTITLE = "BlogTitleLink";
		private const string CONTROL_CATEGORIES_LABEL = "LabelCategories";		
		private const string CONTROL_CATEGORIES = "LinksCategories";
		private const string CONTROL_ACTIONS_LABEL = "LabelActions";
		private const string CONTROL_ACTIONS = "LinksActions";
		private const string CONTROL_TITLE = "PageTitle";
		private const string CONTROL_BREADCRUMBS = "BreadCrumbs";
		
		private const string QRYSTR_CATEGORYFILTER = "catid";

		private string _defaultTemplateFile;
		private string _downlevelTemplateFile;
		private string _defaultContent;

		private Control _template = null;
		private PlaceHolder _defaults = new PlaceHolder();
		private ArrayList _contents = new ArrayList();

		private Control _body;
		private string _title = String.Empty;
		private PlaceHolder _titleControl;
		protected LinkButton _logoutLink;
		private string _tabSectionID;	
		private HyperLink _blogTitle;
		private string _categoriesLabel = "Categories";
		private string _actionsLabel = "Actions";
		private LinkList _categories;
		private LinkList _actions;
		protected int _catType = NULL_CATEGORY;
		private BreadCrumbs _breadcrumbs;

		#region Accessors
		[Category("Page"), Description("Path of Template user control")] 
		public string DefaultTemplateFile 
		{
			get { return _defaultTemplateFile; }
			set { _defaultTemplateFile = value; }
		}

		[Category("Page"), Description("Control ID for default content")] 
		public string DefaultContent 
		{
			get { return _defaultContent; }
			set { _defaultContent = value; }
		}

		[Category("Page"), Description("Page tab section identifier")]
		public string TabSectionID
		{
			get { return _tabSectionID; }
			set { _tabSectionID = value; }
		}

		[Category("Page"), Description("Header label for category links")]
		public string CategoriesLabel
		{
			get { return _categoriesLabel; }
			set { _categoriesLabel = value; }
		}

		public LinkList Categories
		{
			get { return _categories; }
		}

		public string ActionsLabel
		{
			get { return _actionsLabel; }
			set { _actionsLabel = value; }
		}

		public LinkList Actions
		{
			get { return _actions; }
		}

		public CategoryType CategoryType
		{
			get { return (CategoryType)_catType; }
			set { _catType = (int)value; }
		}

		public string Title
		{
			get 
			{
				if (_title.Length > 0)
					return String.Format("{0} : {1}", TITLE_PREFIX, _title);
				else
					return TITLE_PREFIX;
			}
			set { _title = value; }
		}

		public BreadCrumbs BreadCrumbs
		{
			get { return _breadcrumbs; }
		}

		#endregion

		public Page() 
		{
			_defaultTemplateFile = 
				Utilities.AbsolutePath(ConfigurationSettings.AppSettings[TEMPLATE_DEFAULT]);
			_downlevelTemplateFile = 
				Utilities.AbsolutePath(ConfigurationSettings.AppSettings[TEMPLATE_DOWNLEVEL]);
			
			_defaultContent = ConfigurationSettings.AppSettings[DEFAULT_CONTENT];
			if (_defaultContent == null) 
				_defaultContent = "PageContent";
		}

		protected override void RenderBeginTag(System.Web.UI.HtmlTextWriter writer) {}
		protected override void RenderEndTag(System.Web.UI.HtmlTextWriter writer) {}

		protected override void AddParsedSubObject(object obj) 
		{
			if (obj is PlaceHolder) 
			{
				_contents.Add(obj);
			}
			else 
			{
				_defaults.Controls.Add((Control)obj);
			}
		}

		protected override void OnPreRender(EventArgs e)
		{			
			this.BindNavigation();

			base.OnPreRender(e);
		}


		protected override void OnInit(EventArgs e) 
		{
			if (!SiteMap.Instance.IsConfigured)
				SiteMap.LoadConfiguration(Utilities.AbsolutePath(SITEMAP_CONFIG_LOCATION));
			
			this.BuildMasterPage();
			this.BuildContents();
			_logoutLink.Click += new System.EventHandler(this.Logout);

			base.OnInit(e);
		}

		private void BuildMasterPage() 
		{
			string templateFile = _defaultTemplateFile;

			// HACK: The shortcomings of HttpBrowserCapabilities are reasonably well documented.
			// We're going to do two things, one make Mozilla, NN6&7 & Opera uplevel in our 
			// web.config (thus overriding the omissions in machine.config). Then we'll peek in at 
			// css2 and see if we support that in the current browser. The machine.config overrides
			// and the base IE values will let us get this right for most if not all the major
			// browsers. Otherwise we default to downlevel (we're basically no worse off than 
			// with a default machine.config anyway, I suppose).
			if (!Convert.ToBoolean(Page.Request.Browser["css2"]))
				templateFile = _downlevelTemplateFile;

			if (templateFile.Length <= 0)
				throw new Exception("TemplateFile properties for Admin.Page must be defined in your web.config file.");
			
			_template = this.Page.LoadControl(templateFile);
			_template.ID = this.ID + "_Template";

			Control body = _template.FindControl(CONTROL_BODY);
			if (null != body) 
				_body = body;

			// REFACTOR: abstract next three
			Control blogTitle = _template.FindControl(CONTROL_BLOGTITLE);
			if (blogTitle == null || !(blogTitle is HyperLink)) 
				throw new Exception("Cannot find template control: " + CONTROL_BLOGTITLE);
			else
				_blogTitle = (HyperLink)blogTitle;

			Control categories = _template.FindControl(CONTROL_CATEGORIES);
			if (categories == null || !(categories is LinkList)) 
			{
				//throw new Exception("Cannot find template control: " + CONTROL_CATEGORIES);
			}
			else
				_categories = (LinkList)categories;

			Control actions = _template.FindControl(CONTROL_ACTIONS);
			if (actions == null || !(actions is LinkList)) 
				throw new Exception("Cannot find template control: " + CONTROL_ACTIONS);
			else
				_actions = (LinkList)actions;		

			Control logoutLink = _template.FindControl(CONTROL_LOGOUT_LINK);
			if (logoutLink == null || !(logoutLink is LinkButton)) 
				throw new Exception("Cannot find template control: " + CONTROL_LOGOUT_LINK);
			else
				_logoutLink = (LinkButton)logoutLink;	

			Control categoriesLabel = _template.FindControl(CONTROL_CATEGORIES_LABEL);
			if (null != categoriesLabel && (categoriesLabel is PlaceHolder)) 
			{
				categoriesLabel.Controls.Clear();
				categoriesLabel.Controls.Add(new LiteralControl(_categoriesLabel));
			}

			Control actionsLabel = _template.FindControl(CONTROL_ACTIONS_LABEL);
			if (null != actionsLabel && (actionsLabel is PlaceHolder)) 
			{
				actionsLabel.Controls.Clear();
				actionsLabel.Controls.Add(new LiteralControl(_actionsLabel));
			}

			Control title = _template.FindControl(CONTROL_TITLE);
			if (null != title && (title is PlaceHolder)) 
			{
				_titleControl = (PlaceHolder)title;
			}

			Control breadcrumbs = _template.FindControl(CONTROL_BREADCRUMBS);
			if (null != breadcrumbs && (breadcrumbs is BreadCrumbs)) 
			{
				_breadcrumbs = (BreadCrumbs)breadcrumbs;
			}

			int count = _template.Controls.Count;
			for (int index = 0; index < count; index++) 
			{
				Control control = _template.Controls[0];
				_template.Controls.Remove(control);
				if (control.Visible) 
				{
					this.Controls.Add(control);
				}
			}
			this.Controls.AddAt(0, _template);
		}

		private void BuildContents() 
		{
			if (_defaults.HasControls()) 
			{
				_defaults.ID = _defaultContent;
				_contents.Add(_defaults);
			}

			foreach (Subtext.Web.Admin.WebUI.PlaceHolder content in _contents) 
			{
				Control region = this.FindControl(content.ID);
				if (region == null || !(region is PlaceHolder)) 
				{
					throw new Exception("ContentRegion with ID '" + content.ID + "' must be Defined");
				}
				region.Controls.Clear();
				
				int count = content.Controls.Count;
				for (int index = 0; index < count; index++) 
				{
					Control control = content.Controls[0];
					content.Controls.Remove(control);
					region.Controls.Add(control);
				}
			}
		}

		public void BindNavigation()
		{
			// REFACTOR: oof. need to pull this out and put it at the (right) page level.

			

			if (NULL_CATEGORY != _catType)
			{	
				string linkUrl = null;

				// add a unadorned link back to this same page to give an explicit way to clear filter
				if (CategoryType.ImageCollection == (CategoryType)_catType)
				{
					linkUrl = "EditGalleries.aspx";
					_categories.Items.Add("All Galleries", linkUrl);
				}

				else
				{
					linkUrl = Page.Request.Url.LocalPath;
					if(_categories != null)
						_categories.Items.Add("All Categories", linkUrl);
				}

				// get all the categories for this type, and then add filtering links for each
				LinkCategoryCollection cats = Links.GetCategories((CategoryType)_catType,false);
				foreach (LinkCategory current in cats)
				{
					if(_categories != null)
					{
						_categories.Items.Add(current.Title, String.Format("{0}?{1}={2}",
							linkUrl, QRYSTR_CATEGORYFILTER, current.CategoryID));
					}
				}
			}			

			_body.ID = _tabSectionID;
			_blogTitle.Target = String.Empty;
			_blogTitle.NavigateUrl = Config.CurrentBlog.FullyQualifiedUrl;
			_blogTitle.Text = Config.CurrentBlog.Title;
		}

		public void AddToActions(LinkButton lkb)
		{
			// HACK: one without the other doesn't seem to work. If I don't add this
			// to Items it doesn't render, if I don't add to controls it doesn't get
			// wired up. 
			_actions.Items.Add(lkb);
			_actions.Controls.Add(lkb);	
		}

		public void AddToActions(HyperLink lnk)
		{
			_actions.Items.Add(lnk);
		}

		protected void SetTitle()
		{
			if (null != _titleControl)
			{
				_titleControl.Controls.Clear();
				_titleControl.Controls.Add(new LiteralControl(Title));
			}
		}

		protected void Logout(object sender, System.EventArgs e)
		{
			HttpContext.Current.Response.Cookies.Clear();
			if(HttpContext.Current.Session != null)
			{
				HttpContext.Current.Session.Abandon();
			}
			System.Web.Security.FormsAuthentication.SignOut();
			Context.Response.Redirect(Config.CurrentBlog.FullyQualifiedUrl);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			SetTitle();
			base.Render (writer);
		}

	}
}

