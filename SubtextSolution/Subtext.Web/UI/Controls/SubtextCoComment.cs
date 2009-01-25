using System;
using System.Web.UI;
using Subtext.Framework.Configuration;
using Subtext.Web.Controls;
using Subtext.Framework.Routing;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Implements CoComment for Subtext.
	/// </summary>
	public class SubtextCoComment : CoComment
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SubtextCoComment"/> class.
		/// </summary>
		public SubtextCoComment() : base()
		{
		}

        public UrlHelper Url { 
            get {
                var page = this.Page as IRoutableHandler;
                return page.Url;
            }
        }

		/// <summary>
		/// Raises the <see cref="E:System.Web.UI.Control.Load"/>
		/// event.
		/// </summary>
		/// <param name="e">The <see cref="T:System.EventArgs"/> object that contains the event data.</param>
		protected override void OnLoad(EventArgs e)
		{
			this.BlogTitle = Config.CurrentBlog.Title;
			this.BlogTool = "Subtext";
            this.BlogUrl = Url.BlogUrl().ToFullyQualifiedUrl(Config.CurrentBlog).ToString();
			
			this.CommentAuthorFieldName = GetControlUniqueId("tbName");
			this.CommentButtonId = GetControlUniqueId("btnSubmit");
			if(this.CommentButtonId == null || this.CommentButtonId.Length == 0)
			{
				this.CommentButtonId = GetControlUniqueId("btnCompliantSubmit");
			}
			this.CommentTextFieldName = GetControlUniqueId("tbComment");
            this.CommentFormId = ControlHelper.GetPageFormClientId(this.Page);
		}

		private string GetControlUniqueId(string controlId)
		{
			Control control = ControlHelper.FindControlRecursively(this.Page, controlId);
			if(control != null)
			{
				return control.UniqueID;
			}
		
			return string.Empty;
		}
	}
}
