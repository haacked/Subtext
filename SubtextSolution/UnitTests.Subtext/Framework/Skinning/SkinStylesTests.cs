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

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);

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

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);

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
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection("lightz");

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
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection("WPSkin");

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
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection("Nature-rain.css");

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
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);

            string styleElements = renderer.RenderStyleElementCollection(skinKey);
            SkinTemplate template = templates.GetTemplate(skinKey);

            styleElements = styleElements.Trim('\r', '\n');
            string mergedCss = @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/" + template.TemplateFolder + "/css.axd?name=" + skinKey + @""" />";
            if (expectedFirst)
                Assert.IsTrue(styleElements.StartsWith(mergedCss), "Merged CSS is not in first position");
            else
                Assert.IsTrue(styleElements.EndsWith(mergedCss), "Merged CSS is not in last position");
        }


        [RowTest]
        [Row("", "", "/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed")]
        [Row("blog", "", "/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed")]
        public void StyleSheetElementCollectionRendererRendersMergedCssLinkElements(string subFolder, string applicationPath, string expectedPrintCssPath)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection("Piyo");

            string printCss = string.Format(@"<link media=""screen"" type=""text/css"" rel=""stylesheet"" title=""fixed"" href=""{0}"" />", expectedPrintCssPath);
            Assert.IsTrue(styleElements.IndexOf(printCss) > -1, "Expected the fixed screen css to be there.");
        }

        [RowTest]
        [Row("AnotherEon001", @"<link type=""text/css"" rel=""stylesheet"" href=""http://haacked.com/skins/_System/commonstyle.css"" />")]
        [Row("Colors-Blue.css", "")]
        //[Row("RedBook-Blue.css", 6)]
        [Row("Gradient", "<!--[if IE]>\r\n" + @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Gradient/css.axd?name=Gradient&media=screen&conditional=if+IE"" />" + "\r\n<![endif]-->")]
        [Row("RedBook-Green.css", "<!--[if IE]>\r\n" + @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/RedBook/css.axd?name=RedBook-Green.css&conditional=if+IE"" />" + "\r\n<![endif]-->")]
        //[Row("KeyWest", 4)]
        [Row("Nature-Leafy.css", "")]
        //[Row("Lightz", 4)]
        //[Row("Naked", 1)]
        //[Row("Colors", 5)]
        [Row("Origami", "")]
        [Row("Piyo", @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" title=""fixed"" href=""/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed"" />" + "\r\n" + @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" title=""elastic"" href=""/Skins/Piyo/css.axd?name=Piyo&media=screen&title=elastic"" />")]
        //[Row("Nature-rain.css", 7)]
        //[Row("RedBook-Red.css", 6)]
        //[Row("Semagogy", 4)]
        [Row("Submarine", "<!--[if IE]>\r\n" + @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/Submarine/css.axd?name=Submarine&conditional=if+IE"" />" + "\r\n<![endif]-->")]
        //[Row("WPSkin", 4)]
        public void CallsToCssHandlerAreNotRepeated(string skinKey, string exptectedElements)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            string styleElements = renderer.RenderStyleElementCollection(skinKey);
            SkinTemplate template = templates.GetTemplate(skinKey);

            string mergedCss = @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/" + template.TemplateFolder + "/css.axd?name=" + skinKey + @""" />";
            styleElements = styleElements.Replace(mergedCss, string.Empty);
            Assert.IsTrue(styleElements.Trim('\r', '\n').Equals(exptectedElements), "Not the expected stylesheet links");
        }


        [RowTest]
        [Row("", "print", "", "print.css", true)]
        [Row("", "print", "fixed", "print.css", false)]
        [Row("", "", "", "~/skins/_System/csharp.css", true)]
        [Row("if gte IE 7", "", "", "IE7Patches.css", false)]
        [Row("", "screen", "", "~/scripts/lightbox.css", true)]
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
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            List<StyleDefinition> mergedStyles = (List<StyleDefinition>)renderer.GetStylesToBeMerged("WPSkin");

            Assert.IsFalse(mergedStyles.Contains(new StyleDefinition("/Skins/WPSkin/style.css")), "Skin WPSkin should not have the default style.css");
        }

        [Test]
        public void MergedCssContainsDefaultStyle()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            List<StyleDefinition> mergedStyles = (List<StyleDefinition>)renderer.GetStylesToBeMerged("Submarine");

            Assert.IsTrue(mergedStyles.Contains(new StyleDefinition("/Skins/Submarine/style.css")), "Skin Submarine should have the default style.css");
        }


        [Test]
        public void MergedCssContainsStyleWithMedia()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            List<StyleDefinition> mergedStyles = (List<StyleDefinition>)renderer.GetStylesToBeMerged("Piyo");

            Assert.IsTrue(mergedStyles.Contains(new StyleDefinition("/Skins/Piyo/print.css", "print")), "Skin Piyo should have the print css in the merged css");
        }

        [Test]
        public void MergedCssDoesntContainStyleWithMediaAndTitle()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
            StyleSheetElementCollectionRenderer renderer = new StyleSheetElementCollectionRenderer(templates);
            List<StyleDefinition> mergedStyles = (List<StyleDefinition>)renderer.GetStylesToBeMerged("Piyo");

            Assert.IsFalse(mergedStyles.Contains(new StyleDefinition("/Skins/Piyo/piyo-fixed.css", "screen")), "Skin Piyo should not have the fixed screen css in the merged CSS");
        }

        [RowTest]
        [Row("AnotherEon001", 4)]
        [Row("Colors-Blue.css", 7)]
        [Row("RedBook-Blue.css", 6)]
        [Row("Gradient", 5)]
        [Row("RedBook-Green.css", 7)]
        [Row("KeyWest", 5)]
        [Row("Nature-Leafy.css", 8)]
        [Row("Lightz", 5)]
        [Row("Naked", 1)]
        [Row("Colors", 6)]
        [Row("Origami", 8)]
        [Row("Piyo", 6)]
        [Row("Nature-rain.css", 8)]
        [Row("RedBook-Red.css", 7)]
        [Row("Semagogy", 5)]
        [Row("Submarine", 7)]
        [Row("WPSkin", 4)]
        [Row("Haacked", 0)]
        public void MergedCssIsCorrect(string skinKey, int expectedStyles)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
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
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            MockRepository mocks = new MockRepository();

            VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
            mocks.ReplayAll();

            SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
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
            SetupResult.For(pathProvider.GetCacheDependency("", null, DateTime.Now)).IgnoreArguments().Return(null);
            Stream stream = UnitTestHelper.UnpackEmbeddedResource("Skins.Skins.config");
            Expect.Call(vfile.Open()).Return(stream);
            return pathProvider;
        }
    }
}
