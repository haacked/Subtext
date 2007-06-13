using System;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Framework;
using Subtext.Framework.Services;

namespace UnitTests.Subtext.Akismet
{
	[TestFixture]
	public class AkismetSpamServiceTests
	{
		[Test]
		public void CanInstantiate()
		{
			MockRepository mocks = new MockRepository();
			IBlogInfo blog = mocks.CreateMock<IBlogInfo>();
			Uri rootUrl = new Uri("http://localhost:2072");
			Expect.Call(blog.RootUrl).Return(rootUrl);
			mocks.ReplayAll();

			AkismetSpamService service = new AkismetSpamService("api-key", blog);
			Assert.IsNotNull(service);
		}
	}
}
