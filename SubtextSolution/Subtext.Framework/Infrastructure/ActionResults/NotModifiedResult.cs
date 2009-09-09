using System.Web;
using System.Web.Mvc;

namespace Subtext.Infrastructure.ActionResults
{
    public class NotModifiedResult : ActionResult
    {
        public override void ExecuteResult(ControllerContext context)
        {
            HttpResponseBase response = context.HttpContext.Response;
            response.StatusCode = 304;
            response.SuppressContent = true;
        }
    }
}