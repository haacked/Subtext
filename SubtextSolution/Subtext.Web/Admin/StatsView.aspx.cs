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
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Admin.WebUI;

namespace Subtext.Web.Admin.Pages
{
	/// <summary>
	/// Summary description for StatsViews.
	/// </summary>
	public partial class StatsView : StatsPage
	{
		private bool _isListHidden = false;
		private int _resultsPageNumber = 1;

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

		protected override void BindLocalUI()
		{
			if(AdminMasterPage != null && AdminMasterPage.BreadCrumb != null)
			{	
				string title = "Views";

				AdminMasterPage.BreadCrumb.AddLastItem(title);
				AdminMasterPage.Title = title;
			}

            base.BindLocalUI();
		}

		public void BindList()
		{
            IPagedCollection<ViewStat> stats = Stats.GetPagedViewStats(_resultsPageNumber, this.resultsPager.PageSize, DateTime.Now.AddMonths(-1), DateTime.Now);

			if (stats.Count > 0)
			{
				this.resultsPager.ItemCount = stats.MaxItems;
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
		}
		#endregion


	}
}