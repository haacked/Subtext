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
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Xml;
using Subtext.Framework.Data;

namespace Subtext.Web
{
	/// <summary>
	/// Summary description for OPML.
	/// </summary>
	public class OPML : System.Web.UI.Page
	{
		private void Page_Load(object sender, System.EventArgs e)
		{
			string sql = "DNW_Stats";
			string conn = Subtext.Framework.Providers.DbProvider.Instance().ConnectionString;

			int GroupID = 1;

			if(Request.QueryString["GroupID"] !=null)
			{
				try
				{
					GroupID = Int32.Parse(Request.QueryString["GroupID"]);
				}
				catch{}

			}

			SqlParameter[] p = 
				{
					SqlHelper.MakeInParam("@Host",SqlDbType.NVarChar,100,ConfigurationSettings.AppSettings["AggregateHost"] as string),
					SqlHelper.MakeInParam("@GroupID",SqlDbType.Int,4,GroupID)
				};


			DataTable dt = SqlHelper.ExecuteDataTable(conn,CommandType.StoredProcedure,sql,p);
			Response.ContentType = "text/xml";
			//Response.ContentEncoding = System.Text.Encoding.UTF8;
			Response.Write(Opml.Write(dt,Request.ApplicationPath));
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
			this.Load += new System.EventHandler(this.Page_Load);
		}
		#endregion

		#region OPML

		private class Opml
		{
			public Opml()
			{
			}

			public static string Write(DataTable dt, string appPath)
			{
				if(!appPath.EndsWith("/"))
				{
					appPath += "/";
				}
				try
				{
					StringWriter sw = new StringWriter();
					
					XmlTextWriter writer = new XmlTextWriter(sw);
					writer.Formatting = Formatting.Indented;
					writer.WriteStartDocument();

					//OPML ROOT
					writer.WriteStartElement("opml");

					//Body
					writer.WriteStartElement("body");

					int count = dt.Rows.Count;
					string baseUrl = "http://{0}" + appPath + "{1}";
					for(int i = 0; i< count; i++)
					{
						DataRow dr = dt.Rows[i];
						writer.WriteStartElement("outline");

						string title = (string)dr["Title"];
						string htmlUrl = string.Format(baseUrl, (string)dr["Host"], (string)dr["Application"]);
						string xmlUrl= htmlUrl + "/rss.aspx";

						writer.WriteAttributeString("title",title);
						writer.WriteAttributeString("htmlUrl",htmlUrl);
						writer.WriteAttributeString("xmlUrl",xmlUrl);

						writer.WriteEndElement();									
					}
					writer.WriteEndElement();
					writer.WriteEndElement();
					writer.Flush();
					writer.Close();
					sw.Close();
					return sw.ToString();

				}
				catch(Exception e)
				{
					throw e;
				}


			}
		}

		#endregion
	}
}

