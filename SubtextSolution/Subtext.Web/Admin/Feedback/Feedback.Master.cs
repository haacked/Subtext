using System;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Web.Admin.WebUI;

namespace Subtext.Web.Admin.Feedback {
    public partial class FeedbackMaster : AdminMasterPage 
    {
        public string ListUrl() {
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
            string query = "page={0}&status={1}&type={2}";
            return String.Format(query, page, status, type);
        }

        public string CurrentQuery
        {
            get 
            { 
                return GetCurrentQuery(PageIndex, FeedbackStatus, FeedbackType);
            }
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
        int page = NullValue.NullInt32;

        public FeedbackType FeedbackType 
        {
            get 
            {
                if (feedbackType == (Subtext.Extensibility.FeedbackType)(-1) )
                {
                    string typeText = Request.QueryString["type"] ?? Preferences.GetFeedbackItemFilter(FeedbackStatus);
                    try 
                    {
                        feedbackType = (FeedbackType)Enum.Parse(typeof(FeedbackType), typeText);
                    }
                    catch (ArgumentException) 
                    {
                        //Grab it from the cookie.
                        feedbackType = FeedbackType.None;
                    }
                }
                Preferences.SetFeedbackItemFilter(feedbackType.ToString(), FeedbackStatus);
                return feedbackType;
            }
        }
        FeedbackType feedbackType = (Subtext.Extensibility.FeedbackType)(-1);

        public FeedbackStatusFlag FeedbackStatus
        {
            get 
            {
                if (status == (FeedbackStatusFlag)(-1)) 
                {
                    string filter = Request.QueryString["status"] ?? "Approved";
                    try 
                    {
                        status = (FeedbackStatusFlag)Enum.Parse(typeof(FeedbackStatusFlag), filter, true);
                    }
                    catch (ArgumentException) 
                    {
                        status = FeedbackStatusFlag.Approved;
                    }
                }
                return status;
                
            }
        }
        FeedbackStatusFlag status = (FeedbackStatusFlag)(-1);

        public void BindCounts()
        {
            counts = FeedbackItem.GetFeedbackCounts();
        }

        protected FeedbackCounts Counts
        {
            get 
            {
                return counts;
            }
        }
        FeedbackCounts counts;
    }
}
