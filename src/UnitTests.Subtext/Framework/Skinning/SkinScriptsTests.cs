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

using System.Collections.Generic;
using System.Web.Hosting;
using MbUnit.Framework;
using Moq;
using Subtext.Framework.UI.Skinning;

namespace UnitTests.Subtext.Framework.Skinning
{
    [TestFixture]
    public class SkinScriptsTests
    {
        [Test]
        public void CanGetScriptMergeModeAttribute()
        {
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);
            IDictionary<string, SkinTemplate> templates = skinEngine.GetSkinTemplates(false /* mobile */);

            SkinTemplate templateWithMergeScriptMergeMode = templates["Piyo"];
            Assert.IsTrue(templateWithMergeScriptMergeMode.MergeScripts, "ScriptMergeMode should be Merge.");

            SkinTemplate templateWithDontMergeScriptMergeMode = templates["Semagogy"];
            Assert.IsFalse(templateWithDontMergeScriptMergeMode.MergeScripts, "ScriptMergeMode should be DontMerge.");

            SkinTemplate templateWithoutScriptMergeMode = templates["RedBook-Green.css"];
            Assert.IsFalse(templateWithoutScriptMergeMode.MergeScripts, "ScriptMergeMode should be None.");
        }

        [Test]
        public void ScriptElementCollectionRendererRendersScriptElements()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);

            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);
            var renderer = new ScriptElementCollectionRenderer(skinEngine);
            string scriptElements = renderer.RenderScriptElementCollection("RedBook-Green.css");

            string script = @"<script type=""text/javascript"" src=""/Skins/RedBook/blah.js""></script>";
            Assert.IsTrue(scriptElements.Contains(script), "Rendered the script improperly.");

            scriptElements = renderer.RenderScriptElementCollection("Nature-Leafy.css");
            script = @"<script type=""text/javascript"" src=""/scripts/XFNHighlighter.js""></script>";
            Assert.IsTrue(scriptElements.Contains(script), "Rendered the script improperly. We got: " + scriptElements);
        }

        [Test]
        public void ScriptElementCollectionRendererRendersJSHandlerScript()
        {
            UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);

            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            var renderer = new ScriptElementCollectionRenderer(skinEngine);
            string scriptElements = renderer.RenderScriptElementCollection("RedBook-Blue.css");

            string script =
                @"<script type=""text/javascript"" src=""/Skins/RedBook/js.axd?name=RedBook-Blue.css""></script>";
            Assert.IsTrue(scriptElements.Contains(script), "Rendered the script improperly.");
        }

        [Test]
        public void SkinsWithNoScriptsAreNotMerged()
        {
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            IDictionary<string, SkinTemplate> templates = skinEngine.GetSkinTemplates(false /* mobile */);
            SkinTemplate template = templates["Gradient"];
            bool canBeMerged = ScriptElementCollectionRenderer.CanScriptsBeMerged(template);

            Assert.IsFalse(canBeMerged, "Skins without scripts should not be mergeable.");
        }

        [Test]
        public void ScriptsWithRemoteSrcAreNotMerged()
        {
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);

            IDictionary<string, SkinTemplate> templates = skinEngine.GetSkinTemplates(false /* mobile */);
            SkinTemplate template = templates["RedBook-Red.css"];
            bool canBeMerged = ScriptElementCollectionRenderer.CanScriptsBeMerged(template);

            Assert.IsFalse(canBeMerged, "Skins with remote scripts should not be mergeable.");
        }

        [Test]
        public void ScriptsWithNoneScriptMergeModeAreNotMerged()
        {
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);
            IDictionary<string, SkinTemplate> templates = skinEngine.GetSkinTemplates(false /* mobile */);
            SkinTemplate template = templates["Semagogy"];
            bool canBeMerged = ScriptElementCollectionRenderer.CanScriptsBeMerged(template);

            Assert.IsFalse(canBeMerged, "Skins with ScriptMergeMode=\"DontMerge\" should not be mergeable.");
        }

        [Test]
        public void ScriptsWithParametricSrcAreNotMerged()
        {
            var pathProvider = new Mock<VirtualPathProvider>();
            pathProvider.SetupSkins();
            var skinEngine = new SkinEngine(pathProvider.Object);
            IDictionary<string, SkinTemplate> templates = skinEngine.GetSkinTemplates(false /* mobile */);
            SkinTemplate template = templates["Piyo"];
            bool canBeMerged = ScriptElementCollectionRenderer.CanScriptsBeMerged(template);

            Assert.IsFalse(canBeMerged, "Skins with scripts that have query string parameters should not be mergeable.");
        }
    }
}