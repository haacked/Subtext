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

using System.Web.Mvc;
using Subtext.Framework;
using Subtext.Framework.Routing;
using Subtext.Framework.Text;
using UrlHelper = Subtext.Framework.Routing.BlogUrlHelper;

namespace Subtext.Web.Controllers
{
    public class ManifestController : Controller
    {
        const string ManifestXml = @"<?xml version=""1.0"" encoding=""UTF-8""?>
<manifest xmlns=""http://schemas.microsoft.com/wlw/manifest/weblog"">
  <options>
    <supportsEmbeds>Yes</supportsEmbeds>
    <supportsEmptyTitles>No</supportsEmptyTitles>
    <supportsSlug>Yes</supportsSlug>
    <supportsExcerpt>Yes</supportsExcerpt>
    <supportsCategories>Yes</supportsCategories>
    <supportsNewCategories>Yes</supportsNewCategories>
    <futurePublishDateWarning>No</futurePublishDateWarning>
    <supportsScripts>No</supportsScripts>
    <supportsPages>Yes</supportsPages>
    <supportsExtendedEntries>Yes</supportsExtendedEntries>
    <supportsTrackbacks>{0}</supportsTrackbacks>
  </options>
  <weblog>
    <homepageLinkText>View your blog</homepageLinkText>
    <adminLinkText>Administer your blog</adminLinkText>
    <adminUrl>
      <![CDATA[
        {1}
    ]]>
    </adminUrl>
    <postEditingUrl>
      <![CDATA[
        {2}
    ]]>
    </postEditingUrl>
  </weblog>
</manifest>";

        public ManifestController(ISubtextContext context)
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
            var adminUrlHelper = new AdminUrlHelper(UrlHelper);
            var adminUrl = adminUrlHelper.Home().ToFullyQualifiedUrl(Blog);
            var postEditingUrl = adminUrlHelper.PostsEdit().ToFullyQualifiedUrl(Blog);

            string manifestXml = string.Format(ManifestXml, Blog.TrackbacksEnabled.ToYesNo(), adminUrl, postEditingUrl);

            return Content(manifestXml, "text/xml");
        }
    }
}
