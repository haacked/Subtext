#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.ComponentModel;
using System.Web.Services;
using System.Web.Services.Protocols;
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
			try
			{
				if(ValidateUser())
				{
					Entries.SetEntryCategoryList(PostID,CategoryIDs);
					return true;
				}
				return false;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="GetLinkCollectionByPostID",Description="The entry collection methods do not return each Entry and/or Story's categories. This method will return the category collection for the specified Entry/Story",EnableSession=false)]
		public LinkCollection GetLinkCollectionByPostID(int PostID)
		{
			try
			{
				if(ValidateUser())
				{
					return Links.GetLinkCollectionByPostID(PostID);
				}
				return null;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]		
		[WebMethod(MessageName="GetCategories",Description="Returns the categories available for the specificid CategoryType (PostCollection or StoryCollection)",EnableSession=false)]
		public LinkCategoryCollection GetCategories(CategoryType CategoryType)
		{
			try
			{
				if(ValidateUser())
				{
					return Links.GetCategories(CategoryType,false);
				}
				return null;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		#endregion

		#region Update Helpers
		private int InsertEntryCheck(Entry entry, int[] Categories)
		{
			try
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
			catch(Exception e)
			{
				throw e;
			}
		}


		private bool UpdateEntryCheck(Entry entry, int[] Categories)
		{
			try
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
			catch(Exception e)
			{
				throw e;
			}
		}

		#endregion

		#region ResetDates

		private void ResetEntryCollectionDates(ref EntryCollection ec)
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

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="GetRecentUpdatedPosts",Description="Returns the most recent posts")]
		public EntryCollection GetRecentPosts(int ItemCount, DateTime LastUpdatedDate)
		{
			try
			{
				if(ValidateUser())
				{
					EntryCollection ec = Entries.GetRecentPosts(ItemCount,PostType.BlogPost,false,LastUpdatedDate.AddHours(BlogTime.ServerToClientTimeZoneFactor));
					ResetEntryCollectionDates(ref ec);
					return ec;
				}
				return null;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="GetRecentPosts",Description="Returns the most recent posts")]
		public EntryCollection GetRecentPosts(int ItemCount)
		{
			try
			{
				if(ValidateUser())
				{
					EntryCollection ec =  Entries.GetRecentPosts(ItemCount,PostType.BlogPost,false);
					ResetEntryCollectionDates(ref ec);
					return ec;
				}
				return null;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

//		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
//		[WebMethod(MessageName="GetStories",Description="Returns an EntryCollection with all of the current stories",EnableSession=false)]
//		public EntryCollection GetStories()
//		{
//			try
//			{
//				if(ValidateUser())
//				{
//					EntryCollection ec = Entries.GetAllStoreis(false);
//					ResetEntryCollectionDates(ref ec);
//					return ec;
//				}
//				return null;
//			}
//			catch(Exception e)
//			{
//				throw e;
//			}
//		}
//
//		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
//		[WebMethod(MessageName="GetUpdatedStories",Description="Returns an EntryCollection with all of the current stories",EnableSession=false)]
//		public EntryCollection GetStories(DateTime UpdatedSince)
//		{
//			try
//			{
//				if(ValidateUser())
//				{
//					EntryCollection ec = Entries.GetAllStoreis(false,UpdatedSince.AddHours(BlogTime.ServerToClientTimeZoneFactor));
//					ResetEntryCollectionDates(ref ec);
//					return ec;
//				}
//				return null;
//			}
//			catch(Exception e)
//			{
//				throw e;
//			}
//		}

		#endregion

		#region Delete Posts

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="DeleteEntry",Description="Remove an entry and/or story",EnableSession=false)]
		public bool DeleteEntry(int PostID)
		{
			try
			{
				if(ValidateUser())
				{
					Entries.Delete(PostID);
					return true;
				}
				return false;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		#endregion

		#region Update Categories and Links

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="InsertCategory",Description="Insert a new LinkCateogry",EnableSession=false)]
		public int InsertCategory(LinkCategory lc)
		{
			try
			{
				if(ValidateUser())
				{
					return Links.CreateLinkCategory(lc);
				}
				return -1;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="UpdateCategory",Description="Update a LinkCateogry",EnableSession=false)]
		public void UpdateCategory(LinkCategory lc)
		{
			try
			{
				if(ValidateUser())
				{
					Links.UpdateLinkCategory(lc);
				}
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="InsertLink",Description="Insert a new Link",EnableSession=false)]
		public int InsertLink(Link _link)
		{
			try
			{
				if(ValidateUser())
				{
					return Links.CreateLink(_link);
				}
				return -1;
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		[SoapHeaderAttribute("ASPNetWebLogUser", Direction=SoapHeaderDirection.InOut)]
		[WebMethod(MessageName="UpdateLink",Description="Update a Link",EnableSession=false)]
		public void UpdateLink(Link _link)
		{
			try
			{
				if(ValidateUser())
				{
					Links.UpdateLink(_link);
				}
			}
			catch(Exception e)
			{
				throw e;
			}
		}

		#endregion


	}
}

