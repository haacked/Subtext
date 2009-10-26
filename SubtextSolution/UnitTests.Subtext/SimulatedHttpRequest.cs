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
using System.IO;
using System.Text;
using System.Web.Hosting;

namespace UnitTests.Subtext
{
    /// <summary>
    /// Used to simulate an HttpRequest.
    /// </summary>
    public class SimulatedHttpRequest : SimpleWorkerRequest
    {
        string _host;
        string _physicalFilePath;
        int _port;
        Uri _referer;
        string _verb;
        private NameValueCollection formVariables = new NameValueCollection();
        private NameValueCollection headers = new NameValueCollection();

        /// <summary>
        /// Creates a new <see cref="SimulatedHttpRequest"/> instance.
        /// </summary>
        /// <param name="applicationPath">App virtual dir.</param>
        /// <param name="physicalAppPath">Physical Path to the app.</param>
        /// <param name="physicalFilePath">Physical Path to the file.</param>
        /// <param name="page">The Part of the URL after the application.</param>
        /// <param name="query">Query.</param>
        /// <param name="output">Output.</param>
        /// <param name="host">Host.</param>
        /// <param name="port">Port to request.</param>
        /// <param name="verb">The HTTP Verb to use.</param>
        public SimulatedHttpRequest(string applicationPath, string physicalAppPath, string physicalFilePath, string page,
                                    string query, TextWriter output, string host, int port, string verb)
            : base(applicationPath, physicalAppPath, page, query, output)
        {
            if(String.IsNullOrEmpty(host))
            {
                throw new ArgumentNullException("host");
            }

            if(applicationPath == null)
            {
                throw new ArgumentNullException("applicationPath");
            }

            _host = host;
            _verb = verb;
            _port = port;
            _physicalFilePath = physicalFilePath;
        }

        /// <summary>
        /// Gets the headers.
        /// </summary>
        /// <value>The headers.</value>
        public NameValueCollection Headers
        {
            get { return headers; }
        }

        /// <summary>
        /// Gets the format exception.
        /// </summary>
        /// <value>The format exception.</value>
        public NameValueCollection Form
        {
            get { return formVariables; }
        }

        internal void SetReferer(Uri referer)
        {
            _referer = referer;
        }

        /// <summary>
        /// Returns the specified member of the request header.
        /// </summary>
        /// <returns>
        /// The HTTP verb returned in the request
        /// header.
        /// </returns>
        public override string GetHttpVerbName()
        {
            return _verb;
        }

        /// <summary>
        /// Gets the name of the server.
        /// </summary>
        /// <returns></returns>
        public override string GetServerName()
        {
            return _host;
        }

        public override int GetLocalPort()
        {
            return _port;
        }

        /// <summary>
        /// Get all nonstandard HTTP header name-value pairs.
        /// </summary>
        /// <returns>An array of header name-value pairs.</returns>
        public override string[][] GetUnknownRequestHeaders()
        {
            if(headers == null || headers.Count == 0)
            {
                return null;
            }
            var headersArray = new string[headers.Count][];
            for(int i = 0; i < headers.Count; i++)
            {
                headersArray[i] = new string[2];
                headersArray[i][0] = headers.Keys[i];
                headersArray[i][1] = headers[i];
            }
            return headersArray;
        }

        public override string GetKnownRequestHeader(int index)
        {
            if(index == 0x24)
            {
                return _referer == null ? string.Empty : _referer.ToString();
            }

            if(index == 12 && _verb == "POST")
            {
                return "application/x-www-form-urlencoded";
            }

            return base.GetKnownRequestHeader(index);
        }

        /// <summary>
        /// Returns the virtual path to the currently executing
        /// server application.
        /// </summary>
        /// <returns>
        /// The virtual path of the current application.
        /// </returns>
        public override string GetAppPath()
        {
            string appPath = base.GetAppPath();
            return appPath;
        }

        public override string GetAppPathTranslated()
        {
            string path = base.GetAppPathTranslated();
            return path;
        }

        public override string GetUriPath()
        {
            string uriPath = base.GetUriPath();
            return uriPath;
        }

        public override string GetFilePathTranslated()
        {
            return _physicalFilePath;
        }

        /// <summary>
        /// Reads request data from the client (when not preloaded).
        /// </summary>
        /// <returns>The number of bytes read.</returns>
        public override byte[] GetPreloadedEntityBody()
        {
            string formText = string.Empty;

            foreach(string key in formVariables.Keys)
            {
                formText += string.Format(CultureInfo.InvariantCulture, "{0}={1}&", key, formVariables[key]);
            }

            return Encoding.UTF8.GetBytes(formText);
        }

        /// <summary>
        /// Returns a value indicating whether all request data
        /// is available and no further reads from the client are required.
        /// </summary>
        /// <returns>
        /// 	<see langword="true"/> if all request data is available; otherwise,
        /// <see langword="false"/>.
        /// </returns>
        public override bool IsEntireEntityBodyIsPreloaded()
        {
            return true;
        }
    }
}