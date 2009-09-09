using System;
using System.Diagnostics.CodeAnalysis;

namespace Subtext.Web.Controls.Captcha
{
    /// <summary>
    /// Exception thrown when a captcha image has expired.
    /// </summary>
    /// <remarks>
    /// This exception does not have any custom properties, 
    /// thus it does not implement ISerializable.
    /// </remarks>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    [Serializable]
    public sealed class CaptchaExpiredException : Exception
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaExpiredException"/> class.
        /// </summary>
        public CaptchaExpiredException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaExpiredException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        public CaptchaExpiredException(string message) : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaExpiredException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="innerException">The inner exception.</param>
        public CaptchaExpiredException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}