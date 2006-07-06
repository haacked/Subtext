using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Common.Data;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for PreviousNext.
	/// </summary>
	public class PreviousNext : BaseControl
	{
		protected HyperLink NextLink;
		protected HyperLink PrevLink;
		protected HyperLink MainLink;
		protected Label LeftPipe;
		protected Label RightPipe;
		
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
			Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short);			
			
			//if found
			if(entry != null)
			{
				//Sent entry properties
				MainLink.NavigateUrl = CurrentBlog.HomeVirtualUrl;

				string ConnectionString = DbProvider.Instance().ConnectionString;
				SqlParameter[] p =
					{
						SqlHelper.MakeInParam("@ID",SqlDbType.Int,4,entry.EntryID),
						SqlHelper.MakeInParam("@BlogID",SqlDbType.Int,4,Config.CurrentBlog.Id)
					};

				//System.Data.SqlClient.SqlDataReader sdr = SqlHelper.ExecuteReader(ConnectionString,CommandType.StoredProcedure,"blog_GetEntry_PreviousNext",p);
				DataSet ds;
				using (SqlConnection cn = new SqlConnection(ConnectionString))
				{
					cn.Open();

					//call the overload that takes a connection in place of the connection string
					ds = SqlHelper.ExecuteDataset(cn, CommandType.StoredProcedure,"Subtext_GetEntry_PreviousNext",p);
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
				this.Controls.Add(new LiteralControl("<p><strong>The entry could not be found or has been removed</strong></p>"));
			}
		}


		private void SetNav(HyperLink navLink, DataRow dr)
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
