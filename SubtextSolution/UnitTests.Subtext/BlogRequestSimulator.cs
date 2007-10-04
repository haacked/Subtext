using System;
using System.Security.Principal;
using System.Threading;
using System.Web;
using Subtext.Framework;
using Subtext.Framework.Security;
using Subtext.Framework.Web.HttpModules;
using Subtext.TestLibrary;

namespace UnitTests.Subtext
{
	public class BlogRequestSimulator : IDisposable
	{
		private IDisposable membershipScope;
		private HttpSimulator simulator;

		private BlogRequestSimulator(string applicationPath)
		{
			simulator = new HttpSimulator(applicationPath);
		}

		public static BlogRequestSimulator SimulateRequest(BlogInfo blog, string host, string application, string subfolder)
		{
			BlogRequestSimulator blogRequest = SimulateRequest(host, application, subfolder);
			HttpContext.Current.Cache["BlogInfo-" + subfolder] = blog;
			blogRequest.membershipScope = MembershipApplicationScope.SetApplicationName(blog.ApplicationName);
			if (blog.Owner != null)
			{
				Thread.CurrentPrincipal =
					new GenericPrincipal(new GenericIdentity(blog.Owner.UserName), new string[] {RoleNames.Administrators});
				HttpContext.Current.User = Thread.CurrentPrincipal;
			}
			return blogRequest;
		}

		public static BlogRequestSimulator SimulateRequest(string host, string application, string subfolder)
		{
			BlogRequestSimulator blogRequest = new BlogRequestSimulator(application);
			Uri requestedUrl = new Uri("http://" + host + "/" + application + "/" + subfolder + "/default.aspx");
			blogRequest.simulator.SimulateRequest(requestedUrl);
			BlogRequest.Current = new BlogRequest(host, subfolder, requestedUrl, false);
			return blogRequest;
		}

		///<summary>
		///Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
		///</summary>
		///<filterpriority>2</filterpriority>
		public void Dispose()
		{
			if(simulator != null)
				simulator.Dispose();

			if(membershipScope != null)
				membershipScope.Dispose();

		}
	}
}
