using System;
using System.Data;
using System.Web;
using System.Web.UI.WebControls;
using Subtext.Framework.Providers;
using Subtext.Framework.Configuration;
using System.Collections.Generic;
using Subtext.Framework.Components;
using System.Globalization;

namespace Subtext.Web.UI.Controls
{
    public class AggSyndication : BaseControl
    {
        protected Repeater blogGroupRepeater;

        protected override void OnLoad(EventArgs e)
        {
            base.OnLoad(e);

            IList<BlogGroup> groups = Config.ListBlogGroups(true);
            blogGroupRepeater.DataSource = groups;
            blogGroupRepeater.DataBind();
        }
    }
}
