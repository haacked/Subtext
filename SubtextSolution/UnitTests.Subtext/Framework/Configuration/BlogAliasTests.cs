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
            BlogInfo blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = new BlogAlias();
            alias.BlogId = blog.Id;
            alias.Host = UnitTestHelper.GenerateRandomString();
            alias.Subfolder = UnitTestHelper.GenerateRandomString();
            Config.AddBlogAlias(alias);
            Assert.AreNotEqual(alias.Id, NullValue.NullInt32);
            Console.WriteLine(alias.Id.ToString(), "Alias not created.");
        }


        [Test]
        [RollBack2]
        public void GetBlogByAliasNoSubfolder()
        {
            BlogInfo blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateRandomString(), "");

            Config.AddBlogAlias(alias);
            BlogInfo testBlog = Config.GetBlogInfoFromDomainAlias(alias.Host, alias.Subfolder, false);
            Assert.AreEqual(blog.Id, testBlog.Id, "Found the wrong blog.");
        }


        [Test]
        [RollBack2]
        public void GetBlogByAliasWithSubfolders()
        {
            BlogInfo blog1 = UnitTestHelper.CreateBlogAndSetupContext();
            BlogInfo blog2 = UnitTestHelper.CreateBlogAndSetupContext();
            string host = UnitTestHelper.GenerateRandomString();
            BlogAlias alias1 = UnitTestHelper.CreateBlogAlias(blog1, host, UnitTestHelper.GenerateRandomString());
            BlogAlias alias2 = UnitTestHelper.CreateBlogAlias(blog2, host, UnitTestHelper.GenerateRandomString());

            Config.AddBlogAlias(alias1);
            Config.AddBlogAlias(alias2);

            BlogInfo testBlog1 = Config.GetBlogInfoFromDomainAlias(alias1.Host, alias1.Subfolder, false);
            BlogInfo testBlog2 = Config.GetBlogInfoFromDomainAlias(alias2.Host, alias2.Subfolder, false);

            Assert.AreEqual(blog1.Id, testBlog1.Id, "Found the wrong blog.");
            Assert.AreEqual(blog2.Id, testBlog2.Id, "Found the wrong blog.");
        }


        [Test]
        [RollBack2]
        public void CheckThatAliasChecksSubfolderIfBlogOnSameHost()
        {
            BlogInfo blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, blog.Host, UnitTestHelper.GenerateRandomString());

            Config.AddBlogAlias(alias);

            BlogInfo testBlog = Config.GetBlogInfoFromDomainAlias(blog.Host, "", false);
            Console.WriteLine((testBlog == blog).ToString());
            Assert.IsNull(testBlog, "Should not have found a blog, alias is on same host.");
        }


        [Test]
        [RollBack2]
        public void GetBlogAliasById()
        {
            BlogInfo blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateRandomString(), UnitTestHelper.GenerateRandomString());

            Config.AddBlogAlias(alias);

            BlogAlias testAlias = Config.GetBlogAlias(alias.Id);

            Assert.AreEqual(alias.Id, testAlias.Id, "Found the wrong alias.");
        }


        [Test]
        [RollBack2]
        public void GetBlogAliasByBlog()
        {
            string host = UnitTestHelper.GenerateRandomString();
            BlogInfo blog1 = UnitTestHelper.CreateBlogAndSetupContext();
            BlogInfo blog2 = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias1 = new BlogAlias();
            alias1.BlogId = blog1.Id;
            alias1.Host = host;

            BlogAlias alias2 = new BlogAlias();
            alias2.BlogId = blog2.Id;
            alias2.Host = host;
            alias2.Subfolder = UnitTestHelper.GenerateRandomString();

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

            BlogInfo testBlog = Config.GetBlogInfoFromDomainAlias(UnitTestHelper.GenerateRandomString(), UnitTestHelper.GenerateRandomString(), false);

            Assert.IsNull(testBlog);
        }


        [Test]
        [RollBack2]
        public void CheckBlogNotReturnedWithInactiveAlias()
        {
            BlogInfo blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateRandomString(), "", false);

            BlogAlias testAlias = Config.GetBlogAlias(alias.Id);
            BlogInfo testBlog = Config.GetBlogInfoFromDomainAlias(alias.Host, alias.Subfolder, false);

            Assert.AreNotEqual(NullValue.NullInt32, alias.Id, "Alias was not saved.");
            Assert.AreEqual(alias.Id, testAlias.Id, "The test alias is not the alias saved.");
            Assert.IsFalse(testAlias.IsActive, "This alias is active.");
            Assert.IsNull(testBlog, "Should not have found a blog");
        }


        [Test]
        [RollBack2]
        public void UpdateBlogAlias()
        {
            BlogInfo blog = UnitTestHelper.CreateBlogAndSetupContext();
            string host1 = UnitTestHelper.GenerateRandomString();
            string host2 = UnitTestHelper.GenerateRandomString();

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
            BlogInfo blog = UnitTestHelper.CreateBlogAndSetupContext();
            BlogAlias alias1 = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateRandomString(), UnitTestHelper.GenerateRandomString());
            BlogAlias alias2 = UnitTestHelper.CreateBlogAlias(blog, UnitTestHelper.GenerateRandomString(), UnitTestHelper.GenerateRandomString());
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
