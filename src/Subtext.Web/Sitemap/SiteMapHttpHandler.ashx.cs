#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.Handlers;
using Subtext.Web.UI;

namespace Subtext.Web.SiteMap
{
    /// <summary>
    /// Your blog sitemap that search engines can use to decide how to index your site.
    /// </summary>    
    public class SiteMapHttpHandler : SubtextHttpHandler
    {
        public SiteMapHttpHandler(ISubtextContext subtextContext)
            : base(subtextContext)
        {
        }

        public override void ProcessRequest()
        {
            HttpContextBase context = SubtextContext.HttpContext;
            context.Response.ContentType = "text/xml";

            var urlCollection = new UrlCollection();

            // Let's add home page
            var homePage = new UrlElement(Url.BlogUrl().ToFullyQualifiedUrl(Blog), DateTime.UtcNow, ChangeFrequency.Daily, 1.0M);
            urlCollection.Add(homePage);

            // then all the entries

            ICollection<Entry> posts = Repository.GetEntries(0, PostType.BlogPost, PostConfig.IsActive, false
                /* includeCategories */);
            if (posts != null)
            {
                foreach (Entry post in posts)
                {
                    ChangeFrequency frequency = CalculateFrequency(post);
                    urlCollection.Add(
                        new UrlElement(Url.EntryUrl(post).ToFullyQualifiedUrl(Blog), post.DateModifiedUtc,
                                       frequency, 0.8M));
                }
            }

            // all articles
            ICollection<Entry> stories = Repository.GetEntries(0, PostType.Story, PostConfig.IsActive, false
                /* includeCategories */);
            if (stories != null)
            {
                foreach (Entry story in stories)
                {
                    ChangeFrequency frequency = CalculateFrequency(story);
                    urlCollection.Add(
                        new UrlElement(Url.EntryUrl(story).ToFullyQualifiedUrl(Blog),
                                       story.DateModifiedUtc,
                                       frequency, 0.8M));
                }
            }

            // categories
            ICollection<LinkCategory> links = Repository.GetCategories(CategoryType.PostCollection, true
                /* activeOnly */);
            LinkCategory categories = Transformer.MergeLinkCategoriesIntoSingleLinkCategory(string.Empty /* title */,
                                                                                            CategoryType.PostCollection,
                                                                                            links, Url, Blog);
            if (categories != null)
            {
                foreach (Link category in categories.Links)
                {
                    urlCollection.Add(
                        new UrlElement(new Uri(Url.BlogUrl().ToFullyQualifiedUrl(Blog) + category.Url),
                                       DateTime.Today,
                                       ChangeFrequency.Weekly, 0.6M));
                }
            }

            // archives
            // categories            
            ICollection<ArchiveCount> archiveCounts = Repository.GetPostCountsByMonth();
            LinkCategory archives = archiveCounts.MergeIntoLinkCategory(string.Empty, Url, Blog);
            if (archives != null)
            {
                foreach (Link archive in archives.Links)
                {
                    urlCollection.Add(
                        new UrlElement(
                            new Uri(Url.BlogUrl().ToFullyQualifiedUrl(Blog) + archive.Url), DateTime.Today,
                            ChangeFrequency.Weekly, 0.6M));
                }
            }

            // don't index contact form
            urlCollection.Add(new UrlElement(Url.ContactFormUrl().ToFullyQualifiedUrl(Blog), DateTime.Today,
                                             ChangeFrequency.Never, 0.0M));
            var serializer = new XmlSerializer(typeof(UrlCollection));
            var xmlTextWriter = new XmlTextWriter(context.Response.Output);
            serializer.Serialize(xmlTextWriter, urlCollection);
        }

        private static ChangeFrequency CalculateFrequency(Entry entry)
        {
            ChangeFrequency frequency = ChangeFrequency.Hourly;
            if (entry.DateModifiedUtc < DateTime.UtcNow.AddMonths(-12))
            {
                frequency = ChangeFrequency.Yearly;
            }
            else if (entry.DateModifiedUtc < DateTime.UtcNow.AddDays(-60))
            {
                frequency = ChangeFrequency.Monthly;
            }
            else if (entry.DateModifiedUtc < DateTime.UtcNow.AddDays(-14))
            {
                frequency = ChangeFrequency.Weekly;
            }
            else if (entry.DateModifiedUtc < DateTime.UtcNow.AddDays(-2))
            {
                frequency = ChangeFrequency.Daily;
            }
            return frequency;
        }
    }
}