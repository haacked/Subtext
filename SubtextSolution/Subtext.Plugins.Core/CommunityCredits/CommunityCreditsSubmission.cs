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
			application.EntryUpdated += new EventHandler<SubtextEventArgs>(application_EntryUpdated);
		}

		void application_EntryUpdated(object sender, SubtextEventArgs e)
		{
			string result;
			if (e.State == ObjectState.Create)
			{
				Entry entry = (Entry)sender; 

				com.community_credit.www.AffiliateServices wsCommunityCredit = new com.community_credit.www.AffiliateServices();
				wsCommunityCredit.Url = DefaultSettings["WebServiceUrl"];

				string url = entry.FullyQualifiedUrl.ToString();
				string category = String.Empty;
				if (entry.PostType == PostType.BlogPost)
					category = DefaultSettings["Post_SubmissionCategoryName"];
				else if (entry.PostType == PostType.Story)
					category = DefaultSettings["Story_SubmissionCategoryName"];
				string description = "Blogged about: " + entry.Title;
				BlogInfo info = Config.CurrentBlog;
				string firstName = string.Empty;
				string lastName = info.Author;
				string email = info.Owner.Email;
				string affiliateCode = getEffectiveAffiliateCode(e.BlogSettings["AffiliateCode"]);
				string affiliateKey = getEffectiveAffiliateKey(e.BlogSettings["AffiliateKey"]);

				try
				{
					result=wsCommunityCredit.AddCommunityCredit(email, firstName, lastName, description, url, category, affiliateCode, affiliateKey);
				}
				catch (Exception ex)
				{
					//this.Messages.ShowError(String.Format(Constants.RES_EXCEPTION, "Error during Community Credits submission (your post has been saved)", ex.Message));
				}	
			}
		}

		private string getEffectiveAffiliateCode(string overiddenCode)
		{
			if (String.IsNullOrEmpty(overiddenCode))
				return DefaultSettings["AffiliateCode"];
			else
				return overiddenCode;
		}

		private string getEffectiveAffiliateKey(string overiddenKey)
		{
			if (String.IsNullOrEmpty(overiddenKey))
				return DefaultSettings["AffiliateKey"];
			else
				return overiddenKey;
		}
	}
}
