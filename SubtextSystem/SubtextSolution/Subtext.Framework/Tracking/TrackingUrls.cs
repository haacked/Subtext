using System;

namespace Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackingUrls.
	/// </summary>
	public class TrackingUrls
	{
		private TrackingUrls()
		{
			
		}

		private static readonly string ai = "<img src =\"{0}\" width = \"1\" height = \"1\" />";
		public static string  AggBugImage(string url)
		{
			return  string.Format(ai,url);
		}

	}
}
