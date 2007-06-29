using System;
using Subtext.Extensibility.Attributes;
using Subtext.Extensibility.Plugins;
using Subtext.Framework.Components;
using Subtext.Extensibility;
using Subtext.Framework.Configuration;
using Subtext.Framework;

namespace Subtext.Plugins.Core.CommunityCredits
{
	[Identifier("{b827d206-c510-443c-9675-5c6960f02591}")]
	[Description("CommunityCredits",
		Author = "Simone Chiaretta",
		Company = "Subtext",
		Copyright="(c) 2006",
		HomePageUrl = "http://www.subtextproject.com/",
		Version = "0.0.1",
		Description = "This plugin automatically submits to Community Credits the posts you make so you don't have to do it manually")]
	public class CommunityCreditsSubmission: PluginBase
	{
		public override void Init(SubtextApplication application)
		{
			application.EntryUpdated += new EventHandler<EntryEventArgs>(application_EntryUpdated);
		}

		void application_EntryUpdated(object sender, EntryEventArgs e)
		{
			string result;
			if (e.State == ObjectState.Create)
			{
				Entry entry = e.Entry; 

				com.community_credit.www.AffiliateServices wsCommunityCredit = new com.community_credit.www.AffiliateServices();
				wsCommunityCredit.Url = GetDefaultSetting("WebServiceUrl");

				string url = entry.FullyQualifiedUrl.ToString();
				string category = String.Empty;
				if (entry.PostType == PostType.BlogPost)
                    category = GetDefaultSetting("Post_SubmissionCategoryName");
				else if (entry.PostType == PostType.Story)
					category = GetDefaultSetting("Story_SubmissionCategoryName");
				string description = "Blogged about: " + entry.Title;
				BlogInfo info = Config.CurrentBlog;
				string firstName = string.Empty;
				string lastName = info.Author;
				string email = info.Owner.Email;
                string affiliateCode = getEffectiveAffiliateCode(GetBlogSetting("AffiliateCode"));
                string affiliateKey = getEffectiveAffiliateKey(GetBlogSetting("AffiliateKey"));

				try
				{
					result=wsCommunityCredit.AddCommunityCredit(email, firstName, lastName, description, url, category, affiliateCode, affiliateKey);
					if (!result.Equals("Success"))
					{
						((INotifiableControl)sender).ShowError("Error during Community Credits submission (your post has been saved): <br/>\r\nCommunity Server webservice returned the following error code: <br/>\r\n" + result);
					}
				}
				catch (Exception ex)
				{
					((INotifiableControl)sender).ShowError("Error during Community Credits submission (your post has been saved): <br/>\r\n" + ex.Message);
				}
			}
		}

		private string getEffectiveAffiliateCode(string overiddenCode)
		{
			if (String.IsNullOrEmpty(overiddenCode))
                return GetDefaultSetting("AffiliateCode");
			else
				return overiddenCode;
		}

		private string getEffectiveAffiliateKey(string overiddenKey)
		{
			if (String.IsNullOrEmpty(overiddenKey))
                return GetDefaultSetting("AffiliateKey");
			else
				return overiddenKey;
		}
	}
}
