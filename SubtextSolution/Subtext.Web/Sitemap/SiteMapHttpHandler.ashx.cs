using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.Routing;
using System.Xml;
using System.Xml.Serialization;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;
using Subtext.Framework.Routing;

namespace Subtext.Web.SiteMap
{
    /// <summary>
    /// Your blog sitemap that search engines can use to decide how to index your site.
    /// </summary>    
    public class SiteMapHttpHandler : ISubtextHandler
    {
        void IHttpHandler.ProcessRequest(HttpContext context) {
            ProcessRequest(SubtextContext);
        }

        public void ProcessRequest(ISubtextContext subtextContext)
        {
            Url = subtextContext.UrlHelper;

            HttpContextBase context = subtextContext.RequestContext.HttpContext;
            ObjectProvider repository = subtextContext.Repository;

            context.Response.ContentType = "text/xml";

            UrlCollection urlCollection = new UrlCollection();

            // Let's add home page
            UrlElement homePage = new UrlElement(Url.BlogUrl().ToFullyQualifiedUrl(subtextContext.Blog), DateTime.Now, ChangeFrequency.Daily, 1.0M);
            urlCollection.Add(homePage);

            // then all the entries

            ICollection<Entry> posts = repository.GetEntries(0, PostType.BlogPost, PostConfig.IsActive, false /* includeCategories */);
            if (posts != null)
            {
                foreach (Entry post in posts)
                {
                    ChangeFrequency frequency = CalculateFrequency(post);
                    urlCollection.Add(
                        new UrlElement(Url.EntryUrl(post).ToFullyQualifiedUrl(subtextContext.Blog), post.DateModified,
                                frequency, 0.8M));
                }
            }

            // all articles
            ICollection<Entry> stories = repository.GetEntries(0, PostType.Story, PostConfig.IsActive, false /* includeCategories */);
            if (stories != null)
            {
                foreach (Entry story in stories)
                {
                    ChangeFrequency frequency = CalculateFrequency(story);
                    urlCollection.Add(
                        new UrlElement(Url.EntryUrl(story).ToFullyQualifiedUrl(subtextContext.Blog), 
                            story.DateModified,
                            frequency, 0.8M));
                }
            }

            // categories
            var links = repository.GetCategories(CategoryType.PostCollection, true /* activeOnly */);
            LinkCategory categories = Transformer.MergeLinkCategoriesIntoSingleLinkCategory(string.Empty /* title */, CategoryType.PostCollection, links, Url, subtextContext.Blog);
            if (categories != null)
            {
                foreach (Link category in categories.Links)
                {
                    urlCollection.Add(
                        new UrlElement(new Uri(Url.BlogUrl().ToFullyQualifiedUrl(subtextContext.Blog).ToString() + category.Url), 
                            DateTime.Today,
                            ChangeFrequency.Weekly, 0.6M));
                }
            }

            // archives
            // categories            
            ICollection<ArchiveCount> archiveCounts = repository.GetPostCountsByMonth();
            LinkCategory archives = Transformer.MergeArchiveCountsIntoLinkCategory(string.Empty, archiveCounts, Url, subtextContext.Blog);
            if (archives != null)
            {
                foreach (Link archive in archives.Links)
                {
                    urlCollection.Add(
                        new UrlElement(
                            new Uri(Url.BlogUrl().ToFullyQualifiedUrl(subtextContext.Blog).ToString() + archive.Url), DateTime.Today, ChangeFrequency.Weekly, 0.6M));
                }
            }

            // don't index contact form
            urlCollection.Add(new UrlElement(Url.ContactFormUrl().ToFullyQualifiedUrl(subtextContext.Blog), DateTime.Today, ChangeFrequency.Never, 0.0M));
            XmlSerializer serializer = new XmlSerializer(typeof(UrlCollection));
            XmlTextWriter xmlTextWriter = new XmlTextWriter(context.Response.Output);
            serializer.Serialize(xmlTextWriter, urlCollection);
        }

        private static ChangeFrequency CalculateFrequency(Entry entry) {
            ChangeFrequency frequency = ChangeFrequency.Hourly;
            if (entry.DateModified < DateTime.Now.AddMonths(-12)) {
                frequency = ChangeFrequency.Yearly;
            }
            else if (entry.DateModified < DateTime.Now.AddDays(-60)) {
                frequency = ChangeFrequency.Monthly;
            }
            else if (entry.DateModified < DateTime.Now.AddDays(-14)) {
                frequency = ChangeFrequency.Weekly;
            }
            else if (entry.DateModified < DateTime.Now.AddDays(-2)) {
                frequency = ChangeFrequency.Daily;
            }
            return frequency;
        }

        bool IHttpHandler.IsReusable {
            get {
                return false;
            }
        }

        public RequestContext RequestContext
        {
            get {
                return SubtextContext.RequestContext;
            }
            set {
            }
        }

        public UrlHelper Url
        {
            get;
            private set;
        }

        public ISubtextContext SubtextContext
        {
            get;
            set;
        }
    }

}