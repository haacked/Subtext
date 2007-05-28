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
using System.IO;
using System.Xml;
using Subtext.Data;

namespace Subtext.Web
{
	/// <summary>
	/// Summary description for OPML.
	/// </summary>
	public class OPML : System.Web.UI.Page
	{
		private void Page_Load(object sender, EventArgs e)
		{
			int groupId = 1;

			if(Request.QueryString["GroupID"] != null)
			{
				Int32.TryParse(Request.QueryString["GroupID"], out groupId);
			}

			//TODO: put ConfigurationManager.AppSettings["AggregateHost"] in some property.
			DataSet ds = StoredProcedures.DNWStats(ConfigurationManager.AppSettings["AggregateHost"], groupId).GetDataSet();
			if (ds == null || ds.Tables.Count == 0)
				return;

			DataTable dt = ds.Tables[0];
			Response.ContentType = "text/xml";
			//Response.ContentEncoding = System.Text.Encoding.UTF8;
			Response.Write(Write(dt,Request.ApplicationPath));
		}
		
		private static string Write(DataTable dt, string appPath)
		{
			if(!appPath.EndsWith("/"))
			{
				appPath += "/";
			}
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
				string htmlUrl = string.Format(baseUrl, dr["Host"], dr["Application"]);
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
	}
}

