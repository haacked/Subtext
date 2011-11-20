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

using System.Collections.Generic;
using System.Web;
using Subtext.Framework.Services;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Framework.Syndication
{
    public class OpmlHandler : SubtextHttpHandler
    {
        LazyNotNull<HostInfo> _hostInfo;

        public OpmlHandler(ISubtextContext subtextContext, OpmlWriter writer, LazyNotNull<HostInfo> hostInfo)
            : base(subtextContext)
        {
            OpmlWriter = writer;
            _hostInfo = hostInfo;
        }

        protected OpmlWriter OpmlWriter { get; private set; }

        public override void ProcessRequest()
        {
            HttpResponseBase response = SubtextContext.HttpContext.Response;
            response.ContentType = "text/xml";

            IEnumerable<Blog> blogs = null;
            if (!_hostInfo.Value.BlogAggregationEnabled)
            {
                Blog blog = SubtextContext.Blog;
                if (blog != null)
                {
                    blogs = new[] { blog };
                }
            }
            else
            {
                int? groupId = GetGroupIdFromQueryString(SubtextContext.HttpContext.Request);
                blogs = SubtextContext.Repository.GetBlogsByGroup(SubtextContext.HttpContext.Request.Url.Host, groupId);
            }

            OpmlWriter.Write(blogs, response.Output, SubtextContext.UrlHelper);
        }

        public int? GetGroupIdFromQueryString(HttpRequestBase request)
        {
            int? groupId = null;
            string groupIdText = request.QueryString["GroupID"];
            if (groupIdText != null)
            {
                int parsedGroupId;
                if (int.TryParse(groupIdText, out parsedGroupId))
                {
                    groupId = parsedGroupId;
                }
            }
            return groupId;
        }
    }
}