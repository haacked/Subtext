#region Disclaimer/Info
///////////////////////////////////////////////////////////////////////////////////////////////////
// .Text WebLog
// 
// .Text is an open source weblog system started by Scott Watermasysk. 
// Blog: http://ScottWater.com/blog 
// RSS: http://scottwater.com/blog/rss.aspx
// Email: Dottext@ScottWater.com
//
// For updated news and information please visit http://scottwater.com/dottext and subscribe to 
// the Rss feed @ http://scottwater.com/dottext/rss.aspx
//
// On its release (on or about August 1, 2003) this application is licensed under the BSD. However, I reserve the 
// right to change or modify this at any time. The most recent and up to date license can always be fount at:
// http://ScottWater.com/License.txt
// 
// Please direct all code related questions to:
// GotDotNet Workspace: http://www.gotdotnet.com/Community/Workspaces/workspace.aspx?id=e99fccb3-1a8c-42b5-90ee-348f6b77c407
// Yahoo Group http://groups.yahoo.com/group/DotText/
// 
///////////////////////////////////////////////////////////////////////////////////////////////////
#endregion

using System;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.WebUI
{
	// TODO: add clear link (display: none;) plus requried js

	[ToolboxData("<{0}:MessagePanel runat=\"server\"></{0}:MessagePanel>")]
	public class MessagePanel : Panel, INamingContainer
	{		
		private const string VSKEY_MESSAGE = "Message";
		private const string VSKEY_ERROR = "Error";

		bool _showMessagePanel;
		bool _showErrorPanel;

		string _messageCssClass;
		string _errorCssClass;
		string _messageIconUrl;
		string _errorIconUrl;

		public MessagePanel()
		{
			ViewState[VSKEY_MESSAGE] = String.Empty;
			ViewState[VSKEY_ERROR] = String.Empty;
		}

		#region Accessors
		public string Message
		{
			get { return (string)ViewState[VSKEY_MESSAGE]; }
			set { ViewState[VSKEY_MESSAGE] = value; }
		}

		public string Error
		{
			get { return (string)ViewState[VSKEY_ERROR]; }
			set { ViewState[VSKEY_ERROR] = value; }
		}

		public bool ShowMessagePanel
		{
			get { return _showMessagePanel; }
			set { _showMessagePanel = value; }
		}

		public bool ShowErrorPanel
		{
			get { return _showErrorPanel; }
			set { _showErrorPanel = value; }
		}

		public string MessageCssClass
		{
			get 
			{
				if(this._messageCssClass == null)
				{
					this._messageCssClass = "MessagePanel";
				}
				return _messageCssClass; 
			}
			set { _messageCssClass = value; }
		}

		public string ErrorCssClass
		{
			get 
			{
				if(this._errorCssClass == null)
				{
					this._errorCssClass = "ErrorPanel";
				}
				return _errorCssClass; 
			}
			set { _errorCssClass = value; }
		}

		public  string MessageIconUrl
		{
			get 
			{
				if(_messageIconUrl == null)
				{
					_messageIconUrl =  "~/admin/resources/ico_info.gif";
				}
				return _messageIconUrl;
			}
			set { _messageIconUrl = value; }
		}

		public  string ErrorIconUrl
		{
			get 
			{
				if(_errorIconUrl == null)
				{
					_errorIconUrl =  "~/admin/resources/ico_critical.gif";
				}
				return _errorIconUrl;
			}
			set { _errorIconUrl = value; }
		}

		#endregion

		protected override void OnInit(EventArgs e)
		{
			base.OnInit(e);
		}

		protected override void Render(HtmlTextWriter writer)
		{
			if (null != Page) 
				Page.VerifyRenderingInServerForm(this);

			if (this.ShowErrorPanel)
			{			
				Panel errorPanel = BuildPanel(this.Error, this.ErrorCssClass, 
					Utilities.AbsolutePath(this.ErrorIconUrl));
				this.Controls.Add(errorPanel);
			}

			if (this.ShowMessagePanel)
			{			
				Panel messagePanel = BuildPanel(this.Message, this.MessageCssClass, 
					Utilities.AbsolutePath(this.MessageIconUrl));
				this.Controls.Add(messagePanel);
			}

			base.Render(writer);
		}

		protected virtual Panel BuildPanel(string messageText, string cssClass, string imageUrl)
		{
			Panel result = new Panel();
							
			if (null != imageUrl && cssClass.Length > 0) result.CssClass = cssClass;
	
			if (null != imageUrl && imageUrl.Length > 0) 
			{
				System.Web.UI.WebControls.Image image = new System.Web.UI.WebControls.Image();
				image.Attributes.Add("src", imageUrl);
				result.Controls.Add(image);
			}

			Panel message = new Panel();
			message.Controls.Add(new LiteralControl(messageText));
			result.Controls.Add(message);

			return result;
		}

		public void ShowMessage(string message)
		{
			ShowMessage(message, true);
		}

		public void ShowMessage(string message, bool clearExistingMessages)
		{
			if (clearExistingMessages) 
				this.Message = message;
			else
				this.Message += " " + message;

			this.ShowMessagePanel = true;
			this.Visible = true;
		}

		public void ShowError(string message)
		{
			ShowError(message, true);
		}

		public void ShowError(string message, bool clearExistingMessages)
		{
			if (clearExistingMessages) 
				this.Error = message;
			else
				this.Error += " " + message;

			this.ShowErrorPanel = true;
			this.Visible = true;
		}

		public void Clear()
		{
			this.Visible = false;
		}
	}
}

