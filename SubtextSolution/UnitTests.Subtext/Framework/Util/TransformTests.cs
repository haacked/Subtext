using System;
using System.Collections.Generic;
using System.IO;
using MbUnit.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Util;
using Subtext.TestLibrary;

namespace UnitTests.Subtext.Framework.Util
{
	[TestFixture]
	public class TransformTests
	{
		[Test]
		[ExtractResource("UnitTests.Subtext.Resources.Web.emoticons.txt", typeof(TransformTests))]
		[RollBack2]
		public void CanLoadEmoticonsFile()
		{
			//Due to a bug in ExtractResource...
			using (StreamReader reader = new StreamReader(ExtractResourceAttribute.Stream))
			using (StreamWriter writer = new StreamWriter("emoticons.txt"))
			{
				writer.Write(reader.ReadToEnd());
			}

			using (new HttpSimulator().SimulateRequest(new Uri("http://" + UnitTestHelper.GenerateRandomString() + "/")))
			{
				List<string> transforms = Transform.LoadTransformFile("emoticons.txt");
				Assert.AreEqual(48, transforms.Count, "Expected 48 transformations");
				Assert.AreEqual(@"\[\(H\)]", transforms[0], "The first line does not match");
				Assert.AreEqual(@"<img src=""{0}Images/emotions/smiley-cool.gif"" border=""0"" alt=""Cool"" />", transforms[1],
				                "The second line does not match");
			}
		}

		[Test]
		[ExtractResource("UnitTests.Subtext.Resources.Web.emoticons.txt", typeof(TransformTests))]
		[RollBack2]
		public void CanPerformEmoticonTransform()
		{
			//Due to a bug in ExtractResource...
			using (StreamReader reader = new StreamReader(ExtractResourceAttribute.Stream))
			using (StreamWriter writer = new StreamWriter("emoticons.txt"))
			{
				writer.Write(reader.ReadToEnd());
			}

			UnitTestHelper.SetupBlog();
			string result = Transform.EmoticonsTransforms("[:'(]", "emoticons.txt");
			Assert.AreEqual(string.Format(@"<img src=""http://{0}/Images/emotions/smiley-cry.gif"" border=""0"" alt=""Cry"" /> ", Config.CurrentBlog.Host), result);

			result = Transform.EmoticonsTransforms("Wocka Wocka [:'(] The Whip Master", Path.GetFullPath("emoticons.txt"));
			Assert.AreEqual(string.Format(@"Wocka Wocka <img src=""http://{0}/Images/emotions/smiley-cry.gif"" border=""0"" alt=""Cry"" />  The Whip Master", Config.CurrentBlog.Host), result);

		}
	}
}
