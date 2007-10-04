using System;
using System.Data;
using System.Web.UI.WebControls;
using Subtext.Data;
using Subtext.Framework;

namespace Subtext.Web.UI.Controls
{
    public class AggBlogStats : BaseControl
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


			DataSet ds = StoredProcedures.DNWTotalStats(BlogInfo.AggregateBlog.Host, GroupID).GetDataSet();
            if(ds.Tables[0] != null)
			{
				DataRow dr = ds.Tables[0].Rows[0];
				BlogCount.Text = dr["BlogCount"].ToString();
				PostCount.Text = dr["PostCount"].ToString();
				StoryCount.Text = dr["StoryCount"].ToString();
				CommentCount.Text = dr["CommentCount"].ToString();
				PingtrackCount.Text = dr["PingtrackCount"].ToString();

			}
        }
    }
}
