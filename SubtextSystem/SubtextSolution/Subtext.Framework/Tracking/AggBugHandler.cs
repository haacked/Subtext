using System;
using System.Globalization;
using System.Web;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;
using Subtext.Framework.Util;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for AggBugHandler.
	/// </summary>
	public class AggBugHandler : IHttpHandler
	{
		public AggBugHandler()
		{
			//
			// TODO: Add constructor logic here
			//
		}

		static AggBugHandler()
		{
			//Lazy way to include 1x1.gif :)
			_bytes = Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");
		}

		#region IHttpHandler Members

		private static byte[] _bytes;

		public void ProcessRequest(HttpContext context)
		{
			//Check to see if we have sent the 1x1 image in the last 12 hours (requires If-Modified-Since header)
			if(_CachedVersionIsOkay(context.Request))
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
					ev.BlogID = Config.CurrentBlog.BlogID;
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

			
		}

		private bool _CachedVersionIsOkay(HttpRequest Request)
		{
			//Get header value
			string  ifModified = Request.Headers["If-Modified-Since"];

			//Do we have it?
			if (ifModified != null)
			{
				//convert to datetime and add 12 hours. We don't want to count quick reclicks.
				DateTime dt = DateTime.Parse(ifModified).AddHours(12);
				return dt >= DateTime.Now;
			}

			return false;
		}

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
