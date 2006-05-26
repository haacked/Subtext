using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web;
using MbUnit.Framework;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Configuration;
using Subtext.Framework.Tracking;

namespace UnitTests.Subtext.Framework.Tracking
{
	/// <summary>
	/// Summary description for TrackbackHandler.
	/// </summary>
	[TestFixture]
	public class TrackbackHandlerTests
	{
		/// <summary>
		/// Makes sure the HTTP handler used to handle trackbacks does so properly.
		/// </summary>
		[Test]
		[RollBack]
		public void TrackbackPostCreatesProperTrackback()
		{
			string hostname = UnitTestHelper.GenerateRandomHostname();
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty);
			Assert.IsTrue(Config.CreateBlog("", "username", "password", hostname, string.Empty));
			
			Entry entry = UnitTestHelper.CreateEntryInstanceForSyndication("phil", "title", "body");
			entry.DateCreated = entry.DateSyndicated = entry.DateUpdated = DateTime.ParseExact("2006/05/25", "yyyy/MM/dd", CultureInfo.InvariantCulture);
			int id = Entries.Create(entry);
	
			StringBuilder sb = new StringBuilder();
			TextWriter output = new StringWriter(sb);
			UnitTestHelper.SetHttpContextWithBlogRequest(hostname, string.Empty, string.Empty, "/trackback/services/" + id + ".aspx", output, "POST");
			
			TrackBackHandler handler = new TrackBackHandler();
			handler.SourceVerification += new TrackBackHandler.SourceVerificationEventHandler(handler_SourceVerification);
			
			HttpContext.Current.Request.ContentType = "application/x-www-form-urlencoded";
			handler.ProcessRequest(HttpContext.Current);
			HttpContext.Current.Response.Flush();	
			string responseText = sb.ToString();
			Console.WriteLine(responseText);
		}

		private void handler_SourceVerification(object sender, SourceVerificationEventArgs e)
		{
			e.Verified = true;
		}
	}
}
