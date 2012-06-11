using System;
using System.Web;
using System.Web.Mvc;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Services;
using Subtext.Framework.Web;
using Subtext.Infrastructure.ActionResults;

namespace Subtext.Web.Controllers
{
    public class StatisticsController : Controller
    {
        static readonly byte[] AggregatorOnePixelBlankGif =
            Convert.FromBase64String("R0lGODlhAQABAIAAANvf7wAAACH5BAEAAAAALAAAAAABAAEAAAICRAEAOw==");

        public StatisticsController(ISubtextContext context, IStatisticsService statisticsService)
        {
            StatisticsService = statisticsService;
            SubtextContext = context;
        }

        public IStatisticsService StatisticsService { get; private set; }

        public ISubtextContext SubtextContext { get; private set; }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult RecordAggregatorView(int id)
        {
            if (CachedVersionIsOkay(SubtextContext.HttpContext.Request))
            {
                return new NotModifiedResult();
            }

            if (id > 0)
            {
                var entryView = new EntryView
                {
                    BlogId = SubtextContext.Blog.Id,
                    EntryId = id,
                    PageViewType = PageViewType.AggView
                };
                StatisticsService.RecordAggregatorView(entryView);
            }

            return new CacheableFileContentResult(AggregatorOnePixelBlankGif, "image/gif", DateTime.UtcNow, HttpCacheability.Public);
        }

        private static bool CachedVersionIsOkay(HttpRequestBase request)
        {
            //Get header value
            DateTime dt = HttpHelper.GetIfModifiedSinceDateUtc(request);
            if (NullValue.IsNull(dt))
            {
                return false;
            }

            //convert to datetime and add 6 hours. 
            //We don't want to count quick reclicks.
            return dt.AddHours(6) >= DateTime.UtcNow;
        }
    }
}