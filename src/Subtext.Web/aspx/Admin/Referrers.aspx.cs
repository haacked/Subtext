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
using System.Globalization;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;
using Subtext.Framework.Services;
using Subtext.Framework.Text;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.Properties;

namespace Subtext.Web.Admin.Pages
{
    public partial class Referrers : StatsPage
    {
        private readonly static ILog Log = new Log();
        private int _entryId = NullValue.NullInt32;
        private bool _isListHidden = false;
        private int _pageIndex;

        public Referrers()
        {
            TabSectionId = "Stats";
        }

        private int EntryId
        {
            get { return (int)ViewState["EntryId"]; }
            set { ViewState["EntryId"] = value; }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!IsPostBack)
            {
                if (null != Request.QueryString[Keys.QRYSTR_PAGEINDEX])
                {
                    _pageIndex = Convert.ToInt32(Request.QueryString[Keys.QRYSTR_PAGEINDEX]);
                }

                if (null != Request.QueryString["EntryId"])
                {
                    _entryId = Convert.ToInt32(Request.QueryString["EntryId"]);
                }

                resultsPager.PageSize = Preferences.ListingItemCount;
                resultsPager.PageIndex = _pageIndex;

                BindList();
            }
        }

        protected override void BindLocalUI()
        {
            if (_entryId == NullValue.NullInt32)
            {
                //SetReferalDesc("Referrals");
            }
            else
            {
                SetReferalDesc("Entry", _entryId.ToString(CultureInfo.InvariantCulture));
            }
            base.BindLocalUI();
        }

        private void BindList()
        {
            IPagedCollection<Referrer> referrers;

            if (_entryId == NullValue.NullInt32)
            {
                referrers = Repository.GetPagedReferrers(_pageIndex, resultsPager.PageSize, NullValue.NullInt32);
            }
            else
            {
                resultsPager.UrlFormat += string.Format(CultureInfo.InvariantCulture, "&{0}={1}", "EntryId", _entryId);
                referrers = Repository.GetPagedReferrers(_pageIndex, resultsPager.PageSize, _entryId);
            }

            if (referrers != null && referrers.Count > 0)
            {
                resultsPager.ItemCount = referrers.MaxItems;
                rprSelectionList.DataSource = referrers;
                rprSelectionList.DataBind();
            }
        }

        private void SetReferalDesc(string selection, string title)
        {
            if (AdminMasterPage != null)
            {
                string bctitle = string.Format(CultureInfo.InvariantCulture, Resources.Label_Viewing + " {0}:{1}",
                                               selection, title);
                AdminMasterPage.Title = bctitle;
            }
        }

        public string CheckHiddenStyle()
        {
            if (_isListHidden)
            {
                return Constants.CSSSTYLE_HIDDEN;
            }
            else
            {
                return String.Empty;
            }
        }

        public string GetTitle(object dataItem)
        {
            if (dataItem is Referrer)
            {
                var referrer = (Referrer)dataItem;

                if (referrer.PostTitle != null)
                {
                    if (referrer.PostTitle.Trim().Length <= 50)
                    {
                        return "<a href=\"../posts/" + referrer.EntryId + ".aspx\" target=\"_new\">" +
                               referrer.PostTitle + "</a>";
                    }
                    return "<a href=\"../posts/" + referrer.EntryId + ".aspx\" target=\"_new\">" +
                           referrer.PostTitle.Substring(0, 50) + "</a>";
                }
                return "Unknown";
            }
            return "Unknown";
        }

        public string GetReferrer(object dataItem)
        {
            if (dataItem is Referrer)
            {
                var referrer = (Referrer)dataItem;
                string urlEncodedReferrerUrl = Uri.EscapeUriString(referrer.ReferrerUrl);

                // Chop it here because otherwise we could end up with a badly HTML encoded string if the chop appears after the encoding
                string htmlEncodedReferrerUrl = referrer.ReferrerUrl.Length > 50 ? referrer.ReferrerUrl.Substring(0, 50) : referrer.ReferrerUrl;

                return "<a href=\"" + urlEncodedReferrerUrl + "\" target=\"_new\">" +
                       HttpUtility.HtmlEncode(htmlEncodedReferrerUrl) + "</a>";
            }
            else
            {
                return "Unknown";
            }
        }

        private void rprSelectionList_ItemCommand(object source, RepeaterCommandEventArgs e)
        {
            switch (e.CommandName.ToLower(CultureInfo.InvariantCulture))
            {
                case "create":
                    object[] args = e.CommandArgument.ToString().Split('|');
                    EntryId = Int32.Parse(args[0].ToString(), CultureInfo.InvariantCulture);
                    txbUrl.Text = args[1].ToString();
                    Edit.Visible = true;
                    Results.Visible = false;
                    txbTitle.Text = string.Empty;
                    txbBody.Text = string.Empty;
                    break;

                default:
                    break;
            }
        }

        protected void lkbPost_Click(object sender, EventArgs e)
        {
            try
            {
                var entry = new Trackback(EntryId, txbTitle.Text, txbUrl.Text.EnsureUrl(), string.Empty,
                                          txbBody.Text.Trim().Length > 0 ? txbBody.Text.Trim() : txbTitle.Text);
                var commentService = new CommentService(SubtextContext, null);

                if (commentService.Create(entry, true/*runFilters*/) > 0)
                {
                    ICommentSpamService feedbackService = null;
                    if (Config.CurrentBlog.FeedbackSpamServiceEnabled)
                    {
                        feedbackService = new AkismetSpamService(Config.CurrentBlog.FeedbackSpamServiceKey,
                                                                 Config.CurrentBlog, null, Url);
                    }
                    var filter = new CommentFilter(SubtextContext, feedbackService);
                    filter.FilterAfterPersist(entry);
                    Messages.ShowMessage(Constants.RES_SUCCESSNEW);
                    Edit.Visible = false;
                    Results.Visible = true;
                }
                else
                {
                    Messages.ShowError(Constants.RES_FAILUREEDIT
                                       + " There was a baseline problem posting your Trackback.");
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message, ex);
                Messages.ShowError(String.Format(Constants.RES_EXCEPTION,
                                                 Constants.RES_FAILUREEDIT, ex.Message));
            }
        }

        protected void lkbCancel_Click(object sender, EventArgs e)
        {
            Edit.Visible = false;
            Results.Visible = true;
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
            this.rprSelectionList.ItemCommand +=
                new System.Web.UI.WebControls.RepeaterCommandEventHandler(this.rprSelectionList_ItemCommand);
        }

        #endregion
    }
}