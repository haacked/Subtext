using System;
using System.IO;
using MbUnit.Framework;
using Moq;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class FileDeployerTests
    {
        [Test]
        public void Deploy_WithSourceAndDestinationDirectories_CopiesOneToTheOther()
        {
            // arrange
            var webroot = new SubtextDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            webroot.Create();

            var destination = new SubtextDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            destination.Create();

            var dir = webroot.Combine(Guid.NewGuid().ToString());
            dir.Create();

            var file = webroot.CombineFile(Guid.NewGuid().ToString());
            using (var sw = new StreamWriter(file.OpenWrite()))
                sw.WriteLine(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit.");

            var fileDeployer = new FileDeployer(webroot, destination);

            // act
            fileDeployer.Deploy();

            // assert
            Assert.IsTrue(destination.CombineFile(file.Name).Exists);
            Assert.IsTrue(destination.Combine(dir.Name).Exists);            
        }

        [Test]
        public void RemoveOldDirectories_WithDestination_RemovesUnusedDirectories()
        {
            // arrange
            var webroot = new Mock<IDirectory>();
            var destination = new Mock<IDirectory>();
            destination.Setup(d => d.Combine(It.IsAny<string>())).Returns(new Mock<IDirectory>().Object);
            destination.Setup(d => d.CombineFile(It.IsAny<string>())).Returns(new Mock<IFile>().Object);
            var fileDeployer = new FileDeployer(webroot.Object, destination.Object);

            // act
            fileDeployer.RemoveOldDirectories();

            // assert
            destination.Verify(d => d.Combine("Admin"));
            destination.Verify(d => d.Combine("HostAdmin"));
            destination.Verify(d => d.Combine("Install"));
            destination.Verify(d => d.Combine("SystemMessages"));
        }

        [Test]
        public void RemoveOldFiles_WithDestination_RemovesUnusedFiles()
        {
            // arrange
            var webroot = new Mock<IDirectory>();
            var destination = new Mock<IDirectory>();
            destination.Setup(d => d.CombineFile(It.IsAny<string>())).Returns(new Mock<IFile>().Object);
            var fileDeployer = new FileDeployer(webroot.Object, destination.Object);

            // act
            fileDeployer.RemoveOldFiles();

            // assert
            destination.Verify(d => d.CombineFile("AggDefault.aspx"));
        }
    }
}
