using System;

namespace Subtext.Web.Controls.Captcha
{
	/// <summary>
	/// Captcha control with subtext specific defaults.
	/// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    public class SubtextCaptchaControl : CaptchaControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SubtextCaptchaControl"/> class.
		/// </summary>
		public SubtextCaptchaControl() : base()
		{
			this.CaptchaLength = 4;
		}
	}
}
