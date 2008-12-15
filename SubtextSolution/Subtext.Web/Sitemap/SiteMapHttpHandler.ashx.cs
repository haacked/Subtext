using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Format;
using Subtext.Web.SiteMap;

namespace Subtext.Web.SiteMap
{
    /// <summary>
    /// Your blog sitemap that search engines can use to decide how to index your site.
    /// </summary>    
    public class SiteMapHttpHandler : IHttpHandler
    {
        public void ProcessRequest(HttpContext context)
        {
            context.Response.ContentType = "text/xml";

            UrlCollection urlCollection = new UrlCollection();

            // Let's add home page
            Url homePage = new Url(Config.CurrentBlog.HomeFullyQualifiedUrl, DateTime.Now, ChangeFrequency.Daily, 1.0M);
            urlCollection.Add(homePage);

            // then all the entries
            ICollection<Entry> posts = Entries.GetRecentPosts(0, PostType.BlogPost, PostConfig.IsActive, false);
            if (posts != null)
            {
                foreach (Entry post in posts)
                {
                    ChangeFrequency frequency = CalculateFrequency(post);
                    urlCollection.Add(
                        new Url(post.FullyQualifiedUrl, post.DateModified,
                                frequency, 0.8M));
                }
            }

            // all articles
            ICollection<Entry> stories = Entries.GetRecentPosts(0, PostType.Story, PostConfig.IsActive, false);
            if (stories != null)
            {
                foreach (Entry story in stories)
                {
                    ChangeFrequency frequency = CalculateFrequency(story);
                    urlCollection.Add(
                        new Url(story.FullyQualifiedUrl, story.DateModified,
                                frequency, 0.8M));
                }
            }

            // categories
            LinkCategory categories = Transformer.BuildLinks("", CategoryType.PostCollection, new UrlFormats(Config.CurrentBlog.RootUrl));
            if (categories != null)
            {
                foreach (Link category in categories.Links)
                {
                    urlCollection.Add(
                        new Url(new Uri("http://" + Config.CurrentBlog.Host + category.Url), DateTime.Today,
                                ChangeFrequency.Weekly, 0.6M));
                }
            }

            // archives
            // categories            
            LinkCategory archives = Transformer.BuildMonthLinks("",new UrlFormats(Config.CurrentBlog.RootUrl));
            if (archives != null)
            {
                foreach (Link archive in archives.Links)
                {
                    urlCollection.Add(
                        new Url(
                            new Uri("http://" + Config.CurrentBlog.Host + archive.Url), DateTime.Today, ChangeFrequency.Weekly, 0.6M));
                }
            }

            // don't index contact form
            urlCollection.Add(new Url(new Uri(string.Format(System.Globalization.CultureInfo.InvariantCulture, "{0}contact.aspx", Config.CurrentBlog.HostFullyQualifiedUrl)), DateTime.Today, ChangeFrequency.Never, 0.0M));
            XmlSerializer serializer = new XmlSerializer(typeof(UrlCollection));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.OutputStream, Encoding.UTF8);
            serializer.Serialize(xmlTextWriter, urlCollection);            

        }

        private static ChangeFrequency CalculateFrequency(Entry entry)
        {
            ChangeFrequency frequency = ChangeFrequency.Hourly;
            if (entry.DateModified < DateTime.Now.AddMonths(-12))
            {
                frequency = ChangeFrequency.Yearly;
            }
            else if (entry.DateModified < DateTime.Now.AddDays(-60))
            {
                frequency = ChangeFrequency.Monthly;
            }
            else if (entry.DateModified < DateTime.Now.AddDays(-14))
            {
                frequency = ChangeFrequency.Weekly;
            }
            else if (entry.DateModified < DateTime.Now.AddDays(-2))
            {
                frequency = ChangeFrequency.Daily;
            }
            return frequency;
        }

        public bool IsReusable
        {
            get
            {
                return false;
            }
        }
    }

}