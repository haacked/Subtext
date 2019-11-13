using System;
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
            var source = new SubtextDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            source.Create();
            
            var destination = new SubtextDirectory(Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString()));
            destination.Create();

            var file1 = source.CombineFile(Guid.NewGuid().ToString());
            using (var sw = new StreamWriter(file1.OpenWrite()))
                sw.WriteLine(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit.");

            var file2 = source.CombineFile(Guid.NewGuid().ToString());
            using (var sw = new StreamWriter(file2.OpenWrite()))
                sw.WriteLine(@"Lorem ipsum dolor sit amet, consectetur adipiscing elit.");
            
            // act
            source.CopyTo(destination);

            // assert
            FileAssert.Exists(destination.CombineFile(file1.Name).Path);
            FileAssert.Exists(destination.CombineFile(file2.Name).Path);

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
