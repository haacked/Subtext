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
using System.Drawing.Imaging;
using System.Web;

namespace Subtext.Web.Controls.Captcha
{
    /// <summary>
    /// Handles a special request for a CAPTCHA image.  The request must 
    /// pass the specs for the image via an encrypted serialized instance 
    /// of <see cref="CaptchaInfo" />.
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    public class CaptchaImageHandler : IHttpHandler
    {
        #region IHttpHandler Members

        /// <summary>
        /// Renders the Captcha Image.
        /// </summary>
        /// <param name="context">An <see cref="T:System.Web.HttpContext"></see> object that provides 
        /// references to the intrinsic server objects (for example, Request, Response, Session, and Server) 
        /// used to service HTTP requests.</param>
        public void ProcessRequest(HttpContext context)
        {
            HttpApplication application = context.ApplicationInstance;
            string encryptedCaptchaInfo = application.Request.QueryString["spec"];
            CaptchaInfo captcha = CaptchaInfo.FromEncryptedString(encryptedCaptchaInfo);

            string textToRender = captcha.Text;

            if (string.IsNullOrEmpty(textToRender))
            {
                application.Response.StatusCode = 404;
                application.Response.End();
            }
            else
            {
                using (var captchaImage = new CaptchaImage())
                {
                    captchaImage.Width = captcha.Width;
                    captchaImage.Height = captcha.Height;
                    captchaImage.FontWarp = captcha.WarpFactor;
                    captchaImage.Font = captcha.FontFamily;
                    captchaImage.Text = textToRender;
                    captchaImage.Image.Save(application.Context.Response.OutputStream, ImageFormat.Jpeg);
                }
                application.Response.ContentType = "image/jpeg";
                application.Response.StatusCode = 200;
                application.Response.End();
            }
        }


        /// <summary>
        /// Gets a value indicating whether another request can use the <see cref="T:System.Web.IHttpHandler"></see> instance.
        /// </summary>
        /// <value></value>
        /// <returns>true if the <see cref="T:System.Web.IHttpHandler"></see> instance is reusable; otherwise, false.</returns>
        public bool IsReusable
        {
            get { return true; }
        }

        #endregion
    }
}