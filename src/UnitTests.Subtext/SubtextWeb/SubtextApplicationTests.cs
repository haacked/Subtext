using System;
using System.IO;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Framework.Data;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Infrastructure.Installation;
using Subtext.Framework.Routing;
using Subtext.Framework.Web.HttpModules;
using Subtext.Web;

namespace UnitTests.Subtext.SubtextWeb
{
    [TestClass]
    public class SubtextApplicationTests
    {
        [TestMethod]
        public void StartApplication_SetsLogInitializedToFalse()
        {
            // arrange
            var app = new SubtextApplication();
            var server = new Mock<HttpServerUtilityBase>();

            // act
            app.StartApplication(new SubtextRouteMapper(new RouteCollection(), new Mock<IDependencyResolver>().Object),
                                 server.Object);

            // assert
            Assert.IsFalse(app.LogInitialized);
        }

        [TestMethod]
        public void StartApplication_AddsAdminDirectoryToInvalidPaths_IfAdminDirectoryExistsInWrongPlace()
        {
            // arrange
            var app = new SubtextApplication();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.MapPath("~/Admin")).Returns(Directory.CreateDirectory("Admin").FullName);

            // act
            app.StartApplication(new SubtextRouteMapper(new RouteCollection(), new Mock<IDependencyResolver>().Object),
                                 server.Object);

            // assert
            Assert.AreEqual("~/Admin", app.DeprecatedPhysicalPaths[0]);
        }

        [TestMethod]
        public void StartApplication_AddsLoginFileToInvalidPaths_IfLoginFileExistsInWrongPlace()
        {
            // arrange
            var app = new SubtextApplication();
            var server = new Mock<HttpServerUtilityBase>();
            using (StreamWriter writer = File.CreateText("login.aspx"))
            {
                writer.Write("test");
            }
            server.Setup(s => s.MapPath("~/login.aspx")).Returns(Path.GetFullPath("login.aspx"));

            // act
            app.StartApplication(new SubtextRouteMapper(new RouteCollection(), new Mock<IDependencyResolver>().Object),
                                 server.Object);

            // assert
            Assert.AreEqual("~/login.aspx", app.DeprecatedPhysicalPaths[0]);
        }

        [TestMethod]
        public void StartApplication_AddsHostAdminDirectoryToInvalidPaths_IfHostAdminDirectoryExistsInWrongPlace()
        {
            // arrange
            var app = new SubtextApplication();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.MapPath("~/HostAdmin")).Returns(Directory.CreateDirectory("HostAdmin").FullName);

            // act
            app.StartApplication(new SubtextRouteMapper(new RouteCollection(), new Mock<IDependencyResolver>().Object),
                                 server.Object);

            // assert
            Assert.AreEqual("~/HostAdmin", app.DeprecatedPhysicalPaths[0]);
        }

        [TestMethod]
        public void BeginApplicationRequest_LogsThatTheApplicationHasStartedAndSetsLogInitializedTrue()
        {
            // arrange
            var app = new SubtextApplication();
            Assert.IsFalse(app.LogInitialized);
            var log = new Mock<ILog>();
            string logMessage = null;
            log.Setup(l => l.Info(It.IsAny<string>())).Callback<object>(s => logMessage = s.ToString());

            // act
            app.BeginApplicationRequest(log.Object);

            // assert
            Assert.AreEqual("Subtext Application Started", logMessage);
            Assert.IsTrue(app.LogInitialized);
        }

        [TestMethod]
        public void BeginApplicationRequest_WithOldAdminDirectory_ThrowsDeprecatedFileExistsException()
        {
            // arrange
            var app = new SubtextApplication();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.MapPath("~/Admin")).Returns(Directory.CreateDirectory("Admin").FullName);
            app.StartApplication(new SubtextRouteMapper(new RouteCollection(), new Mock<IDependencyResolver>().Object),
                                 server.Object);

            // act, assert
            var exception = UnitTestHelper.AssertThrows<DeprecatedPhysicalPathsException>(() =>
                                                                                          app.BeginApplicationRequest(
                                                                                              new Mock<ILog>().Object));

            Assert.AreEqual("~/Admin", exception.InvalidPhysicalPaths[0]);
        }

        [TestMethod]
        public void UnwrapHttpUnhandledException_WithHttpUnhandledExceptionContainingNoInnerException_ReturnsNull()
        {
            // act
            Exception exception = SubtextApplication.UnwrapHttpUnhandledException(new HttpUnhandledException());

            // assert
            Assert.IsNull(exception);
        }

        [TestMethod]
        public void
            UnwrapHttpUnhandledException_WithHttpUnhandledExceptionContainingInnerException_ReturnsInnerException()
        {
            // arrange
            var innerException = new Exception();

            // act
            Exception exception =
                SubtextApplication.UnwrapHttpUnhandledException(new HttpUnhandledException("whatever", innerException));

            // assert
            Assert.AreEqual(innerException, exception);
        }

        [TestMethod]
        public void OnApplicationError_WithUnhandledExceptionAndCustomErrorsEnabled_TransfersToErrorPage()
        {
            // arrange
            string transferLocation = null;
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.Transfer(It.IsAny<string>())).Callback<string>(s => transferLocation = s);

            // act
            SubtextApplication.HandleUnhandledException(new Exception(), server.Object, true /* customErrorEnabled */,
                                                        new Mock<ILog>().Object);

            // assert
            Assert.AreEqual("~/aspx/SystemMessages/error.aspx", transferLocation);
        }

        [TestMethod]
        public void OnApplicationError_WithUnhandledExceptionAndCustomErrorsDisabled_LogsMessage()
        {
            // arrange
            var log = new Mock<ILog>();
            string logMessage = null;
            log.Setup(l => l.Error(It.IsAny<object>(), It.IsAny<Exception>())).Callback<object, Exception>(
                (s, e) => logMessage = s.ToString());

            // act
            SubtextApplication.HandleUnhandledException(new Exception(), null, false /* customErrorEnabled */,
                                                        log.Object);

            // assert
            Assert.AreEqual("Unhandled Exception trapped in Global.asax", logMessage);
        }

        [TestMethod]
        public void OnApplicationError_WithHttpUnhandledExceptionContainingNoInnerException_Transfers()
        {
            // arrange
            var app = new SubtextApplication();
            string transferLocation = null;
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.Transfer(It.IsAny<string>())).Callback<string>(s => transferLocation = s);

            // act
            app.OnApplicationError(new HttpUnhandledException(), server.Object, new Mock<ILog>().Object, null);

            // assert
            Assert.AreEqual("~/aspx/SystemMessages/error.aspx", transferLocation);
        }

        [TestMethod]
        public void LogIfCommentException_LogsCommentException()
        {
            // arrange
            var exception = new CommentDuplicateException();
            var log = new Mock<ILog>();
            string logMessage = null;
            log.Setup(l => l.Info(It.IsAny<string>(), exception)).Callback<object, Exception>(
                (o, e) => logMessage = o.ToString());

            // act
            SubtextApplication.LogIfCommentException(exception, log.Object);

            // assert
            Assert.AreEqual("Comment exception thrown and handled in Global.asax.", logMessage);
        }

        [TestMethod]
        public void LogIfCommentException_DoesNothingForNonCommentException()
        {
            // arrange
            var exception = new Exception();
            var log = new Mock<ILog>();
            log.Setup(l => l.Info(It.IsAny<string>())).Throws(new Exception("Nothing should have been logged"));

            // act, assert
            SubtextApplication.LogIfCommentException(exception, log.Object);
        }

        [TestMethod]
        public void HandleDrepecatedFilePathsException_WithNonDeprecatedPhysicalPathsException_ReturnsFalse()
        {
            // arrange
            var exception = new Exception();
            var application = new Mock<SubtextApplication>();
            application.Setup(a => a.FinishRequest());

            // act
            bool handled = SubtextApplication.HandleDeprecatedFilePathsException(exception, null, application.Object);

            // assert
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void HandleDeprecatedFilePathsException_WithDepecatedPhysicalPathsException_ReturnsFalse()
        {
            // arrange
            var exception = new DeprecatedPhysicalPathsException(new[] { "~/Admin" });
            var server = new Mock<HttpServerUtilityBase>();
            string transferLocation = null;
            server.Setup(s => s.Execute(It.IsAny<string>(), false)).Callback<string, bool>(
                (s, b) => transferLocation = s);
            var application = new Mock<SubtextApplication>();
            application.Setup(a => a.FinishRequest());

            // act
            bool handled = SubtextApplication.HandleDeprecatedFilePathsException(exception, server.Object,
                                                                                 application.Object);

            // assert
            Assert.AreEqual("~/aspx/SystemMessages/DeprecatedPhysicalPaths.aspx", transferLocation);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void HandleSqlException_ReturnsFalseForNonSqlException()
        {
            // arrange
            var exception = new Exception();

            // act
            bool handled = SubtextApplication.HandleSqlException(exception, null);

            // assert
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void
            HandleSqlException_WithSqlServerDoesNotExistOrAccessDeniedError_TransfersToBadConnectionStringPageAndReturnsTrue
            ()
        {
            // arrange
            var server = new Mock<HttpServerUtilityBase>();
            string transferLocation = null;
            server.Setup(s => s.Transfer(It.IsAny<string>())).Callback<string>(s => transferLocation = s);

            // act
            bool handled =
                SubtextApplication.HandleSqlExceptionNumber((int)SqlErrorMessage.SqlServerDoesNotExistOrAccessDenied, "",
                                                            server.Object);

            // assert
            Assert.AreEqual("~/aspx/SystemMessages/CheckYourConnectionString.aspx", transferLocation);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void
            HandleSqlException_WithSqlServerCouldNotFindStoredProcedureAndProcNameIsBlog_GetConfig_TransfersToBadConnectionStringPageAndReturnsTrue
            ()
        {
            // arrange

            var server = new Mock<HttpServerUtilityBase>();
            string transferLocation = null;
            server.Setup(s => s.Transfer(It.IsAny<string>())).Callback<string>(s => transferLocation = s);

            // act
            bool handled = SubtextApplication.HandleSqlExceptionNumber(
                (int)SqlErrorMessage.CouldNotFindStoredProcedure, "'blog_GetConfig'", server.Object);

            // assert
            Assert.AreEqual("~/aspx/SystemMessages/CheckYourConnectionString.aspx", transferLocation);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void HandleRequestLocationException_WithNullBlogRequest_RedirectsToInstallDefault()
        {
            // arrange
            var response = new Mock<HttpResponseBase>();
            string redirectLocation = null;
            response.Setup(r => r.Redirect(It.IsAny<string>(), true)).Callback<string, bool>(
                (s, endRequest) => redirectLocation = s);
            var blogRequest = new BlogRequest("", "", new Uri("http://haacked.com/"), false);
            var installationManager = new Mock<IInstallationManager>();
            installationManager.Setup(i => i.InstallationActionRequired(It.IsAny<Version>(), null)).Returns(true);

            // act
            bool handled = SubtextApplication.HandleRequestLocationException(null, null, installationManager.Object, response.Object);

            // assert
            Assert.AreEqual("~/install/default.aspx", redirectLocation);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void HandleRequestLocationException_WithInstallationActionRequired_RedirectsToInstallDefault()
        {
            // arrange
            var response = new Mock<HttpResponseBase>();
            string redirectLocation = null;
            response.Setup(r => r.Redirect(It.IsAny<string>(), true)).Callback<string, bool>(
                (s, endRequest) => redirectLocation = s);
            var blogRequest = new BlogRequest("", "", new Uri("http://haacked.com/"), false);
            var installationManager = new Mock<IInstallationManager>();
            installationManager.Setup(i => i.InstallationActionRequired(It.IsAny<Version>(), null)).Returns(true);

            // act
            bool handled = SubtextApplication.HandleRequestLocationException(null, blogRequest, installationManager.Object, response.Object);

            // assert
            Assert.AreEqual("~/install/default.aspx", redirectLocation);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void HandleRequestLocationException_IgnoresInstallationLocation()
        {
            // arrange
            var response = new Mock<HttpResponseBase>();
            response.Setup(r => r.Redirect(It.IsAny<string>(), true)).Throws(
                new Exception("Test Failed. Should not have redirected"));
            var blogRequest = new BlogRequest("", "", new Uri("http://haacked.com/"), false,
                                              RequestLocation.Installation, "/");
            var installManager = new Mock<IInstallationManager>();
            installManager.Setup(i => i.InstallationActionRequired(It.IsAny<Version>(), null)).Throws(new InvalidOperationException());

            // act
            bool handled = SubtextApplication.HandleRequestLocationException(new Exception(), blogRequest, installManager.Object,
                                                                             response.Object);

            // assert
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void HandleRequestLocationException_IgnoresUpgradeLocation()
        {
            // arrange
            var response = new Mock<HttpResponseBase>();
            response.Setup(r => r.Redirect(It.IsAny<string>(), true)).Throws(
                new Exception("Test Failed. Should not have redirected"));
            var blogRequest = new BlogRequest("", "", new Uri("http://haacked.com/"), false, RequestLocation.Upgrade, "/");
            var installManager = new Mock<IInstallationManager>();
            installManager.Setup(i => i.InstallationActionRequired(It.IsAny<Version>(), It.IsAny<Exception>())).Throws(new InvalidOperationException());

            // act
            bool handled = SubtextApplication.HandleRequestLocationException(new Exception(), blogRequest, installManager.Object,
                                                                             response.Object);

            // assert
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void HandleRequestLocationException_HandlesBlogInactiveException()
        {
            // arrange
            var exception = new BlogInactiveException();
            var response = new Mock<HttpResponseBase>();
            string redirectLocation = null;
            response.Setup(r => r.Redirect(It.IsAny<string>(), true)).Callback<string, bool>(
                (s, endRequest) => redirectLocation = s);
            var blogRequest = new BlogRequest("", "", new Uri("http://haacked.com/"), false);
            var installManager = new Mock<IInstallationManager>().Object;

            // act
            bool handled = SubtextApplication.HandleRequestLocationException(exception, blogRequest, installManager,
                                                                             response.Object);

            // assert
            Assert.AreEqual("~/SystemMessages/BlogNotActive.aspx", redirectLocation);
            Assert.IsTrue(handled);
        }

        [TestMethod]
        public void HandleRequestLocationException_IgnoresBlogInactiveExceptionWhenInSystemMessagesDirectory()
        {
            // arrange
            var exception = new BlogInactiveException();
            var response = new Mock<HttpResponseBase>();
            response.Setup(r => r.Redirect(It.IsAny<string>(), true)).Throws(new Exception("Should not have redirected"));
            var blogRequest = new BlogRequest("", "", new Uri("http://haacked.com/"), false,
                                              RequestLocation.SystemMessages, "/");
            var installManager = new Mock<IInstallationManager>().Object;

            // act
            bool handled = SubtextApplication.HandleRequestLocationException(exception, blogRequest, installManager,
                                                                             response.Object);

            // assert
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void
            HandleBadConnectionStringException_WithInvalidOperationExceptionMentioningConnectionString_TransfersToBadConnectionStringPage
            ()
        {
            // arrange
            var exception = new InvalidOperationException("No ConnectionString Found");
            var server = new Mock<HttpServerUtilityBase>();
            string transferLocation = null;
            server.Setup(s => s.Transfer(It.IsAny<string>())).Callback<string>(s => transferLocation = s);

            // act
            bool handled = SubtextApplication.HandleBadConnectionStringException(exception, server.Object);

            // assert
            Assert.IsTrue(handled);
            Assert.AreEqual("~/aspx/SystemMessages/CheckYourConnectionString.aspx", transferLocation);
        }

        [TestMethod]
        public void
            HandleBadConnectionStringException_WithInvalidOperationExceptionContainingOtherMessages_IgnoresException()
        {
            // arrange
            var exception = new InvalidOperationException("Something or other");
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.Transfer(It.IsAny<string>())).Throws(new Exception("Should not have transfered"));

            // act
            bool handled = SubtextApplication.HandleBadConnectionStringException(exception, server.Object);

            // assert
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void
            HandleBadConnectionStringException_WithArgumentExceptionContainingKeywordNotSupported_TransfersToBadConnectionStringPage
            ()
        {
            // arrange
            var exception = new ArgumentException("Keyword not supported");
            var server = new Mock<HttpServerUtilityBase>();
            string transferLocation = null;
            server.Setup(s => s.Transfer(It.IsAny<string>())).Callback<string>(s => transferLocation = s);

            // act
            bool handled = SubtextApplication.HandleBadConnectionStringException(exception, server.Object);

            // assert
            Assert.IsTrue(handled);
            Assert.AreEqual("~/aspx/SystemMessages/CheckYourConnectionString.aspx", transferLocation);
        }

        [TestMethod]
        public void
            HandleBadConnectionStringException_WithArgumentExceptionContainingInvalidValueForKey_TransfersToBadConnectionStringPage
            ()
        {
            // arrange
            var exception = new ArgumentException("Invalid value for key");
            var server = new Mock<HttpServerUtilityBase>();
            string transferLocation = null;
            server.Setup(s => s.Transfer(It.IsAny<string>())).Callback<string>(s => transferLocation = s);

            // act
            bool handled = SubtextApplication.HandleBadConnectionStringException(exception, server.Object);

            // assert
            Assert.IsTrue(handled);
            Assert.AreEqual("~/aspx/SystemMessages/CheckYourConnectionString.aspx", transferLocation);
        }

        [TestMethod]
        public void HandleBadConnectionStringException_WithArgumentExceptionContainingOtherMessages_IgnoresException()
        {
            // arrange
            var exception = new ArgumentException("Something or other");
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.Transfer(It.IsAny<string>())).Throws(new Exception("Should not have transfered"));

            // act
            bool handled = SubtextApplication.HandleBadConnectionStringException(exception, server.Object);

            // assert
            Assert.IsFalse(handled);
        }

        [TestMethod]
        public void HandleBadConnectionStringException_IgnoresOtherExceptions()
        {
            // arrange
            var exception = new Exception();
            var server = new Mock<HttpServerUtilityBase>();
            server.Setup(s => s.Transfer(It.IsAny<string>())).Throws(new Exception("Should not have transfered"));

            // act
            bool handled = SubtextApplication.HandleBadConnectionStringException(exception, server.Object);

            // assert
            Assert.IsFalse(handled);
        }

        [TestInitialize]
        public void TestInitialize()
        {
            CleanupDirectories();
        }

        [TestCleanup]
        public void TestCleanup()
        {
            CleanupDirectories();
            File.Delete("login.aspx");
        }

        private static void CleanupDirectories()
        {
            var directories = new[] { "Admin", "HostAdmin" };
            Array.ForEach(directories, directory =>
                {
                    if (Directory.Exists(directory))
                    {
                        Directory.Delete(directory, true);
                    }
                });
        }
    }
}