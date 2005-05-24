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
using CookComputing.XmlRpc;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;

//Need to find a method that has access to context, so we can terminate the request if AllowServiceAccess == false.
//Users will be able to access the metablogapi page, but will not be able to make a request, but the page should not be visible

namespace Subtext.Framework.XmlRpc
{
	/// <summary>
	/// Summary description for MetaWeblog.
	/// </summary>
	public class MetaWeblog : XmlRpcService, Subtext.Framework.XmlRpc.IMetaWeblog
	{
		public MetaWeblog()
		{
			
		}

		private bool ValidateUser(string username, string password, bool AllowServiceAccess)
		{
			return (Config.Settings.AllowServiceAccess && AllowServiceAccess && Security.IsValidUser(username,password));
		}

		#region BlogApi Members
		public BlogInfo[] getUsersBlogs(string appKey,string username,string password)
		{
			Framework.BlogInfo info = Config.CurrentBlog;
			if(ValidateUser(username,password,info.AllowServiceAccess))
			{
				BlogInfo[] bi = new BlogInfo[1];
				BlogInfo b = new BlogInfo();
				b.blogid = info.BlogID.ToString();
				b.blogName = info.Title;
				b.url = info.FullyQualifiedUrl;
				bi[0]=b;
				return bi;
			}	
			throw new XmlRpcFaultException(0,"User does not exist");
		}

		public bool deletePost(string appKey,string postid,string username,string password,[XmlRpcParameter(Description="Where applicable, this specifies whether the blog should be republished after the post has been deleted.")] bool publish)
		{
			if(ValidateUser(username,password,Config.CurrentBlog.AllowServiceAccess))
			{
				try
				{
					Entries.Delete(Int32.Parse(postid));
					return true;
				}
				catch
				{
					new XmlRpcFaultException(1,"Could not delete post: " + postid);
				}
			}

			throw new XmlRpcFaultException(0,"User does not exist");
			
		}

		#endregion

		public bool editPost(string postid,	string username,string password,Post post,bool publish)
		{
			Framework.BlogInfo info = Config.CurrentBlog;
			if(ValidateUser(username,password,info.AllowServiceAccess))
			{
				CategoryEntry entry = Entries.GetCategoryEntry(Int32.Parse(postid), false);
				if(entry != null)
				{
					entry.Author = info.Author;
					entry.Email = info.Email;
					entry.Body = post.description;
					entry.Title = post.title;
					entry.TitleUrl = post.link;
					entry.SourceName = string.Empty;
					entry.SourceUrl = string.Empty;
					entry.Description = string.Empty;

					entry.Categories = post.categories;
					entry.PostType = PostType.BlogPost;
					entry.IsXHMTL = false;
					entry.IsActive = publish;
			
					entry.DateUpdated = BlogTime.CurrentBloggerTime;
					return Entries.Update(entry);
				}
				return false;
			}
			throw new XmlRpcFaultException(0,"User does not exist");
		}

		public Post getPost(string postid,string username,string password)
		{
			Framework.BlogInfo info = Config.CurrentBlog;
			if(ValidateUser(username,password,info.AllowServiceAccess))
			{
				CategoryEntry entry = Entries.GetCategoryEntry(Int32.Parse(postid),false);
				Post post = new Post();
				post.link = entry.TitleUrl;
				post.description = entry.Body;
				post.dateCreated = entry.DateCreated;
				post.postid = entry.EntryID;
				post.title = entry.Title;
				post.permalink = entry.Link;
				post.categories = entry.Categories;

				return post;
			}
			throw new XmlRpcFaultException(0,"User does not exist");

		}

		private string[] GetCategories(int postID)
		{
			LinkCollection lc = Links.GetLinkCollectionByPostID(postID);
			if(lc != null)
			{
				ArrayList al = new ArrayList();
				foreach(Link l in lc)
				{
					al.Add(l.Title);
				}
				if(al.Count > 0)
				{
					return (string[])al.ToArray(typeof(string));
				}
			}
			return null;
		}

		public Post[] getRecentPosts(string blogid, string username, string password, int numberOfPosts)
		{
			if(ValidateUser(username, password, Config.CurrentBlog.AllowServiceAccess))
			{
				EntryCollection ec = Entries.GetRecentPostsWithCategories(numberOfPosts,false);
				//int i = 0;
				int count = ec.Count;
				Post[] posts = new Post[count];
				for(int i=0;i<count;i++)
				{
					CategoryEntry entry = (CategoryEntry)ec[i];
					Post post = new Post();
					post.dateCreated = entry.DateCreated;
					post.description = entry.Body;
					post.link = entry.TitleUrl;
					post.permalink = entry.Link;
					post.title = entry.Title;
					post.postid = entry.EntryID.ToString();
					post.userid = entry.Body.GetHashCode().ToString();
					if(entry.Categories != null && entry.Categories.Length > 0)
					{
						post.categories = entry.Categories;
					}
					posts[i] = post;
				}
				return posts;
			}
		
			throw new XmlRpcFaultException(0,"User does not exist");
		}

		public CategoryInfo[] getCategories(string blogid,string username,string password)
		{
			Framework.BlogInfo info = Config.CurrentBlog;
			if(ValidateUser(username,password,info.AllowServiceAccess))
			{
				LinkCategoryCollection lcc = Links.GetCategories(CategoryType.PostCollection,false);
				if(lcc == null)
				{
					throw new XmlRpcFaultException(0,"No categories exist");
				}
				CategoryInfo[] categories = new CategoryInfo[lcc.Count];
				CategoryInfo _category;
				for(int i=0; i<lcc.Count; i++)
				{
					_category = new CategoryInfo();
					_category.categoryid = lcc[i].CategoryID.ToString();
					_category.title = lcc[i].Title;
					_category.htmlUrl = info.FullyQualifiedUrl + "Category/" + lcc[i].CategoryID.ToString() + ".aspx";
					_category.rssUrl = info.FullyQualifiedUrl + "rss.aspx?catid=" + lcc[i].CategoryID.ToString();
					_category.description = lcc[i].Title;
					
					categories[i] = _category;
				}
				return categories;
			}
			throw new XmlRpcFaultException(0,"User does not exist");
		}

		public string newPost(string blogid, string username, string password, Post post, bool publish)
		{
			Framework.BlogInfo info = Config.CurrentBlog;
			if(ValidateUser(username,password,info.AllowServiceAccess))
			{
				CategoryEntry entry = new CategoryEntry();
				entry.Author = info.Author;
				entry.Email = info.Email;
				entry.Body = post.description;
				entry.Title = post.title;
				entry.TitleUrl = post.link;
				entry.SourceName = string.Empty;
				entry.SourceUrl = string.Empty;
				entry.Description = string.Empty;
				if(post.dateCreated.Year >= 2003)
				{
					entry.DateCreated = post.dateCreated;
					entry.DateUpdated = post.dateCreated;
				}
				else
				{
					entry.DateCreated = BlogTime.CurrentBloggerTime;
					entry.DateUpdated = entry.DateCreated;
				}
				entry.Categories = post.categories;
				entry.PostType = PostType.BlogPost;
				
				entry.IsActive = publish;
				entry.AllowComments = true;
				entry.DisplayOnHomePage = true;
				entry.IncludeInMainSyndication = true;
				entry.IsAggregated = true;
				entry.SyndicateDescriptionOnly = false;

			
				int postID = Entries.Create(entry);
				if(postID < 0)
				{
					throw new XmlRpcFaultException(0,"The post could not be added");
				}
				return postID.ToString();
			}
			throw new XmlRpcFaultException(0,"User does not exist");
		}

		#region w.bloggar workarounds/nominal MT support - HACKS
		
		// w.bloggar is not correctly implementing metaWeblogAPI on its getRecentPost call, it wants 
		// an instance of blogger.getRecentPosts at various time. 
		// 
		// What works better with w.bloggar is to tell it to use MT settings. For w.bloggar users 
		// with metaWeblog configured, we'll throw a more explanatory exception than method not found.

		public struct BloggerPost
		{
			public string content;
			public DateTime dateCreated;
			public string postid;
			public string userid;
		} 

		[XmlRpcMethod("blogger.getRecentPosts",
			 Description="Workaround for w.bloggar errors. Exists just to throw an exception explaining issue.")]
		public BloggerPost[] GetRecentPosts(string appKey, string blogid, string username, 
			string password, int numberOfPosts)
		{
			throw new XmlRpcFaultException(0, "You are most likely getting this message because you are using w.bloggar or trying to access Blogger API support in .Text--only metaWeblog API is currently supported. If your issue is w.bloggar, read on.\n\nw.bloggar does not correctly implement the metaWeblog API.\n\nIt is trying to call blogger.getRecentPosts, which does not exist in the metaWeblog API. Contact w.bloggar and encourage them to fix this bug.\n\nIn the meantime, to workaround this, go to the Account Properties dialog and hit 'Reload Blogs List'. This should clear the issue temporarily on w.bloggars side.");
		}		

		// we'll also add a couple structs and methods to give us nominal MT API-level support.
		// by doing this we'll allow w.bloggar to run against .Text using w.b's MT configuration.
		[XmlRpcMissingMapping(MappingAction.Ignore)]
		public struct MtCategory
		{
			[XmlRpcMissingMapping(MappingAction.Error)]
			public string categoryId;
			[XmlRpcMissingMapping(MappingAction.Error)]
			public string categoryName;
			public bool isPrimary;

			public MtCategory(string category)
			{
				categoryId = category;
				categoryName = category;
				isPrimary = false;
			}

			public MtCategory(string id, string category)
			{
				categoryId = id;
				categoryName = category;
				isPrimary = false;
			}
		}

		[XmlRpcMethod("mt.getCategoryList", 
			 Description="Gets a list of active categories for a given blog as an array of MT category struct.")]
		public MtCategory[] GetCategoryList(string blogid, string username, string password)
		{
			if(ValidateUser(username,password,Config.CurrentBlog.AllowServiceAccess))
			{
				LinkCategoryCollection lcc = Links.GetCategories(CategoryType.PostCollection,false);
				if(lcc == null)
				{
					throw new XmlRpcFaultException(0, "No categories exist");
				}

				MtCategory[] categories = new MtCategory[lcc.Count];
				MtCategory _category;
				for(int i=0; i<lcc.Count; i++)
				{
					_category = new MtCategory(lcc[i].CategoryID.ToString(), lcc[i].Title);				
					categories[i] = _category;
				}
				return categories;
			}
			throw new XmlRpcFaultException(0,"User does not exist");
		}

		[XmlRpcMethod("mt.setPostCategories",
			Description="Sets the categories for a given post.")]
		public bool SetPostCategories(string postid, string username, string password,
			MtCategory[] categories)
		{
			if(ValidateUser(username,password,Config.CurrentBlog.AllowServiceAccess))
			{			
				if (categories != null && categories.Length > 0)
				{
					int postID = Int32.Parse(postid);

					ArrayList al = new ArrayList();

															
					for (int i = 0; i < categories.Length; i++)
					{
							al.Add(Int32.Parse(categories[i].categoryId));
					}

					if(al.Count > 0)
					{
						Entries.SetEntryCategoryList(postID,(int[])al.ToArray(typeof(int)));
					}
				}				
				
				return true;
			}
			throw new XmlRpcFaultException(0,"User does not exist");
		}		

		[XmlRpcMethod("mt.getPostCategories",
			 Description="Sets the categories for a given post.")]
		public MtCategory[] GetPostCategories(string postid, string username, string password)
		{
			if(ValidateUser(username,password,Config.CurrentBlog.AllowServiceAccess))
			{
				int postID = Int32.Parse(postid);
				LinkCollection postCategories = Links.GetLinkCollectionByPostID(postID);
				MtCategory[] categories = new MtCategory[postCategories.Count];
				if (postCategories.Count > 0)
				{
					// REFACTOR: Might prefer seeing a dictionary come back straight from the provider.
					// for now we'll build our own catid->catTitle lookup--we need it below bc collection
					// from post is going to be null for title.
					LinkCategoryCollection cats = Links.GetCategories(CategoryType.PostCollection, false);
					Hashtable catLookup = new Hashtable(cats.Count);
					foreach (LinkCategory currentCat in cats)
						catLookup.Add(currentCat.CategoryID, currentCat.Title);

					MtCategory _category;
					for (int i = 0; i < postCategories.Count; i++)
					{						
						_category = new MtCategory(postCategories[i].CategoryID.ToString(), 
							(string)catLookup[postCategories[i].CategoryID]);				

						categories[i] = _category;
					}
				}				
				
				return categories;
			}
			throw new XmlRpcFaultException(0,"User does not exist");			
		}
		#endregion

	}
}

