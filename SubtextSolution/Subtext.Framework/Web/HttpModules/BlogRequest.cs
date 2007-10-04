using System;
using System.Web;

namespace Subtext.Framework.Web.HttpModules
{
	/// <summary>
	/// Represents the current blog request.
	/// </summary>
	public struct BlogRequest
	{
		public const int DefaultPort = 80;

		/// <summary>
		/// Gets or sets the current blog request.
		/// </summary>
		/// <value>The current.</value>
		public static BlogRequest Current
		{
			get { return (BlogRequest)HttpContext.Current.Items["Subtext__CurrentRequest"]; }
			set { HttpContext.Current.Items["Subtext__CurrentRequest"] = value; }
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
			this.host = host;
			this.subfolder = subfolder;
			this.rawUrl = url;
			this.isLocal = isLocal;
		}

		public bool IsLocal
		{
			get { return isLocal; }
		}

		private readonly bool isLocal;
		
		/// <summary>
		/// Gets the host.
		/// </summary>
		/// <value>The host.</value>
		public string Host
		{
			get
			{
				return host;
			}
		}
		readonly string host;

	    /// <summary>
		/// Gets the host.
		/// </summary>
		/// <value>The host.</value>
		public string Subfolder
		{
			get
			{
				return subfolder;
			}
			set
			{
				this.subfolder = value;
			}
		}
		string subfolder;

		public Uri RawUrl
		{
			get { return this.rawUrl; }
			set { this.rawUrl = value; }
		}

		private Uri rawUrl;
	}
}
