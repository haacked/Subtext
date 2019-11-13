#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

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