using System.Collections.Generic;
using System.Web;
using Subtext.Framework.Web.Handlers;

namespace Subtext.Framework.Syndication
{
    public class OpmlHandler : SubtextHttpHandler {
        public OpmlHandler(ISubtextContext subtextContext, OpmlWriter writer) : base(subtextContext) {
            OpmlWriter = writer;
        }

        protected OpmlWriter OpmlWriter
        {
            get;
            private set;
        }

        public override void ProcessRequest() {
            ProcessRequest(HostInfo.Instance);
        }

        public virtual void ProcessRequest(HostInfo hostInfo) {
            HttpResponseBase response = SubtextContext.HttpContext.Response;
            response.ContentType = "text/xml";

            IEnumerable<Blog> blogs = null;
            if (!hostInfo.BlogAggregationEnabled)
            {
                Blog blog = SubtextContext.Blog;
                if (blog != null)
                {
                    blogs = new Blog[] { blog };
                }
            }
            else
            {
                int? groupId = GetGroupIdFromQueryString(SubtextContext.HttpContext.Request);
                blogs = this.SubtextContext.Repository.GetBlogsByGroup(hostInfo.AggregateBlog.Host, groupId);
            }
            
            OpmlWriter.Write(blogs, response.Output, SubtextContext.UrlHelper);
        }

        public int? GetGroupIdFromQueryString(HttpRequestBase request) {
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
