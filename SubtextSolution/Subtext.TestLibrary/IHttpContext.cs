using System;
using System.Collections;
using System.Security.Permissions;
using System.Security.Principal;
using System.Web;
using System.Web.Caching;
using System.Web.Profile;
using System.Web.SessionState;

namespace Subtext.TestLibrary
{
	public interface IHttpContext
	{
		void AddError(Exception errorInfo);
		void ClearError();
		object GetSection(string sectionName);
		void RewritePath(string path);
		void RewritePath(string path, bool rebaseClientPath);
		void RewritePath(string filePath, string pathInfo, string queryString);
		void RewritePath(string filePath, string pathInfo, string queryString, bool setClientFilePath);
		
		// Properties
		Exception[] AllErrors { get; }
		HttpApplicationState Application { get; }
		HttpApplication ApplicationInstance { get; set; }
		Cache Cache { get; }
		IHttpHandler CurrentHandler { get; }
		RequestNotification CurrentNotification { get;}
		Exception Error { get; }
		IHttpHandler Handler { get; set; }
		bool IsCustomErrorEnabled { get; }
		bool IsDebuggingEnabled { get; }
		bool IsPostNotification { get; }
		IDictionary Items { get; }
		IHttpHandler PreviousHandler { get; }
		ProfileBase Profile { get; }
		IHttpRequest Request { get; }
		IHttpResponse Response { get; }
		HttpServerUtility Server { get; }
		HttpSessionState Session { get; }
		bool SkipAuthorization { get; [SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)] set; }
		DateTime Timestamp { get; }
		TraceContext Trace { get; }
		IPrincipal User { get; [SecurityPermission(SecurityAction.Demand, ControlPrincipal = true)] set; }
	}


}
