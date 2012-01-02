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
using System.Configuration;
using Subtext.Extensibility;
using Subtext.Framework.com.community_credit.www;
using Subtext.Framework.Components;
using Subtext.Framework.Logging;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Tracking
{
    public static class CommunityCreditNotification
    {
        static readonly Log Log = new Log();

        public static void AddCommunityCredits(Entry entry, BlogUrlHelper urlHelper, Blog blog)
        {
            string result;

            bool commCreditsEnabled;

            if (!bool.TryParse(ConfigurationManager.AppSettings["CommCreditEnabled"], out commCreditsEnabled))
            {
                return;
            }

            if (commCreditsEnabled && entry.IsActive)
            {
                var wsCommunityCredit = new AffiliateServices();

                string url = urlHelper.EntryUrl(entry).ToFullyQualifiedUrl(blog).ToString();
                string category = String.Empty;
                if (entry.PostType == PostType.BlogPost)
                {
                    category = "Blog";
                }
                else if (entry.PostType == PostType.Story)
                {
                    category = "Article";
                }
                string description = "Blogged about: " + entry.Title;

                string firstName = string.Empty;
                string lastName = blog.Author;
                string email = blog.Email;
                string affiliateCode = ConfigurationManager.AppSettings["CommCreditAffiliateCode"];
                string affiliateKey = ConfigurationManager.AppSettings["CommCreditAffiliateKey"];

                Log.InfoFormat("Sending notification to community credit for url {0} in category {1} for user {2}", url,
                               category, email);

                result = wsCommunityCredit.AddCommunityCredit(email, firstName, lastName, description, url, category,
                                                              affiliateCode, affiliateKey);

                Log.InfoFormat("Response Received was: {0}", result);
                if (!result.Equals("Success"))
                {
                    throw new CommunityCreditNotificationException(result);
                }
            }
        }
    }
}