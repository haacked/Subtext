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
using System.Globalization;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Text;
using Subtext.Framework.Util;

namespace Subtext.Web.Admin.Pages
{
	public class Referrers : AdminPage
	{
		private int _resultsPageNumber = 1;
		private bool _isListHidden = false;

		private int _entryID = NullValue.NullInt32;

		protected System.Web.UI.WebControls.Repeater rprSelectionList;
		protected Subtext.Web.Admin.WebUI.Pager ResultsPager;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Results;
		protected Subtext.Web.Admin.WebUI.MessagePanel Messages;
		protected System.Web.UI.WebControls.RequiredFieldValidator RequiredFieldValidator1;
		protected System.Web.UI.WebControls.TextBox txbTitle;
		protected System.Web.UI.WebControls.RequiredFieldValidator Requiredfieldvalidator3;
		protected System.Web.UI.WebControls.TextBox txbUrl;
		protected System.Web.UI.WebControls.TextBox txbBody;
		protected System.Web.UI.WebControls.LinkButton lkbPost;
		protected System.Web.UI.WebControls.LinkButton lkbCancel;
		protected Subtext.Web.Admin.WebUI.AdvancedPanel Edit;
		protected Subtext.Web.Admin.WebUI.Page PageContainer;
	
		private void Page_Load(object sender, System.EventArgs e)
		{

			if(!IsPostBack)
			{
				if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
				{
					_resultsPageNumber = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
				}

				if(null != Request.QueryString["EntryID"])
				{
					_entryID = Convert.ToInt32(Request.QueryString["EntryID"]);
				}

				ResultsPager.PageSize = Preferences.ListingItemCount;
				ResultsPager.PageIndex = _resultsPageNumber;
				Results.Collapsible = false;


				// TODO: Implement category filtering.
				//PageContainer.CategoryType = CategoryType.PostCollection;


				BindList();
			}
			BindLocalUI();
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

//			Control container = Page.FindControl("PageContainer");
//			if (null != container && container is Subtext.Web.Admin.WebUI.Page)
//			{	
//				Subtext.Web.Admin.WebUI.Page page = (Subtext.Web.Admin.WebUI.Page) container;
//				page.BreadCrumbs.AddLastItem("Referrers");
//			}

			if(_entryID == NullValue.NullInt32)
			{

				//SetReferalDesc("Referrals");
			}
			else
			{
				SetReferalDesc("Entry", _entryID.ToString(CultureInfo.InvariantCulture));
			}

		}

		private void BindList()
		{
			PagedReferrerCollection referrers = null;

			if(_entryID == NullValue.NullInt32)
			{

					referrers = Stats.GetPagedReferrers(_resultsPageNumber, ResultsPager.PageSize);
			}
			else
			{
				ResultsPager.UrlFormat += string.Format(System.Globalization.CultureInfo.InvariantCulture, "&{0}={1}", "EntryID", 
					_entryID);
				referrers = Stats.GetPagedReferrers(_resultsPageNumber, ResultsPager.PageSize, _entryID);
			}

			if (referrers != null && referrers.Count > 0)
			{
				ResultsPager.ItemCount = referrers.MaxItems;
				rprSelectionList.DataSource = referrers;
				rprSelectionList.DataBind();
			}

		}

		private void SetReferalDesc(string selection, string title)
		{
			Control container = Page.FindControl("PageContainer");
			if (null != container && container is Subtext.Web.Admin.WebUI.Page)
			{	
				Subtext.Web.Admin.WebUI.Page page = (Subtext.Web.Admin.WebUI.Page)container;
				string bctitle= string.Format(System.Globalization.CultureInfo.InvariantCulture, "Viewing {0}:{1}", selection,title);

				page.BreadCrumbs.AddLastItem(bctitle);
				page.Title = bctitle;
			}
		}

		public string CheckHiddenStyle()
		{
			if (_isListHidden)
				return Constants.CSSSTYLE_HIDDEN;
			else
				return String.Empty;
		}

		public string GetTitle(object dataContainer)
		{
			
			if (dataContainer is Referrer)
			{
				Referrer referrer = (Referrer) dataContainer;


				if(referrer.PostTitle != null)
				{

					if (referrer.PostTitle.Trim().Length <= 50)
					{
						return "<a href=\"../posts/" + referrer.EntryID + ".aspx\" target=\"_new\">" + referrer.PostTitle + "</a>";
					}
					else
					{
						return "<a href=\"../posts/" + referrer.EntryID + ".aspx\" target=\"_new\">" + referrer.PostTitle.Substring(0,50) + "</a>";
					}
				}
				else
				{
					return "Unknown";
				}
			}
			else
			{
				return "Unknown";
			}

		}

		public string GetReferrer(object dataContainer)
		{

			if (dataContainer is Referrer)
			{
				Referrer referrer = (Referrer) dataContainer;


				return "<a href=\"" + referrer.ReferrerURL + "\" target=\"_new\">" + referrer.ReferrerURL.Substring(0,referrer.ReferrerURL.Length > 50 ? 50 : referrer.ReferrerURL.Length) + "</a>";
			}
			else
			{
				return "Unknown";
			}

		}

		private int EntryID
		{
			get{return (int)ViewState["EntryID"];}
			set{ViewState["EntryID"] = value;}
		}

		private void rprSelectionList_ItemCommand(object source, System.Web.UI.WebControls.RepeaterCommandEventArgs e)
		{
			switch (e.CommandName.ToLower(System.Globalization.CultureInfo.InvariantCulture)) 
			{
				case "create" :
					object[] args = e.CommandArgument.ToString().Split('|');
					EntryID = Int32.Parse(args[0].ToString(), CultureInfo.InvariantCulture);
					txbUrl.Text = args[1].ToString();
					this.Edit.Visible = true;
					this.Results.Collapsible = true;
					this.Results.Collapsed = true;
					txbTitle.Text = string.Empty;
					txbBody.Text = string.Empty;
					break;

				default:
					break;
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
			this.rprSelectionList.ItemCommand += new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rprSelectionList_ItemCommand);
			this.lkbPost.Click += new System.EventHandler(this.lkbPost_Click);
			this.lkbCancel.Click += new System.EventHandler(this.lkbCancel_Click);
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion

		private void lkbPost_Click(object sender, System.EventArgs e)
		{
			try
			{
				Entry entry = new Entry(PostType.PingTrack);
				entry.Title = txbTitle.Text;
				entry.Body = txbBody.Text.Trim().Length > 0 ? txbBody.Text.Trim() : txbTitle.Text;
				entry.TitleUrl = HtmlHelper.CheckForUrl(txbUrl.Text);
				entry.DateCreated = entry.DateUpdated = BlogTime.CurrentBloggerTime;
				entry.ParentID = EntryID;

				if( Entries.Create(entry) > 0)
				{
					this.Messages.ShowMessage(Constants.RES_SUCCESSNEW);
					this.Edit.Visible = false;
				}
				else
				{
					this.Messages.ShowError(Constants.RES_FAILUREEDIT 
						+ " There was a baseline problem posting your Trackback.");
				}
			}
			catch(Exception ex)
			{
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, 
					Constants.RES_FAILUREEDIT, ex.Message));
			}
			finally
			{
				Results.Collapsible = false;
			}
		
		}

		private void lkbCancel_Click(object sender, System.EventArgs e)
		{
			Results.Collapsible = false;
			Edit.Visible = false;
		}

	}
}

