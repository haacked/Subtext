using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Components;
using Subtext.Framework.Routing;

namespace UnitTests.Subtext.Framework.Routing
{
    [TestClass]
    public class AdminUrlHelperTests
    {
        [TestMethod]
        public void PostsList_WithBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.PostsList();

            //assert
            Assert.AreEqual("/sub/admin/posts/default.aspx", url);
        }

        [TestMethod]
        public void PostsEdit_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.PostsEdit();

            //assert
            Assert.AreEqual("/sub/admin/posts/edit.aspx", url);
        }

        [TestMethod]
        public void ArticlesList_WithBlogHavingSubfolder_RendersUrlToArticlesListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.ArticlesList();

            //assert
            Assert.AreEqual("/sub/admin/articles/default.aspx", url);
        }

        [TestMethod]
        public void ArticlesEdit_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToArticlesListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.ArticlesEdit();

            //assert
            Assert.AreEqual("/sub/admin/articles/edit.aspx", url);
        }

        [TestMethod]
        public void FeedbackList_WithBlogHavingSubfolder_RendersUrlToFeedbackListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.FeedbackList();

            //assert
            Assert.AreEqual("/sub/admin/feedback/default.aspx", url);
        }

        [TestMethod]
        public void FeedbackEdit_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToFeedbackListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.FeedbackEdit(123);

            //assert
            Assert.AreEqual("/sub/admin/feedback/edit.aspx?return-to-post=true&FeedbackID=123", url);
        }

        [TestMethod]
        public void LinksEdit_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.LinksEdit();

            //assert
            Assert.AreEqual("/sub/admin/EditLinks.aspx", url);
        }

        [TestMethod]
        public void GalleriesEdit_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.GalleriesEdit();

            //assert
            Assert.AreEqual("/sub/admin/EditGalleries.aspx", url);
        }

        [TestMethod]
        public void EditCategories_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.EditCategories(CategoryType.PostCollection);

            //assert
            Assert.AreEqual("/sub/admin/EditCategories.aspx?catType=PostCollection", url);
        }

        [TestMethod]
        public void ErrorLog_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.ErrorLog();

            //assert
            Assert.AreEqual("/sub/admin/ErrorLog.aspx", url);
        }

        [TestMethod]
        public void Home_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.Home();

            //assert
            Assert.AreEqual("/sub/admin/default.aspx", url);
        }

        [TestMethod]
        public void ImportExport_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.ImportExport();

            //assert
            Assert.AreEqual("/sub/admin/ImportExport.aspx", url);
        }

        [TestMethod]
        public void ExportUrl_WithEmbedFalseAndSubFolder_RendersUrlWithQueryStringParameter()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.Export(false);

            //assert
            Assert.AreEqual("/sub/admin/export.ashx?embed=False", url);
        }

        [TestMethod]
        public void ExportUrl_WithEmbedTrue_RendersUrlWithQueryStringParameter()
        {
            //arrange
            AdminUrlHelper helper = SetupUrlHelper("/");

            //act
            string url = helper.Export(true);

            //assert
            Assert.AreEqual("/admin/export.ashx?embed=True", url);
        }

        [TestMethod]
        public void Statistics_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.Statistics();

            //assert
            Assert.AreEqual("/sub/admin/Statistics.aspx", url);
        }

        [TestMethod]
        public void Options_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.Options();

            //assert
            Assert.AreEqual("/sub/admin/Options.aspx", url);
        }

        [TestMethod]
        public void Credits_WithCategoryTypeAndBlogHavingSubfolder_RendersUrlToPostsListPage()
        {
            //arrange
            var routeData = new RouteData();
            routeData.Values.Add("subfolder", "sub");
            AdminUrlHelper helper = SetupUrlHelper("/", routeData);

            //act
            string url = helper.Credits();

            //assert
            Assert.AreEqual("/sub/admin/Credits.aspx", url);
        }

        private static AdminUrlHelper SetupUrlHelper(string appPath)
        {
            return SetupUrlHelper(appPath, new RouteData());
        }

        private static AdminUrlHelper SetupUrlHelper(string appPath, RouteData routeData)
        {
            var routes = new RouteCollection();
            var subtextRoutes = new SubtextRouteMapper(routes, new Mock<IDependencyResolver>().Object);
            Routes.RegisterRoutes(subtextRoutes);
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.ApplicationPath).Returns(appPath);
            httpContext.Setup(c => c.Response.ApplyAppPathModifier(It.IsAny<string>())).Returns<string>(s => s);
            var requestContext = new RequestContext(httpContext.Object, routeData);
            var helper = new BlogUrlHelper(requestContext, routes);
            return new AdminUrlHelper(helper);
        }
    }
}