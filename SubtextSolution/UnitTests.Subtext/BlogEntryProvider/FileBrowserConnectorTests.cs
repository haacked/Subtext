using System;
using MbUnit.Framework;
using Subtext.Providers.BlogEntryEditor.FCKeditor;

namespace UnitTests.Subtext.BlogEntryProvider
{
	[TestFixture]
	public class FileBrowserConnectorTests
	{
		[Test]
		public void FileBrowserSecureCreationTests()
		{
			UnitTestHelper.AssertSecureCreation<FileBrowserConnector>(new string[] {"Admins"});
		}
	}
}
