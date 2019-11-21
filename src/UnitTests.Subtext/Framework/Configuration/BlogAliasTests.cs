using Microsoft.VisualStudio.TestTools.UnitTesting;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Data;

namespace UnitTests.Subtext.Framework.Configuration
{
    [TestClass]
    public class BlogAliasTests
    {
        [DatabaseIntegrationTestMethod]
        public void CreateBlogAlias()
        {
            // Arrange
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            var alias = new BlogAlias();
            alias.BlogId = blog.Id;
            alias.Host = UnitTestHelper.GenerateUniqueString();
            alias.Subfolder = UnitTestHelper.GenerateUniqueString();

            // Act
            repository.AddBlogAlias(alias);

            // Assert
            Assert.AreNotEqual(alias.Id, NullValue.NullInt32);
        }

        [DatabaseIntegrationTestMethod]
        public void GetBlogByAliasNoSubfolder()
        {
            var repository = new DatabaseObjectProvider();
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(), "");

            repository.AddBlogAlias(alias);
            Blog testBlog = repository.GetBlogByDomainAlias(alias.Host, alias.Subfolder, false);
            Assert.AreEqual(blog.Id, testBlog.Id, "Found the wrong blog.");
        }

        [DatabaseIntegrationTestMethod]
        public void GetBlogByAliasWithSubfolders()
        {
            var repository = new DatabaseObjectProvider();
            Blog blog1 = UnitTestHelper.CreateBlogAndSetupContext();
            Blog blog2 = UnitTestHelper.CreateBlogAndSetupContext();
            string host = UnitTestHelper.GenerateUniqueString();
            BlogAlias alias1 = UnitTestHelper.CreateBlogAlias(blog1, host, UnitTestHelper.GenerateUniqueString());
            BlogAlias alias2 = UnitTestHelper.CreateBlogAlias(blog2, host, UnitTestHelper.GenerateUniqueString());

            repository.AddBlogAlias(alias1);
            repository.AddBlogAlias(alias2);

            Blog testBlog1 = repository.GetBlogByDomainAlias(alias1.Host, alias1.Subfolder, false);
            Blog testBlog2 = repository.GetBlogByDomainAlias(alias2.Host, alias2.Subfolder, false);

            Assert.AreEqual(blog1.Id, testBlog1.Id, "Found the wrong blog.");
            Assert.AreEqual(blog2.Id, testBlog2.Id, "Found the wrong blog.");
        }

        [DatabaseIntegrationTestMethod]
        public void CheckThatAliasChecksSubfolderIfBlogOnSameHost()
        {
            var repository = new DatabaseObjectProvider();
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, blog.Host, UnitTestHelper.GenerateUniqueString());

            repository.AddBlogAlias(alias);

            Blog testBlog = repository.GetBlogByDomainAlias(blog.Host, "", false);
            Assert.IsNull(testBlog, "Should not have found a blog, alias is on same host.");
        }

        [DatabaseIntegrationTestMethod]
        public void GetBlogAliasById()
        {
            var repository = new DatabaseObjectProvider();
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(),
                                                             UnitTestHelper.GenerateUniqueString());

            repository.AddBlogAlias(alias);

            BlogAlias testAlias = repository.GetBlogAlias(alias.Id);

            Assert.AreEqual(alias.Id, testAlias.Id, "Found the wrong alias.");
        }

        [DatabaseIntegrationTestMethod]
        public void GetBlogAliasByBlog()
        {
            var repository = new DatabaseObjectProvider();
            string host = UnitTestHelper.GenerateUniqueString();
            Blog blog1 = UnitTestHelper.CreateBlogAndSetupContext();
            Blog blog2 = UnitTestHelper.CreateBlogAndSetupContext();
            var alias1 = new BlogAlias();
            alias1.BlogId = blog1.Id;
            alias1.Host = host;

            var alias2 = new BlogAlias();
            alias2.BlogId = blog2.Id;
            alias2.Host = host;
            alias2.Subfolder = UnitTestHelper.GenerateUniqueString();

            repository.AddBlogAlias(alias1);
            repository.AddBlogAlias(alias2);

            IPagedCollection<BlogAlias> aliases = blog1.GetBlogAliases(new DatabaseObjectProvider(), 0, 10);
            Assert.AreEqual(1, aliases.Count);
            Assert.AreEqual(alias1.Id, aliases[0].Id);
            Assert.AreEqual(string.Empty, alias1.Subfolder);
        }

        [DatabaseIntegrationTestMethod]
        public void CheckBlogNotReturnedWithoutAlias()
        {
            UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            Blog testBlog = repository.GetBlogByDomainAlias(UnitTestHelper.GenerateUniqueString(),
                                                                           UnitTestHelper.GenerateUniqueString(), false);

            Assert.IsNull(testBlog);
        }

        [DatabaseIntegrationTestMethod]
        public void CheckBlogNotReturnedWithInactiveAlias()
        {
            var repository = new DatabaseObjectProvider();
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(), "", false);

            BlogAlias testAlias = repository.GetBlogAlias(alias.Id);
            Blog testBlog = repository.GetBlogByDomainAlias(alias.Host, alias.Subfolder, false);

            Assert.AreNotEqual(NullValue.NullInt32, alias.Id, "Alias was not saved.");
            Assert.AreEqual(alias.Id, testAlias.Id, "The test alias is not the alias saved.");
            Assert.IsFalse(testAlias.IsActive, "This alias is active.");
            Assert.IsNull(testBlog, "Should not have found a blog");
        }

        [DatabaseIntegrationTestMethod]
        public void UpdateBlogAlias()
        {
            var repository = new DatabaseObjectProvider();
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            string host1 = UnitTestHelper.GenerateUniqueString();
            string host2 = UnitTestHelper.GenerateUniqueString();

            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, host1, "");
            repository.AddBlogAlias(alias);

            BlogAlias testAlias = repository.GetBlogAlias(alias.Id);
            testAlias.Host = host2;

            repository.UpdateBlogAlias(testAlias);

            BlogAlias testAlias2 = repository.GetBlogAlias(alias.Id);

            Assert.AreEqual(alias.Id, testAlias.Id, "Did not retrieve the correct alias.");
            Assert.AreEqual(alias.Id, testAlias2.Id, "Did not retrieve the correct alias.");
            Assert.AreEqual(host2, testAlias2.Host, "Alias was not updated");
        }

        [DatabaseIntegrationTestMethod]
        public void DeleteBlogAlias()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            var repository = new DatabaseObjectProvider();
            BlogAlias alias1 = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(),
                                                              UnitTestHelper.GenerateUniqueString());
            BlogAlias alias2 = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(),
                                                              UnitTestHelper.GenerateUniqueString());
            repository.AddBlogAlias(alias1);
            repository.AddBlogAlias(alias2);

            repository.DeleteBlogAlias(alias1);

            BlogAlias testAlias1 = repository.GetBlogAlias(alias1.Id);
            BlogAlias testAlias2 = repository.GetBlogAlias(alias2.Id);

            Assert.IsNull(testAlias1, "Did not delete the alias.");
            Assert.IsNotNull(testAlias2, "Deleted the wrong one");
        }
    }
}