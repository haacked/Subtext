namespace Subtext.Web.UI.Controls
{
	using System;
	using System.Data;
	using System.Data.SqlClient;
	using System.Drawing;
	using System.Web;
	using System.Web.UI.WebControls;
	using System.Web.UI.HtmlControls;
	using Subtext.Common.Data;
	using Subtext.Framework;
	using Subtext.Framework.Data;
	using Subtext.Framework.Util;
	using Subtext.Framework.Components;
	using Subtext.Framework.Configuration;
	/// <summary>
	/// Summary description for PreviousNext.
	/// </summary>
	public class PreviousNext : Subtext.Web.UI.Controls.BaseControl
	{
		protected System.Web.UI.WebControls.HyperLink NextLink;
		protected System.Web.UI.WebControls.HyperLink PrevLink;
		protected System.Web.UI.WebControls.HyperLink MainLink;
		protected System.Web.UI.WebControls.Label LeftPipe;
		protected System.Web.UI.WebControls.Label RightPipe;
		
		public PreviousNext()
		{
			//
			// TODO: Add constructor logic here
			//
		}


		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			
			//Get the entry
			Entry entry = Cacher.GetEntryFromRequest(Subtext.Framework.CacheDuration.Short);			
			
			//if found
			if(entry != null)
			{
				//Sent entry properties
				MainLink.NavigateUrl = CurrentBlog.RootUrl;

				string ConnectionString = Subtext.Framework.Providers.DbProvider.Instance().ConnectionString;
				SqlParameter[] p =
					{
						SqlHelper.MakeInParam("@ID",SqlDbType.Int,4,entry.EntryID),
						SqlHelper.MakeInParam("@BlogID",SqlDbType.Int,4,Config.CurrentBlog.BlogId)
					};

				//System.Data.SqlClient.SqlDataReader sdr = SqlHelper.ExecuteReader(ConnectionString,CommandType.StoredProcedure,"blog_GetEntry_PreviousNext",p);
				System.Data.DataSet ds;
				using (SqlConnection cn = new SqlConnection(ConnectionString))
				{
					cn.Open();

					//call the overload that takes a connection in place of the connection string
					ds = Subtext.Framework.Data.SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure,"Subtext_GetEntry_PreviousNext",p);
					cn.Close();
				}
				switch(ds.Tables[0].Rows.Count)
				{
					case 0:
					{
						//you have no entries. You should blog more
						PrevLink.Visible = false;
						NextLink.Visible = false;
						break;
					}
					case 1:
					{
						//since there is only one record, you are at an end
						//Check EntryID to see if it is greater or less than
						//the current ID
						if ((int)ds.Tables[0].Rows[0]["EntryID"] > entry.EntryID)
						{
							//this is the oldest blog
							PrevLink.Visible = false;
							LeftPipe.Visible = false;							
							SetNav(NextLink, ds.Tables[0].Rows[0]);
							NextLink.Text = NextLink.Text + " >>";
						}
						else
						{
							//this is the latest blog
							NextLink.Visible = false;
							RightPipe.Visible = false;
							SetNav(PrevLink, ds.Tables[0].Rows[0]);
							PrevLink.Text = "<< " + PrevLink.Text;
						}
						break;
					}
					case 2:
					{
						//two records found. The first record will be NEXT
						//the second record will be PREVIOUS
						//This is because the query is sorted by EntryID
						SetNav(PrevLink, ds.Tables[0].Rows[0]);
						PrevLink.Text = "<< " + PrevLink.Text;							
						SetNav(NextLink, ds.Tables[0].Rows[1]);
						NextLink.Text = NextLink.Text + " >>";
						break;
					}
				}
				

			}
			else 
			{
				//No post? Deleted? Help :)
				this.Controls.Clear();
				this.Controls.Add(new System.Web.UI.LiteralControl("<p><strong>The entry could not be found or has been removed</strong></p>"));
			}
		}


		private void SetNav(System.Web.UI.WebControls.HyperLink navLink, System.Data.DataRow dr)
		{
			string linkName;
			navLink.Text = (string)dr["EntryTitle"];
			if (dr["EntryName"] != DBNull.Value)
			{
				linkName = (string)dr["EntryName"];
			}
			else
			{
				linkName = dr["EntryID"].ToString();
			}
			navLink.NavigateUrl = string.Format(CurrentBlog.RootUrl + "archive/{0}/{1}.aspx",((DateTime)dr["EntryDate"]).ToString("yyyy/MM/dd"),linkName);

		}
	}

}
