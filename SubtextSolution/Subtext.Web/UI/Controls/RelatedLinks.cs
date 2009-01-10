using System;
using System.Collections;
using System.Web.UI.WebControls;
using Subtext.Framework;
using Subtext.Framework.Components;
using Subtext.Framework.Data;
using Subtext.Framework.Providers;

namespace Subtext.Web.UI.Controls
{
	public class RelatedLinks : BaseControl
	{
		protected Repeater urlRelatedLinks;

		private void Page_Load(object sender, EventArgs e)
		{
			ArrayList myRelLinks = new ArrayList();
			int blogId = Blog.Id >= 1 ? Blog.Id : 0;

			Entry entry = Cacher.GetEntryFromRequest(CacheDuration.Short, true, SubtextContext);
            var relatedEntries = ObjectProvider.Instance().GetRelatedEntries(blogId, entry.Id, 10);

			foreach(var relatedEntry in relatedEntries)
			{
				string myURL = Url.EntryUrl(relatedEntry);
				myRelLinks.Add(new PositionItems(relatedEntry.Title, myURL));
			}

			urlRelatedLinks.DataSource = myRelLinks;
			urlRelatedLinks.DataBind();
		}

		#region Web Form Designer generated code

		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += new EventHandler(this.Page_Load);
		}

		#endregion

	}
	
	public class PositionItems
	{
		public PositionItems(string title, string url)
		{
			Title = title;
            Url = url;
		}

        public string Title
        {
            get;
            private set;
        }

		public string Url
		{
			get;
            private set;
		}
	}
}
