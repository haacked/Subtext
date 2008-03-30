using System;
using System.Data;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.HtmlControls;
using Subtext.Framework.Providers;
using System.Globalization;

namespace Subtext.Web.UI.Controls
{
    public class AggBloggers : BaseControl
    {
        protected Repeater Bloggers;

        //A value of 0 retrieves all groups
        private int _blogGroup = 0;
        public int BlogGroup
        {
            get { return _blogGroup; }
            set { _blogGroup = value; }
        }

        private bool _showGroups = true;
        public bool ShowGroups
        {
            get { return _showGroups; }
            set { _showGroups = value; }
        }

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            if (Request.QueryString["GroupID"] != null)
            {
            	int blogGroupId;
				if(Int32.TryParse(Request.QueryString["GroupID"], out blogGroupId))
	            	BlogGroup = blogGroupId;
            }
        }

        private string appPath;
        readonly string fullUrl = HttpContext.Current.Request.Url.Scheme + "://{0}{1}{2}/";
        protected string GetFullUrl(string host, string app)
        {
            if (appPath == null)
            {
                appPath = HttpContext.Current.Request.ApplicationPath;
                if (!appPath.ToLower(CultureInfo.InvariantCulture).EndsWith("/"))
                {
                    appPath += "/";
                }
            }

            if (Request.Url.Port != 80)
            {
                host += ":" + Request.Url.Port;
            }

            return string.Format(fullUrl, host, appPath, app);

        }

        protected string GetListHtml()
        {
            HtmlGenericControl ul = new HtmlGenericControl("ul");
            DataSet ds = DbProvider.Instance().GetAggregateStats(BlogGroup);
            DataTable dt = ds.Tables[0];
            if (dt != null && dt.Rows.Count > 0)
            {
                int r = 0;
                int LastGroupId = (Int32)dt.Rows[0]["BlogGroupId"];
                if (ShowGroups)
                {
                    while (r < dt.Rows.Count)
                    {
                        HtmlGenericControl groupLi = new HtmlGenericControl("li");
                        HtmlGenericControl h3 = new HtmlGenericControl("h3");
                        h3.InnerHtml = dt.Rows[r]["BlogGroupTitle"].ToString();
                        groupLi.Controls.Add(h3);
                        ul.Controls.Add(groupLi);
                        HtmlGenericControl groupUl = new HtmlGenericControl("ul");
                        while (r < dt.Rows.Count && LastGroupId == (Int32)dt.Rows[r]["BlogGroupId"])
                        {
                            HtmlGenericControl bloggerLi = new HtmlGenericControl("li");
                            HtmlAnchor anc = new HtmlAnchor();
                            anc.InnerHtml = dt.Rows[r]["Author"].ToString();
                            anc.HRef = GetFullUrl(dt.Rows[r]["Host"].ToString(), dt.Rows[r]["Application"].ToString());
                            bloggerLi.Controls.Add(anc);
                            HtmlGenericControl sm = new HtmlGenericControl("small");
                            sm.InnerHtml = string.Format("<br />{0}, {1} {2}", dt.Rows[r]["PostCount"], ((DateTime)dt.Rows[r]["LastUpdated"]).ToShortDateString(), ((DateTime)dt.Rows[r]["LastUpdated"]).ToShortTimeString());
                            bloggerLi.Controls.Add(sm);
                            groupUl.Controls.Add(bloggerLi);
                            r++;
                        }
                        if (r < dt.Rows.Count)
                            LastGroupId = (Int32)dt.Rows[r]["BlogGroupId"];
                        groupLi.Controls.Add(groupUl);
                    }
                }
                else
                {
                        while (r < dt.Rows.Count)
                        {
                            HtmlGenericControl bloggerLi = new HtmlGenericControl("li");
                            HtmlAnchor anc = new HtmlAnchor();
                            anc.InnerHtml = dt.Rows[r]["Author"].ToString();
                            anc.HRef = GetFullUrl(dt.Rows[r]["Host"].ToString(), dt.Rows[r]["Application"].ToString());
                            bloggerLi.Controls.Add(anc);
                            HtmlGenericControl sm = new HtmlGenericControl("small");
                            sm.InnerHtml = string.Format("<br />{0}, {1} {2}", dt.Rows[r]["PostCount"], ((DateTime)dt.Rows[r]["LastUpdated"]).ToShortDateString(), ((DateTime)dt.Rows[r]["LastUpdated"]).ToShortTimeString());
                            bloggerLi.Controls.Add(sm);
                            ul.Controls.Add(bloggerLi);
                            r++;
                        }
                }
            }
            System.IO.StringWriter sw = new System.IO.StringWriter();
            ul.RenderControl(new HtmlTextWriter(sw));
            return sw.ToString();
        }
    }
}
