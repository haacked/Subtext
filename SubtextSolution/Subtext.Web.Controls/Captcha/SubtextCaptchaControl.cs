using System;
using Subkismet.Captcha;

namespace Subtext.Web.Controls.Captcha
{
	/// <summary>
	/// Captcha control with subtext specific defaults.
	/// </summary>
	public class SubtextCaptchaControl : CaptchaControl
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SubtextCaptchaControl"/> class.
		/// </summary>
		public SubtextCaptchaControl()
		{
			this.CaptchaLength = 4;
		}
	}
}
