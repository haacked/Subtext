#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at SourceForge at http://sourceforge.net/projects/subtext
// The development mailing list is at subtext-devs@lists.sourceforge.net 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Collections.Generic;
using System.Web.UI.WebControls;
using Subtext.Extensibility.Providers;
using Subtext.Framework.Providers;
using Subtext.Framework.Configuration;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///	Implements a search control that can be added to a skin.
	/// </summary>
	public class SubtextSearch : BaseControl
	{
		protected TextBox txtSearch;
		protected Button btnSearch;

		protected Repeater SearchResults;

		private void Page_Load(object sender, EventArgs e)
		{
			if(txtSearch != null)
				txtSearch.ValidationGroup = "SubtextSearch";
			
			if(btnSearch != null)
				btnSearch.ValidationGroup = "SubtextSearch";
		}

		#region Web Form Designer generated code

		override protected void OnInit(EventArgs e)
		{
			//
			// CODEGEN: This call is required by the ASP.NET Web Form Designer.
			//
			InitializeComponent();
			AttachCloseButton();
			base.OnInit(e);
		}

		/// <summary>
		///		Required method for Designer support - do not modify
		///		the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.Load += this.Page_Load;
			this.btnSearch.Click += this.btnSearch_Click;
		}

		#endregion

		private void AttachCloseButton()
		{
			LinkButton closeLinkButton = this.FindControl("closeButton") as LinkButton;
			if (closeLinkButton != null)
			{
				closeLinkButton.Click += OnCloseClick;
				return;
			}

			Button closeButton = this.FindControl("closeButton") as Button;
			if (closeButton != null)
			{
				closeButton.Click += OnCloseClick;
				return;
			}
		}

		void OnCloseClick(object sender, EventArgs e)
		{
			if (SearchResults != null)
				SearchResults.Visible = false;
		}

		public void btnSearch_Click(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(txtSearch.Text))
			{
				//fix for the blogs where only one installed
				int blogId = 0;
                if (CurrentBlog.Id > 0) {
                    blogId = CurrentBlog.Id;
                }

                var searchEngine = new EntrySearchProvider(CurrentBlog, Url, Config.ConnectionString);
                ICollection<SearchResult> searchResults = searchEngine.Search(blogId, txtSearch.Text);

				SearchResults.DataSource = searchResults;
				SearchResults.DataBind();
			}
		}

		public class PositionItems
		{
			private string title;
			private string URL;

			public PositionItems(string title, string URL)
			{
				this.title = title;
				this.URL = URL;
			}

			public string Title
			{
				get { return title; }
			}

			public string url
			{
				get { return URL; }
			}
		}
	}
}