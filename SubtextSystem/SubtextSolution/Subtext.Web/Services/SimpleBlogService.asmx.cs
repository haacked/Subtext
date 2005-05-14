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
using System.Collections;
using System.ComponentModel;
using System.Web.Services;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;

namespace Subtext.Web.Services
{
	/// <summary>
	/// Summary description for SimpleBlogService.
	/// </summary>
	[ WebService(Name="SBS (Simple Blog Service)",Description="Simple ASPNETWebLog Blog Posting Service",Namespace="http://www.aspnetweblog.com/services/simpleblogservice/")]
	public class SBS : System.Web.Services.WebService
	{
		public SBS()
		{
			//CODEGEN: This call is required by the ASP.NET Web Services Designer
			InitializeComponent();
			
		}

		#region Component Designer generated code
		
		//Required by the Web Services Designer 
		private IContainer components = null;
				
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
			if(disposing && components != null)
			{
				components.Dispose();
			}
			base.Dispose(disposing);		
		}
		
		#endregion

		private void CheckUser(string username, string password)
		{
			BlogConfig config = Config.CurrentBlog;
			
			if(!Security.IsValidUser(username,password))
			{
				throw new Exception("Unknown User. Try another UserName/Password...or go away");
			}
		}

		[WebMethod(MessageName="InsertCategoryPost",Description="Insert a Post with Categories",EnableSession=false)]
		public int InsertCategoryPost(string username, string password, DateTime postdate, string title, string body, string[] categories)
		{
			return InsertCategoryPost(username,password,postdate,title,body,null,categories);
		}

		[WebMethod(MessageName="InsertCategoryPostWithUrl",Description="Insert a Post with Categories",EnableSession=false)]
		public int InsertCategoryPost(string username, string password, DateTime postdate, string title, string body, string url, string[] categories)
		{
				BlogConfig config = Config.CurrentBlog;
				CheckUser(username,password);
			CategoryEntry entry = new CategoryEntry();
				entry.PostType = PostType.BlogPost;
				entry.Title = title;
				entry.Body = body;
				entry.TitleUrl = url;
				entry.DateCreated = postdate.AddHours(BlogTime.ServerToClientTimeZoneFactor);
				entry.DateUpdated = entry.DateCreated;
				entry.IsActive = true;
				entry.AllowComments = true;
				entry.DisplayOnHomePage = true;
				entry.IncludeInMainSyndication = true;
				entry.IsAggregated = true;
				entry.SyndicateDescriptionOnly = false;
				entry.Author = config.Author;
				entry.Email = config.Email;
				entry.Categories = categories;
				return Entries.Create(entry);			
		}

		[WebMethod(MessageName="SimplePost",Description="Add a new Blog Entry",EnableSession=false)]
		public int InsertPost(string username, string password, DateTime postdate, string title, string body)
		{
			return InsertPost(username,password,postdate,title,body,null);
		}

		[WebMethod(MessageName="SimplePostUrl",Description="Add a new Blog Entry",EnableSession=false)]
		public int InsertPost(string username, string password, DateTime postdate, string title, string body, string url)
		{
			CheckUser(username,password);
			BlogConfig config = Config.CurrentBlog;
			Entry entry = new Entry(PostType.BlogPost);
			entry.Title = title;
			entry.Body = body;
			entry.TitleUrl = url;
			entry.DateCreated = postdate.AddHours(BlogTime.ServerToClientTimeZoneFactor);
			entry.IsActive = true;
			entry.AllowComments = true;
			entry.DisplayOnHomePage = true;
			entry.IncludeInMainSyndication = true;
			entry.IsAggregated = true;
			entry.SyndicateDescriptionOnly = false;
			entry.Author = config.Author;
			entry.Email = config.Email;
			return Entries.Create(entry);			

		}

		[WebMethod(MessageName="SimpleEntryUpdateUrl",Description="Update existing entry",EnableSession=false)]
		public bool Edit(int entryid, string username, string password, DateTime postupdate, string title, string body, string url)
		{
			CheckUser(username,password);
			Entry entry = Entries.GetEntry(entryid,false);
			if(entry != null)
			{
				entry.Title = title;
				entry.Body = body;
				entry.TitleUrl = url;
				entry.DateUpdated = postupdate.AddHours(BlogTime.ServerToClientTimeZoneFactor);
				entry.IsActive = true;
				return Entries.Update(entry);
			}

			return false;
			
		}

		[WebMethod(MessageName="SimpleEntryUpdate",Description="Update existing entry",EnableSession=false)]
		public bool Edit(int entryid, string username, string password, DateTime postupdate, string title, string body)
		{
			return Edit(entryid,username,password,postupdate,title,body,null);
		}

		[WebMethod(MessageName="GetCategories",Description="A list of Post Categories",EnableSession=false)]
		public string[] GetCategories(string username, string password)
		{
			CheckUser(username,password);
			LinkCategoryCollection lcc = Links.GetCategories(CategoryType.PostCollection,false);
			ArrayList al = new ArrayList();
			foreach(LinkCategory lc in lcc)
			{
				al.Add(lc.Title);
			}
			return (string[])al.ToArray(typeof(string));
		}

	}
}

