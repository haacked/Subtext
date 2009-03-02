using System;
using System.Collections.Generic;
using Subtext.Framework;
using Subtext.Framework.Components;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	///		Summary description for ArticleCategories.
	/// </summary>
	public class ArticleCategories : Subtext.Web.UI.Controls.BaseControl
	{
		protected Subtext.Web.UI.Controls.CategoryList Categories;

		private void Page_Load(object sender, System.EventArgs e)
		{
		}

		protected override void OnLoad(EventArgs e)
		{
			base.OnLoad (e);
			Categories.LinkCategories = GetArchiveCategories(SubtextContext.Blog);
		}

		protected ICollection<LinkCategory> GetArchiveCategories(Blog blog)
		{
            List<LinkCategory> lcc = new List<LinkCategory>();

			lcc.Add(UIData.Links(CategoryType.StoryCollection, blog));			

			return lcc;
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
			this.Load += new System.EventHandler(this.Page_Load);

		}
		#endregion
	}
}
