using System;
using System.IO;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.BlogML;
using Subtext.BlogML.Conversion;
using Subtext.BlogML.Interfaces;

namespace UnitTests.Subtext.BlogML
{
	[TestFixture]
	public class BlogMLReaderTests
	{
		[Test]
		[ExpectedArgumentNullException]
		public void CreateReaderWithNoProviderThrowsArgumentNullException()
		{
			BlogMLReader.Create(null);
		}

		[Test]
		[ExpectedArgumentNullException]
		public void ReadBlogWithNullStreamThrowsException()
		{
			MockRepository mocks = new MockRepository();
			IBlogMLProvider provider = (IBlogMLProvider)mocks.CreateMock(typeof(IBlogMLProvider));
			mocks.ReplayAll();
			BlogMLReader reader = BlogMLReader.Create(provider);
			reader.ReadBlog(null);
			mocks.VerifyAll();
		}
		
		[Test]
		public void CanCreateReaderWithProvider()
		{
			MockRepository mocks = new MockRepository();
			IBlogMLProvider provider = (IBlogMLProvider)mocks.CreateMock(typeof(IBlogMLProvider));
			mocks.ReplayAll();
			BlogMLReader.Create(provider);
			mocks.VerifyAll();
		}

		[Test]
		public void ImportCallsPreAndCompleteMethods()
		{
			MockRepository mocks = new MockRepository();
			IBlogMLProvider provider = (IBlogMLProvider)mocks.DynamicMock(typeof(IBlogMLProvider));
			provider.PreImport();
			LastCall.On(provider).IgnoreArguments();
			SetupResult.For(provider.IdConversion).Return(IdConversionStrategy.Empty);
			provider.ImportComplete();
			LastCall.On(provider).IgnoreArguments();
			
			mocks.ReplayAll();
			BlogMLReader reader = BlogMLReader.Create(provider);
			using (Stream stream = UnitTestHelper.UnpackEmbeddedResource("BlogMl.SimpleBlogMl.xml"))
			{
				reader.ReadBlog(stream);
			}
			mocks.VerifyAll();
		}
	}
}
