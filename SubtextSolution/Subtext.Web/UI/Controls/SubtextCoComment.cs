using System;
using System.Web.UI;
using Subtext.Framework.Configuration;
using Subtext.Web.Controls;

namespace Subtext.Web.UI.Controls
{
	/// <summary>
	/// Summary description for CoCommentScript.
	/// </summary>
	public class SubtextCoComment : CoComment
	{
		/// <summary>
		/// Initializes a new instance of the <see cref="SubtextCoComment"/> class.
		/// </summary>
		public SubtextCoComment()
		{
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
			this.BlogUrl = Config.CurrentBlog.RootUrl;
			
			this.CommentAuthorFieldName = GetControlClientId("tbName");
			this.CommentButtonId = GetControlClientId("btnSubmit");
			if(this.CommentButtonId == null || this.CommentButtonId.Length == 0)
			{
				this.CommentButtonId = GetControlClientId("btnCompliantSubmit");
			}
			this.CommentTextFieldName = GetControlClientId("tbComment");

			this.CommentFormId = "Form1";
		}

		private string GetControlClientId(string controlId)
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
