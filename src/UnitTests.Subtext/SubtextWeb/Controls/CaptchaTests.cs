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
        [Test]
        public void CanRoundTripCaptchaInfo()
        {
            DateTime date =
                DateTime.ParseExact(DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture),
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