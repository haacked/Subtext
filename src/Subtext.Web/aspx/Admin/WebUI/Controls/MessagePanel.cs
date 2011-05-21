#region Disclaimer/Info

///////////////////////////////////////////////////////////////////////////////////////////////////
// Subtext WebLog
// 
// Subtext is an open source weblog system that is a fork of the .TEXT
// weblog system.
//
// For updated news and information please visit http://subtextproject.com/
// Subtext is hosted at Google Code at http://code.google.com/p/subtext/
// The development mailing list is at subtext@googlegroups.com 
//
// This project is licensed under the BSD license.  See the License.txt file for more information.
///////////////////////////////////////////////////////////////////////////////////////////////////

#endregion

using System.Web.UI;
using System.Web.UI.WebControls;

namespace Subtext.Web.Admin.WebUI
{
    // TODO: add clear link (display: none;) plus requried js

    [ToolboxData("<{0}:MessagePanel runat=\"server\"></{0}:MessagePanel>")]
    public class MessagePanel : Panel, INamingContainer
    {
        private const string VSKEY_ERROR = "Error";
        private const string VSKEY_MESSAGE = "Message";

        string _errorCssClass;
        string _errorIconUrl;
        string _messageCssClass;
        string _messageIconUrl;
        bool _showErrorPanel;
        bool _showMessagePanel;

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
                if (_messageCssClass == null)
                {
                    _messageCssClass = "MessagePanel";
                }
                return _messageCssClass;
            }
            set { _messageCssClass = value; }
        }

        public string ErrorCssClass
        {
            get
            {
                if (_errorCssClass == null)
                {
                    _errorCssClass = "ErrorPanel";
                }
                return _errorCssClass;
            }
            set { _errorCssClass = value; }
        }

        public string MessageIconUrl
        {
            get
            {
                if (_messageIconUrl == null)
                {
                    _messageIconUrl = "~/images/icons/ico_info.gif";
                }
                return _messageIconUrl;
            }
            set { _messageIconUrl = value; }
        }

        public string ErrorIconUrl
        {
            get
            {
                if (_errorIconUrl == null)
                {
                    _errorIconUrl = "~/images/icons/ico_critical.gif";
                }
                return _errorIconUrl;
            }
            set { _errorIconUrl = value; }
        }

        #endregion

        protected override void Render(HtmlTextWriter writer)
        {
            if (null != Page)
            {
                Page.VerifyRenderingInServerForm(this);
            }

            if (ShowErrorPanel)
            {
                Panel errorPanel = BuildPanel(Error, ErrorCssClass,
                                              Utilities.AbsolutePath(ErrorIconUrl));
                Controls.Add(errorPanel);
            }

            if (ShowMessagePanel)
            {
                Panel messagePanel = BuildPanel(Message, MessageCssClass,
                                                Utilities.AbsolutePath(MessageIconUrl));
                Controls.Add(messagePanel);
            }

            base.Render(writer);
        }

        protected virtual Panel BuildPanel(string messageText, string cssClass, string imageUrl)
        {
            var result = new Panel();

            if (null != imageUrl && cssClass.Length > 0)
            {
                result.CssClass = cssClass;
            }

            if (!string.IsNullOrEmpty(imageUrl))
            {
                var image = new Image();
                image.Attributes.Add("src", imageUrl);
                result.Controls.Add(image);
            }

            var message = new Panel();
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
            {
                Message = message;
            }
            else
            {
                Message += " " + message;
            }

            ShowMessagePanel = true;
            Visible = true;
        }

        public void ShowError(string message)
        {
            ShowError(message, true);
        }

        public void ShowError(string message, bool clearExistingMessages)
        {
            if (clearExistingMessages)
            {
                Error = message;
            }
            else
            {
                Error += " " + message;
            }

            ShowErrorPanel = true;
            Visible = true;
        }

        public void Clear()
        {
            Visible = false;
        }

        public void ResetMessages()
        {
            Message = string.Empty;
            Error = string.Empty;
        }
    }
}