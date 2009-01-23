using System.Collections.Generic;
using Subtext.Framework.Web;
using Subtext.Framework.Routing;

namespace Subtext.Web.UI.Controls.Aggregate
{
    public class AggregatePage : SubtextPage
    {
        public int? GetGroupIdFromQueryString()
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
    }
}
