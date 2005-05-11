using System;
using System.Globalization;
using System.IO;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Util;

namespace Subtext.Framework.Format
{
	/// <summary>
	/// Default Implemenation of UrlFormats
	/// </summary>
	public class UrlFormats
	{
		protected string fullyQualifiedUrl = null;

		public UrlFormats(string fullyQualifiedUrl)
		{
			this.fullyQualifiedUrl = fullyQualifiedUrl;
		}

		public virtual string PostCategoryUrl(string categoryName, int categoryID)
		{
			return GetUrl("category/{0}.aspx",categoryID);
		}
		
		public virtual string ArticleCategoryUrl(string categoryName, int categoryID)
		{
			return GetUrl("category/{0}.aspx",categoryID);
		}

		public virtual string EntryUrl(Entry entry)
		{
			return GetUrl("archive/" + entry.DateCreated.ToString("yyyy/MM/dd") + "/{0}.aspx", entry.HasEntryName ? entry.EntryName : entry.EntryID.ToString());
		}

		public virtual string ImageUrl(string category, int ImageID)
		{
			return GetUrl("gallery/image/{0}.aspx",ImageID);
		}

		public virtual string YearUrl(DateTime dt)
		{
			return GetUrl("archive/{0}.aspx",dt.ToString("yyyy"));
		}

		public virtual string DayUrl(DateTime dt)
		{
			//return GetUrl("archive/{0}/{1}/{2}.aspx",dt.Year,dt.Month,dt.Day);
			return GetUrl("archive/{0}.aspx",dt.ToString("yyyy/MM/dd"));
		}

		public virtual string GalleryUrl(string category, int GalleryID)
		{
			return GetUrl("gallery/{0}.aspx",GalleryID);
		}

		public virtual string ArticleUrl(Entry entry)
		{
			if(entry.HasEntryName)
			{
				return GetUrl("articles/{0}.aspx",entry.EntryName);
			}

			return GetUrl("articles/{0}.aspx",entry.EntryID);
			
		}

		public virtual string MonthUrl(DateTime dt)
		{
			return GetUrl("archive/{0}.aspx",dt.ToString("yyyy/MM"));
		}

		public virtual string CommentRssUrl(int EntryID)
		{
			return GetUrl("comments/commentRss/{0}.aspx",EntryID);
		}

		public virtual string CommentUrl(Entry ParentEntry, Entry ChildEntry)
		{
			return string.Format("{0}#{1}",ParentEntry.Link,ChildEntry.EntryID);
			//return PostUrl(dt,EntryID) + "#FeedBack";
		}

		public virtual string CommentUrl(Entry entry)
		{
			return 	GetUrl("archive/" + entry.DateCreated.ToString("yyyy/MM/dd") + "/{0}.aspx#{1}", entry.HasEntryName ? entry.EntryName : entry.ParentID.ToString(),entry.EntryID);
		}

		public virtual string CommentApiUrl(int EntryID)
		{
			return GetUrl("comments/{0}.aspx",EntryID);
		}

		public virtual string TrackBackUrl(int EntryID)
		{
			return GetUrl("services/trackbacks/{0}.aspx",EntryID);
		}

		public virtual string AggBugkUrl(int EntryID)
		{
			return GetUrl("aggbug/{0}.aspx",EntryID);
		}

		protected virtual string GetUrl(string pattern, params object[] items)
		{
			return this.fullyQualifiedUrl + string.Format(pattern,items);
		}

		/// <summary>
		/// Returns a <see cref="DateTime"/> instance parsed from the url.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <returns></returns>
		public static DateTime DateFromUrl(string url)
		{
			string date = UrlFormats.GetRequestedFileName(url);
			CultureInfo en = new CultureInfo("en-US");
			switch(date.Length)
			{
				case 8:
					return DateTime.ParseExact(date,"MMddyyyy", en);
				case 6:
					return DateTime.ParseExact(date,"MMyyyy", en);
				default:
					throw new Exception("Invalid Date Format");
			}
		}

		/// <summary>
		/// Returns a <see cref="DateTime"/> instance parsed from the url 
		/// at the startfrom position.
		/// </summary>
		/// <param name="url">URL.</param>
		/// <param name="startfrom">Startfrom.</param>
		/// <returns></returns>
		public static DateTime DateFromUrl(string url, string startfrom)
		{
			CultureInfo en = new CultureInfo("en-US");
			string date = url.Substring(url.ToLower().IndexOf(startfrom.ToLower()) + startfrom.Length);
			if(date.Length == 11)
			{
				return DateTime.ParseExact(date,"/yyyy/MM/dd",en);
			}
			if(date.Length == 8)
			{
				return DateTime.ParseExact(date,"/yyyy/MM",en);
			}
			if(date.Length == 5)
			{
				return DateTime.ParseExact(date,"/yyyy",en);
			}
			return DateTime.Now;
		}

		public static string GetRequestedFileName(string uri)
		{
			return Path.GetFileNameWithoutExtension(uri);
		}

		public static int GetPostIDFromUrl(string uri)
		{
			try
			{
				return Int32.Parse(GetRequestedFileName(uri));
			}
			catch (FormatException)
			{
				throw new ArgumentException("Invalid Post ID.");
			}			
		}

		/// <summary>
		/// Gets the blog app from request.
		/// </summary>
		/// <param name="path">Path.</param>
		/// <param name="app">App.</param>
		/// <returns></returns>
		public static string GetBlogAppFromRequest(string path, string app)
		{
			if(!app.StartsWith("/"))
			{
				app = "/" + app;
			}
			if(!app.EndsWith("/"))
			{
				app += "/";
			}
			if(path.StartsWith(app))
			{
				path = path.Remove(0, app.Length);
			}
			int lastSlash = path.IndexOf("/");
			if(lastSlash > -1)
			{
				path = path.Remove(lastSlash, path.Length - lastSlash);
			}
			return path;
		}

		/// <summary>
		/// From Jason Block @ http://www.angrycoder.com/article.aspx?cid=5&y=2003&m=4&d=15
		/// Basically, it's [Request.UrlReferrer] doing a lazy initialization of its internal 
		/// _referrer field, which is a Uri-type class. That is, it's not created until it's 
		/// needed. The point is that there are a couple of spots where the UriFormatException 
		/// could leak through. One is in the call to GetKnownRequestHeader(). _wr is a field 
		/// of type HttpWorkerRequest. 36 is the value of the HeaderReferer constant - since 
		/// that's being blocked in this case, it may cause that exception to occur. However, 
		/// HttpWorkerRequest is an abstract class, and it took a trip to the debugger to find 
		/// out that _wr is set to a System.Web.Hosting.ISAPIWorkerRequestOutOfProc object. 
		/// This descends from System.Web.Hosting.ISAPIWorkerRequest, and its implementation 
		/// of GetKnownRequestHeader() didn't seem to be the source of the problem. 
		/// </summary>
		/// <param name="request"></param>
		/// <returns></returns>
		public static string GetUriReferrerSafe(HttpRequest request)
		{
			string retVal = null;
    
			try
			{
				retVal = request.UrlReferrer.ToString();
			}
			catch{}
    
			return retVal;
		}

	}
}
