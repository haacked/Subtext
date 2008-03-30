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
using System.Globalization;
using System.Web;
using log4net;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Web;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for AggBugHandler.
	/// </summary>
	public class AggBugHandler : IHttpHandler
	{
		ILog Log = new Subtext.Framework.Logging.Log();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="AggBugHandler"/> class.
		/// </summary>
		public AggBugHandler()
		{
		}

		static AggBugHandler()
		{
			//Lazy way to include 1x1.gif :)
			_bytes = Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");
		}

		#region IHttpHandler Members

		private static byte[] _bytes;

		/// <summary>
		/// Custom <see langword="HttpHandler "/> that implements the <see cref="T:System.Web.IHttpHandler"/> interface. 
		/// This records a visit via the AggBug.
		/// </summary>
		/// <param name="context">An <see cref="T:System.Web.HttpContext"/> object that provides 
		/// references to the intrinsic server objects (for example, <see langword="Request"/>, 
		/// <see langword="Response"/>, <see langword="Session"/>, and <see langword="Server"/>) 
		/// <see langword=""/> used to service HTTP requests.</param>
		public void ProcessRequest(HttpContext context)
		{
			Log.Debug("Entering AggBug Request...");
			//Check to see if we have sent the 1x1 image in the last 12 hours (requires If-Modified-Since header)
			if(CachedVersionIsOkay())
			{
				context.Response.StatusCode = 304;
				context.Response.SuppressContent = true;
			}
			else
			{
				int EntryID = UrlFormats.GetPostIDFromUrl(context.Request.Path);

				//Do we have an EntryID? 
				//Should TrackEntry check to see if the EntryID exists?
				//Should be nicer once we implement queueing for stats.
				if(EntryID > 0)
				{
					EntryView ev = new EntryView();
					ev.BlogId = Config.CurrentBlog.Id;
					ev.EntryID = EntryID;
					ev.PageViewType = PageViewType.AggView;
					EntryTracker.Track(ev);
				}

				//Change ContentType to gif, sent length, Set Modified date to Now. 
				//We will check the Mod date on future requests so that we don't double count with in
				//a 12 hour stretch.
				context.Response.ContentType = "image/gif";
				context.Response.AppendHeader("Content-Length",_bytes.Length.ToString(CultureInfo.InvariantCulture));
				context.Response.Cache.SetLastModified(DateTime.Now);
				context.Response.Cache.SetCacheability(HttpCacheability.Public);
				context.Response.BinaryWrite(_bytes);
			}

			Log.Debug("Leaving AggBug Request...");			
		}

		private bool CachedVersionIsOkay()
		{
			//Get header value
			DateTime dt = HttpHelper.GetIfModifiedSinceDateUTC();
			if(dt == NullValue.NullDateTime)
			{
				return false;
			}

			//convert to datetime and add 12 hours. We don't want to count quick reclicks.
			return dt.AddHours(12) >= DateTime.UtcNow;
		}

		/// <summary>
		/// Gets a value indicating whether another request can use
		/// the <see cref="T:System.Web.IHttpHandler"/>
		/// instance.
		/// </summary>
		/// <value></value>
		public bool IsReusable
		{
			get
			{
				return true;
			}
		}

		#endregion
	}
}
