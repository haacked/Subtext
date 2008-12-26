using System;
using System.Web;

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

		public bool IsLocal
		{
			get;
            private set;
		}
		
		/// <summary>
		/// Gets the host.
		/// </summary>
		/// <value>The host.</value>
		public string Host
		{
			get;
            private set;
		}

	    /// <summary>
		/// Gets the host.
		/// </summary>
		/// <value>The host.</value>
		public string Subfolder
		{
			get;
            private set;
		}

		public Uri RawUrl
		{
			get;
			private set;
		}
	}
}
