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

using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Web;
using Subtext.Web.Properties;

namespace Subtext.Web.Controls.Captcha
{
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    [DefaultProperty("Text")]
    public class CaptchaControl : CaptchaBase, INamingContainer, IPostBackDataHandler
    {
        #region Layout enum

        public enum Layout
        {
            Horizontal,
            Vertical,
            /// <summary>
            /// Indicates that the layout will be handled by external css.
            /// </summary>
            CssBased
        }

        #endregion

        private CaptchaInfo _captcha;
        private Layout _layoutStyle = Layout.Horizontal;
        private const string DefaultText = "Enter the code shown above:";

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaControl"/> class.
        /// </summary>
        public CaptchaControl()
        {
            LayoutStyle = Layout.CssBased;
            ErrorMessage = Resources.Message_PleaseEnterCorrectWord;
            Display = ValidatorDisplay.Dynamic;
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
        [DefaultValue("")]
        [Description("Characters used to render CAPTCHA text. A character will be picked randomly from the string.")]
        [Category("Captcha")]
        public string CaptchaChars
        {
            get { return _captcha.TextChars; }
            set { _captcha.TextChars = value; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
        [Description("Font used to render CAPTCHA text. If font name is blankd, a random font will be chosen.")]
        [DefaultValue("")]
        [Category("Captcha")]
        public string CaptchaFont
        {
            get { return _captcha.FontFamily; }
            set { _captcha.FontFamily = value; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
        [Category("Captcha")]
        [Description("Amount of random font warping used on the CAPTCHA text")]
        [DefaultValue(typeof(CaptchaImage.FontWarpFactor), "Low")]
        public CaptchaImage.FontWarpFactor CaptchaFontWarping
        {
            get { return _captcha.WarpFactor; }
            set { _captcha.WarpFactor = value; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
        [Category("Captcha")]
        [Description("Number of CaptchaChars used in the CAPTCHA text")]
        [DefaultValue(5)]
        public int CaptchaLength
        {
            get { return _captcha.TextLength; }
            set { _captcha.TextLength = value; }
        }

        /// <summary>
        /// The text to render.
        /// </summary>
        private string CaptchaText
        {
            get { return _captcha.Text; }
        }

        private static bool IsDesignMode
        {
            get { return (HttpContext.Current == null); }
        }

        [Category("Captcha")]
        [DefaultValue(typeof(Layout), "Horizontal")]
        [Description("Determines if image and input area are displayed horizontally, or vertically.")]
        public Layout LayoutStyle
        {
            get { return _layoutStyle; }
            set { _layoutStyle = value; }
        }

        public bool LoadPostData(string postDataKey, NameValueCollection values)
        {
            return false;
        }

        /// <summary>
        /// When implemented by a class, signals the server control to notify the ASP.NET application 
        /// that the state of the control has changed.
        /// </summary>
        public void RaisePostDataChangedEvent()
        {
            //Do nothing.
        }

        private void GenerateNewCaptcha()
        {
            if (Width.IsEmpty)
            {
                Width = Unit.Pixel(180);
            }
            if (Height.IsEmpty)
            {
                Height = Unit.Pixel(50);
            }
            _captcha.TextLength = CaptchaLength;
        }

        /// <summary>
        /// When overridden in a derived class, this method contains the code to determine 
        /// whether the value in the input control is valid.
        /// </summary>
        /// <returns>
        /// true if the value in the input control is valid; otherwise, false.
        /// </returns>
        protected override bool EvaluateIsValid()
        {
            bool isValid = base.EvaluateIsValid();

            if (isValid)
            {
                //We don't want the CAPTCHA to change if the 
                //user specifies a correct answer but some other 
                //field is not valid.
                _captcha.Text = GetClientSpecifiedAnswer();
            }
            return isValid;
        }

        /// <summary>
        /// Generates the captcha if it hasn't been generated already.
        /// </summary>
        /// <param name="e">The <see cref="System.EventArgs"/> instance containing the event data.</param>
        protected override void OnPreRender(EventArgs e)
        {
            // We store the answer encrypted so it can't be tampered with.
            if (!Page.IsPostBack || !IsValid)
            {
                GenerateNewCaptcha();
            }

            base.OnPreRender(e);
        }

        void RenderHiddenInputForEncryptedAnswer(TextWriter writer)
        {
            writer.Write("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", HiddenEncryptedAnswerFieldName,
                         EncryptAnswer(CaptchaText));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderHiddenInputForEncryptedAnswer(writer);
            writer.Write("<div id=\"{0}\"", ClientID);
            if (!String.IsNullOrEmpty(CssClass))
            {
                writer.Write(" class=\"{0}\"", CssClass);
            }
            else
            {
                writer.Write(" class=\"captcha\"");
            }
            writer.Write(">");

            string src = HttpHelper.ExpandTildePath("~/images/services/CaptchaImage.ashx");

            writer.Write("<img src=\"{0}", src);
            if (!IsDesignMode)
            {
                writer.Write("?spec={0}", HttpUtility.UrlEncodeUnicode(_captcha.ToEncryptedString()));
            }
            writer.Write("\" border=\"0\"");

            writer.Write(" width=\"{0}\" ", Width.Value);
            writer.Write(" height=\"{0}\" ", Height.Value);
            if (ToolTip.Length > 0)
            {
                writer.Write(" alt='{0}'", ToolTip);
            }
            writer.Write(" />");

            if (DefaultText.Length > 0)
            {
                writer.Write("<label for=\"{0}\">", AnswerFormFieldName);
                writer.Write(DefaultText);
                writer.Write("</label>");
                base.Render(writer);
            }

            writer.Write("<input name=\"{0}\" type=\"text\" size=\"", AnswerFormFieldName);
            writer.Write(_captcha.TextLength.ToString(CultureInfo.InvariantCulture));
            writer.Write("\" maxlength=\"{0}\"", _captcha.TextLength);
            if (AccessKey.Length > 0)
            {
                writer.Write(" accesskey=\"{0}\"", AccessKey);
            }
            if (!Enabled)
            {
                writer.Write(" disabled=\"disabled\"");
            }
            if (TabIndex > 0)
            {
                writer.Write(" tabindex=\"{0}\"", TabIndex);
            }
            if (Page.IsPostBack && IsValid)
            {
                writer.Write(" value=\"{0}\" />", HttpUtility.HtmlEncode(Page.Request.Form[AnswerFormFieldName]));
            }
            else
            {
                writer.Write(" value=\"\" />");
            }

            writer.Write("</div>");
        }
    }


}