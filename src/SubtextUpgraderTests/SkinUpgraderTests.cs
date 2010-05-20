using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using MbUnit.Framework;
using Moq;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class SkinUpgraderTests
    {
        [Test]
        public  void Ctor_FileWithOldAssembly_ReplateWithNewOne()
        {
            //arrange
            var upgrader = new SkinUpgrader(GetSourceDirectory());

            // act
            upgrader.Run();

            //assert
            using (var reader = new StreamReader(File.OpenRead("PageTemplate.ascx")))
            {
                const string expected = @"<%@ Register TagPrefix=""SP"" Namespace=""Subtext.Web.Controls"" Assembly=""Subtext.Web"" %>my other content";
                var text = reader.ReadToEnd();
                Assert.AreEqual(expected, text);
            }
        }

        [Test]
        public void Ctor_FileWithOldAssembly_KeepsOtherContent()
        {
            //arrange
            var upgrader = new SkinUpgrader(GetSourceDirectory());

            // act
            upgrader.Run();

            //assert
            using (var reader = new StreamReader(File.OpenRead("PageTemplate.ascx")))
            {
                const string expected = @"<%@ Register TagPrefix=""SP"" Namespace=""Subtext.Web.Controls"" Assembly=""Subtext.Web"" %>my other content";
                var text = reader.ReadToEnd();
                Assert.AreEqual(expected, text);
            }
        }

        [Test]
        public void Ctor_DirectoryWithSubDirectories_ReplacesFilesUnderNeath()
        {
            //arrange
            using (var writer = File.CreateText("PageTemplate.ascx"))
            {
                writer.Write(@"<%@ Register TagPrefix=""SP"" Namespace=""Subtext.Web.Controls"" Assembly=""Subtext.Web.Controls"" %>");
            }
            var existingCustomSkinFile = new SubtextFile(new FileInfo("PageTemplate.ascx"));
            var files = new List<IFile> { existingCustomSkinFile };
            var sourceDirectory = new Mock<IDirectory>();
            var subDirectory = new Mock<IDirectory>();
            subDirectory.Setup(s => s.GetFiles()).Returns(files);
            var directories = new List<IDirectory> {subDirectory.Object};
            sourceDirectory.Setup(s => s.GetDirectories()).Returns(directories);
            var upgrader = new SkinUpgrader(sourceDirectory.Object);

            // act
            upgrader.Run();

            //assert
            using (var reader = new StreamReader(File.OpenRead("PageTemplate.ascx")))
            {
                const string expected = @"<%@ Register TagPrefix=""SP"" Namespace=""Subtext.Web.Controls"" Assembly=""Subtext.Web"" %>";
                var text = reader.ReadToEnd();
                Assert.AreEqual(expected, text);
            }
        }

        [TearDown]
        public void TearDown()
        {
            if(File.Exists("PageTemplate.ascx"))
            {
                File.Delete("PageTemplate.ascx");
            }
        }

        private static IDirectory GetSourceDirectory()
        {
            using (var writer = File.CreateText("PageTemplate.ascx"))
            {
                writer.Write(@"<%@ Register TagPrefix=""SP"" Namespace=""Subtext.Web.Controls"" Assembly=""Subtext.Web.Controls"" %>");
                writer.Write(@"my other content");
            }
            var existingCustomSkinFile = new SubtextFile(new FileInfo("PageTemplate.ascx"));
            var files = new List<IFile> { existingCustomSkinFile };
            var sourceDirectory = new Mock<IDirectory>();
            sourceDirectory.Setup(s => s.GetFiles()).Returns(files);
            return sourceDirectory.Object;
        }
    }
}
