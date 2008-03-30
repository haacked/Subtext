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

			string scriptTag = ScriptElementCollectionRenderer.RenderScriptElement(skinPath, script);
			Assert.AreEqual(scriptTag, expected + Environment.NewLine, "The rendered script tag was not what we expected.");
		}

        [RowTest]
        [Row("style/test.css", "", "", @"style/test.css")]
        [Row("style/test.css", "", "/Subtext.Web/MyBlog/", @"/Subtext.Web/MyBlog/style/test.css")]
        [Row("~/style/test.css", "Subtext.Web", "/Anything/", @"/Subtext.Web/style/test.css")]
        [Row("~/style/test.css", "", "/Anything/", "/style/test.css")]
        [Row("/style/test.css", "Subtext.Web", "/Anything/", "/style/test.css")]
        [Row("http://haacked.com/style/test.css", "Subtext.Web", "/Anything/", "http://haacked.com/style/test.css")]
        public void GetStylesheetHrefPathRendersAppropriatePath(string src, string virtualDir, string skinPath, string expected)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "Anything", virtualDir);

            Style style = new Style();
            style.Href = src;

            string stylePath = StyleSheetElementCollectionRenderer.GetStylesheetHrefPath(skinPath, style);
            Assert.AreEqual(stylePath, expected, "The rendered style path was not what we expected.");
        }
	}
}
