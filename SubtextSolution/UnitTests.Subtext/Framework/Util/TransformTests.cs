using System;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;

namespace UnitTests.Subtext.Framework.Util
{
	[TestFixture]
	public class TransformTests
	{
		string emoticonsPath = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), "emoticons.txt");

		[Test]
		public void CanLoadEmoticonsFile()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest(UnitTestHelper.GenerateUniqueString(), "");
			UnitTestHelper.UnpackEmbeddedResource("Web.emoticons.txt", emoticonsPath);
			List<string> transforms = Transform.LoadTransformFile(emoticonsPath);
			Assert.AreEqual(48, transforms.Count, "Expected 48 transformations");
			Assert.AreEqual(@"\[\(H\)]", transforms[0], "The first line does not match");
			Assert.AreEqual(@"<img src=""{0}Images/emotions/smiley-cool.gif"" border=""0"" alt=""Cool"" />", transforms[1],
			                "The second line does not match");
		}

		[Test]
        [RollBack2]
		public void CanPerformEmoticonTransform()
		{
			string host = UnitTestHelper.GenerateUniqueString();
			Config.CreateBlog("title", "somebody", "something", host, string.Empty);
			UnitTestHelper.SetHttpContextWithBlogRequest(host, string.Empty);
			UnitTestHelper.UnpackEmbeddedResource("Web.emoticons.txt", emoticonsPath);
			string result = Transform.EmoticonsTransforms("[:'(]", emoticonsPath);
			Assert.AreEqual(string.Format(@"<img src=""http://{0}/Images/emotions/smiley-cry.gif"" border=""0"" alt=""Cry"" /> ", host), result);

			result = Transform.EmoticonsTransforms("Wocka Wocka [:'(] The Whip Master", emoticonsPath);
			Assert.AreEqual(string.Format(@"Wocka Wocka <img src=""http://{0}/Images/emotions/smiley-cry.gif"" border=""0"" alt=""Cry"" />  The Whip Master", host), result);

		}

		[TearDown]
		public void TearDown()
		{
			if (File.Exists(emoticonsPath))
				File.Delete(emoticonsPath);
		}
	}
}
