#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.IO;
using System.Web.Hosting;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Framework.UI.Skinning;

namespace UnitTests.Subtext.Framework.Skinning
{
    [TestFixture]
    public class SkinStylesTests
    {
        [Test]
        public void CanGetExcludeDefaultStyleAttribute()
        {
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);

            SkinTemplate templateWithTrueExcludedDefault = templates.GetTemplate("RedBook-Blue.css");
            Assert.IsTrue(templateWithTrueExcludedDefault.ExcludeDefaultStyle, "ExcludeDefaultStyle should be True.");

            SkinTemplate templateWithFalseExcludedDefault = templates.GetTemplate("Gradient");
            Assert.IsFalse(templateWithFalseExcludedDefault.ExcludeDefaultStyle, "ExcludeDefaultStyle should be false.");

            SkinTemplate templateWithoutExcludedDefault = templates.GetTemplate("AnotherEon001");
            Assert.IsFalse(templateWithoutExcludedDefault.ExcludeDefaultStyle, "ExcludeDefaultStyle should be false.");
        }

        [Test]
        public void CanGetMergeModeAttribute()
        {
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);

            SkinTemplate templateWithMergedFirstMergeMode = templates.GetTemplate("KeyWest");
            Assert.AreEqual(StyleMergeMode.MergedFirst, templateWithMergedFirstMergeMode.StyleMergeMode, "MergeMode should be MergedFirst.");

            SkinTemplate templateWithMergedAfterMergeMode = templates.GetTemplate("Colors-Blue.css");
            Assert.AreEqual(StyleMergeMode.MergedAfter, templateWithMergedAfterMergeMode.StyleMergeMode, "MergeMode should be MergedAfter.");

            SkinTemplate templateWithNoneMergeMode = templates.GetTemplate("Lightz");
            Assert.AreEqual(StyleMergeMode.None, templateWithNoneMergeMode.StyleMergeMode, "MergeMode should be None.");

            Assert.AreNotEqual(StyleMergeMode.MergedAfter, templateWithNoneMergeMode.StyleMergeMode, "MergeMode should not be MergedAfter.");

            SkinTemplate templateWithoutMergeMode = templates.GetTemplate("Semagogy");
            Assert.AreEqual(StyleMergeMode.None, templateWithoutMergeMode.StyleMergeMode, "MergeMode should be None.");
        }

        [RowTest]
        [Row("", "", "/Skins/Lightz/print.css", "/Skins/Lightz/style.css")]
        [Row("blog", "", "/Skins/Lightz/print.css", "/Skins/Lightz/style.css")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/Lightz/print.css", "/Subtext.Web/Skins/Lightz/style.css")]
        public void StyleSheetElementCollectionRendererRendersPlainCssLinkElementsWithNoneMergeMode(string subFolder, string applicationPath, string expectedPrintCssPath, string expectedDefaultCssPath)
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection("lightz");

            Console.WriteLine(styleElements);

            string printCss = string.Format(@"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""{0}"" />", expectedPrintCssPath);
            Assert.IsTrue(styleElements.IndexOf(printCss) > -1, "Expected the printcss to be there.");

            string defaultCss = string.Format(@"<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />", expectedDefaultCssPath);
            Assert.IsTrue(styleElements.IndexOf(defaultCss) > -1, "Expected the default css to be there.");
        }


        [RowTest]
        [Row("", "", "/Skins/WPSkin/print.css", "/Skins/WPSkin/style.css")]
        [Row("blog", "", "/Skins/WPSkin/print.css", "/Skins/WPSkin/style.css")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/WPSkin/print.css", "/Subtext.Web/Skins/WPSkin/style.css")]
        public void StyleSheetElementCollectionRendererRendersPlainCssLinkElementsWithNoneMergeModeAndExcludeDefault(string subFolder, string applicationPath, string expectedPrintCssPath, string expectedDefaultCssPath)
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection("WPSkin");

            Console.WriteLine(styleElements);

            string printCss = string.Format(@"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""{0}"" />", expectedPrintCssPath);
            Assert.IsTrue(styleElements.IndexOf(printCss) > -1, "Expected the print css to be there.");

            string defaultCss = string.Format(@"<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />", expectedDefaultCssPath);
            Assert.IsTrue(styleElements.IndexOf(defaultCss) == -1, "Not expected the default css to be there.");
        }

        [RowTest]
        [Row("", "", "/Skins/Nature/print.css", "/Skins/Nature/style.css", "/Skins/Nature/rain.css")]
        [Row("blog", "", "/Skins/Nature/print.css", "/Skins/Nature/style.css", "/Skins/Nature/rain.css")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/Nature/print.css", "/Subtext.Web/Skins/Nature/style.css", "/Subtext.Web/Skins/Nature/rain.css")]
        public void StyleSheetElementCollectionRendererRendersPlainCssLinkElementsWithNoneMergeModeAndSecondaryStyle(string subFolder, string applicationPath, string expectedPrintCssPath, string expectedDefaultCssPath, string expectedSecondaryCssPath)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection("Nature-rain.css");

            Console.WriteLine(styleElements);

            string printCss = string.Format(@"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""{0}"" />", expectedPrintCssPath);
            Assert.IsTrue(styleElements.IndexOf(printCss) > -1, "Expected the printcss to be there.");

            string defaultCss = string.Format(@"<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />", expectedDefaultCssPath);
            Assert.IsTrue(styleElements.IndexOf(defaultCss) > -1, "Expected the default css to be there.");

            string secondaryCss = string.Format(@"<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />", expectedSecondaryCssPath);
            Assert.IsTrue(styleElements.IndexOf(secondaryCss) > -1, "Expected the secondary css to be there.");
        }


        [RowTest]
        [Row("KeyWest", true)]
        [Row("Gradient", false)]
        public void StyleSheetElementCollectionRendererRendersLinkElementsInRightOrder(string skinKey, bool expectedFirst)
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);

            string styleElements = renderer.RenderStyleElementCollection(skinKey);
            SkinTemplate template = templates.GetTemplate(skinKey);


            Console.WriteLine(styleElements);
            styleElements = styleElements.Trim('\r', '\n');
            string mergedCss = @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/" + template.TemplateFolder + "/css.axd?name=" + skinKey + @""" />";
            if (expectedFirst)
                Assert.IsTrue(styleElements.StartsWith(mergedCss), "Merged CSS is not in first position");
            else
                Assert.IsTrue(styleElements.EndsWith(mergedCss), "Merged CSS is not in last position");
        }


        [RowTest]
        [Row("", "", "/Skins/RedBook/css.axd?name=RedBook-Green.css&media=print")]
        [Row("blog", "", "/Skins/RedBook/css.axd?name=RedBook-Green.css&media=print")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/RedBook/css.axd?name=RedBook-Green.css&media=print")]
        public void StyleSheetElementCollectionRendererRendersMergedCssLinkElements(string subFolder, string applicationPath, string expectedPrintCssPath)
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection("RedBook-Green.css");

            Console.WriteLine(styleElements);

            string printCss = string.Format(@"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""{0}"" />", expectedPrintCssPath);
            Assert.IsTrue(styleElements.IndexOf(printCss) > -1, "Expected the printcss to be there.");
        }

        [RowTest]
        [Row("AnotherEon001", @"<link type=""text/css"" rel=""stylesheet"" href=""http://haacked.com/skins/_System/commonstyle.css"" />" + "\r\n" + @"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""/Skins/AnotherEon001/css.axd?name=AnotherEon001&media=print"" />")]
        [Row("Colors-Blue.css", @"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Colors/css.axd?name=Colors-Blue.css&media=print"" />")]
        //[Row("RedBook-Blue.css", 6)]
        [Row("Gradient", "<!--[if IE]>\r\n" + @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Gradient/css.axd?name=Gradient&media=screen&conditional=if+IE"" />" + "\r\n<![endif]-->\r\n" + @"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Gradient/css.axd?name=Gradient&media=print"" />")]
        [Row("RedBook-Green.css", "<!--[if IE]>\r\n" + @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/RedBook/css.axd?name=RedBook-Green.css&conditional=if+IE"" />" + "\r\n<![endif]-->\r\n" + @"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""/Skins/RedBook/css.axd?name=RedBook-Green.css&media=print"" />")]
        //[Row("KeyWest", 4)]
        [Row("Nature-Leafy.css", @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Nature/css.axd?name=Nature-Leafy.css&media=screen"" />" + "\r\n" + @"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Nature/css.axd?name=Nature-Leafy.css&media=print"" />")]
        //[Row("Lightz", 4)]
        //[Row("Naked", 1)]
        //[Row("Colors", 5)]
        [Row("Origami", @"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Origami/css.axd?name=Origami&media=print"" />" + "\r\n" + @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Origami/css.axd?name=Origami&media=screen"" />")]
        [Row("Piyo", @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Piyo/css.axd?name=Piyo&media=screen"" />" + "\r\n" + @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" title=""fixed"" href=""/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed"" />" + "\r\n" + @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" title=""elastic"" href=""/Skins/Piyo/css.axd?name=Piyo&media=screen&title=elastic"" />" + "\r\n" + @"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Piyo/css.axd?name=Piyo&media=print"" />")]
        //[Row("Nature-rain.css", 7)]
        //[Row("RedBook-Red.css", 6)]
        //[Row("Semagogy", 4)]
        [Row("Submarine", "<!--[if IE]>\r\n" + @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/Submarine/css.axd?name=Submarine&conditional=if+IE"" />" + "\r\n<![endif]-->\r\n" + @"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Submarine/css.axd?name=Submarine&media=print"" />")]
        //[Row("WPSkin", 4)]
        public void CallsToCssHandlerAreNotRepeated(string skinKey, string exptectedElements)
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection(skinKey);
            SkinTemplate template = templates.GetTemplate(skinKey);
            Console.WriteLine(styleElements);

            string mergedCss = @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/" + template.TemplateFolder + "/css.axd?name=" + skinKey + @""" />";
            styleElements = styleElements.Replace(mergedCss, string.Empty);
            Assert.IsTrue(styleElements.Trim('\r', '\n').Equals(exptectedElements), "Not the expected stylesheet links");
        }


        [RowTest]
        [Row("", "print", "", "print.css", false)]
        [Row("", "", "", "~/skins/_System/csharp.css", true)]
        [Row("if gte IE 7", "", "", "IE7Patches.css", false)]
        [Row("", "screen", "", "~/scripts/lightbox.css", false)]
        [Row("", "all", "", "Styles/user-styles.css", true)]
        [Row("", "", "fixed", "print.css", false)]
        [Row("", "all", "fixed", "Styles/user-styles.css", false)]
        [Row("if gte IE 7", "all", "", "Styles/user-styles.css", false)]
        [Row("", "", "", "http://www.google.com/style.css", false)]
        public void StyleToBeMergedAreCorrectlyDetected(string conditional, string media, string title, string href, bool canBeMerged)
        {
            Style style = new Style();
            style.Conditional = conditional;
            style.Media = media;
            style.Href = href;
            style.Title = title;

            bool isMergeable = StyleSheetElementCollectionRenderer.CanStyleBeMerged(style);
            if (canBeMerged)
                Assert.IsTrue(isMergeable, "Expected to be mergeable");
            else
                Assert.IsFalse(isMergeable, "Expected not to be mergeable");
        }

        [Test]
        public void MergedCssDoesntContainDefaultIfExcluded()
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            List<string> mergedStyles = (List<string>)renderer.GetStylesToBeMerged("WPSkin");

            Assert.IsFalse(mergedStyles.Contains("/Skins/WPSkin/style.css"), "Skin WPSkin should not have the default style.css");
        }

        [Test]
        public void MergedCssContainsDefaultStyle()
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            List<string> mergedStyles = (List<string>)renderer.GetStylesToBeMerged("Submarine");

            Assert.IsTrue(mergedStyles.Contains("/Skins/Submarine/style.css"), "Skin Submarine should have the default style.css");
        }

        [RowTest]
        [Row("AnotherEon001", 3)]
        [Row("Colors-Blue.css", 6)]
        [Row("RedBook-Blue.css", 5)]
        [Row("Gradient", 4)]
        [Row("RedBook-Green.css", 6)]
        [Row("KeyWest", 4)]
        [Row("Nature-Leafy.css", 6)]
        [Row("Lightz", 4)]
        [Row("Naked", 1)]
        [Row("Colors", 5)]
        [Row("Origami", 5)]
        [Row("Piyo", 4)]
        [Row("Nature-rain.css", 7)]
        [Row("RedBook-Red.css", 6)]
        [Row("Semagogy", 4)]
        [Row("Submarine", 6)]
        [Row("WPSkin", 3)]
        [Row("Haacked", 0)]
        public void MergedCssIsCorrect(string skinKey, int expectedStyles)
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            int mergedStyles = renderer.GetStylesToBeMerged(skinKey).Count;

            Assert.AreEqual(expectedStyles, mergedStyles, String.Format("Skin {0} should have {1} merged styles but found {2}", skinKey, expectedStyles, mergedStyles));
        }

        [RowTest]
        [Row("AnotherEon001", 1, "print", null, null)]
        [Row("Colors-Blue.css", 1, "print", null, null)]
        [Row("RedBook-Blue.css", 1, "print", null, null)]
        [Row("RedBook-Blue.css", 1, null, null, "if IE")]
        [Row("Gradient", 1, "print", null, null)]
        [Row("Gradient", 0, "screen", null, null)]
        [Row("Gradient", 0, null, null, "if IE")]
        [Row("Gradient", 1, "screen", null, "if IE")]
        [Row("RedBook-Green.css", 1, "print", null, null)]
        [Row("KeyWest", 1, "print", null, null)]
        [Row("Nature-Leafy.css", 1, "print", null, null)]
        [Row("Nature-Leafy.css", 1, "screen", null, null)]
        [Row("Lightz", 1, "print", null, null)]
        [Row("Naked", 0, "print", null, null)]
        [Row("Colors", 1, "print", null, null)]
        [Row("Origami", 1, "print", null, null)]
        [Row("Origami", 2, "screen", null, null)]
        [Row("Piyo", 1, "print", null, null)]
        [Row("Piyo", 1, "screen", "fixed", null)]
        [Row("Piyo", 1, "screen", "elastic", null)]
        [Row("Piyo", 1, "screen", null, null)]
        [Row("Nature-rain.css", 1, "print", null, null)]
        [Row("RedBook-Red.css", 1, "print", null, null)]
        [Row("Semagogy", 1, "print", null, null)]
        [Row("Submarine", 1, "print", null, null)]
        [Row("Submarine", 2, null, null, "if IE")]
        [Row("WPSkin", 1, "print", null, null)]
        public void MergeCssWithAttributes(string skinKey, int expectedStyles, string media, string title, string conditional)
        {
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty, string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplates templates = SkinTemplates.Instance(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            int mergedStyles = renderer.GetStylesToBeMerged(skinKey, media, title, conditional).Count;

            Assert.AreEqual(expectedStyles, mergedStyles, String.Format("Skin {0} should have {1} merged styles but found {2}", skinKey, expectedStyles, mergedStyles));
        }

        private static VirtualPathProvider GetTemplatesPathProviderMock(MockRepository mocks)
        {
            VirtualPathProvider pathProvider = (VirtualPathProvider)mocks.CreateMock(typeof(VirtualPathProvider));
            VirtualFile vfile = (VirtualFile)mocks.CreateMock(typeof(VirtualFile), "~/Admin/Skins.config");
            Expect.Call(pathProvider.GetFile("~/Admin/Skins.config")).Return(vfile);
            Expect.Call(pathProvider.FileExists("~/Admin/Skins.User.config")).Return(false);
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("Skins.Skins.config");
            Expect.Call(vfile.Open()).Return(stream);
            return pathProvider;
        }
    }
}
