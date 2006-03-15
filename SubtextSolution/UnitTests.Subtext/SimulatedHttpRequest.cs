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
using System.Collections.Specialized;
using System.IO;
using System.Web.Hosting;

namespace UnitTests.Subtext
{
	/// <summary>
	/// Used to simulate an HttpRequest.
	/// </summary>
	public class SimulatedHttpRequest : SimpleWorkerRequest
	{
		string _host;

		/// <summary>
		/// Creates a new <see cref="SimulatedHttpRequest"/> instance.
		/// </summary>
		/// <param name="appVirtualDir">App virtual dir.</param>
		/// <param name="appPhysicalDir">App physical dir.</param>
		/// <param name="page">Page.</param>
		/// <param name="query">Query.</param>
		/// <param name="output">Output.</param>
		/// <param name="host">Host.</param>
		public SimulatedHttpRequest(string appVirtualDir, string appPhysicalDir, string page, string query, TextWriter output, string host) : base(appVirtualDir, appPhysicalDir, page, query, output)
		{
			if(host == null || host.Length == 0)
				throw new ArgumentNullException("host", "Host cannot be null nor empty.");

			if(appVirtualDir == null)
				throw new ArgumentNullException("appVirtualDir", "Can't create a request with a null virtual dir. Try empty string.");

			if(appVirtualDir.Length > 0)
				Console.WriteLine("Debug: AppVirtualDir = " + appVirtualDir);
			else
				Console.WriteLine("Debug: Empty Virtual Dir");
			
			_host = host;
		}

		/// <summary>
		/// Gets the name of the server.
		/// </summary>
		/// <returns></returns>
		public override string GetServerName()
		{
			return _host;
		}

		public NameValueCollection Headers
		{
			get
			{
				return this.headers;
			}
		}

		private NameValueCollection headers = new NameValueCollection();

		public override string[][] GetUnknownRequestHeaders()
		{
			if(this.headers == null || this.headers.Count == 0)
			{
				return null;
			}
			string[][] headersArray = new string[this.headers.Count][];
			for(int i = 0; i < this.headers.Count; i++)
			{
				headersArray[i] = new string[2];
				headersArray[i][0] = this.headers.Keys[i];
				headersArray[i][1] = this.headers[i];
			}
			return headersArray;
		}

		/// <summary>
		/// Maps the path to a filesystem path.
		/// </summary>
		/// <param name="virtualPath">Virtual path.</param>
		/// <returns></returns>
		public override string MapPath(string virtualPath)
		{
			return Path.Combine(this.GetAppPath(), virtualPath);
		}

		public override string GetAppPath()
		{
			string appPath = base.GetAppPath();
			Console.WriteLine("DEBUG: Calling GetAppPath()... returning {" + appPath + "}");
			return appPath;
		}


	}
}
