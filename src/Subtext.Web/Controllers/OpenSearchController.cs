using System.Web.Mvc;
using Subtext.Framework;
using Subtext.Framework.Routing;
using UrlHelper = Subtext.Framework.Routing.BlogUrlHelper;

namespace Subtext.Web.Controllers
{
    public class OpenSearchController : Controller
    {
        const string ManifestXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
 <OpenSearchDescription xmlns=""http://a9.com/-/spec/opensearch/1.1/"">
   <ShortName>{0}</ShortName>
   <Description>{1}</Description>
    <Contact>{2}</Contact>
   <Url type=""text/html"" 
        template=""{3}?q={{searchTerms}}""/>
 </OpenSearchDescription>";

        public OpenSearchController(ISubtextContext context)
        {
            SubtextContext = context;
        }


        public ISubtextContext SubtextContext { get; private set; }

        public UrlHelper UrlHelper
        {
            get
            {
                return SubtextContext.UrlHelper;
            }
        }

        public Blog Blog
        {
            get
            {
                return SubtextContext.Blog;
            }
        }

        public ActionResult Index()
        {
            var searchUrl = UrlHelper.SearchPageUrl().ToFullyQualifiedUrl(Blog);

            string manifestXml = string.Format(ManifestXml, Blog.Title, Blog.SubTitle, Blog.Email, searchUrl);

            return Content(manifestXml, "application/opensearchdescription+xml");
        }

    }
}
