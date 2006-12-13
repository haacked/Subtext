using System;
using System.Globalization;
using System.IO;
using System.Text;
using System.Web.UI;
using MbUnit.Framework;
using Subtext.Framework.Text;
using Subtext.Web.Controls;
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
			info.Width = 0;
			info.Height = 0;
			info.WarpFactor = CaptchaImage.FontWarpFactor.High;
			info.DateGenerated = date;
				
			string encrypted = info.ToEncryptedString();
			Assert.IsTrue(encrypted.IndexOf("My Test") < 0);
			info = CaptchaInfo.FromEncryptedString(encrypted);
			Assert.AreEqual("My Test", info.Text);
			Assert.AreEqual(180, info.Width); //180 is default, if width is 0.
			Assert.AreEqual(50, info.Height); //50 is default, if width is 0.
			Assert.AreEqual(CaptchaImage.FontWarpFactor.High, info.WarpFactor);
			Assert.AreEqual(date, info.DateGenerated);
		}

		[Test]
		[ExpectedException(typeof(CaptchaExpiredException))]
		public void DecryptStringThrowsCaptchaExpiredExceptionWithInnerFormatException()
		{
			try
			{
				CaptchaBase.DecryptString("!#@#$@@@&%^!^$!");
			}
			catch(CaptchaExpiredException e)
			{
				Assert.AreEqual(typeof(FormatException), e.InnerException.GetType());
				throw;
			}
		}

		[Test]
		[ExpectedException(typeof(CaptchaExpiredException))]
		public void DecryptStringThrowsCaptchaExpiredException()
		{
			CaptchaBase.DecryptString("blahblah");
		}

		[Test]
		[ExpectedArgumentNullException]
		public void DecryptStringThrowsArgumentNullException()
		{
			CaptchaBase.DecryptString(null);
		}

		[Test]
		public void CanEncryptAnswer()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			string encryptedAnswer = captcha.EncryptTheAnswer("42");
			string unencrypted = CaptchaBase.DecryptString(encryptedAnswer);
			string answer = StringHelper.LeftBefore(unencrypted, "|");
			Assert.AreEqual("42", answer);
		}

		[Test]
		public void CanGetAndSetTimeout()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			captcha.CaptchaTimeout = 23;
			Assert.AreEqual(23, captcha.CaptchaTimeout);
		}

		[Test]
		public void CanGetAndSetCaptchaChars()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			captcha.CaptchaChars = "abc";
			Assert.AreEqual("abc", captcha.CaptchaChars);
		}

		[Test]
		public void CanGetAndSetCaptchaFont()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			captcha.CaptchaFont = "comic sans ms";
			Assert.AreEqual("comic sans ms", captcha.CaptchaFont);
		}

		[Test]
		public void CanGetAndSetCaptchaFontWarping()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			captcha.CaptchaFontWarping = CaptchaImage.FontWarpFactor.Extreme;
			Assert.AreEqual(CaptchaImage.FontWarpFactor.Extreme, captcha.CaptchaFontWarping);
		}

		[Test]
		public void CanGetAndSetCaptchaLength()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			captcha.CaptchaLength = 42;
			Assert.AreEqual(42, captcha.CaptchaLength);
		}

		[Test]
		public void AnswerFormFieldNameSetCorrectly()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			captcha.ID = "CaptchaTest";
			Assert.AreEqual("CaptchaTest_answer", captcha.GetAnswerFormFieldName());
		}

		[Test]
		public void HiddenEncryptedAnswerFieldNameSetCorrectly()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			captcha.ID = "CaptchaTest";
			Assert.AreEqual("CaptchaTest_encrypted", captcha.GetHiddenEncryptedAnswerFieldName());
		}

		//HiddenEncryptedAnswerFieldName

		[Test]
		public void CaptchaInfoGeneratesRandomText()
		{
			CaptchaInfo captchaInfo = new CaptchaInfo();
			captchaInfo.Text = null;
			Assert.IsNotNull(captchaInfo.Text);
		}

		[Test]
		public void CaptchaInfoGeneratesRandomTextUsingTextChars()
		{
			CaptchaInfo captchaInfo = new CaptchaInfo();
			captchaInfo.Text = null;
			captchaInfo.TextLength = 3;
			captchaInfo.TextChars = "a";
			Assert.AreEqual(3, captchaInfo.Text.Length);
			Assert.AreEqual("aaa", captchaInfo.Text);
		}

		[Test]
		public void CaptchaInfoTextLengthDefaultsToFour()
		{
			CaptchaInfo captchaInfo = new CaptchaInfo();
			Assert.AreEqual(4, captchaInfo.TextLength);
			captchaInfo.TextLength = -1;
			Assert.AreEqual(4, captchaInfo.TextLength);
		}

		[Test]
		public void CanCreateCaptchaExpiredException()
		{
			CaptchaExpiredException e = new CaptchaExpiredException();
			Assert.IsNotNull(e);
			e = new CaptchaExpiredException("Test");
			Assert.AreEqual("Test", e.Message);
			e = new CaptchaExpiredException("Test", null);
			Assert.IsNull(e.InnerException);
		}

		[Test]
		public void CanDetermineControlPropertiesValid()
		{
			CaptchaControlTester captcha = new CaptchaControlTester();
			Assert.IsTrue(captcha.GetControlPropertiesValid());
		}
	}
	
	public class CaptchaControlTester : CaptchaControl
	{
		public bool GetControlPropertiesValid()
		{
			return base.ControlPropertiesValid();
		}

		public AnswerAndDate GetTheEncryptedAnswerFromForm()
		{
			return base.GetEncryptedAnswerFromForm();
		}

		public string GetAnswerFormFieldName()
		{
			return this.AnswerFormFieldName;
		}

		public string GetHiddenEncryptedAnswerFieldName()
		{
			return this.HiddenEncryptedAnswerFieldName;
		}

		public string EncryptTheAnswer(string answer)
		{
			return base.EncryptAnswer(answer);
		}

		public void RenderOverride(HtmlTextWriter writer)
		{
			base.Render(writer);
		}
	}
	
}
