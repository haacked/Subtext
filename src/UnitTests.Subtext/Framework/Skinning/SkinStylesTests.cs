#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Collections.Generic;
using System.Web.Hosting;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.Text;
using Subtext.Framework.UI.Skinning;

namespace UnitTests.Subtext.Framework.Skinning
{
    [TestFixture]
    public class SkinStylesTests
    {
        [Test]
        public void CanGetExcludeDefaultStyleAttribute()
        {
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);
            IDictionary<string, SkinTemplate> templates = skinEngine.GetSkinTemplates(false /* mobile */);

            SkinTemplate templateWithTrueExcludedDefault = templates["RedBook-Blue.css"];
            Assert.IsTrue(templateWithTrueExcludedDefault.ExcludeDefaultStyle, "ExcludeDefaultStyle should be True.");

            SkinTemplate templateWithFalseExcludedDefault = templates["Gradient"];
            Assert.IsFalse(templateWithFalseExcludedDefault.ExcludeDefaultStyle, "ExcludeDefaultStyle should be false.");

            SkinTemplate templateWithoutExcludedDefault = templates["Piyo"];
            Assert.IsFalse(templateWithoutExcludedDefault.ExcludeDefaultStyle, "ExcludeDefaultStyle should be false.");
        }

        [Test]
        public void CanGetMergeModeAttribute()
        {
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);
            IDictionary<string, SkinTemplate> templates = skinEngine.GetSkinTemplates(false /* mobile */);

            SkinTemplate templateWithMergedFirstMergeMode = templates["Semagogy"];
            Assert.AreEqual(StyleMergeMode.MergedFirst, templateWithMergedFirstMergeMode.StyleMergeMode,
                            "MergeMode should be MergedFirst.");

            SkinTemplate templateWithMergedAfterMergeMode = templates["RedBook-Green.css"];
            Assert.AreEqual(StyleMergeMode.MergedAfter, templateWithMergedAfterMergeMode.StyleMergeMode,
                            "MergeMode should be MergedAfter.");

            SkinTemplate templateWithNoneMergeMode = templates["RedBook-Red.css"];
            Assert.AreEqual(StyleMergeMode.None, templateWithNoneMergeMode.StyleMergeMode, "MergeMode should be None.");

            Assert.AreNotEqual(StyleMergeMode.MergedAfter, templateWithNoneMergeMode.StyleMergeMode,
                               "MergeMode should not be MergedAfter.");

            SkinTemplate templateWithoutMergeMode = templates["RedBook-Blue.css"];
            Assert.AreEqual(StyleMergeMode.None, templateWithoutMergeMode.StyleMergeMode, "MergeMode should be None.");
        }

        [RowTest]
        [Row("", "", "/Skins/RedBook/print.css", "/Skins/RedBook/style.css")]
        [Row("blog", "", "/Skins/RedBook/print.css", "/Skins/RedBook/style.css")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/RedBook/print.css", "/Subtext.Web/Skins/RedBook/style.css")]
        public void StyleSheetElementCollectionRendererRendersPlainCssLinkElementsWithNoneMergeMode(string subFolder,
                                                                                                    string
                                                                                                        applicationPath,
                                                                                                    string
                                                                                                        expectedPrintCssPath,
                                                                                                    string
                                                                                                        expectedDefaultCssPath)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);
            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            string styleElements = renderer.RenderStyleElementCollection("RedBook-Red.css");

            string printCss =
                string.Format(@"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""{0}"" />",
                              expectedPrintCssPath);
            Assert.IsTrue(styleElements.Contains(printCss, StringComparison.OrdinalIgnoreCase),
                          "Expected the printcss to be there.");

            string defaultCss = string.Format(@"<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />",
                                              expectedDefaultCssPath);
            Assert.IsTrue(styleElements.Contains(defaultCss, StringComparison.OrdinalIgnoreCase),
                          "Expected the default css to be there.");
        }

        [Test]
        public void RenderStyleElementCollection_WithNoStyles_RendersDefaultStyle()
        {
            // arrange
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", string.Empty, "/");
            var pathProvider = new Mock<VirtualPathProvider>();
            var skinEngine = new SkinEngine(pathProvider.Object);
            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            var skinTemplate = new SkinTemplate {ExcludeDefaultStyle = false, Styles = null, TemplateFolder = "TestSkin"};

            // act
            string styleElements = renderer.RenderStyleElementCollection("TestSkin", skinTemplate);

            // assert
            const string defaultStyle = @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/TestSkin/style.css"" />";
            Assert.AreEqual(defaultStyle, styleElements.Trim());
        }

        [RowTest]
        [Row("", "", "/Skins/WPSkin/print.css", "/Skins/WPSkin/style.css")]
        [Row("blog", "", "/Skins/WPSkin/print.css", "/Skins/WPSkin/style.css")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/WPSkin/print.css", "/Subtext.Web/Skins/WPSkin/style.css")]
        public void StyleSheetElementCollectionRenderer_WithNoneMergeModeAndExcludeDefault_RendersPlainCssLinkElements(
            string subFolder, string applicationPath, string expectedPrintCssPath, string expectedDefaultCssPath)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            string styleElements = renderer.RenderStyleElementCollection("WPSkin");

            string printCss =
                string.Format(@"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""{0}"" />",
                              expectedPrintCssPath);
            Assert.IsTrue(styleElements.Contains(printCss, StringComparison.OrdinalIgnoreCase),
                          "Expected the print css to be there.");

            string defaultCss = string.Format(@"<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />",
                                              expectedDefaultCssPath);
            Assert.IsTrue(!styleElements.Contains(defaultCss, StringComparison.OrdinalIgnoreCase),
                          "Not expected the default css to be there.");
        }

        [RowTest]
        [Row("", "", "/Skins/Lightz/print.css", "/Skins/Lightz/style.css", "/Skins/Lightz/light.css")]
        [Row("blog", "", "/Skins/Lightz/print.css", "/Skins/Lightz/style.css", "/Skins/Lightz/light.css")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/Lightz/print.css", "/Subtext.Web/Skins/Lightz/style.css",
            "/Subtext.Web/Skins/Lightz/light.css")]
        public void StyleSheetElementCollectionRenderer_WithNoneMergeModeAndSecondaryStyle_RendersPlainCssLinkElements(
            string subFolder, string applicationPath, string expectedPrintCssPath, string expectedDefaultCssPath,
            string expectedSecondaryCssPath)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            string styleElements = renderer.RenderStyleElementCollection("Lightz-light.css");

            string printCss =
                string.Format(@"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""{0}"" />",
                              expectedPrintCssPath);
            Assert.IsTrue(styleElements.Contains(printCss, StringComparison.OrdinalIgnoreCase),
                          "Expected the printcss to be there.");

            string defaultCss = string.Format(@"<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />",
                                              expectedDefaultCssPath);
            Assert.IsTrue(styleElements.Contains(defaultCss, StringComparison.OrdinalIgnoreCase),
                          "Expected the default css to be there.");

            string secondaryCss = string.Format(@"<link type=""text/css"" rel=""stylesheet"" href=""{0}"" />",
                                                expectedSecondaryCssPath);
            Assert.IsTrue(styleElements.Contains(secondaryCss, StringComparison.OrdinalIgnoreCase),
                          "Expected the secondary css to be there.");
        }

        [RowTest]
        [Row("KeyWest", true)]
        [Row("Gradient", false)]
        public void StyleSheetElementCollectionRendererRendersLinkElementsInRightOrder(string skinKey,
                                                                                       bool expectedFirst)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);

            string styleElements = renderer.RenderStyleElementCollection(skinKey);
            SkinTemplate template = skinEngine.GetSkinTemplates(false)[skinKey];

            styleElements = styleElements.Trim('\r', '\n');
            string mergedCss = @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/" + template.TemplateFolder +
                               "/css.axd?name=" + skinKey + @""" />";
            if(expectedFirst)
            {
                Assert.IsTrue(styleElements.StartsWith(mergedCss, StringComparison.OrdinalIgnoreCase),
                              "Merged CSS is not in first position");
            }
            else
            {
                Assert.IsTrue(styleElements.EndsWith(mergedCss, StringComparison.OrdinalIgnoreCase),
                              "Merged CSS is not in last position");
            }
        }


        [RowTest]
        [Row("", "", "/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed")]
        [Row("blog", "", "/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed")]
        [Row("blog", "Subtext.Web", "/Subtext.Web/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed")]
        public void StyleSheetElementCollectionRendererRendersMergedCssLinkElements(string subFolder,
                                                                                    string applicationPath,
                                                                                    string expectedPrintCssPath)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            string styleElements = renderer.RenderStyleElementCollection("Piyo");

            string printCss =
                string.Format(
                    @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" title=""fixed"" href=""{0}"" />",
                    expectedPrintCssPath);
            Assert.IsTrue(styleElements.Contains(printCss, StringComparison.OrdinalIgnoreCase),
                          "Expected the fixed screen css to be there.");
        }

        [RowTest]
        [Row("AnotherEon001",
            @"<link type=""text/css"" rel=""stylesheet"" href=""http://haacked.com/skins/_System/commonstyle.css"" />")]
        [Row("Gradient",
            "<!--[if IE]>\r\n" +
            @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" href=""/Skins/Gradient/css.axd?name=Gradient&media=screen&conditional=if+IE"" />" +
            "\r\n<![endif]-->")]
        [Row("RedBook-Green.css",
            "<!--[if IE]>\r\n" +
            @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/RedBook/css.axd?name=RedBook-Green.css&conditional=if+IE"" />" +
            "\r\n<![endif]-->")]
        [Row("Nature-leafy.css", "")]
        [Row("Origami", "")]
        [Row("Piyo",
            @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" title=""fixed"" href=""/Skins/Piyo/css.axd?name=Piyo&media=screen&title=fixed"" />" +
            "\r\n" +
            @"<link media=""screen"" type=""text/css"" rel=""stylesheet"" title=""elastic"" href=""/Skins/Piyo/css.axd?name=Piyo&media=screen&title=elastic"" />"
            )]
        [Row("Submarine",
            "<!--[if IE]>\r\n" +
            @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/Submarine/css.axd?name=Submarine&conditional=if+IE"" />" +
            "\r\n<![endif]-->")]
        public void CallsToCssHandlerAreNotRepeated(string skinKey, string exptectedElements)
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            string styleElements = renderer.RenderStyleElementCollection(skinKey);
            IDictionary<string, SkinTemplate> templates = skinEngine.GetSkinTemplates(false);
            SkinTemplate template = templates[skinKey];

            string mergedCss = @"<link type=""text/css"" rel=""stylesheet"" href=""/Skins/" + template.TemplateFolder +
                               "/css.axd?name=" + skinKey + @""" />";
            styleElements = styleElements.Replace(mergedCss, string.Empty);
            Assert.IsTrue(styleElements.Trim('\r', '\n').Equals(exptectedElements), "Not the expected stylesheet links");
        }


        [RowTest]
        [Row("", "print", "", "print.css", true)]
        [Row("", "print", "fixed", "print.css", false)]
        [Row("", "", "", "~/skins/_System/csharp.css", true)]
        [Row("if gte IE 7", "", "", "IE7Patches.css", false)]
        [Row("", "screen", "", "~/css/lightbox.css", true)]
        [Row("", "all", "", "Styles/user-styles.css", true)]
        [Row("", "", "fixed", "print.css", false)]
        [Row("", "all", "fixed", "Styles/user-styles.css", false)]
        [Row("if gte IE 7", "all", "", "Styles/user-styles.css", false)]
        [Row("", "", "", "http://www.google.com/style.css", false)]
        public void StyleToBeMergedAreCorrectlyDetected(string conditional, string media, string title, string href,
                                                        bool canBeMerged)
        {
            var style = new Style();
            style.Conditional = conditional;
            style.Media = media;
            style.Href = href;
            style.Title = title;

            bool isMergeable = StyleSheetElementCollectionRenderer.CanStyleBeMerged(style);
            if(canBeMerged)
            {
                Assert.IsTrue(isMergeable, "Expected to be mergeable");
            }
            else
            {
                Assert.IsFalse(isMergeable, "Expected not to be mergeable");
            }
        }

        [Test]
        public void MergedCssDoesntContainDefaultIfExcluded()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            var mergedStyles = (List<StyleDefinition>)renderer.GetStylesToBeMerged("WPSkin");

            Assert.IsFalse(mergedStyles.Contains(new StyleDefinition("/Skins/WPSkin/style.css")),
                           "Skin WPSkin should not have the default style.css");
        }

        [Test]
        public void MergedCssContainsDefaultStyle()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            var mergedStyles = (List<StyleDefinition>)renderer.GetStylesToBeMerged("Submarine");

            Assert.IsTrue(mergedStyles.Contains(new StyleDefinition("/Skins/Submarine/style.css")),
                          "Skin Submarine should have the default style.css");
        }


        [Test]
        public void MergedCssContainsStyleWithMedia()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            var mergedStyles = (List<StyleDefinition>)renderer.GetStylesToBeMerged("Piyo");

            Assert.IsTrue(mergedStyles.Contains(new StyleDefinition("/Skins/Piyo/print.css", "print")),
                          "Skin Piyo should have the print css in the merged css");
        }

        [Test]
        public void MergedCssDoesntContainStyleWithMediaAndTitle()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new StyleSheetElementCollectionRenderer(skinEngine);
            var mergedStyles = (List<StyleDefinition>)renderer.GetStylesToBeMerged("Piyo");

            Assert.IsFalse(mergedStyles.Contains(new StyleDefinition("/Skins/Piyo/piyo-fixed.css", "screen")),
                           "Skin Piyo should not have the fixed screen css in the merged CSS");
        }
    }
}