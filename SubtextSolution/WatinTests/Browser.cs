using System;
using System.Configuration;
using System.Text.RegularExpressions;
using MbUnit.Framework;
using WatiN.Core;

namespace WatinTests
{
	/// <summary>
	/// Use this instead of IE. It encapsulates Subtext specific logic.
	/// </summary>
	public class Browser : IE
	{
        public Browser() : base() { }
        public Browser(Uri url) : base(url) { }

		public static Uri HomeUrl
		{
			get
			{
				return new Uri(string.Format("http://{0}:{1}/", ConfigurationManager.AppSettings["webServer"],
									 ConfigurationManager.AppSettings["Port"]));
			}
		}

		public static Uri GetUrl(string relativeUrl)
		{
			return new Uri(HomeUrl, relativeUrl);
		}

		public T GoTo<T>() where T : BrowserPageBase
		{
			T page = (T)Activator.CreateInstance(typeof(T), this);
			page.GoToUrl(page.PageUrl);
			if (IsOnLoginPage)
				Login("username", "password");
			return page;
		}

		public void GoToAdmin()
		{
			GoTo(GetUrl("/Admin/"));
		}

		public void GoToHostAdmin()
		{
			GoTo(GetUrl("/HostAdmin/"));
		}

		public void GoToUrl(string relativeUrl)
		{
			GoTo(GetUrl(relativeUrl));
		}

		public bool IsOnLoginPage
		{
			get
			{
				return ContainsText("Please Sign In");
			}
		}

		public TextField ASPTextField(string id)
		{
			return TextField(new Regex(".*" + id));
		}

		public Button ButtonByValue(string value)
		{
			return Button(Find.ByValue(value));
		}

		/// <summary>
		/// Runs through the entire installation process. Assumes that the 
		/// site is not installed with a clean database.
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		public void StepThroughInstallation(string username, string password)
		{
			Assert.IsTrue(ContainsText("Welcome to the Subtext Installation Wizard"), "We don't appear to be on the installation wizard.");
			Button(Find.ByValue("On to Step 2")).Click();
			Button(Find.ByValue("Install Now!")).Click();

			Assert.IsTrue(ContainsText("Host Configuration"), "Should be on the host configuration step.");

			TextField(new Regex(".*txtUserName")).Value = username;
			TextField(new Regex(".*txtPassword")).Value = password;
			TextField(new Regex(".*txtConfirmPassword")).Value = password;

			Button(Find.ByValue("Save")).Click();
			Button(Find.ByValue("Quick Create")).Click();
		}

		public void Login(string username, string password)
		{
			ASPTextField("tbUserName").Value = username;
			ASPTextField("tbPassword").Value = password;
			ButtonByValue("Login").Click();
		}

		/// <summary>
		/// Navigates to the configure page and presents methods and 
		/// properties specific to that page.
		/// </summary>
		/// <value>The configure page.</value>
		public ConfigPage ConfigurePage
		{
			get
			{
				if(config == null)
					config = new ConfigPage(this);
				return config;
			}
		}

		private ConfigPage config = null;


	}
}
