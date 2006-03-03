using System;
using Subtext.Framework.UI.Skinning;
using Subtext.Web.UI.Pages;
using MbUnit.Framework;

namespace UnitTests.Subtext.SubtextWeb
{
	/// <summary>
	/// Unit tests of the <see cref="SubtextMasterPage"/> class.
	/// </summary>
	[TestFixture]
	public class SubtextMasterPageTests
	{
		[RowTest]
		[Row("javascript", "scripts/test.js", "", "", @"<script type=""javascript"" src=""scripts/test.js""></script>")]
		[Row("javascript", "scripts/test.js", "", "/Subtext.Web/MyBlog/", @"<script type=""javascript"" src=""/Subtext.Web/MyBlog/scripts/test.js""></script>")]
		[Row("javascript", "~/scripts/test.js", "Subtext.Web", "/Anything/", @"<script type=""javascript"" src=""/Subtext.Web/scripts/test.js""></script>")]
		[Row("javascript", "~/scripts/test.js", "", "/Anything/", @"<script type=""javascript"" src=""/scripts/test.js""></script>")]
		[Row("javascript", "/scripts/test.js", "Subtext.Web", "/Anything/", @"<script type=""javascript"" src=""/scripts/test.js""></script>")]
		public void RenderScriptElementRendersAppropriatePath(string type, string src, string virtualDir, string skinPath, string expected)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Anything", virtualDir);

			Script script = new Script();
			script.Type = type;
			script.Src = src;

			string scriptTag = SubtextMasterPage.ScriptElementCollectionRenderer.RenderScriptElement(skinPath, script);
			Assert.AreEqual(scriptTag, expected + Environment.NewLine, "The rendered script tag was not what we expected.");
		}
	}
}
