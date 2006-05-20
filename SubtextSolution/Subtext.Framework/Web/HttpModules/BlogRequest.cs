using System;

namespace Subtext.Framework.Web.HttpModules
{
	/// <summary>
	/// Represents the current blog request.
	/// </summary>
	public struct BlogRequest
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="BlogRequest"/> class.
		/// </summary>
		/// <param name="host">The host.</param>
		/// <param name="subfolder">The subfolder.</param>
		public BlogRequest(string host, string subfolder)
		{
			this.host = host;
			this.subfolder = subfolder;
		}
		
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
		string host;
		
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
	}
}
