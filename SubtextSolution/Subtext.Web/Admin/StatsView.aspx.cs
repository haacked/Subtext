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
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Summary description for StatsViews.
	/// </summary>
	public class StatsView : AdminPage
	{
		private bool _isListHidden = false;
		private int _resultsPageNumber = 1;
		protected System.Web.UI.WebControls.Repeater rprSelectionList;
		protected Subtext.Web.Admin.WebUI.Pager ResultsPager;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Results;
		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;

		private void Page_Load(object sender, System.EventArgs e)
		{	
			//TODO: implement "blog_GetPageableViewStats"

//			if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
//				_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
//
//			ResultsPager.PageSize = Preferences.ListingItemCount;
//			ResultsPager.PageIndex = _resultsPageNumber;
//			Results.Collapsible = false;
//
//			BindLocalUI();
//			BindList();
		}

		public string CheckHiddenStyle()
		{
			if (_isListHidden)
				return Constants.CSSSTYLE_HIDDEN;
			else
				return String.Empty;
		}


		public string GetPageTitle(object dataItem)
		{
			ViewStat stat = (ViewStat) dataItem;
			string pageTitle = "Unknown";


			switch (stat.PageType)
			{
				case PageType.HomePage:
					pageTitle = "Home Page";
					break;
				case PageType.ImagePage:
					pageTitle = "Your Gallery";
					break;
				case PageType.RSS:
					pageTitle = "RSS";
					break;
				case PageType.Post:
				case PageType.Story:
					pageTitle = stat.PageTitle;
					break;
				case PageType.Other:
					pageTitle = "Other";
					break;
				case PageType.Date:
					pageTitle = "Date";
					break;
			}

			return pageTitle;

		}

		public void BindLocalUI()
		{
			// Setup the LH navigation.
			HyperLink lnkReferrals = Utilities.CreateHyperLink("Referrals", "Referrers.aspx");
			HyperLink lnkViews		= Utilities.CreateHyperLink("Views", "StatsView.aspx");
			HyperLink lnkErrorLog	= Utilities.CreateHyperLink("Error Log", "ErrorLog.aspx");


			// Add the buttons to the PageContainer.
			PageContainer.AddToActions(lnkReferrals);
			PageContainer.AddToActions(lnkViews);
			PageContainer.AddToActions(lnkErrorLog);

			// Attempt to customize the breadcrumb.
			Control container = Page.FindControl("PageContainer");
			if (null != container && container is Subtext.Web.Admin.WebUI.Page)
			{	
				Subtext.Web.Admin.WebUI.Page page = (Subtext.Web.Admin.WebUI.Page) container;
				string title = "Views";

				page.BreadCrumbs.AddLastItem(title);
				page.Title = title;
			}

		}

		public void BindList()
		{
            IPagedCollection<ViewStat> stats = Stats.GetPagedViewStats(_resultsPageNumber, ResultsPager.PageSize, DateTime.Now.AddMonths(-1), DateTime.Now);

			if (stats.Count > 0)
			{
				ResultsPager.ItemCount = stats.MaxItems;
				rprSelectionList.DataSource = stats;
				rprSelectionList.DataBind();
			}
		}


		#region Web Form Designer generated code
		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}
		
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{    
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion


	}
}