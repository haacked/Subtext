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

using System;
using System.Collections.Specialized;
using System.Globalization;
using System.Web;
using System.Web.Security;

namespace Subtext.Framework.Services
{
    public class GravatarService
    {
        public GravatarService(NameValueCollection settings)
            : this(
                settings["GravatarUrlFormatString"], settings.GetEnum<GravatarEmailFormat>("GravatarEmailFormat"),
                settings.GetBoolean("GravatarEnabled"))
        {
        }

        public GravatarService(string urlFormatString, GravatarEmailFormat emailFormat, bool enabled)
        {
            UrlFormatString = urlFormatString;
            EmailFormat = emailFormat;
            Enabled = enabled;
        }

        public bool Enabled { get; private set; }

        public string UrlFormatString { get; private set; }

        public GravatarEmailFormat EmailFormat { get; private set; }

        public string GenerateUrl(string email, string defaultImage)
        {
            if(String.IsNullOrEmpty(email))
            {
                return defaultImage ?? string.Empty;
            }
            defaultImage = defaultImage ?? "identicon";
            string emailForUrl = email.ToLowerInvariant();
            if(EmailFormat == GravatarEmailFormat.Md5)
            {
                emailForUrl = (FormsAuthentication.HashPasswordForStoringInConfigFile(emailForUrl, "md5") ?? string.Empty).ToLowerInvariant();
            }

            emailForUrl = HttpUtility.UrlEncode(emailForUrl);

            return String.Format(CultureInfo.InvariantCulture, UrlFormatString, emailForUrl, defaultImage);
        }
    }
}