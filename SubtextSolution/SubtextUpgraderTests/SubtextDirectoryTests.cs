using System.IO;
using MbUnit.Framework;
using Moq;
using SubtextUpgrader;

namespace SubtextUpgraderTests
{
    [TestFixture]
    public class SubtextDirectoryTests
    {
        [Test]
        public void CopyTo_WithDestinationDirectory_CopiesFiles()
        {
            // arrange
            var source = new Mock<SubtextDirectory>(new DirectoryInfo("."));
            var destination = new Mock<IDirectory>();
            var file1 = new Mock<IFile>();
            var file2 = new Mock<IFile>();
            var files = new[] {file1.Object, file2.Object};
            source.Setup(d => d.GetFiles()).Returns(files);

            // act
            source.Object.CopyTo(destination.Object);

            // assert
            file1.Verify(f => f.CopyTo(destination.Object));
            file2.Verify(f => f.CopyTo(destination.Object));
        }

        [Test]
        public void CopyTo_WithDestinationDirectory_CopiesSubfolders()
        {
            // arrange
            var source = new Mock<SubtextDirectory>(new DirectoryInfo("."));
            var destination = new Mock<IDirectory>();
            var destDir1 = new Mock<IDirectory>();
            destination.Setup(d => d.Combine("Dir1")).Returns(destDir1.Object);
            var destDir2 = new Mock<IDirectory>();
            destination.Setup(d => d.Combine("Dir2")).Returns(destDir2.Object);
            var dir1 = new Mock<IDirectory>();
            dir1.Setup(d => d.Name).Returns("Dir1");
            var dir2 = new Mock<IDirectory>();
            dir2.Setup(d => d.Name).Returns("Dir2");
            var directories = new[] { dir1.Object, dir2.Object };
            source.Setup(d => d.GetDirectories()).Returns(directories);

            // act
            source.Object.CopyTo(destination.Object);

            // assert
            dir1.Verify(d => d.CopyTo(destDir1.Object));
            dir2.Verify(d => d.CopyTo(destDir2.Object));
        }
    }
}
