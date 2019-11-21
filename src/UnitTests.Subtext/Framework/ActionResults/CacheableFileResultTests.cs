using System;
using System.Web;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Subtext.Infrastructure.ActionResults;

namespace UnitTests.Subtext.Framework.ActionResults
{
    [TestClass]
    public class CacheableFileResultTests
    {
        [TestMethod]
        public void CtorSetsLastModified()
        {
            // arrange
            DateTime dateTime = DateTime.UtcNow;

            // act
            var result = new CacheableFileContentResult(new byte[] { }, "image/gif", dateTime, HttpCacheability.Public);

            // assert
            Assert.AreEqual(dateTime, result.LastModified);
        }

        [TestMethod]
        public void CtorSetsCacheability()
        {
            // arrange, act
            var result = new CacheableFileContentResult(new byte[] { }, "image/gif", DateTime.UtcNow, HttpCacheability.Server);

            // assert
            Assert.AreEqual(HttpCacheability.Server, result.Cacheability);
        }

        [TestMethod]
        public void ExecuteResultSetsCacheLastModified()
        {
            // arrange
            DateTime dateTime = DateTime.UtcNow;
            var result = new CacheableFileContentResult(new byte[] { }, "image/gif", dateTime, HttpCacheability.Server);
            var httpContext = new Mock<HttpContextBase>();
            DateTime lastModified = DateTime.MinValue;
            httpContext.Setup(h => h.Response.Cache.SetLastModified(It.IsAny<DateTime>())).Callback<DateTime>(
                date => lastModified = date);
            httpContext.Setup(h => h.Response.OutputStream.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()));
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = httpContext.Object;

            // act
            result.ExecuteResult(controllerContext);

            // assert
            Assert.AreEqual(dateTime, lastModified);
        }

        [TestMethod]
        public void ExecuteResultSetsCacheCacheability()
        {
            // arrange
            DateTime dateTime = DateTime.UtcNow;
            var result = new CacheableFileContentResult(new byte[] { }, "image/gif", dateTime, HttpCacheability.Public);
            var httpContext = new Mock<HttpContextBase>();
            HttpCacheability cacheability = HttpCacheability.NoCache;
            httpContext.Setup(h => h.Response.Cache.SetCacheability(It.IsAny<HttpCacheability>())).Callback
                <HttpCacheability>(cacheSetting => cacheability = cacheSetting);
            httpContext.Setup(h => h.Response.OutputStream.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>()));
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = httpContext.Object;

            // act
            result.ExecuteResult(controllerContext);

            // assert
            Assert.AreEqual(HttpCacheability.Public, cacheability);
        }

        [TestMethod]
        public void ExecuteResultWritesBytesToResponse()
        {
            // arrange
            DateTime dateTime = DateTime.UtcNow;
            var result = new CacheableFileContentResult(new byte[] { 1, 2, 3, 2, 1 }, "image/gif", dateTime,
                                                        HttpCacheability.Server);
            var httpContext = new Mock<HttpContextBase>();
            DateTime lastModified = DateTime.MinValue;
            httpContext.Setup(h => h.Response.Cache.SetLastModified(It.IsAny<DateTime>())).Callback<DateTime>(
                date => lastModified = date);
            byte[] writtenBytes = null;
            httpContext.Setup(h => h.Response.OutputStream.Write(It.IsAny<byte[]>(), 0, It.IsAny<int>())).Callback
                <byte[], int, int>((bytes, i, j) => writtenBytes = bytes);
            var controllerContext = new ControllerContext();
            controllerContext.HttpContext = httpContext.Object;

            // act
            result.ExecuteResult(controllerContext);

            // assert
            CollectionAssert.AreEqual(new byte[] { 1, 2, 3, 2, 1 }, writtenBytes);
        }
    }
}