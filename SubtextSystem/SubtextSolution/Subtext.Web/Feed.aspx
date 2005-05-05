<%@ Page Language="C#" %>
<%@ import Namespace="System" %>
<%@ import Namespace="System.IO" %>
<%@ import Namespace="System.Configuration" %>
<%@ import Namespace="System.Collections" %>
<%@ import Namespace="System.Data" %>
<%@ import Namespace="System.Web" %>
<%@ import Namespace="System.Xml" %>
<%@ import Namespace="Subtext.SqlDataProvider" %>
<%@ import Namespace="System.Web.Caching" %>
<%@ import Namespace="Subtext.Framework" %>
<script runat="server">

    private void Page_Load(object sender, System.EventArgs e)
    {
        Feed rssFeed = (Feed)Cache["rssFeed"];
        if(rssFeed == null)
        {
            DataTable dt = SqlHelper.ExecuteDataTable(BlogConfigurationSettings.Instance(Context).DataStoreLocation,CommandType.StoredProcedure,"DNW_GetRecentPosts");
            rssFeed = Rss.GetRSS(dt,Request.ApplicationPath);
            Cache.Add("rssFeed",rssFeed,null,DateTime.Now.AddSeconds(120),TimeSpan.Zero,CacheItemPriority.High,null);
        }
    
        if(rssFeed != null)
        {
            string etag = Request.Headers["If-None-Match"];
            if(etag != null && etag == rssFeed.Etag)
            {
                Response.StatusCode = 304;
            }
            else
            {
                Response.ContentEncoding = System.Text.Encoding.UTF8;
                Response.ContentType = "text/xml";
                Response.Cache.SetCacheability(HttpCacheability.Public);
                Response.Cache.SetETag(rssFeed.Etag);
                Response.Write(rssFeed.RSS);
            }
        }
    
    }
    
    private class Feed
    {
        public Feed()
        {
    
        }
    
        public string RSS;
        public string Etag;
    }
    
    private class Rss
    {
        public Rss(){}
    
        public static Feed GetRSS(DataTable dt, string appPath)
        {
            Feed rssFeed = new Feed();
            if(!appPath.EndsWith("/"))
            {
                appPath += "/";
            }
            try
            {
                StringWriter sw = new StringWriter();
                XmlTextWriter writer = new XmlTextWriter(sw);
    
                //writer.WriteStartDocument();
    
                //RSS ROOT
                writer.WriteStartElement("rss");
                writer.WriteAttributeString("version","2.0");
                writer.WriteAttributeString("xmlns:dc","http://purl.org/dc/elements/1.1/");
                writer.WriteAttributeString("xmlns:trackback","http://madskills.com/public/xml/rss/module/trackback/");
                writer.WriteAttributeString("xmlns:wfw","http://wellformedweb.org/CommentAPI/");
                //writer.WriteAttributeString("xmlns:slash","http://purl.org/rss/1.0/modules/slash/");
    
                //Channel
                writer.WriteStartElement("channel");
                //Channel Description
                writer.WriteElementString("title","WebLogs @ ASP.NET");
                writer.WriteElementString("link","http://weblogs.asp.net");
                writer.WriteElementString("description",".NETWeblogs by .NET Developers");
                writer.WriteElementString("generator",Subtext.Framework.VersionInfo.Version);
    
                int count = dt.Rows.Count;
                int servertz = BlogConfigurationSettings.Instance().ServerTimeZone;
                string baseUrl = "http://{0}" + appPath + "{1}/";
                for(int i = 0; i< count; i++)
                {
                    DataRow dr = dt.Rows[i];
    
                    if(i == 0)
                    {
                        rssFeed.Etag = dr["ID"].ToString();
                    }
    
    
                    writer.WriteStartElement("item");
                    writer.WriteElementString("title",(string)dr["Title"]);
    
                    string baselink = string.Format(baseUrl,(string)dr["Host"],(string)dr["Application"]);
                    string link = string.Format(baselink + "posts/{0}.aspx",dr["ID"].ToString());
                    writer.WriteElementString("link",link);
    
                    DateTime time = (DateTime)dr["DateAdded"];
                    int tz = (int)dr["TimeZone"];
                    int offset = (servertz - tz);
    
    
                    writer.WriteElementString("pubDate",(time.AddHours(offset)).ToUniversalTime().ToString("r"));
                    //writer.WriteElementString("guid",link);
                    writer.WriteStartElement("guid");
                    writer.WriteAttributeString("isPermaLink","true");
                    writer.WriteString(link);
                    writer.WriteEndElement();
    
                    writer.WriteElementString("wfw:comment",baselink + "comments/" +dr["ID"].ToString() + ".aspx");
                    writer.WriteElementString("wfw:commentRss", baselink + "comments/commentRss/" + dr["ID"].ToString() + ".aspx");
                    writer.WriteElementString("comments",link + "#comment");
                    //	writer.WriteElementString("slash:comments",dr["CommentCount"].ToString());
                    writer.WriteElementString("trackback:ping",baselink + "trackback.aspx?ID=" + dr["ID"].ToString());
    
                    writer.WriteStartElement("source");
                    writer.WriteAttributeString("url",baselink + "rss.aspx");
                    writer.WriteString((string)dr["BlogTitle"]);
                    writer.WriteEndElement();
    
    
                    if(dr["Description"] != DBNull.Value && ((string)dr["Description"]).Length == 0)
                    {
                        writer.WriteElementString("description",(string)dr["Description"]);
                    }
                    else
                    {
                        writer.WriteElementString("description",(string)dr["Text"]);
                    }
    
                    if(dr["IsXHTML"] != DBNull.Value && (bool)dr["IsXHTML"])
                    {
    
                        writer.WriteStartElement("body");
                        writer.WriteAttributeString("xmlns","http://www.w3.org/1999/xhtml");
                        writer.WriteRaw((string)dr["Text"]);
                        writer.WriteEndElement();
    
                    }
                    writer.WriteElementString("dc:creator",(string)dr["Author"]);
                    writer.WriteEndElement();
    
                }
                writer.WriteEndElement();
    
                writer.WriteEndElement();
                writer.Flush();
                writer.Close();
                sw.Close();
                rssFeed.RSS = sw.ToString();
                return rssFeed;
    
            }
            catch(Exception e)
            {
                throw e;
            }
    
    
    
        }
    }

</script>
