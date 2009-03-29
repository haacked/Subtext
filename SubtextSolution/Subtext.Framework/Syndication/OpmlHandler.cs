using System.Collections.Generic;
using System.Web;
using System.Web.Routing;
using Subtext.Framework.Routing;

namespace Subtext.Framework.Syndication
{
    public class OpmlHandler : ISubtextHandler
    {
        public OpmlHandler() { 
        }

        public OpmlHandler(OpmlWriter writer) {
            OpmlWriter = writer;
        }

        protected OpmlWriter OpmlWriter
        {
            get;
            private set;
        }

        public ISubtextContext SubtextContext
        {
            get;
            set;
        }

        public RequestContext RequestContext
        {
            get;
            set;
        }

        public UrlHelper Url
        {
            get { 
                return SubtextContext.UrlHelper; 
            }
        }

        public bool IsReusable
        {
            get { 
                return false; 
            }
        }

        public void ProcessRequest(HttpContext context)
        {
            ProcessRequest(HostInfo.Instance);
        }

        public virtual void ProcessRequest(HostInfo hostInfo) {
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
                int? groupId = GetGroupIdFromQueryString(SubtextContext.RequestContext.HttpContext.Request);
                blogs = this.SubtextContext.Repository.GetBlogsByGroup(hostInfo.AggregateBlog.Host, groupId);
            }

            HttpResponseBase response = SubtextContext.RequestContext.HttpContext.Response;
            response.ContentType = "text/xml";

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
