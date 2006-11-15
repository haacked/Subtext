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
using log4net;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Text;
using Subtext.Web.Admin.WebUI;

namespace Subtext.Web.Admin.Pages
{
	public partial class Referrers : StatsPage
	{
		private readonly static ILog log = new Subtext.Framework.Logging.Log();
		
		private int pageIndex;
		private int _entryID = NullValue.NullInt32;

		#region Declared Controls
		#endregion
	    
	    public Referrers() : base()
	    {
            this.TabSectionId = "Stats";
	    }
	
		protected void Page_Load(object sender, System.EventArgs e)
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
				Results.Collapsible = false;

				BindList();
			}
		}

		
		protected override void BindLocalUI()
		{
			if(_entryID == NullValue.NullInt32)
			{

				//SetReferalDesc("Referrals");
			}
			else
			{
				SetReferalDesc("Entry", _entryID.ToString(CultureInfo.InvariantCulture));
			}
            base.BindLocalUI();
		}

		private void BindList()
		{
            IPagedCollection<Referrer> referrers;

			if(_entryID == NullValue.NullInt32)
			{
				referrers = Stats.GetPagedReferrers(this.pageIndex, this.resultsPager.PageSize);
			}
			else
			{
				this.resultsPager.UrlFormat += string.Format(System.Globalization.CultureInfo.InvariantCulture, "&{0}={1}", "EntryID", 
					_entryID);
				referrers = Stats.GetPagedReferrers(this.pageIndex, this.resultsPager.PageSize, _entryID);
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
		    if(AdminMasterPage != null && AdminMasterPage.BreadCrumb != null)
			{
				string bctitle= string.Format(System.Globalization.CultureInfo.InvariantCulture, "Viewing {0}:{1}", selection,title);

				AdminMasterPage.BreadCrumb.AddLastItem(bctitle);
                AdminMasterPage.Title = bctitle;
			}
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

		}
		#endregion

		protected void lkbPost_Click(object sender, System.EventArgs e)
		{
			try
			{
				Trackback entry = new Trackback(EntryID, txbTitle.Text, HtmlHelper.CheckForUrl(txbUrl.Text), string.Empty, txbBody.Text.Trim().Length > 0 ? txbBody.Text.Trim() : txbTitle.Text, Config.CurrentBlog.TimeZone.Now);

				if(FeedbackItem.Create(entry, null) > 0)
				{
					CommentFilter filter = new CommentFilter(HttpContext.Current.Cache);
					filter.FilterAfterPersist(entry);
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
				log.Error(ex.Message, ex);
				this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, 
					Constants.RES_FAILUREEDIT, ex.Message));
			}
			finally
			{
				Results.Collapsible = false;
			}
		
		}

		protected void lkbCancel_Click(object sender, System.EventArgs e)
		{
			Results.Collapsible = false;
			Edit.Visible = false;
		}

	}
}

