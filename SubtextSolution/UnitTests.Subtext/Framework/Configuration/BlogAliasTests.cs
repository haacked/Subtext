using System;
using MbUnit.Framework;
using Subtext.Extensibility.Interfaces;
using Subtext.Framework;
using Subtext.Framework.Configuration;

namespace UnitTests.Subtext.Framework.Configuration
{
    [TestFixture]
    public class BlogAliasTests
    {
        [Test]
        [RollBack2]
        public void CreateBlogAlias()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = new BlogAlias();
            alias.BlogId = blog.Id;
            alias.Host = UnitTestHelper.GenerateUniqueString();
            alias.Subfolder = UnitTestHelper.GenerateUniqueString();
            Config.AddBlogAlias(alias);
            Assert.AreNotEqual(alias.Id, NullValue.NullInt32);
        }


        [Test]
        [RollBack2]
        public void GetBlogByAliasNoSubfolder()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(), "");

            Config.AddBlogAlias(alias);
            Blog testBlog = Config.GetBlogInfoFromDomainAlias(alias.Host, alias.Subfolder, false);
            Assert.AreEqual(blog.Id, testBlog.Id, "Found the wrong blog.");
        }


        [Test]
        [RollBack2]
        public void GetBlogByAliasWithSubfolders()
        {
            Blog blog1 = UnitTestHelper.CreateBlogAndSetupContext();
            Blog blog2 = UnitTestHelper.CreateBlogAndSetupContext();
            string host = UnitTestHelper.GenerateUniqueString();
            BlogAlias alias1 = UnitTestHelper.CreateBlogAlias(blog1, host, UnitTestHelper.GenerateUniqueString());
            BlogAlias alias2 = UnitTestHelper.CreateBlogAlias(blog2, host, UnitTestHelper.GenerateUniqueString());

            Config.AddBlogAlias(alias1);
            Config.AddBlogAlias(alias2);

            Blog testBlog1 = Config.GetBlogInfoFromDomainAlias(alias1.Host, alias1.Subfolder, false);
            Blog testBlog2 = Config.GetBlogInfoFromDomainAlias(alias2.Host, alias2.Subfolder, false);

            Assert.AreEqual(blog1.Id, testBlog1.Id, "Found the wrong blog.");
            Assert.AreEqual(blog2.Id, testBlog2.Id, "Found the wrong blog.");
        }


        [Test]
        [RollBack2]
        public void CheckThatAliasChecksSubfolderIfBlogOnSameHost()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, blog.Host, UnitTestHelper.GenerateUniqueString());

            Config.AddBlogAlias(alias);

            Blog testBlog = Config.GetBlogInfoFromDomainAlias(blog.Host, "", false);
            Assert.IsNull(testBlog, "Should not have found a blog, alias is on same host.");
        }


        [Test]
        [RollBack2]
        public void GetBlogAliasById()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString());

            Config.AddBlogAlias(alias);

            BlogAlias testAlias = Config.GetBlogAlias(alias.Id);

            Assert.AreEqual(alias.Id, testAlias.Id, "Found the wrong alias.");
        }


        [Test]
        [RollBack2]
        public void GetBlogAliasByBlog()
        {
            string host = UnitTestHelper.GenerateUniqueString();
            Blog blog1 = UnitTestHelper.CreateBlogAndSetupContext();
            Blog blog2 = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias1 = new BlogAlias();
            alias1.BlogId = blog1.Id;
            alias1.Host = host;

            BlogAlias alias2 = new BlogAlias();
            alias2.BlogId = blog2.Id;
            alias2.Host = host;
            alias2.Subfolder = UnitTestHelper.GenerateUniqueString();

            Config.AddBlogAlias(alias1);
            Config.AddBlogAlias(alias2);

            IPagedCollection<BlogAlias> aliases = blog1.GetBlogAliases(0, 10);
            Assert.AreEqual(1, aliases.Count);
            Assert.AreEqual(alias1.Id, aliases[0].Id);
            Assert.IsEmpty(alias1.Subfolder);
        }


        [Test]
        [RollBack2]
        public void CheckBlogNotReturnedWithoutAlias()
        {
            UnitTestHelper.CreateBlogAndSetupContext();

            Blog testBlog = Config.GetBlogInfoFromDomainAlias(UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString(), false);

            Assert.IsNull(testBlog);
        }


        [Test]
        [RollBack2]
        public void CheckBlogNotReturnedWithInactiveAlias()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(), "", false);

            BlogAlias testAlias = Config.GetBlogAlias(alias.Id);
            Blog testBlog = Config.GetBlogInfoFromDomainAlias(alias.Host, alias.Subfolder, false);

            Assert.AreNotEqual(NullValue.NullInt32, alias.Id, "Alias was not saved.");
            Assert.AreEqual(alias.Id, testAlias.Id, "The test alias is not the alias saved.");
            Assert.IsFalse(testAlias.IsActive, "This alias is active.");
            Assert.IsNull(testBlog, "Should not have found a blog");
        }


        [Test]
        [RollBack2]
        public void UpdateBlogAlias()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            string host1 = UnitTestHelper.GenerateUniqueString();
            string host2 = UnitTestHelper.GenerateUniqueString();

            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, host1, "");
            Config.AddBlogAlias(alias);

            BlogAlias testAlias = Config.GetBlogAlias(alias.Id);
            testAlias.Host = host2;

            Config.UpdateBlogAlias(testAlias);

            BlogAlias testAlias2 = Config.GetBlogAlias(alias.Id);

            Assert.AreEqual(alias.Id, testAlias.Id, "Did not retrieve the correct alias.");
            Assert.AreEqual(alias.Id, testAlias2.Id, "Did not retrieve the correct alias.");
            Assert.AreEqual(host2, testAlias2.Host, "Alias was not updated");
        }

        [Test]
        [RollBack2]
        public void DeleteBlogAlias()
        {
            Blog blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias1 = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString());
            BlogAlias alias2 = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateUniqueString(), UnitTestHelper.GenerateUniqueString());
            Config.AddBlogAlias(alias1);
            Config.AddBlogAlias(alias2);

            Config.DeleteBlogAlias(alias1);

            BlogAlias testAlias1 = Config.GetBlogAlias(alias1.Id);
            BlogAlias testAlias2 = Config.GetBlogAlias(alias2.Id);

            Assert.IsNull(testAlias1, "Did not delete the alias.");
            Assert.IsNotNull(testAlias2, "Deleted the wrong one");
        }
    }
}
