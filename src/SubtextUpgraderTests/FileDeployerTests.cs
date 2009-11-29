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
            var webroot = new Mock<IDirectory>();
            var destination = new Mock<IDirectory>();
            destination.Setup(d => d.Combine(It.IsAny<string>())).Returns(new Mock<IDirectory>().Object);
            destination.Setup(d => d.CombineFile(It.IsAny<string>())).Returns(new Mock<IFile>().Object);
            var fileDeployer = new FileDeployer(webroot.Object, destination.Object);

            // act
            fileDeployer.Deploy();

            // assert
            webroot.Verify(d => d.CopyTo(destination.Object));
        }

        [Test]
        public void RemoveOldFiles_WithDestination_RemovesUnusedfiles()
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

            destination.Verify(d => d.CombineFile("SystemMessages"));
            destination.Verify(d => d.CombineFile("AggDefault.aspx"));
        }
    }
}
