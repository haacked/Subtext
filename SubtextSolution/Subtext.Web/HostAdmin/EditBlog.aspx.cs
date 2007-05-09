using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using Subtext.Framework;

namespace Subtext.Web.HostAdmin
{
	public partial class EditBlog : System.Web.UI.Page
	{
		protected override void OnInit(EventArgs e)
		{
			int blogId;
			if (int.TryParse(Request["blog-id"], out blogId))
				this.blogEditor.BlogId = blogId;
			else
				this.blogEditor.BlogId = NullValue.NullInt32;

			base.OnInit(e);
		}
	}
}
