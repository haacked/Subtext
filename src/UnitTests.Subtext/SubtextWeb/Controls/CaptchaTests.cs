using System;
using System.Globalization;
using System.Web.UI;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Web.Controls.Captcha;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
    [TestClass]
    public class CaptchaTests
    {
        [TestMethod]
        public void CanRoundTripCaptchaInfo()
        {
            DateTime date =
                DateTime.ParseExact(DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
                                    "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);

            var info = new CaptchaInfo("My Test");
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