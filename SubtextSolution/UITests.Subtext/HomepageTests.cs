using System;
using System.Text.RegularExpressions;
using System.Threading;
using MbUnit.Framework;
using WatiN.Core;

namespace UITests.Subtext
{
	/// <summary>
	/// These are Watin tests. They require running the tests as Admin on Vista.
	/// </summary>
	[TestFixture(ApartmentState = ApartmentState.STA)]
	public class HomepageTests
	{
		[Test]
		public void CanClickArchives()
		{
			using (IE ie = new IE("http://localhost:2732/Default.aspx"))
			{
				ie.Link(Find.ById("MyLinks_Archives")).Click();
				ie.WaitForComplete();
				Assert.IsTrue(Regex.IsMatch(ie.Html, @"<h\d>Archives</h\d>", RegexOptions.IgnoreCase), "Could not find the Archives Heading.");
			}
		}
	}
}
