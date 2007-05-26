using System;
using System.Globalization;
using System.Web;
using System.Web.Security;
using Subtext.Framework;
using Subtext.Framework.Configuration;
using Subtext.Framework.Exceptions;
using Subtext.Framework.Format;

namespace Subtext.Web.HostAdmin.UserControls
{
	public partial class BlogEditor : System.Web.UI.UserControl
	{
		protected override void OnInit(EventArgs e)
		{
			Initialize();
			base.OnLoad(e);
		}

		protected void Initialize()
		{
			BindEditHelp();
			string onChangeScript = string.Format(CultureInfo.InvariantCulture, "onPreviewChanged('{0}', '{1}', '{2}', false);", this.hostTextBox.ClientID, this.subfolderTextBox.ClientID, this.virtualDirectory.ClientID);
			string onBlurScript = string.Format(CultureInfo.InvariantCulture, "onPreviewChanged('{0}', '{1}', '{2}', true);", this.hostTextBox.ClientID, this.subfolderTextBox.ClientID, this.virtualDirectory.ClientID);
		
			if (!Page.ClientScript.IsStartupScriptRegistered("SetUrlPreview"))
			{
				string startupScript = "<script type=\"text/javascript\">"
					+ Environment.NewLine
					+ onBlurScript
					+ Environment.NewLine
					+ "</script>";

				Type ctype = this.GetType();
				Page.ClientScript.RegisterStartupScript(ctype, "SetUrlPreview", startupScript);
			}
		
			this.subfolderTextBox.Attributes["onkeyup"] = onChangeScript;
			this.subfolderTextBox.Attributes["onblur"] = onBlurScript;
			this.hostTextBox.Attributes["onkeyup"] = onChangeScript;
			this.hostTextBox.Attributes["onblur"] = onBlurScript;

			this.virtualDirectory.Value = UrlFormats.StripSurroundingSlashes(HttpContext.Current.Request.ApplicationPath);

			if(String.IsNullOrEmpty(this.blogOwnerChooser.UserName))
			{
				if (this.blog != null && this.blog.Owner != null)
					this.blogOwnerChooser.UserName = blog.Owner.UserName;
				else
					this.blogOwnerChooser.UserName = Page.User.Identity.Name;
			}
		}

		/// <summary>
		/// Returns the blog being edited.
		/// </summary>
		public BlogInfo Blog
		{
			get
			{
				if(this.blog == null)
					this.blog = GetBlogById(BlogId);
				return this.blog;
			}
		}

		private BlogInfo blog;

		public string CurrentUserName
		{
			get
			{
				if (Blog.Owner != null)
					return Blog.Owner.UserName;

				if (String.IsNullOrEmpty(this.blogOwnerChooser.UserName))
					return Page.User.Identity.Name;

				return this.blogOwnerChooser.UserName;
			}
		}

		private static BlogInfo GetBlogById(int id)
		{
			if (id == NullValue.NullInt32)
				return new BlogInfo();

			return BlogInfo.GetBlogById(id);
		}

		public event EventHandler SaveComplete;

		protected virtual void OnComplete()
		{
			EventHandler saveCompleteEvent = SaveComplete;
			if (saveCompleteEvent != null)
				saveCompleteEvent(this, EventArgs.Empty);
		}

		public event EventHandler Cancelled;

		protected virtual void OnCancelled()
		{
			EventHandler cancelEvent = Cancelled;
			if (cancelEvent != null)
				cancelEvent(this, EventArgs.Empty);
		}

		/// <summary>
		/// Gets or sets the blog id.
		/// </summary>
		/// <value></value>
		public int BlogId
		{
			get
			{
				if (ViewState[VSKEY_BLOGID] != null)
					return (int)ViewState[VSKEY_BLOGID];
				else
					return NullValue.NullInt32;
			}
			set
			{
				ViewState[VSKEY_BLOGID] = value;
			}
		}
		const string VSKEY_BLOGID = "VS_BLOGID";

		private void SaveConfig()
		{
			if (Page.IsValid)
			{
				try
				{
					if (BlogId != NullValue.NullInt32)
						SaveBlogEdits();
					else
						SaveNewBlog();

					OnComplete();
					return;
				}
				catch (BaseBlogConfigurationException e)
				{
					this.messagePanel.ShowError(e.Message);
				}
			}
		}

		protected void OnSaveClick(object sender, EventArgs e)
		{
			SaveConfig();
		}
		
		// Saves a new blog.  Any exceptions are propagated up to the caller.
		void SaveNewBlog()
		{
			MembershipUser owner = Membership.GetUser(this.blogOwnerChooser.UserName);

			try
			{
				Config.CreateBlog(this.titleTextBox.Text, this.hostTextBox.Text, this.subfolderTextBox.Text, owner);
				this.messagePanel.ShowMessage("Blog Created.");
			}
			catch(Exception e)
			{
				this.messagePanel.ShowError("Darn! An unexpected error occurred.  Not sure what happened. Message: " + e.Message);
			}			 
		}

		// Saves changes to a blog.  Any exceptions are propagated up to the caller.
		void SaveBlogEdits()
		{
			if (this.Blog == null)
				throw new ArgumentNullException("Blog Being Edited", "Ok, somehow the blog you were editing is now null.  This is very odd.");

			blog.Title = this.titleTextBox.Text;
			blog.Host = this.hostTextBox.Text;
			blog.Subfolder = this.subfolderTextBox.Text;
			if (blog.Owner.UserName != this.blogOwnerChooser.UserName)
			{
				MembershipUser newOwner = Membership.GetUser(this.blogOwnerChooser.UserName);
				blog.Owner = newOwner;
			}

			Config.UpdateConfigData(blog);
			this.messagePanel.ShowMessage("Blog Saved.");
		}

		// Contains the various help strings
		void BindEditHelp()
		{
			#region Help Tool Tip Text
			this.blogEditorHelp.HelpText = "<p>Use this page to manage the blogs installed on this server. "
				+ "For more information on configuring blogs, see the <a href=\'http://www.subtextproject.com/Home/Docs/Configuration/tabid/112/Default.aspx\' target=\'_blank\'>configuration docs</a> (opens a new window)."
				+ "</p>";

			this.hostDomainHelpTip.HelpText = "<p><strong>Host Domain</strong> is the domain name for this blog. "
				+ "If you never plan on setting up another blog on this server, then you do not have "
				+ "to worry about this setting.  However, if you decide to add another blog at a later "
				+ "time, it&#8217;s important to update this setting for your initial blog.</p>"
				+ "<p>For example, if you are hosting this blog at http://www.example.com/, the Host Domain "
				+ "would be &#8220;www.example.com&#8221;.</p><p>If you are trying to set this up on your "
				+ "own machine for testing purposes (i.e. it&#8217;s not publicly viewable, you might try "
				+ "&#8220;localhost&#8221; for the host domain.</p>"
				+ "<p><strong>Important:</strong>If you are setting up multiple blogs on the same server, "
				+ "multiple blogs may have the same Host Domain name if they don&#8217;t also have the same "
				+ "Subfolder name.  However, two blogs with different Host Domains may have the same Subfolder "
				+ "name (though that&#8217;s not recommended)."
				+ "</p><p>Also, if there are multiple blogs with the same Host Domain Name, they must all "
				+ "have a non-empty Subfolder name defined.  For more detailed coverage "
				+ ", please visit <a href=\'http://www.subtextproject.com/Developer/UrlToBlogMappings/tabid/119/Default.aspx\' target=\'_blank\'>the multiple blog configuration docs</a> (opens a new window)."
				+ "</p>";

			this.subfolderHelpTip.HelpText = "<p>"
				+ "<strong>This sets the subfolder of the host domain that the blog will appear to be located in.</strong>"
				+ "</p>"
				+ "<p>For example, if you enter &#8220;MyBlog&#8221; "
				+ "(sans quotes of course) for the application, then the root URL to your blog "
				+ "would be <em>http://[HOSTDOMAIN]/MyBlog/Default.aspx</em>"
				+ "</p>"
				+ "<p>"
				+ "Leave this value blank if you wish to host your blog in the root of your website."
				+ "</p>"
				+ "<p><strong>NOTE:</strong> If you specify a sub-folder, you do not need to set up a virtual directory "
				+ "corresponding to the subfolder. But not doing so will require that the blog url include the trailing "
				+ "&#8220;Default.aspx&#8221; or you will have to <a href=\'http://www.subtextproject.com/Configuration/ConfiguringACustom404Page/tabid/121/Default.aspx\' target=\'_blank\'>configure a Custom 404 page</a> (opens a new window).</p>"
				+ "<p>In the above example, if you want the URL to be <em>http://[HOSTDOMAIN]/MyBlog/</em> then you "
				+ "would simply set up a virtual directory (not application) named &#8220;MyBlog&#8221; pointing to the same location as the webroot.</p>"
				+ "<p>For more information, please view <a href=\'http://www.subtextproject.com/Home/Docs/Configuration/tabid/112/Default.aspx\' target=\'_blank\'>the configuration docs</a> (opens a new window).</p>";
			#endregion
		}

		protected bool CreatingBlog
		{
			get
			{
				return BlogId == NullValue.NullInt32;
			}
		}

		protected void OnCancelClick(object sender, EventArgs e)
		{
			OnCancelled();
		}
	}
}