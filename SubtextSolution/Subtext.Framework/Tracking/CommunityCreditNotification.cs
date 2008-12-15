using System;
using System.Configuration;
using System.Collections.Generic;
using System.Text;
using Subtext;
using Subtext.Framework.Components;
using Subtext.Extensibility;
using Subtext.Framework.Configuration;
using Subtext.Framework.Logging;

namespace Subtext.Framework.Tracking
{
   public static class CommunityCreditNotification
   {
      static Log Log = new Log();

      public static void AddCommunityCredits(Entry entry)
      {
         string result = string.Empty;
            
         bool commCreditsEnabled;

         if (!bool.TryParse(ConfigurationManager.AppSettings["CommCreditEnabled"], out commCreditsEnabled))
             return;

         if (commCreditsEnabled && entry.IsActive)
         {
            com.community_credit.www.AffiliateServices wsCommunityCredit = new com.community_credit.www.AffiliateServices();
            string url = entry.FullyQualifiedUrl.ToString();
            string category = String.Empty;
            if (entry.PostType == PostType.BlogPost)
               category = "Blog";
            else if (entry.PostType == PostType.Story)
               category = "Article";
            string description = "Blogged about: " + entry.Title;
            BlogInfo info = Config.CurrentBlog;
            string firstName = string.Empty;
            string lastName = info.Author;
            string email = info.Email;
            string affiliateCode = System.Configuration.ConfigurationManager.AppSettings["CommCreditAffiliateCode"];
            string affiliateKey = System.Configuration.ConfigurationManager.AppSettings["CommCreditAffiliateKey"];

            Log.InfoFormat("Sending notification to community credit for url {0} in category {1} for user {2}", url, category, email);

            result = wsCommunityCredit.AddCommunityCredit(email, firstName, lastName, description, url, category, affiliateCode, affiliateKey);

            Log.InfoFormat("Response Received was: {0}",result);
            if (!result.Equals("Success"))
            {
               throw new CommunityCreditNotificationException(result);
            }
         }
      }
   }
}
