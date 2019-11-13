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
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using log4net;
using Subtext.Framework.Logging;
using Subtext.Web.Properties;

namespace Subtext.Web.Controls.Captcha
{
    /// <summary>
    /// </summary>
    [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
    public abstract class CaptchaBase : BaseValidator
    {
        static readonly SymmetricAlgorithm EncryptionAlgorithm = InitializeEncryptionAlgorithm();
        private readonly static ILog Log = new Log();

        /// <summary>
        /// Gets the name of the hidden form field in which the encrypted answer 
        /// is located.  The answer is sent encrypted to the browser, which must 
        /// send the answer back.
        /// </summary>
        /// <value>The name of the hidden encrypted answer field.</value>
        protected string HiddenEncryptedAnswerFieldName
        {
            get { return ClientID + "_encrypted"; }
        }

        /// <summary>
        /// The input field (possibly hidden) in which the client 
        /// will specify the answer.
        /// </summary>
        protected string AnswerFormFieldName
        {
            get { return ClientID + "_answer"; }
        }

        [SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Captcha")]
        [DefaultValue(0)]
        [Description("Number of seconds this CAPTCHA is valid after it is generated. Zero means valid forever.")]
        [Category("Captcha")]
        public int CaptchaTimeout { get; set; }

        static SymmetricAlgorithm InitializeEncryptionAlgorithm()
        {
            SymmetricAlgorithm rijaendel = Rijndael.Create();
            //TODO: We should set these values in the db the very first time this code is called and load them from the db every other time.
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
            byte[] encrypted = EncryptionAlgorithm.CreateEncryptor().TransformFinalBlock(clearTextBytes, 0,
                                                                                         clearTextBytes.Length);
            return Convert.ToBase64String(encrypted);
        }

        /// <summary>
        /// Decrypts the base64 encrypted string and returns the cleartext.
        /// </summary>
        /// <param name="encryptedEncodedText">The clear text.</param>
        /// <exception type="System.Security.Cryptography.CryptographicException">Thrown the string to be decrypted 
        /// was encrypted using a different encryptor (for example, if we recompile and 
        /// receive an old string).</exception>
        /// <returns></returns>
        public static string DecryptString(string encryptedEncodedText)
        {
            try
            {
                byte[] encryptedBytes = Convert.FromBase64String(encryptedEncodedText);
                byte[] decryptedBytes = EncryptionAlgorithm.CreateDecryptor().TransformFinalBlock(encryptedBytes, 0,
                                                                                                  encryptedBytes.Length);
                return Encoding.UTF8.GetString(decryptedBytes);
            }
            catch (FormatException fe)
            {
                throw new CaptchaExpiredException(
                    String.Format(CultureInfo.InvariantCulture, Resources.CaptchaExpired_EncryptedTextNotValid,
                                  encryptedEncodedText), fe);
            }
            catch (CryptographicException e)
            {
                throw new CaptchaExpiredException(Resources.CaptchaExpired_KeyOutOfSynch, e);
            }
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
            return EncryptString(answer + "|" + DateTime.UtcNow.ToString("yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture));
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
            try
            {
                return ValidateCaptcha();
            }
            catch (CaptchaExpiredException e)
            {
                if (e.InnerException != null)
                {
                    string warning = Resources.Warning_CaptchaExpired;
                    if (HttpContext.Current != null && HttpContext.Current.Request != null)
                    {
                        warning += " User Agent: " + HttpContext.Current.Request.UserAgent;
                    }
                    Log.Warn(warning, e.InnerException);
                }
                ErrorMessage = Resources.Message_FormExpired;
                return false;
            }
        }

        private bool ValidateCaptcha()
        {
            string answer = GetClientSpecifiedAnswer();
            AnswerAndDate answerAndDate = GetEncryptedAnswerFromForm();

            string expectedAnswer = answerAndDate.Answer;
            bool isValid = !String.IsNullOrEmpty(answer)
                           && String.Equals(answer, expectedAnswer, StringComparison.OrdinalIgnoreCase);
            return isValid;
        }

        // Gets the answer from the client, whether entered by 
        // javascript or by the user.
        protected virtual string GetClientSpecifiedAnswer()
        {
            return Page.Request.Form[AnswerFormFieldName];
        }

        /// <summary>
        /// Gets the encrypted answer from form.
        /// </summary>
        /// <returns></returns>
        /// <exception type="CaptchaExpiredException">Thrown when the user takes too long to submit a captcha answer.</exception>
        protected virtual AnswerAndDate GetEncryptedAnswerFromForm()
        {
            string formValue = Page.Request.Form[HiddenEncryptedAnswerFieldName];
            AnswerAndDate answerAndDate = AnswerAndDate.ParseAnswerAndDate(formValue, CaptchaTimeout);
            if (answerAndDate.Expired)
            {
                throw new CaptchaExpiredException(Resources.CaptchaExpired_WaitedTooLong);
            }
            return answerAndDate;
        }
    }

    /// <summary>
    /// Represents the answer and date returned by the 
    /// client.
    /// </summary>
    public struct AnswerAndDate
    {
        public string Answer
        {
            get { return _answer; }
        }
        string _answer;

        public DateTime Date
        {
            get { return _date; }
        }
        DateTime _date;

        public bool Expired
        {
            get { return _expired; }
        }
        bool _expired;

        public static AnswerAndDate ParseAnswerAndDate(string encryptedAnswer, int timeoutInSeconds)
        {
            AnswerAndDate answerAndDate;
            answerAndDate._expired = false;
            answerAndDate._answer = string.Empty;
            answerAndDate._date = DateTime.MinValue;

            if (String.IsNullOrEmpty(encryptedAnswer))
            {
                return answerAndDate;
            }

            string decryptedAnswer = CaptchaBase.DecryptString(encryptedAnswer);
            string[] answerParts = decryptedAnswer.Split('|');
            if (answerParts.Length < 2)
            {
                return answerAndDate;
            }

            answerAndDate._answer = answerParts[0];
            answerAndDate._date = DateTime.ParseExact(answerParts[1], "yyyy/MM/dd HH:mm", CultureInfo.InvariantCulture);

            if (timeoutInSeconds != 0 && (DateTime.UtcNow - answerAndDate._date).TotalSeconds >= timeoutInSeconds)
            {
                throw new CaptchaExpiredException(Resources.CaptchaExpired_WaitedTooLong);
            }

            return answerAndDate;
        }
    }
}