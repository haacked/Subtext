using Subtext.Framework;

namespace Subtext.Web.UI.Controls
{
    public class AggregateUserControl : BaseControl
    {
        protected int? GetGroupIdFromQueryString()
        {
            int? groupId = null;
            string groupIdText = Request.QueryString["GroupID"];
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

        protected string BlogUrl(Blog blog)
        {
            return Url.BlogUrl(blog);
        }
    }
}
