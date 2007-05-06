using System;

namespace Subtext.Web.HostAdmin
{
	public partial class Default : System.Web.UI.Page
	{
		protected override void OnInit(EventArgs e)
		{
			this.blogsList.BlogEdit += OnBlogEdit;
			this.blogEditor.SaveComplete += OnSaveComplete;
			this.blogEditor.Cancelled += OnCancelled;
			base.OnInitComplete(e);
		}

		void OnSaveComplete(object sender, EventArgs e)
		{
			this.blogsList.Visible = true;
			this.blogEditor.Visible = false;
		}

		void OnCancelled(object sender, EventArgs e)
		{
			this.blogsList.Visible = true;
			this.blogEditor.Visible = false;
		}

		void OnBlogEdit(object sender, UserControls.BlogEditEventArgs e)
		{
			this.blogsList.Visible = false;
			this.blogsList.ShowMessage("Blog Update Cancelled. Nothing to see here.");
			this.blogEditor.BlogId = e.BlogId;
			this.blogEditor.Visible = true;
		}
	}
}
