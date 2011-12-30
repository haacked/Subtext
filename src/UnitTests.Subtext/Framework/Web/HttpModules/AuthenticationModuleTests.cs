using System;
using System.Collections.Specialized;
using System.Security.Principal;
using System.Web;
using System.Web.Security;
using MbUnit.Framework;
using Moq;
using Moq.Stub;
using Subtext.Framework;
using Subtext.Framework.Web.HttpModules;

namespace UnitTests.Subtext.Framework.Web.HttpModules
{
    [TestFixture]
    public class AuthenticationModuleTests
    {
        [Test]
        public void AuthenticateRequest_WithRequestForStaticFile_ReturnsImmediately()
        {
            // arrange
            var module = new AuthenticationModule();
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Setup(c => c.Request.Cookies).Throws(new InvalidOperationException());
            var blogRequest = new BlogRequest("localhost", string.Empty, new Uri("http://localhost"), false,
                                              RequestLocation.StaticFile, "/");

            // act, assert
            module.AuthenticateRequest(httpContext.Object, blogRequest);
        }

        [Test]
        public void GetFormsAuthenticationTicket_WithRequestHavingNoCookies_ReturnsNull()
        {
            // arrange
            var module = new AuthenticationModule();

            // act
            var authTicket = module.GetFormsAuthenticationTicket(null);

            // assert
            Assert.IsNull(authTicket);
        }

        [Test]
        public void GetFormsAuthenticationTicket_WithRequestHavingIndecipherableAuthCookies_ReturnsNull()
        {
            // arrange
            var module = new AuthenticationModule();
            var badCookie = new HttpCookie(".ASPXAUTH.42") { Value = "STEOHsuthosaeuthoes234234sThisIsGarbage", Expires = DateTime.UtcNow };

            // act
            var ticket = module.GetFormsAuthenticationTicket(badCookie);

            // assert
            Assert.IsNull(ticket);
        }

        [Test]
        public void GetFormsAuthenticationTicket_WithRequestHavingNullAuthTicket_ReturnsNull()
        {
            // arrange
            var module = new AuthenticationModule();
            var authCookie = new HttpCookie(".ASPXAUTH.42") { Value = null };

            // act
            var ticket = module.GetFormsAuthenticationTicket(authCookie);

            // assert
            Assert.IsNull(ticket);
        }

        [Test]
        public void GetFormsAuthenticationTicket_WithRequestHavingExpiredAuthCookies_SetsUserToGenericPrincipalWithRoles()
        {
            // arrange
            var module = new AuthenticationModule();
            const string roles = "Admins|HostAdmins|Users";
            var ticket = new FormsAuthenticationTicket(1, ".ASPXAUTH.42", DateTime.UtcNow, DateTime.UtcNow.AddDays(-10), true,
                                                       roles);
            Assert.IsTrue(ticket.Expired);
            string cookieValue = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(".ASPXAUTH.42") { Value = cookieValue };

            // act
            var authTicket = module.GetFormsAuthenticationTicket(authCookie);

            // assert
            Assert.IsNull(authTicket);
        }

        [Test]
        public void AuthenticateRequest_WithRequestHavingValidAuthCookies_SetsUserToGenericPrincipalWithRoles()
        {
            // arrange
            var module = new AuthenticationModule();
            const string roles = "Admins|HostAdmins|Users";
            var ticket = new FormsAuthenticationTicket(1, ".ASPXAUTH.42", DateTime.Now, DateTime.Now.AddDays(60), true,
                                                       roles);
            string cookieValue = FormsAuthentication.Encrypt(ticket);
            var authCookie = new HttpCookie(".ASPXAUTH.42") { Value = cookieValue };
            var cookies = new HttpCookieCollection { authCookie };
            var httpContext = new Mock<HttpContextBase>();
            httpContext.Stub(c => c.User);
            httpContext.Setup(c => c.Request.Path).Returns("/");
            httpContext.Setup(c => c.Request.QueryString).Returns(new NameValueCollection());
            httpContext.Setup(c => c.Request.Cookies).Returns(cookies);
            httpContext.Setup(c => c.Response.Cookies).Returns(cookies);
            var blogRequest = new BlogRequest("localhost", string.Empty, new Uri("http://localhost"), false,
                                              RequestLocation.Blog, "/") { Blog = new Blog { Id = 42 } };

            // act
            module.AuthenticateRequest(httpContext.Object, blogRequest);

            // assert
            var principal = httpContext.Object.User as GenericPrincipal;
            Assert.IsNotNull(principal);
            Assert.IsTrue(principal.IsInRole("Admins"));
            Assert.IsTrue(principal.IsInRole("HostAdmins"));
            Assert.IsTrue(principal.IsInRole("Users"));
        }
    }
}