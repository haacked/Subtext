using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using MbUnit.Framework;
using Subtext.Web.Controls.Captcha;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
	[TestFixture]
	public class CaptchaTests
	{
		[Test, Ignore("Need to do more to allow this to work.")]
		public void CanRenderCaptchaControl()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "");
			StringBuilder builder = new StringBuilder();
			StringWriter stringWriter = new StringWriter(builder);
			HtmlTextWriter writer = new HtmlTextWriter(stringWriter);
			CaptchaControlTester control = new CaptchaControlTester();

			Assert.AreEqual(CaptchaControl.Layout.CssBased, control.LayoutStyle, "Expected default of CSS based layout style.");
			
			control.RenderOverride(writer);
			Console.WriteLine(builder);
			Assert.IsTrue(builder.ToString().StartsWith("<div>"), "Expected the output to start with <div>");
			
		}
		
		[Test]
		public void CanRoundTripCaptchaInfo()
		{
			DateTime date = DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture), "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
			
			CaptchaInfo info = new CaptchaInfo("My Test");
			info.WarpFactor = CaptchaImage.FontWarpFactor.High;
			info.DateGenerated = date;
				
			string encrypted = info.ToEncryptedString();
			Assert.IsTrue(encrypted.IndexOf("My Test") < 0);
			info = CaptchaInfo.FromEncryptedString(encrypted);
			Assert.AreEqual("My Test", info.Text);
			Assert.AreEqual(CaptchaImage.FontWarpFactor.High, info.WarpFactor);
			Assert.AreEqual(date, info.DateGenerated);
		}
	}
	
	public class CaptchaControlTester : CaptchaControl
	{
		public void RenderOverride(HtmlTextWriter writer)
		{
			base.Render(writer);
		}
	}
	
}
