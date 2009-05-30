using System;
using System.Web;
using System.Web.Mvc;

namespace Subtext.Framework.Infrastructure.ActionResults
{
    /// <summary>
    /// Just like a FileContentResult, but allows for setting cache parameters.
    /// </summary>
    public class CacheableFileContentResult : FileContentResult
    {
        public CacheableFileContentResult(byte[] fileContents, string contentType, DateTime lastModifed, HttpCacheability cacheability)
            : base(fileContents, contentType) {
            LastModified = lastModifed;
            Cacheability = cacheability;
        }

        public override void ExecuteResult(ControllerContext context)
        {
            var cache = context.HttpContext.Response.Cache;
            cache.SetCacheability(Cacheability);
            cache.SetLastModified(LastModified);
            
            base.ExecuteResult(context);
        }

        public DateTime LastModified {
            get;
            private set;
        }

        public HttpCacheability Cacheability {
            get;
            private set;
        }
    }
}
