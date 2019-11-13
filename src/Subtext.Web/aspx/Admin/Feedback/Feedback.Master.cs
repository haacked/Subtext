using System;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Web.Admin.WebUI;
using Subtext.Web.UI.Controls;

namespace Subtext.Web.Admin.Feedback
{
    public partial class FeedbackMaster : AdminMasterPage
    {
        FeedbackCounts _counts;
        FeedbackType _feedbackType = (FeedbackType)(-1);
        int page = NullValue.NullInt32;
        FeedbackStatusFlag _status = (FeedbackStatusFlag)(-1);

        public string CurrentQuery
        {
            get { return GetCurrentQuery(PageIndex, FeedbackStatus, FeedbackType); }
        }

        public int PageIndex
        {
            get
            {
                if (page == NullValue.NullInt32)
                {
                    string pageText = Request.QueryString["pg"] ?? "0";
                    page = Convert.ToInt32(pageText);
                }
                return page;
            }
        }

        public FeedbackType FeedbackType
        {
            get
            {
                if (_feedbackType == (FeedbackType)(-1))
                {
                    string typeText = Request.QueryString["type"] ?? Preferences.GetFeedbackItemFilter(FeedbackStatus);
                    try
                    {
                        _feedbackType = (FeedbackType)Enum.Parse(typeof(FeedbackType), typeText);
                        if (!Contact.ShowContactMessages && _feedbackType == FeedbackType.ContactPage && !Contact.SendContactMessageToFeedback)
                        {
                            _feedbackType = FeedbackType.None;
                        }
                    }
                    catch (ArgumentException)
                    {
                        //Grab it from the cookie.
                        _feedbackType = FeedbackType.None;
                    }
                }
                Preferences.SetFeedbackItemFilter(_feedbackType.ToString(), FeedbackStatus);
                return _feedbackType;
            }
        }

        public FeedbackStatusFlag FeedbackStatus
        {
            get
            {
                if (_status == (FeedbackStatusFlag)(-1))
                {
                    string filter = Request.QueryString["status"] ?? "Approved";
                    try
                    {
                        _status = (FeedbackStatusFlag)Enum.Parse(typeof(FeedbackStatusFlag), filter, true);
                    }
                    catch (ArgumentException)
                    {
                        _status = FeedbackStatusFlag.Approved;
                    }
                }
                return _status;
            }
        }

        protected FeedbackCounts Counts
        {
            get { return _counts; }
        }

        public string ListUrl()
        {
            return ListUrl(PageIndex, FeedbackStatus, FeedbackType);
        }

        public string ListUrl(FeedbackType filter)
        {
            return ListUrl(PageIndex, FeedbackStatus, filter);
        }

        public string ListUrl(FeedbackStatusFlag status, int pageIndex)
        {
            return ListUrl(pageIndex, status, FeedbackType);
        }

        //For master page only...
        protected string GetListUrl(FeedbackStatusFlag status)
        {
            return "Default.aspx?status=" + status;
        }

        public string ListUrl(FeedbackStatusFlag status)
        {
            return ListUrl(PageIndex, status, FeedbackType);
        }

        public string ListUrl(int page, FeedbackStatusFlag status, FeedbackType type)
        {
            return "Default.aspx?" + GetCurrentQuery(page, status, type);
        }

        public string GetCurrentQuery(int page, FeedbackStatusFlag status, FeedbackType type)
        {
            const string query = "page={0}&status={1}&type={2}";
            return String.Format(query, page, status, type);
        }

        public void BindCounts()
        {
            _counts = Repository.GetFeedbackCounts();
        }
    }
}