using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Subtext.Framework.Providers;
using Subtext.Framework.Data;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
    public partial class AggBlogStats : BaseControl
    {
        protected Literal BlogCount;
        protected Literal PostCount;
        protected Literal StoryCount;
        protected Literal CommentCount;
        protected Literal PingtrackCount;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            int GroupID = 0;

			if(Request.QueryString["GroupID"] !=null)
			{
				try
				{
					GroupID = Int32.Parse(Request.QueryString["GroupID"]);
				}
				catch{}

			}

            HostStats stats = ObjectProvider.Instance().GetTotalBlogStats(BlogInfo.AggregateBlog.Host, GroupID);
            if (stats != null)
            {
                BlogCount.Text = stats.BlogCount.ToString();
                PostCount.Text = stats.PostCount.ToString();
                StoryCount.Text = stats.StoryCount.ToString();
                CommentCount.Text = stats.CommentCount.ToString();
                PingtrackCount.Text = stats.PingTrackCount.ToString();
            }
        }
    }
}
