using System;
using System.Web;
using Subtext.Framework.Configuration;
using Subtext.Framework.Format;

namespace Subtext.Framework.Web.HttpModules
{
	/// <summary>
	/// Represents the current blog request.
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
            get { return (BlogRequest)HttpContext.Current.Items[BlogRequestKey]; }
            set { HttpContext.Current.Items[BlogRequestKey] = value; }
		}

		/// <summary>
		/// Initializes a new instance of the <see cref="BlogRequest"/> class.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="subfolder">The subfolder.</param>
		/// <param name="url">The raw requested URL</param>
		/// <param name="isLocal">True if this requset is a local machine request.</param>
		public BlogRequest(string host, string subfolder, Uri url, bool isLocal)
		{
			Host = host;
			Subfolder = subfolder;
			RawUrl = url;
			IsLocal = isLocal;
		}

        public BlogRequest(HttpRequestBase request)
            : this(HostFromRequest(request), SubfolderFromRequest(request), request.Url, request.IsLocal)
        {
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

		public bool IsLocal {
			get;
            private set;
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
