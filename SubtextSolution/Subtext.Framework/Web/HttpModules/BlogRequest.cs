#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

namespace Subtext.Framework.Web.HttpModules
{
	/// <summary>
	/// Represents the state of the current blog request.
	/// </summary>
	public class BlogRequest
	{
		public const int DefaultPort = 80;
        public const string BlogRequestKey = "Subtext__CurrentRequest";

		/// <summary>
		/// Gets or sets the current blog request.
		/// </summary>
		/// <value>The current.</value>
		public static BlogRequest Current
		{
            get { 
                return (BlogRequest)HttpContext.Current.Items[BlogRequestKey];
            }
            set {
                HttpContext.Current.Items[BlogRequestKey] = value;
            }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogRequest"/> class.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="subfolder">The subfolder.</param>
		/// <param name="url">The raw requested URL</param>
		/// <param name="isLocal">True if this request is a local machine request.</param>
        /// <param name="requestLocation">Defines which type of request this is.</param>
        public BlogRequest(string host, string subfolder, Uri url, bool isLocal, RequestLocation requestLocation)
		{
			Host = host;
			Subfolder = subfolder;
			RawUrl = url;
			IsLocal = isLocal;
            RequestLocation = requestLocation;
		}

        /// <summary>
        /// Initializes a new instance of the <see cref="BlogRequest"/> class.
        /// </summary>
        /// <param name="host">The host.</param>
        /// <param name="subfolder">The subfolder.</param>
        /// <param name="url">The raw requested URL</param>
        /// <param name="isLocal">True if this request is a local machine request.</param>
        public BlogRequest(string host, string subfolder, Uri url, bool isLocal) 
            : this(host, subfolder, url, isLocal, RequestLocation.Blog)
        {
        }

        public BlogRequest(HttpRequestBase request)
            : this(HostFromRequest(request)
                , SubfolderFromRequest(request)
                , request.Url
                , request.IsLocal
                , DetermineRequestLocation(request))
        {
        }

        private static RequestLocation DetermineRequestLocation(HttpRequestBase request) {
            if (IsStaticFileRequest(request)) {
                return RequestLocation.StaticFile;
            }
            if (IsLogin(request)) {
                return RequestLocation.LoginPage;
            }
            if (IsSystemMessage(request)) {
                return RequestLocation.SystemMessages;
            }
            if (IsHostAdmin(request)) {
                return RequestLocation.HostAdmin;
            }
            if (IsInstallation(request)) {
                return RequestLocation.Installation;
            }
            return RequestLocation.Blog;
        }

        /// <summary>
        /// Determines whether the request is for a static file.
        /// </summary>
        /// <returns>
        /// 	<c>true</c> if [is static file request]; otherwise, <c>false</c>.
        /// </returns>
        private static bool IsStaticFileRequest(HttpRequestBase request) {
            string filePath = request.FilePath;

            return filePath.EndsWith(".css", StringComparison.OrdinalIgnoreCase)
                    || filePath.EndsWith(".jpg", StringComparison.OrdinalIgnoreCase)
                    || filePath.EndsWith(".js", StringComparison.OrdinalIgnoreCase)
                    || filePath.EndsWith(".gif", StringComparison.OrdinalIgnoreCase)
                    || filePath.EndsWith(".png", StringComparison.OrdinalIgnoreCase)
                    || filePath.EndsWith(".xml", StringComparison.OrdinalIgnoreCase)
                    || filePath.EndsWith(".txt", StringComparison.OrdinalIgnoreCase)
                    || filePath.EndsWith(".html", StringComparison.OrdinalIgnoreCase)
                    || filePath.EndsWith(".htm", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsInSpecialDirectory(HttpRequestBase request, string folderName) {
            string appPath = request.ApplicationPath ?? string.Empty;

            if (!appPath.EndsWith("/")) {
                appPath += "/";
            }
            appPath += folderName + "/";

            return request.Path.StartsWith(appPath, StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsLogin(HttpRequestBase request) {
            return (request.Path ?? string.Empty).EndsWith("Login.aspx", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsSystemMessage(HttpRequestBase request) {
            return IsInSpecialDirectory(request, "SystemMessages");
        }

        private static bool IsHostAdmin(HttpRequestBase request) {
            return IsInSpecialDirectory(request, "HostAdmin");
        }

        private static bool IsInstallation(HttpRequestBase request)
        {
            return IsInSpecialDirectory(request, "Install");
        }

        private static string SubfolderFromRequest(HttpRequestBase request) {
            string subfolder = UrlFormats.GetBlogSubfolderFromRequest(request.RawUrl, request.ApplicationPath) ?? string.Empty;
            if (!Config.IsValidSubfolderName(subfolder)) {
                subfolder = string.Empty;
            }
            return subfolder;
        }

        private static string HostFromRequest(HttpRequestBase request) {
            string host = request.Params["HTTP_HOST"];
            if (String.IsNullOrEmpty(host)) {
                host = request.Url.Authority;
            }
            return host;
        }

        public RequestLocation RequestLocation {
            get;
            private set;
        }

		public bool IsLocal {
			get;
            private set;
		}

        public Blog Blog {
            get;
            set;
        }
		
		/// <summary>
		/// Gets the host.
		/// </summary>
		/// <value>The host.</value>
		public string Host {
			get;
            private set;
		}

	    /// <summary>
		/// Gets the host.
		/// </summary>
		/// <value>The host.</value>
		public string Subfolder {
			get;
            private set;
		}

		public Uri RawUrl {
			get;
			private set;
		}
	}
}
