using System;
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

		protected virtual void Redirect(object sender, EventArgs e)
		{
			Response.Redirect("Default.aspx");
		}
	}
}
