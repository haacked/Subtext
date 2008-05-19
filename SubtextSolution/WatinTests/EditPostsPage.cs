using System;
using WatiN.Core;
using WatinTests.PageElements;

namespace WatinTests
{
	public class EditPostsPage : BrowserPageBase
	{
		public EditPostsPage(Browser browser) : base(browser)
		{
		}

		public void Reload()
		{
			Browser.GoToUrl(PageUrl);
			Browser.Refresh();
		}

		public override string PageUrl
		{
			get { return "/Admin/Posts/Edit.aspx"; }
		}

		public void ClickNavLink(PostsNavigationLink link)
		{
			base.ClickNavLink(link);
		}

		public TextField TitleField
		{
			get
			{
				return ASPTextField("txbTitle");
			}
		}

		// In AssemblySetupAndTearDown we replace FCKEditor with the plain textbox editor in Web.config.
		public TextField RichTextEditorField
		{
			get
			{
				return Browser.ASPTextField("richTextEditor");
			}
		}

		public Button PostButton
		{
			get { return ButtonByValue("Post"); }
		}

		public PostsTable TableOfPosts
		{
			get { return new PostsTable(Browser, "Listing"); }
		}

		//xEditingArea is the TD in the IFRame...
	}

	public enum PostsNavigationLink
	{
		New_Post,
		Edit_Categories,
		Rebuild_All_Tags,
	}
}
