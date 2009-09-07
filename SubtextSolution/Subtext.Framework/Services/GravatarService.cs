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
            : this(settings["GravatarUrlFormatString"], settings.GetEnum<GravatarEmailFormat>("GravatarEmailFormat"), settings.GetBoolean("GravatarEnabled"))
        { 
        }

        public GravatarService(string UrlFormatString, GravatarEmailFormat emailFormat, bool enabled)
        {
            this.UrlFormatString = UrlFormatString;
            this.EmailFormat = emailFormat;
            this.Enabled = enabled;   
        }

        public bool Enabled { 
            get; 
            private set; 
        }

        public string UrlFormatString { 
            get; 
            private set; 
        }

        public GravatarEmailFormat EmailFormat { 
            get; 
            private set; 
        }

        public string GenerateUrl(string email, string defaultImage) 
        {
            if (String.IsNullOrEmpty(email)) {
                return string.Empty;
            }
            defaultImage = defaultImage ?? "identicon";
            string emailForUrl = email.ToLowerInvariant();
            if (EmailFormat == GravatarEmailFormat.Md5)
            {
                emailForUrl = FormsAuthentication.HashPasswordForStoringInConfigFile(emailForUrl, "md5").ToLowerInvariant();
            }

            emailForUrl = HttpUtility.UrlEncode(emailForUrl);

            return String.Format(CultureInfo.InvariantCulture, UrlFormatString, emailForUrl, defaultImage);
        }
    }
}
