using System;
using System.IO;
using System.Web.Hosting;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Framework.UI.Skinning;
using Subtext.Web.UI.Pages;

namespace UnitTests.Subtext.Framework.Skinning
{
	[TestFixture]
	public class SkinTests
	{
		/// <summary>
		/// Here we load an instance of SkinTemplates from an embedded Skins.config file.
		/// </summary>
		[Test]
		public void CanLoadSkinsFromFile()
		{
			MockRepository mocks = new MockRepository();

			VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
			mocks.ReplayAll();

			SkinTemplates templates = SkinTemplates.Instance(pathProvider);
			Assert.IsNotNull(templates, "Could not instantiate template.");
			Assert.AreEqual(17, templates.Templates.Count, "Expected 17 templates.");
		}

		/// <summary>
		/// Here we load an instance of SkinTemplates from an embedded Skins.config file and 
		/// Skins.User.config file.
		/// </summary>
		[Test]
		public void CanLoadAndMergeUserSkinsFromFile()
		{
			MockRepository mocks = new MockRepository();
			VirtualPathProvider pathProvider = (VirtualPathProvider)mocks.CreateMock(typeof(VirtualPathProvider));
			VirtualFile vfile = (VirtualFile)mocks.CreateMock(typeof(VirtualFile), "~/Admin/Skins.config");
			VirtualFile vUserFile = (VirtualFile)mocks.CreateMock(typeof(VirtualFile), "~/Admin/Skins.User.config");
			
			using (Stream stream = UnitTestHelper.UnpackEmbeddedResource("Skins.Skins.config"))
			using (Stream userStream = UnitTestHelper.UnpackEmbeddedResource("Skins.Skins.User.config"))
			{
				Expect.Call(vfile.Open()).Return(stream);
				Expect.Call(vUserFile.Open()).Return(userStream);
				Expect.Call(pathProvider.GetFile("~/Admin/Skins.config")).Return(vfile);
				Expect.Call(pathProvider.FileExists("~/Admin/Skins.User.config")).Return(true);
				Expect.Call(pathProvider.GetFile("~/Admin/Skins.User.config")).Return(vUserFile);

				mocks.ReplayAll();
				SkinTemplates templates = SkinTemplates.Instance(pathProvider);
				Assert.IsNotNull(templates, "Could not instantiate template.");
				Assert.AreEqual(18, templates.Templates.Count, "Expected 18 templates.");
			}
		}
		
		[Test]
		public void CanGetPropertiesOfSingleTemplate()
		{
			MockRepository mocks = new MockRepository();

			VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
			mocks.ReplayAll();

			SkinTemplates templates = SkinTemplates.Instance(pathProvider);
			SkinTemplate template = templates.GetTemplate("RedBook-Blue.css");
			Assert.IsNotNull(template, "Could not get the template for the skin key 'RedBook-Blue.css'");

			Assert.AreEqual("REDBOOK-BLUE.CSS", template.SkinKey, "SkinKey not correct.");
			Assert.AreEqual("BlueBook", template.Name, "Name not correct.");
			Assert.AreEqual("RedBook", template.TemplateFolder, "Folder not correct.");
			Assert.AreEqual(1, template.Scripts.Length, "Wrong number of scripts.");
			Assert.AreEqual(6, template.Styles.Length, "Wrong number of styles.");
		}
		
		[RowTest]
		[Row("", "", "/Skins/RedBook/print.css")]
		[Row("blog", "", "/Skins/RedBook/print.css")]
		[Row("blog", "Subtext.Web", "/Subtext.Web/Skins/RedBook/print.css")]
		public void StyleSheetElementCollectionRendererRendersCssLinkElements(string subFolder, string applicationPath, string expectedPrintCssPath)
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", subFolder, applicationPath);
			MockRepository mocks = new MockRepository();

			VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
			mocks.ReplayAll();

			SkinTemplates templates = SkinTemplates.Instance(pathProvider);
			SubtextMasterPage.StyleSheetElementCollectionRenderer renderer = new SubtextMasterPage.StyleSheetElementCollectionRenderer(templates);
			string styleElements = renderer.RenderStyleElementCollection("RedBook-Blue.css");

			Console.WriteLine(styleElements);
			
			string printCss = string.Format(@"<link media=""print"" type=""text/css"" rel=""stylesheet"" href=""{0}""></link>", expectedPrintCssPath);
			Assert.IsTrue(styleElements.IndexOf(printCss) > -1, "Expected the printcss to be there.");
		}

		[Test]
		public void ScriptElementCollectionRendererRendersScriptElements()
		{
			UnitTestHelper.SetHttpContextWithBlogRequest("localhost", "blog", string.Empty);
			MockRepository mocks = new MockRepository();

			VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
			mocks.ReplayAll();

			SkinTemplates templates = SkinTemplates.Instance(pathProvider);
			SubtextMasterPage.ScriptElementCollectionRenderer renderer = new SubtextMasterPage.ScriptElementCollectionRenderer(templates);
			string scriptElements = renderer.RenderScriptElementCollection("RedBook-Green.css");
			
			string script = @"<script type=""text/javascript"" src=""/Skins/RedBook/blah.js""></script>";
			Assert.IsTrue(scriptElements.IndexOf(script) > -1, "Rendered the script improperly.");

			scriptElements = renderer.RenderScriptElementCollection("Nature-Leafy.css");
			script = @"<script type=""text/javascript"" src=""/scripts/XFNHighlighter.js""></script>";
			Assert.IsTrue(scriptElements.IndexOf(script) > -1, "Rendered the script improperly. We got: " + scriptElements);
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
