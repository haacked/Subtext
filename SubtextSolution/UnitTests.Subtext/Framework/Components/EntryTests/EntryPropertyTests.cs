using System;
using MbUnit.Framework;
using Subtext.Extensibility;
using Subtext.Framework.Components;

namespace UnitTests.Subtext.Framework.Components.EntryTests
{
	[TestFixture]
	public class EntryPropertyTests
	{
		[Test]
		public void AlternativeTitleUrlShouldNotUseTheUrlValue()
		{
			Entry entry = new Entry(PostType.BlogPost);
			Assert.IsNull(entry.AlternativeTitleUrl);
			entry.Url = "/BlahBlah/Blah.aspx";
			Assert.IsNull(entry.AlternativeTitleUrl, "Since we didn't set the title Url, it should remain null.");
			Assert.AreEqual(entry.Url, entry.TitleUrl, "Since AlternativeTitleUrl is null, entry.TitleUrl should be the same as entry.Url.");
		}
	}
}
