using System.Threading;
using MbUnit.Framework;

namespace WatinTests.Tests.Admin
{
	[TestFixture(ApartmentState = ApartmentState.STA)]
	public class ConfigureTests
	{
		[Test]
		public void CanChangeBlogTitle()
		{
			using(Browser browser = new Browser())
			{
				ConfigPage page = browser.GoTo<ConfigPage>();
				
				page.TitleField.Value = "Title Changed by Watin";
				page.SubtitleField.Value = "Subtitle Changed by Watin";
				page.ClickSave();
				page.ClickNavLink(ConfigNavigationLink.Configure);

				Assert.AreEqual("Title Changed by Watin", page.TitleField.Value, "Expected new title");
				Assert.AreEqual("Subtitle Changed by Watin", page.SubtitleField.Value, "Expected new subtitle");
			}
		}
	}
}
