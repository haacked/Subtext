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

using System.IO;
using System.Web.Hosting;
using MbUnit.Framework;
using Rhino.Mocks;
using Subtext.Framework.UI.Skinning;

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

			SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
			Assert.IsNotNull(templates, "Could not instantiate template.");
			Assert.AreEqual(17, templates.Count, "Expected 17 templates.");
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
				SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
				Assert.IsNotNull(templates, "Could not instantiate template.");
				Assert.AreEqual(18, templates.Count, "Expected 18 templates.");
			}
		}


	    [Test]
		public void CanGetPropertiesOfSingleTemplate()
		{
			MockRepository mocks = new MockRepository();

			VirtualPathProvider pathProvider = GetTemplatesPathProviderMock(mocks);
			mocks.ReplayAll();

			SkinTemplateCollection templates = new SkinTemplateCollection(pathProvider);
			SkinTemplate template = templates.GetTemplate("RedBook-Blue.css");
			Assert.IsNotNull(template, "Could not get the template for the skin key 'RedBook-Blue.css'");

			Assert.AreEqual("REDBOOK-BLUE.CSS", template.SkinKey, "SkinKey not correct.");
			Assert.AreEqual("BlueBook", template.Name, "Name not correct.");
			Assert.AreEqual("RedBook", template.TemplateFolder, "Folder not correct.");
            Assert.IsTrue(template.ExcludeDefaultStyle, "ExcludeDefaultStyle should be true.");
			Assert.AreEqual(1, template.Scripts.Length, "Wrong number of scripts.");
			Assert.AreEqual(6, template.Styles.Length, "Wrong number of styles.");
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
