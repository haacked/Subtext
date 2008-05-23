using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using Subtext.Framework.Configuration;
using Subtext.Framework.Components;
using Subtext.Extensibility;
using Subtext.Framework;

namespace Subtext.Web.Admin.Feedback {
    public partial class FeedbackMaster : System.Web.UI.MasterPage 
    {
        protected string CreateAdminRssUrl(string pageName) 
        {
            return String.Format("{0}Admin/{1}", Config.CurrentBlog.RootUrl, pageName);
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
