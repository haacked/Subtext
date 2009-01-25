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
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Text;
using Subtext.Web.Admin.WebUI;
using Subtext.Framework.Providers;
using Subtext.Framework.Services;
using Subtext.Framework.Data;

namespace Subtext.Web.Admin.Pages
{
	public partial class Referrers : StatsPage
	{
		private readonly static ILog log = new Log();
		private int pageIndex = 0;
		private bool _isListHidden = false;
		private int _entryID = NullValue.NullInt32;
	    
	    public Referrers()
	    {
            this.TabSectionId = "Stats";
	    }
	
		protected void Page_Load(object sender, EventArgs e)
		{

			if(!IsPostBack)
			{
				if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
				{
					this.pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
				}

				if(null != Request.QueryString["EntryID"])
				{
					_entryID = Convert.ToInt32(Request.QueryString["EntryID"]);
				}

				this.resultsPager.PageSize = Preferences.ListingItemCount;
				this.resultsPager.PageIndex = this.pageIndex;

				BindList();
			}
		}
		
		protected override void BindLocalUI()
		{
			if(_entryID == NullValue.NullInt32) {
				//SetReferalDesc("Referrals");
			}
			else {
				SetReferalDesc("Entry", _entryID.ToString(CultureInfo.InvariantCulture));
			}
            base.BindLocalUI();
		}

		private void BindList()
		{
            IPagedCollection<Referrer> referrers;
            StatsRepository stats = new StatsRepository(ObjectProvider.Instance(), Config.Settings.Tracking);

			if(_entryID == NullValue.NullInt32) {
                referrers = stats.GetPagedReferrers(this.pageIndex, this.resultsPager.PageSize);
			}
			else
			{
				this.resultsPager.UrlFormat += string.Format(CultureInfo.InvariantCulture, "&{0}={1}", "EntryID", _entryID);
                referrers = stats.GetPagedReferrers(this.pageIndex, this.resultsPager.PageSize, _entryID);
			}

			if (referrers != null && referrers.Count > 0)
			{
				this.resultsPager.ItemCount = referrers.MaxItems;
				rprSelectionList.DataSource = referrers;
				rprSelectionList.DataBind();
			}
		}

		private void SetReferalDesc(string selection, string title)
		{
		    if(AdminMasterPage != null)
			{
				string bctitle = string.Format(CultureInfo.InvariantCulture, "Viewing {0}:{1}", selection,title);
                AdminMasterPage.Title = bctitle;
			}
		}

		public string CheckHiddenStyle()
		{
			if (_isListHidden)
				return Constants.CSSSTYLE_HIDDEN;
			else
				return String.Empty;
		}

		public string GetTitle(object dataItem)
		{
			if (dataItem is Referrer)
			{
				Referrer referrer = (Referrer) dataItem;

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

		public string GetReferrer(object dataItem)
		{
			if (dataItem is Referrer)
			{
				Referrer referrer = (Referrer) dataItem;
                string urlEncodedReferrerUrl = Uri.EscapeUriString(referrer.ReferrerURL);                
                string htmlEncodedReferrerUrl;
                
                // Chop it here because otherwise we could end up with a badly HTML encoded string if the chop appears after the encoding
                if (referrer.ReferrerURL.Length > 50)
                    htmlEncodedReferrerUrl = referrer.ReferrerURL.Substring(0, 50);
                else
                    htmlEncodedReferrerUrl = referrer.ReferrerURL;

                return "<a href=\"" + urlEncodedReferrerUrl + "\" target=\"_new\">" +
                    HttpUtility.HtmlEncode(htmlEncodedReferrerUrl) + "</a>";
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

		private void rprSelectionList_ItemCommand(object source, RepeaterCommandEventArgs e)
		{
			switch (e.CommandName.ToLower(CultureInfo.InvariantCulture)) 
			{
				case "create" :
					object[] args = e.CommandArgument.ToString().Split('|');
					EntryID = Int32.Parse(args[0].ToString(), CultureInfo.InvariantCulture);
					txbUrl.Text = args[1].ToString();
					this.Edit.Visible = true;
					this.Results.Visible = false;
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

		}
		#endregion

		protected void lkbPost_Click(object sender, EventArgs e)
		{
			try
			{
				Trackback entry = new Trackback(EntryID, txbTitle.Text, HtmlHelper.CheckForUrl(txbUrl.Text), string.Empty, txbBody.Text.Trim().Length > 0 ? txbBody.Text.Trim() : txbTitle.Text, Config.CurrentBlog.TimeZone.Now);

				if(FeedbackItem.Create(entry, null) > 0)
				{
                    IFeedbackSpamService feedbackService = null;
                    if (Config.CurrentBlog.FeedbackSpamServiceEnabled) {
                        feedbackService = new AkismetSpamService(Config.CurrentBlog.FeedbackSpamServiceKey, Config.CurrentBlog, null, Url);
                    }
					CommentFilter filter = new CommentFilter(new SubtextCache(HttpContext.Current.Cache), feedbackService);
					filter.FilterAfterPersist(entry);
					this.Messages.ShowMessage(Constants.RES_SUCCESSNEW);
					this.Edit.Visible = false;
                    this.Results.Visible = true;
				}
				else
				{
					this.Messages.ShowError(Constants.RES_FAILUREEDIT 
						+ " There was a baseline problem posting your Trackback.");
				}
			}
			catch(Exception ex)
			{
				log.Error(ex.Message, ex);
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, 
					Constants.RES_FAILUREEDIT, ex.Message));
			}
		}

		protected void lkbCancel_Click(object sender, EventArgs e)
		{
			Edit.Visible = false;
            Results.Visible = true;
		}
	}
}

