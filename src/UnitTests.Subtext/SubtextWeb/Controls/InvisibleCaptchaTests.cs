using MbUnit.Framework;
using Subtext.Web.Controls.Captcha;

namespace UnitTests.Subtext.SubtextWeb.Controls
{
    [TestFixture]
    public class InvisibleCaptchaTests
    {
        [Test]
        public void CanRoundTripEncryption()
        {
            string encrypted = CaptchaBase.EncryptString("Hello Sucka!");
            string decrypted = CaptchaBase.DecryptString(encrypted);
            Assert.AreEqual("Hello Sucka!", decrypted, "Round trip did not work.");
        }
    }
}