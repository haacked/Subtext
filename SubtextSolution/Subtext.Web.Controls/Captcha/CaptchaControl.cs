using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Text;
using System.Text;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using Subtext.Framework.Util;

namespace Subtext.Web.Controls.Captcha
{
    [DefaultProperty("Text")]
    public class CaptchaControl : CaptchaBase, INamingContainer, IPostBackDataHandler
    {
		/// <summary>
		/// Initializes a new instance of the <see cref="CaptchaControl"/> class.
		/// </summary>
        public CaptchaControl()
        {
			this.LayoutStyle = CaptchaControl.Layout.CssBased;
			this.ErrorMessage = "Please enter the correct word";
			this.Display = ValidatorDisplay.Dynamic;
		}

        private void GenerateNewCaptcha()
        {
			if (this.Width.IsEmpty)
				this.Width = Unit.Pixel(180);
			if (this.Height.IsEmpty)
				this.Height = Unit.Pixel(50);
			this.captcha.TextLength = this.CaptchaLength;
        }

		/// <summary>
		/// Loads the post data.
		/// </summary>
		/// <param name="PostDataKey">The post data key.</param>
		/// <param name="Values">The values.</param>
		/// <returns></returns>
        public bool LoadPostData(string PostDataKey, NameValueCollection Values)
        {
            return false;
        }

    	protected override bool EvaluateIsValid()
    	{
			bool isValid = base.EvaluateIsValid();

			if (isValid)
			{
				//We don't want the CAPTCHA to change if the 
				//user specifies a correct answer but some other 
				//field is not valid.
				this.captcha.Text = GetClientSpecifiedAnswer();
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
			if (!Page.IsPostBack || !this.IsValid)
			{
				this.GenerateNewCaptcha();
			}
			
			base.OnPreRender(e);
        }

		/// <summary>
		/// When implemented by a class, signals the server control to notify the ASP.NET application 
		/// that the state of the control has changed.
		/// </summary>
        public void RaisePostDataChangedEvent()
        {
			//Do nothing.
        }

    	void RenderHiddenInputForEncryptedAnswer(HtmlTextWriter writer)
    	{
    		writer.Write("<input type=\"hidden\" name=\"" + this.HiddenEncryptedAnswerFieldName + "\" value=\"" + EncryptAnswer(CaptchaText) + "\" />");
    	}
    	
        protected override void Render(HtmlTextWriter writer)
        {
			RenderHiddenInputForEncryptedAnswer(writer);
            writer.Write("<div id=\"" + this.ClientID + "\"");
            if (!String.IsNullOrEmpty(this.CssClass))
            {
                writer.Write(" class=\"" + this.CssClass + "\"");
            }
        	else
            {
				writer.Write(" class=\"captcha\"");
            }
            writer.Write(">");

			string src = ControlHelper.ExpandTildePath("~/CaptchaImage.ashx");
        	
            writer.Write("<img src=\"" + src);
            if (!this.IsDesignMode)
            {
				writer.Write("?spec=" + HttpUtility.UrlEncodeUnicode(captcha.ToEncryptedString()));
            }
            writer.Write("\" border=\"0\"");
			
        	writer.Write(" width=\"" + this.Width.Value + "\" ");
			writer.Write(" height=\"" + this.Height.Value + "\" ");
            if (this.ToolTip.Length > 0)
            {
                writer.Write(" alt='" + this.ToolTip + "'");
            }
            writer.Write(" />");
            
        	if (this.text.Length > 0)
            {
				writer.Write("<label for=\"" + this.AnswerFormFieldName + "\">");
                writer.Write(this.text);
				writer.Write("</label>");
            	base.Render(writer);
            }
			
			writer.Write("<input name=\"" + this.AnswerFormFieldName + "\" type=\"text\" size=\"");
            writer.Write(this.captcha.TextLength.ToString());
            writer.Write("\" maxlength=\"" + this.captcha.TextLength.ToString() + "\"");
            if (this.AccessKey.Length > 0)
            {
                writer.Write(" accesskey=\"" + this.AccessKey + "\"");
            }
            if (!this.Enabled)
            {
                writer.Write(" disabled=\"disabled\"");
            }
            if (this.TabIndex > 0)
            {
                writer.Write(" tabindex=\"" + this.TabIndex.ToString() + "\"");
            }
			if (Page.IsPostBack && this.IsValid)
				writer.Write(" value=\"" + HttpUtility.HtmlEncode(Page.Request.Form[AnswerFormFieldName]) + "\" />");
        	else
				writer.Write(" value=\"\" />");
				
            writer.Write("</div>");
        }
    	
    	[DefaultValue(""), Description("Characters used to render CAPTCHA text. A character will be picked randomly from the string."), Category("Captcha")]
        public string CaptchaChars
        {
            get
            {
                return this.captcha.TextChars;
            }
            set
            {
                this.captcha.TextChars = value;
            }
        }

        [Description("Font used to render CAPTCHA text. If font name is blankd, a random font will be chosen."), DefaultValue(""), Category("Captcha")]
        public string CaptchaFont
        {
            get
            {
				return this.captcha.FontFamily;
            }
            set
            {
                this.captcha.FontFamily = value;
            }
        }

        [Category("Captcha"), Description("Amount of random font warping used on the CAPTCHA text"), DefaultValue(typeof(CaptchaImage.FontWarpFactor), "Low")]
        public CaptchaImage.FontWarpFactor CaptchaFontWarping
        {
            get
            {
                return this.captcha.WarpFactor;
            }
            set
            {
				this.captcha.WarpFactor = value;
            }
        }

        [Category("Captcha"), Description("Number of CaptchaChars used in the CAPTCHA text"), DefaultValue(5)]
        public int CaptchaLength
        {
            get
            {
                return this.captcha.TextLength;
            }
            set
            {
                this.captcha.TextLength = value;
            }
        }

    	/// <summary>
    	/// The text to render.
    	/// </summary>
        private string CaptchaText
        {
            get
            {
				return this.captcha.Text;
            }
        }        

        private bool IsDesignMode
        {
            get
            {
                return (HttpContext.Current == null);
            }
        }

        [Category("Captcha"), DefaultValue(typeof(Layout), "Horizontal"), Description("Determines if image and input area are displayed horizontally, or vertically.")]
        public Layout LayoutStyle
        {
            get
            {
                return this.layoutStyle;
            }
            set
            {
                this.layoutStyle = value;
            }
        }

		private CaptchaInfo captcha = new CaptchaInfo();
		private Layout layoutStyle = Layout.Horizontal;
		private string text = "Enter the code shown above:";

        public enum Layout
        {
            Horizontal,
            Vertical,
        	/// <summary>
        	/// Indicates that the layout will be handled by external css.
        	/// </summary>
        	CssBased
        }
    }
	
	[Serializable]
	internal struct CaptchaInfo
	{
		public CaptchaInfo(string text)
		{
			this.Width = 180;
			this.Height = 50;
			this.randomTextLength = 5;
			this.WarpFactor = CaptchaImage.FontWarpFactor.Low;
			this.FontFamily = string.Empty;
			this.text = text;
			this.validRandomTextChars = defaultValidRandomTextChars;
			this.DateGenerated = DateTime.Now;
			this.FontFamily = RandomFontFamily();
		}

		/// <summary>
		/// Returns a random font family name.
		/// </summary>
		/// <returns></returns>
		private string RandomFontFamily()
		{
			InstalledFontCollection collection1 = new InstalledFontCollection();
			FontFamily[] familyArray1 = collection1.Families;
			string fontFamily = "bogus";
			while (goodFontList.IndexOf(fontFamily) == -1)
			{
				fontFamily = familyArray1[random.Next(0, collection1.Families.Length)].Name.ToLower();
			}
			return fontFamily;
		}

		/// <summary>
		/// Returns a base 64 encrypted serialized representation of this object.
		/// </summary>
		/// <returns></returns>
		public string ToEncryptedString()
		{
			if (this.Width == 0)
				this.Width = 180;

			if (this.Height == 0)
				this.Height = 50;
			
			string serialized = SerializationHelper.SerializeToBase64String(this);
			return CaptchaBase.EncryptString(serialized);
		}
		
		/// <summary>
		/// Reconstructs an instance of this type from an encrypted serialized string.
		/// </summary>
		/// <param name="encrypted"></param>
		public static CaptchaInfo FromEncryptedString(string encrypted)
		{
			string decrypted = CaptchaBase.DecryptString(encrypted);
			return SerializationHelper.DeserializeFromBase64String<CaptchaInfo>(decrypted);
		}

		/// <summary>
		/// A string of valid characters to use in the Captcha text.  
		/// A random character will be selected from this string for 
		/// each character.
		/// </summary>
		public string TextChars
		{
			get
			{
				return this.validRandomTextChars ?? defaultValidRandomTextChars;
			}
			set
			{
				this.validRandomTextChars = value;
			}
		}

		private string GenerateRandomText()
		{
			StringBuilder builder = new StringBuilder();
			int length = this.TextChars.Length;
			for (int i = 0; i < TextLength; i++)
			{
				builder.Append(this.TextChars.Substring(random.Next(length), 1));
			}
			DateGenerated = DateTime.Now;
			return builder.ToString();
		}

		/// <summary>
		/// Gets or sets the text to render.
		/// </summary>
		/// <value>The text.</value>
		public string Text
		{
			get
			{
				if (String.IsNullOrEmpty(this.text))
				{
					this.text = this.GenerateRandomText();
				}
				return this.text;
			}
			set
			{
				this.text = value;
			}
		}

		/// <summary>
		/// Number of characters to use in the CAPTCHA test.
		/// </summary>
		/// <value>The length of the text.</value>
		public int TextLength
		{
			get
			{
				if (this.randomTextLength <= 0)
					this.randomTextLength = 4;
				return this.randomTextLength;
			}
			set
			{
				this.randomTextLength = value;
				this.text = this.GenerateRandomText();
			}
		}

		public DateTime DateGenerated;
		public int Width;
		public int Height;
		public string FontFamily;
		private const string goodFontList = "arial; arial black; comic sans ms; courier new; estrangelo edessa; franklin gothic medium; georgia; lucida console; lucida sans unicode; mangal; microsoft sans serif; palatino linotype; sylfaen; tahoma; times new roman; trebuchet ms; verdana;";
		public CaptchaImage.FontWarpFactor WarpFactor;
		private static Random random = new Random();
		private string text;
		private int randomTextLength;
		private string validRandomTextChars;
		private const string defaultValidRandomTextChars = "ABCDEFGHJKLMNPQRSTUVWXYZ23456789";
	}
}

