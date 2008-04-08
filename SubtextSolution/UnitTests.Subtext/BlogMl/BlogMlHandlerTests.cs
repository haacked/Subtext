using System;
using System.Collections.Generic;
using System.Text;
using MbUnit.Framework;
using System.IO;
using Subtext.BlogML;
using System.Web;
using Subtext.Framework.Configuration;
using System.Xml;

namespace UnitTests.Subtext.BlogML
{
    [TestFixture]
    public class BlogMlHandlerTests
    {
        [Test]
        [RollBack2]
        public void CanHandleRequest()
        {
            string host = UnitTestHelper.GenerateRandomString();
            Config.CreateBlog("The Title Of This Blog", "blah", "None-of-your-biz", host, string.Empty);
            StringBuilder sb = new StringBuilder();
            StringWriter writer = new StringWriter(sb);
            UnitTestHelper.SetHttpContextWithBlogRequest(host, string.Empty, "/", "Export.aspx", writer);
            Config.CurrentBlog.Author = "MasterChief";
            Config.UpdateConfigData(Config.CurrentBlog);

            HttpContext.Current.Response.Clear();

            BlogMLProvider.Instance().ConnectionString = Config.ConnectionString;

            BlogMLHttpHandler handler = new BlogMLHttpHandler();
            Assert.AreEqual(0, sb.Length);
            handler.ProcessRequest(HttpContext.Current);
            string result = sb.ToString();
            
            //For some reason we get 2 weird chars at the beginning during the unit test.
            //Need to figure out if this is happening in a real export.
            if(result.IndexOf("<") > 0)
                result = result.Substring(result.IndexOf("<"));

            XmlDocument xml = new XmlDocument();
            xml.LoadXml(result);

            XmlNamespaceManager nsmgr = new XmlNamespaceManager(xml.NameTable);
            nsmgr.AddNamespace("bml", "http://www.blogml.com/2006/09/BlogML");

            Assert.AreEqual("The Title Of This Blog", xml.SelectSingleNode("/bml:blog/bml:title", nsmgr).InnerText);
            Assert.AreEqual("MasterChief", xml.SelectSingleNode("/bml:blog/bml:authors/bml:author/bml:title", nsmgr).InnerText);
        }
    }
}
