using System;
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
			Console.WriteLine(builder.ToString());
			Assert.IsTrue(builder.ToString().StartsWith("<div>"), "Expected the output to start with <div>");
			
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
