using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Drawing;
using System.Drawing.Text;
using System.Globalization;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Web;
using Subtext.Web.Controls.Properties;

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

        private CaptchaInfo captcha = new CaptchaInfo();
        private Layout layoutStyle = Layout.Horizontal;
        private string text = "Enter the code shown above:";

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
            get { return captcha.TextChars; }
            set { captcha.TextChars = value; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
        [Description("Font used to render CAPTCHA text. If font name is blankd, a random font will be chosen.")]
        [DefaultValue("")]
        [Category("Captcha")]
        public string CaptchaFont
        {
            get { return captcha.FontFamily; }
            set { captcha.FontFamily = value; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
        [Category("Captcha")]
        [Description("Amount of random font warping used on the CAPTCHA text")]
        [DefaultValue(typeof(CaptchaImage.FontWarpFactor), "Low")]
        public CaptchaImage.FontWarpFactor CaptchaFontWarping
        {
            get { return captcha.WarpFactor; }
            set { captcha.WarpFactor = value; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
        [Category("Captcha")]
        [Description("Number of CaptchaChars used in the CAPTCHA text")]
        [DefaultValue(5)]
        public int CaptchaLength
        {
            get { return captcha.TextLength; }
            set { captcha.TextLength = value; }
        }

        /// <summary>
        /// The text to render.
        /// </summary>
        private string CaptchaText
        {
            get { return captcha.Text; }
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
            get { return layoutStyle; }
            set { layoutStyle = value; }
        }

        #region IPostBackDataHandler Members

        /// <summary>
        /// Loads the post data.
        /// </summary>
        /// <param name="PostDataKey">The post data key.</param>
        /// <param name="Values">The values.</param>
        /// <returns></returns>
        public bool LoadPostData(string postDataKey, NameValueCollection Values)
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

        #endregion

        private void GenerateNewCaptcha()
        {
            if(Width.IsEmpty)
            {
                Width = Unit.Pixel(180);
            }
            if(Height.IsEmpty)
            {
                Height = Unit.Pixel(50);
            }
            captcha.TextLength = CaptchaLength;
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

            if(isValid)
            {
                //We don't want the CAPTCHA to change if the 
                //user specifies a correct answer but some other 
                //field is not valid.
                captcha.Text = GetClientSpecifiedAnswer();
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
            if(!Page.IsPostBack || !IsValid)
            {
                GenerateNewCaptcha();
            }

            base.OnPreRender(e);
        }

        void RenderHiddenInputForEncryptedAnswer(HtmlTextWriter writer)
        {
            writer.Write("<input type=\"hidden\" name=\"{0}\" value=\"{1}\" />", HiddenEncryptedAnswerFieldName,
                         EncryptAnswer(CaptchaText));
        }

        protected override void Render(HtmlTextWriter writer)
        {
            RenderHiddenInputForEncryptedAnswer(writer);
            writer.Write("<div id=\"{0}\"", ClientID);
            if(!String.IsNullOrEmpty(CssClass))
            {
                writer.Write(" class=\"{0}\"", CssClass);
            }
            else
            {
                writer.Write(" class=\"captcha\"");
            }
            writer.Write(">");

            string src = HttpHelper.ExpandTildePath("~/images/CaptchaImage.ashx");

            writer.Write("<img src=\"{0}", src);
            if(!IsDesignMode)
            {
                writer.Write("?spec={0}", HttpUtility.UrlEncodeUnicode(captcha.ToEncryptedString()));
            }
            writer.Write("\" border=\"0\"");

            writer.Write(" width=\"{0}\" ", Width.Value);
            writer.Write(" height=\"{0}\" ", Height.Value);
            if(ToolTip.Length > 0)
            {
                writer.Write(" alt='{0}'", ToolTip);
            }
            writer.Write(" />");

            if(text.Length > 0)
            {
                writer.Write("<label for=\"{0}\">", AnswerFormFieldName);
                writer.Write(text);
                writer.Write("</label>");
                base.Render(writer);
            }

            writer.Write("<input name=\"{0}\" type=\"text\" size=\"", AnswerFormFieldName);
            writer.Write(captcha.TextLength.ToString(CultureInfo.InvariantCulture));
            writer.Write("\" maxlength=\"{0}\"", captcha.TextLength);
            if(AccessKey.Length > 0)
            {
                writer.Write(" accesskey=\"{0}\"", AccessKey);
            }
            if(!Enabled)
            {
                writer.Write(" disabled=\"disabled\"");
            }
            if(TabIndex > 0)
            {
                writer.Write(" tabindex=\"{0}\"", TabIndex);
            }
            if(Page.IsPostBack && IsValid)
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

    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    [Serializable]
    public struct CaptchaInfo
    {
        private const string defaultValidRandomTextChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";

        private const string goodFontList =
            "arial; arial black; comic sans ms; courier new; estrangelo edessa; franklin gothic medium; georgia; lucida console; lucida sans unicode; mangal; microsoft sans serif; palatino linotype; sylfaen; tahoma; times new roman; trebuchet ms; verdana;";

        private static Random random = new Random();
        public DateTime DateGenerated;
        public string FontFamily;
        public int Height;
        private int randomTextLength;
        private string text;
        private string validRandomTextChars;
        public CaptchaImage.FontWarpFactor WarpFactor;
        public int Width;

        /// <summary>
        /// Initializes a new instance of the <see cref="CaptchaInfo"/> class.
        /// </summary>
        /// <param name="text">The text.</param>
        public CaptchaInfo(string text)
        {
            Width = 180;
            Height = 50;
            randomTextLength = 5;
            WarpFactor = CaptchaImage.FontWarpFactor.Low;
            FontFamily = string.Empty;
            this.text = text;
            validRandomTextChars = defaultValidRandomTextChars;
            DateGenerated = DateTime.Now;
            FontFamily = RandomFontFamily();
        }

        /// <summary>
        /// A string of valid characters to use in the Captcha text.  
        /// A random character will be selected from this string for 
        /// each character.
        /// </summary>
        public string TextChars
        {
            get { return validRandomTextChars ?? defaultValidRandomTextChars; }
            set { validRandomTextChars = value; }
        }

        /// <summary>
        /// Gets or sets the text to render.
        /// </summary>
        /// <value>The text.</value>
        public string Text
        {
            get
            {
                if(String.IsNullOrEmpty(text))
                {
                    text = GenerateRandomText();
                }
                return text;
            }
            set { text = value; }
        }

        /// <summary>
        /// Number of characters to use in the CAPTCHA test.
        /// </summary>
        /// <value>The length of the text.</value>
        public int TextLength
        {
            get
            {
                if(randomTextLength <= 0)
                {
                    randomTextLength = 4;
                }
                return randomTextLength;
            }
            set
            {
                randomTextLength = value;
                text = GenerateRandomText();
            }
        }

        /// <summary>
        /// Returns a random font family name.
        /// </summary>
        /// <returns></returns>
        private static string RandomFontFamily()
        {
            var collection1 = new InstalledFontCollection();
            FontFamily[] familyArray1 = collection1.Families;
            string fontFamily = "bogus";
            while(goodFontList.IndexOf(fontFamily, StringComparison.OrdinalIgnoreCase) == -1)
            {
                fontFamily = familyArray1[random.Next(0, collection1.Families.Length)].Name.ToLowerInvariant();
            }
            return fontFamily;
        }

        /// <summary>
        /// Returns a base 64 encrypted serialized representation of this object.
        /// </summary>
        /// <returns></returns>
        public string ToEncryptedString()
        {
            if(Width == 0)
            {
                Width = 180;
            }

            if(Height == 0)
            {
                Height = 50;
            }

            return CaptchaBase.EncryptString(ToString());
        }

        /// <summary>
        /// Reconstructs an instance of this type from an encrypted serialized string.
        /// </summary>
        /// <param name="encrypted"></param>
        public static CaptchaInfo FromEncryptedString(string encrypted)
        {
            string decrypted = CaptchaBase.DecryptString(encrypted);
            string[] values = decrypted.Split('|');

            var info = new CaptchaInfo();
            info.Width = int.Parse(values[0], CultureInfo.InvariantCulture);
            info.Height = int.Parse(values[1], CultureInfo.InvariantCulture);
            info.WarpFactor = (CaptchaImage.FontWarpFactor)Enum.Parse(typeof(CaptchaImage.FontWarpFactor), values[2]);
            info.FontFamily = values[3];
            info.Text = values[4];
            info.DateGenerated = DateTime.ParseExact(values[5], "yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture);
            return info;
        }

        private string GenerateRandomText()
        {
            var builder = new StringBuilder();
            int length = TextChars.Length;
            for(int i = 0; i < TextLength; i++)
            {
                builder.Append(TextChars.Substring(random.Next(length), 1));
            }
            DateGenerated = DateTime.Now;
            return builder.ToString();
        }

        /// <summary>
        /// Returns the fully qualified type name of this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="T:System.String"></see> containing a fully qualified type name.
        /// </returns>
        public override string ToString()
        {
            return String.Format(CultureInfo.InvariantCulture, "{0}|{1}|{2}|{3}|{4}|{5}"
                                 , Width
                                 , Height
                                 , WarpFactor
                                 , FontFamily
                                 , Text
                                 , DateGenerated.ToString("yyyy/MM/dd HH:mm:ss", CultureInfo.InvariantCulture));
        }
    }
}