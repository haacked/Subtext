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
    //TODO: This service is a bit hard to use. We should refactor it so it has 
    //      a bit more smarts about Subtext. Such as it can figure out the default image FQDN URL
    public class GravatarService
    {
        public GravatarService(NameValueCollection settings)
            : this(settings["GravatarUrlFormatString"], settings.GetBoolean("GravatarEnabled"))
        {
        }

        public GravatarService(string urlFormatString, bool enabled)
        {
            UrlFormatString = urlFormatString;
            Enabled = enabled;
        }

        public bool Enabled { get; private set; }

        public string UrlFormatString { get; private set; }

        public string GenerateUrl(string email, string defaultImage = "identicon")
        {
            if (!String.IsNullOrEmpty(defaultImage))
            {
                defaultImage = HttpUtility.UrlEncode(defaultImage);
            }

            string emailForUrl = String.Empty;
            if (!String.IsNullOrEmpty(email))
            {
                emailForUrl = (FormsAuthentication.HashPasswordForStoringInConfigFile(email.ToLowerInvariant(), "md5") ?? string.Empty).ToLowerInvariant();
            }
            return String.Format(CultureInfo.InvariantCulture, UrlFormatString, emailForUrl, defaultImage);
        }
    }
}
