using System.Diagnostics.CodeAnalysis;

namespace Subtext.Web.Controls.Captcha
{
    /// <summary>
    /// Captcha control with subtext specific defaults.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    public class SubtextCaptchaControl : CaptchaControl
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubtextCaptchaControl"/> class.
        /// </summary>
        public SubtextCaptchaControl()
        {
            CaptchaLength = 4;
        }
    }
}