using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;

namespace Subtext.Web.UI.Controls.Aggregate
{
    public class AggregatePage : Page
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
