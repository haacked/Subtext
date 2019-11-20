using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Web.Controls.Captcha;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
    [TestClass]
    public class InvisibleCaptchaTests
    {
        [TestMethod]
        public void CanRoundTripEncryption()
        {
            string encrypted = CaptchaBase.EncryptString("Hello Sucka!");
            string decrypted = CaptchaBase.DecryptString(encrypted);
            Assert.AreEqual("Hello Sucka!", decrypted, "Round trip did not work.");
        }
    }
}