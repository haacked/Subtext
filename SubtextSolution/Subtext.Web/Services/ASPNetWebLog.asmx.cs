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
using System.Collections.Generic;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
using Subtext.Extensibility;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;

namespace Subtext.Web.Services
{
	/// <summary>
	/// Summary description for Subtext.
	/// </summary>
	[ WebService(Name="ASPNetWebLogApi",Description="Dottext Api (Beta 1)",Namespace="http://www.aspnetweblog.com/services/aspnetweblogapi/")]
	public class ASPNetWebLogApi : System.Web.Services.WebService
	{
		public BlogUser ASPNetWebLogUser;
		public ASPNetWebLogApi()
		{
			ASPNetWebLogUser = new BlogUser();
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer Framework = null;
				
		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose( bool disposing )
		{
			if(disposing && Framework != null)
			{
				Framework.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		#region Valiation

		private bool ValidateUser()
		{
			if(ASPNetWebLogUser != null)
			{
				return  Security.IsValidUser(ASPNetWebLogUser.UserName,ASPNetWebLogUser.Password);
			}
			return false;
		}

		#endregion

		#region Categories/Links

		[SoapHeader("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="AddPostToCategories",Description="Set the categories by PostID",EnableSession=false)]
		public bool AddPostToCategories(int PostID,int[] CategoryIDs)
		{			
			if(ValidateUser())
			{
				Entries.SetEntryCategoryList(PostID,CategoryIDs);
				return true;
			}
			return false;
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="GetLinkCollectionByPostID",Description="The entry collection methods do not return each Entry and/or Story's categories. This method will return the category collection for the specified Entry/Story",EnableSession=false)]
		public ICollection<Link> GetLinkCollectionByPostID(int PostID)
		{
			if(ValidateUser())
			{
				return Links.GetLinkCollectionByPostID(PostID);
			}
			return null;
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]		
		[WebMethod(MessageName="GetCategories",Description="Returns the categories available for the specificid CategoryType (PostCollection or StoryCollection)",EnableSession=false)]
        public ICollection<LinkCategory> GetCategories(CategoryType CategoryType)
		{
			
			if(ValidateUser())
			{
                return Links.GetCategories(CategoryType, ActiveFilter.None);
			}
			return null;
		}

		#endregion

		#region Update Helpers
		private int InsertEntryCheck(Entry entry, int[] Categories)
		{
			if(ValidateUser())
			{
				
				if(entry.Author == null)
				{
					entry.Author = Config.CurrentBlog.Author;
				}
				if(entry.Email == null)
				{
					entry.Email = Config.CurrentBlog.Email;
				}
				entry.DateCreated.AddHours(BlogTime.ServerToClientTimeZoneFactor);

				return Entries.Create(entry,Categories);
			}
			return -1;			
		}


		private bool UpdateEntryCheck(Entry entry, int[] Categories)
		{
			if(ValidateUser())
			{
				if(entry.Author == null)
				{
					entry.Author = Config.CurrentBlog.Author;
				}
				if(entry.Email == null)
				{
					entry.Email = Config.CurrentBlog.Email;
				}
				entry.DateUpdated.AddHours(BlogTime.ServerToClientTimeZoneFactor);
				Entries.Update(entry,Categories);
				return true;
			}
			return false;
		}

		#endregion

		#region ResetDates

        private void ResetEntryCollectionDates(IList<Entry> ec)
		{
			if(ec != null && ec.Count > 0)
			{
				int offset = BlogTime.ServerToClientTimeZoneFactor;
				int count = ec.Count;
				for(int i = 0; i<count; i++)
				{
					ec[i].DateUpdated = ec[i].DateUpdated.AddHours(offset);
					ec[i].DateCreated = ec[i].DateCreated.AddHours(offset);
				}
			}
		}

		#endregion

		#region Insert/Update Methods

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="InsertEntry",Description="Insert a new Entry",EnableSession=false)]
		public int InsertEntry(Entry entry)
		{
			return InsertEntryCheck(entry,null);
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="InsertEntryWithCategories",Description="Add a new Blog Entry with a set of categories (LinkCollection)",EnableSession=false)]
		public int InsertEntry(Entry entry, int[] lc)
		{
			return InsertEntryCheck(entry,lc);
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="UpdateEntry",Description="Update an existing entry",EnableSession=false)]
		public bool UpdateEntry(Entry entry)
		{
			return UpdateEntryCheck(entry,null);
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="UpdateEntryWithCategories",Description="Update an existing entry and its categories",EnableSession=false)]
		public bool UpdateEntry(Entry entry, int[] Categories)
		{
			return UpdateEntryCheck(entry,Categories);
		}

	
		#endregion

		#region Post Collections

		/// <summary>
		/// Gets recent posts for the MetaBlogAPI.
		/// </summary>
		/// <param name="ItemCount">Item count.</param>
		/// <param name="LastUpdatedDate">Last updated date.</param>
		/// <returns></returns>
		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="GetRecentUpdatedPosts",Description="Returns the most recent posts")]
        public IList<Entry> GetRecentPosts(int ItemCount, DateTime LastUpdatedDate)
		{
			if(ValidateUser())
			{
                IList<Entry> ec = Entries.GetRecentPosts(ItemCount, PostType.BlogPost, false, LastUpdatedDate.AddHours(BlogTime.ServerToClientTimeZoneFactor));
				ResetEntryCollectionDates(ec);
				return ec;
			}
			return null;
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="GetRecentPosts",Description="Returns the most recent posts")]
		public IList<Entry> GetRecentPosts(int ItemCount)
		{
			if(ValidateUser())
			{
                IList<Entry> ec = Entries.GetRecentPosts(ItemCount, PostType.BlogPost, false);
				ResetEntryCollectionDates(ec);
				return ec;
			}
			return null;
		}

		#endregion

		#region Delete Posts

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="DeleteEntry",Description="Remove an entry and/or story",EnableSession=false)]
		public bool DeleteEntry(int PostID)
		{
			if(ValidateUser())
			{
				Entries.Delete(PostID);
				return true;
			}
			return false;
		}

		#endregion

		#region Update Categories and Links

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="InsertCategory",Description="Insert a new LinkCateogry",EnableSession=false)]
		public int InsertCategory(LinkCategory lc)
		{
			if(ValidateUser())
			{
				return Links.CreateLinkCategory(lc);
			}
			return -1;
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="UpdateCategory",Description="Update a LinkCateogry",EnableSession=false)]
		public void UpdateCategory(LinkCategory lc)
		{
			if(ValidateUser())
			{
				Links.UpdateLinkCategory(lc);
			}
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="InsertLink",Description="Insert a new Link",EnableSession=false)]
		public int InsertLink(Link _link)
		{
			if(ValidateUser())
			{
				return Links.CreateLink(_link);
			}
			return -1;
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="UpdateLink",Description="Update a Link",EnableSession=false)]
		public void UpdateLink(Link _link)
		{
			if(ValidateUser())
			{
				Links.UpdateLink(_link);
			}
		}

		#endregion


	}
}

