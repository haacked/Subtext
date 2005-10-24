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

using System;
using System.Web.UI.WebControls;
using Subtext.Framework.Data;
using Subtext.Framework.Logging;

namespace Subtext.Web.Admin.Pages
{
	public class ErrorLog : AdminPage
	{
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Log;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
		protected System.Web.UI.WebControls.Repeater LogPage;
		protected System.Web.UI.HtmlControls.HtmlGenericControl NoMessagesLabel;
		protected Subtext.Web.Admin.WebUI.Pager LogPager;
		protected System.Web.UI.WebControls.LinkButton btnClearLog;

		private int _logPageNumber;
	
		protected override void OnLoad(EventArgs e)
		{
			LoadPage();
			base.OnLoad (e);
		}


		private void LoadPage()
		{
			if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
				_logPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);

			LogPager.PageSize = Preferences.ListingItemCount;
			LogPager.PageIndex = _logPageNumber;

			BindLocalUI();
			BindList();
		}

		private void BindList()
		{
			PagedLogEntryCollection logEntries = LoggingProvider.Instance().GetPagedLogEntries(LogPager.PageIndex, LogPager.PageSize, SortDirection.Descending);
			if (logEntries.Count > 0)
			{				
				LogPager.ItemCount = logEntries.MaxItems;
				LogPage.DataSource = logEntries;
				LogPage.DataBind();
			}			
		}

		
		private void BindLocalUI()
		{
			HyperLink lnkReferrals = Utilities.CreateHyperLink("Referrals", "Referrers.aspx");
			HyperLink lnkViews		= Utilities.CreateHyperLink("Views", "StatsView.aspx");
			HyperLink lnkErrorLog	= Utilities.CreateHyperLink("Error Log", "ErrorLog.aspx");


			// Add the buttons to the PageContainer.
			PageContainer.AddToActions(lnkReferrals);
			PageContainer.AddToActions(lnkViews);
			PageContainer.AddToActions(lnkErrorLog);

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
			this.btnClearLog.Click += new System.EventHandler(this.btnClearLog_Click);
		}
		#endregion

		private void btnClearLog_Click(object sender, System.EventArgs e)
		{
			LoggingProvider.Instance().ClearLog();
			BindList();
		}
	}
}

