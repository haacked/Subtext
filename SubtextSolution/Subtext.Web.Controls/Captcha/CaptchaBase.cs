using System;
using System.ComponentModel;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web.UI.WebControls;

namespace Subtext.Web.Controls
{
	/// <summary>
	/// </summary>
	public abstract class CaptchaBase : BaseValidator
	{
		static SymmetricAlgorithm encryptionAlgorithm = InitializeEncryptionAlgorithm();
		
		/// <summary>
		/// Initializes a new instance of the <see cref="InvisibleCaptcha"/> class.
		/// </summary>
		public CaptchaBase() : base()
		{
		}

		static SymmetricAlgorithm InitializeEncryptionAlgorithm()
		{
			SymmetricAlgorithm rijaendel = RijndaelManaged.Create();
			rijaendel.GenerateKey();
			rijaendel.GenerateIV();
			return rijaendel;
		}

		/// <summary>
		/// Encrypts the string and returns a base64 encoded encrypted string.
		/// </summary>
		/// <param name="clearText">The clear text.</param>
		/// <returns></returns>
		public static string EncryptString(string clearText)
		{
			byte[] clearTextBytes = Encoding.UTF8.GetBytes(clearText);
			byte[] encrypted = encryptionAlgorithm.CreateEncryptor().TransformFinalBlock(clearTextBytes, 0, clearTextBytes.Length);
			return Convert.ToBase64String(encrypted);
		}

		/// <summary>
		/// Decrypts the base64 encrypted string and returns the cleartext.
		/// </summary>
		/// <param name="encryptedEncodedText">The clear text.</param>
		/// <returns></returns>
		public static string DecryptString(string encryptedEncodedText)
		{
			byte[] encryptedBytes = Convert.FromBase64String(encryptedEncodedText);
			byte[] decryptedBytes = encryptionAlgorithm.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);
			return Encoding.UTF8.GetString(decryptedBytes);
		}

		/// <summary>Checks the properties of the control for valid values.</summary>
		/// <returns>true if the control properties are valid; otherwise, false.</returns>
		protected override bool ControlPropertiesValid()
		{
			if (!String.IsNullOrEmpty(ControlToValidate))
			{
				CheckControlValidationProperty(ControlToValidate, "ControlToValidate");
			}
			return true;
		}

		/// <summary>
		/// Encrypts the answer along with the current datetime.
		/// </summary>
		/// <param name="answer">The answer.</param>
		/// <returns></returns>
		protected virtual string EncryptAnswer(string answer)
		{
			return EncryptString(answer + "|" + DateTime.Now.ToString("yyyy/MM/dd HH:mm"));
		}

		/// <summary>
		/// Gets the name of the hidden form field in which the encrypted answer 
		/// is located.  The answer is sent encrypted to the browser, which must 
		/// send the answer back.
		/// </summary>
		/// <value>The name of the hidden encrypted answer field.</value>
		protected string HiddenEncryptedAnswerFieldName
		{
			get
			{
				return ClientID + "_encrypted";
			}
		}

		///<summary>
		///When overridden in a derived class, this method contains the code to determine whether the value in the input control is valid.
		///</summary>
		///<returns>
		///true if the value in the input control is valid; otherwise, false.
		///</returns>
		///
		protected override bool EvaluateIsValid()
		{
			string answer = GetClientSpecifiedAnswer();

			AnswerAndDate answerAndDate = GetEncryptedAnswerFromForm();

			if (answerAndDate.Expired)
			{
				this.ErrorMessage = "Sorry, but this form has expired. Please submit again.";
				return false;
			}
			
			if(!answerAndDate.Valid)
				return false;

			string expectedAnswer = answerAndDate.Answer;
			bool isValid = !String.IsNullOrEmpty(answer) && answer == expectedAnswer;
			return isValid;
		}
		
		// Gets the answer from the client, whether entered by 
		// javascript or by the user.
		protected virtual string GetClientSpecifiedAnswer()
		{
			return Page.Request.Form[this.AnswerFormFieldName];
		}
		
		protected virtual AnswerAndDate GetEncryptedAnswerFromForm()
		{
			string formValue = Page.Request.Form[this.HiddenEncryptedAnswerFieldName];
			AnswerAndDate answerAndDate = new AnswerAndDate(formValue, CaptchaTimeout);
			return answerAndDate;
		}

		/// <summary>
		/// The input field (possibly hidden) in which the client 
		/// will specify the answer.
		/// </summary>
		protected string AnswerFormFieldName
		{
			get
			{
				return ClientID + "_answer";
			}
		}

		[DefaultValue(0), Description("Number of seconds this CAPTCHA is valid after it is generated. Zero means valid forever."), Category("Captcha")]
		public int CaptchaTimeout
		{
			get
			{
				return this.timeoutInSeconds;
			}
			set
			{
				this.timeoutInSeconds = value;
			}
		}
		private int timeoutInSeconds = 0;
	}
	
	/// <summary>
	/// Represents the answer and date returned by the 
	/// client.
	/// </summary>
	public struct AnswerAndDate
	{
		bool isValid;
		
		/// <summary>
		/// Initializes a new instance of the <see cref="AnswerAndDate"/> class.
		/// </summary>
		/// <param name="encryptedAnswer">The encrypted answer.</param>
		public AnswerAndDate(string encryptedAnswer, int timeoutInSeconds)
		{
			this.expired = false;
			this.isValid = false;
			this.answer = string.Empty;
			this.date = DateTime.MinValue;
			
			if(String.IsNullOrEmpty(encryptedAnswer))
				return;

			string decryptedAnswer = CaptchaBase.DecryptString(encryptedAnswer);
			string[] answerParts = decryptedAnswer.Split('|');
			if (answerParts.Length < 2)
				return;

			this.answer = answerParts[0];
			this.date = DateTime.ParseExact(answerParts[1], "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);

			isValid = timeoutInSeconds == 0 || (DateTime.Now - this.date).TotalSeconds < timeoutInSeconds;
			if(!isValid)
				this.expired = true;
		}

		/// <summary>
		/// Gets the answer.
		/// </summary>
		/// <value>The answer.</value>
		public string Answer
		{
			get { return this.answer; }
		}

		string answer;

		/// <summary>
		/// Gets the date the answer was rendered.
		/// </summary>
		/// <value>The date.</value>
		public DateTime Date
		{
			get { return this.date; }
		}

		DateTime date;
			
		/// <summary>
		/// Whether or not the answer and date is valid.
		/// </summary>
		public bool Valid
		{
			get
			{
				return isValid;
			}
		}

		/// <summary>
		/// Gets a value indicating whether this <see cref="AnswerAndDate"/> is expired.
		/// </summary>
		/// <value><c>true</c> if expired; otherwise, <c>false</c>.</value>
		public bool Expired
		{
			get
			{
				return this.expired;
			}
		}

		bool expired;
	}
}

