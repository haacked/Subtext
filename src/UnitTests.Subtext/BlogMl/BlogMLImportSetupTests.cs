using System;
using MbUnit.Framework;
using Moq;
using Subtext.Framework;
using Subtext.Framework.Providers;
using Subtext.ImportExport;

namespace UnitTests.Subtext.BlogMl
{
    [TestFixture]
    public class BlogMLImportSetupTests
    {
        [Test]
        public void Ctor_WithBlogHavingDuplicateCommentsDisabled_EnablesDuplicateComments()
        {
            // arrange
            var blog = new Blog {DuplicateCommentsEnabled = false};
            var repository = new Mock<ObjectRepository>();
            bool updateCalled = false;
            repository.Setup(r => r.UpdateBlog(blog)).Callback(() => updateCalled = true);

            // act
            new BlogImportSetup(blog, repository.Object);

            // assert
            Assert.IsTrue(blog.DuplicateCommentsEnabled);
            Assert.IsTrue(updateCalled);
        }

        [Test]
        public void Ctor_WithBlogHavingDuplicateCommentsEnabled_DoesNotChangeBlog()
        {
            // arrange
            var blog = new Blog { DuplicateCommentsEnabled = true };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.UpdateBlog(blog)).Throws(new InvalidOperationException());

            // act
            new BlogImportSetup(blog, repository.Object);

            // assert
            Assert.IsTrue(blog.DuplicateCommentsEnabled);
        }

        [Test]
        public void Dispose_WithBlogHavingDuplicateCommentsEnabled_DoesNotChangeBlog()
        {
            // arrange
            var blog = new Blog { DuplicateCommentsEnabled = true };
            var repository = new Mock<ObjectRepository>();
            repository.Setup(r => r.UpdateBlog(blog)).Throws(new InvalidOperationException());
            var scope = new BlogImportSetup(blog, repository.Object);

            // act
            scope.Dispose();

            // assert
            Assert.IsTrue(blog.DuplicateCommentsEnabled);
        }


        [Test]
        public void Dispose_WithBlogHavingDuplicateCommentsDisabled_DisablesDuplicateCommentsAgain()
        {
            // arrange
            var blog = new Blog { DuplicateCommentsEnabled = false };
            var repository = new Mock<ObjectRepository>();
            bool updateCalled = false;
            var scope = new BlogImportSetup(blog, repository.Object);
            repository.Setup(r => r.UpdateBlog(blog)).Callback(() => updateCalled = true);

            // act
            scope.Dispose();

            // assert
            Assert.IsFalse(blog.DuplicateCommentsEnabled);
            Assert.IsTrue(updateCalled);
        }
    }
}
