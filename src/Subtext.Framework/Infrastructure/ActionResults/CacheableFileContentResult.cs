#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System;
using System.Web;
using System.Web.Mvc;

namespace Subtext.Infrastructure.ActionResults
{
    /// <summary>
    /// Just like a FileContentResult, but allows for setting cache parameters.
    /// </summary>
    public class CacheableFileContentResult : FileContentResult
    {
        public CacheableFileContentResult(byte[] fileContents, string contentType, DateTime lastModifed,
                                          HttpCacheability cacheability)
            : base(fileContents, contentType)
        {
            LastModified = lastModifed;
            Cacheability = cacheability;
        }

        public DateTime LastModified { get; private set; }

        public HttpCacheability Cacheability { get; private set; }

        public override void ExecuteResult(ControllerContext context)
        {
            HttpCachePolicyBase cache = context.HttpContext.Response.Cache;
            cache.SetCacheability(Cacheability);
            cache.SetLastModified(LastModified);

            base.ExecuteResult(context);
        }
    }
}