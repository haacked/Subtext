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
using System.Globalization;
using System.IO;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using Sgml;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;

namespace Subtext.Framework.Util
{
	/// <summary>
	/// Summary description for Globals.
	/// </summary>
	public class Globals
	{
		private Globals()
		{
			
		}

//		public static string SkinImageLink(string image)
//		{
//			return HttpContext.Current.Request.ApplicationPath + "/skins/" + Config.CurrentBlog().Skin + "/images/" + image;
//		}

		#region XHTML

		public static bool IsValidXHTML(ref Entry entry)
		{
			try
			{
				SgmlReader r = new SgmlReader();
				r.SetBaseUri(Config.CurrentBlog().FullyQualifiedUrl);
				r.DocType = "HTML";
				r.InputStream = new StringReader("<bloghelper>" + entry.Body + "</bloghelper>");
				StringWriter sw = new StringWriter();
				XmlTextWriter w = new XmlTextWriter(sw);
				while (r.Read()) 
				{
					w.WriteNode(r, true);
				}
				w.Close(); 

//				string[] postFilters = Config.Settings.PostFilters;
//				if(postFilters != null)
//				{
//
//					StringCollection sc = new StringCollection();
//					
//					foreach(string filter in postFilters)
//					{
//						if(!sc.Contains(filter))
//						{
//							sc.Add(filter);
//						}
//						//
//						//						if(nodes != null)
//						//						{
//						//							throw new XmlException("Illegal tag " + filter + " found in your post. Please remove this before posting");
//						//						}
//					}
//					XmlTextReader reader = new XmlTextReader(new StringReader(sw.ToString()));
//					while(reader.Read())
//					{
//						if(sc.Contains(reader.Name))
//						{
//							throw new XmlException("Illegal tag " + reader.Name + " found in your post. Please remove this before posting");
//						}
//					}
//				}

				string xml = sw.ToString();
				entry.Body = xml.Substring(12,xml.Length - 25);
				entry.IsXHMTL = true;
				return true;
			}
			catch(XmlException ex)
			{
				entry.IsXHMTL = false;
				throw ex;
			}
			catch(Exception ex)
			{
				entry.IsXHMTL = false;
				throw ex;
			}
		}

		#endregion

		#region Urls

//		public static string ArchiveUrl(DateTime dt, string format)
//		{
//			return GetUrl("ArchiveUrl",dt.ToString(format));
//			//return Config.CurrentBlog().FullyQualifiedUrl + "archive/" + dt.ToString(format) + ".aspx";
//		}
//
//		public static string ImageUrl(int imagesID)
//		{
//			return GetUrl("ImageUrl",imagesID.ToString());
//		}
//
//		public static string PostsUrl(int entryid)
//		{
//			return GetUrl("PostUrl",entryid.ToString());
//		}
//		
//		public static string StoryUrl(int entryid)
//		{
//			return GetUrl("StoryUrl",entryid.ToString());
//		}
//
//		public static string PrintStoryUrl(int entryid)
//		{
//			return GetUrl("StoryPrintUrl",entryid.ToString());
//		}
//
//		public static string GetUrl(string name, params object[] items)
//		{
//			return Config.CurrentBlog().FullyQualifiedUrl + string.Format(Config.Settings.UrlSettings.GetUrlPattern(name),items);
//		}

		public static DateTime DateFromUrl(string url)
		{
			string date = Globals.GetReqeustedFileName(url);
			CultureInfo en = new CultureInfo("en-US");
			switch(date.Length)
			{
				case 8:
					return DateTime.ParseExact(date,"MMddyyyy",en);
				case 6:
					return DateTime.ParseExact(date,"MMyyyy",en);
				default:
					throw new Exception("Invalid Date Format");
			}
		}

		

		public static DateTime DateFromUrl(string url, string startfrom)
		{
			CultureInfo en = new CultureInfo("en-US");
			string date = url.Substring(url.ToLower().IndexOf(startfrom.ToLower()) + startfrom.Length);
			if(date.Length == 11)
			{
				
				return DateTime.ParseExact(date,"/yyyy/MM/dd",en);
				//return new DateTime(Int32.Parse(date.Substring(4,4)),Int32.Parse(date.Substring(0,2)),Int32.Parse(date.Substring(2,2)));
			}
			if(date.Length == 8)
			{
				return DateTime.ParseExact(date,"/yyyy/MM",en);
				//return new DateTime(Int32.Parse(date.Substring(2,4)),Int32.Parse(date.Substring(0,2)),1);
			}
			if(date.Length == 5)
			{
				return DateTime.ParseExact(date,"/yyyy",en);
				//return new DateTime(Int32.Parse(date.Substring(2,4)),Int32.Parse(date.Substring(0,2)),1);
			}
			return DateTime.Now;
		}

//		public static string GetLastLastSlashToDot(string url)
//		{
//			return Globals.GetReqeustedFileName(url);
//			int lastslash = url.LastIndexOf("/");
//			int dot =  url.ToLower().LastIndexOf(".");
//			if(lastslash == -1 || dot == -1)
//			{
//				return string.Empty;
//			}
//
//			else
//			{
//				return url.Substring(lastslash+1,(dot-1) - lastslash);
//			}
//		}
//
//
//
//		public static string GeFromLastSlash(string url)
//		{
//			
//			if(url.EndsWith("/"))
//			{
//				url = url.Remove(url.Length-1,1);
//			}
//
//			int lastslash= url.LastIndexOf("/");
//
//			if(lastslash == -1)
//			{
//				return string.Empty;
//			}
//
//			else
//			{
//				return url.Substring(lastslash+1);
//			}
//		}

		#endregion


		#region Url Helpers

		public static string GetReqeustedFileName(string uri)
		{
			return Path.GetFileNameWithoutExtension(uri);
		}

		public static int GetPostIDFromUrl(string uri)
		{
			try
			{
				return Int32.Parse(GetReqeustedFileName(uri));
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


		#endregion


		#region General




		public static int GetIntQS(string qs)
		{
			return Int32.Parse(HttpContext.Current.Request.QueryString[qs]);
		}

		public static string GetStringQS(string qs)
		{
			return HttpContext.Current.Request.QueryString[qs];
		}

		public static bool CheckNullQS(string qs)
		{
			return HttpContext.Current.Request.QueryString[qs] != null;
		}

		public static string GetUserIpAddress(HttpContext context)
		{
			string result = String.Empty;
			if (context == null) return result;

			result = HttpContext.Current.Request.ServerVariables["HTTP_X_FORWARDED_FOR"];
			if (null == result || result == String.Empty)
				result = HttpContext.Current.Request.ServerVariables["REMOTE_ADDR"];

			return result;
		}

		#endregion

		#region Formatting


		public static bool HasIllegalContent(string s)
		{
			if(s == null || s.Trim().Length == 0)
			{
				return false;
			}
			if (s.IndexOf("<script")>-1 || s.IndexOf("&#60script")>-1 || s.IndexOf("&60script")>-1 || s.IndexOf("%60script")>-1)
			{
				throw new IllegalPostCharactersException("Illegal Characters Found");
			}
			return false;
		}


		public static string EnableUrls(string text)
		{
			string pattern = @"(http|ftp|https):\/\/[\w]+(.[\w]+)([\w\-\.,@?^=%&amp;:/~\+#]*[\w\-\@?^=%&amp;/~\+#])";
			MatchCollection matchs;
                        
			matchs = Regex.Matches(text,pattern, RegexOptions.IgnoreCase | RegexOptions.Compiled);
			foreach (Match m in matchs) 
			{
				text = text.Replace(m.ToString(), "<a rel=\"nofollow\" target=\"_new\" href=\"" + m.ToString() + "\">" + m.ToString() + "</a>");
			}
			return text;			
		}

		public static string SafeFormatWithUrl(string stringToTransform)
		{
			return EnableUrls(SafeFormat(stringToTransform));
		}


		/// <summary>
		/// The only HTML we will allow is hyperlinks. We will however, check for line breaks and replace them with <br>
		/// </summary>
		/// <param name="stringToTransform"></param>
		/// <returns></returns>
		public static string SafeFormat(string stringToTransform) 
		{
			stringToTransform = HttpContext.Current.Server.HtmlEncode(stringToTransform);
//			MatchCollection matchs;
//                        
//			// Ensure we have safe anchors
//			matchs = Regex.Matches(stringToTransform, "&lt;a.href=&quot;(?<url>http://((.|\\n)*?))&quot;&gt;(?<target>((.|\\n)*?))&lt;/a&gt;", RegexOptions.IgnoreCase | RegexOptions.Compiled);
//
//			foreach (Match m in matchs) 
//			{
//				stringToTransform = stringToTransform.Replace(m.ToString(), "<a target=\"_new\" href=\"" + m.Groups["url"].ToString() + "\">" + m.Groups["target"].ToString() + "</a>");
//			}

			return stringToTransform.Replace("\n", "<br>");
		}

		public static string StripRTB(string text, string host)
		{
			//    /aspnetweblogweb/aspnetweblogweb    -> http://localhost/aspnetweblogweb/aspnetweblogweb/aspnetweblogweb

			//string s=  Regex.Replace(text, "/" + Config.Settings.AdminDirectory,"", RegexOptions.IgnoreCase);
			//return Regex.Replace(text,"<A href=\"/","<A href=\"" + Config.CurrentBlog().FullyQualifiedUrl,RegexOptions.IgnoreCase);
			string s=  Regex.Replace(text, "/localhost/S*Admin/","", RegexOptions.IgnoreCase);
			//return Regex.Replace(s,"<A href=\"/","<A href=\"" + Config.CurrentBlog().FullyQualifiedUrl,RegexOptions.IgnoreCase);
			return Regex.Replace(s,"<A href=\"/","<A href=\"" + "http://" + host + "/",RegexOptions.IgnoreCase);
			
		}

	

		public static string CheckForUrl(string text)
		{
			
			if(text == null || text.Trim().Length == 0 || text.Trim().ToLower().StartsWith("http://"))
			{
				return text;
			}
			return "http://" + text;
		}

		public static string SafeApplicationDirectoryCombine(string app, string directory)
		{
			//Clean this up. Double Replace is ugly.

			//should match "/app/" and "/dir/"
			//should match "/app" + "/dir"
			//should match "/app/" + dir
			string path = app + "/" + directory;
			return (path.Replace("///","/")).Replace("//","/");
		}

		public static string FormatApplicationPath(string app)
		{
			if(app == "/")
			{
				return string.Empty;
			}

			if(app.StartsWith("//"))
			{
				app = app.Substring(1);
			}

			if(app != null)
			{
				if(!app.StartsWith("/"))
				{
					app = "/" + app;
				}

				if(!app.EndsWith("/"))
				{
					app += "/";
				}
			}
			return app;
		}

		public static string DateString(DateTime dt, Char c)
		{
			switch(c)
			{
				case 'm':
				case 'M':
					return dt.ToString("yyyy/MM");
				case 'd':
				case 'D':
					return dt.ToString("yyyy/MM/dd");
				case 'y':
				case 'Y':
					return dt.ToString("yyyy");
				default:
					return string.Empty;
			}
		}

			#endregion

		#region Helper

		public static int AllowedItemCount(int ItemCount)
		{
			if(ItemCount > Config.Settings.ItemCount)
			{
				ItemCount = Config.Settings.ItemCount;
			}
			return ItemCount;
		}

		public static string WebPathCombine(string uriOne, string uriTwo)
		{
			string newUri = (uriOne + uriTwo);
			return newUri.Replace("//","/");
		}

		#endregion






		
	}
}

